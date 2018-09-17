using System;
using UnityEngine;
using System.Collections.Generic;
using ResourceSystem;

public class ResourcesInfo:ScriptableObject,IDataCollectionTemplete
{
    public List<ResourcesData>  Data = new List<ResourcesData>();
    /// <summary>
    /// 获得内容Type
    /// </summary>
    public Type GetContenType()
    {
        return typeof(ResourcesData);
    }
    /// <summary>
    /// 转换成当前内容类型的列表
    /// </summary>
    public void ConvertToContentList(List<IDataTemplete> contents)
    {
       for(int i = 0; i < contents.Count; i++)
       {
           Data.Add((ResourcesData)contents[i]);
       }
    }
}


[Serializable]
public class ResourcesData:IDataTemplete
{
    /// <summary>
    /// 索引号
    /// </summary>
    public string ID ;
    /// <summary>
    /// 名称
    /// </summary>
    public string Name ;
    /// <summary>
    /// 路径
    /// </summary>
    public string Path ;
    /// <summary>
    /// 版本
    /// </summary>
    public string version ;
    /// <summary>
    /// 依赖
    /// </summary>
    public List<string> Dependencys ;
    /// <summary>
    /// 被依赖的数量
    /// </summary>
    public int DependeciedNum ;
    /// <summary>
    /// 构造方法
    /// </summary>
    public ResourcesData()
    {
        ID= default(string);
        Name= default(string);
        Path= default(string);
        version= default(string);
        Dependencys= new List<string>();
        DependeciedNum= default(int);
    }
    /// <summary>
    /// 反序列化
    /// </summary>
    public void DeSerialize(object[] content)
    {
        ID = content[0].ToString();
        Name = content[1].ToString();
        Path = content[2].ToString();
        version = content[3].ToString();
        Dependencys = ConvertUtils.GetStringListByString(content[4].ToString());
        DependeciedNum = Int32.Parse(content[5].ToString());
    }
}