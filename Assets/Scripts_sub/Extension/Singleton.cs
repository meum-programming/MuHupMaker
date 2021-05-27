using UnityEngine;

namespace HSMLibrary.Generics
{
    public class Singleton<T> where T : class, new()
    {
        private static T instance;
        public static T getInstance
        {
            get
            {
                if(instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }
    }
    
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T getInstance
        {
            get
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent<T>();
                }

                return instance;
            }
        }
    }
    
    public class DontDestroyMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T getInstance
        {
            get
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent<T>();

                    DontDestroyOnLoad(go);
                }

                return instance;
            }
        }
    }
}
