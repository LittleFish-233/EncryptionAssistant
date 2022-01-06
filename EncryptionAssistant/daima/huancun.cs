using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace EncryptionAssistant.daima
{
    public class huancun
    {
        //加密文本
        public Jiami_wenben jiami_wenben = new Jiami_wenben();
        //加密
        public Jiami jiami = new Jiami();
        //解密文本
        public Jiemi_wenben jiemi_wenben = new Jiemi_wenben();
        //解密
        public Jiemi jiemi = new Jiemi();
        //加密文件
        public Jiami_wenjian jiami_wenjian = new Jiami_wenjian();
        //解密文件
        public Jiemi_wenjian jiemi_wenjian = new Jiemi_wenjian();
        //加密/解密存档
        public Jiami_jiemi_cundan cundan = new Jiami_jiemi_cundan();
        //MD5校验
        public Md5_xiaoyan md5_Xiaoyan = new Md5_xiaoyan();
        //下载文件
        public Xiazai xiazai = new Xiazai();
        //清理
        public void qingli()
        {
            jiami_wenben.qingli();
            jiemi_wenben.qingli();
            jiami_wenjian.qingli();
            jiemi_wenjian.qingli();
            cundan.Qingli();
            md5_Xiaoyan.Qingli();
            xiazai.Qingli();
        }
    }
    //加密文件
    public class Jiami_wenjian
    {
        //文件列表
        public Wenjianjia wenjian_liebiao = new Wenjianjia();
        //当前索引
        public Wenjianjia suoying = null;
        //密码状态 0 使用 1 确认 2 重设
        public int mima_zhuangtai = 0;
        //密匙
        public string mishi = "";
        //算法
        public string suanfa = "AES";
        //密匙位数
        public string mishi_weishu = "128";
        //第一次密码储存
        public string mima_linshi = "";
        //密码输入存储
        public string mima_shuru = "";
        //加密结果存储
        public string jieguo_jiami = "";
        //是否使用默认密匙
        public bool? shifou_mishi = true;

        //提示加密
        public bool tishijiami = false;
        //记录密匙
        public bool jilumishi = false;
        //双重加密
        public bool suanchongjiami = false;
        //删除源文件
        public bool shanchuyuanwenjian = false;
        //显示高级
        public bool xianshigaoji = false;

        //保存地址
        public StorageFolder baocun_dizhi = null;
        //加密文件进度
        public Jiami_waibu jiami_jingdu = new Jiami_waibu();
        //加密文件用时
        public double shijian = 0;

        //页数  0 添加目标 1继续添加 2 填写参数 3 保存到 4确认文件列表 5 正在加密 6加密成功 7 加密失败
        public int yeshu = 0;

        //清理
        public void qingli()
        {
            wenjian_liebiao = new Wenjianjia();
            suoying = null;
            mima_zhuangtai = 0;
            mishi = "";
            suanfa = "AES";
            mishi_weishu = "128";
            mima_linshi = "";
            mima_shuru = "";
            jieguo_jiami = "";
            shifou_mishi = true;
            tishijiami = false;
            jilumishi = false;
            suanchongjiami = false;
            xianshigaoji = false;
            baocun_dizhi = null;
            shanchuyuanwenjian = false;
            shijian = 0;
            jiami_jingdu = new Jiami_waibu();
            yeshu = 0;
        }
    }
    //解密文件
    public class Jiemi_wenjian
    {
        //文件列表
        public Wenjianjia wenjian_liebiao = new Wenjianjia();
        //当前索引
        public Wenjianjia suoying = null;
        //密码状态 0 使用 1 确认 2 重设
        public int mima_zhuangtai = 0;
        //密匙
        public string mishi = "";
        //算法
        public string suanfa = "AES";
        //密匙位数
        public string mishi_weishu = "128";
        //第一次密码储存
        public string mima_linshi = "";
        //密码输入存储
        public string mima_shuru = "";
        //加密结果存储
        public string jieguo_jiemi = "";
        //是否使用默认密匙
        public bool? shifou_mishi = true;

        //删除源文件
        public bool shanchuyuanwenjian = false;
        //显示高级
        public bool xianshigaoji = false;

        //保存地址
        public StorageFolder baocun_dizhi = null;
        //解密文件进度
        public Jiemi_waibu jiemi_jingdu = new Jiemi_waibu();
        //解密文件用时
        public double shijian = 0;

        //页数  0 添加目标 1继续添加 2 填写参数 3 保存到 4确认文件列表 5 正在加密 6加密成功 7 加密失败
        public int yeshu = 0;

        //清理
        public void qingli()
        {
            wenjian_liebiao = new Wenjianjia();
            suoying = null;
            mima_zhuangtai = 0;
            mishi = "";
            suanfa = "AES";
            mishi_weishu = "128";
            mima_linshi = "";
            mima_shuru = "";
            jieguo_jiemi = "";
            shifou_mishi = true;
            xianshigaoji = false;
            baocun_dizhi = null;
            shanchuyuanwenjian = false;
            jiemi_jingdu = new Jiemi_waibu();
            shijian = 0;
            yeshu = 0;
        }
    }


    //加密文本
    public class Jiami_wenben
    {
        //密码状态 0 使用 1 确认 2 重设
        public int mima_zhuangtai = 0;
        //密匙
        public string mishi = "";
        //算法
        public string suanfa = "AES";
        //密匙位数
        public string mishi_weishu = "128";
        //第一次密码储存
        public string mima_linshi = "";
        //密码输入存储
        public string mima_shuru = "";
        //加密结果存储
        public string jieguo_jiami = "";
        //文本
        public string wenben_shuru = "";
        //是否使用默认密匙
        public bool? shifou_mishi = true;

        //清理
        public void qingli()
        {
            mima_zhuangtai = 0;
            mishi = "";
            suanfa = "AES";
            mishi_weishu = "128";
            mima_linshi = "";
            mima_shuru = "";
            jieguo_jiami = "";
            wenben_shuru = "";
        }
    }
    //加密
    public class Jiami
    {
        //显示提示 1 警告 2 事件 3 错误
        public delegate void delegateRun(string a, int xuhao);
        //定义一个事件
        public event delegateRun Xianshitishi;

        //开始提示
        public void Kaishitishi(string a, int xuhao)
        {
            Xianshitishi?.Invoke(a, xuhao);
        }

    }

    //解密文本
    public class Jiemi_wenben
    {
        //密码状态 0 使用 1 确认 2 重设
        public int mima_zhuangtai = 0;
        //密匙
        public string mishi = "";
        //算法
        public string suanfa = "AES";
        //密匙位数
        public string mishi_weishu = "128";
        //第一次密码储存
        public string mima_linshi = "";
        //密码输入存储
        public string mima_shuru = "";
        //解密结果存储
        public string jieguo_jiemi = "";
        //文本输入
        public string wenben_shuru = "";
        //是否使用默认密匙
        public bool? shifou_mishi = true;

        //清理
        public void qingli()
        {
            mima_zhuangtai = 0;
            mishi = "";
            suanfa = "AES";
            mishi_weishu = "128";
            mima_linshi = "";
            mima_shuru = "";
            jieguo_jiemi = "";
            wenben_shuru = "";
        }
    }
    //解密
    public class Jiemi
    {
        //显示提示 1 警告 2 事件 3 错误
        public delegate void delegateRun(string a, int xuhao);
        //定义一个事件
        public event delegateRun Xianshitishi;

        //开始提示
        public void Kaishitishi(string a, int xuhao)
        {
            Xianshitishi?.Invoke(a, xuhao);
        }

    }
    //加密/解密存档
    public class Jiami_jiemi_cundan
    {
        public string wenjian_1 = "";
        public string wenjian_2 = "";
        public bool wenjian_shifouzidingyimishi = false;
        public string wenjian_mishi = "";
        public int wenjian_suanfa = 0;
        public int wenjian_moshi = 0;
        public string wenjian_ming1 = "";
        public StorageFile wenjian_wenjain = null;
        public StorageFile wenjain_jieguo = null;
        //显示提示 1 警告 2 事件 3 错误
        public delegate void delegateRun(string a, int xuhao);
        //定义一个事件
        public event delegateRun Xianshitishi;
        //开始提示
        public void Kaishitishi(string a, int xuhao)
        {
            Xianshitishi?.Invoke(a, xuhao);
        }

        public void Qingli()
        {
            wenjian_1 = "";
            wenjian_2 = "";
            wenjian_shifouzidingyimishi = false;
            wenjian_mishi = "";
            wenjian_suanfa = 0;
            wenjian_moshi = 0;
            wenjian_ming1 = "";
            wenjian_wenjain = null;
            wenjain_jieguo = null;
        }
    }
    //md5校验
    public class Md5_xiaoyan
    {
        //显示提示 1 警告 2 事件 3 错误
        public delegate void delegateRun(string a, int xuhao);
        //定义一个事件
        public event delegateRun Xianshitishi;
        //文本算法
        public string suanfa_wenben = "Md5";
        //文本输入
        public string wenben_shuru = "";
        //文件算法
        public string suanfa_wenjian = "Md5";
        //结果 文本
        public string jieguo_wenben = "";
        //结果 文件
        public string jieguo_wenjian = "";
        //文件选择
        public StorageFile wenjian_xuanzhe = null;
        //开始提示
        public void Kaishitishi(string a, int xuhao)
        {
            Xianshitishi?.Invoke(a, xuhao);
        }
        //清理
        public void Qingli()
        {
            suanfa_wenben = "Md5";
            suanfa_wenjian = "Md5";
            wenben_shuru = "";
            jieguo_wenben = "";
            jieguo_wenjian = "";
            wenjian_xuanzhe = null;
        }
    }
    //下载文件
    public class Xiazai
    {
        //显示提示 1 警告 2 事件 3 错误
        public delegate void delegateRun(string a, int xuhao);
        //定义一个事件
        public event delegateRun Xianshitishi;
        //下载任务管理
        public Xiazaiguangli xiazai_guanli = new Xiazaiguangli();
        //选中的文件夹
        public StorageFolder linshi_wenjainjia = null;
        //下载方式
        public int fangshi = -1;
        //下载地址
        public string url = "";
        //开始提示
        public void Kaishitishi(string a, int xuhao)
        {
            Xianshitishi?.Invoke(a, xuhao);
        }
        //清理
        public void Qingli()
        {
            xiazai_guanli = new Xiazaiguangli();
            linshi_wenjainjia = null;
            fangshi = -1;
            url = "";
        }
    }

}
