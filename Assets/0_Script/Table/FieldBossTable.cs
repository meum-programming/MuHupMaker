using System.Collections;
using System.Collections.Generic;
using A_Script;
using UnityEngine;
using System.Linq;

namespace A_Script
{
    public class FieldBossTable : DataTable
    {
        public Dictionary<string,FieldBossData> dataDic = new Dictionary<string, FieldBossData>();

        public override void Load(List<Dictionary<string, object>> _datas)
        {
            var dataEnumerator = _datas.GetEnumerator();

            dataDic = new Dictionary<string, FieldBossData>();

            while (dataEnumerator.MoveNext())
            {
                var data = dataEnumerator.Current;

                FieldBossData info = new FieldBossData();

                info.Id = int.Parse(data["MonsterID"].ToString());
                info.name = data["MonsterName"].ToString();
                info.sceneName = data["SceneName"].ToString();
                info.maxFieldBossCount = int.Parse(data["FieldBossCount"].ToString());
                info.lv = int.Parse(data["Lv"].ToString());
                info.hp = int.Parse(data["HP"].ToString());
                info.powMP = int.Parse(data["PowMP"].ToString());
                info.powAP = int.Parse(data["PowAP"].ToString());
                info.powMPDef = int.Parse(data["PowMPDef"].ToString());
                info.powAPDef = int.Parse(data["PowAPDef"].ToString());
                info.atkSpeed = int.Parse(data["AtkSpeed"].ToString());
                info.movSpeed = int.Parse(data["MovSpeed"].ToString());
                info.exp = int.Parse(data["Exp"].ToString());
                info.gold = int.Parse(data["Gold"].ToString());
                info.drop = int.Parse(data["DropGroupItemID"].ToString());

                dataDic.Add(info.sceneName,info);
            }
        }

        public FieldBossData GetFieldBossInfo(string sceneName)
        {
            FieldBossData data = null;
            if (dataDic.TryGetValue(sceneName, out data))
            {
                return data;
            }
            return null;
        }

        public bool GetFieldBossData(int id, out UnitData value)
        {
            FieldBossData fieldBossData = dataDic.Select(_ => _.Value).Where(data => data.Id == id).FirstOrDefault();

            value = fieldBossData;
            value.bossOn = true;

            if (fieldBossData == null)
            {
                return false;
            }

            return true;
        }
    }
}

[System.Serializable]
public class FieldBossData : UnitData
{
    public string sceneName;
    public int currnetFieldBossCount;
    public int maxFieldBossCount;
    public int lv;
}