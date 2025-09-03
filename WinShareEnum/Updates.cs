using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace WinShareEnum
{
    public class updates
    {
        public static double getCurrentVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            return double.Parse(version);
        }

        public static List<string> getInterestingFileUpdates()
        {
            return readFromSite(new Uri("https://raw.githubusercontent.com/nccgroup/WinShareEnum/master/Info/interestingFiles.txt"));
        }

        public static List<string> getFileFilterUpdates()
        {
            return readFromSite(new Uri("https://raw.githubusercontent.com/nccgroup/WinShareEnum/master/Info/filterRules.txt"));
        }

        public static double getLatestVersion()
        {
            return double.Parse(readFromSite(new Uri("https://raw.githubusercontent.com/nccgroup/WinShareEnum/master/Info/version.txt"))[0]);
        }

        public static string downloadUpdate(double newestVersion)
        {
            WebClient client = new WebClient();
            client.Proxy = null;
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\WinShareEnum-" + newestVersion.ToString() + ".exe";

            if (File.Exists(filePath))
            {
                return filePath;
            }

            client.DownloadFile("https://github.com/nccgroup/WinShareEnum/raw/master/Info/WinShareEnum.exe", filePath);
            return filePath;
        }

        private static List<string> readFromSite(Uri url)
        {
            WebClient client = new WebClient();
            client.Proxy = null;
            return client.DownloadString(url).Split('\n').ToList<string>();
        }
    }
}