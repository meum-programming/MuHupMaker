using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace A_Script
{
    public class UIRaycast : MonoBehaviour
    {
        public Canvas mycanvas; // raycast가 될 캔버스 
        GraphicRaycaster gr;
        PointerEventData ped;
        public List<Image> Ignore = new List<Image>();

        // Use this for initialization 
        void Start()
        {
            if (mycanvas == null)
                mycanvas = GetComponent<Canvas>();
            gr = mycanvas.GetComponent<GraphicRaycaster>();
            ped = new PointerEventData(null);
        }
        int id;
        private void Awake()
        {
            id = GetInstanceID();
        }
        private void OnEnable()
        {
            GameManager.i.MouseUIDic.Add(id, false);
        }
        private void OnDisable()
        {
            GameManager.i.MouseUIDic.Remove(id);
        }

        //void upda
        void Update()
        {
            foreach (var a in Ignore)
            {
                a.raycastTarget = false;
            }
            int count = 0;
#if UNITY_EDITOR

            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장 
            gr.Raycast(ped, results);
            count = results.Count;

#else
        for (int i = 0; i < Input.touchCount; i++) {
          
            ped.position = Input.GetTouch(i).position;
            List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장 
            gr.Raycast(ped, results);
            if (results.Count > 0) {
                count = 999;
                break;
            }
        }
#endif
            foreach (var a in Ignore)
            {
                a.raycastTarget = true;
            }

            /*
            if (Ignore.Count > 0) {
                var r = results.Select(x => x.gameObject).ToList();
                foreach (var a in Ignore) {
                    r.Remove(a);
                }
                count = r.Count;



            }
            */
#if UNITY_EDITOR
            if (GameManager.i != null)
#endif
               GameManager.i.MouseUIDic[id] = count != 0;
            //Debug.Log(IdolGameManager.Instance.IsMouseUI);
        }
    }
}
