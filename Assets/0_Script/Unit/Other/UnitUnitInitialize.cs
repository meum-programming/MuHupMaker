using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace A_Script
{
    public class UnitInitialize : UnitInventory
    {
        public string Unitname;
       protected static int index;
        public int findlayer;
        public void UnitInit(UnitData UD, Vector3 V, Vector3 nomalrize,Vector3 back)
        {
            UnitInitDirect(UD, null);
            Warp(V, nomalrize, back);
        }


        public void UnitInitWarp(UnitData UD, Inventory IV, Transform T, Vector3 nomalrize,Vector3 back)
        {
            UnitInitWarp(UD, IV, T.position, nomalrize, back);

        }
        public void UnitInitWarp(UnitData UD, Inventory IV, Vector3 V, Vector3 nomalrize, Vector3 back)
        {

            UnitInitDirect(UD, IV);
            Warp(V, nomalrize, back);

        }
        public void UnitInit(UnitData UD, Transform T, Vector3 nomalrize,Vector3 back)
        {
            UnitInitWarp(UD, T.position, nomalrize, back);
        }
        public void UnitInitWarp(UnitData UD, Vector3 V, Vector3 nomalrize, Vector3 back)
    {

            UnitInitDirect(UD, (Inventory)null);
            Warp(V, nomalrize, back);
        }
        public virtual void UnitInitDirect(UnitData UD, Inventory IV)
        {
           

        }

    }
}
