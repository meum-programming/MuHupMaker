using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace A_Script
{
    public abstract class TapBase : MonoBehaviour
    {
        protected Toggle t;
        protected InventoryBase ib;
        public virtual void Init(InventoryBase _ib)
        {

            ib = _ib;
            t = GetComponent<Toggle>();
            t.onValueChanged.AddListener(tapevent);

        }
        protected virtual void tapevent(bool b)
        {


        }
    }
}
