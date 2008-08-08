using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Xsd2Code.Library;
using System.Windows.Forms;

namespace Xsd2Code.Addin
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        private Command command;
        private CommandBar projectCmdBar;

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            this._applicationObject = (DTE2)application;
            this._addInInstance = (AddIn)addInInst;

            // Only execute the startup code if the connection mode is a startup mode
            if (connectMode == ext_ConnectMode.ext_cm_Startup)
            {
                object[] contextGUIDS = new object[] { };

                try
                {
                    // Create a Command with name SolnExplContextMenuCS and then add it to the "Item" menubar for the SolutionExplorer
                    command = _applicationObject.Commands.AddNamedCommand(_addInInstance, "Xsd2CodeAddin", "Run Xsd2Code generation", "Xsd2Code", true, 372, ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled);
                    projectCmdBar = ((CommandBars)_applicationObject.CommandBars)["Item"];

                    if (projectCmdBar == null)
                    {
                        System.Windows.Forms.MessageBox.Show("Cannot get the Project menubar", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                    else
                    {
                        command.AddControl(projectCmdBar, 1);
                    }
                }
                catch (Exception)
                {
                }
            }

        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        private DTE2 _applicationObject;
        private AddIn _addInInstance;

        #region IDTCommandTarget Members

        public void Exec(string CmdName, vsCommandExecOption ExecuteOption, ref object VariantIn, ref object VariantOut, ref bool Handled)
        {
            Handled = false;
            if (ExecuteOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                if (CmdName == "Xsd2Code.Addin.Connect.Xsd2CodeAddin")
                {
                    UIHierarchyItem item;
                    UIHierarchy UIH = _applicationObject.ToolWindows.SolutionExplorer;
                    item = (UIHierarchyItem)((Array)UIH.SelectedItems).GetValue(0);

                    ProjectItem Proitem = UIH.DTE.SelectedItems.Item(1).ProjectItem;

                    string fileName = Proitem.get_FileNames(0);
                    FormOption frm = new FormOption();
                    frm.Init(fileName);

                    DialogResult Result = frm.ShowDialog();

                    string NameSpace = frm.NameSpace;
                    GenerateCodeType Gen = frm.GenerateCodeType;
                    CollectionType Coll = frm.CollectionType;
                    bool UseIPropertyNotifyChanged = frm.UseIPropertyNotifyChanged;
                    bool HidePrivateField = frm.HidePrivateFieldInIDE;

                    if (fileName.Length > 0)
                    {
                        if (Result == DialogResult.OK)
                        {
                            Xsd2CodeGenerator gen = new Xsd2CodeGenerator(fileName, NameSpace, Gen, Coll, UseIPropertyNotifyChanged, HidePrivateField);
                            string ErrorMessage = "";
                            if (!gen.Process(out ErrorMessage))
                            {
                                MessageBox.Show(ErrorMessage, "XSD2Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    Handled = true;
                    return;
                }
            }
        }

        public void QueryStatus(string CmdName, vsCommandStatusTextWanted NeededText, ref vsCommandStatus StatusOption, ref object CommandText)
        {
            if (NeededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                StatusOption = vsCommandStatus.vsCommandStatusUnsupported;
                if (CmdName == "Xsd2Code.Addin.Connect.Xsd2CodeAddin")
                {
                    UIHierarchyItem item;
                    UIHierarchy UIH = _applicationObject.ToolWindows.SolutionExplorer;
                    item = (UIHierarchyItem)((Array)UIH.SelectedItems).GetValue(0);
                    if (item.Name.ToLower().EndsWith(".xsd"))
                    {
                        StatusOption = vsCommandStatus.vsCommandStatusSupported;
                        StatusOption |= vsCommandStatus.vsCommandStatusEnabled;
                    }
                }
            }
        }

        #endregion
    }
}