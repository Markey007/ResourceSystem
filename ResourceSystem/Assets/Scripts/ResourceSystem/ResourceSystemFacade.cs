using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeLineSystem;
using System.Data;
using System;
using System.Reflection;
using NetSystem;

namespace ResourceSystem
{
    /// <summary>
    /// 资源管理
    /// </summary>
    public class ResourceSystemFacade : Singleton<ResourceSystemFacade>
    {
        public bool IsAB { get; private set; }

        public bool IsInit { get; private set; }

        public Dictionary<string, ResourcesData> ResDict = new Dictionary<string, ResourcesData>();

        public List<string> LoadedRes = new List<string>();

        public static string AbOutPath = Application.dataPath+"/../../AssetBundles/Windows";

        public static string FileExtension = "assetbundle";

        public bool InitSystem(bool isAb = true)
        {
            DisposeSystem();
            IsAB = isAb;
            ResourcesInfo info = Resources.Load<ResourcesInfo>("ScriptableObject/ResourcesInfo");
            for (int i = 0; i < info.Data.Count; i++)
            {
                ResDict.Add(info.Data[i].Name, info.Data[i]);
            }
            IsInit = true;
            return ResDict.Count > 0;
        }

        public void DisposeSystem()
        {
            IsInit = false;
            ResDict.Clear();
        }

        #region Get

        public string GetTargetResIDByPath(string path)
        {
            return "";
        }

        public string GetTargetVersionByResID(string path)
        {
            return "";
        }

        #endregion

        #region Excel读写

        public void WriteToExcel(string path ,string sheetName,string[,] content)
        {
            ExcelStream stream = new ExcelStream();
            ExcelWriteArgs args = new ExcelWriteArgs();
            args.Path = path;
            args.SheetName = sheetName;
            args.Content = content;
            stream.WriteResource(args);
        }

        public List<IDataTemplete> ReadExcel(Type type,string path,int sheetIndex=0)
        {
            ConstructorInfo[] infos= type.GetConstructors();
            int index = -1;
            for(int i = 0; i < infos.Length; i++)
            {
                if (infos[i].GetParameters().Length == 0)
                {
                    index = i;
                }
            }
            List<IDataTemplete> data = new List<IDataTemplete>();
            if (index != -1)
            {
                ExcelStream stream = new ExcelStream();
                DataTableCollection collection = (DataTableCollection)stream.ReadResouce(path);
                for (int i = 3; i < collection[sheetIndex].Rows.Count; i++)
                {
                    IDataTemplete temp =(IDataTemplete)infos[index].Invoke(new object[] { });
                    temp.DeSerialize(collection[sheetIndex].Rows[i].ItemArray);
                    data.Add(temp);
                }
            }

            
            return data;
        }

        public DataTableCollection ReadExcel(string path) 
        {
            ExcelStream stream = new ExcelStream();
            return (DataTableCollection)stream.ReadResouce(path);
        }

        #endregion

        #region 资源加载

        public void DownLoadResourcesAndSave(string relativePath)
        {
            string path = NetSystemFacade.ResourceURL + "/" + relativePath+"."+ FileExtension;
            AssetBundleStream abStream = new AssetBundleStream();
            abStream.OnLoadFinish += () =>
            {
                AssetBundleWriteArgs args = new AssetBundleWriteArgs();
                args.Bytes = abStream.Bytes;
                args.Path = path;
                abStream.WriteResource(args);
            };
            abStream.ReadResouce(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resName"></param>
        /// <returns></returns>
        public object LoadResource(string resName)
        {
            object resObj = null;
            if (ResDict.ContainsKey(resName))
            {
                resObj = Resources.Load(ResDict[resName].Path + ResDict[resName].Name);
            }
            return resObj;
        }

        public AssetBundleStream LoadResourceFromAB(string resName, Action onLoadFinish = null)
        {
            AssetBundleStream abStream= new AssetBundleStream();
            abStream.OnLoadFinish += onLoadFinish;
           
            if (ResDict.ContainsKey(resName))
            {
                string path = NetSystemFacade.ResourceURL + "/"+ resName+"."+FileExtension;
                for (int i = 0; i < ResDict[resName].Dependencys.Count; i++)
                {
                    AssetBundleStream abChildStream = LoadResourceFromAB(ResDict[resName].Dependencys[i]);
                    abChildStream.OnLoadFinish += () =>
                    {
                        bool isLoaded=true;
                        LoadedRes.Add(PathUtils.GetFileNameWithoutExtension(abChildStream.Bundle.name));
                        DebugUtils.DebugRedInfo(abChildStream.Bundle.name);
                        for (int j = 0; j < ResDict[resName].Dependencys.Count; j++)
                        {
                            if (!LoadedRes.Contains(ResDict[resName].Dependencys[j]))
                            {
                                DebugUtils.DebugRedInfo(ResDict[resName].Dependencys[j]);
                                isLoaded = false;
                            }
                        }

                        if (isLoaded)
                        {
                            abStream.ReadResouce(path);
                        }
                    };
                    string chidResName = ResDict[resName].Dependencys[i];
                    DebugUtils.DebugRedInfo(chidResName);
                    if (ResDict.ContainsKey(chidResName))
                    {
                        string childPath = NetSystemFacade.ResourceURL + "/" + chidResName+"."+FileExtension;
                        abChildStream.ReadResouce(childPath);
                    }
                }
            }
            
            return abStream;
        }

#endregion

        #region 卸载资源
        public bool UnLoadResource()
        {
            return true;
        }
        #endregion


    }
}