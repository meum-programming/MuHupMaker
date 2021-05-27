using System.Collections;
using System.Collections.Generic;
using A_Script;
using UnityEngine;

public class SkillInventory : BasicInventory
{
    public List<ItemData> Kunckle = new List<ItemData>();
    public List<ItemData> Sword = new List<ItemData>();
    public List<ItemData> Katana = new List<ItemData>();
    public List<ItemData> Lance = new List<ItemData>();
    public List<ItemData> Mace = new List<ItemData>();
    public List<ItemData> Move = new List<ItemData>();
    public override void Add(ItemData i)
    {
        AddReceive(i);
        PlayerDataManager.ins.SaveChaItemData(InventoryType.Skill, Total);
    }
    public override bool Remove(ItemData i)
    {
        bool result = base.Remove(i);
        PlayerDataManager.ins.SaveChaItemData(InventoryType.Skill, Total);
        return result;
    }
    public override List<ItemData> GetList(ItemData i)
    {
        switch (i.type)
        {
            case 1:
                return Kunckle;
            case 2:
                return Sword;
            case 3:
                return Katana;
            case 4:
                return Lance;
            case 5:
                return Mace;
            case 6:
                return Move;
            default:
                return null;
        }
    }
    public override ItemData AddReceive(ItemData i)
    {
        if (GameManager.MaxInvenSkillCount <= Total.Count)
        {
            Debug.Log("SkillInvenFull");
            return null;
        }

        var target = GetList(i);
        var item = i.Copy();

        target.Add(item);
        target.Sort((ItemData i1, ItemData i2) =>
        {
            if (i1.grade > i2.grade) return -1;
            else if (i1.grade < i2.grade) return 1;
            return 0;

        });
        UpdateTotal();
        base.AddShow(i);
        return item;
    }
    public override void UpdateTotal()
    {
        Total.Clear();
        Total.AddRange(Kunckle);
        Total.AddRange(Sword);
        Total.AddRange(Katana);
        Total.AddRange(Lance);
        Total.AddRange(Mace);
        Total.AddRange(Move);
    }
}