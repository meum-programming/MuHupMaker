using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBase : MonoBehaviour
{
    protected bool isinit = false;
    public void localinit() {
        if (!isinit) {
            isinit = true;
            initactive();
        }
    }
    public virtual void initactive() {

    }
}
