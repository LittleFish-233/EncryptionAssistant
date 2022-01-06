using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography.Core;
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

namespace EncryptionAssistant.zhuye
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class zhuye : Page
    {

        private DispatcherTimer timer = new DispatcherTimer();
        
        public zhuye()
        {
            this.InitializeComponent();

            tishi_zuida.Visibility = Visibility.Collapsed;

            SizeChanged += Zhuye_SizeChanged;

            timer.Tick += Timer_Tick;
        }

    

        private void Zhuye_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 680)
            {
                Grid.SetRow(jie_zuida, 1);
                Grid.SetColumn(jie_zuida, 0);
                Grid.SetColumnSpan(jie_zuida, 2);
                Grid.SetColumnSpan(jiami_zuida, 2);
                dierhang.Height = new GridLength(500);
                jie_zuida.Margin = new Thickness(50, 0, 0, 0);
                jiami_zuida.Margin = new Thickness(0, 0, 50, 0);
                tishi.Margin = new Thickness(50, 50, 50, 50);
            }
            else
            {
                Grid.SetRow(jie_zuida, 0);
                Grid.SetColumn(jie_zuida, 1);
                Grid.SetColumnSpan(jie_zuida, 1);
                Grid.SetColumnSpan(jiami_zuida, 1);
                dierhang.Height = new GridLength(0);
                jie_zuida.Margin = new Thickness(0, 0, 0, 0);
                jiami_zuida.Margin = new Thickness(0, 0, 0, 0);
                tishi.Margin = new Thickness(70, 50, 50, 50);
            }
        }
        private void Zhuye_Xianshitishi(string a, int xuhao)
        {
            // 显示提示
            textblock14.Text = a;
            tishi_zuida.Visibility = Visibility.Visible;
            //匹配图标
            switch (xuhao)
            {
                case 1:
                    tubiao12.Visibility = Visibility.Visible;
                    tubiao13.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    tubiao13.Visibility = Visibility.Visible;
                    tubiao12.Visibility = Visibility.Collapsed;
                    break;

            }

         
            
            //设置timer
            timer.Interval = new TimeSpan(0,0,5);
            //设置是否重复计时，如果该属性设为False,则只执行timer_Elapsed方法一次。
            //timer.Tick += Timer_Tick;
            //设置timer可用
            timer.Start();
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

        private void Banzhu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(banzhu.wenzhi));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(xiazai.xiazai_zhu));
        }

        private void Shengcheng_jia_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("zhuye");
            //"重置"
            if ((string)shengcheng_jia.Content == resourceLoader.GetString("String"))//是否处于显示加密结果状态
            {
                wenben_jiami.Text = "";
                shengcheng_jia.Content = "生成";
                return;
            }
            try
            {

                //检查密匙
                string mishi_linshi = "15236784515632154896321548963245125";
                string jieguo = "";
                if (mishi_linshi.Length != 128 / 8)
                {
                    for (int i = 0; i < 128 / 8; i++)
                    {
                        if (i < mishi_linshi.Length)
                            jieguo += (char)mishi_linshi[i];
                        else
                            jieguo += '\0';
                    }
                    mishi_linshi = jieguo;
                }
                //初始化算法
                //默认算法
                var syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
                //直接加密文本和密码
                string miwen = daima.Jiami_jiemi.Jiami(wenben_jiami.Text, mishi_linshi, syprd);
                //判断
                if (miwen == null)
                {
                    //"无法加密这个文本，请重试"
                    Zhuye_Xianshitishi(resourceLoader.GetString("String2"), 1);
                    return;
                }
                else
                {
                    //处理
                    miwen = "##" + (wenben_jiami.Text).Length.ToString() + "A" + "0" + "##" + miwen;
                }
                //显示
                //wenben_jiami.Text = miwen;
                //"重置"
                //shengcheng_jia.Content = resourceLoader.GetString("String3");

                //将结果复制到剪切板 并提示
                CopyText(miwen);
                Zhuye_Xianshitishi("已复制结果：" + miwen, 2);
            }
            catch (Exception exc)
            {
                Zhuye_Xianshitishi(exc.Message, 1);
            }
        }
        private void CopyText(string str)
        {
            DataPackage dp = new DataPackage();
            dp.SetText(str);
            Clipboard.SetContent(dp);
        }

        private void Shengcheng_jie_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("zhuye");
            //"重置"
            if ((string)shengcheng_jie.Content == resourceLoader.GetString("String4"))//是否处于显示加密结果状态
            {
                wenben_jiemi.Text = "";
                shengcheng_jie.Content = "生成";
                return;
            }
            try
            {
                //检查密匙
                string jieguo = "";
                string mishi_linshi = "15236784515632154896321548963245125";
                if (mishi_linshi.Length != 128 / 8)
                {
                    for (int i = 0; i < 128 / 8; i++)
                    {
                        if (i < mishi_linshi.Length)
                            jieguo += (char)mishi_linshi[i];
                        else
                            jieguo += '\0';
                    }
                    mishi_linshi = jieguo;
                }
                //检查密码
                string mima_linshi = "";
                if(string.IsNullOrEmpty( wenben_jiemi.Text))
                {
                    Zhuye_Xianshitishi("Input is required", 1);
                    return;
                }
                //检查输入版本
                if (wenben_jiemi.Text[0] != '#')
                {
                    //转到旧版本        "你输入的文本不符合新版格式，若要解密旧版文本，请访问存档"
                    Zhuye_Xianshitishi(resourceLoader.GetString("String5"), 1);
                    return;
                }
                //初始化算法
                //默认算法
                var syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
                //分离附加值
                string fujia = "";
                string linshi = wenben_jiemi.Text;
                string miwen = "";
                int jishu = 0;
                for (int i = 0; i < linshi.Length; i++)
                {
                    if (linshi[i] == '#')
                    {
                        jishu++;
                        fujia += linshi[i];
                        continue;
                    }
                    if (jishu < 4)
                    {
                        fujia += linshi[i];
                    }
                    else
                    {
                        miwen += linshi[i];
                    }
                }
                //解密
                string mingwen = daima.Jiami_jiemi.Jiemi(miwen, mishi_linshi, syprd);
                //判断
                if (mingwen == null)
                {
                    //"无法解密此文本，请确保解密密匙和算法正确"
                    App.Huancun.jiemi.Kaishitishi(resourceLoader.GetString("String6"), 1);
                    return;
                }
                //获取位数
                string weishu_wen = "";
                string weishu_ma = "";
                bool kaishi = false;
                for (int i = 0; i < fujia.Length; i++)
                {
                    if (fujia[i] == '#')
                    {
                        continue;
                    }
                    if (fujia[i] == 'A')
                    {
                        kaishi = true;
                        continue;
                    }
                    if (kaishi == false)
                    {
                        weishu_wen += fujia[i];
                    }
                    else
                    {
                        weishu_ma += fujia[i];
                    }
                }
                int wei_wen = Convert.ToInt32(weishu_wen);
                int wei_ma = Convert.ToInt32(weishu_ma);
                //分离密码
                string mima_j = "";
                for (int i = 0; i < wei_ma; i++)
                {
                    mima_j += mingwen[i];
                }
                //对比密码
                if (mima_linshi != mima_j)
                {
                    //"密码错误，请确认后重试"
                    App.Huancun.jiemi.Kaishitishi(resourceLoader.GetString("String7"), 1);
                    return;
                }
                //分离文本
                string wenben_j = "";
                for (int i = wei_ma; i < wei_wen; i++)
                {
                    wenben_j += mingwen[i];
                }
                //显示
                //wenben_jiemi.Text = wenben_j;
                //"重置"
                //shengcheng_jie.Content = resourceLoader.GetString("String7");
                //将结果复制到剪切板 并提示
                CopyText(wenben_j);
                Zhuye_Xianshitishi("已复制结果：" + wenben_j, 2);

            }
            catch (Exception exc)
            {
                Zhuye_Xianshitishi(exc.Message, 1);
            }
        }
    }
}
