using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muhwang;

public enum ATK_EFFECT
{
    NONE = 0,
    STUN = 1,
    KNOCKBACK = 2
}

[System.Serializable]
public class AtkCompute
{
    public float distance; //.. 공격 거리
    public float angle; //.. 공격범위 사잇각

    public int delay;
    public CHAR_TYPE targetType;

    public int targetMax = 0; //.. 공격대상 갯수
    public int damage; //.. 데미지

    [System.NonSerialized]
    public Vector3 pos;

    [System.NonSerialized]
    public Vector3 dir;

    private List<Muhwang.CharacterController> targetList = null;

    public AtkCompute()
    {
        targetList = new List<Muhwang.CharacterController>();
        targetList.Clear();
    }
    
    public void SetTarget(Muhwang.CharacterController _target)
    {
        if (!_target.IsValid())
            return;

        targetList.Add(_target);
    }

    public float TwoPointAngle(Vector3 targetPos, Vector3 curPos)
    {
        float xx = targetPos.x - curPos.x;
        float yy = targetPos.z - curPos.z;

        float rad = Mathf.Atan2(yy, xx);
        float angle = rad * 180f / Mathf.PI;
        if (angle < 0f) angle = 360f + angle;

        return angle;
    }

    /// <summary>
    /// 타겟들에 대한 실제적 처리
    /// </summary>
    /// <returns></returns>
    public bool Excute()
    {
        var targetEnumerator = targetList.GetEnumerator();
        while(targetEnumerator.MoveNext())
        {
            var targetChar = targetEnumerator.Current;
            var targetPos = targetChar.trans.position;

            float angle = (1f - Vector3.Dot(dir, (targetPos - pos))) * 360f;

            bool isAngleCheck = angle < this.angle;
            bool isDisChk = Vector3.Distance(pos, targetPos) < distance;

            bool isResult = isAngleCheck && isDisChk;

            //Debug.Log("DIS = " + Vector3.Distance(pos, targetPos) + " ANGLE = " + angle);

            if (isDisChk)
            {
                targetChar.OnDamaged(damage);
                targetMax--;
            }

            if (targetMax <= 0)
                break;
        }

        return false;
    }
}
