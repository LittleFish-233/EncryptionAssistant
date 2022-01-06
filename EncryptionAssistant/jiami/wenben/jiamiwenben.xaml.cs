using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace EncryptionAssistant.jiami.wenben
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class jiamiwenben : Page
    {

        public jiamiwenben()
        {
            this.InitializeComponent();
            Loaded += Jiamiwenben_Loaded;
            SizeChanged += Zhuye_SizeChanged;
        }

        private void Jiamiwenben_Loaded(object sender, RoutedEventArgs e)
        {
            
            //算法
            chuli_mishi.Suanfa = App.Huancun.jiami_wenben.suanfa;
            chuli_suan.Suanfa_huoqu = App.Huancun.jiami_wenben.suanfa;

            //密匙
            chuli_mishi.mishi_huoqu = App.Huancun.jiami_wenben.mishi;
            //位数
            chuli_mishi.Weishu_huoqu = App.Huancun.jiami_wenben.mishi_weishu;
            //密码
            chuli_mima.Mima = App.Huancun.jiami_wenben.mima_shuru;
            //密码状态
            chuli_mima.Mima_zhuangtai = App.Huancun.jiami_wenben.mima_zhuangtai;
            //文本
            textbox.Text = App.Huancun.jiami_wenben.wenben_shuru;
            //默认密匙
            chuli_mishi.shifou = App.Huancun.jiami_wenben.shifou_mishi;

            //加密结果
            if(App.Huancun.jiami_wenben.jieguo_jiami!="")
            {
                textbox2.Text = App.Huancun.jiami_wenben.jieguo_jiami;
                textbox2.Visibility = Visibility.Visible;
                textblock2.Visibility = Visibility.Visible;
                fuzhi.Visibility = Visibility.Visible;
            }

            //订阅事件
            chuli_mishi.mishi_changed += Chuli_mishi_mishi_changed;
            chuli_mishi.mishi_weishu_changed += Chuli_mishi_mishi_weishu_changed;
            chuli_suan.Suanfa_changed += Chuli_suan_Suanfa_changed;
            chuli_mishi.shifou_moren_changed += Chuli_mishi_shifou_moren_changed;
            chuli_mima.Mimazhuangtai_gaibian += Chuli_mima_Mimazhuangtai_gaibian;
            chuli_mima.PasswordChanged += Chuli_mima_PasswordChanged;
            chuli_mima.Cuowu += Chuli_mima_Cuowu;
        }
        private void Zhuye_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 610)
            {
                Grid.SetRow(chuli_li, 0);
                Grid.SetColumn(chuli_li, 0);
                Grid.SetColumnSpan(chuli_li, 2);

                Grid.SetRow(shuru_re, 1);
                Grid.SetColumnSpan(shuru_re, 2);

                shuru_re.Margin = new Thickness(0, 10, 0, 0);
                chuli_li.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                Grid.SetRow(chuli_li, 0);
                Grid.SetColumn(chuli_li, 1);
                Grid.SetColumnSpan(chuli_li, 1);

                Grid.SetRow(shuru_re, 0);
                Grid.SetColumnSpan(shuru_re, 1);

                shuru_re.Margin = new Thickness(0, 0, 20, 0);
                chuli_li.Margin = new Thickness(20, 0, 0, 0);
            }
        }
        private void Chuli_mima_Cuowu(string a, int xuhao)
        {
            switch (xuhao)
            {
                case 1:
                    App.Huancun.jiami.Kaishitishi(a, xuhao);
                    break;
                case 2:
                    App.Huancun.jiami.Kaishitishi(a, xuhao);
                    break;
                default:
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_wenben");
                    //"在类<tianxiecanshu>方法<Mima_shu_Cuowu>中发生异常<传递的参数无效<"
                    throw new Exception(resourceLoader.GetString("String1") + xuhao + ">>");
            }
        }

        private void Chuli_mima_PasswordChanged(string a, int xuhao)
        {
            App.Huancun.jiami_wenben.mima_shuru = chuli_mima.Mima;
        }

        private void Chuli_mima_Mimazhuangtai_gaibian(string a, int xuhao)
        {
            App.Huancun.jiami_wenben.mima_zhuangtai = chuli_mima.Mima_zhuangtai;
            if (App.Huancun.jiami_wenben.mima_zhuangtai == 2)
            {
                App.Huancun.jiami_wenben.mima_linshi = chuli_mima.Mima;
            }
            else
            {
                App.Huancun.jiami_wenben.mima_linshi = "";
            }
        }

        private void Chuli_mishi_shifou_moren_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            App.Huancun.jiami_wenben.shifou_mishi = chuli_mishi.shifou;
        }

        private void Chuli_suan_Suanfa_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            chuli_mishi.Suanfa = chuli_suan.Suanfa_huoqu;
            App.Huancun.jiami_wenben.suanfa = chuli_suan.Suanfa_huoqu;
        }

        private void Chuli_mishi_mishi_weishu_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            App.Huancun.jiami_wenben.mishi_weishu = chuli_mishi.Weishu_huoqu;
        }

        private void Chuli_mishi_mishi_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            App.Huancun.jiami_wenben.mishi = chuli_mishi.mishi_huoqu;
        }

        private async void Zhantie_ClickAsync(object sender, RoutedEventArgs e)
        {
            //获取数据
            DataPackageView con = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
            string str = string.Empty;
            if (con.Contains(StandardDataFormats.Text))
            {
                str = await con.GetTextAsync();
            }
            //复制数据
            textbox.Text = str;
            //提示
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_wenben");
            //"粘贴成功"
            App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String2"), 2);
        }

        private void jiami_wen_an_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_wenben");
            try
            {

                //检查密匙
                string jieguo = "";
                string mishi_linshi = chuli_mishi.mishi_huoqu;

                if (App.Huancun.jiami_wenben.shifou_mishi == true)
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
                chuli_mishi.mishi_huoqu = mishi_linshi;

                //检查密码
                //检查密码
                string mima_linshi = "";
                if (App.Huancun.jiami_wenben.mima_zhuangtai == 2 || (App.Huancun.jiami_wenben.mima_zhuangtai == 0 && App.Huancun.jiami_wenben.mima_shuru == ""))
                {
                    if (App.Huancun.jiami_wenben.mima_zhuangtai == 2)
                    {
                        mima_linshi = App.Huancun.jiami_wenben.mima_linshi;

                    }
                    else
                    {
                        mima_linshi = "";
                    }
                }
                else
                {
                    

                    if (App.Huancun.jiami_wenben.mima_zhuangtai == 0)
                    {
                        //"请单击 < 使用 > 按钮以确认密码无误"
                        App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String3"), 2);
                    }
                    else
                    {
                        //"请先设置密码,再进行加密"
                        App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String4"), 1);

                    }
                    return;
                }

                //初始化算法
                SymmetricKeyAlgorithmProvider syprd = null;
                switch (App.Huancun.jiami_wenben.suanfa)
                {
                    case "AES"://默认算法
                        syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
                        break;
                    case "DES":
                        syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.DesEcbPkcs7);
                        break;
                    case "RC2":
                        syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc2EcbPkcs7);
                        break;
                    case "RC4":
                        syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc4);
                        break;
                    case "3_DES":
                        syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.TripleDesEcbPkcs7);
                        break;
                }
                //直接加密文本和密码
                string miwen = daima.Jiami_jiemi.Jiami(mima_linshi + textbox.Text, mishi_linshi, syprd);
                //判断
                if (miwen == null)
                {
                    //"无法加密这个文本，请重试"
                    App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String5"), 1);
                    return;
                }
                else
                {
                    //处理
                    miwen = "##" + (textbox.Text + mima_linshi).Length.ToString() + "A" + mima_linshi.Length.ToString() + "##" + miwen;
                }
                //显示
                textbox2.Visibility = Visibility.Visible;
                textblock2.Visibility = Visibility.Visible;
                fuzhi.Visibility = Visibility.Visible;
                textbox2.Text = miwen;

            }
            catch (Exception exc)
            {
                App.Huancun.jiami.Kaishitishi(exc.Message, 1);
            }
        }

        private void textbox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.Huancun.jiami_wenben.jieguo_jiami = textbox2.Text;
        }

        private void fuzhi_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dp = new DataPackage();
            dp.SetText(textbox2.Text);
            Clipboard.SetContent(dp);
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_wenben");
            //"复制成功"
            App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String6"), 2);
        }

        private void textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.Huancun.jiami_wenben.wenben_shuru = textbox.Text;
        }
    }
}
