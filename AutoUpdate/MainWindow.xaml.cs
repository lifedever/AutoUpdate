using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace AutoUpdate
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string remote = "http://git.oschina.net/gefangshuai/BingApplication/raw/master/BingApplication/bin/setup/";
        private Thread downloadThread;
        private Version obj;
        public MainWindow()
        {
            InitializeComponent();
            doAction();
        }

        private void doAction()
        {
            // 获取版本信息
            string uri = "version.xml";
            WebClient client = new WebClient();
            string versionInfo = Encoding.UTF8.GetString(client.DownloadData(remote + uri));

            // 将信息转换为对象 
            obj = XmlHelper.XmlDeserailize(versionInfo, typeof(Version)) as Version;

            downloadThread = new Thread(download);
            downloadThread.Start();

        }

        private void download()
        {
            WebClient wc = new WebClient();
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate()
                    {
                        foreach (string item in obj.Files)
                        {
                            textBlockFileInfo.Text = string.Format("需更新文件总数：{0}, 正在更新第{1}个文件", obj.Files.Count, obj.Files.IndexOf(item)+1); 
                            wc.DownloadFileAsync(new Uri(remote + item), item);
                            wc.DownloadProgressChanged += client_DownloadProgressChanged;
                            wc.DownloadFileCompleted += client_DownloadFileCompleted;
                        }
                    });
        }

        private void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            textBlockFileInfo.Text = "下载完成！";
            textBlockSizeInfo.Visibility = Visibility.Hidden;
            buttonUpdate.Visibility = Visibility.Visible;
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            textBlockSizeInfo.Visibility = Visibility.Visible;
            long iTotalSize = e.TotalBytesToReceive;
            long iSize = e.BytesReceived;
            textBlockSizeInfo.Text = string.Format("文件大小总共 {1} KB, 当前已接收 {0} KB", (iSize / 1024), (iTotalSize / 1024)); 

            probar.Value = Convert.ToDouble(iSize) / Convert.ToDouble(iTotalSize) * 100;
        }

        private void updateProgressBar()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate()
                {

                });
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "setup.exe";
            process.Start();
            Application.Current.Shutdown();
        }
    }
}
