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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace EncryptionAssistant.kongjian
{
    public sealed partial class xinjianxiazai : UserControl
    {

        
        public xinjianxiazai()
        {
            this.InitializeComponent();
            Loaded += Xinjianxiazai_Loaded;
        }

        private void Xinjianxiazai_Loaded(object sender, RoutedEventArgs e)
        {
            textbox1.Text = App.Huancun.xiazai.url;
            fangshi.SelectedIndex = App.Huancun.xiazai.fangshi;
            if(App.Huancun.xiazai.linshi_wenjainjia!=null)
            {
                textbox2.Text = App.Huancun.xiazai.linshi_wenjainjia.Path;
            }
        }

        private async void liulan_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                var folderPicker = new Windows.Storage.Pickers.FolderPicker();
                folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                folderPicker.FileTypeFilter.Add("*");

                Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
                if (folder != null)
                {

                    //添加我
                    App.Huancun.xiazai.linshi_wenjainjia = folder;
                    textbox2.Text = folder.Path;


                    Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                }
                else
                {
                    return;
                }
            }
            catch (Exception exc)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("xinjianxiazai");
                //"选择文件夹时发生错误<"
                App.Huancun.xiazai.Kaishitishi(resourceLoader.GetString("String1") + exc.Message + ">", 1);
            }
        }

        private void Quexiao_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Queding_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("xinjianxiazai");

            //检查
            if (App.Huancun.xiazai.fangshi<0)
            {
                //"请选择下载方式"
                App.Huancun.xiazai.Kaishitishi(resourceLoader.GetString("String2"), 1);
                return;
            }
            if(App.Huancun.xiazai.url=="")
            {
                //"请输入下载地址"
                App.Huancun.xiazai.Kaishitishi(resourceLoader.GetString("String3"), 1);
                return;
            }
            if(App.Huancun.xiazai.linshi_wenjainjia==null&&App.Huancun.xiazai.fangshi!=1)
            {
                //"请选择保存到的文件夹"
                App.Huancun.xiazai.Kaishitishi(resourceLoader.GetString("String4"), 1);
                return;
            }
            //调用API
            App.Huancun.xiazai.xiazai_guanli.Xinjianrenwu(App.Huancun.xiazai.url, DateTime.Now, App.Huancun.xiazai.linshi_wenjainjia, App.Huancun.xiazai.fangshi+1);
            //消失
            this.Visibility = Visibility.Collapsed;
        }

        private void Fangshi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("xinjianxiazai");

            App.Huancun.xiazai.fangshi = fangshi.SelectedIndex;
            switch(App.Huancun.xiazai.fangshi)
            {
                case 0:
                    if(App.Huancun.xiazai.linshi_wenjainjia!=null)
                    {
                        textbox2.Text = App.Huancun.xiazai.linshi_wenjainjia.Path;
                    }
                    else
                    {
                        textbox2.Text = "";
                    }
                    liulan.IsEnabled = true;
                    break;
                case 1:
                    liulan.IsEnabled = false;
                    //"此下载方式只能保存到图片库"
                    textbox2.Text = resourceLoader.GetString("String5");
                    break;
            }
        }

        private void Textbox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.Huancun.xiazai.url = textbox1.Text;
        }
    }
}
