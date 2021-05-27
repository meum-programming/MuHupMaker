using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    public class UnitInventory : UnitSkill
    {
        public Inventory Inventory;
        public virtual void SetInventory(Inventory IV)
        {
            if (IV != null)
                Inventory = IV;
        }
        public List<ItemDropGroupInfoData> Drop = new List<ItemDropGroupInfoData>();

        public List<ItemDropGroupInfoData> GetDropItem() {
            List<ItemDropGroupInfoData> items = new List<ItemDropGroupInfoData>();

            foreach (var d in Drop) {
                var R = Random.Range(0, 1f)*10000f;
                if (R < d.itemid_rate) {
                    items.Add(d);
                }
            }
            return items;
        }
    }
}
