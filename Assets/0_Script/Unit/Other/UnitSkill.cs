using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace A_Script
{
    public class UnitSkill : UnitEquip
    {
        public SubSkillData Nomal = new SubSkillData(SkillType.Nomal); 
        public SubSkillData Speed=new SubSkillData(SkillType.Speed);
        public SubSkillData Special=new SubSkillData(SkillType.Special);
        public SubSkillData Resection=new SubSkillData(SkillType.Resection);

        public void SkillInit() {
            Speed.ActiveSkill += SpeedActive;
            Speed.UnActiveSkill += SpeedUnActive;
        }
        float NavSpeed = 2.5f;
        public void SpeedActive(SkillData sd)
        {
            switch (sd.id)
            {
                case 600101:
                    MoveAni = "run";
                    NavSpeed += sd.skillmove;
                    Navagent.speed = NavSpeed;
                  //  Debug.LogError(sd.skillmove + " : " + NavSpeed + " : " + Navagent.speed);
                    if (Aniname == "walk") {
                        PlayAnimationCrossfade(MoveAni);
                    }
                    break;
            }
        }
        public void SpeedUnActive(SkillData sd)
        {
            switch (sd.id)
            {
                case 600101:
                    MoveAni = "walk";
                    NavSpeed  = 2.5f;
                    Navagent.speed = NavSpeed;
                    break;
            }
        }
        public SubSkillData GetSkill(SkillType st)
        {
            switch (st)
            {
                case SkillType.Nomal:
                    return Nomal;
                case SkillType.Speed:
                    return Speed;
                case SkillType.Special:
                    return Special;
                case SkillType.Resection:
                    return Resection;

            }
            return null;
        }
        public override void EquipInit()
        {
            base.EquipInit();
            Weapon.RemoveCallback += Nomal.Remove;
            Nomal.ItemCheck += WaeponCheck;
            SkillInit();
        }
        bool WaeponCheck(SkillData sd) {
            if (Weapon.isit) {
                return Weapon.item.type == sd.type;
            }

            return false;
        }

    }
    public class SubSkillData {
        public bool IsIt = false;
        public SkillData Skill;
        public SkillType st;
        public Func<SkillData,bool> ItemCheck = null;
        public Action<SkillData> ActiveSkill = null;
        public Action<SkillData> UnActiveSkill = null;
        public SubSkillData(SkillType _st)
        {
            st = _st;
        }
        public SubSkillData()
        {

        }
        public void Set(SkillData data , bool setOnly = false)
        {
            if (st == SkillType.Nomal) {
                if (!(bool)ItemCheck?.Invoke(data))
                    return;
            }
            if (IsIt)
            {
                Remove();
            }

            Skill = data;
            IsIt = true;
            if (isText)
            {
                foreach (var u in updatetext)
                {
                    u.text = Skill.skillname;
                }
            }
            if (isImage)
            {
                foreach (var u in updateImage)
                {
                    u.enabled = true;

                    u.sprite = BasicPool.i.Get(data.iconName, RPath.SkillIcon);
                }
            }
            
            Skill.E = true;
            if (setOnly == false)
            {
                PlayerDataManager.ins.SaveSkillData();
            }
            
            ActiveSkill?.Invoke(Skill);
        }
        public void Remove()
        {
            if (Skill != null)
            {
                Skill.E = false;
                UnActiveSkill?.Invoke(Skill);
            }
            Skill = null;
            IsIt = false;
            if (isText)
            {
                foreach (var u in updatetext)
                {
                    u.text = "";
                }
            }
            if (isImage)
            {
                foreach (var u in updateImage)
                {
                    u.enabled = false;

                    u.sprite = null;
                }
            }
            PlayerDataManager.ins.SaveSkillData();
        }

        public List<Text> updatetext = new List<Text>();
        bool isText = false;
        bool isImage = false;
        public List<Image> updateImage = new List<Image>();
        public void SetText(Text _t) {
            updatetext.Add(_t);
            isText = true;
        }
        public void SetImage(Image _t)
        {
            Debug.Log("SetImage");
            updateImage.Add(_t);
            _t.enabled = false;
            isImage = true;
        }

    }
}
