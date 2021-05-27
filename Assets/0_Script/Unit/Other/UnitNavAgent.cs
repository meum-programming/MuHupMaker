using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
namespace A_Script
{
    public class UnitNavAgent : UnitAnimation
    {
        public NavMeshAgent Navagent;
        public UnitState _Us;
        public UnitState Us {
            get {
                return _Us;
            }
            set {

                _Us = value;
             //   if(GetComponent<Player>())
              //  Debug.LogError(value);

            }
        }
     
        public List<Transform> Waypoint;
        protected void Warp(Vector3 V)
        {
            Warp(V,Vector3.zero);
        }
            protected void Warp(Vector3 V, Vector3 nomalrize,Vector3 back)
        {
            int warpcount = 0;
            while (!Navagent.Warp(V))
            {
                V += nomalrize;
                warpcount++;
                if (warpcount >= 10)
                {
                    Navagent.Warp(back);
                    break;
                }
            }
        }
        protected void Warp(Vector3 V, Vector3 back)
        {
            Vector3 nomalrize = (V - this.transform.position).normalized;
            int warpcount = 0;
            while (!Navagent.Warp(V))
            {
                V += nomalrize;
                warpcount++;
                if (warpcount >= 10)
                {
                    Navagent.Warp(back);
                    break;
                }
            }
        }
        public void UnitWarp(Vector3 V)
        {
            Warp(V,Vector3.zero);
        }
        public void UnitWarp(Vector3 V, Vector3 back) {
            Warp(V, back);
        }
        protected void Move(Vector3 V)
        {
            Move(V,Vector3.zero);
        }
            protected void Move(Vector3 V,Vector3 backo)
        {
         
              Vector3 nomalrize = (V - this.transform.position).normalized;
            int warpcount = 0;
        
            while (!Navagent.SetDestination(V))
            {
                V += nomalrize;
                warpcount++;
                if (warpcount >= 10)
                    break;
            }
        }
        protected void OnEnable()
        {
            StartWaypointExplorer();

        }
        protected void StartWaypointExplorer()
        {
            if (waypointexplorer != null)
                StopCoroutine(waypointexplorer);
            waypointexplorer = StartCoroutine("WaypointExplorer", waypointstate);
        }
        protected void WaypointExploreroff()
        {
            if (waypointexplorer != null)
            {
                StopCoroutine(waypointexplorer);
            }
            waypointexplorer = null;
           // Debug.Log(this.name + "waypointexplorer off");
        }
       protected  Coroutine waypointexplorer = null;
        protected Action Waypointstart = null;
        protected Action Waypointend = null;
        public int waypointstate;
        protected bool isinit = false;

        protected virtual IEnumerator WaypointExplorer(int lastpoint)
        {
            if (!isinit)
                yield return new WaitUntil(() => isinit);
            Us = UnitState.Move;
            PlayAnimationCrossfade(MoveAni);
            while (true)
            {
                int i = waypointstate;
                for (; i < Waypoint.Count; i++)
                {
                 //   Debug.LogError(i);
                    waypointstate = i;
                    Waypointstart?.Invoke();
                    Move(Waypoint[i].position);
                    yield return new WaitUntil(() => Navagent.pathStatus == NavMeshPathStatus.PathComplete);
                    yield return new WaitUntil(() => !Navagent.pathPending);

                    yield return new WaitUntil(() => Navagent.remainingDistance < 1f);

                    Navagent.ResetPath();


                    Waypointend?.Invoke();
                }
                waypointstate = 0;
            }
        }

    }
}
