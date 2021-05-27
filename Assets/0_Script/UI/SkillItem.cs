using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace A_Script {
    public class SkillItem : MonoBehaviour, LoopItem
    {






  
   


    

        SkillData sd;
        public Image line;
        public Text skillname;
        public Button button;
        public Image bg;
        public GameObject E;
      
        SkillType st;
        static SkillData Select = null;
        public void ScrollCellIndex(int idx)
        {
            line.gameObject.SetActive(false);
          //  Debug.Log(idx);
            //if (idx != index) {

        
#if UNITY_EDITOR
            this.name = idx.ToString();
#endif
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(showdetail);

            st = GameManager.i.SIU.selectst;
        

            var selectlist = GameManager.i.SIU.selectlist;
            if (selectlist.Count > idx)
            {
                sd = selectlist[idx];







                skillname.text = sd.skillname;


          
                E.SetActive(sd.E);
                if (sd.select)
                {
                    line.gameObject.SetActive(true);
                }
            }
            else
            {
                E.SetActive(false);

            }

            // }

        }











 
        
        void showdetail() {
            if (sd != null)
            {
                if (Select != null)
                    Select.select = false;
                Select = sd;
                line.gameObject.SetActive(true);
                sd.select = true;
                GameManager.i.SIU.ShowDetail(sd, st);
             
            }
          
        }

    }

}