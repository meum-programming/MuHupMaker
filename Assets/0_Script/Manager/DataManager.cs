using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    public class DataManager
    {
        private static DataManager _i;
        public static DataManager i
        {
            get
            {
                if (_i == null)
                    _i = new DataManager();
                return _i;
            }
        }
        
        private DataManager()
        {
            Load();
        }

        public MonsterTable monsterTable = new MonsterTable();
        public CharacterTable characterTable = new CharacterTable();
        public WeaponTable WaeponTable = new WeaponTable();
        public ArmorTable armorTable = new ArmorTable();
        public TotalTable totalTable = new TotalTable();
        public SkillTable skillTable = new SkillTable();
        public SkillItemTable skillitemTable = new SkillItemTable();
        public SkillLvInfoTable  skilllvinfoTable = new SkillLvInfoTable();
        public CharLvTable charlvTable = new CharLvTable();
        public ItemDropGroupInfoTable itemdginfoTable = new ItemDropGroupInfoTable();
        public MapTable mapTable = new MapTable();
        public FieldBossTable fieldBossTable = new FieldBossTable();
        public ActiveSkillTable activeSkillTable = new ActiveSkillTable();
		

		public void Load()
        {
            characterTable.Load(CSVReader.Read("DB/CharInfo"));
            monsterTable.Load(CSVReader.Read("DB/MonsterInfo"));
            WaeponTable.Load(CSVReader.Read("DB/WeaponItemInfo"));
            armorTable.Load(CSVReader.Read("DB/ArmorItemInfo"));
     
            skillTable.Load(CSVReader.Read("DB/SkillInfo"));
            skillitemTable.Load(CSVReader.Read("DB/SkillItemInfo"));
            charlvTable.Load(CSVReader.Read("DB/CharLvInfo"));
            itemdginfoTable.Load(CSVReader.Read("DB/DropGroupItemInfo"));
            skilllvinfoTable.Load(CSVReader.Read("DB/SkillILvnfo"));
            mapTable.Load(CSVReader.Read("DB/MapInfo"));
            activeSkillTable.Load(CSVReader.Read("DB/ActiveSkillInfo"));
            fieldBossTable.Load(CSVReader.Read("DB/FieldBossInfo"));
            totalTable.AddRangeW(WaeponTable.weaponInfos);
            totalTable.AddRangeA(armorTable.armorInfo);
            //monsterTable.Load(CSVReader.Read("DB/CharLvInfo"));
        }
       
    }
    public class TotalTable {
        public Dictionary<int, ItemData> totalInfo = new Dictionary<int, ItemData>();
        public void AddRangeW(Dictionary<int, WeaponData> items)
        {
            foreach (var i in items)
            {
                totalInfo.Add(i.Key, i.Value);
            }
        }
        public void AddRangeA(Dictionary<int, ArmorData> items)
        {
            foreach (var i in items)
            {
                totalInfo.Add(i.Key, i.Value);
            }
        }
        public ItemData GetTotalInfo(int _totalID)
        {
            ItemData td = null;
            if (totalInfo.TryGetValue(_totalID, out td))
            {
                return td;
            }
            return null;
        }
    }
}
