using System.Collections;
using System.Collections.Generic;
using System.Linq;
using A_Script;
using UnityEngine;

public abstract class BasicSkillInventory
{
    public List<SkillData> Total = new List<SkillData>();
    public bool Contain(SkillData i)
    {
        var list = GetList(i);
        if (list == null)
            return true;
        return list.Any(x => x.id == i.id);
    }
    public List<SkillData> GetSort(int sort, SkillData Equip = null)
    {
        List<SkillData> Temp = Total;
        switch (sort)
        {
            case 0:
                Temp.Sort((SkillData a, SkillData b) =>
                {
                    if (a.grade > b.grade) return -1;
                    else if (a.grade < b.grade) return 1;
                    return 0;
                }
                    );
                break;
            case 1:
                Temp.Sort((SkillData a, SkillData b) =>
                {
                    if (a.skilllv > b.skilllv) return -1;
                    else if (a.skilllv < b.skilllv) return 1;
                    return 0;
                }
                  );
                break;
            case 2:
                Temp = Temp.Where((x) => x.type == 1).ToList(); ;

                Temp.Sort((SkillData a, SkillData b) =>
                {
                    if (a.grade > b.grade) return -1;
                    else if (a.grade < b.grade) return 1;
                    return 0;
                }
                   );

                break;
            case 3:
                Temp = Temp.Where((x) => x.type == 2).ToList(); ;
                Temp.Sort((SkillData a, SkillData b) =>
                {
                    if (a.grade > b.grade) return -1;
                    else if (a.grade < b.grade) return 1;
                    return 0;
                }
                   );
                break;
            case 4:
                Temp = Temp.Where((x) => x.type == 3).ToList(); ;
                Temp.Sort((SkillData a, SkillData b) =>
                {
                    if (a.grade > b.grade) return -1;
                    else if (a.grade < b.grade) return 1;
                    return 0;
                }
                   );
                break;
            case 5:
                Temp = Temp.Where((x) => x.type == 4).ToList(); ;
                Temp.Sort((SkillData a, SkillData b) =>
                {
                    if (a.grade > b.grade) return -1;
                    else if (a.grade < b.grade) return 1;
                    return 0;
                }
                   );
                break;
            case 6:
                Temp = Temp.Where((x) => x.type == 5).ToList(); ;
                Temp.Sort((SkillData a, SkillData b) =>
                {
                    if (a.grade > b.grade) return -1;
                    else if (a.grade < b.grade) return 1;
                    return 0;
                }
                   );
                break;
        }
        if (Equip != null)
        {

            if (Temp.Remove(Equip))
                Temp.Insert(0, Equip);
        }
        return Temp;
    }
    public Dictionary<int, List<SkillData>> Show = new Dictionary<int, List<SkillData>>();
    public abstract List<SkillData> GetList(SkillData i);
    public virtual bool Add(SkillData i)
    {
        if (!Contain(i))
        {
            AddReceive(i);
            PlayerDataManager.ins.SaveSkillData();
            return true;
        }
        return false;
    }
    public abstract SkillData AddReceive(SkillData i);
    public abstract void UpdateTotal();
    public virtual bool Remove(SkillData i)
    {
        var target = GetList(i);
        var b = target.Remove(i);

        RemoveShow(i);
        UpdateTotal();
        return b;

    }
    public void AddShow(SkillData i)
    {
        List<SkillData> Temp;
        if (!Show.TryGetValue(i.id, out Temp))
        {
            Show[i.id] = new List<SkillData>();
        }
        Show[i.id].Add(i);
    }
    public void RemoveShow(SkillData i)
    {
        List<SkillData> Temp;
        if (Show.TryGetValue(i.id, out Temp))
        {
            Temp.Remove(i);

        }

    }
}
