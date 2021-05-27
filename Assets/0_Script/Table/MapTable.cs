using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    public class MapTable : DataTable
    {
        public List<MapData> mapDataList = new List<MapData>();

        public override void Load(List<Dictionary<string, object>> _datas)
        {
            var dataEnumerator = _datas.GetEnumerator();

            mapDataList = new List<MapData>();
            while (dataEnumerator.MoveNext())
            {
                var data = dataEnumerator.Current;

                MapData info = new MapData();

                info.id = int.Parse(data["MapID"].ToString());
                info.mapName = data["MapName"].ToString();
                info.sceneName = data["SceneName"].ToString();

                mapDataList.Add(info);
            }
        }
    }
}

[System.Serializable]
public class MapData
{
    public int id;
    public string mapName;
    public string sceneName;
}
