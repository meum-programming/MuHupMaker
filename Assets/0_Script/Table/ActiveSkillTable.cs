using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    public class ActiveSkillTable : DataTable
    {
        public List<ActiveSkillData> activeSkillList = new List<ActiveSkillData>();

        public override void Load(List<Dictionary<string, object>> _datas)
        {
            var dataEnumerator = _datas.GetEnumerator();

            activeSkillList = new List<ActiveSkillData>();
            while (dataEnumerator.MoveNext())
            {
                var data = dataEnumerator.Current;

                ActiveSkillData info = new ActiveSkillData();

                info.activeSkillID = int.Parse(data["ActiveSkillID"].ToString());
                info.activeSkillName = data["ActiveSkillName"].ToString();
                info.activeSkillPrefab = data["ActiveSkillPrefab"].ToString();
                info.activeSkillIcon = data["ActiveSkillIcon"].ToString();
                info.target = int.Parse(data["Target"].ToString());
                info.coolTime = float.Parse(data["CoolTime"].ToString());
                info.useHP = float.Parse(data["UseHP"].ToString());
                info.useMP = float.Parse(data["UseMP"].ToString());
                info.useAP = float.Parse(data["UseAP"].ToString());

                info.a_s_dmg_List = new List<A_S_Dmg_Data>()
                {
                    new A_S_Dmg_Data(int.Parse(data["DmgType_01"].ToString()), float.Parse(data["DmgTypeValue_01"].ToString())),
                    new A_S_Dmg_Data(int.Parse(data["DmgType_02"].ToString()), float.Parse(data["DmgTypeValue_02"].ToString())),
                    new A_S_Dmg_Data(int.Parse(data["DmgType_03"].ToString()), float.Parse(data["DmgTypeValue_03"].ToString()))
                };

                info.a_s_cond_List = new List<A_S_Cond_Data>()
                {
                    new A_S_Cond_Data(int.Parse(data["Condition_01"].ToString()), int.Parse(data["ConditionValue_01"].ToString()),float.Parse(data["ConditionTime_01"].ToString())),
                    new A_S_Cond_Data(int.Parse(data["Condition_02"].ToString()), int.Parse(data["ConditionValue_02"].ToString()),float.Parse(data["ConditionTime_02"].ToString())),
                    new A_S_Cond_Data(int.Parse(data["Condition_03"].ToString()), int.Parse(data["ConditionValue_03"].ToString()),float.Parse(data["ConditionTime_03"].ToString()))
                };

                activeSkillList.Add(info);
            }
        }

        public ActiveSkillData GetData(int _ID)
        {
            ActiveSkillData a_s_data = null;

            int checkId = _ID + 10001;

            foreach (var data in activeSkillList)
            {
                if (data.activeSkillID == checkId)
                {
                    a_s_data = data;
                    break;
                }
            }

            return a_s_data;
        }
    }
}

public class ActiveSkillData
{
    public int activeSkillID;
    public string activeSkillName;
    public string activeSkillPrefab;
    public string activeSkillIcon;
    public int target;
    public float coolTime;
    public float useHP;
    public float useMP;
    public float useAP;
    public List<A_S_Dmg_Data> a_s_dmg_List = new List<A_S_Dmg_Data>();
    public List<A_S_Cond_Data> a_s_cond_List = new List<A_S_Cond_Data>();
}

public class A_S_Dmg_Data
{
    public int dmgType;
    public float dmgTypeValue;

    public A_S_Dmg_Data() { }
    public A_S_Dmg_Data(int dmgType, float dmgTypeValue)
    {
        this.dmgType = dmgType;
        this.dmgTypeValue = dmgTypeValue;
    }
}

public class A_S_Cond_Data
{
    public int conditionType;
    public int conditionValue;
    public float conditionTime;

    public A_S_Cond_Data() { }
    public A_S_Cond_Data(int conditionType, int conditionValue, float conditionTime)
    {
        this.conditionType = conditionType;
        this.conditionValue = conditionValue;
        this.conditionTime = conditionTime;
    }
}