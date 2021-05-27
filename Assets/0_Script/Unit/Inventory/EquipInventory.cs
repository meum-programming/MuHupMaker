using System.Collections;
using System.Collections.Generic;
using A_Script;
using UnityEngine;

public class EquipInventory : BasicInventory
{
    public List<ItemData> Weapon = new List<ItemData>();
    public List<ItemData> Helmet = new List<ItemData>();
    public List<ItemData> Armor = new List<ItemData>();
    public List<ItemData> Pants = new List<ItemData>();
    public List<ItemData> Boots = new List<ItemData>();
    public List<ItemData> Guard = new List<ItemData>();

    public override void Add(ItemData i)
    {
        AddReceive(i);
        PlayerDataManager.ins.SaveChaItemData(InventoryType.Equipment, Total);
    }

    public override bool Remove(ItemData i)
    {
        bool result = base.Remove(i);
        PlayerDataManager.ins.SaveChaItemData(InventoryType.Equipment, Total);
        return result;
    }

    public override List<ItemData> GetList(ItemData i)
    {
        switch (i.itemtype)
        {
            case "W":
                return Weapon;
            case "H":
                return Helmet;
            case "A":
                return Armor;
            case "P":
                return Pants;
            case "B":
                return Boots;
            case "G":
                return Guard;
            default:
                return null;
        }
    }
    public override ItemData AddReceive(ItemData i)
    {
        if (GameManager.MaxInvenEquipCount <= Total.Count)
        {
            Debug.Log("EquipInvenFull");
            return null;
        }

        var target = GetList(i);

        var item = i.Copy();

        target.Add(item);

        if (i.itemtype == "W")
        {
            target.Sort((ItemData i1, ItemData i2) =>
            {
                //무기 종류 정렬
                if (i1.type > i2.type) return 1;
                else if (i1.type < i2.type) return -1;
                else if (i1.type == i2.type)
                {
                    //등급 정렬
                    if (i1.grade > i2.grade) return -1;
                    else if (i1.grade < i2.grade) return 1;
                }

                return 0;
            });
        }
        else
        {
            target.Sort((ItemData i1, ItemData i2) =>
            {
                if (i1.grade > i2.grade) return -1;
                else if (i1.grade < i2.grade) return 1;
                return 0;

            });
        }

        UpdateTotal();
        base.AddShow(i);
        return item;
    }
    public override void UpdateTotal()
    {
        Total.Clear();
        Total.AddRange(Weapon);
        Total.AddRange(Helmet);
        Total.AddRange(Armor);
        Total.AddRange(Pants);
        Total.AddRange(Boots);
        Total.AddRange(Guard);
    }
}
