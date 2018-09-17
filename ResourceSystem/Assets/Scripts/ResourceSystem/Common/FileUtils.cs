using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ResourceSystem
{
    public class FileUtils
    {
        /// <summary>
        /// 获得文件MD5
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileMD5(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open);

            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < retVal.Length; j++)
            {
                sb.Append(retVal[j].ToString("x2"));
            }
            return sb.ToString();
        }


        /// <summary>
        /// 处理文件夹是否存在，不存在就创建一个，存在就删掉重新建一个
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static void PrecessDirectoryExist(string dirPath)
        {
            DirectoryInfo info=new DirectoryInfo(dirPath);
            if (info.Exists)
            {
               info.Delete(true);
            }
            Directory.CreateDirectory(dirPath);
        }
    }
}
