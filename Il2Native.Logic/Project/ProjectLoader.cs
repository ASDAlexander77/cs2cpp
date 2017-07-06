namespace Il2Native.Logic.Project
{
    using Il2Native.Logic.Project.Tasks;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml.Linq;

    public class ProjectLoader
    {
        private const string msbuildRegistryPath = @"SOFTWARE\Microsoft\MSBuild";

        private static XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

        private string folder;
        private string initialTarget;

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
                BuildWellKnownValues();
                BuildWellKnownValues("Project", projectFilePath);
                return this.LoadProjectInternal(projectFilePath);
            }
            finally
            {
                Directory.SetCurrentDirectory(currentDirectory);
            }
        }

        private static string GetReferenceValue(XElement element)
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
            var projectSubPath = !string.IsNullOrWhiteSpace(projectPath) ? Path.GetDirectoryName(projectPath) : string.Empty;
            var projectFileName = Path.GetFileName(projectPath);

            var projectExistPath = string.Empty;
            try
            {
                projectExistPath = !string.IsNullOrWhiteSpace(projectSubPath) && Directory.GetFiles(projectSubPath, projectFileName).Any()
                                ? Directory.GetFiles(projectSubPath, projectFileName).First()
                                : Directory.GetFiles(Path.Combine(this.folder, projectSubPath), projectFileName).Any()
                                    ? Directory.GetFiles(Path.Combine(this.folder, projectSubPath), projectFileName).First()
                                    : null;
            }
            catch (Exception)
            {
                // TODO: finish evaluating path such as c:\XXX*\**\*.bbb
                return true;
            }

            if (projectExistPath == null)
            {
                throw new FileNotFoundException(projectPath);
            }

            var project = XDocument.Load(projectExistPath);

            BuildWellKnownValues("ThisFile", projectExistPath);

            Directory.SetCurrentDirectory(this.folder);

            this.initialTarget = project.Root.Attribute("InitialTargets")?.Value ?? string.Empty;

            foreach (var element in project.Root.Elements())
            {
                if (!ProcessElement(element))
                {
                    return false;
                }
            }

            foreach (var reference in this.LoadReferencesFromProject(projectExistPath, project))
            {
                this.References.Add(reference);
            }

            return true;
        }

        private void BuildWellKnownValues()
        {
            var disk = "C:";
            var version = "14.0";
            var path = disk + @"\Program Files (x86)\MSBuild";
            var path64 = disk + @"\Program Files\MSBuild";

            this.Options["MSBuildExtensionsPath"] = path;
            this.Options["MSBuildExtensionsPath32"] = path;
            this.Options["MSBuildExtensionsPath64"] = path64;
            this.Options["MSBuildToolsPath"] = string.Format(@"{0}\{1}\bin", path, version);
            this.Options["MSBuildToolsVersion"] = version;

            try
            {
                var key = Registry.LocalMachine.OpenSubKey(msbuildRegistryPath);
                Version registryVersion = new Version("0.0");
                foreach (var keyVersionStr in key.GetSubKeyNames())
                {
                    Version versionKey;
                    if (Version.TryParse(keyVersionStr, out versionKey) && versionKey > registryVersion)
                    {
                        registryVersion = versionKey;
                        version = registryVersion.ToString();

                        // read paths
                        var toolsSetKey = Registry.LocalMachine.OpenSubKey(msbuildRegistryPath + @"\ToolsVersions\" + version);
                        var toolsPath = toolsSetKey.GetValue("MSBuildToolsPath")?.ToString();
                        if (toolsPath != default(string))
                        {
                            path = toolsPath.Split(new string[] { @"\" + version + @"\bin" }, StringSplitOptions.None).First();
                            this.Options["MSBuildExtensionsPath"] = path;
                            this.Options["MSBuildExtensionsPath32"] = path;
                            this.Options["MSBuildExtensionsPath64"] = path.Replace(@" (x86)\", @"\");

                            this.Options["MSBuildToolsPath"] = toolsPath;
                            this.Options["MSBuildToolsVersion"] = version;
                        }

                        var frameworkToolsPath = toolsSetKey.GetValue("MSBuildFrameworkToolsPath")?.ToString();
                        if (frameworkToolsPath != null)
                        {
                            this.Options["MSBuildFrameworkToolsPath"] = frameworkToolsPath;
                            this.Options["MSBuildFrameworkToolsPath32"] = frameworkToolsPath;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void BuildWellKnownValues(string name, string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            this.folder = fileInfo.Directory.FullName;
            this.Options[string.Format("MSBuild{0}", name)] = Path.GetFileName(fileInfo.FullName);
            this.Options[string.Format("MSBuild{0}Name", name)] = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            this.Options[string.Format("MSBuild{0}FullPath", name)] = Helpers.NormalizePath(fileInfo.FullName);
            this.Options[string.Format("MSBuild{0}Extension", name)] = fileInfo.Extension;
            this.Options[string.Format("MSBuild{0}Directory", name)] = Helpers.EnsureTrailingSlash(folder);

            string directory = Path.GetDirectoryName(fileInfo.FullName);
            int rootLength = Path.GetPathRoot(directory).Length;
            string directoryNoRoot = directory.Substring(rootLength);
            directoryNoRoot = Helpers.EnsureTrailingSlash(directoryNoRoot);
            this.Options[string.Format("MSBuild{0}DirectoryNoRoot", name)] = Helpers.EnsureNoLeadingSlash(directoryNoRoot);
        }

        private bool ProcessElement(XElement element)
        {
            if (!ProjectCondition(element))
            {
                return true;
            }

            return ProcessElementNoCondition(element);
        }

        private bool ProcessElementNoCondition(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case "Import":
                    if (!LoadImport(element))
                    {
                        return false;
                    }

                    break;
                case "Target":
                    if (!ProcessTarget(element))
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
                case "Choose":
                    if (!ProcessChoose(element))
                    {
                        return false;
                    }

                    break;

                case "GenerateResourcesCode":
                    // custom task
                    var generateResourcesCode = new GenerateResourcesCode();
                    generateResourcesCode.ResxFilePath = this.FillProperties(element.Attribute("ResxFilePath").Value);
                    generateResourcesCode.OutputSourceFilePath = this.FillProperties(element.Attribute("OutputSourceFilePath").Value);
                    generateResourcesCode.AssemblyName = this.FillProperties(element.Attribute("AssemblyName").Value);
                    generateResourcesCode.OmitResourceAccess = true;
                    generateResourcesCode.DebugOnly = this.Options["Configuration"] != "Release";
                    generateResourcesCode.Execute();
                    break;
            }

            return true;
        }

        private bool LoadImport(XElement element)
        {
            var cloned = new ProjectProperties(this.Options.Where(k => k.Key.StartsWith("MSBuild")).ToDictionary(k => k.Key, v => v.Value));
            var folder = this.folder;
            var initialTarget = this.initialTarget;
            var value = element.Attribute("Project").Value;
            var result = this.LoadProjectInternal(this.FillProperties(value));
            foreach (var copyCloned in cloned)
            {
                this.Options[copyCloned.Key] = copyCloned.Value;
            }

            this.folder = folder;
            Directory.SetCurrentDirectory(this.folder);

            this.initialTarget = initialTarget;

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
            var fullFileName = PathCombine(this.FillProperties(element.Attribute("Include").Value));
            Debug.Assert(fullFileName.EndsWith(".cs"));
            this.Sources.Add(fullFileName);
        }

        private void LoadContent(XElement element)
        {
            this.Content.Add(PathCombine(this.FillProperties(element.Attribute("Include").Value)));
        }

        private void ProcessError(XElement element)
        {
            this.Errors.Add(this.FillProperties(element.Attribute("Text").Value));
        }

        private bool ProcessTarget(XElement element)
        {
            var name = element.Attribute("Name").Value;
            if (name == this.initialTarget || (this.Options["CompileDependsOn"]?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Any(i => i.Trim() == name) ?? false))
            {
                foreach (var targetElement in element.Elements())
                {
                    if (!ProcessElement(targetElement))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool ProcessChoose(XElement element)
        {
            var result = true;
            var any = false;
            foreach (var item in element.Elements(ns + "When").Where(i => ProjectCondition(i)))
            {
                any = true;
                foreach (var subItem in item.Elements())
                {
                    result |= ProcessElement(subItem);
                }
            }

            if (!any)
            {
                foreach (var item in element.Elements(ns + "Otherwise"))
                {
                    foreach (var subItem in item.Elements())
                    {
                        result |= ProcessElement(subItem);
                    }
                }
            }

            return result;
        }

        private string[] LoadReferencesFromProject(string firstSource, XDocument project)
        {
            var xElement = project.Root;
            if (xElement != null)
            {
                return xElement.Elements(ns + "ItemGroup").Elements(ns + "Reference")
                    .Select(e => GetReferenceValue(e))
                    .Union(this.GetReferencesFromProject(firstSource, xElement)).ToArray();
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

        private bool ExecuteConditionBool(string condition)
        {
            var result = ExecuteCondition(condition);
            if (result is bool)
            {
                return (bool)result;
            }

            return result != null && result.ToString().ToLowerInvariant() == "true";
        }

        private object ExecuteCondition(string condition)
        {
            var start = 0;

            if (condition.StartsWith("!"))
            {
                var right = condition.Substring(1, condition.Length - 1).Trim();
                return !ExecuteConditionBool(right);
            }

            if (condition.StartsWith("("))
            {
                var right = FindNextBracket(condition.Substring(1), '(', ')');
                if (right != -1)
                {
                    if (right == condition.Length - 2)
                    {
                        var inner = condition.Substring(1, condition.Length - 2).Trim();
                        return ExecuteCondition(inner);
                    }

                    start = right;
                }
            }

            var andOperator = condition.IndexOf(" and ", start, StringComparison.OrdinalIgnoreCase);
            if (andOperator != -1)
            {
                var left = condition.Substring(0, andOperator).Trim();
                var right = condition.Substring(andOperator + " and ".Length).Trim();
                return ExecuteConditionBool(left) && ExecuteConditionBool(right);
            }

            var orOperator = condition.IndexOf(" or ", start, StringComparison.OrdinalIgnoreCase);
            if (orOperator != -1)
            {
                var left = condition.Substring(0, orOperator).Trim();
                var right = condition.Substring(orOperator + " or ".Length).Trim();
                return ExecuteConditionBool(left) || ExecuteConditionBool(right);
            }

            var equalOperator = condition.IndexOf("==", start, StringComparison.OrdinalIgnoreCase);
            if (equalOperator != -1)
            {
                var left = condition.Substring(0, equalOperator).Trim();
                var right = condition.Substring(equalOperator + "==".Length).Trim();
                return left.Equals(right);
            }

            var notEqualOperator = condition.IndexOf("!=", start, StringComparison.OrdinalIgnoreCase);
            if (notEqualOperator != -1)
            {
                var left = condition.Substring(0, notEqualOperator).Trim();
                var right = condition.Substring(notEqualOperator + "!=".Length).Trim();
                return !left.Equals(right);
            }

            return ExecuteFunction(condition);
        }

        private string FillProperties(string conditionValueParam)
        {
            var processed = new StringBuilder();
            string conditionValue = conditionValueParam;

            var lastIndex = 0;
            var poisition = 0;
            while (poisition < conditionValue.Length)
            {
                poisition = conditionValue.IndexOf('$', lastIndex);
                if (poisition == -1)
                {
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

                conditionValue = conditionValue.Substring(left + 1);

                var right = FindNextBracket(conditionValue, '(', ')');
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

            processed.Append(conditionValue.Substring(lastIndex));

            return processed.ToString();
        }

        private static int FindNextBracket(string value, char leftChar, char rightChar)
        {
            var right = -1;
            var nextPosition = -1;
            var nested = 0;
            while (++nextPosition < value.Length)
            {
                if (value[nextPosition] == leftChar)
                {
                    nested++;
                }

                if (value[nextPosition] == rightChar)
                {
                    if (nested > 0)
                    {
                        nested--;
                        continue;
                    }

                    right = nextPosition;
                    break;
                }
            }

            return right;
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

                if (parameters != null)
                {
                    parameters = parameters.Select(p => FillProperties(p)).ToArray();
                }

                bool isProperty = parameters == null;

                var msbuild = "MSBuild";
                if (typeName == msbuild)
                {
                    switch (functionName)
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
                    var path = StripQuotes(parameters[0]);
                    result = (Directory.Exists(path) || File.Exists(path));
                }
                else if (functionName == "HasTrailingSlash")
                {
                    var path = StripQuotes(parameters[0]);
                    var lastCharacter = path[path.Length - 1];
                    // Either back or forward slashes satisfy the function: this is useful for URL's
                    return (lastCharacter == Path.DirectorySeparatorChar || lastCharacter == Path.AltDirectorySeparatorChar || lastCharacter == '\\');
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
                    else if (isProperty)
                    {
                        // this is variable
                        result = this.Options[functionName] ?? string.Empty;
                    }
                    else
                    {
                        Debug.Fail("Method '" + functionName + "' could not be found");
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
                            var foundMethod = targetType.GetMethods().FirstOrDefault(m => m.Name == functionName && m.GetParameters().Count() == parameters.Count() && m.GetParameters().Zip(parameters, (a, b) => Tuple.Create(a, b)).All(IsAssignable));
                            Debug.Assert(foundMethod != null, "Method could not be found");
                            if (foundMethod != null)
                            {
                                result = foundMethod.Invoke(result, foundMethod.GetParameters().Zip(parameters, (p, a) => PrepareArgument(p, a)).ToArray());
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool IsAssignable(Tuple<ParameterInfo, string> arg)
        {
            if (arg.Item1.ParameterType.IsAssignableFrom(arg.Item2.GetType()))
            {
                return true;
            }

            switch (arg.Item1.ParameterType.FullName)
            {
                case "System.Char":
                    return true;
                case "System.Char[]":
                    return true;
                case "System.SByte":
                    sbyte resultSByte;
                    return sbyte.TryParse(arg.Item2, out resultSByte);
                case "System.Int16":
                    short resultInt16;
                    return short.TryParse(arg.Item2, out resultInt16);
                case "System.Int32":
                    int resultInt32;
                    return int.TryParse(arg.Item2, out resultInt32);
                case "System.Int64":
                    long resultInt64;
                    return long.TryParse(arg.Item2, out resultInt64);
                case "System.Byte":
                    byte resultByte;
                    return byte.TryParse(arg.Item2, out resultByte);
                case "System.UInt16":
                    ushort resultUInt16;
                    return ushort.TryParse(arg.Item2, out resultUInt16);
                case "System.UInt32":
                    uint resultUInt32;
                    return uint.TryParse(arg.Item2, out resultUInt32);
                case "System.UInt64":
                    ulong resultUInt64;
                    return ulong.TryParse(arg.Item2, out resultUInt64);
            }

            return false;
        }

        private object PrepareArgument(ParameterInfo parameter, string argument)
        {
            if (parameter.ParameterType.IsAssignableFrom(argument.GetType()))
            {
                return StripQuotes(argument);
            }

            switch (parameter.ParameterType.FullName)
            {
                case "System.Char":
                    return StripQuotes(argument).ToCharArray().First();
                case "System.Char[]":
                    return StripQuotes(argument).ToCharArray();
                case "System.SByte":
                    return sbyte.Parse(argument);
                case "System.Int16":
                    return short.Parse(argument);
                case "System.Int32":
                    return int.Parse(argument);
                case "System.Int64":
                    return long.Parse(argument);
                case "System.Byte":
                    return byte.Parse(argument);
                case "System.UInt16":
                    return ushort.Parse(argument);
                case "System.UInt32":
                    return uint.Parse(argument);
                case "System.UInt64":
                    return ulong.Parse(argument);
            }

            return argument;
        }

        private string StripQuotes(string parameter)
        {
            var trimmed = parameter.Trim();
            var strippedOrDefault = trimmed.StartsWith("'") && trimmed.EndsWith("'") ? trimmed.Substring(1, trimmed.Length - 2) : parameter;
            return strippedOrDefault;
        }

        private bool ParseFunction(string propertyNameOfFunctionCall, out string typeName, out string functionOrPropertyName, out string[] parameters, out string propertyNameOfFunctionCallLeft)
        {
            typeName = null;
            functionOrPropertyName = null;
            parameters = null;
            propertyNameOfFunctionCallLeft = propertyNameOfFunctionCall;

            var isPropertyName = false;
            var startFunctionName = 0;
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

                    while (++nestedPosition < propertyNameOfFunctionCall.Length && propertyNameOfFunctionCall[nestedPosition] == ':')
                    {
                    }

                    startFunctionName = nestedPosition;
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

            functionOrPropertyName = propertyNameOfFunctionCall.Substring(startFunctionName, poisition - startFunctionName);
            if (!isPropertyName)
            {
                var poisitionEnd = poisition;
                var nested = 0;
                while (++poisitionEnd < propertyNameOfFunctionCall.Length)
                {
                    if (propertyNameOfFunctionCall[poisitionEnd] == '(')
                    {
                        nested++;
                    }

                    if (propertyNameOfFunctionCall[poisitionEnd] == ')')
                    {
                        if (nested > 0)
                        {
                            nested--;
                            continue;
                        }

                        break;
                    }
                }

                var paramsSubString = propertyNameOfFunctionCall.Substring(poisition + 1, poisitionEnd - poisition - 1);
                parameters = paramsSubString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
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
        private IEnumerable<string> GetReferencesFromProject(string prjectFullFilePath, XElement xElement)
        {
            foreach (var projectReference in xElement.Elements(ns + "ItemGroup").Elements(ns + "ProjectReference"))
            {
                var projectFile = this.GetRealFolderFromProject(prjectFullFilePath, projectReference);
                var project = XDocument.Load(projectFile);
                foreach (var reference in this.LoadReferencesFromProject(projectFile, project))
                {
                    yield return reference;
                }

                yield return this.GetReferenceFromProjectValue(projectReference, prjectFullFilePath);
            }
        }
    }
}
