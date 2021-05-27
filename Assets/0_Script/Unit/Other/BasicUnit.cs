using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
namespace A_Script
{
    public abstract class BasicUnit : MonoBehaviour
    {
        public CharacterType charactertype;
        public UnitData Unitdata;
        protected UnitData OriginData;
        protected Action HPChangeEvent = null;
        protected Action APChangeEvent = null;
        protected Action MPChangeEvent = null;
        protected Action EXPChangeEvent = null;
        protected Action<DmgType,ShowType,float> ChangeDmgEvent = null;
        
        protected virtual void SetUnitData(UnitData UD)
        {
            OriginData = UD;
            Unitdata = UD.Copy();
            CurrentHP = Unitdata.hp;
            CurrentMP = Unitdata.mp;
            CurrentAP = Unitdata.ap;
            //load lv
            GetLvData(CharLv);
        }

        protected virtual void LvUp()
        {
        }
        protected virtual void GetLvData(int i, bool add = false) {
            var datasingle = CaracterDataManager.GetLvSingleData(charactertype, i);
            var datasingleexp = CaracterDataManager.GetLvSingleData(charactertype, i+1);
            var data = CaracterDataManager.GetLvData(charactertype, i);
            Unitdata.hp = OriginData.hp + data.hp;
            Unitdata.mp = OriginData.mp + data.mp;
            Unitdata.ap = OriginData.ap + data.ap;
           
            EXP = datasingleexp.exp;
            if (add)
            {
                CurrentHP += datasingle.hp;
                CurrentMP += datasingle.mp;
                CurrentAP += datasingle.ap;
            }
            LvUpEvent();
        }
        protected virtual void LvUpEvent() {

        }
     
        protected int _EXP;
        protected virtual int EXP
        {
            get
            {
                return _EXP;
            }
            set
            {
                _EXP = value;
            }
        }
        public int _CharLv;
        protected virtual int CharLv
        {
            get
            {
                return _CharLv;
            }
            set
            {
                _CharLv = value;
            }
        }
        protected virtual float HP
        {
            get
            {
                return Unitdata.hp;
            }
            set
            {
                Unitdata.hp = value;
             
            }
        }
       
        protected virtual float MP
        {
            get
            {
                return Unitdata.mp;
            }
            set
            {
                Unitdata.mp = value;
            }
        }
  
        protected virtual float AP
        {
            get
            {
                return Unitdata.ap;
            }
            set
            {
                Unitdata.ap = value;
             
            }
        }







        protected float _CurrentHP;
        protected virtual float CurrentHP
        {
            get
            {
                return _CurrentHP;
            }
            set
            {
                _CurrentHP = value;
                if (_CurrentHP > HP)
                    _CurrentHP = HP;
                HPChangeEvent?.Invoke();
            }
        }
        protected float _CurrentMP;
        protected virtual float CurrentMP
        {
            get
            {
                return _CurrentMP;
            }
            set
            {
                _CurrentMP = value;
                if (_CurrentMP > MP)
                    _CurrentMP = MP;
                MPChangeEvent?.Invoke();
            }
        }
        protected float _CurrentAP;
        protected virtual float CurrentAP
        {
            get
            {
                return _CurrentAP;
            }
            set
            {
                _CurrentAP = value;
                if (_CurrentAP > AP)
                    _CurrentAP = AP;
                APChangeEvent?.Invoke();
            }
        }
        protected int GetExp {
            get {
                return Unitdata.exp;
            }
        }

        protected int _CurrentEXP;
        protected virtual int CurrentEXP
        {
            get
            {
                return _CurrentEXP;
            }
            set
            {
                _CurrentEXP = value;

                if (_CurrentEXP >= 0)
                {
                    LvUp();
                }

                EXPChangeEvent?.Invoke();
            }
        }





        protected virtual int PowMP
        {
            get
            {
                return Unitdata.powMP;
            }
            set
            {
                Unitdata.powMP = value;
            }
        }
      
        protected virtual int PowAP
        {
            get
            {
                return Unitdata.powAP;
            }
            set
            {
                Unitdata.powAP = value;
            }
        }
   
        protected virtual int PowMPDef
        {
            get
            {
                return Unitdata.powMPDef;
            }
            set
            {
                Unitdata.powMPDef = value;
            }
        }
   
        protected virtual int PowAPDef
        {
            get
            {
                return Unitdata.powAPDef;
            }
            set
            {
                Unitdata.powAPDef = value;
            }
        }

        public virtual float AtkSpeed
        {
            get
            {
                return Unitdata.atkSpeed;
            }
            set
            {
                Unitdata.atkSpeed = value;
            }
        }
     
        public virtual float MovSpeed
        {
            get
            {
                return Unitdata.movSpeed;
            }
            set
            {
                Unitdata.movSpeed =value;
            }
        }
     
        public virtual float CrtRate
        {
            get
            {
                return Unitdata.crtRate*0.0001f;
            }
            set
            {
                Unitdata.crtRate = value;
            }
        }
     
        public virtual float CrtDmg
        {
            get
            {
                return Unitdata.crtDmg * 0.0001f;
            }
            set
            {
                Unitdata.crtDmg = value;
            }
        }

        public virtual float Dodge
        {
            get
            {
                return Unitdata.dodge;
            }
            set
            {
                Unitdata.dodge = value;
            }
        }
       
       
    }
}
