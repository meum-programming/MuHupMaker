using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A_Script
{

    [System.Serializable]
    public class WeaponTable : DataTable
    {


    public Dictionary<int,WeaponData> weaponInfos = new Dictionary<int,WeaponData>();



        public WeaponTable()
        {


        }

        public override void Load(List<Dictionary<string, object>> _datas)
        {
            var dataEnumerator = _datas.GetEnumerator();
            while (dataEnumerator.MoveNext())
            {
                var data = dataEnumerator.Current;
                /*
                foreach (var d in data) {
                    Debug.Log(d.Key);
                }
                */
                WeaponData info = new WeaponData();
                //.. TODO :: ADD -> WeaponPrefab,DismantleID,Sell,Description,AtlasName,IconName
                info.id = int.Parse(data["WeaponItemID"].ToString());
                info.name = data["WeaponItemName"].ToString();
                info.itemtype = data["ItemType"].ToString(); //int.Parse(data["ItemType"].ToString());
                info.type = int.Parse(data["Type"].ToString());
                info.grade = int.Parse(data["Grade"].ToString());
                info.prefab = data["WeaponItemPrefab"].ToString();
                info.powMP = int.Parse(data["PowMP"].ToString());
                info.powAP = int.Parse(data["PowAP"].ToString());
                info.factorPowMP = int.Parse(data["FactorPowMP"].ToString());
                info.factorPowAP = int.Parse(data["FactorPowAP"].ToString());
                info.range = float.Parse(data["AttackRange"].ToString());
                info.speed = int.Parse(data["AttackSpeed"].ToString());
                info.enchantId = int.Parse(data["EnchantID"].ToString());
                info.dismantleId = int.Parse(data["DismantleID"].ToString());
                info.sell = int.Parse(data["Sell"].ToString());
                info.description = data["Description"].ToString();
                info.iconName = data["IconName"].ToString();

                weaponInfos.Add(info.id, info);
            }
        }

        public WeaponData GetWeaponInfo(int _weaponID)
        {
            WeaponData wd = null;
            if (weaponInfos.TryGetValue(_weaponID, out wd))
            {
                return wd;
            }
            return null;
        }
    }
}
