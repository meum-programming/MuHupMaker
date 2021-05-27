using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace A_Script
{
    public abstract class UnitEquip : UnitNavAgent
    {
        public Transform WeaponTranL;
        public Transform WeaponTranR;
        public EquipData Weapon = new EquipData();
        public EquipData Helmet = new EquipData();
        public EquipData Armor = new EquipData();
        public EquipData Pants = new EquipData();
        public EquipData Boots = new EquipData();
        public EquipData Guard = new EquipData();

        public virtual void EquipInit()
        {
            Weapon.TranL = WeaponTranL;
            Weapon.TranR = WeaponTranR;
       
        }

        /// <summary>
        /// 넘겨 받은 타입에 해당되는 EquipData 리턴
        /// </summary>
        /// <param name="itemtype"></param>
        /// <returns></returns>
        public EquipData GetEquipData(string itemtype)
        {
            switch (itemtype)
            {
                case "W":
                    return Weapon;
                case "H":
                    return Helmet;
                case "A":
                    return Armor;
                case "P":
                    return Pants;
                case "B":
                    return Boots;
                case "G":
                    return Guard;
                default:
                    return null;
            }
        }

    }
    
    public class EquipData {
        public Transform TranL;
        public Transform TranR;
        public ItemData item;
        public bool isit = false;
        public Action<ItemData, UsingItem> ChangeCallback = null;
        public Action RemoveCallback = null;
        void RemovePrefab() {
            if (TranL == null || TranR == null)
                return;
           
            foreach (Transform t in TranL)
            {
                GameObject.Destroy(t.gameObject);
            }
          
                foreach (Transform t in TranR) {
                GameObject.Destroy(t.gameObject);
            }
        }
        void SetPrefab(int type,string key) {
         //   Debug.Log(TranL == null || TranR == null);
            if (TranL == null || TranR == null)
                return;

            GameObject temp;
            switch (type) {
                case 1:
                    temp = GameObject.Instantiate(Resources.Load<GameObject>(RPath.Kunckle + key));
                    temp.transform.SetParent(TranL);
                    temp.transform.localPosition = new Vector3(-0.046f, -0.011f, 0.057f);
                    temp.transform.localEulerAngles = new Vector3(0, -113.82f, -90);
                    temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    temp = GameObject.Instantiate(Resources.Load<GameObject>(RPath.Kunckle + key));
                    temp.transform.SetParent(TranR);
                    temp.transform.localPosition = new Vector3(-0.046f, -0.011f, 0.057f);
                    temp.transform.localEulerAngles = new Vector3(0, -113.82f, -90);
                    temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                case 2:

                    temp = GameObject.Instantiate(Resources.Load<GameObject>(RPath.Sword + key));
                    temp.transform.SetParent(TranR);
                    temp.transform.localPosition = new Vector3(-0.076f, -0.01f, 0.008f);
                    temp.transform.localEulerAngles = new Vector3(0, 180, 90);
                    temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                case 3:
                    temp = GameObject.Instantiate(Resources.Load<GameObject>(RPath.Katana + key));
                    temp.transform.SetParent(TranR);
                    temp.transform.localPosition = new Vector3(-0.065f, -0.006f, 0.015f);
                    temp.transform.localEulerAngles = new Vector3(0, 180, 90);
                    temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                case 4:
                    temp = GameObject.Instantiate(Resources.Load<GameObject>(RPath.Lance + key));
                    temp.transform.SetParent(TranR);
                    temp.transform.localPosition = new Vector3(-0.069f, -0.015f, -0.236f);
                    temp.transform.localEulerAngles = new Vector3(0, 180, 90);
                    temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                case 5:
                    temp = GameObject.Instantiate(Resources.Load<GameObject>(RPath.Mace + key));
                    temp.transform.SetParent(TranR);
                    temp.transform.localPosition = new Vector3(-0.092f, -0.015f, -0.035f);
                    temp.transform.localEulerAngles = new Vector3(0, 180, 90);
                    temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
            }

        }
        public virtual void Equip(ItemData i)
        {
            if (item != null && item != i)
            {
                bool sameType = (item.itemtype == i.itemtype && item.type == i.type);
                Unequip(sameType);
            }
                
            item = i;
            item.E = true;
               isit = true;
            Debug.Log(updatetext != null);
            if (isText)
                updatetext.text = item.name;
           
            if (isImage)
            {
                updateImage.enabled = true;
                
                updateImage.sprite = BasicPool.i.Get(i.iconName, RPath.GetItemIconPath(i.itemtype));
            }
            RemovePrefab();
            SetPrefab(i.type,i.prefab);
            ChangeCallback?.Invoke(item, usingitem);

            //장착 아이템 저장
            PlayerDataManager.ins.SaveEquipItemData(i);
        }
        public virtual void Unequip(bool sameType = false)
        {
            if (sameType == false)
                RemoveCallback?.Invoke();
            
            if (item == null)
                return;
            
            item.E = false;
            item = null;
            isit = false;
            if (isText)
                updatetext.text = "";
          
            if (isImage)
            {
                updateImage.enabled = false;
                updateImage.sprite = null;
            }
            RemovePrefab();
            ChangeCallback?.Invoke(null, usingitem);
        }
        public Text updatetext;
        public Image updateImage;
        public UsingItem usingitem = null;
        bool isText = false;
        bool isImage = false;
        public void SetText(Text _t)
        {
            //  Debug.Log("SetText");
            updatetext = _t;
            isText = true;
        }
      
        public void SetImage(Image _t)
        {
          //  Debug.Log("SetImage");
            updateImage = _t;
            updateImage.enabled = false;
            isImage = true;
        }
    }
}
