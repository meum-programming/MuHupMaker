using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicInventory
{
    public List<ItemData> Total = new List<ItemData>();
    public Dictionary<int, List<ItemData>> Show = new Dictionary<int, List<ItemData>>();
    public abstract List<ItemData> GetList(ItemData i);
    public virtual void Add(ItemData i)
    {
        AddShow(i);
    }
    public abstract ItemData AddReceive(ItemData i);
    public abstract void UpdateTotal();
    public virtual bool Remove(ItemData i)
    {
        var target = GetList(i);
        var b = target.Remove(i);

        RemoveShow(i);
        UpdateTotal();
        return b;

    }
    public void AddShow(ItemData i)
    {
        List<ItemData> Temp;
        if (!Show.TryGetValue(i.id, out Temp))
        {
            Show[i.id] = new List<ItemData>();
        }
        Show[i.id].Add(i);
    }
    public void RemoveShow(ItemData i)
    {
        List<ItemData> Temp;
        if (Show.TryGetValue(i.id, out Temp))
        {
            Temp.Remove(i);

        }

    }
}