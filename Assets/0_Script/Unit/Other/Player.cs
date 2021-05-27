using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

namespace A_Script
{
    public class Player : UnitWithUI
    {
        public int readySkillBtnIndex = -1;
        public UnityAction<int> rebirthReadyEvent;
        public void Reset() {
    
            Us = UnitState.Idle;

            if (Ani != null)
            {
                Ani.Rebind();
                Ani.enabled = true;
            }

            Aniname = "idle";            
            waypointstate = 0;

            if (Navagent != null)
            {
                Navagent.enabled = true;
                Navagent.isStopped = false;
                Navagent.ResetPath();
            }
            
            findoff();
            follwoff();
            WaypointExploreroff();
            ForceMoveStop();
            ForceFollwStop();
            Findlist.Clear();
            // Warp(GameObject.Find("STAGE/StartPoint").transform.position);
            StartCoroutine(WaitFrame());

            GameManager.i.FieldBossCntAdd(0);

        }
        IEnumerator WaitFrame() {
            yield return new WaitUntil(()=> GameManager.i.Waypoints.Count>0);
         //   PlayAnimation("idle", true);
            Waypoint = GameManager.i.Waypoints;
            StartWaypointExplorer();
        }
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            GameManager.i.PointEvent += ForceMove;
            GameManager.i.TargetEvent += ForceFollw;
        }
     
        UILookAt UI;

        public void Wait()
        {
            Navagent.enabled = false;

            Findlist.Clear();
            findoff();
            follwoff();
            WaypointExploreroff();
            ForceMoveStop();
            ForceFollwStop();

            Us = UnitState.Idle;

            Ani.Play("idle");
        }

        public void ResetUI()
        {
            WithUI();
        } 

        protected override void WithUI()
        {
            base.WithUI();
            Text nameText = ui.GetComponentInChildren<Text>();
            nameText.text = GameManager.i.csd.nickName;
            nameText.color = new Color32(255, 150, 0, 255);

            ui = FindObjectOfType<PlayerStateUI>();
            ui.UpdateLV(CharLv);
            ui.Init(null, HP, AP,MP, CurrentHP, CurrentAP,CurrentMP,EXP,CurrentEXP,true);
            DropEvent = ((PlayerStateUI)ui).ShowText;
            HPChangeEvent = hpChangeEvent;
            APChangeEvent = apChangeEvent;
            MPChangeEvent = mpChangeEvent;
            EXPChangeEvent = expChangeEvent;
            UI = transform.GetComponentInChildren<UILookAt>();
          //  UI.Init(GameManager.i.ActiveStageCamera.GetComponent<Camera>());
        }
        protected override void LvUpEvent()
        {
            if (ui != null)
            {
                CurrentHP = HP;
                CurrentAP = AP;
                CurrentMP = MP;
                
                ui.UpdateData(HP, AP, MP, CurrentHP, CurrentAP, CurrentMP, EXP, CurrentEXP);
                ui.UpdateLV(CharLv);
            }
        }
        protected override void ShowDamaged(DmgType d, ShowType s, float f)
        {
            return;
            PoolData p = new PoolData();
            if (ObjectPoolManager.i.GetObj("DamageUI", ref p))
            {
                p.G.transform.SetParent(UI.transform);
                p.G.transform.localPosition = Vector3.zero;
                p.G.transform.localRotation = Quaternion.identity;
                p.G.transform.localScale = Vector3.one;
                p.G.GetComponent<DamageUI>().Init(p, d, s, f);


            }
        }
    
        public override void DieDestroy()
        {
            // Debug.LogError("DieDestroy");
       /*
            foreach (Transform t in this.transform)
            {
                if (t.name != "UI")
                {
                    t.gameObject.SetActive(false);
                }
            }
            */
            Findlist.Clear();
            findoff();
            follwoff();
            WaypointExploreroff();
            ForceMoveStop();
            ForceFollwStop();
            StartCoroutine(resurrection());
        
        }
        void ForceMoveStop()
        {

            if (Fm != null)
            {
                StopCoroutine(Fm);
                Fm = null;
            }
        }
        void ForceFollwStop()
        {

            if (Ff != null)
            {
                StopCoroutine(Ff);
                Ff = null;
            }
        }
        
        public void ForceFollw(Transform u)
        {
            findoff();
            follwoff();
            WaypointExploreroff();
            ForceFollwStop();
            ForceMoveStop();
            TargetUnit = u.GetComponent<Unit>();
            Ff = StartCoroutine(forcefollow());
            Us = UnitState.ForceMove;
        }
        Unit TargetUnit;
        protected override void FindTarget()
        {
            if (Us == UnitState.ForceMove){
                MainTarget = TargetUnit;
            }
            else{
                base.FindTarget();
            }
           
        }
        Coroutine Ff;
        public void ForceMove(Vector3 v) {
            findoff();
            follwoff();
            WaypointExploreroff();
            ForceFollwStop();
            ForceMoveStop();
            Fm = StartCoroutine(forcemove(v));
            Us = UnitState.ForceMove;
        }
        Coroutine Fm;
        IEnumerator forcemove(Vector3 v) {
            PlayAnimationCrossfade(MoveAni);
                    Move(v);
                    yield return new WaitUntil(() => Navagent.pathStatus == NavMeshPathStatus.PathComplete);
                    yield return new WaitUntil(() => !Navagent.pathPending);

                    yield return new WaitUntil(() => Navagent.remainingDistance < 1f);

                    Navagent.ResetPath();
            Us = UnitState.Idle;
            startfollow();
        }
        IEnumerator forcefollow()
        {
      

            while (true)
            {
                float target = GetLenth(t, TargetUnit.transform);

                float r = 1;
                if (Weapon.isit)
                {
                    r = 1 * Weapon.item.range / 10000f;

                }
                if (target < r)
                {

                    Attackoder(TargetUnit);
                    follwoff();
                    yield break;

                }
                Move(TargetUnit.transform.position);
                yield return new WaitForSeconds(0.1f);
            }


        }
        IEnumerator resurrection() {
            PlayAnimationCrossfade("die");

            yield return new WaitForSeconds(3);
            GameManager.i.Resurrection.SetActive(true);
            for (int i = 0; i < 7; i++) {
                GameManager.i.ResurrectionText.text = (7 - i).ToString() + "초 후 부활";
               yield return new WaitForSeconds(1);
            }
            GameManager.i.Resurrection.SetActive(false);
        
            Ani.Rebind();
            Aniname = "idle";
            Ani.enabled = true;

            PlayerDataManager.ins.DataSet(true);

			foreach (Transform t in this.transform)
            {
                if (t.name != "UI")
                {
                    t.gameObject.SetActive(false);
                }
            }
            
            Us = UnitState.Idle;
            Ani.enabled = true;
        //    transform.position = GameObject.Find("STAGE/StartPoint").transform.position;
            Navagent.enabled = true;
            Navagent.isStopped = false;
            Warp(GameObject.Find("STAGE/StartPoint").transform.position);
            yield return new WaitForSeconds(1);
            foreach (Transform t in this.transform)
            {
                if (t.name != "UI")
                {
                    t.gameObject.SetActive(true);
                }
            }
            StartWaypointExplorer();
        }


        public override void KillEvent(Unit u)
        {
            GameManager.i.CD.Gold += u.Unitdata.gold;
            base.KillEvent(u);
            if (Us == UnitState.ForceAttack)
            {
                ForceFollwStop();
                Us = UnitState.Idle;
                startfollow();
            }
            GameManager.i.IUI.UpdateShow();
        
        }
        protected override void Attackanimation()
        {
            if (readySkillBtnIndex != -1)
            {
                FindTarget();
                
                if (rebirthReadyEvent != null)
                    rebirthReadyEvent(readySkillBtnIndex);

                readySkillBtnIndex = -1;

                return;
            }

            if (Us == UnitState.RebirthSkill)
            {
                return;
            }

            if (Weapon.isit)
            {
                switch (Weapon.item.type) {
                    case 1:
                        PlayAnimation("Knuckle");
                        break;
                  //  case 2:
                    //    PlayAnimation("Sword");
                   //     break;
                    default:
                        // PlayAnimation("All");
                        PlayAnimation("Sword");
                        break;
                }
            }
            else
            {
                PlayAnimation("Knuckle");
            }
        }


        public void RebirthSkillOn(int weaponType)
        {
            string animName = string.Empty;

            switch (weaponType)
            {
                case 1: //권
                    animName = "Knuckle_01";
                    break;
                case 2: //검
                    animName = "Sword_01";
                    break;
                case 3: //도
                    animName = "Katana_01"; 
                    break;
                case 4: //창
                    animName = "Lance_01";
                    break;
                case 5: //퇴
                    animName = "Mace_01";
                    break;
            }

            Us = UnitState.RebirthSkill;

            if (animName != string.Empty)
                PlayAnimation(animName);
        }

        /// <summary>
        /// 환생 스킬 데미지 세팅(애니메이션 이벤트에서 호출)
        /// </summary>
        /// <param name="weaponType"></param>
        public void RebirthSkillDmgSet(int weaponType)
        {
            ActiveSkillData activeSkillData = DataManager.i.activeSkillTable.GetData(weaponType - 1);

            //target 1 = 자신, 2 = 대상, 3 = 전체
            if (activeSkillData.target == 1)
            {

            }
            else if (activeSkillData.target == 2)
            {
                if (MainTarget != null && MainTarget.Us != UnitState.Die)
                {
                    float dmg = GetDmg(activeSkillData.a_s_dmg_List);
                    MainTarget.Damaged(this, dmg, DmgType.Nomal);
                }
            }
            else if (activeSkillData.target == 3)
            {

            }
        }

        /// <summary>
        /// 환생 스킬 데미지 추출
        /// </summary>
        /// <param name="dmgDataList"></param>
        /// <returns></returns>
        private float GetDmg(List<A_S_Dmg_Data> dmgDataList)
        {
            float dmg = 0;
            foreach (var data in dmgDataList)
            {
                switch (data.dmgType)
                {
                    //고정값
                    case 1 :
                        dmg += data.dmgTypeValue;
                        break;
                    //내공 비례
                    case 2:
                        dmg += PlayerDataManager.ins.maxData.mp * (data.dmgTypeValue / 10000.0f);
                        break;
                    //외공 비례
                    case 3:
                        dmg += PlayerDataManager.ins.maxData.ap * (data.dmgTypeValue / 10000.0f);
                        break;
                }
            }

            return dmg;
        }

        /// <summary>
        /// 환생 스킬 속성 효과 정보 추출
        /// </summary>
        /// <param name="condDataList"></param>
        private void GetConditionData(List<A_S_Cond_Data> condDataList)
        {
            foreach (var data in condDataList)
            {
                switch (data.conditionType)
                {
                    //스턴
                    case 1:
                        break;
                    //슬로우
                    case 2:
                        break;
                    //도트 데미지
                    case 3:
                        break;
                    //체력회복
                    case 4:
                        break;
                    //내공회복
                    case 5:
                        break;
                    //외공회복
                    case 6:
                        break;
                }
            }
            
        }

        /// <summary>
        /// 환생 이펙트 세팅
        /// </summary>
        public void RebirthEffectOn()
        {
            transform.Find("Effect/Rebirth_1").gameObject.SetActive(true);
        }

        

    }

}