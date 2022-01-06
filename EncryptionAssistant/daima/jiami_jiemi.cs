using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace EncryptionAssistant.daima
{
    public static class Jiami_jiemi
    {

        //加 第一步 
        //通过SymmetricKeyAlgorithmProvider的静态方法OpenAlgorithm()得到一个SymmetricKeyAlgorithmProvider实例
        //该方法参数是要使用的加/解密算法的名字
        // private static SymmetricKeyAlgorithmProvider syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.TripleDesEcb);

        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="data">需要加密的字符串</param>
        /// <returns>返回加密后的结果</returns>
        public static string Jiami(string data, string key, SymmetricKeyAlgorithmProvider syprd)
        {
            string encryptedData = null;
            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                //这里我自己写了一个pkcs5对齐，不过后来有看到过C#pkcs5和pkcs7是一样的说法，有待验证
                byte[] pkcs5databytes = Gongju.Pkcs5(dataBytes, syprd);
                IBuffer databuffer = pkcs5databytes.AsBuffer();

                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                IBuffer keybuffer = keyBytes.AsBuffer();
                //构造CryptographicKey
                CryptographicKey cryptographicKey = syprd.CreateSymmetricKey(keybuffer);
                //加密
                //Encrypt需要三个参数分别为加密使用的Key，需要加密的Data，以及向量IV
                //Des加密中Ecb模式和Cbc模式的区别就在于是否必须使用向量IV
                IBuffer cryptBuffer = CryptographicEngine.Encrypt(cryptographicKey, databuffer, null);
                byte[] enctyptedBytes = cryptBuffer.ToArray();
                //进行base64编码
                encryptedData = Convert.ToBase64String(enctyptedBytes);
            }
            catch
            {
                //DebugHelper.Log("TripleDesEncryptHelper.Encrypt", ex.Message);
                return null;
            }
            return encryptedData.Trim();
        }
        //核心
        private static IBuffer Jiami_cun(IBuffer buffer, SymmetricKeyAlgorithmProvider Syprd, string mishi)
        {
            //读取文件
            byte[] dataBytes = buffer.ToArray();

            //这里我自己写了一个pkcs5对齐，不过后来有看到过C#pkcs5和pkcs7是一样的说法，有待验证
            dataBytes = Gongju.Pkcs5(dataBytes, Syprd);
            buffer = dataBytes.AsBuffer();
            dataBytes = null;

            //修正密匙
            byte[] keyBytes = Encoding.UTF8.GetBytes(mishi);
            IBuffer keybuffer = keyBytes.AsBuffer();
            keyBytes = null;

            //构造CryptographicKey
            CryptographicKey cryptographicKey = Syprd.CreateSymmetricKey(keybuffer);
            keybuffer = null;
            GC.Collect();

            //加密
            //Encrypt需要三个参数分别为加密使用的Key，需要加密的Data，以及向量IV
            //Des加密中Ecb模式和Cbc模式的区别就在于是否必须使用向量IV
            buffer = CryptographicEngine.Encrypt(cryptographicKey, buffer, null);

            return buffer;
        }
        //文件加密 外部 负责协调参数
        public static async Task<int> Jiami_wenjianAsync(StorageFile wenjian, StorageFolder dizhi, bool jilumishi, bool tishi, bool suanchong, bool sanchuyuanwenjian, string mishi, string suanfa, string mima)
        {
            //初始化算法
            SymmetricKeyAlgorithmProvider syprd = null;
            switch (suanfa)
            {
                case "AES"://默认算法
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
                    break;
                case "DES":
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.DesEcbPkcs7);
                    break;
                case "RC2":
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc2EcbPkcs7);
                    break;
                case "RC4":
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc4);
                    break;
                case "3_DES":
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.TripleDesEcbPkcs7);
                    break;
            }
            if (suanchong == true)
            {
                //调用基本算法
                StorageFile linshi = await Jiami_jieben_wenjianAsync(wenjian, jilumishi, tishi, mishi, mima, suanfa, syprd, ApplicationData.Current.TemporaryFolder);
                //调用双重加密
                await jiami_gaoji_wenjianAsync(linshi, dizhi, tishi);
                //删除临时文件
                await linshi.DeleteAsync(StorageDeleteOption.PermanentDelete);

            }
            else
            {
                await Jiami_jieben_wenjianAsync(wenjian, jilumishi, tishi, mishi, mima, suanfa, syprd, dizhi);
            }
            //删除源文件
            if (sanchuyuanwenjian == true)
            {
                await wenjian.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            return 0;
        }
        //基本加密
        private static async Task<StorageFile> Jiami_jieben_wenjianAsync(StorageFile wenjian, bool jilumishi, bool tishi, string mishi, string mima, string suanfa, SymmetricKeyAlgorithmProvider syprd, StorageFolder wenjianjia)
        {
            //检查
            if (wenjian == null || wenjianjia == null)
            {
                throw new Exception("文件或地址不能为空。");
            }
            //读取文件
            IBuffer buffer = await FileIO.ReadBufferAsync(wenjian);
            int weishu_bu = (int)buffer.Length;      //文件的位数
            //调用加密
            try
            {
                buffer = Jiami_cun(buffer, syprd, mishi);
                GC.Collect();
            }
            catch (Exception exc)
            {
                throw new Exception("文件太大无法加密<" + exc.Message + ">");
            }

            //创建文件
            string wenjianming = wenjian.DisplayName;
            if (tishi == true)
            {
                wenjianming = wenjian.DisplayName + ".lfjia";
            }
            else
            {
                wenjianming = wenjian.Name;
            }
            StorageFile wenjian_new = await wenjianjia.CreateFileAsync(wenjianming, CreationCollisionOption.GenerateUniqueName);

            //写入版本号
            var stream = await wenjian_new.OpenAsync(FileAccessMode.ReadWrite);
            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new DataWriter(outputStream))
                {
                    //写入版本
                    dataWriter.WriteString("##");

                    //写入版本号写入附加  1:1.4.12.0  2:1.5.21.0单重 3:1.5.21.0双重
                    dataWriter.WriteInt32(2);

                    //密码
                    dataWriter.WriteInt32(mima.Length);
                    string mima_xin = Jiami(mima, mishi, syprd);
                    dataWriter.WriteUInt32((uint)mima_xin.Length);
                    dataWriter.WriteString(mima_xin);

                    //是否记录加密信息
                    dataWriter.WriteBoolean(jilumishi);
                    if (jilumishi == true)
                    {
                        //算法
                        dataWriter.WriteUInt32((uint)Encoding.UTF8.GetBytes(suanfa.ToCharArray()).Length);
                        dataWriter.WriteString(suanfa);
                        //密匙
                        dataWriter.WriteUInt32((uint)Encoding.UTF8.GetBytes(mishi.ToCharArray()).Length);
                        dataWriter.WriteString(mishi);
                    }

                    //原文件名称
                    dataWriter.WriteUInt32((uint)Encoding.UTF8.GetBytes(wenjian.Name.ToCharArray()).Length);
                    dataWriter.WriteString(wenjian.Name);


                    //写入加密后的文件
                    dataWriter.WriteInt32(weishu_bu);//原文件位数
                    dataWriter.WriteUInt32(buffer.Length);
                    dataWriter.WriteBuffer(buffer);

                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
            stream.Dispose(); // Or use the stream variable (see previous code snippet) with a 

            return wenjian_new;
        }
        //双重加密
        private static async Task<int> jiami_gaoji_wenjianAsync(StorageFile wenjian, StorageFolder dizhi, bool tishi)
        {
            //确定算法和密匙
            string mishi = "AA1245FFAA1245FF";
            SymmetricKeyAlgorithmProvider syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            //检查
            if (wenjian == null || dizhi == null)
            {
                throw new Exception("文件或地址不能为空。");
            }
            //读取文件
            IBuffer buffer = await FileIO.ReadBufferAsync(wenjian);
            int weishu_bu = (int)buffer.Length;      //文件的位数
            //调用加密
            try
            {
                buffer = Jiami_cun(buffer, syprd, mishi);
                GC.Collect();
            }
            catch (Exception exc)
            {
                throw new Exception("文件太大无法加密<" + exc.Message + ">");
            }

            //创建文件
            string wenjianming = wenjian.DisplayName;
            if (tishi == true)
            {
                wenjianming = wenjian.DisplayName + ".lfjia";
            }
            else
            {
                wenjianming = wenjian.Name;
            }
            StorageFile wenjian_new = await dizhi.CreateFileAsync(wenjianming, CreationCollisionOption.GenerateUniqueName);

            //写入版本号
            var stream = await wenjian_new.OpenAsync(FileAccessMode.ReadWrite);
            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new DataWriter(outputStream))
                {
                    //写入版本
                    dataWriter.WriteString("##");

                    //写入版本号写入附加  1:1.4.12.0  2:1.5.21.0单重 3:1.5.21.0双重
                    dataWriter.WriteInt32(3);

                    //写入加密后的文件
                    dataWriter.WriteInt32(weishu_bu);//原文件位数
                    dataWriter.WriteUInt32(buffer.Length);
                    dataWriter.WriteBuffer(buffer);

                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
            stream.Dispose(); // Or use the stream variable (see previous code snippet) with a 

            return 0;
        }


        /// <summary>
        /// 解密函数
        /// </summary>
        /// <param name="data">待解密的字符串</param>
        /// <returns>解密后的数据</returns>
        public static string Jiemi(string data, string key, SymmetricKeyAlgorithmProvider syprd)
        {
            string decryptedData = null;
            try
            {
                byte[] dataBytes = Convert.FromBase64String(data);
                IBuffer dataBuffer = dataBytes.AsBuffer();
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                IBuffer keybuffer = keyBytes.AsBuffer();
                CryptographicKey cryptographicKey = syprd.CreateSymmetricKey(keybuffer);
                IBuffer decryptedBuffer = CryptographicEngine.Decrypt(cryptographicKey, dataBuffer, null);
                decryptedData = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, decryptedBuffer);
                //防止乱码
                //var result = System.Text.RegularExpressions.Regex.Match(decryptedData, "[a-zA-Z0-9]*");
                //decryptedData.Replace("\u0005", " ");
                //decryptedData = result.ToString();
            }
            catch
            {
                // DebugHelper.Log("TripleDesEncryptHelper.Decrypt", ex.Message);
                return null;
            }
            return decryptedData;
        }

        //核心解密
        private static IBuffer Jiemi_cun(IBuffer buffer, SymmetricKeyAlgorithmProvider Syprd, string mishi)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(mishi);
            IBuffer keybuffer = keyBytes.AsBuffer();
            CryptographicKey cryptographicKey = Syprd.CreateSymmetricKey(keybuffer);
            IBuffer decryptedBuffer = CryptographicEngine.Decrypt(cryptographicKey, buffer, null);

            //清理
            GC.Collect();
            return decryptedBuffer;
        }
        //文件解密 外部 负责协调参数
        public static async Task<int> Jiemi_wenjianAsync(StorageFile wenjian, StorageFolder dizhi, bool sanchuyuanwenjian, string mishi, string suanfa, string mima)
        {
            //初始化算法
            SymmetricKeyAlgorithmProvider syprd = null;
            switch (suanfa)
            {
                case "AES"://默认算法
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
                    break;
                case "DES":
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.DesEcbPkcs7);
                    break;
                case "RC2":
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc2EcbPkcs7);
                    break;
                case "RC4":
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc4);
                    break;
                case "3_DES":
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.TripleDesEcbPkcs7);
                    break;
            }

            await Jiami_jieben_wenjianAsync(wenjian, mishi, mima, suanfa, syprd, dizhi);
            //删除源文件
            if (sanchuyuanwenjian == true)
            {
                await wenjian.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            return 0;
        }
        //解密 读取版本号
        private static async Task<int> Jiami_jieben_wenjianAsync(StorageFile wenjian, string mishi, string mima, string suanfa, SymmetricKeyAlgorithmProvider syprd, StorageFolder dizhi)
        {
            int banben = 0;
            //读取文件
            var stream = await wenjian.OpenAsync(Windows.Storage.FileAccessMode.Read);
            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                {
                    //读取新旧版本
                    uint numBytesLoaded = await dataReader.LoadAsync(2);
                    string text = dataReader.ReadString(numBytesLoaded);

                    if (text != "##")
                    {
                        banben = 0;
                    }
                    else
                    {
                        //读取版本号
                        await dataReader.LoadAsync(sizeof(int));
                        banben = dataReader.ReadInt32();
                    }
                }
            }

            //按版本跳转
            switch (banben)
            {
                case 0:
                    throw new Exception("当前文件没有被加密，或加密该文件应用版本低于1.4.12，请访问帮助中心");
                case 1:
                    guoshi.Renwu_xinxi linshi = new guoshi.Renwu_xinxi();
                    linshi.mima = mima;
                    linshi.mishi = mishi;
                    //转换算法
                    switch (suanfa)
                    {
                        case "AES"://默认算法
                            linshi.suanfa = 0;
                            break;
                        case "DES":
                            linshi.suanfa = 1;
                            break;
                        case "RC2":
                            linshi.suanfa = 2;
                            break;
                        case "RC4":
                            linshi.suanfa = 3;
                            break;
                        case "3_DES":
                            linshi.suanfa = 4;
                            break;
                    }
                    await Jiemi_1_4_12Async(wenjian, linshi, dizhi);
                    break;
                case 2:
                    await Jiemi_2_1_5_21Async(wenjian,mima,mishi,suanfa,dizhi);
                    break;
                case 3:
                    await Jiemi_3_1_5_21Async(wenjian, mima, mishi, suanfa, dizhi);
                    break;
            }


            return 1;
        }
        //解密 1 1.4.12
        private static async Task<int> Jiemi_1_4_12Async(StorageFile wenjian,guoshi.Renwu_xinxi xinxi, StorageFolder wenjianjia)
        {
            int weishu = 0;                //解密后的密码位数
            string mima = "";              //解密后的密码
            string mima_xin = "";          //加密的密码
            string wenjianming = "";       //文件名
            string mishi = xinxi.mishi;    //密匙
            int suanfa = xinxi.suanfa;     //算法
            bool panduan = false;          //是否记录详细信息
            IBuffer buffer;                //要解密的文件
            int weishu_bu = 0;             //源文件的位数
                                           //继续读取
            var stream = await wenjian.OpenAsync(Windows.Storage.FileAccessMode.Read);
            try
            {
                using (var inputStream = stream.GetInputStreamAt(2 + sizeof(int)))
                {
                    using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                    {
                        //读取密码
                        await dataReader.LoadAsync(sizeof(int));
                        weishu = dataReader.ReadInt32();

                        await dataReader.LoadAsync(sizeof(uint));
                        uint weishu_xin = dataReader.ReadUInt32();

                        await dataReader.LoadAsync(weishu_xin);
                        mima_xin = dataReader.ReadString(weishu_xin);

                        //是否记录加密信息
                        await dataReader.LoadAsync(sizeof(bool));
                        panduan = dataReader.ReadBoolean();
                        if (panduan == true)
                        {
                            await dataReader.LoadAsync(sizeof(int));
                            suanfa = dataReader.ReadInt32();

                            await dataReader.LoadAsync(sizeof(uint));
                            weishu_xin = dataReader.ReadUInt32();

                            await dataReader.LoadAsync(weishu_xin);
                            mishi = dataReader.ReadString(weishu_xin);
                        }

                        //原来的文件名
                        await dataReader.LoadAsync(sizeof(uint));
                        uint changdu = dataReader.ReadUInt32();

                        await dataReader.LoadAsync(changdu);
                        wenjianming = dataReader.ReadString(changdu);

                        //读取加密后的文件
                        await dataReader.LoadAsync(sizeof(int));
                        weishu_bu = dataReader.ReadInt32();

                        await dataReader.LoadAsync(sizeof(uint));
                        changdu = dataReader.ReadUInt32();

                        await dataReader.LoadAsync(changdu);
                        buffer = dataReader.ReadBuffer(changdu);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception("文件被损坏<" + exc.Message + ">");
            }

            //修改密匙
            SymmetricKeyAlgorithmProvider linshi = null;
            switch (suanfa)
            {
                case 0://默认算法
                    linshi = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
                    break;
                case 1:
                    linshi = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.DesEcbPkcs7);
                    break;
                case 2:
                    linshi = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc2EcbPkcs7);
                    break;
                case 3:
                    linshi = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc4);
                    break;
                case 4:
                    linshi = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.TripleDesEcbPkcs7);
                    break;

            }

            //解密密码
            string linshi_1 = Jiemi(mima_xin, mishi, linshi);
            if (linshi_1 == null)
            {
                throw new Exception("解密算法或密匙错误");
            }
            for (int i = 0; i < weishu; i++)
            {
                mima += linshi_1[i];
            }

            //对比密码
            if (mima != xinxi.mima)
            {
                throw new Exception("密码错误");
            }

            //解密文件
            try
            {
                buffer = Jiemi_cun(buffer,linshi,mishi);
            }
            catch (Exception exc)
            {
                throw new Exception("此文件不是被加密的文件，或者文件太大<" + exc.Message + ">");
            }
            GC.Collect();

            //写入文件
            StorageFile wenjian_xin = await wenjianjia.CreateFileAsync(wenjianming, CreationCollisionOption.GenerateUniqueName);
            stream = await wenjian_xin.OpenAsync(FileAccessMode.ReadWrite);
            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new DataWriter(outputStream))
                {
                    dataWriter.WriteBuffer(buffer, 0, (uint)weishu_bu);

                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }

            }
            stream.Dispose(); // Or use the stream variable (see previous code snippet) with a 
            return 1;
        }
        //解密 2 1.5.21
        private static async Task<int> Jiemi_2_1_5_21Async(StorageFile wenjian,string mima_,string mishi,string suanfa, StorageFolder wenjianjia)
        {
            int weishu = 0;                //解密后的密码位数
            string mima = "";              //解密后的密码
            string mima_xin = "";          //加密的密码
            string wenjianming = "";       //文件名
            bool panduan = false;          //是否记录详细信息
            IBuffer buffer;                //要解密的文件
            int weishu_bu = 0;             //源文件的位数
                                           //继续读取
            var stream = await wenjian.OpenAsync(Windows.Storage.FileAccessMode.Read);
            try
            {
                using (var inputStream = stream.GetInputStreamAt(2 + sizeof(int)))
                {
                    using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                    {
                        //读取密码
                        await dataReader.LoadAsync(sizeof(int));
                        weishu = dataReader.ReadInt32();

                        await dataReader.LoadAsync(sizeof(uint));
                        uint weishu_xin = dataReader.ReadUInt32();

                        await dataReader.LoadAsync(weishu_xin);
                        mima_xin = dataReader.ReadString(weishu_xin);

                        //是否记录加密信息
                        await dataReader.LoadAsync(sizeof(bool));
                        panduan = dataReader.ReadBoolean();
                        if (panduan == true)
                        {
                            //算法
                            await dataReader.LoadAsync(sizeof(uint));
                            weishu_xin = dataReader.ReadUInt32();

                            await dataReader.LoadAsync(weishu_xin);
                            suanfa = dataReader.ReadString(weishu_xin);
                            //密匙
                            await dataReader.LoadAsync(sizeof(uint));
                            weishu_xin = dataReader.ReadUInt32();

                            await dataReader.LoadAsync(weishu_xin);
                            mishi = dataReader.ReadString(weishu_xin);
                        }

                        //原来的文件名
                        await dataReader.LoadAsync(sizeof(uint));
                        uint changdu = dataReader.ReadUInt32();

                        await dataReader.LoadAsync(changdu);
                        wenjianming = dataReader.ReadString(changdu);

                        //读取加密后的文件
                        await dataReader.LoadAsync(sizeof(int));
                        weishu_bu = dataReader.ReadInt32();

                        await dataReader.LoadAsync(sizeof(uint));
                        changdu = dataReader.ReadUInt32();

                        await dataReader.LoadAsync(changdu);
                        buffer = dataReader.ReadBuffer(changdu);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception("文件被损坏<" + exc.Message + ">");
            }

            //修改密匙
            SymmetricKeyAlgorithmProvider linshi = null;
            switch (suanfa)
            {
                case "AES"://默认算法
                    linshi = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
                    break;
                case "DES":
                    linshi = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.DesEcbPkcs7);
                    break;
                case "RC2":
                    linshi = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc2EcbPkcs7);
                    break;
                case "RC4":
                    linshi = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc4);
                    break;
                case "3_DES":
                    linshi = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.TripleDesEcbPkcs7);
                    break;
            }

            //解密密码
            string linshi_1 = Jiemi(mima_xin, mishi, linshi);
            if (linshi_1 == null)
            {
                throw new Exception("解密算法或密匙错误");
            }
            for (int i = 0; i < weishu; i++)
            {
                mima += linshi_1[i];
            }

            //对比密码
            if (mima != mima_)
            {
                throw new Exception("密码错误");
            }

            //解密文件
            try
            {
                buffer = Jiemi_cun(buffer, linshi,mishi);
            }
            catch (Exception exc)
            {
                throw new Exception("此文件不是被加密的文件，或者文件太大<" + exc.Message + ">");
            }
            GC.Collect();

            //写入文件
            StorageFile wenjian_xin = await wenjianjia.CreateFileAsync(wenjianming, CreationCollisionOption.GenerateUniqueName);
            stream = await wenjian_xin.OpenAsync(FileAccessMode.ReadWrite);
            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new DataWriter(outputStream))
                {
                    dataWriter.WriteBuffer(buffer, 0, (uint)weishu_bu);

                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }

            }
            stream.Dispose(); // Or use the stream variable (see previous code snippet) with a 
            return 1;
        }
        //解密 3 1.5.21
        private static async Task<int> Jiemi_3_1_5_21Async(StorageFile wenjian, string mima_, string mishi, string suanfa, StorageFolder wenjianjia)
        {
            int wenjian_weishu = 0; //原文件位数
            uint weishu_bu = 0;      //加密的缓冲区位数
            IBuffer buffer;         //加密的缓冲区
            
            string mishi_tongyi = "AA1245FFAA1245FF";//确定算法和密匙
            SymmetricKeyAlgorithmProvider syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            //检查
            if (wenjian == null || wenjianjia == null)
            {
                throw new Exception("文件或地址不能为空。");
            }
            //读取文件
            var stream = await wenjian.OpenAsync(Windows.Storage.FileAccessMode.Read);
            try
            {
                using (var inputStream = stream.GetInputStreamAt(2 + sizeof(int)))
                {
                    using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                    {
                        //原文件位数
                        await dataReader.LoadAsync(sizeof(int));
                        int weishu = dataReader.ReadInt32();
                        //加密的缓冲区位数
                        await dataReader.LoadAsync(sizeof(uint));
                        weishu_bu = dataReader.ReadUInt32();
                        //加密部分
                        await dataReader.LoadAsync(weishu_bu);
                        buffer = dataReader.ReadBuffer(weishu_bu);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception("文件被损坏<" + exc.Message + ">");
            }
            //调用解密
            try
            {
                buffer = Jiemi_cun(buffer, syprd, mishi_tongyi);
                GC.Collect();
            }
            catch (Exception exc)
            {
                throw new Exception("文件太大无法解密<" + exc.Message + ">");
            }
            //创建一个临时文件
            StorageFile wenjian_new = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("123", CreationCollisionOption.GenerateUniqueName);
            //写入版本号
            stream = await wenjian_new.OpenAsync(FileAccessMode.ReadWrite);
            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new DataWriter(outputStream))
                {
                    //写入解密后的文件
                    dataWriter.WriteBuffer(buffer,0,(uint)wenjian_weishu);

                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
            stream.Dispose(); // Or use the stream variable (see previous code snippet) with a 
            //调用基本解密
            await Jiemi_2_1_5_21Async(wenjian_new, mima_, mishi, suanfa, wenjianjia);
            //删除临时文件
            await wenjian_new.DeleteAsync();

            return 1;
        }
    }
}
