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

namespace EncryptionAssistant.jiami.wenjian
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class tianxiecanshu : Page
    {
        //密码错误次数
        private static int cuowu_cishu = 0;

        public tianxiecanshu()
        {
            this.InitializeComponent();
            Loaded += Tianxiecanshu_Loaded;
        }

        private void Tianxiecanshu_Loaded(object sender, RoutedEventArgs e)
        {
            gaibiandaxiao((int)(ActualWidth - 100) / 4, 60);
            SizeChanged += Jixutianjia_SizeChanged;
            //记录
            App.Huancun.jiami_wenjian.yeshu = 2;

            //密码状态
            mima_shu.Mima_zhuangtai = App.Huancun.jiami_wenjian.mima_zhuangtai;
            //算法
            chuli_mishi.Suanfa = App.Huancun.jiami_wenjian.suanfa;
            chuli_suan.Suanfa_huoqu = App.Huancun.jiami_wenjian.suanfa;

            //密匙
            chuli_mishi.mishi_huoqu = App.Huancun.jiami_wenjian.mishi;
            
            //位数
            chuli_mishi.Weishu_huoqu = App.Huancun.jiami_wenjian.mishi_weishu;
            //密码
            mima_shu.Mima = App.Huancun.jiami_wenjian.mima_shuru;
            //默认密匙
            chuli_mishi.shifou = App.Huancun.jiami_wenjian.shifou_mishi;

            //高级
            if (App.Huancun.jiami_wenjian.xianshigaoji==true)
            {
                tishi.Visibility = Visibility.Visible;
                jilu.Visibility = Visibility.Visible;
                suanchong.Visibility = Visibility.Visible;
                textblock14.Visibility = Visibility.Visible;
                textblock18.Visibility = Visibility.Collapsed;
            }
            else
            {
                tishi.Visibility = Visibility.Collapsed;
                jilu.Visibility = Visibility.Collapsed;
                suanchong.Visibility = Visibility.Collapsed;
                textblock18.Visibility = Visibility.Visible;
                textblock14.Visibility = Visibility.Collapsed;
            }
            //选项
            tishi.IsChecked = App.Huancun.jiami_wenjian.tishijiami;
            jilu.IsChecked = App.Huancun.jiami_wenjian.jilumishi;
            suanchong.IsChecked = App.Huancun.jiami_wenjian.suanchongjiami;
            sanchu.IsChecked = App.Huancun.jiami_wenjian.shanchuyuanwenjian;

            //订阅事件
            chuli_mishi.mishi_changed += Chuli_mishi_mishi_changed;
            chuli_mishi.mishi_weishu_changed += Chuli_mishi_mishi_weishu_changed;
            chuli_suan.Suanfa_changed += Chuli_suan_Suanfa_changed;
            chuli_mishi.shifou_moren_changed += Chuli_mishi_shifou_moren_changed;
            mima_shu.PasswordChanged += Mima_shu_PasswordChanged;
            mima_shu.Cuowu += Mima_shu_Cuowu;
            mima_shu.Mimazhuangtai_gaibian += Mima_shu_Mimazhuangtai_gaibian;

        }

        private void Mima_shu_Mimazhuangtai_gaibian(string a, int xuhao)
        {
            App.Huancun.jiami_wenjian.mima_zhuangtai = mima_shu.Mima_zhuangtai;
            if(App.Huancun.jiami_wenjian.mima_zhuangtai==2)
            {
                App.Huancun.jiami_wenjian.mima_linshi = mima_shu.Mima;
            }
            else
            {
                App.Huancun.jiami_wenjian.mima_linshi = "";
            }
        }

        private void Mima_shu_Cuowu(string a, int xuhao)
        {
            switch(xuhao)
            {
                case 1:
                    App.Huancun.jiami.Kaishitishi(a, xuhao);
                    break;
                case 2:
                    App.Huancun.jiami.Kaishitishi(a, xuhao);
                    break;
                default:
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_tianxiecanshu");
                    //"在类<tianxiecanshu>方法<Mima_shu_Cuowu>中发生异常<传递的参数无效<" 
                    throw new Exception(resourceLoader.GetString("String1") + xuhao + ">>");
            }
        }

        private void Mima_shu_PasswordChanged(string a, int xuhao)
        {
             App.Huancun.jiami_wenjian.mima_shuru = mima_shu.Mima;
        }

        private void Chuli_mishi_shifou_moren_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            App.Huancun.jiami_wenjian.shifou_mishi = chuli_mishi.shifou;
        }

        private void Chuli_suan_Suanfa_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            chuli_mishi.Suanfa = chuli_suan.Suanfa_huoqu;
            App.Huancun.jiami_wenjian.suanfa = chuli_suan.Suanfa_huoqu;
        }

        private void Chuli_mishi_mishi_weishu_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            App.Huancun.jiami_wenjian.mishi_weishu = chuli_mishi.Weishu_huoqu;
        }

        private void Chuli_mishi_mishi_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            App.Huancun.jiami_wenjian.mishi = chuli_mishi.mishi_huoqu;
        }



        private void Jixutianjia_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int gao = 60;
            int kuang = ((int)e.NewSize.Width - 100) / 4;
            gaibiandaxiao(kuang, gao);
        }

        private void gaibiandaxiao(int kuang, int gao)
        {
            PointCollection zuobiao = new PointCollection();

            //1
            zuobiao.Add(new Point(20, gao));
            zuobiao.Add(new Point(20, gao + 10));
            zuobiao.Add(new Point(20 + kuang, gao + 10));
            zuobiao.Add(new Point(20 + kuang - 10, gao));

            juxing1.Points = zuobiao;
            //2
            zuobiao = new PointCollection();
            zuobiao.Add(new Point(20 + kuang + 20, gao));
            zuobiao.Add(new Point(20 + kuang + 20 + 10, gao + 10));
            zuobiao.Add(new Point(20 + kuang + 20 + kuang, gao + 10));
            zuobiao.Add(new Point(20 + kuang + 20 + kuang - 10, gao));

            juxing2.Points = zuobiao;
            //3
            zuobiao = new PointCollection();
            zuobiao.Add(new Point(20 + kuang * 2 + 20 * 2, gao));
            zuobiao.Add(new Point(20 + kuang * 2 + 20 * 2 + 10, gao + 10));
            zuobiao.Add(new Point(20 + kuang * 2 + 20 * 2 + kuang, gao + 10));
            zuobiao.Add(new Point(20 + kuang * 2 + 20 * 2 + kuang - 10, gao));
            juxing3.Points = zuobiao;
            //4
            zuobiao = new PointCollection();
            zuobiao.Add(new Point(20 + kuang * 3 + 20 * 3, gao));
            zuobiao.Add(new Point(20 + kuang * 3 + 20 * 3 + 10, gao + 10));
            zuobiao.Add(new Point(20 + kuang * 3 + 20 * 3 + kuang, gao + 10));
            zuobiao.Add(new Point(20 + kuang * 3 + 20 * 3 + kuang, gao));

            juxing4.Points = zuobiao;
        }

        private void gaoji_Click(object sender, RoutedEventArgs e)
        {
            if(textblock14.Visibility==Visibility.Collapsed)
            {
                tishi.Visibility = Visibility.Visible;
                jilu.Visibility = Visibility.Visible;
                suanchong.Visibility = Visibility.Visible;
                sanchu.Visibility = Visibility.Visible;
                textblock14.Visibility = Visibility.Visible;
                textblock18.Visibility = Visibility.Collapsed;
                App.Huancun.jiami_wenjian.xianshigaoji = true;
            }
            else
            {
                tishi.Visibility = Visibility.Collapsed;
                jilu.Visibility = Visibility.Collapsed;
                suanchong.Visibility = Visibility.Collapsed;
                sanchu.Visibility = Visibility.Collapsed;
                textblock18.Visibility = Visibility.Visible;
                textblock14.Visibility = Visibility.Collapsed;
                App.Huancun.jiami_wenjian.xianshigaoji = false ;
            }
        }

        private void suanchong_Checked(object sender, RoutedEventArgs e)
        {
            App.Huancun.jiami_wenjian.suanchongjiami = true;
        }

        private void suanchong_Unchecked(object sender, RoutedEventArgs e)
        {
            App.Huancun.jiami_wenjian.suanchongjiami = false;
        }

        private void jilu_Checked(object sender, RoutedEventArgs e)
        {
            App.Huancun.jiami_wenjian.jilumishi = true;
        }

        private void jilu_Unchecked(object sender, RoutedEventArgs e)
        {
            App.Huancun.jiami_wenjian.jilumishi = false;
        }

        private void shangyibu_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(jixutianjia));
        }

        private void xiayibu_Click(object sender, RoutedEventArgs e)
        {
            //检查密码
            if (App.Huancun.jiami_wenjian.mima_zhuangtai == 2 || (App.Huancun.jiami_wenjian.mima_zhuangtai == 0 && App.Huancun.jiami_wenjian.mima_shuru == ""&&App.Huancun.jiami_wenjian.mima_linshi==""))
            {
                
            }
            else
            {
                if (App.Huancun.jiami_wenjian.mima_zhuangtai == 0)
                {
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_tianxiecanshu");
                    //"请单击 < 使用 > 按钮以确认密码无误"
                    App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String2"), 2);
                }
                else
                {
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_tianxiecanshu");
                    //"请先设置密码,再进行加密"
                    App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String3"), 1);

                }
                return;
            }
            //检查密匙
            string jieguo = "";
            string mishi_linshi = chuli_mishi.mishi_huoqu;

            if (App.Huancun.jiami_wenjian.shifou_mishi == true)
            {
                mishi_linshi = "15236784515632154896321548963245125";
            }
            if (mishi_linshi.Length != Convert.ToInt32(chuli_mishi.Weishu_huoqu) / 8)
            {
                for (int i = 0; i < Convert.ToInt32(chuli_mishi.Weishu_huoqu) / 8; i++)
                {
                    if (i < mishi_linshi.Length)
                        jieguo += (char)mishi_linshi[i];
                    else
                        jieguo += '\0';
                }
                mishi_linshi = jieguo;
            }
            App.Huancun.jiami_wenjian.mishi = jieguo;
            Frame.Navigate(typeof(baocun));
        }

        private void tishi_Checked(object sender, RoutedEventArgs e)
        {
            App.Huancun.jiami_wenjian.tishijiami = true;
        }

        private void tishi_Unchecked(object sender, RoutedEventArgs e)
        {
            App.Huancun.jiami_wenjian.tishijiami = false;
        }

        private void sanchu_Checked(object sender, RoutedEventArgs e)
        {
            App.Huancun.jiami_wenjian.shanchuyuanwenjian = true;
        }

        private void sanchu_Unchecked(object sender, RoutedEventArgs e)
        {
            App.Huancun.jiami_wenjian.shanchuyuanwenjian = false;
        }
    }
}
