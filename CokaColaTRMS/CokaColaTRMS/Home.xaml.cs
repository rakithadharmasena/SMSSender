using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CokaColaTRMS.Models;
using CokaColaTRMS.Helpers;
using System.IO.Ports;
using CokaColaTRMS.Common;
using System.ComponentModel;

namespace CokaColaTRMS
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {

        #region SMS related members

        SerialPort port = new SerialPort();
        SMSHelper smsHelper = new SMSHelper();

        #endregion

        #region members

        private const int ContactsTabNumber = 0;
        private const int ConfigureTabNumber = 1;
        //Flags
        //shows wether modem is connected
        private bool modemIsConnected = false;
        #endregion
        
        #region Constructor

        public Home()
        {
            InitializeComponent();
        } 
        #endregion

        #region Events
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selectedTabID = tabMain.SelectedIndex;

                if (selectedTabID < 0)
                {
                    return;
                }

                //contacts tab selected
                if (selectedTabID.Equals(ConfigureTabNumber))
                {
                    //load the ports combo box
                    if (!this.modemIsConnected)
                    {
                        List<string> portsList = new List<string>();
                        portsList = PortHelper.GetModemPorts();
                        if (portsList.Count > 0)
                        {
                            cbPortName.ItemsSource = portsList;
                           
                        }
                        else
                        {
                            configureStatusBar.Text = "No modem detected";
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void tabContacts_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = "Document"; // Default file name 
                dlg.DefaultExt = ".xlsx"; // Default file extension 
                dlg.Filter = "Excel documents (.xlsx)|*.xlsx"; // Filter files by extension    
                dlg.Multiselect = false;

                dlg.ShowDialog();

                tbFilePath.Text = dlg.FileName;
            }
            catch (Exception)
            {

            }
        }

        private void btnImportExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.SetCursor(Cursors.Wait);

                if (string.IsNullOrWhiteSpace(tbFilePath.Text))
                {
                    ContactsTabStatusBar.Text = "Please select a file";
                    return;
                }

                System.Collections.ObjectModel.ObservableCollection<ImportContactsModel> importedContacts = new System.Collections.ObjectModel.ObservableCollection<ImportContactsModel>(ExcelHelper.GetContactsFromExcel(tbFilePath.Text));
                if (importedContacts == null || importedContacts.Count.Equals(0))
                {
                    ContactsTabStatusBar.Text = "No Contacts were imported";
                    return;
                }

                contactsGrid.ItemsSource = importedContacts;

                //send the texts
                #region Send the texts
                foreach (ImportContactsModel contact in importedContacts)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(contact.TelephoeNumber))
                            continue;

                        if (smsHelper.sendMsg(this.port, contact.TelephoeNumber, contact.Message))
                        {
                            contact.MessageSent = true;
                        }
                        else
                        {
                            contact.MessageSent = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        contact.MessageSent = false;
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {

            }

            finally
            {
                Mouse.SetCursor(Cursors.Arrow);
            }
        }


        private void btnConnectModem_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(cbPortName.Text))
                {
                    configureStatusBar.Text = "Please select a port";
                    return;
                }

                //Open communication port 
                this.port = smsHelper.OpenPort(
                    this.cbPortName.Text,
                    Convert.ToInt32(this.cbBaudRate.Text),
                    Convert.ToInt32(this.cbDataBits.Text),
                    Convert.ToInt32(this.tbReadTimeOut.Text),
                    Convert.ToInt32(this.tbWriteTimeOut.Text));
            }
            catch (Exception ex)
            {
                configureStatusBar.Text = "Cannot connect to modem using " + cbPortName.Text;
                return;
            }

            configureStatusBar.Text = "Successfully connected to modem using " + cbPortName.Text;
            btnConnectModem.IsEnabled = false;
            btnDisconnectModem.IsEnabled = true;
            tabContacts.IsEnabled = true;
        }

        private void tabConfigure_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                btnDisconnectModem.IsEnabled = false;
            }
            catch (Exception)
            {

            }
        }

        private void btnDisconnectModem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                smsHelper.ClosePort(this.port);
                btnDisconnectModem.IsEnabled = false;
                btnConnectModem.IsEnabled = true;
                configureStatusBar.Text = "Modem Disconnected";
            }
            catch (Exception ex)
            {
                configureStatusBar.Text = "An error occured while disconnecting modem";
                return;
            }
        }

        #endregion

        #region Public Methods

        public void SelectTab(Tabs tabToSelect)
        {
            try
            {
                switch (tabToSelect)
                {
                    case Tabs.ContactsTab:
                        tabMain.SelectedIndex = ContactsTabNumber;
                        break;
                    case Tabs.ConfigureTab:
                        tabMain.SelectedIndex = ConfigureTabNumber;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                
            }
        }

        public void ToggleTabEnable(Tabs tabToDisable,bool tabEnabled)
        {
            try
            {
                switch (tabToDisable)
                {
                    case Tabs.ContactsTab:
                        tabContacts.IsEnabled = tabEnabled;
                        break;
                    case Tabs.ConfigureTab:
                        tabConfigure.IsEnabled = tabEnabled;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                
            }
        }
        #endregion

    }
}
