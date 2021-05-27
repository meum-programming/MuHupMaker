using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script {

    public class InventoryTapUI : TapBase
    {
        // Start is called before the first frame update
        InventoryType it;
        InventoryUI iu;
        public override void Init(InventoryBase _ib)
        {

            base.Init(_ib);
            it = (InventoryType)System.Enum.Parse(typeof(InventoryType), this.name);
            iu = (InventoryUI)_ib;

        }
        protected override void tapevent(bool b)
        {

            if (b)
            {
                iu.ShowTap(it);
            }
        }
    }
}
