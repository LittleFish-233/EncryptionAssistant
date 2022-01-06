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

namespace EncryptionAssistant.jiami.wenjian
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class zhengzaijiami : Page
    {
        //计时器启动次数
        static ulong t = 0;
        //上一次激发计时器时的大小
        double shang_daxiao = 0;
        //当前速度
        double shudu_dangqian = 0;
        //计时器
        DispatcherTimer jishi = new DispatcherTimer();

        public zhengzaijiami()
        {
            this.InitializeComponent();
            Loaded += Zhengzaijiami_Loaded;
        }

        private void Zhengzaijiami_Loaded(object sender, RoutedEventArgs e)
        {
            //记录 
            App.Huancun.jiami_wenjian.yeshu = 5;
            if(App.Huancun.jiami_wenjian.jiami_jingdu!=null)
            {
                if(App.Huancun.jiami_wenjian.jiami_jingdu.shifouwangcheng==false)
                {
                    //启动监视
                    jishi.Interval = new TimeSpan(0, 0, 0, 0,250);
                    jishi.Tick += Jishi_Tick;
                    jishi.Start();
                    t = 0;
                }
                else
                {
                    if (App.Huancun.jiami_wenjian.jiami_jingdu.cuowuliebiao.Count < 1)
                    {
                        Frame.Navigate(typeof(chenggong));
                    }
                    else
                    {
                        Frame.Navigate(typeof(shibai));
                        var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_zhengzaijiami");
                        //"加密文件失败"
                        App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String1"), 1);
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
            jingdutiao_zheng.Value = ((double)((double)App.Huancun.jiami_wenjian.jiami_jingdu.zijie_yijing / (double)App.Huancun.jiami_wenjian.jiami_jingdu.zijie_zong)) * 100;
            
            //更新参数
            if (App.Huancun.jiami_wenjian.jiami_jingdu.zijie_yijing != 0)
            {
                //记录时间
                App.Huancun.jiami_wenjian.shijian = t / 20;
                textblock3.Text = daima.Gongju.shijianzhuanghuan(t / 20);
                if (App.Huancun.jiami_wenjian.jiami_jingdu.zijie_yijing != shang_daxiao)
                {
                    //计算速度
                    shudu_dangqian=((double)App.Huancun.jiami_wenjian.jiami_jingdu.zijie_yijing - shang_daxiao) / ((double)t / (double)20);
                    textblock7.Text = daima.Gongju.zhanyongkongjian((ulong)shudu_dangqian) + "/S";
                }
                textblock5.Text = daima.Gongju.shijianzhuanghuan((App.Huancun.jiami_wenjian.jiami_jingdu.zijie_zong - App.Huancun.jiami_wenjian.jiami_jingdu.zijie_yijing) / ((ulong)shudu_dangqian));
                textblock9.Text = jingdutiao_zheng.Value.ToString("0.00") + "%";

            }
            //更新大小
            shang_daxiao = (double)App.Huancun.jiami_wenjian.jiami_jingdu.zijie_yijing;

            //检查是否加密成功
            if (App.Huancun.jiami_wenjian.jiami_jingdu.shifouwangcheng==true)
            {
                jishi.Stop();

                if (App.Huancun.jiami_wenjian.jiami_jingdu.cuowuliebiao.Count < 1)
                {
                    Frame.Navigate(typeof(chenggong));
                }
                else
                {
                    Frame.Navigate(typeof(shibai));
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("jia_zhengzaijiami");
                    //"加密文件失败"
                    App.Huancun.jiami.Kaishitishi(resourceLoader.GetString("String2"), 1);
                }
            }
        }
    }
}
