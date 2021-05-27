using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkillInventory : BasicSkillInventory
{
    public List<SkillData> Special = new List<SkillData>();


    public override List<SkillData> GetList(SkillData i)
    {

        List<SkillData> target = new List<SkillData>();

        switch (i.type)
        {


            default:
                return Special;
        }
    }
    public override SkillData AddReceive(SkillData i)
    {
        var target = GetList(i);
        var item = i.Copy();

        target.Add(item);
        target.Sort((SkillData i1, SkillData i2) =>
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
        Total.AddRange(Special);


    }
}
