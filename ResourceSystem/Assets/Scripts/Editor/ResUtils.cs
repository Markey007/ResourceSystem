using ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Reflection;

public class ResUtils
{

    static string resConfigPath = Application.dataPath + "/Resources/Config/Resources.xlsx";

    /// <summary>
    /// 初始化信息数据
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="isWriteExcel"></param>
    public static void InitResInfo(string resPath, bool isWriteExcel = false)
    {
        List<ResourcesData> infos = new List<ResourcesData>();
        SerializeResInfo(resPath, ref infos);
        ResourceSystemFacade.Inst.InitSystem();



        for (int i = 0; i < infos.Count; i++)
        {
            
            if (ResourceSystemFacade.Inst.ResDict.ContainsKey(infos[i].Name))
            {
                if (ResourceSystemFacade.Inst.ResDict[infos[i].Name].ID != infos[i].ID)
                {
                    //TODO版本号加一
                    ResourceSystemFacade.Inst.ResDict[infos[i].Name]= infos[i];

                    DebugUtils.DebugError(infos[i].Name + "文件有修改");
                }
            }
            else
            {
                ResourceSystemFacade.Inst.ResDict.Add(infos[i].Name, infos[i]);
            }
        }

        List<string> deleteFiles=new List<string>();
        foreach (var item in ResourceSystemFacade.Inst.ResDict)
        {
            for (int i = 0; i < infos.Count; i++)
            {
                if (infos[i].Name==item.Key)
                {
                    break;
                }

                if (i == infos.Count - 1)
                {
                    deleteFiles.Add(item.Key);
                }
            }
            item.Value.DependeciedNum = 0;
        }

        for (int i = 0; i < deleteFiles.Count; i++)
        {
            ResourceSystemFacade.Inst.ResDict.Remove(deleteFiles[i]);
        }

        foreach (var item in ResourceSystemFacade.Inst.ResDict)
        {
            for (int i = 0; i < item.Value.Dependencys.Count; i++)
            {
                if (string.IsNullOrEmpty(item.Value.Dependencys[i]))
                    continue;
                if (ResourceSystemFacade.Inst.ResDict.ContainsKey(item.Value.Dependencys[i]))
                {
                    ResourceSystemFacade.Inst.ResDict[item.Value.Dependencys[i]].DependeciedNum++;
                }
                else
                {
                    DebugUtils.DebugRedInfo("沒有包含名字为" + item.Value.Dependencys[i] + "的文件，有些奇怪");
                }
            }
        }
        
        FileInfo fileInfo = new FileInfo(resConfigPath);
        ConfigUtils.GenerateConfigToScriptableObject(ConfigFileType.Excel, fileInfo, "Assets/Resources/ScriptableObject");
        if (isWriteExcel)
        {
            string[,] content = ConvertUtils.ConvertInfosToContent(ResourceSystemFacade.Inst.ResDict.Values.ToList());
            ResourceSystemFacade.Inst.WriteToExcel(resConfigPath, "Sheet1", content);

        }
    }

    /// <summary>
    /// 序列化资源数据
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="isWriteExcel"></param>
    static void SerializeResInfo(string resPath,ref List<ResourcesData> infos)
    {
        string[] fileNames = Directory.GetFiles(resPath);
        string[] dirNames = Directory.GetDirectories(resPath);
        for (int i = 0; i < dirNames.Length; i++)
        {
            SerializeResInfo(dirNames[i], ref infos);
        }
        for (int i=0; i < fileNames.Length; i++)
        {
            ResourcesData info = new ResourcesData();
            if (PathUtils.GetExtension(fileNames[i]) != ".meta")
            {
                info.ID = FileUtils. GetFileMD5(fileNames[i]);
                info.Name = PathUtils.GetFileNameWithoutExtension(fileNames[i]);
                info.Path = PathUtils.GetRelativePathWithoutRootAndFileName("Resources",fileNames[i]);
                string assetPath = PathUtils.GetRelativePath("Assets", fileNames[i]);
                string[] dependencyPath = AssetDatabase.GetDependencies(assetPath);
                for (int j = 0; j < dependencyPath.Length; j++)
                {
                    if (assetPath!= dependencyPath[j])
                        info.Dependencys.Add(PathUtils.GetFileNameWithoutExtension(dependencyPath[j]));
                }
                info.version = "";
                infos.Add(info);
            }
        }
    }

}