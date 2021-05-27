using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager i;
    public List<DefaultInitData> Default = new List<DefaultInitData>();
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        i = this;
        for (int i = 0; i < Default.Count; i++) {
            InitObj(Default[i].key, Default[i].G);
        }
    }
    Dictionary<string, PoolData> SaveObj = new Dictionary<string, PoolData>();

    Dictionary<string, List<PoolData>> Empty = new Dictionary<string, List<PoolData>>();

    public void InitObj(string key,GameObject g) {
        PoolData temp;
        if (!SaveObj.TryGetValue(key, out temp))
        {
            temp = new PoolData();
            g.SetActive(false);
            temp.G = g;
            temp.key = key;
            SaveObj[key] = temp;
            g.transform.SetParent(this.transform);
            Empty[key] = new List<PoolData>();
        }
    }

    public bool GetObj(string key,ref PoolData obj)
    {
        List<PoolData> target;
        if (Empty.TryGetValue(key, out target)) {
          
                bool isfind = false;
                foreach (var t in target) {
                    if (!t.Use) {
                        obj = t;
                        obj.Use = true;
                        obj.G.SetActive(true);
                        isfind = true;

                    target.Remove(obj);
                    break;

                }
                }
           

            if (!isfind)
            {
                PoolData p;
                if (SaveObj.TryGetValue(key, out p))
                {
                    obj = p.Copy();
                    obj.Use = true;
                    obj.G.SetActive(true);

                }
                else
                {
                    return false;
                }

            }

            return true;
        }
        return false;

    
    }
    public void ReturnObj(PoolData obj) {
        obj.G.transform.SetParent(this.transform);
        obj.G.SetActive(false);
        obj.Use = false;
        Empty[obj.key].Add(obj);

    }
}
public class PoolData {
    public string key;
    public GameObject G;
    public bool Use = false;
        public PoolData Copy() {
            PoolData temp = new PoolData();
            temp.key = key;
            temp.G = GameObject.Instantiate(G);
            temp.G.transform.SetParent(G.transform.parent);
            temp.Use = Use;
            return temp;
        }
    }
[System.Serializable]
public class DefaultInitData {
    public string key;
    public GameObject G;
}

