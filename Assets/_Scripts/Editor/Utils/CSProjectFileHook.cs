#if UNITY_EDITOR_WIN
using SyntaxTree.VisualStudio.Unity.Bridge;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace feerik.editor.utils
{

    /// <summary>
    /// Provide a hook into Unity's Project File Generation so thatwe re add the correct static analyzer each time.
    /// 
    /// Thanks to : https://answers.unity.com/questions/867993/unityvs-keep-target-framework-in-rewritten-and-rel.html
    /// </summary>
    /// <see cref="https://answers.unity.com/questions/867993/unityvs-keep-target-framework-in-rewritten-and-rel.html"/>
    [InitializeOnLoad]
    public class CSProjectFileHook
    {
        // necessary for XLinq to save the xml project file in utf8
        class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }

        private const string SCHEMA = "http://schemas.microsoft.com/developer/msbuild/2003";
        private const string ASSEMBLY_NAME = "AssemblyName";
        private const string ASSEMBLY_CSHARP = "Assembly-CSharp";
        private const string ASSEMBLY_CSHARP_EDITOR = "Assembly-CSharp-Editor";
        private const string PROPERTY_GROUP = "PropertyGroup";
        private const string CODE_ANALYSIS_RULE_SET = "CodeAnalysisRuleSet";
        private const string RULESET_NAME = "Assembly-CSharp.ruleset";
        private const string DEBUG_CONDITION = " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ";
        private const string CONDITION_ATTRIBUTE = "Condition";

        static CSProjectFileHook()
        {
            ProjectFilesGenerator.ProjectFileGeneration += UpdateXMLProjectFile;
        }

        private static string UpdateXMLProjectFile(string name, string content)
        {
            //Debug.Log(string.Format("{0} starting on \"{1}\"", typeof(CSProjectFileHook).Name, name));
            XDocument document = XDocument.Parse(content);
            XElement assemblyNameElement = document.Descendants(XName.Get(ASSEMBLY_NAME, SCHEMA)).FirstOrDefault();

            bool isUserProjectAssembly = assemblyNameElement != null
                && (assemblyNameElement.Value.Contains(ASSEMBLY_CSHARP) || assemblyNameElement.Value.Contains(ASSEMBLY_CSHARP_EDITOR));
            if (isUserProjectAssembly)
            {
                XElement staticAnalyzerElement = document.Descendants(XName.Get(CODE_ANALYSIS_RULE_SET, SCHEMA)).FirstOrDefault();
                bool isCodeAnalysisRuleSetDefined = staticAnalyzerElement != null;

                if (isCodeAnalysisRuleSetDefined)
                {
                    staticAnalyzerElement.Value = RULESET_NAME;
                }
                else
                {
                    CreateCodeAnalysisRuleSetXMLNode(document);
                }
            }

            var str = new Utf8StringWriter();
            document.Save(str);

            return str.ToString();
        }

        private static void CreateCodeAnalysisRuleSetXMLNode(XDocument document)
        {
            XNamespace namespaceSchema = SCHEMA;
            XElement debugPropertyGroup = document.Descendants(XName.Get(PROPERTY_GROUP, SCHEMA))
                .Where(xElement => xElement.Attribute(CONDITION_ATTRIBUTE) != null
                                     && xElement.Attribute(CONDITION_ATTRIBUTE).Value.Equals(DEBUG_CONDITION))
                .FirstOrDefault();
            XElement codeAnalyzer = new XElement(namespaceSchema + CODE_ANALYSIS_RULE_SET);
            codeAnalyzer.Value = RULESET_NAME;
            debugPropertyGroup.Add(codeAnalyzer);
        }
    }
}
#endif