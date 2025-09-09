using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace WinShareEnum
{
    /// <summary>
    /// Interaction logic for options.xaml
    /// </summary>
    public partial class setup : Window
    {
        public static List<IPAddress> ImportedIPs = new List<IPAddress>();
        public static bool useImportedIPs = false;

        private bool isUpdating = false;

        // Default values to check against
        private readonly string defaultDomainName = "EARTH.LAB";
        private readonly string defaultDomainController = "DC01.EARTH.LAB";
        private readonly string defaultNeo4jUri = "bolt://localhost:7687";
        private readonly string defaultIPRange = "10.1.2.3-250";
        private readonly string defaultUsername = "Username";

        public setup()
        {
            InitializeComponent();

            //cb_recursiveSearch.IsChecked = MainWindow.recursiveSearch;

            //cb_includeBinaryFiles.IsChecked = MainWindow.includeBinaryFiles;

            //tb_domain_name.Text = MainWindow.MAX_FILESIZE.ToString();

        }
        private void tbIPRange_TextChanged(object sender, TextChangedEventArgs e)
        {
            //assume since they changed the IP list, they no longer want to use their imported list..
            if (tbIPRange.Text.ToLower() != "using imported list")
            {
                useImportedIPs = false;
            }
        }

        #region logging
        private void rb_debug_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.logLevel = MainWindow.LOG_LEVEL.DEBUG;
        }

        private void rb_info_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.logLevel = MainWindow.LOG_LEVEL.INFO;
        }

        private void rb_error_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.logLevel = MainWindow.LOG_LEVEL.ERROR;
        }

        private void rb_useful_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.logLevel = MainWindow.LOG_LEVEL.INTERESTINGONLY;
        }

        #endregion

        private void tb_domain_name_changed(object sender, TextChangedEventArgs e)
        {

            //MainWindow.MAX_FILESIZE = fileSize;
            //MessageBox.Show("You entered " + tb_domain_name.Text);
        }

        private void cb_recursiveSearch_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.recursiveSearch = true;
        }

        private void cb_recursiveSearch_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.recursiveSearch = false;
        }

        private void cb_includeBinaryFiles_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.includeBinaryFiles = true;
        }

        private void cb_includeBinaryFiles_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.includeBinaryFiles = false;
        }


        private void cb_ResolveSIDs_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.resolveGroupSIDs = true;
        }

        private void cb_ResolveSIDs_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.resolveGroupSIDs = false;
        }

        private void cb_includeWindowsFiles_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.INCLUDE_WINDOWS_DIRS = true;
        }

        private void cb_includeWindowsFiles_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.INCLUDE_WINDOWS_DIRS = false;
        }

        private void tb_ip_input1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tb_domain_input2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OnDomainInputChanged(object sender, EventArgs e)
        {
            if (!isUpdating)
            {
                isUpdating = true;
                UpdateControlStates("domain");
                isUpdating = false;
            }
        }

        private void OnBloodHoundInputChanged(object sender, EventArgs e)
        {
            if (!isUpdating)
            {
                isUpdating = true;
                UpdateControlStates("bloodhound");
                isUpdating = false;
            }
        }

        private void OnIPRangeInputChanged(object sender, EventArgs e)
        {
            if (!isUpdating)
            {
                isUpdating = true;
                UpdateControlStates("iprange");
                isUpdating = false;
            }
        }

        private void OnAuthInputChanged(object sender, EventArgs e)
        {
            // Authentication section doesn't participate in mutual exclusion
            // Do nothing - let users freely enter auth credentials
        }

        private void UpdateControlStates(string activeSection)
        {
            // Check if any inputs have been modified from defaults
            bool domainModified = HasDomainInput();
            bool bloodhoundModified = HasBloodHoundInput();
            bool iprangeModified = HasIPRangeInput();

            // Enable/disable sections based on which one is active
            // Authentication should NEVER be disabled - removed from all conditions
            gb_domain_setup.IsEnabled = activeSection == "domain" || (!bloodhoundModified && !iprangeModified);
            gb_bloodhound_setup.IsEnabled = activeSection == "bloodhound" || (!domainModified && !iprangeModified);
            gb_iprange_setup.IsEnabled = activeSection == "iprange" || (!domainModified && !bloodhoundModified);

            // Authentication is always enabled
            gb_auth_setup.IsEnabled = true;
        }

        private bool HasDomainInput()
        {
            // Check if there's any actual input in domain fields
            return !string.IsNullOrWhiteSpace(tb_domain_input1.Text) ||
                   !string.IsNullOrWhiteSpace(tb_domain_input2.Text) ||
                   cb_scan_workstations.IsChecked == true;
        }

        private bool HasBloodHoundInput()
        {
            // Check if there's any actual input in BloodHound fields
            return !string.IsNullOrWhiteSpace(tb_neo4j_uri.Text) ||
                   !string.IsNullOrWhiteSpace(tb_neo4j_username.Text) ||
                   !string.IsNullOrWhiteSpace(pb_neo4j_password.Password) ||
                   cb_bloodhound_enabled.IsChecked == true;
        }

        private bool HasIPRangeInput()
        {
            // Check if there's any actual input in IP range fields
            return !string.IsNullOrWhiteSpace(tbIPRange.Text) ||
                   cb_netbios_lookup.IsChecked == true;
        }

        private bool HasAuthInput()
        {
            // Check if there's any actual input in auth fields
            return !string.IsNullOrWhiteSpace(tb_auth_username.Text) ||
                   !string.IsNullOrWhiteSpace(pb_auth_password.Password);
        }
    }
}