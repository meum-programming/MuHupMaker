using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace A_Script
{
    public enum SubPopupType {
        Using,Slots
    }
    public class InventorySubPopup : InitBase
    {
        public SubPopupType SPT;
        Button Use;
        Button Unuse;
        Button Equip;
        Button Get;
        Button Info;
        InventoryUI iu;
        InventoryType it;
        ItemData id;
      
        public void Init(InventoryType _it, ItemData _id, InventoryUI _iu) {
            localinit();
     
            iu = _iu;
            it = _it;
            id = _id;
            Unuse.gameObject.SetActive(false);

            if (SPT == SubPopupType.Slots)
            {
                Use.gameObject.SetActive(false);
                Equip.gameObject.SetActive(false);
                Get.gameObject.SetActive(false);
                Info.gameObject.SetActive(true);
            }
         
            switch (_it) {
                case InventoryType.Equipment:
                    if (!_id.E)
                    {
                        Equip.gameObject.SetActive(true);
                        Equip.onClick.RemoveAllListeners();

                        Equip.onClick.AddListener(() =>
                        {
                            id.select = false;
                            iu.Equip(_it, _id);
                            this.gameObject.SetActive(false);
                        }
                        );
                    }
                    else {
                        Unuse.gameObject.SetActive(true);
                        Unuse.onClick.RemoveAllListeners();

                        Unuse.onClick.AddListener(() =>
                        {
                            id.select = false;
                            iu.UnEquip(_it, _id);
                            this.gameObject.SetActive(false);
                        }
                        );
                    }
                    break;
                case InventoryType.Skill:
                    Get.gameObject.SetActive(true);
                    Get.onClick.RemoveAllListeners();
                    Get.onClick.AddListener(() => {
                        id.select = false;
                        iu.Get(_it, _id);
                        this.gameObject.SetActive(false);
                    }
                   );
                    break;
                case InventoryType.Consumption:
                    Use.gameObject.SetActive(true);
                    Use.onClick.RemoveAllListeners();
                    Use.onClick.AddListener(() => {
                        id.select = false;
                        iu.Use(_it, _id);

                        this.gameObject.SetActive(false);

                    }
                   );
                    break;
            }
            
        }
        void ShowDetail() {
            id.select = false;
            iu.Showdetail(it,id);
            this.gameObject.SetActive(false);
        }
        public void CloseEvent() {
            id.select = false;
            iu.UpdateShow();
            this.gameObject.SetActive(false);
        }
        public override void initactive()
        {
            if (SPT == SubPopupType.Slots) {

                Use = transform.Find("Use").GetComponent<Button>();
 
                Equip = transform.Find("Equip").GetComponent<Button>();
                Get = transform.Find("Get").GetComponent<Button>();
                Info = transform.Find("Info").GetComponent<Button>();
                Info.onClick.AddListener(ShowDetail);
               
            }
            Unuse = transform.Find("Unuse").GetComponent<Button>();
          
        }
    }
}
