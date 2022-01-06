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

namespace EncryptionAssistant.daima.guoshi
{
    public static class Jiami_jiemi
    {

        //加/解密 第一步 
        //通过SymmetricKeyAlgorithmProvider的静态方法OpenAlgorithm()得到一个SymmetricKeyAlgorithmProvider实例
        //该方法参数是要使用的加/解密算法的名字
        // private static SymmetricKeyAlgorithmProvider syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.TripleDesEcb);
        private static int banbenhao_putong = 0;

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
                byte[] pkcs5databytes = Pkcs5(dataBytes, syprd);
                IBuffer databuffer = pkcs5databytes.AsBuffer();

                byte[] keyBytes = Convert.FromBase64String(key);
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
        public static async Task<StorageFile> JiamiAsync(StorageFile wenjian, string key, int leixing, int suanfa, string mima = "")
        {
            SymmetricKeyAlgorithmProvider syprd = null;
            switch (suanfa)
            {
                case 0://默认算法
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.TripleDesEcb);
                    break;
                case 1:
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.DesCbc);

                    break;
                case 2:
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc2Cbc);
                    break;
                case 3:
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc2Ecb);
                    break;
                case 4:
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc4);
                    break;
                case 5:
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.TripleDesEcbPkcs7);
                    break;
                default:
                    return null;
            }
            switch (leixing)
            {
                case 0:
                    return await Jiami_yuanshiAsync(wenjian, key, syprd);
                case 1:
                    return await Jiami_putongAsync(wenjian, key, mima, syprd);
                default:
                    return null;
            }
        }

        public static async Task<StorageFile> Jiami_yuanshiAsync(StorageFile wenjian, string key, SymmetricKeyAlgorithmProvider syprd)
        {
            if (wenjian == null)
            {
                return null;
            }
            try
            {
                StorageFolder wenjianjia = ApplicationData.Current.TemporaryFolder;//LCT


                IBuffer buffer = await FileIO.ReadBufferAsync(wenjian);
                byte[] dataBytes = buffer.ToArray();
                buffer = null;
                //这里我自己写了一个pkcs5对齐，不过后来有看到过C#pkcs5和pkcs7是一样的说法，有待验证
                dataBytes = Pkcs5(dataBytes, syprd);
                buffer = dataBytes.AsBuffer();
                dataBytes = null;

                byte[] keyBytes = Convert.FromBase64String(key);
                IBuffer keybuffer = keyBytes.AsBuffer();
                keyBytes = null;
                //构造CryptographicKey
                CryptographicKey cryptographicKey = syprd.CreateSymmetricKey(keybuffer);
                keybuffer = null;
                //加密
                //Encrypt需要三个参数分别为加密使用的Key，需要加密的Data，以及向量IV
                //Des加密中Ecb模式和Cbc模式的区别就在于是否必须使用向量IV
                buffer = CryptographicEngine.Encrypt(cryptographicKey, buffer, null);

                //创建新文件
                wenjian = await wenjianjia.CreateFileAsync(wenjian.Name, CreationCollisionOption.ReplaceExisting);
                //写入文件
                await FileIO.WriteBufferAsync(wenjian, buffer);

                buffer = null;
                return wenjian;
            }
            catch
            {
                return null;
            }

        }
        public static async Task<StorageFile> Jiami_putongAsync(StorageFile wenjian, string key, string mima, SymmetricKeyAlgorithmProvider syprd)
        {
            if (wenjian == null)
            {
                return null;
            }
            try
            {
                StorageFolder wenjianjia = ApplicationData.Current.TemporaryFolder;//LCT

                IBuffer buffer = await FileIO.ReadBufferAsync(wenjian);
                byte[] dataBytes = buffer.ToArray();
                //这里我自己写了一个pkcs5对齐，不过后来有看到过C#pkcs5和pkcs7是一样的说法，有待验证
                byte[] pkcs5databytes = Pkcs5(dataBytes, syprd);
                IBuffer databuffer = pkcs5databytes.AsBuffer();

                byte[] keyBytes = Convert.FromBase64String(key);
                IBuffer keybuffer = keyBytes.AsBuffer();
                //构造CryptographicKey
                CryptographicKey cryptographicKey = syprd.CreateSymmetricKey(keybuffer);
                //加密
                //Encrypt需要三个参数分别为加密使用的Key，需要加密的Data，以及向量IV
                //Des加密中Ecb模式和Cbc模式的区别就在于是否必须使用向量IV
                IBuffer cryptBuffer = CryptographicEngine.Encrypt(cryptographicKey, databuffer, null);

                //写入文件信息
                using (InMemoryRandomAccessStream memorystream = new InMemoryRandomAccessStream())
                {
                    using (DataWriter datawriter = new DataWriter(memorystream))
                    {
                        datawriter.WriteInt32(banbenhao_putong);//1

                        datawriter.WriteInt32(Encoding.UTF8.GetByteCount(mima));
                        datawriter.WriteString(mima);//2

                        datawriter.WriteUInt32(cryptBuffer.Length);
                        datawriter.WriteBuffer(cryptBuffer);//3
                        //整合文件信息
                        buffer = datawriter.DetachBuffer();

                    }
                }

                //创建新文件
                wenjian = await wenjianjia.CreateFileAsync(wenjian.Name, CreationCollisionOption.ReplaceExisting);
                //写入文件
                await FileIO.WriteBufferAsync(wenjian, buffer);

                return wenjian;
            }
            catch
            {
                return null;
            }
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
                byte[] keyBytes = Convert.FromBase64String(key);
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
        public static async Task<StorageFile> JiemiAsync(StorageFile wenjian, string key, int leixing, int suanfa, string mima = "")
        {
            SymmetricKeyAlgorithmProvider syprd = null;
            switch (suanfa)
            {
                case 0://默认算法
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.TripleDesEcb);
                    break;
                case 1:
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.DesCbc);

                    break;
                case 2:
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc2Cbc);
                    break;
                case 3:
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc2Ecb);
                    break;
                case 4:
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.Rc4);
                    break;
                case 5:
                    syprd = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.TripleDesEcbPkcs7);
                    break;
                default:
                    return null;
            }

            switch (leixing)
            {
                case 0:
                    return await Jiemi_yuanshiAsync(wenjian, key, syprd);
                case 1:
                    return await Jiemi_putongAsync(wenjian, key, mima, syprd);
                default:
                    return null;
            }
        }

        public static async Task<StorageFile> Jiemi_yuanshiAsync(StorageFile wenjian, string key, SymmetricKeyAlgorithmProvider syprd)
        {
            if (wenjian == null)
            {
                return null;
            }
            try
            {
                StorageFolder wenjianjia = ApplicationData.Current.TemporaryFolder;//LCT



                IBuffer dataBuffer = await FileIO.ReadBufferAsync(wenjian);
                byte[] keyBytes = Convert.FromBase64String(key);
                IBuffer keybuffer = keyBytes.AsBuffer();
                CryptographicKey cryptographicKey = syprd.CreateSymmetricKey(keybuffer);
                IBuffer decryptedBuffer = CryptographicEngine.Decrypt(cryptographicKey, dataBuffer, null);

                //创建新文件
                wenjian = await wenjianjia.CreateFileAsync(wenjian.Name, CreationCollisionOption.ReplaceExisting);
                //写入文件
                await FileIO.WriteBufferAsync(wenjian, decryptedBuffer);

                return wenjian;
            }
            catch
            {
                return null;
            }

        }
        public static async Task<StorageFile> Jiemi_putongAsync(StorageFile wenjian, string key, string mima, SymmetricKeyAlgorithmProvider syprd)
        {
            if (wenjian == null)
            {
                return null;
            }
            try
            {
                StorageFolder wenjianjia = ApplicationData.Current.TemporaryFolder;//LCT
                string wenjianmima = "";
                IBuffer dataBuffer = await FileIO.ReadBufferAsync(wenjian);

                //读取文件信息
                using (DataReader datareader = DataReader.FromBuffer(dataBuffer))
                {
                    int banben = datareader.ReadInt32();
                    switch (banben)
                    {
                        case 0:
                            int stringsize = datareader.ReadInt32();
                            wenjianmima = datareader.ReadString((uint)stringsize);//2

                            uint changdu = datareader.ReadUInt32();
                            dataBuffer = datareader.ReadBuffer(changdu);//3

                            break;
                        default:
                            return null;
                    }
                }

                //核对密码
                if (mima != wenjianmima)
                {
                    return null;
                }
                byte[] keyBytes = Convert.FromBase64String(key);
                IBuffer keybuffer = keyBytes.AsBuffer();
                CryptographicKey cryptographicKey = syprd.CreateSymmetricKey(keybuffer);
                IBuffer decryptedBuffer = CryptographicEngine.Decrypt(cryptographicKey, dataBuffer, null);


                //创建新文件
                wenjian = await wenjianjia.CreateFileAsync(wenjian.Name, CreationCollisionOption.ReplaceExisting);
                //写入文件
                await FileIO.WriteBufferAsync(wenjian, decryptedBuffer);

                return wenjian;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 把数据进行pkcs5对齐
        /// </summary>
        /// <param name="databytes">待处理的数据</param>
        /// <returns></returns>
        private static byte[] Pkcs5(byte[] databytes, SymmetricKeyAlgorithmProvider syprd)
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

        /// <summary>
        /// 消息摘要（字符串）
        /// </summary>
        /// <param name="databytes">明文</param> 
        /// <param name="databytes">算法</param>
        /// <returns></returns>
        public static string Jiami_md5(string mingwen, HashAlgorithmProvider suanfa)
        {
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(mingwen, BinaryStringEncoding.Utf8);
            var hashed = suanfa.HashData(buff);
            var result = CryptographicBuffer.EncodeToHexString(hashed);

            return result;
        }
        /// <summary>
        /// 消息摘要（文件）
        /// </summary>
        /// <param name="databytes">文件</param> 
        /// <param name="databytes">算法</param>
        /// <returns></returns>
        public static async Task<string> Jiami_md5Async(StorageFile mingwen, HashAlgorithmProvider suanfa)
        {
            IBuffer buffer = await FileIO.ReadBufferAsync(mingwen);

            var hashed = suanfa.HashData(buffer);
            var result = CryptographicBuffer.EncodeToHexString(hashed);

            //清理
            GC.Collect();
            return result;
        }

    }
    public class Renwu_xinxi
    {
        //算法
        public SymmetricKeyAlgorithmProvider Syprd
        {
            get
            {
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
                return linshi;
            }
        }
        public int suanfa = 0;
        public string Suanfa
        {
            get
            {
                switch (suanfa)
                {
                    case 0:
                        return "AES";
                    case 1:
                        return "DES";
                    default:
                        return "None";
                }
            }
        }
        //密匙
        public string mishi = "";
        //密码
        public string mima = "";
        //附带任务  0,启用快速解密 1,开启超级保护 2记录加密算法和密匙 
        public List<bool> renwu = new List<bool>();
        //状态
        public int zhuangtai = 0;
        //是否自定义密匙
        public bool? zidingyi = false;
        //保存到的文件夹
        public StorageFolder wenjianjia_bao = null;


    }

}
