using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RebirthTapController : MonoBehaviour
{
    public GameObject contentObj;

    int selectmidTap = 0;
    int selectWeaponType = 0;

    public Text discriptText;

    public GameObject midTapToggleGroup;
    private List<Toggle> midTapList = new List<Toggle>();

    public GameObject weaponToggleGroup;
    private List<Toggle> weaponToggleList = new List<Toggle>();

    private void Awake()
    {
        midTapList = midTapToggleGroup.GetComponentsInChildren<Toggle>().ToList();
        for (int i = 0; i < midTapList.Count; i++)
        {
            midTapList[i].onValueChanged.RemoveAllListeners();
            midTapList[i].onValueChanged.AddListener(midTapSet(i));
        }

        weaponToggleList = weaponToggleGroup.GetComponentsInChildren<Toggle>().ToList();
        for (int i = 0; i < weaponToggleList.Count; i++)
        {
            weaponToggleList[i].onValueChanged.RemoveAllListeners();
            weaponToggleList[i].onValueChanged.AddListener(WeaponTypeSet(i));
        }

        ContentObjSetActive(false);
    }

    // Start is called before the first frame update
    public void ContentObjSetActive(bool active)
    {
        contentObj.gameObject.SetActive(active);

        if (active)
        {
            DataReset();
        }
    }

    /// <summary>
    /// 데이터 초기화
    /// </summary>
    void DataReset()
    {
        int rebirthID = PlayerDataManager.ins.p_data.rebirthID;

        for (int i = 0; i < midTapList.Count; i++)
        {
            //탭 활성화 플래그
            bool activeOn = (i < rebirthID);

            midTapList[i].interactable = activeOn;

            Text discriptText = midTapList[i].transform.Find("Text").GetComponent<Text>();
            discriptText.color = activeOn ? new Color32(217, 156, 156, 255) :
                                            new Color32(77, 77, 77, 255);
        }

        selectWeaponType = 0;
        DiscriptSet();

        //환생 진행 여부 플래그
        bool rebirthOn = rebirthID > 0;

        discriptText.gameObject.SetActive(rebirthOn);
        weaponToggleGroup.gameObject.SetActive(rebirthOn);
    }

    /// <summary>
    /// 중앙 탭 토글 선택시 호출
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    UnityEngine.Events.UnityAction<bool> midTapSet(int i)
    {
        return (b) => {
            if (b == true)
            {
                selectmidTap = i;
                //selectWeaponType = i;
                //DiscriptSet();
            }
        };
    }

    /// <summary>
    /// 무기 종류 선택시 호출
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    UnityEngine.Events.UnityAction<bool> WeaponTypeSet(int i)
    {
        return (b) => {
            if (b == true)
            {
                selectWeaponType = i;
                DiscriptSet();
            }
        };
    }

    private void DiscriptSet()
    {
        TempRebirthData data = GetText(selectWeaponType);

        discriptText.text = string.Format("{0}\n\n{1}\n\n{2}", data.title,
                                                               data.discript_0,
                                                               data.discript_1);
    }

    // 추후 CSV에서 추출
    TempRebirthData GetText(int selectWeaponType)
    {
        string title = A_Script.DataManager.i.activeSkillTable.GetData(selectWeaponType).activeSkillName;
        string discript_0 = string.Empty;
        string discript_1 = string.Empty;

        switch (selectWeaponType)
        {
            case 0:
                discript_0 = "내공의 10 %\n외공의 10 %\n피해를 입히고 대상을 1초간 경직 시킨다.";
                discript_1 = "기예:1성";
                break;
            case 1:
                discript_0 = "내공의 10 %\n외공의 10 %\n피해를 입히고 대상을 1초간 경직 시킨다.";
                discript_1 = "기예:1성";
                break;
            case 2:
                discript_0 = "내공의 10 %\n외공의 10 %\n피해를 입히고 대상을 1초간 경직 시킨다.";
                discript_1 = "기예:1성";
                break;
            case 3:
                discript_0 = "내공의 10 %\n외공의 10 %\n피해를 입히고 대상을 1초간 경직 시킨다.";
                discript_1 = "기예:1성";
                break;
            case 4:
                discript_0 = "내공의 10 %\n외공의 10 %\n피해를 입히고 대상을 1초간 경직 시킨다.";
                discript_1 = "기예:1성";
                break;
        }

        return new TempRebirthData(title, discript_0, discript_1);
    }

}



// 추후 Manager에서 관리
public class TempRebirthData
{
    public string title = string.Empty;
    public string discript_0 = string.Empty;
    public string discript_1 = string.Empty;

    public TempRebirthData() { }
    public TempRebirthData(string title, string discript_0, string discript_1)
    {
        this.title = title;
        this.discript_0 = discript_0;
        this.discript_1 = discript_1;
    }
}
