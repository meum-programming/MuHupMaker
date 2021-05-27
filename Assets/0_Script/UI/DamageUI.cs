using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace A_Script
{
    public class DamageUI : MonoBehaviour
    {
        Vector3 HoldPosition;
        PoolData P;
        public Text text;
        public void Init(PoolData p, DmgType d,ShowType s,float f) {
          
            P = p;
            text.text = Mathf.Round(f).ToString();
            if (s == ShowType.HP)
            {
                switch (d)
                {
                    case DmgType.Nomal:
                        text.color = Color.yellow;
                        text.fontSize = 50;
                        break;
                    case DmgType.Critical:
                        text.color = Color.red;
                        text.fontSize = 70;
                        break;
                }
            }
            else {
                switch (d)
                {
                    case DmgType.Nomal:
                        text.color = Color.blue;
                        text.fontSize = 50;
                        break;
                    case DmgType.Critical:
                        text.color = Color.magenta;
                        text.fontSize = 70;
                        break;
                }
            }
           
            HoldPosition = this.transform.position;
       
        }
        
        private void LateUpdate()
        {
            this.transform.position = HoldPosition;
        }
        
        public void AnimationEnd() {
            ObjectPoolManager.i.ReturnObj(P);
        }
    }
}
