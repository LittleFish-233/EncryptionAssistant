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
using EncryptionAssistant.kongjian;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace EncryptionAssistant.jiemi.wenben
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class jiemiwenben : Page
    {
        //密码错误次数
        private static int cuowu_cishu = 0;

        public jiemiwenben()
        {
            this.InitializeComponent();
            Loaded += Jiemiwenben_Loaded;
            SizeChanged += Zhuye_SizeChanged;
        }

        private void Jiemiwenben_Loaded(object sender, RoutedEventArgs e)
        {
            //密码
            chuli_mima.Mima = App.Huancun.jiemi_wenben.mima_shuru;
            //密码状态
            chuli_mima.Mima_zhuangtai = App.Huancun.jiemi_wenben.mima_zhuangtai;
            //算法
            chuli_mishi.Suanfa = App.Huancun.jiemi_wenben.suanfa;
            chuli_suan.Suanfa_huoqu = App.Huancun.jiemi_wenben.suanfa;

            //密匙
            chuli_mishi.mishi_huoqu = App.Huancun.jiemi_wenben.mishi;
            //位数
            chuli_mishi.Weishu_huoqu = App.Huancun.jiemi_wenben.mishi_weishu;
            
            //文本
            textbox.Text = App.Huancun.jiemi_wenben.wenben_shuru;
            //加密结果
            if (App.Huancun.jiemi_wenben.jieguo_jiemi != "")
            {
                textbox2.Text = App.Huancun.jiemi_wenben.jieguo_jiemi;
                textbox2.Visibility = Visibility.Visible;
                textblock2.Visibility = Visibility.Visible;
                fuzhi.Visibility = Visibility.Visible;
            }
            //默认密匙
            chuli_mishi.shifou = App.Huancun.jiemi_wenben.shifou_mishi;

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
                    App.Huancun.jiemi.Kaishitishi(a, xuhao);
                    break;
                case 2:
                    App.Huancun.jiemi.Kaishitishi(a, xuhao);
                    break;
                default:
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jie_wenben");
                    //"在类<tianxiecanshu>方法<Mima_shu_Cuowu>中发生异常<传递的参数无效<"
                    throw new Exception(resourceLoader.GetString("String1") + xuhao + ">>");
            }
        }

        private void Chuli_mima_PasswordChanged(string a, int xuhao)
        {
            App.Huancun.jiemi_wenben.mima_shuru =chuli_mima.Mima;
        }

        private void Chuli_mima_Mimazhuangtai_gaibian(string a, int xuhao)
        {
            App.Huancun.jiemi_wenben.mima_zhuangtai = chuli_mima.Mima_zhuangtai;
            if (App.Huancun.jiemi_wenben.mima_zhuangtai == 2)
            {
                App.Huancun.jiemi_wenben.mima_linshi = chuli_mima.Mima;
            }
            else
            {
                App.Huancun.jiemi_wenben.mima_linshi = "";
            }
        }

        private void Chuli_mishi_shifou_moren_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            App.Huancun.jiemi_wenben.shifou_mishi = chuli_mishi.shifou;
        }

        private void Chuli_suan_Suanfa_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            chuli_mishi.Suanfa = chuli_suan.Suanfa_huoqu;
            App.Huancun.jiemi_wenben.suanfa = chuli_suan.Suanfa_huoqu;
        }

        private void Chuli_mishi_mishi_weishu_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            App.Huancun.jiemi_wenben.mishi_weishu = chuli_mishi.Weishu_huoqu;
        }

        private void Chuli_mishi_mishi_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            App.Huancun.jiemi_wenben.mishi = chuli_mishi.mishi_huoqu;
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
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jie_wenben");
            //"粘贴成功"
            App.Huancun.jiemi.Kaishitishi(resourceLoader.GetString("String2"), 2);
        }

        private void jiami_wen_an_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jie_wenben");

            try
            {

                //检查密匙
                string jieguo="";
                string mishi_linshi= chuli_mishi.mishi_huoqu;

                if (App.Huancun.jiemi_wenben.shifou_mishi == true)
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
                string mima_linshi = "";
                if (App.Huancun.jiemi_wenben.mima_zhuangtai == 2 || (App.Huancun.jiemi_wenben.mima_zhuangtai == 0 && App.Huancun.jiemi_wenben.mima_shuru == ""))
                {
                    if (App.Huancun.jiemi_wenben.mima_zhuangtai == 2)
                    {
                        mima_linshi = App.Huancun.jiemi_wenben.mima_linshi;

                    }
                    else
                    {
                        mima_linshi = "";
                    }
                }
                else
                {
                    if (App.Huancun.jiemi_wenben.mima_zhuangtai == 0)
                    {
                        //"请单击 < 使用 > 按钮以确认密码无误"
                        App.Huancun.jiemi.Kaishitishi(resourceLoader.GetString("String3"), 2);
                    }
                    else
                    {
                        //"请先设置密码,再进行加密"
                        App.Huancun.jiemi.Kaishitishi(resourceLoader.GetString("String4"), 1);

                    }
                    return;
                }

                //检查输入版本
                if (textbox.Text[0] != '#')
                {

                    //转到旧版本"你输入的文本不符合新版格式，若要解密旧版文本，请访问存档"
                    App.Huancun.jiemi.Kaishitishi(resourceLoader.GetString("String5"), 1);
                    return;
                }
                //初始化算法
                SymmetricKeyAlgorithmProvider syprd = null;
                switch (chuli_suan.Suanfa_huoqu)
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
                //分离附加值
                string fujia = "";
                string linshi = textbox.Text;
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
                textbox2.Visibility = Visibility.Visible;
                textblock2.Visibility = Visibility.Visible;
                fuzhi.Visibility = Visibility.Visible;
                textbox2.Text = wenben_j;

            }
            catch (Exception exc)
            {
                App.Huancun.jiemi.Kaishitishi(exc.Message, 1);
            }
        }

        private void textbox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.Huancun.jiemi_wenben.jieguo_jiemi = textbox2.Text;
        }

        private void fuzhi_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dp = new DataPackage();
            dp.SetText(textbox2.Text);
            Clipboard.SetContent(dp);

            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jie_wenben");
            //"复制成功"
            App.Huancun.jiemi.Kaishitishi(resourceLoader.GetString("String8"), 2);
        }

        private void textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.Huancun.jiemi_wenben.wenben_shuru = textbox.Text;
        }
    }
}
