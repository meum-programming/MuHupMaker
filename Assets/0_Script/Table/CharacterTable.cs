using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    [System.Serializable]
    public class CharacterTable : DataTable
    {
#if UNITY_EDITOR
        public DicUD charInfos = new DicUD();
#else
    public  Dictionary<int,UnitData> charInfos = new Dictionary<int,UnitData>();
#endif

        public CharacterTable()
        {


        }

        public override void Load(List<Dictionary<string, object>> _datas)
        {
            var dataEnumerator = _datas.GetEnumerator();
            while (dataEnumerator.MoveNext())
            {
                var data = dataEnumerator.Current;

                UnitData info = new UnitData();
                info.Id = int.Parse(data["CharID"].ToString());
                info.hp = int.Parse(data["HP"].ToString());
                info.mp = int.Parse(data["MP"].ToString());
                info.ap = int.Parse(data["AP"].ToString());
                info.powMP = int.Parse(data["PowMP"].ToString());
                info.powAP = int.Parse(data["PowAP"].ToString());
                info.powMPDef = int.Parse(data["PowMPDef"].ToString());
                info.powAPDef = int.Parse(data["PowAPDef"].ToString());
                info.atkSpeed = int.Parse(data["AtkSpeed"].ToString());
                info.movSpeed = float.Parse(data["MovSpeed"].ToString());
                info.crtRate = int.Parse(data["CrtRate"].ToString());
                info.crtDmg = int.Parse(data["CrtDmg"].ToString());
                info.dodge = int.Parse(data["Dodge"].ToString());
                info.Prefab = data["CharPrefab"].ToString();

                charInfos.Add(info.Id, info);
            }
        }
    }
}
