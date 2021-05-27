using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
namespace A_Script
{
    public class Unit : UnitInitialize
    {
        public float FollowTime = 5;

        public UnitType UT;

        /// <summary>
        /// 공격 데미지 세팅
        /// </summary>
        /// <param name="dmgtype"></param>
        /// <returns></returns>
        public float Atk(out DmgType dmgtype)
        {
            //Debug.Log("===  전투 공식 시작   " + UT + "(이)가 공격 === ");
            dmgtype = DmgType.Nomal;

            float _powMP = GetPowMP();
            float _powAP = GetPowAP();

            //내공,외공 사용 처리
            if (UT == UnitType.Player && Nomal.IsIt)
            {
                PlayerDataManager.ins.AddValue(PlayerStatusEnum.MP, Nomal.Skill.use_MP);
                PlayerDataManager.ins.AddValue(PlayerStatusEnum.AP, Nomal.Skill.use_AP);
            }

            float totalDamage = (_powMP + _powAP);

            //Debug.Log(string.Format("3. PowMP({0}) + PowAP({1}) = TotalDamage({2})", _powMP, _powAP, totalDamage));

            float result = totalDamage;

            var R = UnityEngine.Random.Range(0,1f);

            bool crtFlag = (CrtDmg != 0 && R < CrtRate);

            if (crtFlag) {
                dmgtype = DmgType.Critical;
                result = totalDamage * CrtDmg;
                if (UT == UnitType.Player) {
                    if (GameManager.i.ActiveStageCamera != null) {
                        GameManager.i.ActiveStageCamera.Shack();
                    }
                }
            }

            //Debug.Log(string.Format("4. TotalDamage({0}) = totalDamage({1}) * CrtDmg({2})  크리티컬 = {3}", result, totalDamage, CrtDmg, crtFlag));
            //
            return result;
        }

        /// <summary>
        /// 내력값 계산
        /// </summary>
        /// <returns></returns>
        public float GetPowMP()
        {
            //기본 내력 값 세팅
            float max_mp = UT == UnitType.Player ? PlayerDataManager.ins.maxData.mp : MP;

            //무공 내력 배율
            float skill_factorPowMP = 0;
            //무공 고정값
            float skill_powMP = 0;
            if (Nomal.IsIt)
            {
                skill_factorPowMP = Nomal.Skill.GetLvFactorPowMP;
                skill_powMP = Nomal.Skill.powMP;
            }

            //무기 배율
            float weapon_factorPowMP = 0;
            //무기 고정값
            float weapon_powMP = 0;
            if (Weapon.isit)
            {
                WeaponData weaponData = DataManager.i.WaeponTable.GetWeaponInfo(Weapon.item.id);
                weapon_factorPowMP = weaponData.factorPowMP;
                weapon_powMP = weaponData.powMP;
            }

            //내력 최종값 계산
            float dmg = ((max_mp * skill_factorPowMP) + skill_powMP) + ((max_mp * weapon_factorPowMP) + weapon_powMP);

            if (dmg == 0)
            {
                dmg = PowMP;
            }

            //Debug.Log(string.Format("1. 내력(PowMP) -> {0} = (({1} * {2}) + {3}) + (({1} * {4}) + {5})"
              //                                       , dmg, max_mp, skill_factorPowMP, skill_powMP, weapon_factorPowMP, weapon_powMP));

            return dmg;
        }

        /// <summary>
        /// 외력값 계산
        /// </summary>
        /// <returns></returns>
        public float GetPowAP()
        {
            //기본 외력 값 세팅
            float max_ap = UT == UnitType.Player ? PlayerDataManager.ins.maxData.ap : AP;

            //무공 외력 배율
            float skill_factorPowAP = 0;
            //무공 고정값
            float skill_powAP = 0;
            if (Nomal.IsIt)
            {
                skill_factorPowAP = Nomal.Skill.GetLvFactorPowAP;
                skill_powAP = Nomal.Skill.powAP;
            }

            //무기 배율
            float weapon_factorPowAP = 0;
            //무기 고정값
            float weapon_powAP = 0;

            if (Weapon.isit)
            {
                WeaponData weaponData = DataManager.i.WaeponTable.GetWeaponInfo(Weapon.item.id);
                weapon_factorPowAP = weaponData.factorPowAP;
                weapon_powAP = weaponData.powAP;
            }

            //외력 최종값 계산
            float dmg = ((max_ap * skill_factorPowAP) + skill_powAP) + ((max_ap * weapon_factorPowAP) + weapon_powAP);

            if (dmg == 0)
            {
                dmg = PowAP;
            }

            //Debug.Log(string.Format("2. 외력(PowAP) -> {0} = (({1} * {2}) + {3}) + (({1} * {4}) + {5})"
              //                                       ,dmg, max_ap, skill_factorPowAP, skill_powAP, weapon_factorPowAP, weapon_powAP));

            return dmg;
        }

        public override void UnitInitDirect(UnitData UD, Inventory IV)
        {
            Waypoint = GameManager.i.Waypoints;
            SetUnitData(UD);
            SetInventory(IV);
            Navagent = GetComponent<NavMeshAgent>();

            Navagent.updateRotation = false;
            Unitname = "Unit_" + index.ToString();
            var temp = new GameObject();
            temp.transform.SetParent(this.transform);
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localRotation = Quaternion.identity;
            temp.transform.localScale = Vector3.one;
            var f = temp.AddComponent<Finder>();
            f.Init(this, findlayer);
            var c = temp.AddComponent<SphereCollider>();
            c.radius = Findrange * 0.5f;
            c.isTrigger = true;
            var r = temp.AddComponent<Rigidbody>();
            r.useGravity = false;
            Ani = GetComponent<Animator>();
            isinit = true;
            Afterattackendevent += () => { MainTarget = null; };

            ItemSaveData itemSaveData = PlayerDataManager.ins.itemSaveData;
            if (UT == UnitType.Player && itemSaveData != null)
            {
                List<int> saveItemData = itemSaveData.itemIDList;
                saveItemData.AddRange(itemSaveData.skillItemIDList);

                //아이템 세팅
                List<ItemDropGroupInfoData> listData = new List<ItemDropGroupInfoData>();

                for (int i = 0; i < saveItemData.Count; i++)
                {
                    ItemDropGroupInfoData data = new ItemDropGroupInfoData();
                    data.itemid = saveItemData[i];
                    listData.Add(data);
                }

                DropItemSet(listData);


            }
        }

        Transform looktarget;
        public float damping = 1;
        public float Attackdamping = 1;
        public float AttackRotationMaxtime = 0.2f;
        public float AttackRotationlocaltime =0;
        protected Transform t;
        protected virtual void Start()
        {
            t = transform;
            Afterattackendevent += startfollow;
            Preattackendevent += FindTarget;
        }
       
        protected bool UpdateRotation = true;
        private void LateUpdate()
        {
            if (Navagent == null)
                return;
            if (Us == UnitState.Idle)
                return;
            if (Us == UnitState.Move|| Us == UnitState.ForceMove)
            {
                Navagent.isStopped = false;

                var vel = Navagent.velocity;
                Quaternion rotation = Quaternion.identity;
                if (vel != Vector3.zero)
                {
                    rotation = Quaternion.LookRotation(Navagent.velocity.normalized);
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }
            else if (Us == UnitState.Attack)
            {
                //메인타겟
                if (Look && (MainTarget != null && AttackRotationlocaltime < AttackRotationMaxtime)) {
                    var dt = Time.deltaTime;
                    AttackRotationlocaltime += dt;
                    Quaternion rotation = Quaternion.LookRotation(MainTarget.transform.position - t.position);
                    if (!Smooth)
                    {
                        t.rotation = rotation;
                    }
                    else
                    {
                        t.rotation = Quaternion.Slerp(t.rotation, rotation, dt * Attackdamping);
                    }
                    //Debug.LogError("rotation");
                } 
            }
            else if(Us == UnitState.RebirthSkill)
            {
                Navagent.isStopped = true;
            }
        }
        public bool Look = true;
        public bool Smooth = true;
        public float Preattckspeed;
        public float Afterattckspeed;
        public Action Preattackstartevent = null;
        public Action Preattackendevent = null;
        public Action Aftackstartevent = null;
        public Action Aftackendevent = null;
        public Action Afterattackstartevent = null;
        public Action Afterattackendevent = null;
        List<Unit> target = new List<Unit>();
    
        public void Attackoder(List<Unit> _target)
        {
            if (Us == UnitState.Attack || Us == UnitState.ForceAttack)
                return;
            target = _target;
            Preattackstartevent?.Invoke();
            //Start preattackanimation
            attackaniplay();
        }
       protected Unit MainTarget = null;
        public void Attackoder(Unit _target, bool force = false)
        {
     
                if (Us == UnitState.Attack|| Us == UnitState.ForceAttack)
                    return;
     
            target.Clear();
         
            target.Add(_target);
         
      
            Preattackstartevent?.Invoke();
            //Start preattackanimation
            attackaniplay();
        }
       
        void attackaniplay() {
          //  Debug.Log("attackaniplay");
            Navagent.ResetPath();
            Navagent.velocity = Vector3.zero;
            Us = UnitState.Attack;

            //   PlayAnimation("none",true);
            Attackanimation();


        }
        protected virtual void Attackanimation() {
            PlayAnimation("attack");
        }
       protected virtual void FindTarget() {
            foreach (var u in target)
            {
                if (u != null)
                {
              
                    MainTarget = u;
                    break;
                }


            }
        }
        public bool attack()
        {
            bool isdie = false;
            DmgType dmgtype;
            var atk = Atk(out dmgtype);

            if (MainTarget != null && MainTarget.Damaged(this, atk, dmgtype))
            {
                isdie = true;
                GameManager.i.FieldBossCntAdd(1);

                if (MainTarget.Unitdata.bossOn)
                {
                    if (GameManager.i.bossKillEvent != null)
                    {
                        GameManager.i.bossKillEvent();
                    }
                }
            }
            Aftackendevent?.Invoke();
            Afterattackstartevent?.Invoke();
            //Start afterattackanmation
            return isdie;
        }
        public void Avoid(Unit u) {
            Debug.Log(u.name +" avoid "+this.name+" attack" );
        }
        public (float ap,float mp) GetTotalPowDef() {

            if (UT == UnitType.Unit)
                return (PowAPDef, PowMPDef);
            
            float _Ap = 0;
            float _Mp = 0;

            float player_max_AP = PlayerDataManager.ins.maxData.ap;
            float player_max_MP = PlayerDataManager.ins.maxData.mp;

            if (Armor.isit)
            {
                ArmorData armorData = DataManager.i.armorTable.GetArmorInfo(Armor.item.id);
                _Ap += (armorData.powAPDef + (player_max_AP * armorData.factorPowAP));
                _Mp += (armorData.powMPDef + (player_max_MP * armorData.factorPowMP));
            }
            if (Helmet.isit)
            {
                ArmorData armorData = DataManager.i.armorTable.GetArmorInfo(Helmet.item.id);
                _Ap += (armorData.powAPDef + (player_max_AP * armorData.factorPowAP));
                _Mp += (armorData.powMPDef + (player_max_MP * armorData.factorPowMP));
            }
            if (Pants.isit)
            {
                ArmorData armorData = DataManager.i.armorTable.GetArmorInfo(Pants.item.id);
                _Ap += (armorData.powAPDef + (player_max_AP * armorData.factorPowAP));
                _Mp += (armorData.powMPDef + (player_max_MP * armorData.factorPowMP));
            }
            if (Boots.isit)
            {
                ArmorData armorData = DataManager.i.armorTable.GetArmorInfo(Boots.item.id);
                _Ap += (armorData.powAPDef + (player_max_AP * armorData.factorPowAP));
                _Mp += (armorData.powMPDef + (player_max_MP * armorData.factorPowMP));
            }
            if (Guard.isit)
            {
                ArmorData armorData = DataManager.i.armorTable.GetArmorInfo(Guard.item.id);
                _Ap += (armorData.powAPDef + (player_max_AP * armorData.factorPowAP));
                _Mp += (armorData.powMPDef + (player_max_MP * armorData.factorPowMP));
            }

            return (_Ap, _Mp);
        }

        public virtual bool Damaged(Unit u, float Damage, DmgType dmgtype)
        {
            var avoid = UnityEngine.Random.Range(0, 1f);
            if (avoid < Dodge) {
                u.Avoid(this);
                return false;
            }

            var def = GetTotalPowDef();

            float totalDamage =  Mathf.Max((Damage) - (def.mp + def.ap) , 1);

            //Debug.Log(string.Format("5. TotalDamage({0}) = TotalDamage({1}) - (PowMPDef({2}) + PowAPDef({3}))", totalDamage, Damage, def.mp, def.ap));

            float _currentHP = UT == UnitType.Player ? PlayerDataManager.ins.p_data.currentData.hp : CurrentHP;

            //Debug.Log(string.Format("6. HP({0}) - TotalDamage({1})", _currentHP, totalDamage));

            //죽음
            if (_currentHP - totalDamage < 0)
            {
                Die(u);

                if (UT == UnitType.Player)
                {
                    PlayerDataManager.ins.AddValue(PlayerStatusEnum.HP, totalDamage);
                }
                else
                {
                    CurrentHP = 0;
                }

                ChangeDmgEvent?.Invoke(dmgtype, ShowType.HP, totalDamage);
                return true;
            }
            else
            {
                if (UT == UnitType.Player)
                {
                    PlayerDataManager.ins.AddValue(PlayerStatusEnum.HP,totalDamage);
                }
                else
                {
                    CurrentHP -= totalDamage;
                }
     
                ChangeDmgEvent?.Invoke(dmgtype, ShowType.HP, totalDamage);
            }

            return false;
        }
        protected Action<string> DropEvent = null;
        public virtual void KillEvent(Unit u)
        {
            //Debug.Log(this.name + " Kill -> " + u.name);
            //애니체크, 
            //CurrentEXP += (u.GetExp);
            PlayerDataManager.ins.AddExp(u.GetExp);

            if (Nomal.IsIt)
            {
                Nomal.Skill.currentexp += u.GetExp;
            }
            if (Speed.IsIt)
            {
              //  Speed.Skill.currentexp += u.GetExp;
            }
            if (Special.IsIt)
            {
                Special.Skill.currentexp += u.GetExp;
            }
            if (Resection.IsIt)
            {
                Resection.Skill.currentexp += u.GetExp;
            }

            DropItemSet(u.GetDropItem());
            
            Exit(u);
        }

        public void DropItemSet(List<ItemDropGroupInfoData> items)
        {
            foreach (var item in items)
            {
                var id = item.itemid;

                switch (item.droptype)
                {
                    case DropType.Item:
                        var w = DataManager.i.totalTable.totalInfo[id];
                        Inventory.Equip.Add(w);
                        DropEvent?.Invoke(w.name);
                        break;
                    case DropType.Skill:
                        var s = DataManager.i.skillitemTable.skillitemInfos[id];
                        Inventory.Skill.Add(s);
                        DropEvent?.Invoke(s.name);
                        break;
                }

            }
        }
        public Action<Unit> DieEvent = null;
        public virtual void Die(Unit u)
        {
          
                

       //     Debug.Log(u.Unitname + " -> " + Unitname + " : Kill");
            u.KillEvent(this);
            DieEvent?.Invoke(this);
            if (Us == UnitState.Die)
                return;
            Us = UnitState.Die;
            Navagent.ResetPath();
            Navagent.isStopped = true;
        
            DieDestroy();
        }
        public virtual void DieDestroy() {
            findoff();
            follwoff();
            WaypointExploreroff();
            GetComponent<Collider>().enabled = false;
            Navagent.enabled = false;
            PlayAnimationCrossfade("die");
            Destroy(this.gameObject,3);
        }

        public float Findrange = 10f;
    
        IEnumerator follow()
        {
            WaypointExploreroff();
      
            while (Findlist.Count>0)
            {
                float target = GetLenth(t, Findlist[0].transform);
                Unit TargetUnit = Findlist[0];
                for (int i = 1; i < Findlist.Count; i++)
                {
                    var l = GetLenth(t, Findlist[i].transform);
                    if (target > l)
                    {
                        TargetUnit = Findlist[i];
                        target = l;
                    }

                }
                float r = 1;
                if (Weapon.isit)
                {
                    r = 1*Weapon.item.range/10000f;
                    // Debug.Log(target + " : " + SelectWeapon.range);

                }
            //     Debug.Log(this.name + " : " +target + " : " + r);
                if (target <r)
                {

                    Attackoder(TargetUnit);
                    follwoff();
                    yield break;

                }
                Move(TargetUnit.transform.position);
                yield return new WaitForSeconds(0.1f);
            }
            Us = UnitState.Move;
            startfollow();
        }
       protected float GetLenth(Transform a, Transform b) {
            var pos = (a.position-b.position).sqrMagnitude;
            return Math.Abs(pos);
        }

        protected Coroutine findcoroutine = null;
        protected Coroutine followcoroutine = null;
        public List<Unit> Findlist = new List<Unit>();
        public void Enter(Unit u)
        {
            if (Findlist.Contains(u)||Us == UnitState.Die)
                return;
            Findlist.Add(u);
            Debug.Log(this.name + " : Enter : " + u.name);
            /*
            if (findcoroutine == null)
            {

                findcoroutine = StartCoroutine(Stay());
            }
            */
            startfollow();
        }

        protected Action FollowStartEvent = null;
        protected void startfollow() {
            if (Us == UnitState.Attack||Us == UnitState.ForceMove|| Us == UnitState.ForceAttack)
                return;
            Debug.Log("startfollow");
                Us = UnitState.Move;
            follwoff();
            if (followcoroutine == null&& Findlist.Count > 0)
            {
                FollowStartEvent?.Invoke();
                PlayAnimationCrossfade(MoveAni);
                followcoroutine = StartCoroutine(follow());
            }else if(Findlist.Count == 0)
            {
                UnitHpRollBack();
                StartWaypointExplorer();
            }
        }
        void UnitHpRollBack() {
            if (UT == UnitType.Unit)
            {
                CurrentHP = HP;
            }
        }
        public void Exit(Unit u)
        {//
         //   Debug.LogError("Exit" + u.name);
            if (Findlist.Contains(u))
            {
              //  Debug.LogError("Contains" + u.name);
                if (Findlist.Remove(u)) {
                  //  Debug.LogError("Remove" + Findlist.Count);
                }

                if (Findlist.Count == 0)
                {
                    UnitHpRollBack();
                    if (Us == UnitState.Attack || Us == UnitState.ForceMove)
                    {
                        findoff();
                        follwoff();
                        if (Us == UnitState.Attack)
                        {
                            StartWaypointExplorer();
                           
                        }
                      
                    }
                    
                
                
                }
            }

        }
       protected void findoff() {
            if (findcoroutine != null)
            {
                StopCoroutine(findcoroutine);
            }
            findcoroutine = null;
        }
        protected void follwoff() {
            if (followcoroutine != null)
            {
                StopCoroutine(followcoroutine);
            }
            followcoroutine = null;
        }
        float findupdatetime = 0.1f;
        /*
        IEnumerator Stay()
        {
            while (true)
            {

                yield return new WaitForSeconds(findupdatetime);
            }
        }
        */
    }

	public enum UnitType {
		Unit, Player
	}
	public enum UnitState {
        Idle,Move,Attack,Die, ForceMove, ForceAttack, RebirthSkill
    }

	public enum DmgType {
		Nomal, Critical
	}

	public enum ShowType {
		AP, HP
	}

	[System.Serializable]
    public class UnitData
    {
        public string name;
        public int index;
        public int Id;
        public float hp;
        public float mp;
        public float ap;
        public int powMP;
        public int powAP;
        public int powMPDef;
        public int powAPDef;
        public float atkSpeed;
        public float movSpeed;
        public float crtDmg;
        public float dodge;
        public float crtRate;
        public int exp;
        public int gold;
        public int drop;
        public string Prefab;
        public bool bossOn;

        public UnitData Copy()
        {
            var ud = new UnitData();
            ud.name = name;
            ud.index = index;
            ud.Id = Id;
            ud.hp = hp;
            ud.mp = mp;
            ud.ap = ap;
            ud.powMP = powMP;
            ud.powAP = powAP;
            ud.powMPDef = powMPDef;
            ud.powAPDef = powAPDef;
            ud.atkSpeed = atkSpeed;
            ud.movSpeed = movSpeed;
            ud.crtDmg = crtDmg;
            ud.dodge = dodge;
            ud.crtRate = crtRate;

            ud.Prefab = Prefab;
            ud.exp = exp;
            ud.gold = gold;
            ud.drop = drop;
            ud.bossOn = bossOn;

            return ud;
        }
    }
}