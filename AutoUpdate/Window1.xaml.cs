using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoUpdate
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        private string remote;
        private string version;
        private const string VERSION_FILE = "version.xml";
        private Version versionObj;

        public Window1()
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
            remote = Application.Current.Properties["url"].ToString();
            version = Application.Current.Properties["version"].ToString();

            // 获取版本信息
            WebClient client = new WebClient();
            string versionInfo = Encoding.UTF8.GetString(client.DownloadData(remote + VERSION_FILE));

            // 将信息转换为对象 
            versionObj = XmlHelper.XmlDeserailize(versionInfo, typeof(Version)) as Version;

            if (versionObj.AppVersion.Equals(version))
            {
                Environment.Exit(0);
            }
            else
            {
                this.Visibility = Visibility.Visible;
                List<String> infos = versionObj.AppInfo;
                string content = "";
                foreach (string item in infos)
	            {
                    if (infos.IndexOf(item) == infos.Count-1)
                    {
                        content += item.Trim();
                    }
                    else 
                    {
                        content += item.Trim() + "\n";
                    }
	            }

                groupBox.Content = content;
            }

        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            
            this.Hide();
            MainWindow main = new MainWindow(remote, versionObj);
            main.Owner = this;
            main.Show();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
