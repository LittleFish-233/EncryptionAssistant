using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace EncryptionAssistant.daima
{
    //加密文件
    public class Jiami_waibu
    {
        //总共字节数
        public ulong zijie_zong = 0;
        //已经加密的字节数
        public ulong zijie_yijing = 0;
        //总文件数
        public Int32 wenjianshu_zhong = 0;
        //已加密文件数
        public Int32 wenjianshu_yijing = 0;
        //错误列表
        public ObservableCollection<Cuowuxinxi> cuowuliebiao = new ObservableCollection<Cuowuxinxi>();
        //是否加密完成
        public bool shifouwangcheng = false;

        //计算总文件数字节数
        private void Jisuanwenjianshu(Wenjianjia wenjianjia)
        {
            foreach (wenjian_liebiao item in wenjianjia.liebiao)
            {
                if (item is Wenjian)
                {
                    wenjianshu_zhong++;
                    zijie_zong += (item as Wenjian).Daxiao_shuzi;
                }
                else
                {
                    Jisuanwenjianshu(item as Wenjianjia);
                }
            }
        }
        //加密文件
        public async Task<int> JiamiwemjianAsync(Wenjianjia wenjianjia, StorageFolder dizhi, bool jilumishi, bool tishi, bool suanchong, bool sanchuyuanwenjian, string mishi, string suanfa, string mima, int censhu)
        {
            if (censhu == 0)
            {
                //清理
                zijie_yijing = 0;
                zijie_zong = 0;
                wenjianshu_yijing = 0;
                wenjianshu_zhong = 0;
                shifouwangcheng = false;
                cuowuliebiao = new ObservableCollection<Cuowuxinxi>();
                //计算
                Jisuanwenjianshu(wenjianjia);
            }
            //遍历
            foreach (wenjian_liebiao item in wenjianjia.liebiao)
            {
                if (item is Wenjian)
                {
                    try
                    {
                        await Jiami_jiemi.Jiami_wenjianAsync((item as Wenjian).wenjian, dizhi, jilumishi, tishi, suanchong, sanchuyuanwenjian, mishi, suanfa, mima);
                    }
                    catch (Exception exc)
                    {
                        Cuowuxinxi linshi = new Cuowuxinxi(item.Wenjianming, item.Dizhi, dizhi.Path, item.Daxiao, exc.Message, cuowuliebiao.Count + 1,DateTime.Now);
                        cuowuliebiao.Add(linshi);
                    }
                    zijie_yijing += item.Daxiao_shuzi;
                    wenjianshu_yijing++;
                }
                else
                {
                    //创建新的文件夹
                    StorageFolder xin = await dizhi.CreateFolderAsync(item.Wenjianming, CreationCollisionOption.GenerateUniqueName);

                    await JiamiwemjianAsync(item as Wenjianjia, xin, jilumishi, tishi, suanchong, sanchuyuanwenjian, mishi, suanfa, mima, censhu + 1);
                }
            }
            //删除原文件夹
            if (sanchuyuanwenjian == true && censhu != 0)
            {
                await wenjianjia.wenjianjia_jilu.DeleteAsync();
            }
            if(censhu==0)
            {
                shifouwangcheng = true;
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
    //解密文件
    public class Jiemi_waibu
    {
        //总共字节数
        public ulong zijie_zong = 0;
        //已经加密的字节数
        public ulong zijie_yijing = 0;
        //总文件数
        public Int32 wenjianshu_zhong = 0;
        //已加密文件数
        public Int32 wenjianshu_yijing = 0;
        //错误列表
        public ObservableCollection<Cuowuxinxi> cuowuliebiao = new ObservableCollection<Cuowuxinxi>();
        //是否加密完成
        public bool shifouwangcheng = false;

        //计算总文件数字节数
        private void Jisuanwenjianshu(Wenjianjia wenjianjia)
        {
            foreach (wenjian_liebiao item in wenjianjia.liebiao)
            {
                if (item is Wenjian)
                {
                    wenjianshu_zhong++;
                    zijie_zong += (item as Wenjian).Daxiao_shuzi;
                }
                else
                {
                    Jisuanwenjianshu(item as Wenjianjia);
                }
            }
        }
        //解密文件
        public async Task<int> JiemiwemjianAsync(Wenjianjia wenjianjia, StorageFolder dizhi, bool sanchuyuanwenjian, string mishi, string suanfa, string mima, int censhu)
        {
            if (censhu == 0)
            {
                //清理
                zijie_yijing = 0;
                zijie_zong = 0;
                wenjianshu_yijing = 0;
                wenjianshu_zhong = 0;
                shifouwangcheng = false;
                cuowuliebiao = new ObservableCollection<Cuowuxinxi>();
                //计算
                Jisuanwenjianshu(wenjianjia);
            }
            //遍历
            foreach (wenjian_liebiao item in wenjianjia.liebiao)
            {
                if (item is Wenjian)
                {
                    try
                    {
                        await Jiami_jiemi.Jiemi_wenjianAsync((item as Wenjian).wenjian, dizhi, sanchuyuanwenjian, mishi, suanfa, mima);
                    }
                    catch (Exception exc)
                    {                     
                        Cuowuxinxi linshi = new Cuowuxinxi(item.Wenjianming, item.Dizhi, dizhi.Path, item.Daxiao, exc.Message, cuowuliebiao.Count + 1,DateTime.Now);
                        cuowuliebiao.Add(linshi);
                    }
                    zijie_yijing += item.Daxiao_shuzi;
                    wenjianshu_yijing++;
                }
                else
                {
                    //创建新的文件夹
                    StorageFolder xin = await dizhi.CreateFolderAsync(item.Wenjianming, CreationCollisionOption.GenerateUniqueName);

                    await JiemiwemjianAsync((item as Wenjianjia), xin,  sanchuyuanwenjian, mishi, suanfa, mima, censhu + 1);
                }
            }
            //删除原文件夹
            if (sanchuyuanwenjian == true && censhu != 0)
            {
                await wenjianjia.wenjianjia_jilu.DeleteAsync();
            }
            if (censhu == 0)
            {
                shifouwangcheng = true;
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }

    //错误信息类
    public class Cuowuxinxi
    {
        private string wenjianming = "";
        private string lujing = "";
        private string baocundao = "";
        private string daxiao = "";
        private string yuanyin = "";
        private int sunxu = 0;
        private DateTime shijian = new DateTime();

        public string Baocundao { get => "保存到："+baocundao; }
        public string Wenjianming { get => wenjianming; }
        public string Lujing { get => "源路径："+lujing; }
        public string Baocundao1 { get => baocundao; }
        public string Daxiao { get => daxiao; }
        public string Yuanyin { get => "错误信息："+yuanyin; }
        public int Sunxu { get => sunxu; }
        public string Shijian { get => shijian.ToString(); }

        public Cuowuxinxi(string wenjianming_, string lujing_, string baocundao_, string daxiao_, string yuanyin_, int sunxu_,DateTime shijian_)
        {
            wenjianming = wenjianming_;
            lujing = lujing_;
            baocundao = baocundao_;
            daxiao = daxiao_;
            yuanyin = yuanyin_;
            sunxu = sunxu_;
            shijian = shijian_;
        }
    }
}
