﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Lang.Cs.Compiler.VSProject
{

    /*
    smartClass
    option NoAdditionalFile
    
    property WorkingDirectory DirectoryInfo 
    
    property AssemblyName string nazwa DLL lub Exe
    
    property OutputType string 
    
    property ProjectGuid Guid 
    
    property ReferencedAssemblies List<Assembly> 
    	init #
    
    property Reference List<string> 
    	init #
    
    property ProjectReferences List<ProjectReference> 
    	init #
    
    property PropertyGroups List<PropertyGroup> 
    	init #
    
    property Items List<ProjectItem> 
    	init #
    smartClassEnd
    */
    
    public partial class Project1
    {
/*
        public Assembly[] LoadReferencedAssemblies(List<CompileResult> compiled)
        {
            List<Assembly> result = new List<Assembly>();
            foreach (var i in reference)
            {
                // todo:use Application domain instead of Assembly.Load
                var aa = Assembly.LoadWithPartialName(i);
                result.Add(aa);
            }
            foreach (var projectReference in ProjectReferences)
            {
                var compiledProject = compiled.Where(i => i.ProjectGuid == projectReference.ProjectGuid).First();
                result.Add(compiledProject.CompiledAssembly);
            }


            return result.ToArray();
        }
*/
        #region Static Methods

        // Public Methods 

        public static Project1 Load(string filename)
        {
            var project = new Project1
            {
                _workingDirectory = new FileInfo(filename).Directory
            };
            var doc = XDocument.Load(filename);
            var docRoot = doc.Root;
            if (docRoot == null) return project;

            var ns = docRoot.Name.Namespace;

            foreach (var i in docRoot.Elements(ns + "PropertyGroup"))
                project.ParsePropertyGroup(i);
            var itemGroups = docRoot.Elements(ns + "ItemGroup").ToArray();
            foreach (var ig in itemGroups)
            {
                /*
                 *  <Reference Include="System" />
    <Reference Include="System.Core" />
    <Reference Include="System.Xml.Linq" />
    <Reference Include="System.Data.DataSetExtensions" />
    <Reference Include="Microsoft.CSharp" />
    <Reference Include="System.Data" />
    <Reference Include="System.Xml" />
                 */
                foreach (var i in ig.Elements())
                {
                    if (i.Name == ns + "Reference")
                    {
                        project._reference.Add((string)i.Attribute("Include"));
                    }
                    else if (i.Name == ns + "Compile")
                    {
                        var name = (string)i.Attribute("Include");
                        var pi = new ProjectItem(name, BuildActions.Compile);
                        project._items.Add(pi);

                    }
                    else if (i.Name == ns + "ProjectReference")
                    {
                        var g = ProjectReference.Deserialize(i);
                        project._projectReferences.Add(g);
                    }
                    else if (i.Name == ns + "Content")
                    {
                        var name = (string)i.Attribute("Include");
                        var pi = new ProjectItem(name, BuildActions.Content);
                        project._items.Add(pi);
                    }
                }
            }
            return project;
        }

        #endregion Static Methods

        #region Methods

        // Public Methods 

/*
        public CompileResult Compile(string outDir, string[] compiledReferences)
        {
            using (Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider())
            {

                ICodeCompiler loCompiler = provider.CreateCompiler();
                CompilerParameters loParameters = new CompilerParameters();
                {
                    var imports = Reference.Select(Q => Q.Trim() + ".dll").Distinct().ToArray();
                    loParameters.ReferencedAssemblies.AddRange(imports);
                    if (compiledReferences != null && compiledReferences.Any())
                        loParameters.ReferencedAssemblies.AddRange(compiledReferences);
                }

                // loParameters.GenerateInMemory = true;
                loParameters.OutputAssembly = Path.Combine(outDir, assemblyName + ".dll");
                new FileInfo(loParameters.OutputAssembly).Directory.Create();
                // CompilerResults loCompiled = loCompiler.CompileAssemblyFromSource(loParameters, csCode);
                string[] codeBatch;
                {

                    List<string> codeBatchList = new List<string>();
                    foreach (var i in items.Where(i => i.BuildAction == BuildActions.Compile).Select(u => u.Name).Distinct())
                    {
                        var fn = Path.Combine(workingDirectory.FullName, i);
                        codeBatchList.Add(File.ReadAllText(fn));
                    }
                    // codeBatchList.Add(csCode);
                    codeBatch = codeBatchList.ToArray();
                }

                {
                    CompilerResults loCompiled = loCompiler.CompileAssemblyFromSourceBatch(loParameters, codeBatch);

                    if (loCompiled.Errors.HasErrors)
                    {
                        string lcErrorMsg = "";
                        lcErrorMsg = loCompiled.Errors.Count.ToString() + " Errors:";
                        for (int x = 0; x < loCompiled.Errors.Count; x++)
                            lcErrorMsg = lcErrorMsg + "\r\nLine: " + loCompiled.Errors[x].Line.ToString() + " - " +
                                         loCompiled.Errors[x].ErrorText;
                        // MessageBox.Show(lcErrorMsg + "\r\n\r\n" + lcCode, "Compiler Demo");
                        // throw new Exception(lcErrorMsg + "\r\n\r\n" + csCode);
                        var g = loCompiled.Errors.OfType<CompilerError>().Where(i => !i.IsWarning).ToArray();
                        throw new Exception("Compile error: " + g.First().ErrorText);
                    }
                    var a = new CompileResult();
                    a.CompiledAssembly = loCompiled.CompiledAssembly;
                    a.OutputAssemblyFilename = loParameters.OutputAssembly;
                    a.ProjectGuid = ProjectGuid;
                    return a;
                }
            }
        }
*/
        // Private Methods 

        void ParsePropertyGroup(XElement xElement)
        {
            var a = PropertyGroup.Deserialize(xElement);
            if (!string.IsNullOrEmpty(a.OutputType))
            {
                _outputType = a.OutputType;
                _assemblyName = a.AssemblyName;
                if (a.ProjectGuid.HasValue)
                    _projectGuid = a.ProjectGuid.Value;
            }
            else
            {
                PropertyGroups.Add(a);
            }
        }

        #endregion Methods
    }
}


// -----:::::##### smartClass embedded code begin #####:::::----- generated 2014-09-02 19:37
// File generated automatically ver 2013-07-10 08:43
// Smartclass.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0c4d5d36fb5eb4ac
namespace Lang.Cs.Compiler.VSProject
{
    public partial class Project1 
    {
        /*
        /// <summary>
        /// Tworzy instancję obiektu
        /// </summary>
        public Project1()
        {
        }

        Przykłady użycia

        implement INotifyPropertyChanged
        implement INotifyPropertyChanged_Passive
        implement ToString ##WorkingDirectory## ##AssemblyName## ##OutputType## ##ProjectGuid## ##ReferencedAssemblies## ##Reference## ##ProjectReferences## ##PropertyGroups## ##Items##
        implement ToString WorkingDirectory=##WorkingDirectory##, AssemblyName=##AssemblyName##, OutputType=##OutputType##, ProjectGuid=##ProjectGuid##, ReferencedAssemblies=##ReferencedAssemblies##, Reference=##Reference##, ProjectReferences=##ProjectReferences##, PropertyGroups=##PropertyGroups##, Items=##Items##
        implement equals WorkingDirectory, AssemblyName, OutputType, ProjectGuid, ReferencedAssemblies, Reference, ProjectReferences, PropertyGroups, Items
        implement equals *
        implement equals *, ~exclude1, ~exclude2
        */
        #region Constants
        /// <summary>
        /// Nazwa własności WorkingDirectory; 
        /// </summary>
        public const string PropertyNameWorkingDirectory = "WorkingDirectory";
        /// <summary>
        /// Nazwa własności AssemblyName; nazwa DLL lub Exe
        /// </summary>
        public const string PropertyNameAssemblyName = "AssemblyName";
        /// <summary>
        /// Nazwa własności OutputType; 
        /// </summary>
        public const string PropertyNameOutputType = "OutputType";
        /// <summary>
        /// Nazwa własności ProjectGuid; 
        /// </summary>
        public const string PropertyNameProjectGuid = "ProjectGuid";
        /// <summary>
        /// Nazwa własności ReferencedAssemblies; 
        /// </summary>
        public const string PropertyNameReferencedAssemblies = "ReferencedAssemblies";
        /// <summary>
        /// Nazwa własności Reference; 
        /// </summary>
        public const string PropertyNameReference = "Reference";
        /// <summary>
        /// Nazwa własności ProjectReferences; 
        /// </summary>
        public const string PropertyNameProjectReferences = "ProjectReferences";
        /// <summary>
        /// Nazwa własności PropertyGroups; 
        /// </summary>
        public const string PropertyNamePropertyGroups = "PropertyGroups";
        /// <summary>
        /// Nazwa własności Items; 
        /// </summary>
        public const string PropertyNameItems = "Items";
        #endregion Constants

        #region Methods
        #endregion Methods

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public DirectoryInfo WorkingDirectory
        {
            get
            {
                return _workingDirectory;
            }
            set
            {
                _workingDirectory = value;
            }
        }
        private DirectoryInfo _workingDirectory;
        /// <summary>
        /// nazwa DLL lub Exe
        /// </summary>
        public string AssemblyName
        {
            get
            {
                return _assemblyName;
            }
            set
            {
                value = (value ?? String.Empty).Trim();
                _assemblyName = value;
            }
        }
        private string _assemblyName = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string OutputType
        {
            get
            {
                return _outputType;
            }
            set
            {
                value = (value ?? String.Empty).Trim();
                _outputType = value;
            }
        }
        private string _outputType = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public Guid ProjectGuid
        {
            get
            {
                return _projectGuid;
            }
            set
            {
                _projectGuid = value;
            }
        }
        private Guid _projectGuid;
        /// <summary>
        /// 
        /// </summary>
        public List<Assembly> ReferencedAssemblies
        {
            get
            {
                return _referencedAssemblies;
            }
            set
            {
                _referencedAssemblies = value;
            }
        }
        private List<Assembly> _referencedAssemblies = new List<Assembly>();
        /// <summary>
        /// 
        /// </summary>
        public List<string> Reference
        {
            get
            {
                return _reference;
            }
            set
            {
                _reference = value;
            }
        }
        private List<string> _reference = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        public List<ProjectReference> ProjectReferences
        {
            get
            {
                return _projectReferences;
            }
            set
            {
                _projectReferences = value;
            }
        }
        private List<ProjectReference> _projectReferences = new List<ProjectReference>();
        /// <summary>
        /// 
        /// </summary>
        public List<PropertyGroup> PropertyGroups
        {
            get
            {
                return _propertyGroups;
            }
            set
            {
                _propertyGroups = value;
            }
        }
        private List<PropertyGroup> _propertyGroups = new List<PropertyGroup>();
        /// <summary>
        /// 
        /// </summary>
        public List<ProjectItem> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }
        private List<ProjectItem> _items = new List<ProjectItem>();
        #endregion Properties

    }
}
