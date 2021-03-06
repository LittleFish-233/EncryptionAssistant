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
    public sealed partial class queren : Page
    {
        //屏蔽
        private kongjian.pingbi msgPopup = new kongjian.pingbi();

        public queren()
        {
            this.InitializeComponent();
            Loaded += Queren_Loaded;
        }

        private void Queren_Loaded(object sender, RoutedEventArgs e)
        {
            gaibiandaxiao((int)(ActualWidth - 100) / 4, 60);
            SizeChanged += Queren_SizeChanged;
            listview.ItemsSource = App.Huancun.jiami_wenjian.wenjian_liebiao.liebiao;
            App.Huancun.jiami_wenjian.suoying = App.Huancun.jiami_wenjian.wenjian_liebiao;
            fanhuishangyiji.Visibility = Visibility.Collapsed;
            //记录
            App.Huancun.jiami_wenjian.yeshu = 4;
        }

        private void Queren_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int gao = 60;
            int kuang = ((int)e.NewSize.Width - 100) / 4;
            gaibiandaxiao(kuang, gao);
        }


        private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            daima.wenjian_liebiao linshi = (daima.wenjian_liebiao)listview.SelectedItem;
            if (linshi == null)
                return;
            if (linshi is daima.Wenjianjia)
            {
                daima.Wenjianjia wenjianjia_linshi = linshi as daima.Wenjianjia;
                listview.ItemsSource = wenjianjia_linshi.liebiao;
                App.Huancun.jiami_wenjian.suoying = wenjianjia_linshi;
                if (wenjianjia_linshi.shangyiji != null)
                {
                    fanhuishangyiji.Visibility = Visibility.Visible;
                }
            }
        }

        private void shanchu_ClickAsync(object sender, RoutedEventArgs e)
        {
            daima.wenjian_liebiao linshi = (daima.wenjian_liebiao)(sender as Button).DataContext;

            App.Huancun.jiami_wenjian.suoying.liebiao.Remove(linshi);

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

        private async void tianjiawenjian_ClickAsync(object sender, RoutedEventArgs e)
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
                        //添加文件
                        App.Huancun.jiami_wenjian.suoying.tianjiawenjian(file);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception exc)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_queren");
                //"选择文件时发生错误<"
                App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String1") + exc.Message + ">", 1);
            }
        }

        private async void tianjiawenjianjia_ClickAsync(object sender, RoutedEventArgs e)
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
                    //添加文件夹
                    daima.Wenjianjia linshi_wenjianjia = new daima.Wenjianjia(folder, App.Huancun.jiami_wenjian.suoying);
                    App.Huancun.jiami_wenjian.suoying.liebiao.Add(linshi_wenjianjia);
                    //关闭
                    msgPopup.DismissWindow();

                    Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);

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
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_queren");
                //"选择文件夹时发生错误<"
                App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String2") + exc.Message + ">", 1);
            }
        }

        private void fanhuishangyiji_Click(object sender, RoutedEventArgs e)
        {
            listview.ItemsSource = App.Huancun.jiami_wenjian.suoying.shangyiji.liebiao;
            App.Huancun.jiami_wenjian.suoying = App.Huancun.jiami_wenjian.suoying.shangyiji;
            if (App.Huancun.jiami_wenjian.suoying.shangyiji == null)
            {
                fanhuishangyiji.Visibility = Visibility.Collapsed;
            }
            else
            {
                fanhuishangyiji.Visibility = Visibility.Visible;
            }
        }

        private void xiayibu_Click(object sender, RoutedEventArgs e)
        {
            //检查是否有对象
            if (App.Huancun.jiami_wenjian.wenjian_liebiao.liebiao.Count == 0)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_queren");
                //"你没有添加任何一个文件或文件夹"
                App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String3"), 1);
                return;
            }
            //检查是否为空的文件夹
            if (App.Huancun.jiami_wenjian.wenjian_liebiao.jiancha_wenjian() == false)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_queren");
                //"你没有添加任何一个文件，无法加密空的文件夹"
                App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String4"), 1);
                return;
            }
            //加密
            App.Huancun.jiami_wenjian.jiami_jingdu.JiamiwemjianAsync(App.Huancun.jiami_wenjian.wenjian_liebiao, App.Huancun.jiami_wenjian.baocun_dizhi, App.Huancun.jiami_wenjian.jilumishi, App.Huancun.jiami_wenjian.tishijiami, App.Huancun.jiami_wenjian.suanchongjiami, App.Huancun.jiami_wenjian.shanchuyuanwenjian, App.Huancun.jiami_wenjian.mishi, App.Huancun.jiami_wenjian.suanfa, App.Huancun.jiami_wenjian.mima_linshi, 0);
            //翻页
            this.Frame.Navigate(typeof(zhengzaijiami));
        }

        private void shangyibu_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(baocun));
        }
    }
    
}
