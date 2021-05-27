using System;
using System.Collections.Generic;
using A_Script;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SkillData : ItemData
{

    public string skillname;
    public float skillFactorpowmp;
    public float skillFactorpowap;

    public int powmpdef;
    public int powapdef;
    public int skillmove;
    public int skilllvid;
    public int conditionid_01;
    public int conditionid_02;
    public int conditionid_03;
    public int skilllv;
    public int use_MP;
    public int use_AP;
    public Text updatetext;
    public Text updatelvuptext;
    public bool isText = false;
    public bool isLvUpText = false;
    public void SetText(Text _updatetext)
    {
        updatetext = _updatetext;
        isText = true;
    }
    public void SetLvUpText(Text _updatetext)
    {

        updatelvuptext = _updatetext;
        isLvUpText = true;
    }
    public void RemoveText()
    {
        updatetext = null;
        isText = false;
    }
    public void RemoveLvUpText()
    {
        updatelvuptext = null;
        isLvUpText = false;
    }
    int _currentexp;
    public int currentexp
    {
        get
        {
            return _currentexp;
        }
        set
        {
            _currentexp = value;
            if (exp == -1)
                return;
            if (_currentexp >= exp)
            {
                _currentexp -= exp;
                skilllvup();
            }
            if (isText)
            {
                updatetext.text = "lv:" + skilllv + " " + _currentexp.ToString() + "/" + exp.ToString() + "(" + ((int)(((float)_currentexp / (float)exp) * 100)) + "%)";
            }
        }
    }
    public int exp = 1;

    public SkillLvInfoData slid = null;

    /// <summary>
    /// 레벨값이 적용된 내공 배율값
    /// </summary>
    public float GetLvFactorPowMP
    {
        get
        {
            float value = skillFactorpowmp * 0.0001f;
            if (slid == null)
                return value;

            return value * slid.getpowmp;
        }
    }

    /// <summary>
    /// 레벨값이 적용된 외공 배율값
    /// </summary>
    public float GetLvFactorPowAP
    {
        get
        {
            float value = skillFactorpowap * 0.0001f;
            if (slid == null)
                return value;

            return value * slid.getpowap;

        }
    }

    public void skillinit(int i)
    {


        List<SkillLvInfoData> temp;
        if (DataManager.i.skilllvinfoTable.skilllvInfos.TryGetValue(skilllvid, out temp))
        {
            slid = temp[i - 1];
            exp = slid.needexp;
        }
        else
        {
            exp = -1;
        }

        Debug.Log(exp + " : " + skilllvid + " : " + i);


    }
    public void skilllvup()
    {
        Debug.Log(skilllv + " : " + DataManager.i.skilllvinfoTable.skilllvInfos[skilllvid].Count);
        if (skilllv < DataManager.i.skilllvinfoTable.skilllvInfos[skilllvid].Count + 1)
        {
            skilllv++;
            skillinit(skilllv);

            if (isLvUpText)
            {
                updatelvuptext.text = string.Format("내공 : {0}%\n외공: {1}%", GetLvFactorPowMP, GetLvFactorPowAP);
            }
        }
    }

    new public SkillData Copy()
    {
        SkillData id = new SkillData();
        id.id = this.id;
        id.name = name;
        id.itemtype = itemtype;
        id.type = type;
        id.grade = grade;
        id.prefab = prefab;
        id.powMP = powMP;
        id.powAP = powAP;
        id.speed = speed;
        id.range = range;

        id.enchantId = enchantId;
        id.dismantleId = dismantleId;
        id.sell = sell;
        id.description = description;
        id.atlasName = atlasName;
        id.iconName = iconName;
        id.skillname = skillname;
        id.skillFactorpowmp = skillFactorpowmp;
        id.skillFactorpowap = skillFactorpowap;
        id.powmpdef = powmpdef;
        id.powapdef = powapdef;
        id.skilllvid = skilllvid;
        id.skillmove = skillmove;
        id.use_MP = use_MP;
        id.use_AP = use_AP;
        id.conditionid_01 = conditionid_01;
        id.conditionid_02 = conditionid_02;
        id.conditionid_03 = conditionid_03;
        id.skilllv = 1;
        id.skillinit(id.skilllv);
        return id;
    }

}
