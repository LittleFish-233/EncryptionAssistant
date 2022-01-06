using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
    public sealed partial class zhengzaijiemi : Page
    {
        //计时器启动次数
        static ulong t = 0;
        //上一次激发计时器时的大小
        double shang_daxiao = 0;
        //当前速度
        double shudu_dangqian = 0;
        //计时器
        DispatcherTimer jishi = new DispatcherTimer();

        public zhengzaijiemi()
        {
            this.InitializeComponent();
            Loaded += Zhengzaijiami_Loaded;
        }

        private void Zhengzaijiami_Loaded(object sender, RoutedEventArgs e)
        {
            //记录 
            App.Huancun.jiemi_wenjian.yeshu = 5;
            if(App.Huancun.jiemi_wenjian.jiemi_jingdu!=null)
            {
                if(App.Huancun.jiemi_wenjian.jiemi_jingdu.shifouwangcheng==false)
                {
                    //启动监视
                    jishi.Interval = new TimeSpan(0, 0, 0, 0,250);
                    jishi.Tick += Jishi_Tick;
                    jishi.Start();
                    t = 0;
                }
                else
                {
                    if (App.Huancun.jiemi_wenjian.jiemi_jingdu.cuowuliebiao.Count < 1)
                    {
                        Frame.Navigate(typeof(chenggong));
                    }
                    else
                    {
                        var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jie_zhengzaijiemi");
                        //"解密文件失败"
                        App.Huancun.jiemi.Kaishitishi(resourceLoader.GetString("String1"), 1);
                        Frame.Navigate(typeof(shibai));
                    }
                }
            }
            else
            {
                Frame.Navigate(typeof(tianjiamubiao));
            }
        }

        private void Jishi_Tick(object sender, object e)
        {
            //更新假进度条
            t = t + 5;
            //ProgressBar1.Value = t / 100
            jingdutiao_jia.Value = 100 - 100 * Math.Pow(Math.E, (-0.001 * t));
            //更新真进度条
            jingdutiao_zheng.Value = ((double)((double)App.Huancun.jiemi_wenjian.jiemi_jingdu.zijie_yijing / (double)App.Huancun.jiemi_wenjian.jiemi_jingdu.zijie_zong)) * 100;

            //更新参数
            if (App.Huancun.jiemi_wenjian.jiemi_jingdu.zijie_yijing != 0)
            {
                App.Huancun.jiemi_wenjian.shijian = t / 20;//记录时间
                textblock3.Text = daima.Gongju.shijianzhuanghuan(t / 20);//转化时间
                if (App.Huancun.jiemi_wenjian.jiemi_jingdu.zijie_yijing != shang_daxiao)//判断大小是否改变
                {
                    //计算速度
                    shudu_dangqian = ((double)App.Huancun.jiemi_wenjian.jiemi_jingdu.zijie_yijing - shang_daxiao) / ((double)t / (double)20);
                    textblock7.Text = daima.Gongju.zhanyongkongjian((ulong)shudu_dangqian) + "/S";
                }
                //剩余时间
                textblock5.Text = daima.Gongju.shijianzhuanghuan((App.Huancun.jiemi_wenjian.jiemi_jingdu.zijie_zong - App.Huancun.jiemi_wenjian.jiemi_jingdu.zijie_yijing) / ((ulong)shudu_dangqian));
                textblock9.Text = jingdutiao_zheng.Value.ToString("0.00") + "%";//百分比

            }
            //更新大小
            shang_daxiao = (double)App.Huancun.jiemi_wenjian.jiemi_jingdu.zijie_yijing;

            //检查是否解密成功
            if (App.Huancun.jiemi_wenjian.jiemi_jingdu.shifouwangcheng==true)
            {
                jishi.Stop();

                if (App.Huancun.jiemi_wenjian.jiemi_jingdu.cuowuliebiao.Count < 1)
                {
                    Frame.Navigate(typeof(chenggong));
                }
                else
                {
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jie_zhengzaijiemi");

                    App.Huancun.jiemi.Kaishitishi(resourceLoader.GetString("String1"), 1);
                    Frame.Navigate(typeof(shibai));
                }
            }
        }
    }
}
