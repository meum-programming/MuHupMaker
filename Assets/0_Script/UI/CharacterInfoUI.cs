using System.Collections;
using System.Collections.Generic;
using A_Script;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    public Text textIns;

    public Transform simpleTextPanel;
    private List<Text> simpleTextList = new List<Text>();

    public Transform detailTextPanel_0;
    private List<Text> detailTextList_0 = new List<Text>();

    public Transform detailTextPanel_1;
    private List<Text> detailTextList_1 = new List<Text>();

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.i.player;
        PlayerDataManager.ins.updateOn += TextSet;
        TextSet();
    }

    void TextSet()
    {
        SetSimpleText();

        if (player != null)
        {
            SetDetailText_0();
            SetDetailText_1();
        }
        
    }

    /// <summary>
    /// 기본 정보 텍스트 세팅
    /// </summary>
    void SetSimpleText()
    {
        PlayerDataManager manager = PlayerDataManager.ins;
        Dictionary<string, string> dataDic = new Dictionary<string, string>()
        {
            {"레벨", manager.p_data.currentData.lv.ToString() },
            {"오성", manager.GetCharacterTypeStr((PlayerCharacterType)manager.p_data.Jap)},
            {"오의", manager.p_data.rebirthID == 0 ? "없음":manager.GetRebirthInfo(manager.p_data.rebirthID).rebirthName},
            {"체력", manager.maxData.hp.ToString()},
            {"내공", manager.maxData.mp.ToString()},
            {"외공", manager.maxData.ap.ToString()},
        };

        if (simpleTextList.Count != dataDic.Count)
        {
            for (int i = simpleTextList.Count; i < dataDic.Count; i++)
            {
                Text obj = Instantiate(textIns, simpleTextPanel);
                obj.gameObject.SetActive(true);
                simpleTextList.Add(obj);
            }
        }

        int index = 0;
        foreach (KeyValuePair<string, string> kvp in dataDic)
        {
            string strValue = string.Format("{0} : {1}", kvp.Key, kvp.Value);
            simpleTextList[index].text = strValue;
            index++;
        }
    }

    /// <summary>
    /// 상세 정보(0) 텍스트 세팅
    /// </summary>
    void SetDetailText_0()
    {
        var def = player.GetTotalPowDef();

        float moveSpeed = player.MovSpeed + (player.GetSkill(SkillType.Speed).IsIt ? player.GetSkill(SkillType.Speed).Skill.skillmove : 0);

        Dictionary<string, string> dataDic = new Dictionary<string, string>()
        {
            {"내력", player.GetPowMP().ToString()},
            {"외력", player.GetPowAP().ToString()},
            {"내력방어", def.mp.ToString()},
            {"외력방어", def.ap.ToString()},
            {"공격속도", (player.AtkSpeed * 0.0001).ToString()},
            {"이동속도", (moveSpeed * 0.0001).ToString()},
        };

        if (detailTextList_0.Count != dataDic.Count)
        {
            for (int i = detailTextList_0.Count; i < dataDic.Count; i++)
            {
                Text obj = Instantiate(textIns, detailTextPanel_0);
                obj.gameObject.SetActive(true);
                detailTextList_0.Add(obj);
            }
        }

        int index = 0;
        foreach (KeyValuePair<string, string> kvp in dataDic)
        {
            string strValue = string.Format("{0} : {1}", kvp.Key, kvp.Value);
            detailTextList_0[index].text = strValue;
            index++;
        }
    }

    /// <summary>
    /// 상세 정보(1) 텍스트 세팅
    /// </summary>
    void SetDetailText_1()
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>()
        {
            {"급소타격확률", string.Format("{0}%",player.CrtRate * 100)},
            {"급소타격피해", string.Format("{0}%",player.CrtDmg * 100)},
            {"회피확률",    string.Format("{0}%",player.Dodge * 100)},
        };

        if (detailTextList_1.Count != dataDic.Count)
        {
            for (int i = detailTextList_1.Count; i < dataDic.Count; i++)
            {
                Text obj = Instantiate(textIns, detailTextPanel_1);
                obj.gameObject.SetActive(true);
                detailTextList_1.Add(obj);
            }
        }

        int index = 0;
        foreach (KeyValuePair<string, string> kvp in dataDic)
        {
            string strValue = string.Format("{0} : {1}", kvp.Key, kvp.Value);
            detailTextList_1[index].text = strValue;
            index++;
        }
    }
}
