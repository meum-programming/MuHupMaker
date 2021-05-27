using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace A_Script
{
    public class UsingItem : MonoBehaviour
    {
        int index = -1;
        ItemData id;

        public Button button;
        public Image image;
 

    
        public void Init(ItemData _id)
        {
     

            id = _id;
#if UNITY_EDITOR
            this.name = index.ToString();
#endif
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(showdetail);

         
       
            if (id != null)
            {






                image.sprite = BasicPool.i.Get(id.iconName, id.inventype == InventoryType.Equipment ? RPath.GetItemIconPath(_id.itemtype) : RPath.SkillIcon);
                image.enabled = true;
             
            }
            else
            {
                image.sprite = null;
                image.enabled = false;
             
            }

            // }

        }

        void showdetail()
        {
            if (id != null)
            {
                GameManager.i.IUI.ShowUsingsubpopup(InventoryType.Equipment, id, this.transform.position);
            }

            //    iu.ShowDetail(id, it);
        }

    }
}