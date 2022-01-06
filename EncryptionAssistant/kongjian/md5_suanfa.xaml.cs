using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace EncryptionAssistant.kongjian
{
    public sealed partial class md5_suanfa: UserControl
    {
        //算法列表
        ObservableCollection<string> suanfa_1 { get; set; } = new ObservableCollection<string>();
        //算法改变事件
        public event PropertyChangedEventHandler Suanfa_changed;

        //算法获取
        private string suanfa_huoqu = "Md5";
        public string Suanfa_huoqu
        {
            get
            {
                return suanfa_huoqu;
            }
            set
            {
                suanfa_huoqu = value;
                puzhu(true);
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("md5_suanfa");
                //算法
                Suanfa_changed?.Invoke(this, new PropertyChangedEventArgs(resourceLoader.GetString("String1")));

            }
        }
        //true 变量赋值给文本框 
        private void puzhu(bool v)
        {
            if (v == true)
            {
                if (xianshi.Text != suanfa_huoqu)
                {
                    xianshi.Text = Suanfa_huoqu;
                }
            }
            else
            {
                if (xianshi.Text != suanfa_huoqu)
                {
                    Suanfa_huoqu = xianshi.Text;
                }
            }
        }

        public md5_suanfa()
        {
            this.InitializeComponent();
            Loaded += Suanfa_Loaded;
        }

        private void Suanfa_Loaded(object sender, RoutedEventArgs e)
        {
            //删除原有的项
            suanfa_1.Clear();
            //添加算法
            suanfa_1.Add("Md5");
            suanfa_1.Add("Sha1");
            suanfa_1.Add("Sha256");
            suanfa_1.Add("Sha384");
            suanfa_1.Add("Sha512");
        }

        private void xaunzhe_Click(object sender, RoutedEventArgs e)
        {
            if (listview.Visibility == Visibility.Collapsed)
            {
                listview.Visibility = Visibility.Visible;
            }
            else
            {
                listview.Visibility = Visibility.Collapsed;
            }
        }

        private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            xianshi.Text = suanfa_1[listview.SelectedIndex].ToString();
            listview.Visibility = Visibility.Collapsed;
        }
    }

}
