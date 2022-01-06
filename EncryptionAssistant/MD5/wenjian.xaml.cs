using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
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
    public sealed partial class wenjian : Page
    {
        public wenjian()
        {
            this.InitializeComponent();
            Loaded += Wenjian_Loaded;
        }

        private void Wenjian_Loaded(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("md5_wenjian");

            if (App.Huancun.md5_Xiaoyan.wenjian_xuanzhe == null)
            {
                //"将文件拖拽到该区域内开始校验"
                textblock1.Text = resourceLoader.GetString("String1");
            }
            else
            {
                //"检测到文件  可以继续添加以覆盖旧文件"
                textblock1.Text = resourceLoader.GetString("String2");
            }
            //算法
            chuli_suanfa.Suanfa_huoqu = App.Huancun.md5_Xiaoyan.suanfa_wenjian;
            //结果
            if (App.Huancun.md5_Xiaoyan.jieguo_wenjian == "")
            {
                jieguo.Visibility = Visibility.Collapsed;
            }
            else
            {
                jieguo.Visibility = Visibility.Visible;
                jieguo.Text = App.Huancun.md5_Xiaoyan.jieguo_wenjian;
            }
            //订阅事件
            chuli_suanfa.Suanfa_changed += Chuli_suanfa_Suanfa_changed;
        }

        private void Chuli_suanfa_Suanfa_changed(object sender, PropertyChangedEventArgs e)
        {
            App.Huancun.md5_Xiaoyan.suanfa_wenjian = chuli_suanfa.Suanfa_huoqu;
        }

        private async void button1_ClickAsync(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("md5_wenjian");

            try
            {
                var picker = new Windows.Storage.Pickers.FileOpenPicker();
                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add("*");

                var files = await picker.PickMultipleFilesAsync();
                if (files.Count > 0)
                {
                    App.Huancun.md5_Xiaoyan.wenjian_xuanzhe = files[0];
                    //显示"检测到文件  可以继续添加以覆盖旧文件"
                    textblock1.Text = resourceLoader.GetString("String3");
                }
                else
                {
                    return;
                }
            }
            catch (Exception exc)
            {
                //"选择文件时发生错误<" 
                App.Huancun.md5_Xiaoyan.Kaishitishi(resourceLoader.GetString("String4")+ exc.Message + ">", 1);
            }
        }
        private async void VcBorder_DropAsync(object sender, DragEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("md5_wenjian");

            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();

                //文件过滤 只取vcf文件 PS:如果拖过来的是文件夹 则需要对文件夹处理 取出文件夹文件
                var items_1 = items.OfType<StorageFile>();
                foreach (StorageFile linshi_1 in items_1)
                {
                    App.Huancun.md5_Xiaoyan.wenjian_xuanzhe = linshi_1;
                    //显示 "检测到文件  可以继续添加以覆盖旧文件"
                    textblock1.Text =resourceLoader.GetString("String5");
                }
            }

        }

        private void VcBorder_DragOver(object sender, DragEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("md5_wenjian");

            //设置操作类型
            e.AcceptedOperation = DataPackageOperation.Copy;

            //设置提示文字"拖放此处即可添加文件 o(^▽^)o"
            e.DragUIOverride.Caption = resourceLoader.GetString("String6");
        }

        private async void Jisuan_ClickAsync(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("md5_wenjian");

            if (App.Huancun.md5_Xiaoyan.wenjian_xuanzhe==null)
            {
                //"请先添加文件"
                App.Huancun.md5_Xiaoyan.Kaishitishi(resourceLoader.GetString("String7"), 1);
                return;
            }
            //屏蔽"正     在     计     算"
            jisuan.Content = resourceLoader.GetString("String8");
            jisuan.IsEnabled = false;
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
            string jieguo = null;
            try
            {
                jieguo = await daima.guoshi.Jiami_jiemi.Jiami_md5Async(App.Huancun.md5_Xiaoyan.wenjian_xuanzhe, alg);
            }
            catch (NullReferenceException exc)
            {
                //"对文件进行消息摘要时发生错误（" "）.若重试多次后仍然出现这条消息，请在反馈中心提出，我们会尽快调查并解决问题。"
                App.Huancun.md5_Xiaoyan.Kaishitishi(resourceLoader.GetString("String9") + exc.Message + resourceLoader.GetString("String10"), 1);
                this.jieguo.Visibility = Visibility.Visible;
                //"记                     算"
                jisuan.Content = resourceLoader.GetString("String11");
                jisuan.IsEnabled = true;
                return;
            }
            //显示
            App.Huancun.md5_Xiaoyan.jieguo_wenben = this.jieguo.Text = jieguo;
            this.jieguo.Visibility = Visibility.Visible;
            jisuan.Content =resourceLoader.GetString("String11");
            jisuan.IsEnabled = true;
        }

        private void Wenzi_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.Huancun.md5_Xiaoyan.jieguo_wenjian = jieguo.Text;
        }
    }
}
