using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPool : MonoBehaviour
{
    public static BasicPool i;
    public Dictionary<string, Sprite> Pool = new Dictionary<string, Sprite>();
    private void Awake()
    {
        i = this;
    }
    public Sprite Get(string key, string path)
    {
        Sprite sp;
        if (Pool.TryGetValue(key, out sp))
        {
            return sp;
        }
        else
        {
            Debug.Log(path + key);
            sp = Resources.Load<Sprite>(path+key);
            Debug.Log(sp == null);
            if (sp != null)
                Pool[key] = sp;
            return sp;
        }
     
    
        
    }
    void Add(string key,Sprite sp,bool force = true) {

        if (force)
        {
            Pool[key] = sp;
        }
        else
        {
            Sprite temp;
            if (!Pool.TryGetValue(key, out temp))
            {
                Pool[key] = sp;
            }
        }
    }
    public void Remove(string key) {
        if (Pool.ContainsKey(key)) {
            Pool.Remove(key);
        }
    }
    public void RemoveAll() {
        Pool.Clear();
    }
}
