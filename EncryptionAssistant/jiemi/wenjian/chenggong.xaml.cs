using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace EncryptionAssistant.jiemi.wenjian
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class chenggong : Page
    {
        public chenggong()
        {
            this.InitializeComponent();
            Loaded += Chenggong_Loaded;
        }

        private void Chenggong_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFilePath(App.Huancun.jiemi_wenjian.baocun_dizhi);
            //记录
            App.Huancun.jiemi_wenjian.yeshu = 7;
            //解密文件总数
            textblock3.Text = App.Huancun.jiemi_wenjian.jiemi_jingdu.wenjianshu_zhong.ToString();
            //解密文件总共大小
            textblock5.Text = daima.Gongju.zhanyongkongjian(App.Huancun.jiemi_wenjian.jiemi_jingdu.zijie_zong);
            //已用时间
            textblock7.Text = daima.Gongju.shijianzhuanghuan((ulong)App.Huancun.jiemi_wenjian.shijian);
            //平均速度
            textblock9.Text= daima.Gongju.zhanyongkongjian((ulong)(App.Huancun.jiemi_wenjian.jiemi_jingdu.zijie_zong/ (double)App.Huancun.jiemi_wenjian.shijian))+"/S";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.Huancun.jiemi_wenjian.qingli();
            Frame.Navigate(typeof(tianjiamubiao));
        }

        private async void OpenFilePath(StorageFolder folder)
        {
            var t = new FolderLauncherOptions();
            await Launcher.LaunchFolderAsync(folder, t);
        }
    }
}
