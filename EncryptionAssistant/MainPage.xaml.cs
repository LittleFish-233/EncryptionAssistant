using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace EncryptionAssistant
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
        }

       

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (ContentFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }

        /*
        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                //to do
            }
            else
            {

                NavigationViewItem item = args.SelectedItem as NavigationViewItem;
                switch (item.Tag)
                {
                    case "home":
                        ContentFrame.Navigate(typeof(zhuye.zhuye));
                        break;
                    case "jiami":
                        ContentFrame.Navigate(typeof(jiami.jia_zhuye));
                        break;
                    case "jiemi":
                        ContentFrame.Navigate(typeof(jiemi.jie_zhuye));
                        break;
                    case "guanyu":
                        ContentFrame.Navigate(typeof(banzhu.wenzhi));
                        break;
                    case "md5_haxi":
                        ContentFrame.Navigate(typeof(MD5.md5_zhu));
                        break;

                }
            }
        }
        */

        private void NavView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                //to do
            }
            else
            {

                Microsoft.UI.Xaml.Controls.NavigationViewItem item = args.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItem;
                switch (item.Tag)
                {
                    case "home":
                        ContentFrame.Navigate(typeof(zhuye.zhuye));
                        break;
                    case "jiami":
                        ContentFrame.Navigate(typeof(jiami.jia_zhuye));
                        break;
                    case "jiemi":
                        ContentFrame.Navigate(typeof(jiemi.jie_zhuye));
                        break;
                    case "guanyu":
                        ContentFrame.Navigate(typeof(banzhu.wenzhi));
                        break;
                    case "md5_haxi":
                        ContentFrame.Navigate(typeof(MD5.md5_zhu));
                        break;
                    case "xiazai":
                        ContentFrame.Navigate(typeof(xiazai.xiazai_zhu));
                        break; 
                }
            }
        }
    }

}
