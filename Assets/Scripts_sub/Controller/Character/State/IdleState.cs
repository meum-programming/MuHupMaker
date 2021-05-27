using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override BaseState Init(GameObject _go)
    {
        go = _go;

        return this;
    }

    public override void UpdateState()
    {

    }
}
