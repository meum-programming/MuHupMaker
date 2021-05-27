using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalSkillInventory : BasicSkillInventory
{
    public List<SkillData> Kunckle = new List<SkillData>();
    public List<SkillData> Sword = new List<SkillData>();
    public List<SkillData> Katana = new List<SkillData>();
    public List<SkillData> Lance = new List<SkillData>();
    public List<SkillData> Mace = new List<SkillData>();
    public List<SkillData> Move = new List<SkillData>();


    public override List<SkillData> GetList(SkillData i)
    {

        List<SkillData> target = new List<SkillData>();

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
    public override SkillData AddReceive(SkillData i)
    {
        /*
                    if (GameManager.MaxInvenSkillCount <= Total.Count)
                    {
                        Debug.Log("SkillInvenFull");
                        return null;
                    }
                    */
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
        Total.AddRange(Kunckle);
        Total.AddRange(Sword);
        Total.AddRange(Katana);
        Total.AddRange(Lance);
        Total.AddRange(Mace);
        Total.AddRange(Move);

    }
}
