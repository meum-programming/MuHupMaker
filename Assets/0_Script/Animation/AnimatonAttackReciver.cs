using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    public class AnimatonAttackReciver : StateMachineBehaviour
    {
        public int soundindex = -1;
        public bool IsEnd = false;
        public bool Rotation = true;
        public bool Smo0th = true;
        Unit u;
        public float AttackPoint = 0.3f;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (u == null)
                u = animator.GetComponent<Unit>();

            u.Preattackendevent?.Invoke();
            state = 0;
            iskill = false;
            isenter = true;
           
                u.Look = Rotation;
            
            u.AttackRotationlocaltime = 0;


        }
        bool isenter = false;

       // public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
       // {
       //     u.Afterattackendevent?.Invoke();
       // }
        int state = 0;
        bool iskill;
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (state == 0 && u.Us != UnitState.RebirthSkill)
            {
                if (stateInfo.normalizedTime >= AttackPoint)
                {

                    state = 1;
                 
                    u.Aftackstartevent?.Invoke();
                    if(soundindex != -1)
                    SoundManager.i.Play("Hit_"+soundindex.ToString("00"));
                    if (u.attack())
                    {
                        animator.SetBool("IsKill",true);
                        iskill = true;
                    }
                    else
                    {

                    
                    }
                }

            }
        //    Debug.Log(stateInfo.normalizedTime);
            if (stateInfo.normalizedTime >=1&& isenter)
            {
             //   Debug.Log(u.name + " Afterattackendevent");
                isenter = false;
                if (iskill|| IsEnd||u.Findlist.Count==0)
                {
                    u.Us = UnitState.Idle;
                    u.Afterattackendevent?.Invoke();
                }
            }

        }
    }
}
