using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeSystemFacade : MonoBehaviour {

    public static GameTimeSystemFacade Instance { get; private set; }
    
    void Awake ()
    {
        Instance = this;
    }



}
