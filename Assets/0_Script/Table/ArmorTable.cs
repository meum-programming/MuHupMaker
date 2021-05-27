using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A_Script
{

    [System.Serializable]
    public class ArmorTable : DataTable
    {
        public Dictionary<int,ArmorData> armorInfo = new Dictionary<int,ArmorData>();

        public override void Load(List<Dictionary<string, object>> _datas)
        {
            var dataEnumerator = _datas.GetEnumerator();
            while (dataEnumerator.MoveNext())
            {
                var data = dataEnumerator.Current;
                
                ArmorData info = new ArmorData();
                info.id = int.Parse(data["ArmorItemID"].ToString());
                info.name = data["ArmorItemName"].ToString();
                info.itemtype = data["ItemType"].ToString();
                info.grade = int.Parse(data["Grade"].ToString());
                info.powMPDef = int.Parse(data["PowMPDef"].ToString());
                info.powAPDef = int.Parse(data["PowAPDef"].ToString());
                info.factorPowMP = int.Parse(data["FactorPowMPDef"].ToString());
                info.factorPowAP = int.Parse(data["FactorPowAPDef"].ToString());
                info.enchantId = int.Parse(data["EnchantID"].ToString());
                info.dismantleId = int.Parse(data["DismantleID"].ToString());
                info.sell = int.Parse(data["Sell"].ToString());
                info.description = data["Description"].ToString();
                info.iconName = data["IconName"].ToString();

                armorInfo.Add(info.id, info);
            }
        }

        public ArmorData GetArmorInfo(int _armorID)
        {
            ArmorData ad = null;
            if (armorInfo.TryGetValue(_armorID, out ad))
            {
                return ad;
            }
            return null;
        }
    }
}
