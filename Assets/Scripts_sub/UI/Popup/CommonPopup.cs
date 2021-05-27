using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonPopup : MonoBehaviour
{
    protected GameObject popupObj = null;

    private PopupSetter setter = null;


    private void Awake()
    {
        popupObj = gameObject;

#if USING_NGUI
        setter = new NGUIPopupSetter();
#else
        setter = new UGUIPpopupSetter();
#endif
    }

    public virtual void SetPopup(string _title, string _content)
    {

    }

    public virtual void SetPopup(string _title, string _content, System.Action _onConfirmCallback)
    {
        SetPopup(_title, _content);
    }

    public virtual void SetPopup(string _title, string _content, System.Action _onConfirmCallback, System.Action _onCancelCallback)
    {
        SetPopup(_title, _content, _onConfirmCallback);
    }
    
    public virtual void Show()
    {
        popupObj.SetActive(true);
    }

    public virtual void Close()
    {
        popupObj.SetActive(false);
    }
}
