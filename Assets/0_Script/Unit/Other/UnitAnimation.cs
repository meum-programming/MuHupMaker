using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    public class UnitAnimation : BasicUnit
    {
        public Animator Ani;
        public string MoveAni = "walk";
        public string Aniname = "idle";
        public void PlayAnimationCrossfade(string key, float nomalrize = 0)
        {
            if (Aniname == key)
                return;
            //  Debug.Log(key + " PlayAnimationCrossfade"); 
            Aniname = key;
            Ani.CrossFadeInFixedTime(key, 0.1f, 0, nomalrize);
        }
        public void PlayAnimation(string key, bool force = false)
        {
            if (!force)
            {
                if (Aniname == key)
                    return;
            }
            //  Debug.Log(key + " PlayAnimation");
            //플레이어 캐릭터라면
            if (charactertype != CharacterType.None)
                Ani.SetBool("IsKill", false);
              Aniname = key;
            Ani.Play(key, 0, 0);
        }
    }
}
