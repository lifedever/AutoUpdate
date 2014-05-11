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
        private string remote;
        private Version obj;
        

        private Thread downloadThread;
        
        public MainWindow(string remote, Version obj)
        {
            InitializeComponent();

            try
            {
                this.remote = remote;
                this.obj = obj;
                doAction();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void doAction()
        {
            
           

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


        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "setup.exe";
            process.Start();
            System.Environment.Exit(0);
        }

    }
}
