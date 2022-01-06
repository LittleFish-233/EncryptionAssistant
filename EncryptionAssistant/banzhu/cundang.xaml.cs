using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace EncryptionAssistant.banzhu
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>

    public sealed partial class cundang : Page
    {
        FileOpenPicker openPicker = new FileOpenPicker();

        bool shifouchushihua = false;

        string mishi_zhifu = "";
        string mima_zifu = "";
        private bool shifoushiyouzhidingyi = false;

        public cundang()
        {
            this.InitializeComponent();

            textblock4.Visibility = Visibility.Collapsed;
            mima.Visibility = Visibility.Collapsed;
            wangcheng(true);
            TextBlock text1 = new TextBlock();
            text1 = new TextBlock();
            text1.Text = (string)combobox1.Header;
            combobox1.Header = text1;
            text1 = new TextBlock();
            text1.Text = (string)combobox.Header;
            combobox.Header = text1;
            text1 = new TextBlock();
            text1.Text = (string)textbox2.Header;
            textbox2.Header = text1;
            text1 = new TextBlock();
            text1.Text = (string)textbox3.Header;
            textbox3.Header = text1;
            text1 = new TextBlock();
            text1.Text = (string)textbox4.Header;
            textbox4.Header = text1;

            //恢复
            textbox3.Password = App.Huancun.cundan.wenjian_1;
            textbox4.Password = App.Huancun.cundan.wenjian_2;
            if (App.Huancun.cundan.wenjian_shifouzidingyimishi == true)
            {
                mccheckbox1.IsChecked = false;
            }
            else
            {
                mccheckbox1.IsChecked = true;
            }
            textbox2.Text = App.Huancun.cundan.wenjian_mishi;
            combobox1.SelectedIndex = App.Huancun.cundan.wenjian_suanfa;
            combobox.SelectedIndex = App.Huancun.cundan.wenjian_moshi;
            if (combobox.SelectedIndex == 0)
            {
                mima.Visibility = Visibility.Collapsed;
            }
            else
            {
                mima.Visibility = Visibility.Visible;
            }
            textblock9.Text = App.Huancun.cundan.wenjian_ming1;

            shifouchushihua = true;
        }

        private async void button1_ClickAsync(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("cundang");

            if (App.Huancun.cundan.wenjian_wenjain != null)
            {
                wangcheng(false);
                //确认文件大小
                var li = await App.Huancun.cundan.wenjian_wenjain.GetBasicPropertiesAsync();
                double kongjian = li.Size;

                if (kongjian >= 1024 * 1024 * 390)
                {
                    //"应用内存不足无法加密大于（390MB）的文件，敬请期待后续更新"
                    App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String1"), 1);
                    wangcheng(true);
                    return;
                }
                //核对密匙

                if (shifoushiyouzhidingyi == true)
                {
                    if (mishi_zhifu.Length < 40)
                    {
                        //"密匙长度必须是40，请核对密匙"
                        App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String2"), 1);
                        wangcheng(true);
                        return;

                    }
                }
                else
                {
                    mishi_zhifu = "0000000000000000000000000101001101010011";
                }
                //检查密码
                if (combobox.SelectedIndex != 0)
                {
                    if (textbox4.Password != textbox3.Password || textblock3.Text.Length > 60)
                    {
                        //"设置密码时，请确保两次输入的密码一致，且长度小于60个字符"
                        App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String3"), 1);
                        wangcheng(true);
                        return;
                    }
                }
                //进行加密
                App.Huancun.cundan.wenjain_jieguo = await daima.guoshi.Jiami_jiemi.JiamiAsync(App.Huancun.cundan.wenjian_wenjain, mishi_zhifu, combobox.SelectedIndex, combobox1.SelectedIndex, mima_zifu);
                if (App.Huancun.cundan.wenjain_jieguo == null)
                {
                    //"加密时出现问题，请确保加密密匙，加密模式，加密算法的正确性，核对后请重试。如果你看到过这个提示很多次，请在反馈中心把这个问题报告给我们，谢谢"
                    App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String4"), 1);
                }
                else
                {
                    //"加密成功，点击下面的图片保存"
                    App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String5"), 2);
                }
            }
            else
            {
                //"请点击上面的图片选择要进行加密的文件。"
                App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String6"), 1);
            }

            wangcheng(true);
        }

        private async void button2_ClickAsync(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("cundang");

            if (App.Huancun.cundan.wenjian_wenjain != null)
            {
                wangcheng(false);
                //确认文件大小
                var li = await App.Huancun.cundan.wenjian_wenjain.GetBasicPropertiesAsync();
                double kongjian = li.Size;

                if (kongjian >= 1024 * 1024 * 390)
                {
                    //"应用内存不足无法解密大于（390MB）的文件，敬请期待后续更新"
                    App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String7"), 1);
                    wangcheng(true);
                    return;
                }
                //核对密匙
                if (shifoushiyouzhidingyi == true)
                {
                    if (mishi_zhifu.Length < 40)
                    {
                        //"密匙长度必须是40，请核对密匙"
                        App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String8"), 1);
                        wangcheng(true);
                        return;

                    }
                }
                else
                {
                    mishi_zhifu = "0000000000000000000000000101001101010011";
                }
                //检查密码
                if (combobox.SelectedIndex != 0)
                {
                    if (textbox4.Password != textbox3.Password || textblock3.Text.Length > 60)
                    {
                        //"输入密码时，请确保两次输入的密码一致，且长度小于60个字符"
                        App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String9"), 1);
                        wangcheng(true);
                        return;
                    }

                }
                //检查是否为新文件
                try
                {
                    string text;
                    var stream = await App.Huancun.cundan.wenjian_wenjain.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    using (var inputStream = stream.GetInputStreamAt(0))
                    {
                        // We'll add more code here in the next step.
                        using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                        {
                            uint numBytesLoaded = await dataReader.LoadAsync((uint)2);
                            text = dataReader.ReadString(numBytesLoaded);
                        }
                    }
                    if (text == "##")
                    {
                        //"请前往主页解密此文件<版本高于1.3.711>"
                        App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String10"), 1);
                        wangcheng(true);
                        return;
                    }
                }
                catch
                {
                    //"尝试读取文件版本失败"
                    App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String11"), 1);
                }



                //进行解密
                App.Huancun.cundan.wenjain_jieguo = await daima.guoshi.Jiami_jiemi.JiemiAsync(App.Huancun.cundan.wenjian_wenjain, mishi_zhifu, combobox.SelectedIndex, combobox1.SelectedIndex, mima_zifu);
                if (App.Huancun.cundan.wenjain_jieguo == null)
                {
                    //"解密时出现问题，请确保解密密匙，解密模式，解密算法的正确性，核对后请重试。如果你看到过这个提示很多次，请在反馈中心把这个问题报告给我们，谢谢"
                    App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String12"), 1);
                }
                else
                {
                    //"解密成功，点击下面的图片保存"
                    App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String13"), 2);
                }
            }
            else
            {
                //"请点击上面的图片选择要进行解密的文件"
                App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String14"), 2);
            }
            wangcheng(true);
        }

        private async void button3_ClickAsync(object sender, RoutedEventArgs e)
        {
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add("*");

            App.Huancun.cundan.wenjian_wenjain = await openPicker.PickSingleFileAsync();
            if (App.Huancun.cundan.wenjian_wenjain != null)
            {
                textblock9.Text = App.Huancun.cundan.wenjian_wenjain.Path;
            }
            else
            {
                textblock9.Text = "";
            }
            App.Huancun.cundan.wenjian_ming1 = textblock9.Text;

        }

        private async void button4_ClickAsync(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("cundang");

            if (App.Huancun.cundan.wenjain_jieguo != null)
            {
                wangcheng(false);
                var folderPicker = new Windows.Storage.Pickers.FolderPicker();
                folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                folderPicker.FileTypeFilter.Add("*");

                Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
                if (folder == null)
                {
                    wangcheng(true);
                    return;
                }
                if (await daima.Gongju.FuzhiwenjainAsync(folder, App.Huancun.cundan.wenjain_jieguo) == null)
                {
                    //"保存结果时出现问题，请重试。若重试多次后依旧出现这条提示，请将此问题反馈给我们，我们会尽力解决，谢谢
                    App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String15"), 1);
                }
                else
                {
                    //"保存成功"
                    App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String16"), 2);

                }
            }
            else
            {
                //"请先进行加密/解密。"
                App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String17"), 1);
            }
            wangcheng(true);

        }
        public void wangcheng(bool shifouwangcheng)
        {
            if (shifouwangcheng == false)
            {
                progreering1.Visibility = Visibility.Visible;
                button1.IsEnabled = false;
                button2.IsEnabled = false;
                button3.IsEnabled = false;
                button4.IsEnabled = false;
            }
            else
            {
                progreering1.Visibility = Visibility.Collapsed;
                button1.IsEnabled = true;
                button2.IsEnabled = true;
                button3.IsEnabled = true;
                button4.IsEnabled = true;

            }
        }

        private void button6_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (textbox2.Text.Length < 40)
            {
                textbox2.Text += "0";
            }
            else
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("cundang");
                //"恭喜你，密匙长度已经达到要求的40个字符。"
                App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String18"), 2);
            }
        }

        private void button7_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (textbox2.Text.Length < 40)
            {
                textbox2.Text += "1";
            }
            else
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("cundang");
                //"恭喜你，密匙长度已经达到要求的40个字符。"
                App.Huancun.cundan.Kaishitishi(resourceLoader.GetString("String19"), 2);
            }

        }

        private void Textbox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textbox2.Text.Length == 40)
            {
                textblock4.Visibility = Visibility.Visible;
            }
            else
            {
                textblock4.Visibility = Visibility.Collapsed;
            }
            mishi_zhifu = textbox2.Text;
            if (shifouchushihua == true)
            {
                App.Huancun.cundan.wenjian_mishi = textbox2.Text;
            }
        }

        private void mccheckbox1_Unchecked(object sender, RoutedEventArgs e)
        {
            mishi.Visibility = Visibility.Visible;
            shifoushiyouzhidingyi = true;
            if (shifouchushihua == true)
            {
                App.Huancun.cundan.wenjian_shifouzidingyimishi = true;
            }

        }


        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            textbox2.Text = "";
        }

        private void mccheckbox1_Checked(object sender, RoutedEventArgs e)
        {
            mishi.Visibility = Visibility.Collapsed;
            shifoushiyouzhidingyi = false;
            if (shifouchushihua == true)
            {
                App.Huancun.cundan.wenjian_shifouzidingyimishi = false;
            }
        }
        private void textbox3_PasswordChanged(object sender, RoutedEventArgs e)
        {
            textblock10.Text = "";
            if (shifouchushihua == true)
            {
                App.Huancun.cundan.wenjian_1 = textbox3.Password;
            }
            if (textbox3.Password.Length >= 60)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("cundang");
                string zifu = resourceLoader.GetString("String20");//密码过长，应在0~20字符之间。
                textblock10.Text = zifu;
                return;
            }
        }

        private void textbox4_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (shifouchushihua == true)
            {
                App.Huancun.cundan.wenjian_2 = textbox4.Password;
            }
            if (textbox3.Password != textbox4.Password)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("cundang");
                string zifu = resourceLoader.GetString("String21");//两次输入的密码不一致。
                textblock11.Text = zifu;
                mima_zifu = "";
            }
            else
            {
                textblock11.Text = "";
                mima_zifu = textbox4.Password;
            }
        }

        private void combobox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (shifouchushihua == true)
            {
                App.Huancun.cundan.wenjian_suanfa = combobox1.SelectedIndex;
            }
        }

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (shifouchushihua == true)
            {

                if (combobox.SelectedIndex == 0)
                {
                    mima.Visibility = Visibility.Collapsed;
                }
                else
                {
                    mima.Visibility = Visibility.Visible;
                }


                App.Huancun.cundan.wenjian_moshi = combobox.SelectedIndex;
            }
        }
    }

}
