using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace A_Script {
    public class SpawnActiver : MonoBehaviour
    {
        SpawnManager SM;
        public Vector3 Position;
        public List<int> Objs = new List<int>();
        
        public int Startcount = 1;
        public int Max;
        public float Spawntime = 5;
        public float Range = 5;
        public List<Unit> Units = new List<Unit>();
     
        public void Init(SpawnManager sm) {
            Position = this.transform.position;
            SM = sm;


            for (int i = 0; i < Startcount; i++)
            {
                var key = Objs[Random.Range(0, Objs.Count)];
                var obj = Instantiate<GameObject>(SM.GetObj(key));
                obj.transform.SetParent(this.transform);
                obj.transform.position = Position;
                obj.GetComponent<NavMeshAgent>().enabled = true;
                var u = obj.GetComponent<Unit>();

                UnitInit(u, key);

            }

            su =  StartCoroutine("SpawnUpdate");
        }
        Coroutine su = null;
        IEnumerator SpawnUpdate() {
          
          


            while (Units.Count < Max) {
                yield return new WaitForSeconds(Spawntime);
                var key = Objs[Random.Range(0, Objs.Count)];
                var obj = Instantiate<GameObject>(SM.GetObj(key));
                obj.transform.SetParent(this.transform);
                obj.transform.position = Position;
                obj.GetComponent<NavMeshAgent>().enabled = true;
                var u = obj.GetComponent<Unit>();
        
                UnitInit(u, key);

            }
            su = null;
        }
        void Remove(Unit u) {
            Units.Remove(u);
            if (Units.Count < Max && su == null) {
                su = StartCoroutine("SpawnUpdate");
            }
        }
      public  Vector3 RandomPos() {
            var r = Random.insideUnitCircle * Range;
            var pos = Position;
           // pos.y += 1;
            pos.x += r.x;
            pos.z += r.y;
            return pos;
        }
        void UnitInit(Unit u, int key) {
            UnitData ud;
            if (DataManager.i.monsterTable.enemyInfos.TryGetValue(key, out ud) ||
                DataManager.i.fieldBossTable.GetFieldBossData(key, out ud))
            {
                Units.Add(u);

                var rpos = RandomPos();
            //    Debug.Log(this.name + " : " + rpos);
                u.UnitInit(ud, rpos, (rpos- Position).normalized,Position);
                var e = (Enemy)u;
                if (e != null)
                    e.SetupSpawnActiver(this);
                    u.DieEvent += Remove;
            }
            else {
                Destroy(u.gameObject);
            }


        }
        
    }
}
