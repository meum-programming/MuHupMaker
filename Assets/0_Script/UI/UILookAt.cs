using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script {
    public class UILookAt : MonoBehaviour
    {
        Transform t;
        private void OnEnable()
        {
            t = this.transform;
            GameManager.i.UILookAt.Add(t);
    


    }
        private void OnDisable() {
            GameManager.i.UILookAt.Remove(t);
        }


    }
}
