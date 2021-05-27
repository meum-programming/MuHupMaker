using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace A_Script {
    public class SkillTapUI : TapBase
    {
        // Start is called before the first frame update
        SkillType st;
        SkillUI su;
    public override void Init(InventoryBase _ib) {

            base.Init(_ib);
            st = (SkillType)System.Enum.Parse(typeof(SkillType), this.name);
            su = (SkillUI)_ib;

        }
    protected override void tapevent(bool b)
    {

        if (b)
        {
                su.ShowTap(st);
        }
    }
}
}
