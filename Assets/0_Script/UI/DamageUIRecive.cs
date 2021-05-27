using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    public class DamageUIRecive : MonoBehaviour
    {
        public DamageUI dui;
    // Start is called before the first frame update
 

        public void AnimationEnd()
        {
            dui.AnimationEnd();
        }
    }
}
