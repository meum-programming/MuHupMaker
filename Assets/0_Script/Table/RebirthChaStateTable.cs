using System.Collections;
using System.Collections.Generic;
using A_Script;
using UnityEngine;

public class RebirthStatInfoTable : DataTable
{
    public List<RebirthStatInfo> list = new List<RebirthStatInfo>();

    public override void Load(List<Dictionary<string, object>> _datas)
    {
        var dataEnumerator = _datas.GetEnumerator();
        while (dataEnumerator.MoveNext())
        {
            var data = dataEnumerator.Current;

            RebirthStatInfo info = new RebirthStatInfo();
            info.charType = int.Parse(data["CharType"].ToString());
            info.rebirthID = int.Parse(data["RebirthID"].ToString());
            info.addHP = float.Parse(data["AddHP"].ToString());
            info.addAP = float.Parse(data["AddAP"].ToString());
            info.addMP = float.Parse(data["AddMP"].ToString());

            list.Add(info);
        }
    }

    public RebirthStatInfo GetData(int _charType , int _rebirthID)
    {
        RebirthStatInfo info = null;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].charType == _charType && list[i].rebirthID == _rebirthID)
            {
                info = list[i];
                break;
            }
        }

        return info;
    }
}

public class RebirthStatInfo
{
    public int charType;
    public int rebirthID;
    public float addHP;
    public float addAP;
    public float addMP;
}
