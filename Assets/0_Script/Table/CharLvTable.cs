using System;
using System.Collections;
using System.Collections.Generic;
using A_Script;
using UnityEngine;

[System.Serializable]
public class CharLvTable : DataTable
{
    public Dictionary<int, List<CharLvInfoData>> charlvinfos = new Dictionary<int, List<CharLvInfoData>>();
#if UNITY_EDITOR
    public List<CharLvInfoData> charlvinfosView = new List<CharLvInfoData>();
#endif

    public override void Load(List<Dictionary<string, object>> _datas)
    {
        var dataEnumerator = _datas.GetEnumerator();
        int index = -1;
        List<CharLvInfoData> datas = new List<CharLvInfoData>();
        while (dataEnumerator.MoveNext())
        {
            var data = dataEnumerator.Current;

            CharLvInfoData info = new CharLvInfoData();
             
            info.chartype = int.Parse(data["CharType"].ToString());
            if (index == -1) {
                index = info.chartype;
            }
            info.charlv = int.Parse(data["CharLv"].ToString());
            info.needexp = int.Parse(data["NextNeedExp"].ToString());
            info.addhp = int.Parse(data["AddHP"].ToString());
            info.addmp = int.Parse(data["AddMP"].ToString());
            info.addap = int.Parse(data["AddAP"].ToString());
#if UNITY_EDITOR
            charlvinfosView.Add(info);
#endif
            datas.Add(info);
            if (index != info.chartype)
            {
                Debug.Log(info.chartype);
                charlvinfos.Add(index, datas);
                datas = new List<CharLvInfoData>();
                index = info.chartype;
            }
        }
        charlvinfos.Add(index, datas);
    }
}

[Serializable]
public class CharLvInfoData
{
    public int index;
    public int chartype;
    public int charlv;
    public int needexp;
    public int addhp;
    public int addmp;
    public int addap;
}