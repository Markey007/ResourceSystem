using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

namespace NetSystem
{
    public class NetSystemFacade:Singleton<NetSystemFacade>
    {

        public static string ResourceURL = "file://D:/WorkSpace/Demos/ResourceSystem/AssetBundles/Windows";

        public static string CachePath = Application.streamingAssetsPath+ "/AssetBundles";

        public void DownLoadFile(string resName)
        {
            FileStream fs = new FileStream(resName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            // 设置参数
            HttpWebRequest request = WebRequest.Create(ResourceURL) as HttpWebRequest;
            //发送请求并获取相应回应数据
          Debug.Log(request==null);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流
            //Stream stream = new FileStream(tempFile, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                //stream.Write(bArr, 0, size);
                fs.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            //stream.Close();
            fs.Close();
            responseStream.Close();
        }
    }
}
