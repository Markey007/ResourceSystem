using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceSystem;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public enum ConfigFileType
{
    Excel,
}
public class ConfigUtils
{
    #region 自动生成配置代码
    static List<string> argsNames = new List<string>();
    static List<string> argsType = new List<string>();
    static List<string> argsChineseName = new List<string>();
    public static void GenerateConfigsToCode(ConfigFileType fileType, List<FileInfo> fileInfos, string targetPath)
    {
        for(int i=0;i< fileInfos.Count;i++)
        {
            GenerateConfigToCode(fileType, fileInfos[i], targetPath);
        }
    }

    public static void GenerateConfigToCode(ConfigFileType fileType,FileInfo fileInfo,string targetPath)
    {
        if (fileType== ConfigFileType.Excel)
        {
            GenerateExcelToCode(fileInfo.FullName, targetPath);
        }
    }

    private static void GenerateExcelToCode(string path,string targetPath)
    {
        DataTableCollection tables = ResourceSystemFacade.Inst.ReadExcel(path);

        argsNames.Clear();
        argsType.Clear();
        argsChineseName.Clear();

        int columns = tables[0].Columns.Count;//获取列数
        int rows = tables[0].Rows.Count;

        for (int n = 0; n < columns; n++)
        {
            argsNames.Add(tables[0].Rows[0][n].ToString());
            argsType.Add(tables[0].Rows[1][n].ToString());
            argsChineseName.Add(tables[0].Rows[2][n].ToString());
        }

        string fileName = Path.GetFileNameWithoutExtension(path);
        string code = GenerateCode(fileName);
        if (!string.IsNullOrEmpty(code))
        {
            DirectoryInfo info = new DirectoryInfo(targetPath);
            if (!info.Exists)
            {
                Directory.CreateDirectory(targetPath);
            }
            File.WriteAllText(targetPath +"/"+ fileName + "Info.cs", code);
        }
        else
        {
            DebugUtils.DebugError("生成代码有误");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private static string GenerateCode(string fileName)
    {
        StringBuilder code = new StringBuilder();
        code.Append("using System;\nusing UnityEngine;\nusing System.Collections.Generic;\nusing ResourceSystem;\n\n");
        code.Append("public class ");
        code.Append(fileName);
        code.Append("Info:ScriptableObject,IDataCollectionTemplete\n{");

        code.Append("\n    public List<");
        code.Append(fileName);
        code.Append("Data>  Data = new List<");
        code.Append(fileName);
        code.Append("Data>();");

        code.Append("\n    /// <summary>\n ");
        code.Append("   /// 获得内容Type");
        code.Append("\n    /// </summary>\n    public Type GetContenType()\n    {");
        code.Append("\n        return typeof(");
        code.Append(fileName);
        code.Append("Data);");
        code.Append("\n    }");

        code.Append("\n    /// <summary>\n ");
        code.Append("   /// 转换成当前内容类型的列表");
        code.Append("\n    /// </summary>\n    public void ConvertToContentList(List<IDataTemplete> contents)\n    {");
        code.Append("\n       for(int i = 0; i < contents.Count; i++)\n       {");
        code.Append("\n           Data.Add((");
        code.Append(fileName);
        code.Append("Data)contents[i]);");

        code.Append("\n       }");
        code.Append("\n    }");
        code.Append("\n}\n\n");


        code.Append("\n[Serializable]\n");
        code.Append("public class ");
        code.Append(fileName);
        code.Append("Data:IDataTemplete\n{");
        for(int i = 0; i < argsNames.Count; i++)
        {
            code.Append("\n    /// <summary>\n ");
            code.Append("   /// ");
            code.Append(argsChineseName[i]);
            code.Append("\n    /// </summary>");
            code.Append("\n    public ");
            code.Append(argsType[i]);
            code.Append(" ");
            code.Append(argsNames[i]);
            code.Append(" ;");
        }

        code.Append("\n    /// <summary>\n ");
        code.Append("   /// 构造方法");
        code.Append("\n    /// </summary>\n    public ");
        code.Append(fileName);
        code.Append("Data()\n    {");
        for (int i = 0; i < argsNames.Count; i++)
        {
            code.Append("\n        ");
            code.Append(argsNames[i]);
            if (argsType[i].Contains("List"))
            {
                code.Append("= new ");
                code.Append(argsType[i]);
                code.Append("();");
            }
            else
            {
                code.Append("= default(");
                code.Append(argsType[i]);
                code.Append(");");
            }
        }
        code.Append("\n    }");

        code.Append("\n    /// <summary>\n ");
        code.Append("   /// 反序列化");
        code.Append("\n    /// </summary>\n    public void DeSerialize(object[] content)\n    {");
        for (int i = 0; i < argsNames.Count; i++)
        {
            code.Append("\n        ");
            code.Append(argsNames[i]);
            if (argsType[i] == "int")
            {
                code.Append(" = Int32.Parse(content[");
                code.Append(i);
                code.Append("].ToString());");
            }
            else if (argsType[i] == "List<int>")
            {
                code.Append(" = ConvertUtils.GetIntListByString(content[");
                code.Append(i);
                code.Append("].ToString());");
            }
            else if (argsType[i] == "List<string>")
            {
                code.Append(" = ConvertUtils.GetStringListByString(content[");
                code.Append(i);
                code.Append("].ToString());");
            }
            else 
            {
                code.Append(" = content[");
                code.Append(i);
                code.Append("].ToString();");
            }
        }
        code.Append("\n    }");

        code.Append("\n}");
        return code.ToString();
    }

    #endregion

    #region 生成ScriptableObject
    public static void GenerateConfigsToScriptableObject(ConfigFileType fileType, List<FileInfo> fileInfos, string targetPath)
    {
        
        for (int i = 0; i < fileInfos.Count; i++)
        {
            
            GenerateConfigToScriptableObject( fileType, fileInfos[i], targetPath);
        }
    }

    public static void GenerateConfigToScriptableObject(ConfigFileType fileType, FileInfo fileInfo, string targetPath)
    {
        string typeName = Path.GetFileNameWithoutExtension(fileInfo.Name) + "Info";
        if (fileType == ConfigFileType.Excel)
        {
            GenerateExcelToScriptableObject(typeName,fileInfo.FullName, targetPath);
        }
    }

    private static void GenerateExcelToScriptableObject(string typeName,string path, string targetPath)
    { 
        ScriptableObject instance = ScriptableObject.CreateInstance(typeName);
        if (instance == null)
        {
            return;
        }

        ((IDataCollectionTemplete)instance).ConvertToContentList(ResourceSystemFacade.Inst.ReadExcel(((IDataCollectionTemplete)instance).GetContenType(), path));

        string[] paths = targetPath.Split('/');
        string finalPath = "";
        bool isStart = false;
        for(int i = 0; i < paths.Length; i++)
        {
            if (paths[i] == "Assets")
            {
                isStart = true;
            }
            if (isStart)
            {
                finalPath += paths[i] + "/";
            }
        }
        DirectoryInfo dir = new DirectoryInfo(finalPath);
        if (!dir.Exists)
        {
            DebugUtils.DebugError("文件夹不存在");
            Directory.CreateDirectory(targetPath);
        }
        string initAssetPath = finalPath + typeName + ".asset";
        AssetDatabase.DeleteAsset(initAssetPath);
        AssetDatabase.CreateAsset(instance, initAssetPath);

    }
    

    #endregion
    
}

