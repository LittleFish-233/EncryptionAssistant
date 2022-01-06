using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace EncryptionAssistant.jiami
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class jia_zhuye : Page
    {
        private DispatcherTimer timer = new DispatcherTimer();

        public jia_zhuye()
        {
            this.InitializeComponent();
            Loaded += Jia_zhuye_Loaded;
        }

        private void Jia_zhuye_Loaded(object sender, RoutedEventArgs e)
        {
            wenben.Navigate(typeof(wenben.jiamiwenben));
            switch (App.Huancun.jiami_wenjian.yeshu)
            {
                case 0:
                    wenjian.Navigate(typeof(wenjian.tianjiamubiao));
                    break;
                case 1:
                    wenjian.Navigate(typeof(wenjian.jixutianjia));
                    break;
                case 2:
                    wenjian.Navigate(typeof(wenjian.tianxiecanshu));
                    break;
                case 3:
                    wenjian.Navigate(typeof(wenjian.baocun));
                    break;
                case 4:
                    wenjian.Navigate(typeof(wenjian.queren));
                    break;
                case 5:
                    wenjian.Navigate(typeof(wenjian.zhengzaijiami));
                    break;
                case 6:
                    wenjian.Navigate(typeof(wenjian.chenggong));
                    break;
                case 7:
                    wenjian.Navigate(typeof(wenjian.shibai));
                    break;
            }
            try
            {
                App.Huancun.jiami.Xianshitishi -= Jiami_Xianshitishi;
            }
            catch
            {

            }
            App.Huancun.jiami.Xianshitishi += Jiami_Xianshitishi;
        }

        private void Jiami_Xianshitishi(string a,int xuhao)
        {
            // 显示提示
            textblock2.Text = a;
            tishi_zuida.Visibility = Visibility.Visible;
            //匹配图标
            switch(xuhao)
            {
                case 1:
                    tubiao1.Visibility = Visibility.Visible;
                    tubiao3.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    tubiao3.Visibility = Visibility.Visible;
                    tubiao1.Visibility = Visibility.Collapsed;
                    break;

            }

            //设置timer可用
            timer.Start();

            //设置timer
            timer.Interval = new TimeSpan(0,0,5);
            //设置是否重复计时，如果该属性设为False,则只执行timer_Elapsed方法一次。
            timer.Tick += Timer_Tick;
        }

        private async void Timer_Tick(object sender, object e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { action(); });
        }

        private void rootPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tishi_zuida.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tishi_zuida.Visibility = Visibility.Collapsed;
        }

        private void action()
        {
            tishi_zuida.Visibility = Visibility.Collapsed;
            timer.Stop();

        }
    }
}
