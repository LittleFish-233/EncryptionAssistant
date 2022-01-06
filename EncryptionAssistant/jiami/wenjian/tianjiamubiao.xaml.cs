using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class tianjiamubiao : Page
    {
        //屏蔽
        private kongjian.pingbi msgPopup = new kongjian.pingbi();



        public tianjiamubiao()
        {
            this.InitializeComponent();
            Loaded += Tianjiamubiao_Loaded;
        }

        private void Tianjiamubiao_Loaded(object sender, RoutedEventArgs e)
        {
            gaibiandaxiao((int)(ActualWidth - 100) / 4, 60);
            SizeChanged += Tianjiamubiao_SizeChanged;
            //清理
            App.Huancun.jiami_wenjian.qingli();
            //记录
            App.Huancun.jiami_wenjian.yeshu = 0;
        }

        private void Tianjiamubiao_SizeChanged(object sender, SizeChangedEventArgs e)
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

        private async void tianjia_wenjian_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new Windows.Storage.Pickers.FileOpenPicker();
                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add("*");

                var files = await picker.PickMultipleFilesAsync();
                if (files.Count > 0)
                {
                    foreach (Windows.Storage.StorageFile file in files)
                    {
                        App.Huancun.jiami_wenjian.wenjian_liebiao.tianjiawenjian(file);
                    }
                    Frame.Navigate(typeof(jixutianjia));

                }
                else
                {
                    return;
                }
            }
            catch (Exception exc)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_tianxiecanshu");
                //"选择文件时发生错误<"
                App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String1") + exc.Message + ">", 1);
            }
        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                var folderPicker = new Windows.Storage.Pickers.FolderPicker();
                folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                folderPicker.FileTypeFilter.Add("*");

                Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
                if (folder != null)
                {
                    //开启屏蔽
                    msgPopup.ShowWIndow();
                    //添加我
                    await App.Huancun.jiami_wenjian.wenjian_liebiao.TianjiawenjianjiaAsync(folder);
                    //关闭
                    msgPopup.DismissWindow();

                    Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                    Frame.Navigate(typeof(jixutianjia));

                }
                else
                {
                    return;
                }
            }
            catch (Exception exc)
            {
                //关闭
                msgPopup.DismissWindow();
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_tianxiecanshu");
                //"选择文件夹时发生错误<"
                App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String2") + exc.Message + ">", 1);
            }
        }

        private async void VcBorder_DropAsync(object sender, DragEventArgs e)
        {
            Debug.WriteLine("[Info] Drop");

            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                Debug.WriteLine("[Info] DataView Contains StorageItems");
                var items = await e.DataView.GetStorageItemsAsync();

                //文件过滤 只取vcf文件 PS:如果拖过来的是文件夹 则需要对文件夹处理 取出文件夹文件
                var items_1 = items.OfType<StorageFile>();
                foreach(StorageFile linshi_1 in items_1)
                {
                    App.Huancun.jiami_wenjian.wenjian_liebiao.tianjiawenjian(linshi_1);
                }
                //文件夹
                var items_2 = items.OfType<StorageFolder>();
                foreach (StorageFolder linshi_2 in items_2)
                {
                    //开启屏蔽
                    msgPopup.ShowWIndow();
                    //添加我
                    await App.Huancun.jiami_wenjian.wenjian_liebiao.TianjiawenjianjiaAsync(linshi_2);
                    //关闭
                    msgPopup.DismissWindow();
                }
                Frame.Navigate(typeof(jixutianjia));
            }

        }

        private void VcBorder_DragOver(object sender, DragEventArgs e)
        {
            //设置操作类型
            e.AcceptedOperation = DataPackageOperation.Copy;

            //设置提示文字
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_tianxiecanshu");
            //"拖放此处即可添加文件 o(^▽^)o"
            e.DragUIOverride.Caption = resourceLoader.GetString("String3");


        }
    }
}
