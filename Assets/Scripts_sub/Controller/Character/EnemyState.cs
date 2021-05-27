using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    private Dictionary<STATE_TYPE, BaseState> stateDic = null;

    private STATE_TYPE curState = STATE_TYPE.IDLE;

    public EnemyState(GameObject go)
    {
        stateDic = new Dictionary<STATE_TYPE, BaseState>();
        stateDic.Add(STATE_TYPE.IDLE, new IdleState().Init(go));
        stateDic.Add(STATE_TYPE.ATTACK, new AttackState().Init(go));
        stateDic.Add(STATE_TYPE.TRACE, new TraceState().Init(go));
    }

    public void SetState(STATE_TYPE _state)
    {
        curState = _state;
    }

    public void UpdateState()
    {
        stateDic[curState].UpdateState();
    }
}
