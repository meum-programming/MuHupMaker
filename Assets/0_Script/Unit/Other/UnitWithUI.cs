using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    public class UnitWithUI : Unit
    {
        protected UnitStateUI ui;
        protected override void Start()
        {
            base.Start();
            ChangeDmgEvent += ShowDamaged;
         
        }
        protected virtual void ShowDamaged(DmgType d,ShowType s, float f)
        {
            PoolData p = new PoolData();
            if(ObjectPoolManager.i.GetObj("DamageUI",ref p))
            {
                p.G.transform.SetParent(ui.transform);
                p.G.transform.localPosition = Vector3.zero;
                p.G.transform.localRotation = Quaternion.identity;
                p.G.transform.localScale = Vector3.one;
                p.G.GetComponent<DamageUI>().Init(p,d,s,f);
                    

            }
        }
     
        public override void UnitInitDirect(UnitData UD, Inventory IV)
        {
            base.UnitInitDirect(UD, IV);
         
            WithUI();
        }
       
       protected void hpChangeEvent() {
            ui.hp = CurrentHP;
        }
        protected void apChangeEvent()
        {
            if (Unitdata.ap > 0)
                ui.ap = CurrentAP;
        }
        protected void mpChangeEvent()
        {

            ui.mp = CurrentMP;
        }
        protected void expChangeEvent()
        {

            ui.exp = CurrentEXP;
        }

        protected virtual void WithUI() {
            var temp = Instantiate<GameObject>(Resources.Load<GameObject>("UI/UnitStateUI"));
            temp.transform.SetParent(this.transform);
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localRotation = Quaternion.identity;
            temp.name = "UI";
            ui = temp.GetComponent<UnitStateUI>();
            ui.Init(GameManager.i.ActiveStageCamera.SubCamera,HP, AP,MP,CurrentHP,CurrentAP,CurrentMP);
            HPChangeEvent += hpChangeEvent;
            APChangeEvent += apChangeEvent;
            MPChangeEvent += apChangeEvent;
        }
    }
    
}
