using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace A_Script
{
    public class InventoryUI : InventoryBase
    {
        // Start is called before the first frame update


        ToggleGroup tg;


        UsingItem usingWeapon;
        UsingItem usingHaed;
        UsingItem usingTop;
        UsingItem usingBottom;
        UsingItem usingShose;
        UsingItem usingBreacelet;
       public InventoryType selectit;
       public InventorySubPopup ISPUsing;
        public InventorySubPopup ISPSlots;
        Transform detail;
        Text detailname;
        Text detaildescription;
        Button detailcalcel;
        Button detailreinforce;
        Button detaildecomposition;
        Button detailsell;
        Button detailactive;
        Button detailunactive;
        public LoopScrollRect Scroll;
        

        private void Start()
        {
        }
        bool isinit = false;
        float pos = 1080f;
        public void Show() {
            this.gameObject.SetActive(true);
            transform.Find("Popup/Tap/Equipment").GetComponent<Toggle>().isOn = true;
            ShowTap(InventoryType.Equipment);
        }
        public void Hide() {
            this.gameObject.SetActive(false);
        }
    
 
       public override void Init()
        {
           
          
            if (!isinit)
            {
                
                /*
                   GameManager.i.CD.inventory.AddSkill(100101, DataManager.i.skillitemTable.skillitemInfos[100101]);
                   GameManager.i.CD.inventory.AddSkill(200101, DataManager.i.skillitemTable.skillitemInfos[200101]);
                   GameManager.i.CD.inventory.AddSkill(300101, DataManager.i.skillitemTable.skillitemInfos[300101]);
                   GameManager.i.CD.inventory.AddSkill(400101, DataManager.i.skillitemTable.skillitemInfos[400101]);
                   GameManager.i.CD.inventory.AddSkill(500101, DataManager.i.skillitemTable.skillitemInfos[500101]);
            */
                //   GameManager.i.CD.inventory.AddSkill(600101, DataManager.i.skillitemTable.skillitemInfos[600101]); ;





                detail = transform.Find("Popup/Slots/Detail");
                detailname = transform.Find("Popup/Slots/Detail/Name/Text").GetComponent<Text>();
                detaildescription = transform.Find("Popup/Slots/Detail/Description/Text").GetComponent<Text>();
                detailcalcel = transform.Find("Popup/Slots/Detail/Name/Close").GetComponent<Button>();
                detailreinforce = transform.Find("Popup/Slots/Detail/Button/Reinforce").GetComponent<Button>();
                detaildecomposition = transform.Find("Popup/Slots/Detail/Button/Decomposition").GetComponent<Button>();
                detailsell = transform.Find("Popup/Slots/Detail/Button/Sell").GetComponent<Button>();
                detailactive = transform.Find("Popup/Slots/Detail/Button/Active").GetComponent<Button>();
                detailunactive = transform.Find("Popup/Slots/Detail/Button/UnActive").GetComponent<Button>();
                detailcalcel.onClick.AddListener(()=> {
                    detail.gameObject.SetActive(false);
                });
                isinit = true;
             //   transform.Find("Popup/Tap/All").GetComponent<TapBase>().Init(this);
                transform.Find("Popup/Tap/Equipment").GetComponent<TapBase>().Init(this);
                transform.Find("Popup/Tap/Skill").GetComponent<TapBase>().Init(this);
                transform.Find("Popup/Tap/Consumption").GetComponent<TapBase>().Init(this);

                usingWeapon = transform.Find("Popup/Using/Weapon").GetComponent<UsingItem>();
                GameManager.i.player.Weapon.usingitem=(usingWeapon);
                GameManager.i.player.Weapon.ChangeCallback += ChangeEvent;
                usingHaed = transform.Find("Popup/Using/Haed").GetComponent<UsingItem>();
                GameManager.i.player.Helmet.usingitem=(usingHaed);
                GameManager.i.player.Helmet.ChangeCallback = ChangeEvent;
                usingTop = transform.Find("Popup/Using/Top").GetComponent<UsingItem>();
                GameManager.i.player.Armor.usingitem=(usingTop);
                GameManager.i.player.Armor.ChangeCallback = ChangeEvent;
                usingBottom = transform.Find("Popup/Using/Bottom").GetComponent<UsingItem>();
                GameManager.i.player.Pants.usingitem=(usingBottom);
                GameManager.i.player.Pants.ChangeCallback = ChangeEvent;
                usingShose = transform.Find("Popup/Using/Shose").GetComponent<UsingItem>();
                GameManager.i.player.Boots.usingitem=(usingShose);
                GameManager.i.player.Boots.ChangeCallback = ChangeEvent;
                usingBreacelet = transform.Find("Popup/Using/Bracelet").GetComponent<UsingItem>();
                GameManager.i.player.Guard.usingitem=(usingBreacelet);
                GameManager.i.player.Guard.ChangeCallback = ChangeEvent;
               // ISPSlots = transform.Find("Popup/Slots/Subpopup").GetComponent<InventorySubPopup>();
             //   ISPUsing = transform.Find("Popup/Using/Subpopup").GetComponent<InventorySubPopup>();
            }
            ShowTap(selectit);
        }
        void ChangeEvent(ItemData id,UsingItem ui) {
            ui.Init(id);
        }
       
     
        int getcount(InventoryType it)
        {
            switch (it)
            {
        
                case InventoryType.Consumption:
                    return GameManager.MaxInvenConsumCount;
                case InventoryType.Equipment:
                    return GameManager.MaxInvenEquipCount;
                case InventoryType.Skill:
                    return GameManager.MaxInvenSkillCount;
            }
            return 0;
        }
       
        public BasicInventory selectinven;
        public void ShowTap(InventoryType it) {
            selectit = it;
            selectinven = getinven(selectit);
            ReloadShow();
        }
        public void ReloadShow() {
            Scroll.totalCount = getcount(selectit);
            Scroll.RefillCells();
        }
        
        public void UpdateShow()
        {
            Scroll.totalCount = getcount(selectit);
            Scroll.RefreshCells();
        }
        
        public void ShowSlotssubpopup(InventoryType it, ItemData id, Vector3 v)
        {
            ISPSlots.transform.position = v;
            ISPSlots.gameObject.SetActive(true);
            ISPSlots.Init(it, id, this);
        }
        
        public void ShowUsingsubpopup(InventoryType it, ItemData id, Vector3 v)
        {
            ISPUsing.transform.position = v;
            ISPUsing.gameObject.SetActive(true);
            ISPUsing.Init(it, id, this);
        }
        
        public void Showdetail(InventoryType it,ItemData id) {
         
            detail.gameObject.SetActive(true);
            detailname.text = id.name;
            detaildescription.text = id.description;
            detailsell.onClick.RemoveAllListeners();
            detailsell.onClick.AddListener(()=> {
                Sell(it,id);
            });
            detailactive.onClick.RemoveAllListeners();
            detailunactive.onClick.RemoveAllListeners();
            switch (it) {
                case InventoryType.Equipment:
                    if (!id.E)
                    {
                        detailunactive.gameObject.SetActive(false);
                        detailactive.gameObject.SetActive(true);
                        detailactive.onClick.AddListener(() => Equip(it, id));
                    }
                    else {
                        detailactive.gameObject.SetActive(false);
                        detailunactive.gameObject.SetActive(true);
                        detailunactive.onClick.AddListener(() => UnEquip(it, id));
                    }
                    break;
                case InventoryType.Skill:
                    detailunactive.gameObject.SetActive(false);
                    detailactive.gameObject.SetActive(true);
                    detailactive.onClick.AddListener(() => Get(it, id));
                    break;
                case InventoryType.Consumption:
                    detailunactive.gameObject.SetActive(false);
                    detailactive.gameObject.SetActive(true);
                    detailactive.onClick.AddListener(() => Use(it, id));
                    break;
            }

        }
        public void Sell(InventoryType it, ItemData itemData)
        {
            if (it == InventoryType.Equipment)
            {
                EquipData equipData = GameManager.i.player.GetEquipData(itemData.itemtype);

                //장착중인 아이템을 판매한다면
                if (equipData.isit && equipData.item == itemData)
                    equipData.Unequip();
            }

            var select = getinven(it);
            if (select.Remove(itemData))
            {
                UpdateShow();
                detail.gameObject.SetActive(false);
            }
        }
        public void Get(InventoryType it, ItemData id)
        {
            Debug.Log("Get : " + it + " : " + id.id + " : " + id.name);
            if (it != InventoryType.Skill)
                return;
            var select = getinven(it);
           
                Debug.Log(id.itemtype);
                switch (id.itemtype)
                {
                    case "S":
                   
                        Debug.Log("select.Remove");
                        var skill = DataManager.i.skillTable.skillInfos[id.id];

                        switch (id.type)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            if (GameManager.i.CD.skillinventory.Nomal.Add(skill)) {
                                select.Remove(id);
                            }
                                break;

                            case 6:
                            if (GameManager.i.CD.skillinventory.Speed.Add(skill)){
                                select.Remove(id);
                            }

                            break;

                            case 7:
                            if (GameManager.i.CD.skillinventory.Special.Add(skill)){
                                select.Remove(id);
                            }
                            break;

                            case 8:
                            case 9:
                            if (GameManager.i.CD.skillinventory.Resection.Add(skill)){
                                select.Remove(id);
                            }
                            break;
                    }
                    UpdateShow();
                    break;
                }
                detail.gameObject.SetActive(false);
        }
        public void Use(InventoryType it, ItemData id)
        {
            var select = getinven(it);
            if (select.Remove(id))
            {
                UpdateShow();
                detail.gameObject.SetActive(false);
            }
        }
        public void Equip(InventoryType it, ItemData itemData)
        {
            if (it != InventoryType.Equipment)
                return;
            GameManager.i.player.GetEquipData(itemData.itemtype).Equip(itemData);
            detail.gameObject.SetActive(false);
            UpdateShow();
        }
        public void UnEquip(InventoryType it, ItemData itemData)
        {
            if (it != InventoryType.Equipment)
                return;

            GameManager.i.player.GetEquipData(itemData.itemtype).Unequip();
            detail.gameObject.SetActive(false);
            UpdateShow();
        }

    }

    public enum InventoryType
    {
        Equipment, Skill, Consumption
    }
}