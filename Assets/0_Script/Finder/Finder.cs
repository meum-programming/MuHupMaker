using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    public class Finder : MonoBehaviour
    {
        Unit u;
        int layerindex;
        public void Init(Unit _u, int _layerindex)
        {
            u = _u;
            layerindex = _layerindex;
        }
        private void OnTriggerEnter(Collider other)
        {
            var obj = other.gameObject;
            if (obj.layer == layerindex)
            {
                Unit temp;
                if (obj.TryGetComponent<Unit>(out temp))
                {
                    if(temp.Us != UnitState.Die)
                    u.Enter(temp);
                }

            }

        }
        private void OnTriggerExit(Collider other)
        {
            var obj = other.gameObject;
            if (obj.layer == layerindex)
            {
                Unit temp;
                if (obj.TryGetComponent<Unit>(out temp))
                {
                    u.Exit(temp);
                }

            }
        }
    }
}
