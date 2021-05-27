using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : HSMLibrary.Generics.Singleton<GlobalData>
{
    public LocalUserInfo user = null;

    public bool isInitialized = false;

    public GlobalData()
    {
        user = new LocalUserInfo();
    }
}
