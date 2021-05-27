using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace A_Script
{
    public class SkillUI : InventoryBase
    {
        Transform inventory;
        Transform detail;
        Button cancel;
        Button equip;
        Image usingNomal;
        Image usingSpeed;
        Image usingSpecial;

       public SkillType selectst;

        public Image ShowNomalImage;
        public Image ShowSpeedImage;
        public Image ShowSpecialImage;
        public Text ShowNomalText;
        public Text ShowSpeedText;
        public Text ShowSpecialText;
        bool isinit = false;
        public LoopScrollRect Scroll;

        public RebirthTapController rebirthTapController;

        public void Show()
        {
            gameObject.SetActive(true);
            ShowTap(selectst);
        }
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
        public override void Init() {
            if (!isinit)
            {
                /*
                GameManager.i.CD.skillinventory.Nomal[100101] = DataManager.i.skillTable.skillInfos[100101];
                GameManager.i.CD.skillinventory.Nomal[200101] = DataManager.i.skillTable.skillInfos[200101];
                GameManager.i.CD.skillinventory.Nomal[300101] = DataManager.i.skillTable.skillInfos[300101];
                GameManager.i.CD.skillinventory.Nomal[400101] = DataManager.i.skillTable.skillInfos[400101];
                GameManager.i.CD.skillinventory.Nomal[500101] = DataManager.i.skillTable.skillInfos[500101];
                GameManager.i.CD.skillinventory.Nomal[600101] = DataManager.i.skillTable.skillInfos[600101];
                */
                //Debug.Log(GameManager.i.CD.skillinventory.Nomal[100101].skillname);
                inventory = transform.Find("Popup/Info/Inventory/View");
                detail = transform.Find("Popup/Info/Detail");
                cancel = detail.Find("Cancel").GetComponent<Button>();
                equip = detail.Find("Equip").GetComponent<Button>();
                isinit = true;
                transform.Find("Popup/Tap/Nomal").GetComponent<TapBase>().Init(this);
                transform.Find("Popup/Tap/Speed").GetComponent<TapBase>().Init(this);
                transform.Find("Popup/Tap/Special").GetComponent<TapBase>().Init(this);
                transform.Find("Popup/Tap/Resection").GetComponent<TapBase>().Init(this);
                usingNomal = transform.Find("Popup/Using/Nomal/Image").GetComponent<Image>();
                GameManager.i.player.Nomal.SetImage(usingNomal);
                GameManager.i.player.Nomal.SetImage(ShowNomalImage);
                GameManager.i.player.Nomal.SetText(ShowNomalText);
                usingSpeed = transform.Find("Popup/Using/Speed/Image").GetComponent<Image>();
                GameManager.i.player.Speed.SetImage(usingSpeed);
                GameManager.i.player.Speed.SetImage(ShowSpeedImage);
                GameManager.i.player.Speed.SetText(ShowSpeedText);
                usingSpecial = transform.Find("Popup/Using/Special/Image").GetComponent<Image>();
                GameManager.i.player.Special.SetImage(usingSpecial);
                GameManager.i.player.Special.SetImage(ShowSpecialImage);
                GameManager.i.player.Special.SetText(ShowSpecialText);
                Dd.onValueChanged.AddListener(DdChange);
            }
            ShowTap(SkillType.Nomal);
        }


        /*
        bool isuse(SkillData s) {
            var player = GameManager.i.player;
            switch (selectst)
            {
                case SkillType.Nomal:
                    return !player.Nomal.IsIt?true: player.Nomal.Skill.SkillID == s.SkillID;

                case SkillType.Speed:
                    return !player.Speed.IsIt ? true : player.Nomal.Skill.SkillID == s.SkillID;

                case SkillType.Special:
                    return !player.Special.IsIt ? true : player.Nomal.Skill.SkillID == s.SkillID;

                case SkillType.Resection:
                    return !player.Resection.IsIt ? true : player.Nomal.Skill.SkillID == s.SkillID;

            }
            return false;
        }
       */
       /*
        int getcount(SkillType it)
        {
            switch (it)
            {

                case SkillType.Nomal:
                    return GameManager.i.CD.skillinventory.Nomal.Total.Count;
                case SkillType.Speed:
                    return GameManager.i.CD.skillinventory.Speed.Total.Count;
                case SkillType.Special:
                    return GameManager.i.CD.skillinventory.Special.Total.Count;
                case SkillType.Resection:
                    return GameManager.i.CD.skillinventory.Resection.Total.Count;
            }
            return 0;
        }
        */
        public BasicSkillInventory selectinven;
        public List<SkillData> selectlist;
        public void ShowTap(SkillType st) {
            selectst = st;

            rebirthTapController.ContentObjSetActive(selectst == SkillType.Resection);

            if (selectst == SkillType.Resection)
                return;
            
            selectinven = getskill(selectst);

            if (st != SkillType.Nomal)
            {
                selectlist = selectinven.Total;
            }
            else {
                SkillData sd = null;
                if (GameManager.i.player.Nomal.IsIt)
                    sd = GameManager.i.player.Nomal.Skill;
                selectlist = selectinven.GetSort(Dd.value, sd);
            }

            Scroll.totalCount = selectlist.Count;
            Scroll.RefillCells();
        }
        public Dropdown Dd;
        public void DdChange(int i) {
            SkillData sd = null;
            if (GameManager.i.player.Nomal.IsIt)
                sd = GameManager.i.player.Nomal.Skill;
            selectlist = selectinven.GetSort(i, sd);
            UpdateShow();
        }
        public void UpdateShow()
        {
            Scroll.totalCount = selectlist.Count;
            Scroll.RefillCells();
        }
        SkillData last = null;
        public void ShowDetail(SkillData sd, SkillType st) {
            if (last != null) {
                last.RemoveText();
                last.RemoveLvUpText();
            }
            detail.Find("Name").GetComponent<Text>().text = sd.skillname;
          
            if (sd.type != 6)
            {
                detail.Find("Exp").GetComponent<Text>().text =
                "lv:" + sd.skilllv + " " + sd.currentexp.ToString() + "/" + sd.exp.ToString() + "(" + ((int)(((float)sd.currentexp / (float)sd.exp) * 100)) + "%)";
                detail.Find("Description").GetComponent<Text>().text = string.Format("내공 : {0}%\n외공: {1}%", sd.GetLvFactorPowMP, sd.GetLvFactorPowAP);
                sd.SetText(detail.Find("Exp").GetComponent<Text>());
                sd.SetLvUpText(detail.Find("Description").GetComponent<Text>());
            }
            else {
                detail.Find("Exp").GetComponent<Text>().text = "";
                detail.Find("Description").GetComponent<Text>().text =
             "이동속도 : " + (sd.skillmove).ToString();
            }
              
            var select = getskill(st);

            equip.onClick.RemoveAllListeners();
            equip.onClick.AddListener(() => {
                GameManager.i.player.GetSkill(st).Set(sd);
                UpdateShow();

            });
            cancel.onClick.RemoveAllListeners();
            cancel.onClick.AddListener(() => {
                detail.Find("Name").GetComponent<Text>().text = "";
                detail.Find("Description").GetComponent<Text>().text = "";
                detail.Find("Exp").GetComponent<Text>().text = "";
             GameManager.i.player.GetSkill(st).Remove();
                UpdateShow();
            });


            UpdateShow();
        }
    }
    public enum SkillType {
        Nomal, Speed, Special, Resection
    }
}
