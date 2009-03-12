﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Xsd2Code.Library.Helpers;

namespace Xsd2Code.Library
{
    /// <summary>
    /// Represents all generation parameters
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Modified 2009-02-20 by Ruslan Urban
    ///     Added Platform and GenerateCloneMethod properties
    /// 
    /// </remarks>
    public class GeneratorParams
    {
        #region Private

        /// <summary>
        /// Type of collection
        /// </summary>
        private CollectionType collectionObjectTypeField = CollectionType.List;

        /// <summary>
        /// List of custom usings
        /// </summary>
        private List<NamespaceParam> customUsingsField = new List<NamespaceParam>();

        /// <summary>
        /// Name of deserialize method
        /// </summary>
        private string deserializeMethodNameField = "Deserialize";

        /// <summary>
        /// Indicate if allow debug into generated code.
        /// </summary>
        private bool disableDebugField = true;

        /// <summary>
        /// Indicate if hide private field in ide.
        /// </summary>
        private bool hidePrivateFieldInIdeField = true;

        /// <summary>
        /// Name of load from file method.
        /// </summary>
        private string loadFromFileMethodNameField = "LoadFromFile";

        /// <summary>
        /// Name of save to file method.
        /// </summary>
        private string saveToFileMethodNameField = "SaveToFile";

        /// <summary>
        /// Name of serialize method.
        /// </summary>
        private string serializeMethodNameField = "Serialize";

        #endregion

        private CodeBase platform = default(CodeBase);
        private bool enableDataBinding = true;

        /// <summary>
        /// Gets or sets the name space.
        /// </summary>
        /// <value>The name space.</value>
        [Category("Code")]
        [Description("namespace of generated file")]
        public string NameSpace { get; set; }

        /// <summary>
        /// Gets or sets generation language
        /// </summary>
        [Category("Code")]
        [Description("Language")]
        public GenerationLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets the output file path.
        /// </summary>
        /// <value>The output file path.</value>
        [Browsable(false)]
        public string OutputFilePath { get; set; }

        /// <summary>
        /// Gets or sets the input file path.
        /// </summary>
        /// <value>The input file path.</value>
        [Browsable(false)]
        public string InputFilePath { get; set; }

        /// <summary>
        /// Gets or sets collection type to use for code generation
        /// </summary>
        [Category("Collection")]
        [Description("Set type of collection for unbounded elements")]
        public CollectionType CollectionObjectType
        {
            get { return this.collectionObjectTypeField; }
            set { this.collectionObjectTypeField = value; }
        }

        /// <summary>
        /// Gets or sets collection base
        /// </summary>
        [Category("Collection")]
        [Description("Set the collection base if using CustomCollection")]
        public string CollectionBase { get; set; }

        /// <summary>
        /// Gets or sets custom usings
        /// </summary>
        [Category("Collection")]
        [Description("list of custom using for CustomCollection")]
        public List<NamespaceParam> CustomUsings
        {
            get { return this.customUsingsField; }
            set { this.customUsingsField = value; }
        }

        /// <summary>
        /// Gets or sets the custom usings string.
        /// </summary>
        /// <value>The custom usings string.</value>
        [Browsable(false)]
        public string CustomUsingsString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if implement INotifyPropertyChanged
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Indicating whether if implement INotifyPropertyChanged.")]
        public bool EnableDataBinding
        {
            get { return this.enableDataBinding; }
            set { this.enableDataBinding = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether serialize/deserialize method support
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether serialize/deserialize method nust be generate.")]
        public bool IncludeSerializeMethod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether serialize/deserialize method support
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether serialize/deserialize method nust be generate.")]
        public bool GenerateCloneMethod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether serialize/deserialize method support
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(CodeBase.NetFX20)]
        [Description("Generated code base")]
        public CodeBase Platform
        {
            get { return this.platform; }
            set { this.platform = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if generate EditorBrowsableState.Never attribute
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Indicating whether if generate EditorBrowsableState.Never attribute.")]
        public bool HidePrivateFieldInIde
        {
            get { return this.hidePrivateFieldInIdeField; }
            set { this.hidePrivateFieldInIdeField = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [disable debug].
        /// </summary>
        /// <value><c>true</c> if [disable debug]; otherwise, <c>false</c>.</value>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Indicating whether if generate attribute for debug into generated code.")]
        public bool DisableDebug
        {
            get { return this.disableDebugField; }
            set { this.disableDebugField = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if generate summary documentation
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicating whether if generate summary documentation from xsd annotation.")]
        public bool EnableSummaryComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if generate summary documentation
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Generate WCF data contract attributes")]
        public bool GenerateDataContracts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of Serialize method.
        /// </summary>
        [Category("Serialize")]
        [DefaultValue("Serialize")]
        [Description("The name of Serialize method.")]
        public string SerializeMethodName
        {
            get { return this.serializeMethodNameField; }
            set { this.serializeMethodNameField = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating the name of Deserialize method.
        /// </summary>
        [Category("Serialize")]
        [DefaultValue("Deserialize")]
        [Description("The name of deserialize method.")]
        public string DeserializeMethodName
        {
            get { return this.deserializeMethodNameField; }
            set { this.deserializeMethodNameField = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating the name of Serialize method.
        /// </summary>
        [Category("Serialize")]
        [DefaultValue("SaveToFile")]
        [Description("The name of save to xml file method.")]
        public string SaveToFileMethodName
        {
            get { return this.saveToFileMethodNameField; }
            set { this.saveToFileMethodNameField = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating the name of SaveToFile method.
        /// </summary>
        [Category("Serialize")]
        [DefaultValue("LoadFromFile")]
        [Description("The name of load from xml file method.")]
        public string LoadFromFileMethodName
        {
            get { return this.loadFromFileMethodNameField; }
            set { this.loadFromFileMethodNameField = value; }
        }

        /// <summary>
        /// Loads from file.
        /// </summary>
        /// <param name="xsdFilePath">The XSD file path.</param>
        /// <returns>GeneratorParams instance</returns>
        public static GeneratorParams LoadFromFile(string xsdFilePath)
        {
            string outFile;
            return LoadFromFile(xsdFilePath, out outFile);
        }

        /// <summary>
        /// Loads from file.
        /// </summary>
        /// <param name="xsdFilePath">The XSD file path.</param>
        /// <param name="outputFile">The output file.</param>
        /// <returns>GeneratorParams instance</returns>
        public static GeneratorParams LoadFromFile(string xsdFilePath, out string outputFile)
        {
            var parameters = new GeneratorParams();

            // TODO:Change this to default project code langage.

            #region Search generationFile

            outputFile = string.Empty;

            foreach (GenerationLanguage language in Enum.GetValues(typeof(GenerationLanguage)))
            {
                string fileName = Utility.GetOutputFilePath(xsdFilePath, language);
                if (File.Exists(fileName))
                {
                    outputFile = fileName;
                    break;
                }
            }


            if (outputFile.Length == 0)
                return null;

            #endregion

            #region Try to get Last options

            using (TextReader streamReader = new StreamReader(outputFile))
            {
                // TODO:Change this to search method
                streamReader.ReadLine();
                streamReader.ReadLine();
                streamReader.ReadLine();
                string optionLine = streamReader.ReadLine();
                if (optionLine != null)
                {
                    parameters.NameSpace = XmlHelper.ExtractStrFromXML(optionLine, GeneratorContext.NameSpaceTag);
                    parameters.CollectionObjectType =
                            Utility.ToEnum<CollectionType>(
                                    XmlHelper.ExtractStrFromXML(optionLine, GeneratorContext.CollectionTag));
                    parameters.Language =
                            Utility.ToEnum<GenerationLanguage>(XmlHelper.ExtractStrFromXML(optionLine,
                                                                                           GeneratorContext.CodeTypeTag));
                    parameters.EnableDataBinding =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.EnableDataBindingTag));
                    parameters.HidePrivateFieldInIde =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.HidePrivateFieldTag));
                    parameters.EnableSummaryComment =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.EnableSummaryCommentTag));
                    parameters.IncludeSerializeMethod =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.IncludeSerializeMethodTag));
                    parameters.GenerateCloneMethod =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.GenerateCloneMethodTag));
                    parameters.GenerateDataContracts =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.GenerateDataContractsTag));
                    parameters.Platform =
                            Utility.ToEnum<CodeBase>(optionLine.ExtractStrFromXML(GeneratorContext.CodeBaseTag));
                    parameters.DisableDebug =
                            Utility.ToBoolean(optionLine.ExtractStrFromXML(GeneratorContext.DisableDebugTag));

                    string str = optionLine.ExtractStrFromXML(GeneratorContext.SerializeMethodNameTag);
                    parameters.SerializeMethodName = str.Length > 0 ? str : "Serialize";

                    str = optionLine.ExtractStrFromXML(GeneratorContext.DeserializeMethodNameTag);
                    parameters.DeserializeMethodName = str.Length > 0 ? str : "Deserialize";

                    str = optionLine.ExtractStrFromXML(GeneratorContext.SaveToFileMethodNameTag);
                    parameters.SaveToFileMethodName = str.Length > 0 ? str : "SaveToFile";

                    str = optionLine.ExtractStrFromXML(GeneratorContext.LoadFromFileMethodNameTag);
                    parameters.LoadFromFileMethodName = str.Length > 0 ? str : "LoadFromFile";

                    // TODO:get custom using
                    string customUsingString = optionLine.ExtractStrFromXML(GeneratorContext.CustomUsingsTag);
                    if (!string.IsNullOrEmpty(customUsingString))
                    {
                        string[] usings = customUsingString.Split(';');
                        foreach (string item in usings)
                            parameters.CustomUsings.Add(new NamespaceParam { NameSpace = item });
                    }
                    parameters.CollectionBase = optionLine.ExtractStrFromXML(GeneratorContext.CollectionBaseTag);
                }
            }

            return parameters;

            #endregion
        }

        /// <summary>
        /// Gets the params.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>GeneratorParams instance</returns>
        public static GeneratorParams GetParams(string parameters)
        {
            var newparams = new GeneratorParams();

            return newparams;
        }

        /// <summary>
        /// Save values into xml string
        /// </summary>
        /// <returns>xml string value</returns>
        public string ToXmlTag()
        {
            var optionsLine = new StringBuilder();

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.NameSpaceTag, this.NameSpace));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.CollectionTag,
                                                          this.CollectionObjectType.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.CodeTypeTag, this.Language.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.EnableDataBindingTag,
                                                          this.EnableDataBinding.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.HidePrivateFieldTag,
                                                          this.HidePrivateFieldInIde.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.EnableSummaryCommentTag,
                                                          this.EnableSummaryComment.ToString()));

            if (!string.IsNullOrEmpty(this.CollectionBase))
                optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.CollectionBaseTag, this.CollectionBase));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.IncludeSerializeMethodTag,
                                                          this.IncludeSerializeMethod.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.GenerateCloneMethodTag,
                                                          this.GenerateCloneMethod.ToString()));

            /* RU20090225: TODO: Implement WCF attribute generation
              optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.GenerateDataContractsTag,
                                                          this.GenerateDataContracts.ToString()));
             */


            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.CodeBaseTag,
                                                          this.Platform.ToString()));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.SerializeMethodNameTag,
                                                          this.SerializeMethodName));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.DeserializeMethodNameTag,
                                                          this.DeserializeMethodName));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.SaveToFileMethodNameTag,
                                                          this.SaveToFileMethodName));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.LoadFromFileMethodNameTag,
                                                          this.LoadFromFileMethodName));

            optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.DisableDebugTag, this.DisableDebug.ToString()));

            var customUsingsStr = new StringBuilder();
            if (this.CustomUsings != null)
            {
                foreach (NamespaceParam usingStr in this.CustomUsings)
                {
                    if (usingStr.NameSpace.Length > 0)
                        customUsingsStr.Append(usingStr.NameSpace + ";");
                }

                // remove last ";"
                if (customUsingsStr.Length > 0)
                {
                    if (customUsingsStr[customUsingsStr.Length - 1] == ';')
                        customUsingsStr = customUsingsStr.Remove(customUsingsStr.Length - 1, 1);
                }

                optionsLine.Append(XmlHelper.InsertXMLFromStr(GeneratorContext.CustomUsingsTag,
                                                              customUsingsStr.ToString()));
            }

            return optionsLine.ToString();
        }

        /// <summary>
        /// Shallow clone
        /// </summary>
        /// <returns></returns>
        public GeneratorParams Clone()
        {
            return MemberwiseClone() as GeneratorParams;
        }

        public Result Validate()
        {
            var result = new Result(true);

            #region Validate input

            if (string.IsNullOrEmpty(this.NameSpace))
            {
                result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the NameSpace");
            }

            if (this.CollectionObjectType.ToString() == CollectionType.DefinedType.ToString())
            {
                if (string.IsNullOrEmpty(this.CollectionBase))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the custom collection base type");
                }
            }

            if (this.IncludeSerializeMethod)
            {
                if (string.IsNullOrEmpty(this.SerializeMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the Serialize method name.");
                }

                if (!IsValidMethodName(this.SerializeMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, string.Format("Serialize method name {0} is invalid.",
                                                  this.SerializeMethodName));
                }

                if (string.IsNullOrEmpty(this.DeserializeMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the Deserialize method name.");
                }

                if (!IsValidMethodName(this.DeserializeMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, string.Format("Deserialize method name {0} is invalid.",
                                                  this.DeserializeMethodName));
                }

                if (string.IsNullOrEmpty(this.SaveToFileMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the save to xml file method name.");
                }

                if (!IsValidMethodName(this.SaveToFileMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, string.Format("Save to file method name {0} is invalid.",
                                                  this.SaveToFileMethodName));
                }

                if (string.IsNullOrEmpty(this.LoadFromFileMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, "you must specify the load from xml file method name.");
                }

                if (!IsValidMethodName(this.LoadFromFileMethodName))
                {
                    result.Success = false; result.Messages.Add(MessageType.Error, string.Format("Load from file method name {0} is invalid.",
                                                  this.LoadFromFileMethodName));
                }
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static bool IsValidMethodName(string value)
        {
            foreach (char item in value)
            {
                int ascii = Convert.ToInt16(item);
                if ((ascii < 65 || ascii > 90) && (ascii < 97 || ascii > 122) && (ascii != 8))
                    return false;
            }
            return true;
        }

    }
}