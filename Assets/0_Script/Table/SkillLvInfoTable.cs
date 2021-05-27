using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    [System.Serializable]
    public class SkillLvInfoTable : DataTable
    {
        public Dictionary<int, List<SkillLvInfoData>> skilllvInfos = new Dictionary<int, List<SkillLvInfoData>>();

#if UNITY_EDITOR
        public List<SkillLvInfoData> skilllvInfosView = new List<SkillLvInfoData>();
#endif

        public SkillLvInfoTable()
        {


        }

        public override void Load(List<Dictionary<string, object>> _datas)
        {
            var dataEnumerator = _datas.GetEnumerator();
            int index = -1;
            List<SkillLvInfoData> datas = new List<SkillLvInfoData>();
            while (dataEnumerator.MoveNext())
            {
                var data = dataEnumerator.Current;

                SkillLvInfoData info = new SkillLvInfoData();
                //.. TODO :: ADD -> WeaponPrefab,DismantleID,Sell,Description,AtlasName,IconName
                info.skilllvid = int.Parse(data["SkillLvID"].ToString());
                if (index == -1) {
                    index = info.skilllvid;
                }
                info.type = int.Parse(data["Type"].ToString()); //int.Parse(data["ItemType"].ToString());
                info.grade = int.Parse(data["Grade"].ToString());
                info.skilllv = int.Parse(data["SkillLv"].ToString());
                info.needexp = int.Parse(data["NeedExp"].ToString());
                info.powmp = int.Parse(data["PowMP"].ToString());
                info.powap = int.Parse(data["PowAP"].ToString());
 
                info.powmpdef = int.Parse(data["PowMPDef"].ToString());
                info.powapdef = int.Parse(data["PowAPDef"].ToString());
                info.usemp = int.Parse(data["UseMP"].ToString());
                info.useap = int.Parse(data["UseAP"].ToString());
#if UNITY_EDITOR
                skilllvInfosView.Add(info);
#endif
                datas.Add(info);
             
                if (index != info.skilllvid)
                {
                   // Debug.Log(info.skilllvid);
                    skilllvInfos.Add(index, datas);
                    datas = new List<SkillLvInfoData>();
                    index = info.skilllvid;
                }
            }
        }

       
    }
    [System.Serializable]
    public class SkillLvInfoData {
        public int skilllvid;
        public int type;
        public int grade;
        public int skilllv;
        public int needexp;

        public int powmpdef;
        public int powapdef;
        public int usemp;
        public int useap;
        public float getpowmp;
        public float getpowap;
        public int powmp
        {

            set
            {
                getpowmp = (float)value / 10000f;
            }
        }
        public int powap
        {

            set
            {
                getpowap = (float)value / 10000f;
            }
        }
    }
}
