using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE_TYPE
{
    IDLE,
    ATTACK,
    PATROL,
    TRACE
}

public abstract class BaseState
{
    protected GameObject go = null;
    protected Animator anim = null;

    public abstract BaseState Init(GameObject _go);
    public abstract void UpdateState();
}
