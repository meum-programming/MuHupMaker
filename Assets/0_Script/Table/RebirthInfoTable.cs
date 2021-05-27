using System.Collections;
using System.Collections.Generic;
using A_Script;
using UnityEngine;

public class RebirthInfoTable : DataTable
{
    public List<RebirthInfo> list = new List<RebirthInfo>();

    public override void Load(List<Dictionary<string, object>> _datas)
    {
        var dataEnumerator = _datas.GetEnumerator();
        while (dataEnumerator.MoveNext())
        {
            var data = dataEnumerator.Current;

            RebirthInfo info = new RebirthInfo();
            info.rebirthID = int.Parse(data["RebirthID"].ToString());
            info.rebirthLv = int.Parse(data["RebirthLv"].ToString());
            info.rebirthName = data["RebirthName"].ToString();

            list.Add(info);
        }
    }

    public RebirthInfo GetData(int _rebirthID)
    {
        RebirthInfo info = null;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].rebirthID == _rebirthID)
            {
                info = list[i];
                break;
            }
        }

        return info;
    }
}

public class RebirthInfo
{
    public int rebirthID;
    public int rebirthLv;
    public string rebirthName;
}

