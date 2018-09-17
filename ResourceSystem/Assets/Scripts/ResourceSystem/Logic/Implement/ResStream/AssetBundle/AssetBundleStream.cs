using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OfficeOpenXml.FormulaParsing.Utilities;
using UnityEngine;
using UnityEngine.Networking;

namespace ResourceSystem
{
    public class AssetBundleStream : IResourceReader, IResourceWriter,IDisposable
    {
        public float LoadProgress { get; private set; }

        public object Content { get; private set; }

        public AssetBundle Bundle { get; private set; }

        public byte[] Bytes { get; private set; }

        public Action OnLoadFinish;

        IEnumerator LoadResouce(string resPath, int version)
        {
            while (!Caching.ready)
                yield return null;
            DebugUtils.DebugError(resPath);
            using (UnityWebRequest uwr = UnityWebRequest.GetAssetBundle(resPath))
            {
                
                yield return uwr.SendWebRequest();
                if (uwr.error != null)
                    throw new Exception("www download had an error" + uwr.error);
                LoadProgress = uwr.downloadProgress;
                if (uwr.isDone)
                {
                    Bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                    //Bytes = www.bytes;

                    //FileInfo fileInfo = new FileInfo(Application.streamingAssetsPath + "/" + Bundle.name);
                    //FileStream fs = fileInfo.Create();
                    
                    ////fs.Write(字节数组, 开始位置, 数据长度);
                    //fs.Write(uwr.downloadHandler.data, 0, uwr.downloadHandler.data.Length);

                    //fs.Flush();     //文件写入存储到硬盘
                    //fs.Close();     //关闭文件流对象
                    //fs.Dispose();   //销毁文件对象

                    Content = Bundle.LoadAsset(PathUtils.GetFileNameWithoutExtension(resPath));
                    if (OnLoadFinish != null)
                    {
                        OnLoadFinish();
                    }
                }
            }
        }

        public object ReadResouce(string path)
        {
            GameTimeSystemFacade.Instance.StartCoroutine(LoadResouce(path, 0));
            return this;
        }

        public bool WriteResource(IWriteArgs data)
        {
            SaveAssetBundle((AssetBundleWriteArgs) data);
            return true;
        }

        /// <summary>
        /// 保存AssetBundle文件到本地
        /// </summary>
        private void SaveAssetBundle(AssetBundleWriteArgs args)
        {
            FileInfo fileInfo = new FileInfo(args.Path);
            FileStream fs = fileInfo.Create();

            //fs.Write(字节数组, 开始位置, 数据长度);
            fs.Write(args.Bytes, 0, args.Bytes.Length);

            fs.Flush();     //文件写入存储到硬盘
            fs.Close();     //关闭文件流对象
            fs.Dispose();   //销毁文件对象
        }

        public void Dispose()
        {
            OnLoadFinish = null;
        }
    }
}
