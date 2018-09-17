using System;
using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using FileUtils = ResourceSystem.FileUtils;

public class ABUtils
{
    /// <summary>
    /// 资源放置的文件夹路径
    /// </summary>
    private static List<string> resDircectoryPaths = new List<string>(){ "Assets/Resources" };

    private static List<string> needDependenyPaths = new List<string>() { "Assets/Resources/Materials" };

    public static void BuildABByDirectory(string resPath,string outPath)
    {
        string[] fileNames = System.IO.Directory.GetFiles(resPath);

        Dictionary<string, List<string>> realFileNames = PathUtils.GetNormalFilesName(fileNames);
        //对文件的依赖数进行计算
        Dictionary<string, int> dependencyNum = new Dictionary<string, int>();
        foreach (var item in realFileNames)
        {
            dependencyNum.Add(item.Key, 0);
        }

        foreach (var item in realFileNames)
        {
            string[] dependencies = AssetDatabase.GetDependencies(item.Key);
            for (int i = 0; i < dependencies.Length; i++)
            {
                if (dependencyNum.ContainsKey(dependencies[i]))
                {
                    dependencyNum[dependencies[i]]++;
                }
                else
                {
                    dependencyNum.Add(dependencies[i], 1);
                }
            }
        }

        //有依赖的设置assetbundle配置，无依赖则取消assetbundle
        foreach (var item in dependencyNum)
        {
            string curPath = System.IO.Path.GetDirectoryName(item.Key);
            if ((item.Value > 0 && curPath == PathUtils.NormalFileName(resPath)) || (item.Value > 1 && curPath != PathUtils.NormalFileName(resPath)))
            { 
                AssetImporter importer = AssetImporter.GetAtPath(item.Key);

                importer.assetBundleName = System.IO.Path.GetFileNameWithoutExtension(item.Key);
                importer.assetBundleVariant = "assetbundle";
                if (item.Value > 1 && curPath != PathUtils.NormalFileName(resPath))
                {
                    Debug.Log("文件" + item.Key + "在非打包路径下，对其依赖的包数量为  " + item.Value);
                }
                if (item.Value > 0 && curPath == PathUtils.NormalFileName(resPath))
                {
                    Debug.Log("文件" + item.Key + "在打包路径下，对其依赖的包数量为  " + item.Value);
                }
            }
            else
            {
                AssetImporter importer = AssetImporter.GetAtPath(item.Key);
                importer.assetBundleName = string.Empty;
            }
        }

        DebugUtils.DebugRedInfo(ResourceSystemFacade.AbOutPath);
        BuildPipeline.BuildAssetBundles(ResourceSystemFacade.AbOutPath, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows64);

    }



    /// <summary>
    /// 快速构建AB包，不考虑依赖
    /// </summary>
    public static void BuildQuicklyABByFiles(List<string> resPath, string outPath)
    {

    }

    public static void BuildAllAbAboutDependency(string outPath)
    {
        ResourceSystem.FileUtils.PrecessDirectoryExist(outPath);
        List<string> resPaths=InitBuildEnvironment();
        string fileName;
        string relationPath;
        ResourcesData data;
        bool isNeedDependcy;
        for (int i = 0; i < resPaths.Count; i++)
        {
            isNeedDependcy = false;
            fileName = PathUtils.GetFileNameWithoutExtension(resPaths[i]);
            if (ResourceSystemFacade.Inst.ResDict.ContainsKey(fileName))
            {
                data = ResourceSystemFacade.Inst.ResDict[fileName];
                relationPath = PathUtils.GetRelativePath("Assets", resPaths[i]);
                for (int j = 0; j < needDependenyPaths.Count; j++)
                {
                    if (relationPath.Contains(needDependenyPaths[j]))
                    {
                        isNeedDependcy = true;
                        if (data.DependeciedNum > 1)
                        {
                            AssetImporter importer = AssetImporter.GetAtPath(relationPath);

                            importer.assetBundleName = data.Name;
                            importer.assetBundleVariant = ResourceSystemFacade.FileExtension;
                        }
                    }
                }

                if (!isNeedDependcy)
                {
                    AssetImporter importer = AssetImporter.GetAtPath(relationPath);

                    importer.assetBundleName = data.Name;
                    importer.assetBundleVariant = ResourceSystemFacade.FileExtension;
                }

            }
            else
            {
                DebugUtils.DebugError("未包含文件" + fileName);
            }
        }
        DebugUtils.DebugRedInfo(outPath);
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows64);
    }

    /// <summary>
    /// 构建AB包，考虑依赖
    /// </summary>
    /// <param name="resPaths"></param>
    /// <param name="outPath"></param>
    public static void BuildAbByFilesAboutDependency(List<string> resPaths, string outPath)
    {

    }

    /// <summary>
    /// 初始化打包环境，将所有的文件的ab属性都置空
    /// </summary>
    static List<string> InitBuildEnvironment()
    {
        List<string> paths=new List<string>();
        string[] files = AssetDatabase.GetAllAssetPaths();
        for (int i = 0; i < files.Length; i++)
        {
            for (int j = 0; j < resDircectoryPaths.Count; j++)
            {
                if (files[i].Contains(resDircectoryPaths[j]))
                {
                    AssetImporter importer = AssetImporter.GetAtPath(files[i]);
                    paths.Add(files[i]);
                    importer.assetBundleName = String.Empty;
                }
            }
        }

        return paths;
    }
}
