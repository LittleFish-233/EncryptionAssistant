using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace EncryptionAssistant.daima
{
    public class Xiazaiguangli
    {
        //唯一识别号
        private int weiyi = -1;
        public int Weiyi
        {
            get
            {
                return weiyi;
            }
            set
            {
                weiyi = value;
                baocun_weiyi();
            }
        }
        //正在下载信息列表
        public ObservableCollection<Yuan_xiazai> zhengzai_list { get; set; } = new ObservableCollection<Yuan_xiazai>();
        //已经完成信息列表
        public ObservableCollection<Yuan_xiazai> wangcheng_list { get; set; } = new ObservableCollection<Yuan_xiazai>();

        public Xiazaiguangli()
        {
            duqu_weiyi();
            duqu_jinxing();
        }
        //新建任务
        public void Xinjianrenwu(string dizhi, DateTime kaishishijian, StorageFolder baocun, int leixing,int weiyi_=-1)
        {
            switch (leixing)
            {
                case 1:
                    Xiazaixinxi xiazaixinxi = new Xiazaixinxi();
                    xiazaixinxi.chushihua(dizhi, kaishishijian, baocun, weiyi);
                    Weiyi++;
                    zhengzai_list.Add(xiazaixinxi);
                    //监听事件
                    xiazaixinxi.wancheng += Xiazaixinxi_wancheng;
                    break;
                case 2:
                    http_duan_dan http_Duan_ = new http_duan_dan();
                    if (weiyi_ == -1)
                    {
                        http_Duan_.chushihua(dizhi, kaishishijian, KnownFolders.PicturesLibrary, weiyi);
                        Weiyi++;
                        zhengzai_list.Add(http_Duan_);
                        //监听事件
                        http_Duan_.wancheng += Xiazaixinxi_wancheng;
                        baocun_jinxing();
                    }
                    else
                    {
                        http_Duan_.chushihua(dizhi, kaishishijian, KnownFolders.PicturesLibrary, weiyi_);
                        zhengzai_list.Add(http_Duan_);
                        //监听事件
                        http_Duan_.wancheng += Xiazaixinxi_wancheng;
                    }
                    break;
                case 3:
                    http_fei_duo http_Fei_Duo= new http_fei_duo();
                    http_Fei_Duo.chushihua(dizhi, kaishishijian, baocun, weiyi,10);
                    Weiyi++;
                    zhengzai_list.Add(http_Fei_Duo);
                    //监听事件
                    http_Fei_Duo.wancheng += Xiazaixinxi_wancheng;
                    break;
            }
        }
        //完成任务
        private async void Xiazaixinxi_wancheng(Yuan_xiazai xiazaixinxi)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                zhengzai_list.Remove(xiazaixinxi);
                wangcheng_list.Add(xiazaixinxi);
                baocun_jinxing();
            });
        }
        //删除任务
        public async void shanchurenwuAsync(Yuan_xiazai yuan_Xiazai)
        {
            yuan_Xiazai.stop();
            if(yuan_Xiazai.leixing==2)
            {
                http_duan_dan linshi = yuan_Xiazai as http_duan_dan;
                await Gongju.SanchuwenjianAsync(linshi.linshi_wenjianjia);
            }
            zhengzai_list.Remove(yuan_Xiazai);
        }
        //清空已完成
        public void qingkong_wancheng()
        {
            for (int i = 0; i < wangcheng_list.Count; i++)
            {
                wangcheng_list.RemoveAt(i);
            }
        }
        //保存正在进行的任务
        private async void baocun_jinxing()
        {
            StorageFile wenjian_new = await ApplicationData.Current.LocalFolder.CreateFileAsync("正在进行的任务.txt", CreationCollisionOption.ReplaceExisting);

            var stream = await wenjian_new.OpenAsync(FileAccessMode.ReadWrite);
            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new DataWriter(outputStream))
                {
                    foreach (Yuan_xiazai xiazaixinxi in zhengzai_list)
                    {
                        switch (xiazaixinxi.leixing)
                        {
                            case 2:
                                http_duan_dan xiazaixinxi_ = xiazaixinxi as http_duan_dan;
                                //类型
                                dataWriter.WriteInt32(xiazaixinxi_.leixing);
                                //唯一识别号
                                dataWriter.WriteInt64(xiazaixinxi_.weiyi);

                                //下载地址
                                dataWriter.WriteUInt32((uint)Encoding.UTF8.GetBytes(xiazaixinxi_.dizhi.ToCharArray()).Length);
                                dataWriter.WriteString(xiazaixinxi_.dizhi);

                                break;
                        }
                    }
                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
            stream.Dispose(); // Or use the stream variable (see previous code snippet) with a 
        }
        //读取正在进行的任务
        private async void duqu_jinxing()
        {
            StorageFile wenjian;
            try
            {
                wenjian = await ApplicationData.Current.LocalFolder.GetFileAsync("正在进行的任务.txt");
            }
            catch
            {
                baocun_jinxing();
                return;
            }
            try
            {
                //读取文件
                var stream = await wenjian.OpenAsync(Windows.Storage.FileAccessMode.Read);
                using (var inputStream = stream.GetInputStreamAt(0))
                {
                    using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                    {
                        while (true)
                        {
                            string dizhi_ = "";
                            int leixing_ = 0;
                            long weiyi_ = 0;
                            //类型
                            await dataReader.LoadAsync(sizeof(int));
                            leixing_ = dataReader.ReadInt32();
                            //唯一识别号
                            await dataReader.LoadAsync(sizeof(long));
                            weiyi_ = dataReader.ReadInt64();
                            //下载地址
                            await dataReader.LoadAsync(sizeof(uint));
                            uint changdu = dataReader.ReadUInt32();

                            await dataReader.LoadAsync(changdu);
                            dizhi_ = dataReader.ReadString(changdu);

                            //新建任务
                            Xinjianrenwu(dizhi_,DateTime.Now, KnownFolders.PicturesLibrary, leixing_, (int)weiyi_);
                        }
                    }
                }

            }
            catch(Exception exc)
            {

            }
        }
        //保存唯一识别号
        public async void baocun_weiyi()
        {
           StorageFile  wenjian_new = await ApplicationData.Current.LocalFolder.CreateFileAsync("唯一识别号.txt", CreationCollisionOption.ReplaceExisting);

            var stream = await wenjian_new.OpenAsync(FileAccessMode.ReadWrite);
            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new DataWriter(outputStream))
                {
                    dataWriter.WriteInt32(weiyi);

                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
            stream.Dispose(); // Or use the stream variable (see previous code snippet) with a 
        }
        //读取唯一识别号
        public async void duqu_weiyi()
        {
            StorageFile wenjian;
            try
            {
                wenjian = await ApplicationData.Current.LocalFolder.GetFileAsync("唯一识别号.txt");
            }
            catch
            {
                weiyi = 0;
                baocun_weiyi();
                return;
            }
            //读取文件
            var stream = await wenjian.OpenAsync(Windows.Storage.FileAccessMode.Read);
            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                {
                    await dataReader.LoadAsync(sizeof(int));
                    weiyi = dataReader.ReadInt32();
                }
            }
        }
    }
    public class Yuan_xiazai:INotifyPropertyChanged
    {
        //唯一识别号
        public int weiyi = 0;
        //保存地址
        public StorageFolder baocun_dizhi = null;
        //类型
        public int leixing = 0;
        //已下载大小
        protected long yijing_daxiao = 0;
        //文件名
        protected string wenjianming = "";
        public string Wenjianming {
            get
            {
                return wenjianming;
            }
            set
            {
                wenjianming = value;
                OnpropertyChanged("Wenjianming");
            }
        }
        //大小
        protected long daxiao = 0;
        public string Daxiao
        {
            get
            {
                return "大小："+Gongju.zhanyongkongjian((ulong)yijing_daxiao)+" / "+Gongju.zhanyongkongjian((ulong)daxiao);
            }
        }
        //速度
        protected long sudu = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Sudu_shishi
        {
            get
            {
                return "速度："+Gongju.zhanyongkongjian((ulong)sudu) + "/s";
            }
        }
        //剩余时间
        public string Shengyu_shijian
        {
            get
            {
                if (sudu == 0)
                {
                    return "剩余时间：N/A";
                }
                else
                {
                    return "剩余时间：" + Gongju.zhuanghuashijian(((daxiao - yijing_daxiao) / sudu));
                }
            }
        }
        //百分比
        public int Baifenbi
        {
            get
            {
                return (int)(((double)yijing_daxiao / (double)daxiao) * 100);
            }
        }
        public string Baifenbi_Wenzi
        {
            get
            {
                return Baifenbi.ToString() + " %";
            }
        }
        //结果
        public StorageFile file = null;
        //是否暂停
        public bool shifou_zanting = false;
        public Visibility Kaishi
        {
            get
            {
                if(shifou_zanting==false)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }
        public Visibility Zanting
        {
            get
            {
                if(shifou_zanting==false)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        //开始时间
        public DateTime kaishishijian;
        //结束时间
        public DateTime jieshushijian;

        //暂停下载
        public void stop()
        {
            shifou_zanting = true;
            OnpropertyChanged("Kaishi");
            OnpropertyChanged("Zanting");
        }
        //开始下载
        public void star()
        {
            shifou_zanting = false;
            OnpropertyChanged("Kaishi");
            OnpropertyChanged("Zanting");

        }
        protected async void OnpropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    handler(this, new PropertyChangedEventArgs(name));

                });
            }
        }
    }
    public class Xiazaixinxi:Yuan_xiazai
    {
        //已经使用的时间
        public double yijing_shijian = 0;
        //目标地址
        public string dizhi = "";
        //缓存已下载大小
        public long lishi_daxiao = 0;
        //计时器
        public Timer jishiqi;
        //线程
        public BackgroundWorker worker;       
        //错误信息
        public string cuowuxinxi = "";
        //完成事件
        public delegate void delegateRun(Yuan_xiazai xiazaixinxi);
        //定义一个事件
        public event delegateRun wancheng;
        

        //开始下载
        public void chushihua(string Dizhi,DateTime Shijian,StorageFolder Baocun,int Weiyi)
        {
            leixing = 1;
            //复制地址时间
            dizhi = Dizhi;
            kaishishijian = Shijian;
            baocun_dizhi = Baocun;
            weiyi = Weiyi;
            //启动线程
            worker = new BackgroundWorker();
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted); ;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
            //启动计时
            jishiqi = new System.Threading.Timer(new TimerCallback(Jishiqi_Tick), null, 0, 1000);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            dowloadFile();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        //更新数据
        private void Jishiqi_Tick(object sender)
        {
            sudu = yijing_daxiao - lishi_daxiao;
            lishi_daxiao = yijing_daxiao;
            yijing_shijian++;
            //通知
            OnpropertyChanged("Daxiao");
            OnpropertyChanged("Shengyu_shijian");
            OnpropertyChanged("Sudu_shishi");
            OnpropertyChanged("Baifenbi");
            OnpropertyChanged("Baifenbi_Wenzi");
        }
        //下载主进程
        async void dowloadFile()
        {
            string serverUrl = dizhi;//"https://files.cnblogs.com/files/wgscd/jyzsUpdate.zip"; 
            if(serverUrl=="")
            {
                cuowuxinxi = "要下载的文件地址不能为空";
                return;
            }
            if(baocun_dizhi==null)
            {
                cuowuxinxi = "保存到的文件夹不能为空";
                return;
            }
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(serverUrl);
                System.Net.WebResponse response = await request.GetResponseAsync();

                System.IO.Stream ns = response.GetResponseStream();
                daxiao = response.ContentLength;
                byte[] nbytes = new byte[512];//521,2048 etc
                StorageFolder folder = baocun_dizhi;
                Wenjianming = Gongju.huoquwenjainming(serverUrl);
                file = await folder.CreateFileAsync(Wenjianming, CreationCollisionOption.ReplaceExisting);
                var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
                using (var outputStream = stream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new DataWriter(outputStream))
                    {
                        //初始化
                        int nReadSize = 512;
                        while (true)
                        {
                            //检查是否处于暂停阶段
                            if(shifou_zanting==true)
                            {
                                //Thread.Sleep(100);
                                continue;
                            }
                            //检查是否合格
                            if (daxiao - yijing_daxiao < 512)
                            {
                                nbytes = new byte[daxiao - yijing_daxiao];
                                nReadSize = ns.Read(nbytes, 0, (int)(daxiao - yijing_daxiao));                  
                                dataWriter.WriteBytes(nbytes);

                                yijing_daxiao += daxiao - yijing_daxiao;
                                break;
                            }
                            else
                            {
                                nReadSize = ns.Read(nbytes, 0, 512);
                                dataWriter.WriteBytes(nbytes);

                                yijing_daxiao += nReadSize;
                            }


                        }

                        await dataWriter.StoreAsync();
                        await outputStream.FlushAsync();
                    }
                }
                stream.Dispose();
            }
            catch (Exception ex)
            {
                // fs.Close();
            }
            jishiqi.Change(Timeout.Infinite, 1000);
            jieshushijian = DateTime.Now;
            //触发事件
            wancheng?.Invoke(this);
        }
    }
    public class http_duan_dan : Yuan_xiazai
    {
        //临时文件保存文件夹
        public StorageFolder linshi_wenjianjia = null;

        //保存已经下载完的大小
        public async void baocun_yijing()
        {
            try
            {
                StorageFile wenjian_new = await linshi_wenjianjia.CreateFileAsync("已经下载.txt", CreationCollisionOption.ReplaceExisting);

                var stream = await wenjian_new.OpenAsync(FileAccessMode.ReadWrite);
                using (var outputStream = stream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new DataWriter(outputStream))
                    {
                        dataWriter.WriteInt64(yijing_daxiao);

                        await dataWriter.StoreAsync();
                        await outputStream.FlushAsync();
                    }
                }
                stream.Dispose(); // Or use the stream variable (see previous code snippet) with a 

            }
            catch
            {

            }
        }
        //读取已经下载完的大小
        public async void duqu_yijing()
        {
            StorageFile wenjian;
            try
            {
                wenjian = await linshi_wenjianjia.GetFileAsync("已经下载.txt");
            }
            catch
            {
                yijing_daxiao = 0;
                baocun_yijing();
                return;
            }
            //读取文件
            var stream = await wenjian.OpenAsync(Windows.Storage.FileAccessMode.Read);
            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                {
                    await dataReader.LoadAsync(sizeof(long));
                    yijing_daxiao = dataReader.ReadInt64();
                }
            }
        }


        //已经使用的时间
        public double yijing_shijian = 0;
        //目标地址
        public string dizhi = "";
        //缓存已下载大小
        public long lishi_daxiao = 0;
        //计时器
        public Timer jishiqi;
        //线程
        public BackgroundWorker worker;
        //错误信息
        public string cuowuxinxi = "";
        //完成事件
        public delegate void delegateRun(Yuan_xiazai xiazaixinxi);
        //定义一个事件
        public event delegateRun wancheng;


        //开始下载
        public async void chushihua(string Dizhi, DateTime Shijian, StorageFolder Baocun, int Weiyi, int yijing = 0)
        {
            leixing = 2;
            //复制地址时间
            dizhi = Dizhi;
            kaishishijian = Shijian;
            baocun_dizhi = Baocun;
            weiyi = Weiyi;
            //创建临时文件夹
            linshi_wenjianjia = await ApplicationData.Current.LocalFolder.CreateFolderAsync(weiyi.ToString(), CreationCollisionOption.OpenIfExists);
            //读取已经下载的大小
            duqu_yijing();
            //启动线程
            worker = new BackgroundWorker();
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted); ;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
            //启动计时
            jishiqi = new System.Threading.Timer(new TimerCallback(Jishiqi_Tick), null, 0, 1000);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            dowloadFile();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        //更新数据
        private void Jishiqi_Tick(object sender)
        {
            sudu = yijing_daxiao - lishi_daxiao;
            lishi_daxiao = yijing_daxiao;
            yijing_shijian++;
            //通知
            OnpropertyChanged("Daxiao");
            OnpropertyChanged("Shengyu_shijian");
            OnpropertyChanged("Sudu_shishi");
            OnpropertyChanged("Baifenbi");
            OnpropertyChanged("Baifenbi_Wenzi");
        }
        //下载主进程
        async void dowloadFile()
        {
            string serverUrl = dizhi;
            if (serverUrl == "")
            {
                cuowuxinxi = "要下载的文件地址不能为空";
                return;
            }
            if (baocun_dizhi == null)
            {
                cuowuxinxi = "保存到的文件夹不能为空";
                return;
            }
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(serverUrl);
                System.Net.WebResponse response = await request.GetResponseAsync();

                System.IO.Stream ns = response.GetResponseStream();
                daxiao = response.ContentLength;
                byte[] nbytes = new byte[512];//521,2048 etc

                Wenjianming = Gongju.huoquwenjainming(serverUrl);
                //将网络缓冲区提升
                int linshi_ = 0;
                while(true)
                {
                    if(yijing_daxiao -linshi_<512)
                    {
                        nbytes = new byte[yijing_daxiao-linshi_];
                        ns.Read(nbytes, 0, (int)(yijing_daxiao-linshi_));
                        break;
                    }
                    else
                    {
                        nbytes = new byte[512];
                        ns.Read(nbytes, 0, 512);
                        linshi_ += 512;
                    }
                }
                //正式写入
                int nReadSize = 512;
                while (true)
                {
                    //检查是否处于暂停阶段
                    if (shifou_zanting == true)
                    {
                        //System.Threading.Thread.Sleep(100);
                        continue;
                    }
                    //检查是否合格
                    if (daxiao - yijing_daxiao < 512)
                    {
                        file = await linshi_wenjianjia.CreateFileAsync("A", CreationCollisionOption.OpenIfExists);
                        var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
                        using (var outputStream = stream.GetOutputStreamAt((ulong)yijing_daxiao))
                        {
                            using (var dataWriter = new DataWriter(outputStream))
                            { 
                                nbytes = new byte[daxiao - yijing_daxiao];
                                nReadSize = ns.Read(nbytes, 0, (int)(daxiao - yijing_daxiao));
                                dataWriter.WriteBytes(nbytes);

                                await dataWriter.StoreAsync();
                                await outputStream.FlushAsync();

                                yijing_daxiao += daxiao - yijing_daxiao;
                                baocun_yijing();
                                
                            }
                        }
                        stream.Dispose();
                        break;
                    }
                    else
                    {
                        file = await linshi_wenjianjia.CreateFileAsync("A", CreationCollisionOption.OpenIfExists);
                        var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
                        using (var outputStream = stream.GetOutputStreamAt((ulong)yijing_daxiao))
                        {
                            using (var dataWriter = new DataWriter(outputStream))
                            {

                                for (int i=0;i<1000;i++)
                                {
                                    //检查是否处于暂停阶段
                                    if (shifou_zanting == true)
                                    {
                                        //System.Threading.Thread.Sleep(100);
                                        continue;
                                    }
                                    //检查是否合格
                                    if (daxiao - yijing_daxiao < 512)
                                    {
                                        nbytes = new byte[daxiao - yijing_daxiao];
                                        nReadSize = ns.Read(nbytes, 0, (int)(daxiao - yijing_daxiao));
                                        dataWriter.WriteBytes(nbytes);

                                        yijing_daxiao += daxiao - yijing_daxiao;
                                        break;
                                    }
                                    else
                                    {
                                        nbytes = new byte[512];
                                        nReadSize = ns.Read(nbytes, 0, 512);
                                        dataWriter.WriteBytes(nbytes);

                                        yijing_daxiao += nReadSize;
                                    }
                                }

                                await dataWriter.StoreAsync();
                                await outputStream.FlushAsync();


                                baocun_yijing();
                            }
                        }
                        stream.Dispose();
                    }

                }

                //复制到目标文件夹
                await file.CopyAsync(baocun_dizhi,Wenjianming,NameCollisionOption.ReplaceExisting);
                //删除临时文件
                await Gongju.SanchuwenjianAsync(linshi_wenjianjia);
            }
            catch (Exception ex)
            {
                // fs.Close();
            }
            jishiqi.Change(Timeout.Infinite, 1000);
            jieshushijian = DateTime.Now;
            //触发事件
            wancheng?.Invoke(this);
        }
    }

    public class http_fei_duo:Yuan_xiazai
    {
        //状态 1完成 2已领取 3未领取
        public int[] zhuangtai;
        //剩余线程数目
        public int shenyu_xiancheng = 0;
        //总计线程数目
        public int zong_xiancheng = 0;
        //单次任务大小
        public int renwu_daxiao = 102400;
        //任务缓存
        public Dictionary<int,byte[]> renwu_huancun = new Dictionary<int,byte[]>(); 


        //已经使用的时间
        public double yijing_shijian = 0;
        //目标地址
        public string dizhi = "";
        //缓存已下载大小
        public long lishi_daxiao = 0;
        //计时器
        public Timer jishiqi;
        //线程
        public List<BackgroundWorker> worker = new List<BackgroundWorker>();
        public BackgroundWorker xieru;
        //错误信息
        public string cuowuxinxi = "";
        //完成事件
        public delegate void delegateRun(Yuan_xiazai xiazaixinxi);
        //定义一个事件
        public event delegateRun wancheng;

        //领取任务
        public int lingqurenwu()
        {
            for (int i = 0; i < zhuangtai.Length; i++)
            {
                if (zhuangtai[i] == 3)
                {
                    return i;
                }
            }
            return -1;

        }
        //开始下载
        public async void chushihua(string Dizhi, DateTime Shijian, StorageFolder Baocun, int Weiyi,int jingchengshu)
        {
            leixing = 3;
            //复制地址时间
            dizhi = Dizhi;
            kaishishijian = Shijian;
            baocun_dizhi = Baocun;
            weiyi = Weiyi;
            zong_xiancheng = jingchengshu;
            shenyu_xiancheng = jingchengshu;
            //获取任务大小
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(dizhi);
            System.Net.WebResponse response = await request.GetResponseAsync();
            System.IO.Stream ns = response.GetResponseStream();
            daxiao = response.ContentLength;
            //分割任务
            zhuangtai = new int[(daxiao / renwu_daxiao)+1];
            for(int i=0;i<zhuangtai.Length;i++)
            {
                zhuangtai[i] = 3;
            }
            //启动线程
            for (int i = 0; i < jingchengshu; i++)
            {
                BackgroundWorker linshi = new BackgroundWorker();
                linshi.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted); ;
                linshi.DoWork += new DoWorkEventHandler(worker_DoWork);

                linshi.RunWorkerAsync();
                worker.Add(linshi);
            }
            //启动写入文件线程
            xieru = new BackgroundWorker();
            xieru.RunWorkerCompleted += new RunWorkerCompletedEventHandler(xieru_RunWorkerCompleted); ;
            xieru.DoWork += new DoWorkEventHandler(xieru_DoWork);
            xieru.RunWorkerAsync();
            //启动计时
            jishiqi = new System.Threading.Timer(new TimerCallback(Jishiqi_Tick), null, 0, 1000);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int linshi = lingqurenwu();
            if(linshi==-1)
            {
                return;
            }
            dowloadFile(linshi);
            zhuangtai[linshi] = 2;
        }
        private void xieru_DoWork(object sender, DoWorkEventArgs e)
        {
            xieruwenjian();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int linshi = lingqurenwu();
            if (linshi == -1)
            {
                shenyu_xiancheng--;
                return;
            }
            (sender as BackgroundWorker).RunWorkerAsync();
        }
        private void xieru_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        //更新数据
        private void Jishiqi_Tick(object sender)
        {
            sudu = yijing_daxiao - lishi_daxiao;
            lishi_daxiao = yijing_daxiao;
            yijing_shijian++;
            //通知
            OnpropertyChanged("Daxiao");
            OnpropertyChanged("Shengyu_shijian");
            OnpropertyChanged("Sudu_shishi");
            OnpropertyChanged("Baifenbi");
            OnpropertyChanged("Baifenbi_Wenzi");
        }
        //下载主进程
        async void dowloadFile(int qishiweizhi)
        {
            string serverUrl = dizhi;//"https://files.cnblogs.com/files/wgscd/jyzsUpdate.zip"; 
            if (serverUrl == "")
            {
                cuowuxinxi = "要下载的文件地址不能为空";
                return;
            }
            if (baocun_dizhi == null)
            {
                cuowuxinxi = "保存到的文件夹不能为空";
                return;
            }
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(serverUrl);
                System.Net.WebResponse response = await request.GetResponseAsync();

                System.IO.Stream ns = response.GetResponseStream();
                daxiao = response.ContentLength;
                //判断当前位置和读取大小
                int linshi_daxiao=0;
                int linshi_weizhi = qishiweizhi * renwu_daxiao;
                if(qishiweizhi==(daxiao/renwu_daxiao))
                {
                    linshi_daxiao = (int)(daxiao - (long)renwu_daxiao * (long)qishiweizhi);
                }
                else
                {
                    linshi_daxiao = renwu_daxiao;
                }
                if (linshi_daxiao > 0)
                {
                    //提升缓冲区位置
                    ns.Position = linshi_weizhi;
                    //读取
                    byte[] nbytes = new byte[linshi_daxiao];//521,2048 etc
                    ns.Read(nbytes, 0, (int)(linshi_daxiao));
                    //提交到任务缓存
                    renwu_huancun[qishiweizhi] = nbytes;
                }
            }
            catch (Exception ex)
            {
                // fs.Close();
                zhuangtai[qishiweizhi] = 3;
                return;
            }
            zhuangtai[qishiweizhi] = 1;
        }
        //文件写入
        async void xieruwenjian()
        {
            try
            {
                StorageFolder folder = baocun_dizhi;
                Wenjianming = Gongju.huoquwenjainming(dizhi);
                file = await folder.CreateFileAsync(Wenjianming, CreationCollisionOption.ReplaceExisting);
                var stream = await file.OpenAsync(FileAccessMode.ReadWrite);

                while (shenyu_xiancheng > 0&&lingqurenwu()==-1)
                {
                    foreach (KeyValuePair<int, byte[]> by in renwu_huancun)
                    {
                        using (var outputStream = stream.GetOutputStreamAt((ulong)(by.Key * renwu_daxiao)))
                        {
                            using (var dataWriter = new DataWriter(outputStream))
                            {

                                dataWriter.WriteBytes(by.Value);
                                await dataWriter.StoreAsync();
                                await outputStream.FlushAsync();
                            }
                        }
                    }
                    //Thread.Sleep(100);
                }
                stream.Dispose();

            }
            catch (Exception exc)
            {

            }
            jishiqi.Change(Timeout.Infinite, 1000);
            jieshushijian = DateTime.Now;
            //触发事件
            wancheng?.Invoke(this);

        }
    }
}