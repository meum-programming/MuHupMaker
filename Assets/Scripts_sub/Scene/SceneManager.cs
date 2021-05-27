using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace HSMLibrary.Scene
{
    public class SceneManager : MonoBehaviour
    {
        public string StartSceneName = null;

        private Scene currentScene = null;
        private string currentSceneName = null;
        public Scene getScene
        {
            get { return currentScene; }
        }

        List<Type> sceneTypes = new List<Type>();

        private void Awake()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLevelFinishedLoading;

            Type ti = typeof(Scene);
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in asm.GetTypes())
                {
                    if (ti.IsAssignableFrom(type) && type.IsClass)
                    {
                        sceneTypes.Add(type);
                    }
                }
            }
        }

        private void Start()
        {
            foreach (Type sceneType in sceneTypes)
            {
                if (sceneType.Name == StartSceneName)
                {
                    SceneHelper.getInstance.setFirstScene = sceneType;
                    ReplaceScene(sceneType);
                }
            }
        }

        public void ReplaceScene(Type newSceneType)
        {
            if (currentScene != null)
            {
                currentScene.OnDeactivate();
            }

            if (!newSceneType.IsSubclassOf(typeof(Scene)))
            {
                throw new Exception("Invalid Scene!");
            }

            Destroy(currentScene);

            currentScene = this.gameObject.AddComponent(newSceneType) as Scene;

            Debug.Log(String.Format("ReplaceSceneName : {0}", newSceneType));

            if (currentScene != null)
            {
                SceneNameAttribute sceneEntryAttr = null;

                object[] attributes = currentScene.GetType().GetCustomAttributes(typeof(SceneNameAttribute), true);
                foreach (Attribute attr in attributes)
                {
                    sceneEntryAttr = attr as SceneNameAttribute;

                    break;
                }

                if (sceneEntryAttr == null)
                {
                    currentSceneName = currentScene.GetType().Name;
                }
                else
                {
                    currentSceneName = sceneEntryAttr.getSceneName;
                }
                
                UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
            }
        }

        private void OnLevelFinishedLoading(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            if (scene.name == currentSceneName)
            {
                currentScene.OnActivate();
            }
        }

        /*
        private void Update()
        {
            if (currentScene != null)
            {
                currentScene.OnUpdate();
            }
        }*/
    }
}
