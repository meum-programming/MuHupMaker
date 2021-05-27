using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSMLibrary.Generics
{
    using Generics;

    public class ApplicationManager : MonoSingleton<ApplicationManager>
    {
        private void OnApplicationFocus(bool focus)
        {
            
        }

        private void OnApplicationPause(bool pause)
        {
            
        }

        public void ApplicationExit()
        {
            Application.Quit();
        }

        private void OnApplicationQuit()
        {
            Application.CancelQuit();
            StartCoroutine(OnApplicationDelayQuit());
        }

        private IEnumerator OnApplicationDelayQuit()
        {
            //.. TODO :: 앱이 완전히 종료되기전 처리들

            yield return new WaitForSeconds(0.5f);

            Application.Quit();
        }
    }
}