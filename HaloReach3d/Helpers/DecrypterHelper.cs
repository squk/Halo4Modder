using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace HaloReach3d.Helpers
{
    /// <summary>
    /// Our class to help with decryption
    /// </summary>
    public abstract class DecrypterHelper
    {
        static string HGUB = "HackGetsUBanned!";
        static byte[] Key2 = Encoding.ASCII.GetBytes(HGUB);
        static byte[] Key2Xored
        {
            get
            {
                byte[] rVal = new byte[Key2.Length];
                return rVal;
            }
        }
        static string Keyname = "BungieHaloReach!";
        static byte[] Key = Encoding.ASCII.GetBytes(Keyname);
        static byte[] KeyXored
        {
            get
            {
                byte[] rVal = new byte[Key.Length];
                for (int i = 0; i < Key.Length; i++)
                {
                    rVal[i] = (byte)((int)Key[i] ^ 0xA5);
                }
                return rVal;
            }
        }

        static byte[] IV
        {
            get
            {
                byte[] rVal = new byte[Key.Length];
                byte[] xoredKey = KeyXored;
                for (int i = 0; i < xoredKey.Length; i++)
                {
                    rVal[i] = (byte)((int)(xoredKey[i]) ^ 0x3C);
                }
                return rVal;
            }
        }
        public static byte[] DecryptStringData(byte[] data)
        {
            Aes AEDec = new AesCryptoServiceProvider();
            AEDec.Mode = CipherMode.CBC;
            AEDec.Key = KeyXored;
            AEDec.IV = IV;
            AEDec.Padding = PaddingMode.None;
            ICryptoTransform unenc = AEDec.CreateDecryptor(AEDec.Key, AEDec.IV);
            return unenc.TransformFinalBlock(data, 0, data.Length);
        }
        public static byte[] EncryptStringData(byte[] data)
        {
            Aes AEEnc = new AesCryptoServiceProvider();
            AEEnc.Mode = CipherMode.CBC;
            AEEnc.Key = KeyXored;
            AEEnc.IV = IV;
            AEEnc.Padding = PaddingMode.None;
            ICryptoTransform enc = AEEnc.CreateEncryptor(AEEnc.Key, AEEnc.IV);
            return enc.TransformFinalBlock(data, 0, data.Length);
        }
    }
}
