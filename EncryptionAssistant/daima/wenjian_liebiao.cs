using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace EncryptionAssistant.daima
{
    //文件列表
    public class wenjian_liebiao
    {
        //文件名
        public string Wenjianming { get; set; }
        //大小
        protected ulong daxiao = 0;
        public string Daxiao
        {
            get
            {
                if(daxiao<=0)
                {
                    return "This is a folder";
                }
                else
                {
                   return"大小："+ Gongju.zhanyongkongjian(daxiao);
                }
            }
        }
        public ulong Daxiao_shuzi
        {
            get
            {
                return daxiao;
            }
        }
        //修改时间
        public string Xiugaishijian { get; set; } = "";
        //地址
        public string Dizhi { get; set; }
    }
    public class Wenjianjia:wenjian_liebiao
    {
        //文件夹内列表
        public ObservableCollection<wenjian_liebiao> liebiao { get; set; } = new ObservableCollection<wenjian_liebiao>();
        //当前文件夹对象
        public StorageFolder wenjianjia_jilu;
        //上一个文件夹
        public Wenjianjia shangyiji = null;

        //初始化
        public Wenjianjia(StorageFolder wenjianjia_linshi,Wenjianjia shang)
        {
            //初始化参数
            wenjianjia_jilu = wenjianjia_linshi;
            Wenjianming = wenjianjia_linshi.Name;
            Dizhi = wenjianjia_linshi.Path;
            TianjiawenjianjiaAsync(wenjianjia_linshi);
            //保存引用
            shangyiji = shang;
        }

        public Wenjianjia()
        {
        }
        //检查是否存在文件
        public bool jiancha_wenjian()
        {
            //在当前列表中寻找
            foreach(wenjian_liebiao linshi in liebiao)
            {
                if(linshi is Wenjian)
                {
                    return true;
                }
                else
                { 
                    //继续在文件夹中寻找

                    bool shifou = (linshi as Wenjianjia).jiancha_wenjian();
                    if(shifou==true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //添加文件
        public async Task tianjiawenjian(StorageFile wenjian)
        {
            if (wenjian == null)
            {
                throw new Exception("在类<Wenjianjia>方法<TianjiawenjianjiaAsync>中发生异常<<" + wenjianjia_jilu.Name + ">中初始化子文件时传递的文件为空>");
            }
            else
            {
                try
                {
                    Wenjian wenjian_linshi = new Wenjian();
                  await  wenjian_linshi.chushihuaAsync(wenjian);
                    liebiao.Add(wenjian_linshi);
                }
                catch(Exception exc)
                {
                    throw new Exception("在类<Wenjianjia>方法<tianjiawenjian>中发生异常<" + exc.Message + ">可能的解释是<文件不存在,无法向列表中添加文件>", exc);
                }

            }
        }
        //添加文件夹
        public async Task<int> TianjiawenjianjiaAsync(StorageFolder wenjianjia)
        {
            //检查
            if(wenjianjia==null)
            {
                throw new Exception("在类<Wenjianjia>方法<TianjiawenjianjiaAsync>中发生异常<<" + wenjianjia_jilu.Name + ">中传递的文件夹为空>");
            }
            try
            {
                //读取文件夹
                IReadOnlyList<StorageFolder> itemsList_ = await wenjianjia.GetFoldersAsync();
                foreach (var item in itemsList_)
                {
                    Wenjianjia linshi = new Wenjianjia(item,this);
                    liebiao.Add(linshi);
                }
                //读取文件列表
                IReadOnlyList<StorageFile> itemsList = await wenjianjia.GetFilesAsync();
                foreach (var item in itemsList)
                {
                    Wenjian linshi = new Wenjian();
                    await linshi.chushihuaAsync(item);
                    liebiao.Add(linshi);
                }

            }
            catch(Exception exc)
            {
                throw new Exception("在类<Wenjianjia>方法<TianjiawenjianjiaAsync>中发生异常<" + exc.Message + ">可能的解释是<无法读取文件或文件夹列表>",exc);
            }
            return 0;
        }
    }
    public class Wenjian:wenjian_liebiao
    {
        public StorageFile wenjian;

        public Wenjian()
        {

        }

        public async Task chushihuaAsync(StorageFile wenjian_linshi)
        {
            if(wenjian_linshi==null)
            {
                throw new Exception("文件不能为空");
            }
            else
            {
                wenjian = wenjian_linshi;
            }

            Wenjianming = wenjian.Name;
            Dizhi = wenjian.Path;
            await Huoqu_Async(wenjian);
        }
        private async Task<int> Huoqu_Async(StorageFile linshi)
        {
            try
            {
                var li = await linshi.GetBasicPropertiesAsync();
                daxiao = li.Size;
                Xiugaishijian ="修改时间："+ li.DateModified.ToString("G");
                
            }
            catch(Exception exc)
            {
                throw new Exception("在类<Wenjianjia>方法<huoqu_Async>中发生异常<" + exc.Message + ">可能的解释是<传入的文件无效或系统强制中断>", exc);
            }
            return 0;
        }
    }
}
