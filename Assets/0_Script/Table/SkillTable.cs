using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    [System.Serializable]
    public class SkillTable : DataTable
    {
#if UNITY_EDITOR
        public DicSD skillInfos = new DicSD();
       // public Dictionary<int, SkillData> skillInfos = new Dictionary<int, SkillData>();
#else
    public Dictionary<int,SkillData> skillInfos = new Dictionary<int,SkillData>();
#endif


        public SkillTable()
        {

            
        }

        public override void Load(List<Dictionary<string, object>> _datas)
        {
            var dataEnumerator = _datas.GetEnumerator();
            while (dataEnumerator.MoveNext())
            {
                var data = dataEnumerator.Current;

                SkillData info = new SkillData();
                //.. TODO :: ADD -> WeaponPrefab,DismantleID,Sell,Description,AtlasName,IconName
                info.id = int.Parse(data["SkillID"].ToString());
                info.skillname = data["SkillName"].ToString(); //int.Parse(data["ItemType"].ToString());
                info.type = int.Parse(data["SkillType"].ToString());
                info.grade = int.Parse(data["Grade"].ToString());
                info.prefab = data["SkillPrefab"].ToString();
                info.skillFactorpowmp = int.Parse(data["FactorPowMP"].ToString());
                info.skillFactorpowap = int.Parse(data["FactorPowAP"].ToString());
                info.powMP = int.Parse(data["PowMP"].ToString());
                info.powAP = int.Parse(data["PowAP"].ToString());
                info.powmpdef = int.Parse(data["PowMPDef"].ToString());
                info.powapdef = int.Parse(data["PowAPDef"].ToString());
                info.skillmove = int.Parse(data["SkillMove"].ToString());
                info.skilllvid = int.Parse(data["SkillLvID"].ToString());
                info.use_MP = int.Parse(data["Use_MP"].ToString());
                info.use_AP = int.Parse(data["Use_AP"].ToString());
                info.conditionid_01 = int.Parse(data["ConditionID_01"].ToString());
                info.conditionid_02 = int.Parse(data["ConditionID_02"].ToString());
                info.conditionid_03 = int.Parse(data["ConditionID_03"].ToString());
                info.description = data["Description"].ToString();
                info.iconName =data["IconName"].ToString();

                skillInfos.Add(info.id, info);
            }
        }

        public SkillData GetskillInfos(int _skilID)
        {
            SkillData sd = null;
            if (skillInfos.TryGetValue(_skilID, out sd))
            {
                return sd;
            }
            return null;
        }
    }
}
