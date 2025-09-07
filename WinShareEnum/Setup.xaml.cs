using System;
using System.Windows;
using System.Windows.Controls;

namespace WinShareEnum
{
    /// <summary>
    /// Interaction logic for options.xaml
    /// </summary>
    public partial class setup : Window
    {
        public setup()
        {
            InitializeComponent();




            cb_recursiveSearch.IsChecked = MainWindow.recursiveSearch;

            cb_includeBinaryFiles.IsChecked = MainWindow.includeBinaryFiles;

            tb_max_fileSize.Text = MainWindow.MAX_FILESIZE.ToString();

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

        private void tb_max_fileSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            int fileSize;
            if (!int.TryParse(tb_max_fileSize.Text, out fileSize))
            {
                MessageBox.Show("Filesize can only be a number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MainWindow.MAX_FILESIZE = fileSize;
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
    }
}
