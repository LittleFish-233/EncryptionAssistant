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

namespace EncryptionAssistant.MD5
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class wenben : Page
    {
        public wenben()
        {
            this.InitializeComponent();
            Loaded += Wenben_Loaded;
        }

        private void Wenben_Loaded(object sender, RoutedEventArgs e)
        {
            //文本输入
            wenzi.Text = App.Huancun.md5_Xiaoyan.wenben_shuru;
            //算法
            chuli_suanfa.Suanfa_huoqu = App.Huancun.md5_Xiaoyan.suanfa_wenben;
            //结果
            if (App.Huancun.md5_Xiaoyan.jieguo_wenben == "")
            {
                jieguo.Visibility = Visibility.Collapsed;
            }
            else
            {
                jieguo.Visibility = Visibility.Visible;
                jieguo.Text = App.Huancun.md5_Xiaoyan.jieguo_wenben;
            }
            //订阅事件
            chuli_suanfa.Suanfa_changed += Chuli_suanfa_Suanfa_changed;
        }

        private void Chuli_suanfa_Suanfa_changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            App.Huancun.md5_Xiaoyan.suanfa_wenben = chuli_suanfa.Suanfa_huoqu;
        }

        private void Jisuan_Click(object sender, RoutedEventArgs e)
        {
            //算法
            HashAlgorithmProvider alg = null;
            switch (chuli_suanfa.Suanfa_huoqu)
            {
                case "Md5":
                    alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
                    break;
                case "Sha1":
                    alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
                    break;
                case "Sha256":
                    alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
                    break;
                case "Sha384":
                    alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha384);
                    break;
                case "Sha512":
                    alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha512);
                    break;
            }
            //加密
            string jieguo = "";
            try
            {
                jieguo = daima.guoshi.Jiami_jiemi.Jiami_md5(wenzi.Text, alg);
            }
            catch (Exception exc)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("md5_wenben");
                //"对文件进行消息摘要时发生错误（" "）.若重试多次后仍然出现这条消息，请在反馈中心提出，我们会尽快调查并解决问题"
                App.Huancun.md5_Xiaoyan.Kaishitishi(resourceLoader.GetString("String1")+ exc.Message + resourceLoader.GetString("String2"), 1);
                jieguo = "";
            }
            //显示
            App.Huancun.md5_Xiaoyan.jieguo_wenben = this.jieguo.Text = jieguo;
            this.jieguo.Visibility = Visibility.Visible;

        }

        private void Wenzi_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.Huancun.md5_Xiaoyan.wenben_shuru = wenzi.Text;
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
            wenzi.Text = str;
            //提示
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("md5_wenben");
            //"粘贴成功"
            App.Huancun.jiemi.Kaishitishi(resourceLoader.GetString("String3"), 2);
        }

        private void Jieguo_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.Huancun.md5_Xiaoyan.jieguo_wenben = jieguo.Text;
        }
    }
}
