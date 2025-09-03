using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace WinShareEnum
{
    /// <summary>
    /// impersonation helper
    /// </summary>
    public class RemoteAccessHelper
    {
        public class NetworkConnection : IDisposable
        {
            string _networkName;

            public NetworkConnection(string networkName, NetworkCredential credentials, bool timeout)
            {
                _networkName = networkName;

                var netResource = new NetResource()
                {
                    Scope = ResourceScope.GlobalNetwork,
                    ResourceType = ResourceType.Disk,
                    DisplayType = ResourceDisplaytype.Share,
                    RemoteName = networkName
                };

                try
                {
                    var result = 0;

                    if (timeout == true)
                    {
                        var tokenSource = new CancellationTokenSource();
                        CancellationToken token = tokenSource.Token;
                        int timeOut = MainWindow.TIMEOUT;

                        var task = Task.Factory.StartNew(() =>
                        {
                            result = WNetAddConnection2(
                            netResource,
                            credentials.Password,
                            credentials.UserName,
                            0);

                        }, token);

                        if (!task.Wait(timeOut, token))
                            throw new Win32Exception("The request timed out.");

                    }
                    else
                    {

                        result = WNetAddConnection2(
                        netResource,
                        credentials.Password,
                        credentials.UserName,
                        0);

                    }
                    if (result != 0)
                    {
                        throw new Win32Exception(result);
                    }
                }

                catch (Exception ex)
                {
                    throw new Exception("Error connecting to remote share: " + ex.Message);
                }
            }

            ~NetworkConnection()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                WNetCancelConnection2(_networkName, 0, true);
            }

            [DllImport("mpr.dll")]
            private static extern int WNetAddConnection2(NetResource netResource,
                string password, string username, int flags);

            [DllImport("mpr.dll")]
            private static extern int WNetCancelConnection2(string name, int flags,
                bool force);
        }

        [StructLayout(LayoutKind.Sequential)]
        public class NetResource
        {
            public ResourceScope Scope;
            public ResourceType ResourceType;
            public ResourceDisplaytype DisplayType;
            public int Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        public enum ResourceScope : int
        {
            Connected = 1,
            GlobalNetwork,
            Remembered,
            Recent,
            Context
        };

        public enum ResourceType : int
        {
            Any = 0,
            Disk = 1,
            Print = 2,
            Reserved = 8,
        }

        public enum ResourceDisplaytype : int
        {
            Generic = 0x0,
            Domain = 0x01,
            Server = 0x02,
            Share = 0x03,
            File = 0x04,
            Group = 0x05,
            Network = 0x06,
            Root = 0x07,
            Shareadmin = 0x08,
            Directory = 0x09,
            Tree = 0x0a,
            Ndscontainer = 0x0b
        }
    }
}