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
    public sealed partial class mishi : UserControl
    {

        //密匙改变事件
        public event PropertyChangedEventHandler mishi_changed;
        //密匙位数改变事件
        public event PropertyChangedEventHandler mishi_weishu_changed;
        //是否适用默认密匙改变事件
        public event PropertyChangedEventHandler shifou_moren_changed;

        //密匙位数列表
        ObservableCollection<int> weishu_1 { get; set; } = new ObservableCollection<int>();
        //是否使用默认
        public bool? shifou
        {
            get
            {
                return mishi_.IsChecked;
            }
            set
            {
                mishi_.IsChecked = value;
            }
        }
        //密匙
        public string mishi_huoqu
        {
            get
            {
                return mishi_shuru.Text;
            }

            set
            {
                mishi_shuru.Text = value;
            }
        }

        //内部密匙位数绑定
        private string mishi_weishu_n = "";
        private string Mishi_weishi_n
        {
            get
            {
                return mishi_weishu_n;
            }
            set
            {
                mishi_weishu_n = value;
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("mishi");
                //"密匙位数"
                mishi_weishu_changed?.Invoke(this, new PropertyChangedEventArgs(resourceLoader.GetString("String1")));
            }
        }

        //密匙种类
        private string suangfa = "AES";
        public string Suanfa
        {
            get
            {
                return suangfa;
            }
            set
            {

                suangfa = value;
                Xiugaimishi(suangfa);
            }
        }
        //密匙位数获取
        public string Weishu_huoqu
        {
            get
            {
                return xianshi.Text;
            }
            set
            {
                xianshi.Text = value;
                //检查
                for(int i=0;i<weishu_1.Count;i++)
                {
                    if (weishu_1[i] == Convert.ToInt32(xianshi.Text))
                    {
                        return;
                    }
                }
                if (weishu_1.Count != 0)
                {
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("mishi");
                    //"无效的密匙位数"
                    throw new Exception(resourceLoader.GetString("String2"));
                }
            }
        }

        public mishi()
        {
            this.InitializeComponent();
            Loaded += Mishi_Loaded;
        }

        private void Mishi_Loaded(object sender, RoutedEventArgs e)
        {
            //mishi_.IsChecked = true;
        }

        private void mishi__Checked(object sender, RoutedEventArgs e)
        {
            textblock1.Visibility = Visibility.Collapsed;
            xianshi.Visibility = Visibility.Collapsed;
            tubiao7.Visibility = Visibility.Collapsed;
            xaunzhe.Visibility = Visibility.Collapsed;
            mishi_shuru.Visibility = Visibility.Collapsed;
            suiji.Visibility = Visibility.Collapsed;
            listview.Visibility = Visibility.Collapsed;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("mishi");
            //"默认"
            shifou_moren_changed?.Invoke(this, new PropertyChangedEventArgs(resourceLoader.GetString("String3")));

        }

        private void mishi__Unchecked(object sender, RoutedEventArgs e)
        {
            textblock1.Visibility = Visibility.Visible;
            xianshi.Visibility = Visibility.Visible;
            tubiao7.Visibility = Visibility.Visible;
            xaunzhe.Visibility = Visibility.Visible;
            mishi_shuru.Visibility = Visibility.Visible;
            suiji.Visibility = Visibility.Visible;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("mishi");
            //"默认"
            shifou_moren_changed?.Invoke(this, new PropertyChangedEventArgs(resourceLoader.GetString("String4")));
        }


        //密匙改变时修改相应位数
        private void Xiugaimishi(string mihsi_k)
        {
            //删除原有的项
            weishu_1.Clear();

            //匹配
            switch (suangfa)
            {
                case "AES":
                    //记录 
                    weishu_1.Add(128);
                    weishu_1.Add(192);
                    weishu_1.Add(256);
                    xianshi.Text = "128";
                    break;
                case "DES":
                    //记录 
                    weishu_1.Add(64);
                    xianshi.Text = "64";
                    break;
                case "RC2":
                    //记录
                    weishu_1.Add(128);
                    xianshi.Text = "128";
                    break;
                case "RC4":
                    //记录
                    weishu_1.Add(128);
                    xianshi.Text = "128";
                    break;
                case "3_DES":
                    //记录
                    weishu_1.Add(192);
                    xianshi.Text = "192";
                    break;
                default:
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("mishi");
                    //"无效的算法"
                    throw new Exception(resourceLoader.GetString("String5"));
            }
        }

        private void xaunzhe_Click(object sender, RoutedEventArgs e)
        {
            if(listview.Visibility==Visibility.Collapsed)
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
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("mishi");

            int xuanzhe = listview.SelectedIndex;
            //匹配
            switch (suangfa)
            {
                case "AES":
                    //记录 
                    switch(xuanzhe)
                    {
                        case -1:
                            xianshi.Text = "128";
                            break;
                        case 0:
                            xianshi.Text = "128";
                            break;
                        case 1:
                            xianshi.Text = "192";
                            break;
                        case 2:
                            xianshi.Text = "256";
                            break;
                        default:
                            //"无效的位数"
                            throw new Exception(resourceLoader.GetString("String6"));
                    }
                    break;
                case "DES":
                    //记录 
                    xianshi.Text = "64";
                    break;
                case "RC2":
                    //记录
                    xianshi.Text = "128";
                    break;
                case "RC4":
                    //记录
                    xianshi.Text = "128";
                    break;
                case "3_DES":
                    //记录
                    xianshi.Text = "192";
                    break;
                default:
                    //"无效的算法"
                    throw new Exception(resourceLoader.GetString("String7"));
            }
            listview.Visibility = Visibility.Collapsed;
        }

        private void suiji_Click(object sender, RoutedEventArgs e)
        {
            int linshi = Convert.ToInt32(xianshi.Text);

            //创建密匙
            string str = "AA";
            //初始化种子
            Random suijishu = new Random();
            while (str.Length < (linshi / 8))
            {
                int linshi_1 = suijishu.Next(0, 999999999);
                str += linshi_1.ToString();
            }
            mishi_shuru.Text = str;
        }

        private void mishi_shuru_TextChanged(object sender, TextChangedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("mishi");
            //"密匙"
            mishi_changed?.Invoke(this, new PropertyChangedEventArgs(resourceLoader.GetString("String8")));
        }
    }
}
