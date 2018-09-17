using System.Collections;
using System.Collections.Generic;
using NetSystem;
using UnityEngine;
using ResourceSystem;

public class Tese : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ResourceSystemFacade.Inst.InitSystem();
        AssetBundleStream abStream = ResourceSystemFacade.Inst.LoadResourceFromAB("Cube 1");
        abStream.OnLoadFinish += () => { MonoBehaviour.Instantiate((GameObject)abStream.Content); };
        //NetSystemFacade.Inst.DownLoadFile("test.txt");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
