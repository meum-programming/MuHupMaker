using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    [System.Serializable]
    public class MonsterTable : DataTable
    {


#if UNITY_EDITOR
        public DicUD enemyInfos = new DicUD();
#else
    public Dictionary<int,UnitData> enemyInfos = new Dictionary<int,UnitData>();
#endif


        public MonsterTable()
        {


        }

        public override void Load(List<Dictionary<string, object>> _datas)
        {
            var dataEnumerator = _datas.GetEnumerator();
            while (dataEnumerator.MoveNext())
            {
                var data = dataEnumerator.Current;

                UnitData info = new UnitData();
                info.Id = int.Parse(data["MonsterID"].ToString());
                info.hp = int.Parse(data["HP"].ToString());
                info.powMP = int.Parse(data["PowMP"].ToString());
                info.powAP = int.Parse(data["PowAP"].ToString());
                info.powMPDef = int.Parse(data["PowMPDef"].ToString());
                info.powAPDef = int.Parse(data["PowAPDef"].ToString());
                info.atkSpeed = int.Parse(data["AtkSpeed"].ToString());
                info.movSpeed = float.Parse(data["MovSpeed"].ToString());

                info.exp = int.Parse(data["Exp"].ToString());
                info.gold = int.Parse(data["Gold"].ToString());
                info.drop = int.Parse(data["DropGroupItemID"].ToString());
                info.Prefab = data["MonsterPrefab"].ToString();
                info.name = data["MonsterName"].ToString();

                enemyInfos.Add(info.Id, info);
            }
        }
    }
}
