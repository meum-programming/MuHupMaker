using System.Collections;
using System.Collections.Generic;
using A_Script;
using UnityEngine;

public class ConsumInventory : BasicInventory
{
    public List<ItemData> Consum = new List<ItemData>();

    public override void Add(ItemData i)
    {
        AddReceive(i);
    }
    public override List<ItemData> GetList(ItemData i)
    {
        switch (i.type)
        {
            default:
                return Consum;
        }
    }
    public override ItemData AddReceive(ItemData i)
    {
        if (GameManager.MaxInvenConsumCount <= Total.Count)
        {
            Debug.Log("ConsumInvenFull");
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
        base.AddShow(item);
        return item;
    }

    public override void UpdateTotal()
    {
        Total.Clear();
        Total.AddRange(Consum);
    }
}