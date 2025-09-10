using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace WinShareEnum
{
    /// <summary>
    /// saves to file, nb this also updates the program internal lists (interestingFileList, fileContentsFilters)
    /// </summary>
    public class persistance
    {
        public persistance()
        {
            if (Settings.Default.interestingFileNameRules == null)
            {
                Settings.Default.interestingFileNameRules = new StringCollection();
                foreach (string s in MainWindow.interestingFileList)
                {
                    Settings.Default.interestingFileNameRules.Add(s);
                }
                Settings.Default.Save();
            }

            if (Settings.Default.FileContentRules == null)
            {
                Settings.Default.FileContentRules = new StringCollection();
                foreach (string s in MainWindow.fileContentsFilters)
                {
                    Settings.Default.FileContentRules.Add(s);
                }

                Settings.Default.Save();
            }
        }

        #region get
        public static List<string> getInterestingFiles()
        {
            return Settings.Default.interestingFileNameRules.Cast<string>().ToList();
        }

        public static List<string> getFileContentRules()
        {
            return Settings.Default.FileContentRules.Cast<string>().ToList();
        }
        #endregion

        #region set
        public static void saveInterestingRule(string interesting)
        {
            Settings.Default.interestingFileNameRules.Add(interesting);
            MainWindow.interestingFileList.Add(interesting);
            Settings.Default.Save();
        }

        public static void saveFileContentRule(string fileContent)
        {
            Settings.Default.FileContentRules.Add(fileContent);
            MainWindow.fileContentsFilters.Add(fileContent);
            Settings.Default.Save();
        }
        #endregion

        #region delete
        public static void deleteInterestingRule(string interesting)
        {
            Settings.Default.interestingFileNameRules.Remove(interesting);
            MainWindow.interestingFileList.Remove(interesting);
            Settings.Default.Save();
        }

        public static void deleteFileContentRule(string fileContent)
        {
            Settings.Default.FileContentRules.Remove(fileContent);
            MainWindow.fileContentsFilters.Remove(fileContent);
            Settings.Default.Save();
        }
        #endregion
    }
}