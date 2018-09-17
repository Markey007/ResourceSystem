using ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class BuildTargetFileWindow  : EditorWindow
{

    static string targetPath;

    private List<string> resPaths=new List<string>();

    private bool isSelecteChanged = false;

    void OnSelectionChange()
    {
        isSelecteChanged = true;
    }

    [MenuItem("ResTools/AssetBundle/OpenBuildTargetFileWindow")]
    static void OpenBuildTargetFileWindow()
    {
        ResUtils.InitResInfo(Application.dataPath + "/Resources",true);
        EditorWindow.GetWindow<BuildTargetFileWindow>();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.Space(15);
        if (isSelecteChanged)
        {
            isSelecteChanged = false;
            resPaths.Clear();
        }
        if (GUILayout.Button("依赖打包"))
        {
            ABUtils.BuildAllAbAboutDependency(ResourceSystemFacade.AbOutPath);
        }

        if (GUILayout.Button("非依赖快速打包"))
        {

        }



        GUILayout.EndVertical();
    }
}