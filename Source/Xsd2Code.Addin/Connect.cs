//-----------------------------------------------------------------------
// <copyright file="Connect.cs" company="Xsd2Code">
//     Copyright (c) Pascal Cabanel.
// </copyright>
//-----------------------------------------------------------------------

namespace Xsd2Code.Addin
{
    using System;
    using System.Windows.Forms;
    using EnvDTE;
    using EnvDTE80;
    using Extensibility;
    using Microsoft.VisualStudio.CommandBars;
    using Xsd2Code.Library;

    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        #region Fields
        /// <summary>
        /// EnvDTE command
        /// </summary>
        private Command commandField;

        /// <summary>
        /// CommandBar command
        /// </summary>
        private CommandBar projectCmdBarField;

        /// <summary>
        /// interface DTE2
        /// </summary>
        private DTE2 applicationObjectField;

        /// <summary>
        /// interface AddIn
        /// </summary>
        private AddIn addInInstanceField;
        #endregion

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        /// <summary>
        /// Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.
        /// </summary>
        /// <param name="application">Root object of the host application.</param>
        /// <param name="connectMode">Describes how the Add-in is being loaded.</param>
        /// <param name="addInInst">Object representing this Add-in.</param>
        /// <param name="custom">Array of custom params</param>
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            this.applicationObjectField = (DTE2)application;
            this.addInInstanceField = (AddIn)addInInst;



            // Only execute the startup code if the connection mode is a startup mode
            if (connectMode == ext_ConnectMode.ext_cm_Startup)
            {
                object[] contextGUIDS = new object[] { };

                try
                {
                    // Create a Command with name SolnExplContextMenuCS and then add it to the "Item" menubar for the SolutionExplorer
                    this.commandField = this.applicationObjectField.Commands.AddNamedCommand(this.addInInstanceField, "Xsd2CodeAddin", "Run Xsd2Code generation", "Xsd2Code", true, 372, ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled);
                    this.projectCmdBarField = ((CommandBars)this.applicationObjectField.CommandBars)["Item"];

                    if (this.projectCmdBarField == null)
                    {
                        System.Windows.Forms.MessageBox.Show("Cannot get the Project menubar", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                    else
                    {
                        this.commandField.AddControl(this.projectCmdBarField, 1);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param name='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param name='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param name='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param name='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param name='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        #region IDTCommandTarget Members

        /// <summary>
        /// Execute Addin Command
        /// </summary>
        /// <param name="cmdName">Command name</param>
        /// <param name="executeOption">Execute options</param>
        /// <param name="variantIn">object variant in</param>
        /// <param name="variantOut">object variant out</param>
        /// <param name="handled">Handled true or false</param>
        public void Exec(string cmdName, vsCommandExecOption executeOption, ref object variantIn, ref object variantOut, ref bool handled)
        {
            handled = false;
            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                if (cmdName == "Xsd2Code.Addin.Connect.Xsd2CodeAddin")
                {
                    UIHierarchyItem item;
                    UIHierarchy uIH = this.applicationObjectField.ToolWindows.SolutionExplorer;
                    item = (UIHierarchyItem)((Array)uIH.SelectedItems).GetValue(0);
                    UIHierarchyItems items = item.UIHierarchyItems;
                    ProjectItem proitem = uIH.DTE.SelectedItems.Item(1).ProjectItem;
                    Project proj = proitem.ContainingProject;

                    string lg = proj.CodeModel.Language;

                    string fileName = proitem.get_FileNames(0);

                    try
                    {
                        proitem.Save(fileName);
                    }
                    catch (Exception)
                    {
                    }

                    FormOption frm = new FormOption();
                    frm.Init(fileName);

                    DialogResult result = frm.ShowDialog();

                    string nameSpaceStr = frm.NameSpace;
                    GenerationLanguage generateLanguage = frm.GenerateCodeType;
                    CollectionType collectionType = frm.CollectionType;
                    bool useIPropertyNotifyChanged = frm.UseIPropertyNotifyChanged;
                    bool hidePrivateField = frm.HidePrivateFieldInIDE;
                    bool enableSummaryComment = frm.EnableSummaryComment;
                    bool includeSerializeMethod = frm.IncludeSerializeMethod;
                    string SerializeMethod = frm.SerializeMethodName;
                    string DeserializeMethod = frm.DeserializeMethodName;

                    string saveTofileMethod = frm.SaveTofileMethodName;
                    string loadFromFileMethod = frm.LoadFromFileMethodName;
                    bool DisableDebug = frm.DisableDebug;

                    string customUsings = "";
                    foreach (var usingStr in frm.CustomUsings)
                    {
                        if (usingStr.Length > 0)
                        {
                            customUsings += usingStr + ";";
                        }
                    }

                    // remove last ";"
                    if (frm.CustomUsings.Count > 0)
                    {
                        if (customUsings[customUsings.Length - 1] == ';')
                        {
                            customUsings = customUsings.Substring(0, customUsings.Length - 1);
                        }
                    }

                    string collectionBase = frm.CollectionBase;
                    GeneratorFacade gen = new GeneratorFacade(fileName, nameSpaceStr, generateLanguage, collectionType, useIPropertyNotifyChanged, hidePrivateField, enableSummaryComment, customUsings, collectionBase, includeSerializeMethod, SerializeMethod, DeserializeMethod, saveTofileMethod, loadFromFileMethod, DisableDebug);

                    // Close file if open in IDE
                    ProjectItem projElmts = null;
                    bool found = this.FindInProject(proj.ProjectItems, gen.OutputFile, out projElmts);
                    if (found)
                    {
                        Window window = projElmts.Open(Constants.vsViewKindCode);
                        window.Close(vsSaveChanges.vsSaveChangesNo);
                    }

                    if (fileName.Length > 0)
                    {
                        if (result == DialogResult.OK)
                        {
                            string errorMessage = "";
                            string outputFileName = "";
                            if (!gen.ProcessCodeGeneration(out outputFileName, out errorMessage))
                            {
                                MessageBox.Show(errorMessage, "XSD2Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                if (!found)
                                {
                                    projElmts = proitem.Collection.AddFromFile(outputFileName);
                                }
                                Window window = projElmts.Open(Constants.vsViewKindCode);                                                                
                                window.Activate();
                                window.SetFocus();

                                try
                                {                                    
                                    applicationObjectField.DTE.ExecuteCommand("Edit.RemoveAndSort", "");
                                    applicationObjectField.DTE.ExecuteCommand("Edit.FormatDocument", "");
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }

                    handled = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Returns the current status (enabled, disabled, hidden, and so forth) of the specified named command
        /// </summary>
        /// <param name="cmdName">Command Name</param>
        /// <param name="neededText">Constant specifying if information is returned from the check</param>
        /// <param name="statusOption">The current status of the command</param>
        /// <param name="commandText">Command text value</param>
        public void QueryStatus(string cmdName, vsCommandStatusTextWanted neededText, ref vsCommandStatus statusOption, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                statusOption = vsCommandStatus.vsCommandStatusUnsupported;
                if (cmdName == "Xsd2Code.Addin.Connect.Xsd2CodeAddin")
                {
                    UIHierarchyItem item;
                    UIHierarchy uIH = this.applicationObjectField.ToolWindows.SolutionExplorer;
                    item = (UIHierarchyItem)((Array)uIH.SelectedItems).GetValue(0);
                    if (item.Name.ToLower().EndsWith(".xsd"))
                    {
                        statusOption = vsCommandStatus.vsCommandStatusSupported;
                        statusOption |= vsCommandStatus.vsCommandStatusEnabled;
                    }
                }
            }
        }

        /// <summary>
        /// Recursive search in project solution.
        /// </summary>
        /// <param name="projectItems">Root projectItems</param>
        /// <param name="filename">Full path of search element</param>
        /// <param name="item">output ProjectItem</param>
        /// <returns>true if found else false</returns>
        private bool FindInProject(ProjectItems projectItems, string filename, out ProjectItem item)
        {
            item = null;
            if (projectItems == null)
            {
                return false;
            }

            foreach (ProjectItem projElmts in projectItems)
            {
                if (projElmts.get_FileNames(0) == filename)
                {
                    item = projElmts;
                    return true;
                }
                else
                {
                    if (this.FindInProject(projElmts.ProjectItems, filename, out item))
                        return true;
                }
            }

            return false;
        }
        #endregion
    }
}