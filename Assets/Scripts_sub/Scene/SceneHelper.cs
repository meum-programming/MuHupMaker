using System;
using UnityEngine;

namespace HSMLibrary.Scene
{
    using Generics;

    public class SceneHelper : Singleton<SceneHelper>
    {
        private Type firstScene = null;        
        public Type setFirstScene { set { firstScene = value; } }

        public SceneHelper()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        /// <summary>
        /// 씬 변경
        /// </summary>
        /// <param name="sceneType"></param>
        public void ReplaceScene(Type sceneType)
        {
            SceneCallStack.getInstance.Push( sceneType );
            Client.SceneManager.ReplaceScene(sceneType);
        }

        /// <summary>
        /// 씬 뒤로가기
        /// </summary>
        /// <returns></returns>
        public bool BackScene()
        {
            SceneCallStack.getInstance.Pop();
            Type callSet = SceneCallStack.getInstance.Pop();
            if (callSet == null)
            {
                ReplaceScene(firstScene);
                return true;
            }

            ReplaceScene(callSet);

            return true;
        }

        private void OnLevelFinishedLoading(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            //UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnLevelFinishedLoading;
            
            GameObject ctrlObj = GameObject.Find("Controller");
            if (ctrlObj != null)
            {
                Controller ctrl = ctrlObj.GetComponent<Controller>();
                if (ctrl.musicName != String.Empty)
                {
                    Client.MusicManager.Play(ctrl.musicName);
                }
            }
        }
    }
}
