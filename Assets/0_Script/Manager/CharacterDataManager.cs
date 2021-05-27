using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    public class CaracterDataManager
    {
        public static UnitSendData GetLvData(CharacterType type, int lv)
        {
            UnitSendData ud = new UnitSendData();
            if (type == CharacterType.None)
                return ud;
            ud.lv = lv;
            Debug.Log(type);
            var data = DataManager.i.charlvTable.charlvinfos[(int)type];
            lv = lv > data.Count ? data.Count : lv;
            for (int i = 0; i < lv; i++)
            {
                ud.hp += data[i].addhp;
                ud.mp += data[i].addmp;
                ud.ap += data[i].addap;
                ud.exp += data[i].needexp;


            }
            return ud;
        }
        public static UnitSendData GetLvSingleData(CharacterType type, int lv)
        {
            UnitSendData ud = new UnitSendData();
            if (type == CharacterType.None)
                return ud;
            ud.lv = lv;
            Debug.Log(type);
            var data = DataManager.i.charlvTable.charlvinfos[(int)type];
            lv = lv > data.Count ? data.Count : lv;
            int i = lv - 1;
            
                ud.hp += data[i].addhp;
                ud.mp += data[i].addmp;
                ud.ap += data[i].addap;
                ud.exp += data[i].needexp;


            
            return ud;
        }
        public static CharacterType GetCtype(string key)
        {

            switch (key)
            {
                case "무인":
                    return CharacterType.Mooin;

                case "도인":
                    return CharacterType.Doin;

                case "서생":
                    return CharacterType.Seosaeng;


            }
            return CharacterType.None;
        }
        public static string GetCName(int key)
        {

            switch (key)
            {
                case 1:
                    return "무인";
                
                case 2:
                    return "도인";
                  
                case 3:
                    return "서생";
               


            }
            return "";
        }
        public static CharacterType GetCtype(int key)
        {

            return (CharacterType)key;
        }

    }
    public class UnitSendData {
        public int lv;
        public int exp;
        public int hp;
        public int mp;
        public int ap;

    }
    public enum CharacterType {
        None, Mooin, Doin, Seosaeng
    }

}