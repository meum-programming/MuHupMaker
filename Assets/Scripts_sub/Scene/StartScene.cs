using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HSMLibrary.Scene;


public class StartScene : Scene
{
    public override void OnActivate()
    {
        //.. INFO :: CSV Data Load
  

        GlobalData.getInstance.isInitialized = true;

        SceneHelper.getInstance.ReplaceScene(typeof(StageScene));
    }

    public override void OnDeactivate()
    {
        
    }
}
