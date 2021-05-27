using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EQUIPMENT_TYPE : int
{
    WEAPON,
    CLOTH,
    PANTS,
    SHOOS,
    HANDS
}

public class LocalUserInfo
{
    private Dictionary<EQUIPMENT_TYPE, int> equipmentsInfo;

    public LocalUserInfo()
    {
        equipmentsInfo = new Dictionary<EQUIPMENT_TYPE, int>();
    }


}
