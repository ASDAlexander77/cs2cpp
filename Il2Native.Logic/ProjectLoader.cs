using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Il2Native.Logic
{
    public class ProjectLoader
    {
        private string folder;

        public ProjectLoader(IDictionary<string, string> options)
        {
            this.Sources = new List<string>();
            this.Content = new List<string>();
            this.References = new List<string>();
            this.Options = options;
        }

        public IList<string> Sources { get; private set; }

        public IList<string> Content { get; private set; }

        public IList<string> References { get; private set; }

        public IDictionary<string, string> Options { get; private set; }

        public void Load(string projectFilePath)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(projectFilePath));
                this.LoadProjectInternal(projectFilePath);
            }
            finally
            {
                Directory.SetCurrentDirectory(currentDirectory);
            }
        }
        private static string GetReferenceValue(XNamespace ns, XElement element)
        {
            var xElement = element.Element(ns + "HintPath");
            if (xElement != null)
            {
                return xElement.Value;
            }

            return element.Attribute("Include").Value;
        }

        private void LoadProjectInternal(string projectPath)
        {
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var project = XDocument.Load(projectPath);
            this.folder = new FileInfo(projectPath).Directory.FullName;

            this.Options["MSBuildThisFileDirectory"] = folder + @"\";

            foreach (var element in project.Root.Elements())
            {
                if (!ProjectCondition(element))
                {
                    continue;
                }

                switch (element.Name.LocalName)
                {
                    case "Import":
                        LoadImport(element);
                        break;
                    case "PropertyGroup":
                        LoadPropertyGroup(element);
                        break;
                    case "ItemGroup":
                        LoadItemGroup(element);
                        break;
                }
            }

            foreach (var reference in this.LoadReferencesFromProject(projectPath, project, ns))
            {
                this.References.Add(reference);
            }
        }

        private void LoadImport(XElement element)
        {
            this.LoadProjectInternal(this.FillProperties(element.Attribute("Project").Value));
        }

        private void LoadPropertyGroup(XElement element)
        {
            foreach (var property in element.Elements().Where(i => ProjectCondition(i)))
            {
                this.Options[property.Name.LocalName] = this.FillProperties(property.Value);
            }
        }

        private void LoadItemGroup(XElement element)
        {
            foreach (var item in element.Elements().Where(i => ProjectCondition(i)))
            {
                switch (item.Name.LocalName)
                {
                    case "Compile":
                        LoadCompile(item);
                        break;
                    case "Content":
                        LoadContent(item);
                        break;
                }
            }
        }
        private void LoadCompile(XElement element)
        {
            this.Sources.Add(PathCombine(this.FillProperties(element.Attribute("Include").Value)));
        }

        private void LoadContent(XElement element)
        {
            this.Content.Add(PathCombine(this.FillProperties(element.Attribute("Include").Value)));
        }

        private string[] LoadReferencesFromProject(string firstSource, XDocument project, XNamespace ns)
        {
            var xElement = project.Root;
            if (xElement != null)
            {
                return xElement.Elements(ns + "ItemGroup").Elements(ns + "Reference")
                    .Select(e => GetReferenceValue(ns, e))
                    .Union(this.GetReferencesFromProject(firstSource, ns, xElement)).ToArray();
            }

            return null;
        }

        private string PathCombine(string filePath)
        {
            if (File.Exists(filePath))
            {
                return new FileInfo(filePath).FullName;
            }

            var filePathExt = Path.Combine(this.folder, filePath);
            if (File.Exists(filePathExt))
            {
                return new FileInfo(filePathExt).FullName;
            }

            return filePath;
        }

        private bool ProjectCondition(XElement element)
        {
            var conditionAttribute = element.Attribute("Condition");
            if (conditionAttribute == null)
            {
                return true;
            }

            return ExecuteCondition(this.FillProperties(conditionAttribute.Value));
        }

        private bool ExecuteCondition(string condition)
        {
            var andOperator = condition.IndexOf("and", StringComparison.Ordinal);
            if (andOperator != -1)
            {
                var left = condition.Substring(0, andOperator).Trim();
                var right = condition.Substring(andOperator + "and".Length).Trim();
                return ExecuteCondition(left) && ExecuteCondition(right);
            }

            var orOperator = condition.IndexOf("or", StringComparison.Ordinal);
            if (orOperator != -1)
            {
                var left = condition.Substring(0, orOperator).Trim();
                var right = condition.Substring(orOperator + "or".Length).Trim();
                return ExecuteCondition(left) || ExecuteCondition(right);
            }

            var equalOperator = condition.IndexOf("==", StringComparison.Ordinal);
            if (equalOperator != -1)
            {
                var left = condition.Substring(0, equalOperator).Trim();
                var right = condition.Substring(equalOperator + "==".Length).Trim();
                return left.Equals(right);
            }

            var notEqualOperator = condition.IndexOf("!=", StringComparison.Ordinal);
            if (notEqualOperator != -1)
            {
                var left = condition.Substring(0, notEqualOperator).Trim();
                var right = condition.Substring(notEqualOperator + "!=".Length).Trim();
                return !left.Equals(right);
            }

            return !string.IsNullOrWhiteSpace(condition) && condition.Trim() == "true";
        }

        private string FillProperties(string conditionValue)
        {
            var processed = new StringBuilder();

            var lastIndex = 0;
            var poisition = 0;
            while (poisition < conditionValue.Length)
            {
                poisition = conditionValue.IndexOf('$', lastIndex);
                if (poisition == -1)
                {
                    processed.Append(conditionValue.Substring(lastIndex));
                    break;
                }
                else
                {
                    processed.Append(conditionValue.Substring(lastIndex, poisition - lastIndex));
                }

                var left = conditionValue.IndexOf('(', poisition);
                var right = conditionValue.IndexOf(')', poisition);
                if (left == -1 || right == -1)
                {
                    ////throw new IndexOutOfRangeException("Condition is not correct");
                    break;
                }

                var propertyName = conditionValue.Substring(left + 1, right - left - 1);
                processed.Append(this.Options[propertyName]);

                lastIndex = right + 1;
            }

            return processed.ToString();
        }

        public string GetRealFolderForProject(string fullProjectFilePath, string referenceFolder)
        {
            return Path.Combine(Path.GetDirectoryName(fullProjectFilePath), Path.GetDirectoryName(referenceFolder));
        }

        private string GetRealFolderFromProject(string projectFullFilePath, XElement projectReference)
        {
            var nestedProject = projectReference.Attribute("Include").Value;
            var projectFolder = this.GetRealFolderForProject(projectFullFilePath, nestedProject);
            var projectFile = Path.Combine(projectFolder, Path.GetFileName(nestedProject));
            return projectFile;
        }

        private string GetReferenceFromProjectValue(XElement element, string projectFullFilePath)
        {
            var referencedProjectFilePath = element.Attribute("Include").Value;

            var filePath = Path.Combine(this.GetRealFolderForProject(projectFullFilePath, referencedProjectFilePath),
                string.Concat("bin\\", this.Options["Configuration"]),
                string.Concat(Path.GetFileNameWithoutExtension(referencedProjectFilePath), ".dll"));

            return filePath;
        }
        private IEnumerable<string> GetReferencesFromProject(string prjectFullFilePath, XNamespace ns, XElement xElement)
        {
            foreach (var projectReference in xElement.Elements(ns + "ItemGroup").Elements(ns + "ProjectReference"))
            {
                var projectFile = this.GetRealFolderFromProject(prjectFullFilePath, projectReference);
                var project = XDocument.Load(projectFile);
                foreach (var reference in this.LoadReferencesFromProject(projectFile, project, ns))
                {
                    yield return reference;
                }

                yield return this.GetReferenceFromProjectValue(projectReference, prjectFullFilePath);
            }
        }
    }
}
