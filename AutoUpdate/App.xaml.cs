using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace AutoUpdate
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
          
            base.OnStartup(e);

            if (e.Args != null && e.Args.Length >= 2)
            {
                Application.Current.Properties["url"] = e.Args[0];
                Application.Current.Properties["version"] = e.Args[1];
            }
            else
            {
                //Application.Current.Properties["url"] = "http://git.oschina.net/gefangshuai/app/";
                Application.Current.Properties["url"] = "http://git.oschina.net/gefangshuai/BingApplication/raw/master/BingApplication/bin/setup/";
                Application.Current.Properties["version"] = "v0.0.1";
            }

        }
    }
}
