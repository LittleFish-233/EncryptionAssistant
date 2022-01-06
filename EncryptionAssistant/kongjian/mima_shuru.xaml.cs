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
using static EncryptionAssistant.daima.Jiami;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace EncryptionAssistant.kongjian
{
    public sealed partial class mima_shuru : UserControl
    {
        //密码错误次数
        private int cuowu = 0;
        //错误信息通知
        public event delegateRun Cuowu;
        //密码状态
        private int mima_zhuangtai=0;
        //密码改变事件
        public event delegateRun PasswordChanged;
        //密码状态改变事件
        public event delegateRun Mimazhuangtai_gaibian;
        //密码记录
        private string mima_shang = "";

        public int Mima_zhuangtai
        {
            get
            {
                return mima_zhuangtai;
            }
            set
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("mima_shuru");

                TextBlock linshi = shiyong.Content as TextBlock;
                switch (value)
                {
                    case 0:
                        //"输入密码（可选）"
                        textblock3.Text = resourceLoader.GetString("String1");
                        //"使用"
                        linshi.Text = resourceLoader.GetString("String2");
                        break;
                    case 1:
                        //"再一次输入密码"
                        textblock3.Text = resourceLoader.GetString("String3");
                        //"确认"
                        linshi.Text = resourceLoader.GetString("String4");
                        break;
                    case 2:
                        //"这是你的密码"
                        textblock3.Text = resourceLoader.GetString("String5");
                        //"重设"
                        linshi.Text = resourceLoader.GetString("String6");
                        break;
                    default:
                        //"在类<mima_shuru>方法<Mimazhuangtai.get>中发生异常<传递的参数无效<"
                        throw new Exception(resourceLoader.GetString("String7") + value + ">>");
                }
            }
        }
        //密码
        public string Mima
        {
            get
            {
                return mima_shu.Password;
            }
            set
            {
                mima_shu.Password = value;
            }
        }

        public mima_shuru()
        {
            this.InitializeComponent();
        }

        private void shiyong_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("mima_shuru");

            //检查
            if (mima_shu.Password == "")
            {
                //"密码不能为空"
                Cuowu?.Invoke(resourceLoader.GetString("String8"), 1);//密码不能为空
                return;
            }


            TextBlock linshi = shiyong.Content as TextBlock;
            //"使用"
            if (linshi.Text == resourceLoader.GetString("String9"))
            {
                //记录
                mima_shang = mima_shu.Password;
                //"再一次输入密码"
                textblock3.Text = resourceLoader.GetString("String10");
                //"确认"
                linshi.Text = resourceLoader.GetString("String11");
                mima_zhuangtai = 1;
                Mimazhuangtai_gaibian?.Invoke(null, 1);
                shiyong.Content = linshi;
                //清空
                mima_shu.Password = "";
                //提示"再一次输入密码"
                Cuowu?.Invoke(resourceLoader.GetString("String12"), 2);//再一次输入密码
            }
            else//"重设"
            if (linshi.Text == resourceLoader.GetString("String13"))
            {
                //"输入密码（可选）"
                textblock3.Text = resourceLoader.GetString("String14");
                //"使用"
                linshi.Text = resourceLoader.GetString("String15");
                mima_zhuangtai = 0;
                Mimazhuangtai_gaibian?.Invoke(null, 0);
                shiyong.Content = linshi;
                //清空
                mima_shu.Password = "";
            }
            else//"确认
            if (linshi.Text == resourceLoader.GetString("String16"))
            {
                if (mima_shu.Password == mima_shang)
                {
                    //"这是你的密码"
                    textblock3.Text = resourceLoader.GetString("String17");
                    //"重设"
                    linshi.Text = resourceLoader.GetString("String18"); 
                    shiyong.Content = linshi;
                    mima_zhuangtai = 2;
                    Mimazhuangtai_gaibian?.Invoke(null, 2);
                }
                else
                {
                    if (cuowu == 3)
                    {
                        //"输入密码（可选）"
                        textblock3.Text = resourceLoader.GetString("String19");
                        // "使用"
                        linshi.Text =resourceLoader.GetString("String20");
                        mima_zhuangtai = 0;
                        Mimazhuangtai_gaibian?.Invoke(null, 0);
                        shiyong.Content = linshi;
                        //清空
                        mima_shu.Password = "";
                        cuowu = 0;
                        return;
                    }
                    cuowu++;
                    //提示"密码错误，请确认后重试"
                    Cuowu?.Invoke(resourceLoader.GetString("String21"), 1);//"密码错误，请确认后重试
                    //"重新输入密码"
                    textblock3.Text = resourceLoader.GetString("String22");
                    mima_shu.Password = "";
                }
            }
        }

        private void mima_shu_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordChanged?.Invoke(mima_shu.Password, 0);
        }
    }
}
