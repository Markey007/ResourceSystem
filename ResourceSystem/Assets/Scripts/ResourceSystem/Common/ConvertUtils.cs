using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ResourceSystem
{
    public class ConvertUtils
    {
        /// <summary>
        /// 从字符串中获取到int型列表
        /// </summary>
        /// <param name="content"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static List<int> GetIntListByString(string content, char splitChar = ',')
        {
            string[] temp = content.Split(splitChar);
            List<int> returnList = new List<int>();
            for (int i = 0; i < temp.Length; i++)
            {
                returnList.Add(Int32.Parse(temp[i]));
            }
            return returnList;
        }

        /// <summary>
        /// 从字符串中获取到int型列表
        /// </summary>
        /// <param name="content"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static List<string> GetStringListByString(string content, char splitChar = ',')
        {
            string[] temp = content.Split(splitChar);
            List<string> returnList = new List<string>();
            for (int i = 0; i < temp.Length; i++)
            {
                returnList.Add(temp[i]);
            }
            return returnList;
        }


        /// <summary>
        /// 将数据类集合转化为二维字符串数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="infos"></param>
        /// <returns></returns>
        public static string[,] ConvertInfosToContent<T>(List<T> infos)where T:IDataTemplete
        {
            Type type = typeof(T);
            FieldInfo[] fields = type.GetFields();
            string[,] content = new string[infos.Count, fields.Length];
            for (int j = 0; j < fields.Length; j++)
            {
                FieldInfo propertyInfo = type.GetField(fields[j].Name);


                for (int i = 0; i < infos.Count; i++)
                {
                    if (propertyInfo.FieldType == typeof(List<string>))
                    {
                        StringBuilder sb = new StringBuilder();
                        List<string> item = (List<string>)propertyInfo.GetValue(infos[i]);
                        if (item.Count > 0)
                        {
                            sb.Append(item[0]);
                            for (int k = 1; k < item.Count; k++)
                            {
                                sb.Append(",");
                                sb.Append(item[k]);
                            }
                        }
                        content[i, j] = sb.ToString();
                    }
                    else
                    {
                        content[i, j] = propertyInfo.GetValue(infos[i]).ToString();
                    }
                }
            }
            return content;
        }
    }
}
