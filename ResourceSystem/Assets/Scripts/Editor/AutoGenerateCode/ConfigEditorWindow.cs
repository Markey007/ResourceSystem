using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ConfigEditorWindow: EditorWindow
{

    List<FileInfo> files = new List<FileInfo>();

    int lastSelectFileCount = 0;
    string lastSelectAssetGUID;
    [MenuItem("ResTools/Config/ConfigEditorWindow")]
    static void OpenConfigEditorWindow()
    {
        EditorWindow.GetWindow<ConfigEditorWindow>();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.Label("需要生成代码的配置文件列表");
        GUILayout.Space(15);

        if (Selection.assetGUIDs.Length != lastSelectFileCount || 
            (Selection.assetGUIDs.Length > 0 && lastSelectAssetGUID != Selection.assetGUIDs[Selection.assetGUIDs.Length - 1]))
        {
            files.Clear();
            lastSelectFileCount = Selection.assetGUIDs.Length;
            for (int i = 0; i < Selection.assetGUIDs.Length; i++)
            {
                FileInfo fileInfo =
                   new FileInfo(AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[i]));
                if (fileInfo.Exists)
                {
                    if (!files.Contains(fileInfo))
                        files.Add(fileInfo);
                }
            }
            lastSelectAssetGUID = Selection.assetGUIDs.Last<string>();
        }

        for (int i = 0; i < files.Count; i++)
        {
            if (files[i].Exists)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label("名称  :" + files[i].Name);
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("生成"))
        {
            ConfigUtils.GenerateConfigsToCode(ConfigFileType.Excel, files,Application.dataPath+"/Scripts/Config");
        }

        if (GUILayout.Button("转换成ScriptsObject"))
        {
            ConfigUtils.GenerateConfigsToScriptableObject(ConfigFileType.Excel, files, "Assets/Resources/ScriptableObject");
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}

