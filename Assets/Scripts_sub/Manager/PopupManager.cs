using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSMLibrary.UI
{
    using Generics;

    public class PopupManager : MonoSingleton<PopupManager>
    {
        private Stack<CommonPopup> popupStack = null;

        private void Awake()
        {
            popupStack = new Stack<CommonPopup>();
            popupStack.Clear();
        }

        public void Show(CommonPopup _popup)
        {
            _popup.Show();
            popupStack.Push(_popup);
        }

        public int Close()
        {
            int popupCnt = popupStack.Count;
            if (popupCnt == 0)
                return 0;

            var popup = popupStack.Pop();
            popup.Close();

            return popupCnt;
        }

        private void FixedUpdate()
        {
            if(Input.GetKeyUp(KeyCode.Backspace))
            {
                if(Close() == 0)
                {
                    ApplicationManager.getInstance.ApplicationExit();
                }
            }
        }
    }
}
