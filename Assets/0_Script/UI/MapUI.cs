using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace A_Script {
    public class MapUI : MonoBehaviour
    {
        public int index = 0;
        public Transform btnPanel;
        public Transform Icons;
        public GameObject EnterPopup;
        public Text EnterText;

        public Button ScrollBtnorigin;
        public ScrollRect mapBtnScrollPanel;

        int Select = 0;

        // Start is called before the first frame update
        void Start()
        {
            ScrollSet();

            /*
            foreach (Transform icon in Icons)
            {
                icon.Find("round").gameObject.SetActive(false);
            }
            Icons.Find(maps[index]).Find("round").gameObject.SetActive(true);
            */
        }

        void ScrollSet()
        {
            List<MapData> mapDataList = DataManager.i.mapTable.mapDataList;

            for (int i = 0; i < mapDataList.Count; i++)
            {
                Button scrollBtn = Instantiate(ScrollBtnorigin, mapBtnScrollPanel.content.transform);
                scrollBtn.onClick.AddListener(ToggleEvent(i));

                string mapName = mapDataList[i].mapName;
                scrollBtn.transform.GetComponentInChildren<Text>().text = mapName;
                scrollBtn.gameObject.SetActive(true);
            }
        }

        UnityEngine.Events.UnityAction ToggleEvent(int i) {

            return () =>
            {
                if (Select != i)
                {
                    index = i;

                    string mapName = DataManager.i.mapTable.mapDataList[index].mapName;

                    EnterText.text = string.Format("{0}(으)로 이동하시겠습니까?", mapName);
                    EnterPopup.SetActive(true);
                }
            };
        }

        public void Enter() {

            if (FindObjectOfType<Player>().Us == UnitState.Die)
                return;
            
            Select = index;

            /*
            foreach (Transform icon in Icons)
            {
                icon.Find("round").gameObject.SetActive(false);
            }
            Icons.Find(maps[index]).Find("round").gameObject.SetActive(true);
            */

            GameManager.i.Waypoints.Clear();
            string sceneName = DataManager.i.mapTable.mapDataList[index].sceneName;
            GameManager.i.LoadScene(sceneName);
            EnterPopup.SetActive(false);
            this.gameObject.SetActive(false);
        }

    }
}