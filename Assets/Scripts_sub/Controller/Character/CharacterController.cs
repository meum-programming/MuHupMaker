using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Muhwang
{
    public enum CHAR_TYPE
    {
        PLAYER,
        ENEMY,
        ALL
    }

    public abstract class CharacterController : MonoBehaviour
    {
        protected CHAR_TYPE type = CHAR_TYPE.PLAYER;

        [System.NonSerialized]
        public Transform trans = null;

        protected CharacterController target = null;
        protected Animator animator = null;
        protected NavMeshAgent agent = null;

        protected float detectRange = 10f;

        protected bool isValid = false;

        [System.NonSerialized]
        public bool motionEnd = true;
        
        public virtual void SetValid(bool _valid)
        {
            isValid = _valid;
            //go.SetActive(isValid);
        }

        public virtual bool IsValid()
        {
            return isValid;
        }

        public virtual void DetectTarget(CharacterController _target)
        {
            if (target == null)
                target = _target;
        }

        public abstract void OnDamaged(int damage);
    }
}
