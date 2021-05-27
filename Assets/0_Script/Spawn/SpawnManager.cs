using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    public class SpawnManager : MonoBehaviour
    {
        Dictionary<int, GameObject> Objpool = new Dictionary<int, GameObject>();
        public GameObject GetObj(int key) {
            GameObject g;
            if (Objpool.TryGetValue(key, out g))
            {
                return g;
            }
            else {
                var temp = Resources.Load("Enemy/Model/"+key) as GameObject;
                if (temp != null)
                    Objpool.Add(key,temp);

                return temp;
            }

        }
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.i.ActiveStageCamera != null);
;            Init();
        }
        SpawnActiver[] spawnlist;
        void Init() {
            spawnlist = GetComponentsInChildren<SpawnActiver>();
            foreach (var s in spawnlist) {
                s.Init(this);
            }
        }
  
    }

}