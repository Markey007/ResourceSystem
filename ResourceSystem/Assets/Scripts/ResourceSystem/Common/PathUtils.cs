using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ResourceSystem
{
    public class PathUtils
    {
        #region 包一层Path的方法

        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public static string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        public static string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        #endregion

        #region 自定义方法
        /// <summary>
        /// 获取相对路径，不包含根目录名字
        /// </summary>
        /// <param name="rootDir"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetRelativePathWithoutRoot(string rootDir, string fileName)
        {
            //筛选文件
            int index = 0;
            fileName = fileName.Replace('\\', '/');
            string[] tempPath = fileName.Split('/');
            for (int j = 0; j < tempPath.Length; j++)
            {
                if (tempPath[j] == rootDir)
                {
                    index = j;
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(tempPath[index + 1]);

            for (int j = index + 2; j < tempPath.Length; j++)
            {
                sb.Append('/');
                sb.Append(tempPath[j]);
            }
            return sb.ToString();
        }


        /// <summary>
        /// 获取相对路径，不包含文件名与根目录名字
        /// </summary>
        /// <param name="rootDir"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetRelativePathWithoutRootAndFileName(string rootDir,string fileName)
        {
            //筛选文件
            int index = 0;
            fileName = fileName.Replace('\\', '/');
            string[] tempPath = fileName.Split('/');
            for (int j = 0; j < tempPath.Length; j++)
            {
                if (tempPath[j] == rootDir)
                {
                    index = j;
                }
            }
            StringBuilder sb = new StringBuilder();
            if(index + 1< tempPath.Length - 1)
            {
                sb.Append(tempPath[index + 1]);

                for (int j = index + 2; j < tempPath.Length - 1; j++)
                {
                    sb.Append('/');
                    sb.Append(tempPath[j]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取相对路径，不包含文件名
        /// </summary>
        /// <param name="rootDir"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetRelativePathWithoutFileName(string rootDir, string fileName)
        {
            //筛选文件
            int index = 0;
            fileName= fileName.Replace('\\', '/');
            string[] tempPath = fileName.Split('/');
            for (int j = 0; j < tempPath.Length; j++)
            {
                if (tempPath[j] == rootDir)
                {
                    index = j;
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(tempPath[index]);
            for (int j = index + 1; j < tempPath.Length-1 ; j++)
            {
                sb.Append('/');
                sb.Append(tempPath[j]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取相对路径
        /// </summary>
        /// <param name="rootDir"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetRelativePath(string rootDir, string fileName)
        {
            //筛选文件
            int index = 0;
            fileName = fileName.Replace('\\', '/');
            string[] tempPath = fileName.Split('/');
            for (int j = 0; j < tempPath.Length; j++)
            {
                if (tempPath[j] == rootDir)
                {
                    index = j;
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(tempPath[index]);
            for (int j = index + 1; j < tempPath.Length; j++)
            {
                sb.Append('/');
                sb.Append(tempPath[j]);
            }
            return sb.ToString();
        }

        #endregion


        public static Dictionary<string, List<string>> GetNormalFilesName(string[] fileNames)
        {
         
            Dictionary<string, List<string>> realFileNames = new Dictionary<string, List<string>>();
            string temp;
            //筛选文件
            for (int i = 0; i < fileNames.Length; i++)
            {
                temp = NormalFileName(fileNames[i]);
                if (temp != null)
                {
                    realFileNames.Add(temp, new List<string>());
                }
            }
            return realFileNames;
        }

        public static string NormalFileName(string fileName)
        {
            //筛选文件
            int index = 0;
            string[] tempPath = fileName.Split('/');
            for (int j = 0; j < tempPath.Length; j++)
            {
                if (tempPath[j] == "Assets")
                {
                    index = j;
                }
            }
            fileName = tempPath[index];
            for (int j = index + 1; j < tempPath.Length; j++)
            {
                fileName += '/' + tempPath[j];
            }

            if (System.IO.Path.GetExtension(fileName) != ".meta")
            {
                fileName = fileName.Replace('\\', '/');
                return fileName;
            }
            return null;
        }
    }
}
