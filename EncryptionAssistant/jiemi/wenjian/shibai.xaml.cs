using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class shibai : Page
    {
        public shibai()
        {
            this.InitializeComponent();
            Loaded += Shibai_Loaded;
        }

        private void Shibai_Loaded(object sender, RoutedEventArgs e)
        {

            listview.ItemsSource = App.Huancun.jiemi_wenjian.jiemi_jingdu.cuowuliebiao;
            //记录
            App.Huancun.jiemi_wenjian.yeshu = 7;
            //文件总数
            textblock3.Text = App.Huancun.jiemi_wenjian.jiemi_jingdu.wenjianshu_zhong.ToString();
            //无法解密的文件总数
            textblock5.Text = App.Huancun.jiemi_wenjian.jiemi_jingdu.cuowuliebiao.Count.ToString();
            //已用时间
            textblock7.Text = daima.Gongju.shijianzhuanghuan((ulong)App.Huancun.jiemi_wenjian.shijian);

        }

        private void xiayibu_Click(object sender, RoutedEventArgs e)
        {
            App.Huancun.jiemi_wenjian.qingli();
            this.Frame.Navigate(typeof( tianjiamubiao));
        }
    }
}
