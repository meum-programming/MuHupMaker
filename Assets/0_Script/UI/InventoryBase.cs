using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace A_Script
{
    public abstract class InventoryBase : MonoBehaviour
    {
      
        public abstract void Init();
      
        protected  BasicInventory getinven(InventoryType it)
        {

            switch (it)
            {
                case InventoryType.Equipment:
                    return GameManager.i.CD.inventory.Equip;

                case InventoryType.Skill:
                    return GameManager.i.CD.inventory.Skill;

                case InventoryType.Consumption:
                    return GameManager.i.CD.inventory.Consum;

             


            }
            return null;
        }
       
        protected BasicSkillInventory getskill(SkillType st)
        {

            switch (st)
            {
                case SkillType.Nomal:
                    return GameManager.i.CD.skillinventory.Nomal;

                case SkillType.Speed:
                    return GameManager.i.CD.skillinventory.Speed;

                case SkillType.Special:
                    return GameManager.i.CD.skillinventory.Special;

                case SkillType.Resection:
                    return GameManager.i.CD.skillinventory.Resection;

            }
            return null;
        }
      
    }

}