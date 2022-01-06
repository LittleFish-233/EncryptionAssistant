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

namespace EncryptionAssistant.xiazai
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class zhengzai_xiazai : Page
    {
        public zhengzai_xiazai()
        {
            Loaded += Zhengzai_xiazai_Loaded;
            this.InitializeComponent();
            
        }

        private void Zhengzai_xiazai_Loaded(object sender, RoutedEventArgs e)
        {
            listview.ItemsSource = App.Huancun.xiazai.xiazai_guanli.zhengzai_list;
        }

        private void Xinjian_Click(object sender, RoutedEventArgs e)
        {
            xinjian_xiazai.Visibility = Visibility.Visible;
        }

        private void Sanchu_Click(object sender, RoutedEventArgs e)
        {
            daima.Yuan_xiazai linshi = (daima.Yuan_xiazai)(sender as Button).DataContext;
            App.Huancun.xiazai.xiazai_guanli.shanchurenwuAsync(linshi);
        }
        private void Kaishi_Click(object sender, RoutedEventArgs e)
        {
            daima.Yuan_xiazai linshi = (daima.Yuan_xiazai)(sender as Button).DataContext;
            linshi.star();

        }

        private void Zanting_Click(object sender, RoutedEventArgs e)
        {
            daima.Yuan_xiazai linshi = (daima.Yuan_xiazai)(sender as Button).DataContext;
            linshi.stop();
        }

        private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
