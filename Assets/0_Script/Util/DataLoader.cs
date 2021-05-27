using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A_Script
{

    public abstract class DataTable
    {
        public abstract void Load(List<Dictionary<string, object>> _datas);
    }
#if UNITY_EDITOR
    [System.Serializable] public class DicUD : SerializableDictionary<int, UnitData> { }
    [System.Serializable] public class DicWD : SerializableDictionary<int, WeaponData> { }
    [System.Serializable] public class DicBD : SerializableDictionary<int, BoostsData> { }
    [System.Serializable] public class DicAD : SerializableDictionary<int, ArmorData> { }
    [System.Serializable] public class DicSD : SerializableDictionary<int, SkillData> { }
    [System.Serializable] public class DicSID : SerializableDictionary<int, SkillItemData> { }
    [System.Serializable] public class DicCLD : SerializableDictionary<int, List<CharLvInfoData>> { }

    [System.Serializable] public class DicIDGI : SerializableDictionary<int, List<ItemDropGroupInfoData>> { }
    [System.Serializable] public class DicSLD : SerializableDictionary<int, List<SkillLvInfoData>> { }
#endif


}

