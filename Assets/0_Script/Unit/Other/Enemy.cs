using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
namespace A_Script
{
    public class Enemy : UnitWithUI
    {
       
     
        public override void UnitInitDirect(UnitData UD, Inventory IV)
        {
            FollowStartEvent += () => { Us = UnitState.Move; };
            base.UnitInitDirect(UD, IV);
            Drop = DataManager.i.itemdginfoTable.itemdginfo[UD.drop];


        }
       
        SpawnActiver spawnactiver;

        public void SetupSpawnActiver(SpawnActiver _spawnactiver) {
            spawnactiver = _spawnactiver;
        }
        public float waittimemin = 1;
        public float waittimemax = 3;
        protected override IEnumerator WaypointExplorer(int lastpoint)
        {
            if (!isinit)
                yield return new WaitUntil(() => isinit);
         
            while (true) {

                
                Waypointstart?.Invoke();
             
                Us = UnitState.Move;

                PlayAnimationCrossfade(MoveAni);
                Move(spawnactiver.RandomPos(), spawnactiver.Position);
                yield return new WaitUntil(() => Navagent.pathStatus == NavMeshPathStatus.PathComplete);

                yield return new WaitUntil(() => Navagent.remainingDistance < 1f);
               
                Navagent.ResetPath();
                Us = UnitState.Idle;
                PlayAnimationCrossfade("idle");
                yield return new WaitForSeconds(Random.Range(waittimemin, waittimemax));
                Waypointend?.Invoke();
            }
        }
        protected override void WithUI()
        {
            base.WithUI();
            ui.GetComponentInChildren<Text>().text = OriginData.name;
            


        }
    }
}
