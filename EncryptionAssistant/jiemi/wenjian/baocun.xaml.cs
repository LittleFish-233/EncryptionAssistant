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

namespace EncryptionAssistant.jiemi.wenjian
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class baocun : Page
    {
        public baocun()
        {
            this.InitializeComponent();
            Loaded += Baocun_Loaded;
        }

        private void Baocun_Loaded(object sender, RoutedEventArgs e)
        {
            if(App.Huancun.jiemi_wenjian.baocun_dizhi!=null)
            {
                dizhi_shuru.Text = App.Huancun.jiemi_wenjian.baocun_dizhi.Path;
            }
            gaibiandaxiao((int)(ActualWidth - 100) / 4, 60);

            SizeChanged += Baocun_SizeChanged;
            //记录
            App.Huancun.jiemi_wenjian.yeshu = 3;
        }

        private void Baocun_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int gao = 60;
            int kuang = ((int)e.NewSize.Width - 100) / 4;
            gaibiandaxiao(kuang, gao);
        }

        private void shangyibu_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(tianxiecanshu));
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

        private void xiayibu_Click(object sender, RoutedEventArgs e)
        {
            if(App.Huancun.jiemi_wenjian.baocun_dizhi!=null)
            {
                Frame.Navigate(typeof(queren));
            }
            else
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jie_baocun");
                //"请选择一个文件夹以保存加密结果"
                App.Huancun.jiemi.Kaishitishi(resourceLoader.GetString("String1"), 1);
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
                    App.Huancun.jiemi_wenjian.baocun_dizhi = folder;
                    dizhi_shuru.Text = folder.Path;
                   

                    Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                }
                else
                {
                    return;
                }
            }
            catch (Exception exc)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jie_baocun");
                //"选择文件夹时发生错误<" 
                App.Huancun.jiemi.Kaishitishi(resourceLoader.GetString("String2") + exc.Message + ">", 1);
            }
        }
    }
}
