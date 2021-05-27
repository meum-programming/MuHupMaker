using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    [System.Serializable]
    public class SkillItemTable : DataTable
    {
#if UNITY_EDITOR
        public DicSID skillitemInfos = new DicSID();
        // public Dictionary<int, SkillData> skillInfos = new Dictionary<int, SkillData>();
#else
    public Dictionary<int,SkillItemData> skillitemInfos = new Dictionary<int,SkillItemData>();
#endif


        public SkillItemTable()
        {


        }

        public override void Load(List<Dictionary<string, object>> _datas)
        {
            var dataEnumerator = _datas.GetEnumerator();
            while (dataEnumerator.MoveNext())
            {
                var data = dataEnumerator.Current;

                SkillItemData info = new SkillItemData();
                //.. TODO :: ADD -> WeaponPrefab,DismantleID,Sell,Description,AtlasName,IconName
                info.id = int.Parse(data["SkillItemID"].ToString());
                info.name = data["SkillItemName"].ToString(); //int.Parse(data["ItemType"].ToString());
                info.itemtype = data["ItemType"].ToString();
                info.type = int.Parse(data["SkillType"].ToString());
                info.grade = int.Parse(data["Grade"].ToString());
                info.prefab = data["SkillItemPrefab"].ToString();
                info.sell = int.Parse(data["Sell"].ToString());
 
                info.description = data["Description"].ToString();
                info.iconName = data["IconName"].ToString();


                skillitemInfos.Add(info.id, info);
            }
        }

        public SkillItemData GetskillitemInfos(int _ID)
        {
            SkillItemData sd = null;
            if (skillitemInfos.TryGetValue(_ID, out sd))
            {
                return sd;
            }
            return null;
        }
    }
}
