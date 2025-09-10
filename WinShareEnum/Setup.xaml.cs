using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WinShareEnum
{
    /// <summary>
    /// Interaction logic for setup.xaml
    /// </summary>
    public partial class setup : Window
    {
        public static List<IPAddress> ImportedIPs = new List<IPAddress>();
        public static bool useImportedIPs = false;

        public static string USERNAME = "";
        public static string PASSSWORD = "";
        public static bool UseNullSession { get; set; } = false;

        // Add static property for IP range text
        public static string IPRangeText { get; set; } = "";

        private bool isUpdating = false;

        public setup()
        {
            InitializeComponent();
        }

        public void tbIPRange_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update the static property whenever the textbox changes
            IPRangeText = tbIPRange.Text;

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

        private void tbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pb_auth_password.Password != "")
            {
                PASSSWORD = pb_auth_password.Password;
            }
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

        // Fixed: Use consistent naming for null session checkbox
        private void cb_use_null_session_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAuthenticationControls();

            // Clear credentials when null session is enabled
            USERNAME = "";
            PASSSWORD = "";
        }

        private void cb_use_null_session_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateAuthenticationControls();
        }

        private void UpdateAuthenticationControls()
        {
            // Check if the null session checkbox exists and is checked
            bool useNullSession = cb_use_null_session?.IsChecked == true;

            // Disable/enable authentication fields based on null session checkbox
            if (tb_auth_username != null)
                tb_auth_username.IsEnabled = !useNullSession;
            if (pb_auth_password != null)
                pb_auth_password.IsEnabled = !useNullSession;
            if (lbl_username != null)
                lbl_username.IsEnabled = !useNullSession;
            if (lbl_password != null)
                lbl_password.IsEnabled = !useNullSession;

            // If switching to null session, clear the fields
            if (useNullSession)
            {
                if (tb_auth_username != null)
                    tb_auth_username.Text = string.Empty;
                if (pb_auth_password != null)
                    pb_auth_password.Password = string.Empty;
            }
        }

        private void UpdateControlStates(string activeSection)
        {
            // Check if any inputs have been modified from defaults
            bool domainModified = HasDomainInput();
            bool bloodhoundModified = HasBloodHoundInput();
            bool iprangeModified = HasIPRangeInput();

            // Enable/disable sections based on which one is active
            // Authentication should NEVER be disabled - removed from all conditions
            if (gb_domain_setup != null)
                gb_domain_setup.IsEnabled = activeSection == "domain" || (!bloodhoundModified && !iprangeModified);
            if (gb_bloodhound_setup != null)
                gb_bloodhound_setup.IsEnabled = activeSection == "bloodhound" || (!domainModified && !iprangeModified);
            if (gb_iprange_setup != null)
                gb_iprange_setup.IsEnabled = activeSection == "iprange" || (!domainModified && !bloodhoundModified);

            // Authentication is always enabled
            if (gb_auth_setup != null)
                gb_auth_setup.IsEnabled = true;

            // Update authentication controls based on null session setting
            UpdateAuthenticationControls();
        }

        private bool HasDomainInput()
        {
            // Check if there's any actual input in domain fields
            return (!string.IsNullOrWhiteSpace(tb_domain_input1?.Text)) ||
                   (!string.IsNullOrWhiteSpace(tb_domain_input2?.Text)) ||
                   (cb_scan_workstations?.IsChecked == true);
        }

        private bool HasBloodHoundInput()
        {
            // Check if there's any actual input in BloodHound fields
            return (!string.IsNullOrWhiteSpace(tb_neo4j_uri?.Text)) ||
                   (!string.IsNullOrWhiteSpace(tb_neo4j_username?.Text)) ||
                   (!string.IsNullOrWhiteSpace(pb_neo4j_password?.Password)) ||
                   (cb_bloodhound_enabled?.IsChecked == true);
        }

        private bool HasIPRangeInput()
        {
            // Check if there's any actual input in IP range fields
            return (!string.IsNullOrWhiteSpace(tbIPRange?.Text)) ||
                   (cb_netbios_lookup?.IsChecked == true);
        }

        private bool HasAuthInput()
        {
            // Check if there's any actual input in auth fields
            // Don't count inputs if null session is enabled
            if (cb_use_null_session?.IsChecked == true)
                return false;

            return (!string.IsNullOrWhiteSpace(tb_auth_username?.Text)) ||
                   (!string.IsNullOrWhiteSpace(pb_auth_password?.Password));
        }

        private void ClearAllFields()
        {
            // Clear domain setup fields
            if (tb_domain_input1 != null) tb_domain_input1.Text = string.Empty;
            if (tb_domain_input2 != null) tb_domain_input2.Text = string.Empty;
            if (cb_scan_workstations != null) cb_scan_workstations.IsChecked = false;

            // Clear BloodHound setup fields
            if (tb_neo4j_uri != null) tb_neo4j_uri.Text = string.Empty;
            if (tb_neo4j_username != null) tb_neo4j_username.Text = string.Empty;
            if (pb_neo4j_password != null) pb_neo4j_password.Password = string.Empty;
            if (cb_bloodhound_enabled != null) cb_bloodhound_enabled.IsChecked = false;

            // Clear IP range setup fields
            if (tbIPRange != null) tbIPRange.Text = string.Empty;
            IPRangeText = string.Empty; // Clear the static property too
            if (cb_netbios_lookup != null) cb_netbios_lookup.IsChecked = false;

            // Clear authentication fields
            if (tb_auth_username != null) tb_auth_username.Text = string.Empty;
            if (pb_auth_password != null) pb_auth_password.Password = string.Empty;
            if (cb_use_null_session != null) cb_use_null_session.IsChecked = false;

            // Reset control states
            if (gb_domain_setup != null) gb_domain_setup.IsEnabled = true;
            if (gb_bloodhound_setup != null) gb_bloodhound_setup.IsEnabled = true;
            if (gb_iprange_setup != null) gb_iprange_setup.IsEnabled = true;
            if (gb_auth_setup != null) gb_auth_setup.IsEnabled = true;

            // Update authentication controls to ensure they're enabled
            UpdateAuthenticationControls();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearAllFields();
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            // Save/apply settings here before closing
            this.Close();
        }

        private void tbUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_auth_username?.Text != "")
            {
                USERNAME = tb_auth_username.Text;
            }
        }
    }
}