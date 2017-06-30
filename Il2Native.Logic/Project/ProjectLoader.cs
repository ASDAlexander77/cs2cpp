namespace Il2Native.Logic.Project
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    public class ProjectLoader
    {
        private string folder;

        public ProjectLoader(IDictionary<string, string> options)
        {
            this.Sources = new List<string>();
            this.Content = new List<string>();
            this.References = new List<string>();
            this.Errors = new List<string>();
            this.Options = options;
        }

        public IList<string> Sources { get; private set; }

        public IList<string> Content { get; private set; }

        public IList<string> References { get; private set; }

        public IList<string> Errors { get; private set; }

        public IDictionary<string, string> Options { get; private set; }

        public bool Load(string projectFilePath)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(projectFilePath));
                return this.LoadProjectInternal(projectFilePath);
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

        private bool LoadProjectInternal(string projectPath)
        {
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

            var project = File.Exists(projectPath) ? XDocument.Load(projectPath) : File.Exists(Path.Combine(this.folder, projectPath)) ? XDocument.Load(Path.Combine(this.folder, projectPath)) : null;
            if (project == null)
            {
                throw new FileNotFoundException(projectPath);
            }

            var fileInfo = new FileInfo(projectPath);
            this.folder = fileInfo.Directory.FullName;
            this.Options["MSBuildThisFileDirectory"] = folder + @"\";
            this.Options["MSBuildThisFile"] = Path.GetFileName(fileInfo.FullName);
            this.Options["MSBuildThisFileFullPath"] = fileInfo.FullName;

            var initialTarget = project.Root.Attribute("InitialTargets")?.Value ?? string.Empty;

            foreach (var element in project.Root.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "Target":
                        if (element.Attribute("Name").Value == initialTarget)
                        {
                            foreach (var targetElement in element.Elements())
                            {
                                if (!ProcessElement(targetElement))
                                {
                                    return false;
                                }
                            }
                        }

                        break;
                    default:
                        if (!ProcessElement(element))
                        {
                            return false;
                        }

                        break;
                }
            }

            foreach (var reference in this.LoadReferencesFromProject(projectPath, project, ns))
            {
                this.References.Add(reference);
            }

            return true;
        }

        private bool ProcessElement(XElement element)
        {
            if (!ProjectCondition(element))
            {
                return true;
            }

            switch (element.Name.LocalName)
            {
                case "Import":
                    if (!LoadImport(element))
                    {
                        return false;
                    }

                    break;
                case "PropertyGroup":
                    LoadPropertyGroup(element);
                    break;
                case "ItemGroup":
                    LoadItemGroup(element);
                    break;
                case "Error":
                    ProcessError(element);
                    return false;
            }

            return true;
        }

        private bool LoadImport(XElement element)
        {
            var cloned = new ProjectProperties(this.Options.Where(k => k.Key.StartsWith("MSBuildThisFile")).ToDictionary(k => k.Key, v => v.Value));
            var value = element.Attribute("Project").Value;
            var result = this.LoadProjectInternal(this.FillProperties(value));
            foreach (var copyCloned in cloned)
            {
                this.Options[copyCloned.Key] = copyCloned.Value;
            }

            return result;
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

        private void ProcessError(XElement element)
        {
            this.Errors.Add(this.FillProperties(element.Attribute("Text").Value));
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

            return ExecuteConditionBool(this.FillProperties(conditionAttribute.Value));
        }

        private bool ExecuteConditionBool (string condition)
        {
            return ExecuteCondition(condition)?.ToLowerInvariant() == "true";
        }

        private string ExecuteCondition(string condition)
        {
            if (condition.StartsWith("!"))
            {
                var right = condition.Substring(1, condition.Length - 1).Trim();
                switch (ExecuteCondition(right))
                {
                    case "true": return "false";
                    case "false": return "true";
                    default: return string.Empty;
                }
            }

            var andOperator = condition.IndexOf( " and ", StringComparison.Ordinal);
            if (andOperator != -1)
            {
                var left = condition.Substring(0, andOperator).Trim();
                var right = condition.Substring(andOperator + " and ".Length).Trim();
                return ExecuteConditionBool(left) && ExecuteConditionBool(right) ? "true" : "false";
            }

            var orOperator = condition.IndexOf(" or ", StringComparison.Ordinal);
            if (orOperator != -1)
            {
                var left = condition.Substring(0, orOperator).Trim();
                var right = condition.Substring(orOperator + " or ".Length).Trim();
                return ExecuteConditionBool(left) || ExecuteConditionBool(right) ? "true" : "false";
            }

            var equalOperator = condition.IndexOf("==", StringComparison.Ordinal);
            if (equalOperator != -1)
            {
                var left = condition.Substring(0, equalOperator).Trim();
                var right = condition.Substring(equalOperator + "==".Length).Trim();
                return left.Equals(right) ? "true" : "false";
            }

            var notEqualOperator = condition.IndexOf("!=", StringComparison.Ordinal);
            if (notEqualOperator != -1)
            {
                var left = condition.Substring(0, notEqualOperator).Trim();
                var right = condition.Substring(notEqualOperator + "!=".Length).Trim();
                return !left.Equals(right) ? "true" : "false";
            }

            if (condition.EndsWith(")") || condition.IndexOf('.') != -1)
            {
                // function call
                var functionResult = ExecuteFunction(condition);
                condition = functionResult?.ToString();
            }

            return condition;
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
                if (left == -1)
                {
                    ////throw new IndexOutOfRangeException("Condition is not correct");
                    break;
                }

                conditionValue = FillProperties(conditionValue.Substring(left + 1));

                var right = -1;
                var propertyNamePosition = -1;
                var nested = 0;
                while (++propertyNamePosition < conditionValue.Length)
                {
                    if (conditionValue[propertyNamePosition] == '(')
                    {
                        nested++;
                    }

                    if (conditionValue[propertyNamePosition] == ')')
                    {
                        if (nested > 0)
                        {
                            nested--;
                            continue;
                        }

                        right = propertyNamePosition;
                        break;
                    }
                }

                if (right == -1)
                {
                    ////throw new IndexOutOfRangeException("Condition is not correct");
                    break;
                }

                var propertyNameOfFunctionCall = conditionValue.Substring(0, right).Trim();
                var functionResult = ExecuteFunction(propertyNameOfFunctionCall);
                processed.Append(functionResult);

                lastIndex = right + 1;
            }

            return processed.ToString();
        }

        private object ExecuteFunction(string propertyNameOfFunctionCallParam)
        {
            var propertyNameOfFunctionCall = propertyNameOfFunctionCallParam;
            object result = null;
            while (!string.IsNullOrWhiteSpace(propertyNameOfFunctionCall))
            {
                string typeName;
                string functionName;
                string[] parameters;
                var parsed = ParseFunction(propertyNameOfFunctionCall, out typeName, out functionName, out parameters, out propertyNameOfFunctionCall);
                if (!parsed)
                {
                    return this.Options[propertyNameOfFunctionCall];
                }

                bool isProperty = parameters == null;

                var msbuild = "MSBuild";
                var systemDateTime = "[System.DateTime]::";
                if (typeName == msbuild)
                {
                    var clearName = functionName.Substring(msbuild.Length);
                    switch (clearName)
                    {
                        case "MakeRelative":
                            result = IntrinsicFunctions.MakeRelative(parameters[0], parameters[1]);
                            break;
                        case "GetDirectoryNameOfFileAbove":
                            result = IntrinsicFunctions.GetDirectoryNameOfFileAbove(parameters[0], parameters[1]);
                            break;
                    }
                }
                else if (functionName == "Exists")
                {
                    var trimmed = parameters[0].Trim();
                    var path = trimmed.StartsWith("'") && trimmed.EndsWith("'") ? trimmed.Substring(1, trimmed.Length - 2) : trimmed;
                    result = (Directory.Exists(path) || File.Exists(path));
                }
                else
                {
                    Type targetType = null; 
                    if (!string.IsNullOrWhiteSpace(typeName))
                    {
                        targetType = Type.GetType(typeName);
                    }
                    else if (result != null)
                    {
                        targetType = result.GetType();
                    }
                    else
                    {
                        // this is variable
                        result = this.Options[functionName];
                    }

                    if (targetType != null)
                    {
                        if (parameters == null)
                        {
                            var foundProperty = targetType.GetProperty(functionName);
                            if (foundProperty != null)
                            {
                                result = foundProperty.GetValue(result);
                            }
                        }
                        else
                        {
                            var foundMethod = targetType.GetMethods().FirstOrDefault(m => m.Name == functionName && m.GetParameters().Count() == parameters.Count());
                            if (foundMethod != null)
                            {
                                result = foundMethod.Invoke(result, parameters);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool ParseFunction(string propertyNameOfFunctionCall, out string typeName, out string functionOrPropertyName, out string[] parameters, out string propertyNameOfFunctionCallLeft)
        {
            typeName = null;
            functionOrPropertyName = null;
            parameters = null;
            propertyNameOfFunctionCallLeft = propertyNameOfFunctionCall;

            var isPropertyName = false;
            var poisition = -1;
            var nestedPosition = -1;
            while (++nestedPosition < propertyNameOfFunctionCall.Length)
            {
                if (propertyNameOfFunctionCall[nestedPosition] == '[')
                {
                    var typeNameBuilder = new StringBuilder();
                    while (++nestedPosition < propertyNameOfFunctionCall.Length && propertyNameOfFunctionCall[nestedPosition] != ']')
                    {
                        typeNameBuilder.Append(propertyNameOfFunctionCall[nestedPosition]);
                    }

                    typeName = typeNameBuilder.ToString();
                }

                if (propertyNameOfFunctionCall[nestedPosition] == '.')
                {
                    poisition = nestedPosition;
                    isPropertyName = true;
                    break;
                }

                if (propertyNameOfFunctionCall[nestedPosition] == '(')
                {
                    poisition = nestedPosition;
                    break;
                }
            }

            if (nestedPosition == propertyNameOfFunctionCall.Length)
            {
                return false;
            }

            functionOrPropertyName = propertyNameOfFunctionCall.Substring(0, poisition);
            if (!isPropertyName)
            {
                var poisitionEnd = propertyNameOfFunctionCall.IndexOf(')', poisition);
                var paramsSubString = propertyNameOfFunctionCall.Substring(poisition + 1, poisitionEnd - poisition - 1);
                parameters = paramsSubString.Split(',').Select(s => s.Trim()).ToArray();
                poisition = poisitionEnd;
            }

            propertyNameOfFunctionCallLeft = propertyNameOfFunctionCall.Substring(poisition + 1, propertyNameOfFunctionCall.Length - poisition - 1);

            return true;
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
