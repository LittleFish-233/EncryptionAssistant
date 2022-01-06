using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace EncryptionAssistant.xiazai
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class wangcheng : Page
    {
        public wangcheng()
        {
            Loaded += Wangcheng_Loaded;
            this.InitializeComponent();
        }

        private void Wangcheng_Loaded(object sender, RoutedEventArgs e)
        {
            listview.ItemsSource = App.Huancun.xiazai.xiazai_guanli.wangcheng_list;
        }

        private void Qinchu_Click(object sender, RoutedEventArgs e)
        {
            App.Huancun.xiazai.xiazai_guanli.qingkong_wancheng();
        }

        private void Listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void Dakai_Click(object sender, RoutedEventArgs e)
        {
            daima.Yuan_xiazai linshi = (daima.Yuan_xiazai)(sender as Button).DataContext;
            var t = new FolderLauncherOptions();
            t.ItemsToSelect.Add(linshi.file);
            await Launcher.LaunchFolderAsync(linshi.baocun_dizhi, t);
        }
    }
}
