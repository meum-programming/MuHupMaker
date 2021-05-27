using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using A_Script;

public class PlayerDataManager
{
    public static PlayerDataManager ins
    {
        get
        {
            if (_ins == null)
                _ins = new PlayerDataManager();
            return _ins;
        }
    }
    private static PlayerDataManager _ins;

    public PlayerData maxData = new PlayerData();

    public A_Script.CharacterTable characterTable = new A_Script.CharacterTable();
    public RebirthStatInfoTable rebirthStatInfoTable = new RebirthStatInfoTable();
    public RebirthInfoTable rebirthInfoTable = new RebirthInfoTable();

    public UnityAction updateOn;

    A_Script.UnitData charInfo = null;

    public int selectindex;
    public string chaStateDataKey = "chaStateData";
    public CharacterSelectData p_data = new CharacterSelectData();

	public string chaItemDataKey = "chaItemData";
    public ItemSaveData itemSaveData = new ItemSaveData();

    public string chaSkillDataKey = "chaSkillData";
    public SkillSaveData skillSaveData = new SkillSaveData();
    
    private PlayerDataManager()
    {
        CSVSet();
    }

    void CSVSet()
    {
        characterTable.Load(CSVReader.Read("DB/CharInfo"));
        charInfo = characterTable.charInfos[100101];

        //환생 데이터 세팅
        rebirthStatInfoTable.Load(CSVReader.Read("DB/RebirthStatInfo"));
        rebirthInfoTable.Load(CSVReader.Read("DB/RebirthInfo"));
    }

    /// <summary>
    /// 초기화
    /// </summary>
    /// <param name="_selectindex"></param>
    /// <param name="_p_data"></param>
    public void Init(int _selectindex, CharacterSelectData _p_data)
    {
        selectindex = _selectindex;
        p_data = _p_data;
        DataSet();
        GetChaItemData();
        GetChaSkillData();
    }

    public void DataSet(bool resetOn = false)
    {
        maxData = GetLvData((PlayerCharacterType)p_data.Jap, (p_data.currentData.lv));

        RebirthStatInfo rebirthChaState = p_data.rebirthID == 0 ? new RebirthStatInfo():
                                                           rebirthStatInfoTable.GetData(p_data.Jap, p_data.rebirthID);

        maxData.hp += (charInfo.hp + rebirthChaState.addHP);
        maxData.mp += (charInfo.mp + rebirthChaState.addMP);
        maxData.ap += (charInfo.ap + rebirthChaState.addAP);

        if (resetOn || (p_data.currentData.hp == 0 &&
                        p_data.currentData.mp == 0 &&
                        p_data.currentData.ap == 0))
        {
            p_data.currentData.hp = maxData.hp;
            p_data.currentData.mp = maxData.mp;
            p_data.currentData.ap = maxData.ap;
        }

        if (updateOn != null)
            updateOn();

        string keyValue = string.Format("{0}{1}", chaStateDataKey, selectindex);
        PlayerPrefsUtils.SetObject(keyValue, p_data);
        PlayerPrefs.Save();
    }

    public PlayerData GetLvData(PlayerCharacterType type, int lv)
    {
        PlayerData ud = new PlayerData();
        if (type == PlayerCharacterType.None)
            return ud;
        ud.lv = lv;
        //Debug.Log(type);
        var data = A_Script.DataManager.i.charlvTable.charlvinfos[(int)type];

        lv = lv > data.Count ? data.Count : lv;
        for (int i = 0; i < lv; i++)
        {
            ud.hp += data[i].addhp;
            ud.mp += data[i].addmp;
            ud.ap += data[i].addap;
        }
        ud.exp = data[lv-1].needexp;

        return ud;
    }

    public void AddExp(int add_exp)
    {
        p_data.currentData.exp += add_exp;

        LevelUpCheck();

        if (updateOn != null)
            updateOn();
    }

    public void TemplvUp()
    {
        p_data.currentData.lv += 10;
        DataSet(true);
    }

    public void LevelUpCheck()
    {
        if (p_data.currentData.exp >= maxData.exp)
        {
            p_data.currentData.exp = 0;
            p_data.currentData.lv++;
            DataSet(true);
        }
    }

    public void SetRebirthWeapon(int rebirthID, int WeaponType)
    {
        int index = rebirthID - 1;
        if (p_data.rebirthWeaponList.Count <= index)
        {
            for (int i = p_data.rebirthWeaponList.Count; i < rebirthID; i++)
            {
                p_data.rebirthWeaponList.Add(0);
            }
        }

        p_data.rebirthWeaponList[index] = WeaponType;
    }

    public int GetRebirthWeapon(int rebirthID)
    {
        int index = rebirthID - 1;
        return (p_data.rebirthWeaponList.Count > index) ? p_data.rebirthWeaponList[index] : 0;
    }

    public void RebirthStart()
    {
        p_data.rebirthID++;
        p_data.currentData.lv = 1;
        DataSet(true);
    }

    public void AddValue(PlayerStatusEnum enumValue, float _value)
    {
        switch (enumValue)
        {
            case PlayerStatusEnum.HP:
                p_data.currentData.hp = Mathf.Max(p_data.currentData.hp - _value, 0);
                break;
            case PlayerStatusEnum.AP:
                p_data.currentData.ap = Mathf.Max(p_data.currentData.ap - _value, 0);
                break;
            case PlayerStatusEnum.MP:
                p_data.currentData.mp = Mathf.Max(p_data.currentData.mp - _value, 0);
                break;
        }

        DataSet();
    }

    public RebirthInfo GetRebirthInfo(int _rebirthID)
    {
        return rebirthInfoTable.GetData(_rebirthID);
    }

    /// <summary>
    /// 직업 텍스트 리턴
    /// </summary>
    /// <param name="playerCharacterType"></param>
    /// <returns></returns>
    public string GetCharacterTypeStr(PlayerCharacterType playerCharacterType)
    {
        switch (playerCharacterType)
        {
            case PlayerCharacterType.Mooin:
                return "무인";
            case PlayerCharacterType.Doin:
                return "도인";
            case PlayerCharacterType.Seosaeng:
                return "서생";
        }

        return string.Empty;
    }

    /// <summary>
    /// 캐릭터 저장 정보를 삭제
    /// </summary>
    /// <param name="selectindex"></param>
    public void DataDelete(int selectindex)
    {
        //캐릭터 정보를 삭제
        string chaStateDataKeyValue = string.Format("{0}{1}", chaStateDataKey, selectindex);
        PlayerPrefs.DeleteKey(chaStateDataKeyValue);
        p_data = null;

        //아이템 정보를 삭제
        string chaItemDataKeyValue = string.Format("{0}{1}", chaItemDataKey, selectindex);
        PlayerPrefs.DeleteKey(chaItemDataKeyValue);
        itemSaveData = null;

        //무공 정보를 삭제
        string chaSkillDataKeyValue = string.Format("{0}{1}", chaSkillDataKey, selectindex);
        PlayerPrefs.DeleteKey(chaSkillDataKeyValue);
        skillSaveData = null;
    }

    /// <summary>
    /// 아이템 정보 저장
    /// </summary>
    /// <param name="data"></param>
    public void SaveChaItemData(InventoryType inventoryType, List<ItemData> data)
	{
        if (itemSaveData == null)
            itemSaveData = new ItemSaveData();

        if (inventoryType == InventoryType.Equipment)
        {
            itemSaveData.itemIDList.Clear();
            foreach (var item in data)
            {
                itemSaveData.itemIDList.Add(item.id);
            }
        }
        else if (inventoryType == InventoryType.Skill)
        {
            itemSaveData.skillItemIDList.Clear();
            foreach (var item in data)
            {
                itemSaveData.skillItemIDList.Add(item.id);
            }
        }

        string keyValue = string.Format("{0}{1}", chaItemDataKey, selectindex);
        PlayerPrefsUtils.SetObject(keyValue, itemSaveData);
    }

    /// <summary>
    /// 장착 아이템 정보 저장
    /// </summary>
    /// <param name="itemData"></param>
    public void SaveEquipItemData(ItemData itemData)
    {
        if (itemSaveData == null)
        {
            SaveChaItemData(InventoryType.Equipment,GameManager.i.player.Inventory.Equip.Total);
            SaveChaItemData(InventoryType.Skill, GameManager.i.player.Inventory.Skill.Total);
        }

        switch (itemData.itemtype)
        {
            case "W":
                itemSaveData.weaponId = itemData.id;
                break;
            case "A":
                itemSaveData.armorId = itemData.id;
                break;
            case "B":
                itemSaveData.bootsId = itemData.id;
                break;
            case "G":
                itemSaveData.guardId = itemData.id;
                break;
            case "H":
                itemSaveData.helmetId = itemData.id;
                break;
            case "P":
                itemSaveData.pantsId = itemData.id;
                break;
        }

        string keyValue = string.Format("{0}{1}", chaItemDataKey, selectindex);
        PlayerPrefsUtils.SetObject(keyValue, itemSaveData);
    }

    /// <summary>
    /// 저장된 아이템 정보 가져오기
    /// </summary>
    public void GetChaItemData()
    {
        string keyValue = string.Format("{0}{1}", chaItemDataKey, selectindex);
        itemSaveData = PlayerPrefsUtils.GetObject<ItemSaveData>(keyValue);
    }

    /// <summary>
    /// 무공 정보 저장
    /// </summary>
    public void SaveSkillData()
    {
        if (skillSaveData == null)
            skillSaveData = new SkillSaveData();

        ActiveSkillInventory skillInven = GameManager.i.CD.skillinventory;
        Player player = GameManager.i.player;

        SetSkillSaveData(skillInven.Nomal.Total,   out skillSaveData.nomalSkillIdList   , player.Nomal,   out skillSaveData.subSkillNomalId);
        SetSkillSaveData(skillInven.Speed.Total,   out skillSaveData.speedSkillIdList   , player.Speed,   out skillSaveData.subSkillSpeedId);
        SetSkillSaveData(skillInven.Special.Total, out skillSaveData.specialSkillIdList , player.Special, out skillSaveData.subSkillSpecialId);

        string keyValue = string.Format("{0}{1}", chaSkillDataKey, selectindex);
        PlayerPrefsUtils.SetObject(keyValue, skillSaveData);
    }

    /// <summary>
    /// 무공 저장 데이터 세팅
    /// </summary>
    /// <param name="skillDataList"></param>
    /// <param name="saveData"></param>
    /// <param name="subSkillData"></param>
    /// <param name="skillId"></param>
    private void SetSkillSaveData(List<SkillData> skillDataList ,out List<int> saveData ,
                                  SubSkillData subSkillData , out int skillId)
    {
        saveData = new List<int>();
        foreach (var skillData in skillDataList)
        {
            saveData.Add(skillData.id);
        }

        skillId = 0;
        if (subSkillData.IsIt)
        {
            skillId = subSkillData.Skill.id;
        }
    }

    /// <summary>
    /// 저장된 무공 정보 가져오기
    /// </summary>
    public void GetChaSkillData()
    {
        string keyValue = string.Format("{0}{1}", chaSkillDataKey, selectindex);
        skillSaveData = PlayerPrefsUtils.GetObject<SkillSaveData>(keyValue);
    }
}

[System.Serializable]
public class PlayerData
{
    public int lv = 1;
    public int exp;
    public float hp;
    public float ap;
    public float mp;

    public PlayerData() { }
    public PlayerData(float _hp, float _ap, float _mp)
    {
        this.hp = _hp;
        this.ap = _ap;
        this.mp = _mp;
    }
}

/// <summary>
/// 아이템 저장 정보
/// </summary>
public class ItemSaveData
{
    public List<int> itemIDList = new List<int>();
    public List<int> skillItemIDList = new List<int>();
    public int weaponId = 0;
    public int helmetId = 0;
    public int armorId = 0;
    public int pantsId = 0;
    public int bootsId = 0;
    public int guardId = 0;
}

/// <summary>
/// 스킬 저장 정보
/// </summary>
public class SkillSaveData
{
    public List<int> nomalSkillIdList = new List<int>();
    public List<int> speedSkillIdList = new List<int>();
    public List<int> specialSkillIdList = new List<int>();

    public int subSkillNomalId = 0;
    public int subSkillSpeedId = 0;
    public int subSkillSpecialId = 0;
}


public enum PlayerCharacterType
{
    None, Mooin, Doin, Seosaeng
}

public enum PlayerStatusEnum
{
    LV = 0,
    EXP,
    HP,
    AP,
    MP
}


