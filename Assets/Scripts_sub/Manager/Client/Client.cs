using UnityEngine;
using HSMLibrary.Scene;
using HSMLibrary.Sound;

namespace HSMLibrary
{
    public class Client : MonoBehaviour
    {
        private static bool isCreated = false;

        private static SceneManager sceneManager;
        public static SceneManager SceneManager
        {
            get { return sceneManager; }
        }

        private static MusicManager musicManager;
        public static MusicManager MusicManager
        {
            get { return musicManager; }
        }

        private void Awake()
        {
            if ( !isCreated )
            {
                DontDestroyOnLoad(this.gameObject);
                isCreated = true;

                Constructor();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            Constructor();
        }

        private void Constructor()
        {
            sceneManager = GetComponentInChildren<SceneManager>();
            musicManager = GetComponentInChildren<MusicManager>();
        }
    }
}
