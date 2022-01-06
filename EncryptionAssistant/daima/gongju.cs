using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace EncryptionAssistant.daima
{
    public static class Gongju
    {
        /// <summary>
        /// 把数据进行pkcs5对齐
        /// </summary>
        /// <param name="databytes">待处理的数据</param>
        /// <returns></returns>
        public static byte[] Pkcs5(byte[] databytes, SymmetricKeyAlgorithmProvider syprd)
        {
            byte[] newBytes = null;

            int datalength = databytes.Length;
            int blocksize = (int)syprd.BlockLength;
            if (!(datalength % blocksize == 0))
            {
                int need = blocksize - (datalength % 8);
                newBytes = new byte[datalength + need];
                for (int i = 0; i < datalength; i++)
                {
                    newBytes[i] = databytes[i];
                }
                byte xx = 0x0;
                switch (need)
                {
                    case 1:
                        xx = 0x1;
                        break;
                    case 2:
                        xx = 0x2;
                        break;
                    case 3:
                        xx = 0x3;
                        break;
                    case 4:
                        xx = 0x4;
                        break;
                    case 5:
                        xx = 0x5;
                        break;
                    case 6:
                        xx = 0x6;
                        break;
                    case 7:
                        xx = 0x7;
                        break;
                }
                for (int i = 0; i < need; i++)
                {
                    newBytes[datalength + i] = xx;
                }
            }
            else
            {
                newBytes = new byte[datalength + 8];
                for (int i = 0; i < datalength; i++)
                {
                    newBytes[i] = databytes[i];
                }
                byte xx = 0x8;
                for (int i = 0; i < 8; i++)
                {
                    newBytes[datalength + i] = xx;
                }
            }
            return newBytes;
        }
        public static IBuffer SampleCipherEncryption(String strMsg, String strAlgName, UInt32 keyLength, out BinaryStringEncoding encoding, out IBuffer iv, out CryptographicKey key)
        {
            // Initialize the initialization vector.
            iv = null;

            // Initialize the binary encoding value.
            encoding = BinaryStringEncoding.Utf8;

            // Create a buffer that contains the encoded message to be encrypted. 
            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(strMsg, encoding);

            // Open a symmetric algorithm provider for the specified algorithm. 
            SymmetricKeyAlgorithmProvider objAlg = SymmetricKeyAlgorithmProvider.OpenAlgorithm(strAlgName);

            // Demonstrate how to retrieve the name of the algorithm used.
            String strAlgNameUsed = objAlg.AlgorithmName;

            // Determine whether the message length is a multiple of the block length.
            // This is not necessary for PKCS #7 algorithms which automatically pad the
            // message to an appropriate length.
            if (!strAlgName.Contains("PKCS7"))
            {
                if ((buffMsg.Length % objAlg.BlockLength) != 0)
                {
                    throw new Exception("Message buffer length must be multiple of block length.");
                }
            }

            // Create a symmetric key.
            IBuffer keyMaterial = CryptographicBuffer.GenerateRandom(keyLength);
            key = objAlg.CreateSymmetricKey(keyMaterial);

            // CBC algorithms require an initialization vector. Here, a random
            // number is used for the vector.
            if (strAlgName.Contains("CBC"))
            {
                iv = CryptographicBuffer.GenerateRandom(objAlg.BlockLength);
            }

            // Encrypt the data and return.
            IBuffer buffEncrypt = CryptographicEngine.Encrypt(key, buffMsg, iv);
            return buffEncrypt;
        }

        public static async Task<StorageFile> FuzhiwenjainAsync(StorageFolder wenjianjia, StorageFile wenjian)
        {
            if (wenjian == null || wenjianjia == null)
            {
                return null;
            }
            try
            {

                return await wenjian.CopyAsync(wenjianjia, wenjian.Name, NameCollisionOption.GenerateUniqueName);
            }
            catch
            {
                return null;
            }
        }
        //将字节数，转化成带单位的大小   样式 11GB,500MB  2  1.48GB
        public static string zhanyongkongjian(ulong gb, ulong mb, ulong kb, ulong zijie, int yangshi = 2)
        {
            switch (yangshi)
            {
                case 1:
                    return zhanyonglongjian_1(gb, mb, kb, zijie);
                case 2:
                    return zhanyongkongjian_2(gb, mb, kb, zijie);
                default:
                    break;
            }
            return "0 B";
        }
        public static string zhanyongkongjian(ulong zijie, int yangshi = 2)
        {
            if (zijie < 0)
            {
                return "无";
            }
            if (zijie == 0)
            {
                return "无";
            }
            ulong kb = 0;
            ulong mb = 0;
            ulong gb = 0;
            kb = zijie / 1024;
            zijie = zijie % 1024;
            mb = kb / 1024;
            kb = kb % 1024;
            gb = mb / 1024;
            mb = mb % 1024;
            return zhanyongkongjian(gb, mb, kb, zijie, yangshi);
        }

        private static string zhanyonglongjian_1(ulong gb, ulong mb, ulong kb, ulong zijie)
        {
            if (gb < 0 || mb < 0 || kb < 0 || zijie < 0)
            {
                return "无";
            }
            if (gb == 0 && mb == 0 && kb == 0 && zijie == 0)
            {
                return "无";
            }
            string gb_;
            string mb_;
            string kb_;
            string zijie_;
            if (gb == 0)
            {
                gb_ = "";
            }
            else
            {
                gb_ = gb.ToString() + " GB, ";

            }
            if (mb == 0)
            {
                mb_ = "";
            }
            else
            {
                mb_ = mb.ToString() + " MB, ";

            }
            if (kb == 0)
            {
                kb_ = "";
            }
            else
            {
                kb_ = kb.ToString() + " KB, ";
            }
            if (zijie == 0)
            {
                zijie_ = "";
            }
            else
            {
                zijie_ = zijie.ToString() + " B, ";
            }
            return gb_ + mb_ + kb_ + zijie_;
        }
        private static string zhanyongkongjian_2(ulong gb_, ulong mb_, ulong kb_, ulong zijie_)
        {
            double gb = gb_;
            double mb = mb_;
            double kb = kb_;
            double zijie = zijie_;
            if (gb < 0 || mb < 0 || kb < 0 || zijie < 0)
            {
                return "0 B";
            }
            if (gb == 0 && mb == 0 && kb == 0 && zijie == 0)
            {
                return "0 B";
            }
            double xiaoshu = 0;
            if (gb == 0 && mb == 0 && kb == 0)
            {
                return zijie.ToString() + " B";
            }
            else
            {
                if (gb == 0 && mb == 0)
                {
                    xiaoshu = 0;
                    xiaoshu = zijie / 1024;
                    xiaoshu = kb + xiaoshu;
                    xiaoshu = Math.Round(xiaoshu, 2, MidpointRounding.AwayFromZero);
                    return xiaoshu.ToString() + " KB";
                }
                else
                {
                    if (gb == 0)
                    {
                        xiaoshu = 0;
                        xiaoshu = zijie / 1024;
                        xiaoshu = (kb + xiaoshu) / 1024;
                        xiaoshu = mb + xiaoshu;
                        xiaoshu = Math.Round(xiaoshu, 2, MidpointRounding.AwayFromZero);
                        return xiaoshu.ToString() + " MB";
                    }
                    else
                    {
                        xiaoshu = 0;
                        xiaoshu = zijie / 1024;
                        xiaoshu = (kb + xiaoshu) / 1024;
                        xiaoshu = (mb + xiaoshu) / 1024;
                        xiaoshu = gb + xiaoshu;
                        xiaoshu = Math.Round(xiaoshu, 2, MidpointRounding.AwayFromZero);
                        return xiaoshu.ToString() + " GB";
                    }
                }
            }
        }
        //计算文件夹的大小
        public static async Task<ulong> jisuankongjian_wenjianjia(StorageFolder wenjianjia)
        {
            IReadOnlyList<StorageFile> wenjianliebiao = null;
            IReadOnlyList<StorageFolder> wenjianjialiebiao = null;
            ulong changdu = 0;
            //第一步，获取文件夹中的所有文件夹和文件
            try
            {
                wenjianliebiao = await wenjianjia.GetFilesAsync();
                if (wenjianliebiao.Count != 0)
                {
                    int jishu = 0;
                    while (jishu < wenjianliebiao.Count)
                    {
                        StorageFile linshi = await wenjianjia.GetFileAsync(wenjianliebiao[jishu].Name);
                        var li = await linshi.GetBasicPropertiesAsync();
                        changdu = li.Size + changdu;
                        jishu++;
                    }
                }
                wenjianjialiebiao = await wenjianjia.GetFoldersAsync();
                if (wenjianjialiebiao.Count != 0)
                {
                    int jishu = 0;
                    while (jishu < wenjianjialiebiao.Count)
                    {
                        StorageFolder linshi = await wenjianjia.GetFolderAsync(wenjianjialiebiao[jishu].Name);
                        ulong size = await jisuankongjian_wenjianjia(linshi);
                        changdu = size + changdu;
                        jishu++;
                    }
                }
                return changdu;
            }
            catch
            {
                return 0;
            }
        }
        //删除一个文件夹里的所有文件 不包括子文件夹
        public static async Task<string> SanchuwenjianAsync(StorageFolder wenjianjia)
        {
            try
            {
                //获取文件列表
                IReadOnlyList<StorageFile> wenjianliebiao = null;
                wenjianliebiao = await wenjianjia.GetFilesAsync();
                //删除
                int jishu = 0;
                while (wenjianliebiao.Count > jishu)
                {
                    await wenjianliebiao[jishu].DeleteAsync();
                    jishu++;
                }
                return null;
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
        }
        public static string shijianzhuanghuan(ulong miao_)
        {
            int miao =(int)( miao_ % 60);
            miao_ = miao_ / 60;
            int fengzhong = (int)(miao_ % 60);
            miao_ = miao_ / 60;
            int xiaoshi = (int)(miao_ % 24);
            miao_ = miao_ / 24;
            int tian = (int)miao_;

            return tian.ToString() + ":" + xiaoshi.ToString() + ":" + fengzhong.ToString() + ":" + miao.ToString();
        }
        /// <summary>
        /// 获取URL中的末尾文件名
        /// </summary>
        /// <param name="path">要处理的URL</param>
        /// <returns>文件名</returns>
        public static string huoquwenjainming(string path)
        {
            string result = "";
            for(int i=path.Length-1;i>=0;i--)
            {
                if(path[i]=='/')
                {
                    break;
                }
                else
                {
                    result += path[i];
                }
            }
            return fangxiangwenzi(result);
        }
        /// <summary>
        /// 将字符串倒序输出
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>倒序字符串</returns>
        public static string fangxiangwenzi(string str)
        {
            string result = "";
            for(int i=str.Length-1;i>=0;i--)
            {
                result += str[i];
            }
            return result;
        }
        /// <summary>
        /// 转化时间样式
        /// </summary>
        /// <param name="miao">时间 秒为单位</param>
        /// <param name="yangshi">样式 1 12:15:12   2  1.25 时</param>
        /// <returns>转化后的时间</returns>
        public static string zhuanghuashijian(long miao,int yangshi=2)
        {
            switch(yangshi)
            {
                case 1:
                    return zhuanghuashijian_1(miao);
                case 2:
                    return zhuanghuashijian_2(miao);
                default:
                    return "";
            }
        }
        private static string zhuanghuashijian_1(long miao)
        {
            long miao_ = miao - (miao / 60) * 60;
            long fen_ = miao / 60 - (miao / 60 / 60) * 60;
            long shi_ = miao / 3600;

            return shi_.ToString() + ":" + fen_.ToString() + ":" + miao_.ToString();
        }
        private static string zhuanghuashijian_2(long miao)
        {
            if(miao>60)
            {
                if (miao > 3600)
                {
                    if (miao > 86400)
                    {
                        return Math.Round(((double)miao / (double)86400), 2, MidpointRounding.AwayFromZero).ToString() + " 天";
                    }
                    else
                    {
                        return Math.Round( ((double)miao / (double)3600), 2, MidpointRounding.AwayFromZero).ToString() + " 时";
                    }
                }
                else
                {
                    return Math.Round(((double)miao / (double)60), 2, MidpointRounding.AwayFromZero).ToString() + " 分";
                }
            }
            else
            {
                return miao.ToString() + " 秒";
            }
        }
    }
}
