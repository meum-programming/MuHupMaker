using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace A_Script
{
    public class InventoryItem : MonoBehaviour, LoopItem
    {

        ItemData id;

        public Button button;
        public Image image;
        public GameObject E;
        public Image line;

        InventoryType it;
        public void ScrollCellIndex(int idx)
        {
            line.gameObject.SetActive(false);
        //    Debug.Log(idx);
            //if (idx != index) {


#if UNITY_EDITOR
            this.name = idx.ToString();
#endif
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(showdetail);

            it = GameManager.i.IUI.selectit;
            var selectlist = GameManager.i.IUI.selectinven.Total;
            if (selectlist.Count > idx) {
                id = selectlist[idx];







                image.sprite = BasicPool.i.Get(id.iconName, id.inventype == InventoryType.Equipment ? RPath.GetItemIconPath(id.itemtype) : RPath.SkillIcon);
                image.enabled = true;
                if (id.E)
                {
                    E.SetActive(true);
                }
                else {
                    E.SetActive(false);
                }
                if (id.select) {
                    line.gameObject.SetActive(true);
                }
            }
            else {
                image.sprite = null;
                image.enabled = false;
                E.SetActive(false);

            }

            // }

        }

        void showdetail()
        {
            if (id != null)
            {
                line.gameObject.SetActive(true);
                id.select = true;
                GameManager.i.IUI.ShowSlotssubpopup(it, id, this.transform.position);
            }

            //    iu.ShowDetail(id, it);
        }

    }
    public interface LoopItem{
        void ScrollCellIndex(int idx);
    }
}
