using System.Collections;
using System.Collections.Generic;
using System.Linq;
using A_Script;
using UnityEngine;
using UnityEngine.UI;

public class RebirthController : MonoBehaviour
{
    public Button rebirthBtn;
    public Text rebirthText;

    public GameObject popup;
    public Text popupTitleText;
    public Text popupSelectWeaponText;
    public ToggleGroup popupWeaponToggleGroup;
    int selectWeaponType = 1;

    public GameObject skillBtnPanel;
    private List<SkillBtn> skillBtnList = new List<SkillBtn>();

    private Player player;
    bool itemChangeEventOn = false;

    bool skillAutoOn = false;
    public RectTransform skillAutoBtnObj;
    public Animator skillAutoRatateAnim;
    float autoDelay = 0;

    void Awake()
    {
        PlayerDataManager.ins.updateOn += Reset;

        GameManager.i.player.rebirthReadyEvent += AutoSkillOn;

        //환생 팝업의 무기 선택 토글
        List<Toggle> toggleList = popupWeaponToggleGroup.GetComponentsInChildren<Toggle>().ToList();
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].onValueChanged.RemoveAllListeners();
            toggleList[i].onValueChanged.AddListener(WeaponType(i));
        }

        //환생 스킬 버튼 리스트
        skillBtnList = skillBtnPanel.GetComponentsInChildren<SkillBtn>().ToList();
        for (int i = 0; i < skillBtnList.Count; i++)
        {
            skillBtnList[i].btnClass.onClick.RemoveAllListeners();
            int index = i;
            skillBtnList[i].btnClass.onClick.AddListener(()=> SkillBtnClickOn(index));
        }

        Reset();
    }

    /// <summary>
    /// 무기 변경시 호출
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ui"></param>
    void ChangeEvent(ItemData id, UsingItem ui)
    {
        Reset();
    }

    /// <summary>
    /// 환생정보 초기화
    /// </summary>
    public void Reset()
    {
        SkillBtnReset();

        int charLv = PlayerDataManager.ins.p_data.currentData.lv;
        int rebirthID = PlayerDataManager.ins.p_data.rebirthID;
        RebirthInfo nextRebirthInfo = PlayerDataManager.ins.GetRebirthInfo(rebirthID+1);

        if (nextRebirthInfo == null)
            return;

        bool againLifeOn = charLv >= nextRebirthInfo.rebirthLv;
        rebirthBtn.gameObject.SetActive(againLifeOn);
        rebirthText.text = nextRebirthInfo.rebirthName;
    }

    /// <summary>
    /// 환생 스킬 버튼 초기화
    /// </summary>
    void SkillBtnReset()
    {
        int rebirthID = PlayerDataManager.ins.p_data.rebirthID;

        bool autoBtnOn = false;

        for (int i = 0; i < skillBtnList.Count; i++)
        {
            //이미지 활성화 플래그
            int rebirthWeaponType = PlayerDataManager.ins.GetRebirthWeapon(i + 1);
            int playerWeaponType = PlayerClassIsNull() == false ? player.Weapon.item.type : 0;

            bool activeOn = (i < rebirthID) && (rebirthWeaponType == playerWeaponType);
			skillBtnList[i].BtnActiveSet(activeOn);

            if (activeOn)
                autoBtnOn = true;
        }

        skillAutoBtnObj.gameObject.SetActive(autoBtnOn);
    }

    /// <summary>
    /// 예약된 스킬 사용
    /// </summary>
    /// <param name="skillBtnIndex"></param>
    void AutoSkillOn(int skillBtnIndex)
    {
        SkillBtnClickOn(skillBtnIndex);
    }

    /// <summary>
    /// 환생 스킬 버튼 클릭시 호출
    /// </summary>
    /// <param name="skillBtnIndex"></param>
    void SkillBtnClickOn(int skillBtnIndex)
    {
        int weaponType = PlayerDataManager.ins.GetRebirthWeapon(skillBtnIndex + 1);
        ActiveSkillData activeSkillData = DataManager.i.activeSkillTable.GetData(weaponType - 1);

        if (activeSkillData.target != 1 && player.Us == UnitState.Move)
        {
            player.readySkillBtnIndex = skillBtnIndex;
            return;
        }
        
        int rebirthWeaponType = PlayerDataManager.ins.GetRebirthWeapon(skillBtnIndex + 1);
        float cullTime = DataManager.i.activeSkillTable.GetData(rebirthWeaponType).coolTime;
        bool cullTimeSetClear = skillBtnList[skillBtnIndex].CullTimeSet(cullTime);

        if (cullTimeSetClear == false)
            return;

        if (PlayerClassIsNull())
            return;

        player.RebirthSkillOn(weaponType);

        PlayerDataManager.ins.AddValue(PlayerStatusEnum.AP, activeSkillData.useAP);
        PlayerDataManager.ins.AddValue(PlayerStatusEnum.MP, activeSkillData.useMP);
    }

    bool PlayerClassIsNull()
    {
        if (player == null)
        {
            player = GameManager.i.player;

            if (player != null && itemChangeEventOn == false)
            {
                player.Weapon.ChangeCallback += ChangeEvent;
                itemChangeEventOn = true;
            }
            else
            {
                return true;
            }
        }

        if (player == null || player.Weapon == null || player.Weapon.item == null)
            return true;
        
        return false;
    }


    /// <summary>
    /// 환생하기 버튼 클릭시 호출
    /// </summary>
    /// <param name="activeOn"></param>
    public void RebirthReady(bool activeOn)
    {
        if (activeOn)
        {
            int rebirthID = PlayerDataManager.ins.p_data.rebirthID;
            RebirthInfo nextRebirthInfo = PlayerDataManager.ins.GetRebirthInfo(rebirthID + 1);
            string titleStr = string.Format("Lv.{0} {1}\n{2}차 오의 습득 가능", nextRebirthInfo.rebirthLv,
                                                                            nextRebirthInfo.rebirthName,
                                                                            nextRebirthInfo.rebirthID);
            popupTitleText.text = titleStr;

            string selectWeaponStr = string.Format("{0}차 오의 선택", nextRebirthInfo.rebirthID);
            popupSelectWeaponText.text = selectWeaponStr;
        }

        popup.gameObject.SetActive(activeOn);
    }

    /// <summary>
    /// 환생 진행중 무기 타입 선택시 호출
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    UnityEngine.Events.UnityAction<bool> WeaponType(int i)
    {
        return (b) => {
            if (b == true)
            {
                selectWeaponType = (i + 1);
            }
        };
    }

    /// <summary>
    /// 환생 시작
    /// </summary>
    public void RebirthStart()
    {
        popup.gameObject.SetActive(false);
        rebirthBtn.gameObject.SetActive(false);

        PlayerDataManager.ins.SetRebirthWeapon(PlayerDataManager.ins.p_data.rebirthID +1, selectWeaponType);
        PlayerDataManager.ins.RebirthStart();

        //환생 이펙트 실행
        player.RebirthEffectOn();
    }

    public void AutoBtnClick()
    {
        skillAutoOn = !skillAutoOn;

        string animName = skillAutoOn ? "Rotate" : "Idle";
        skillAutoRatateAnim.Play(animName);

        if (skillAutoOn)
        {
            autoDelay = 1;
        }
    }

    void Update()
    {
        AutoDelayCheck();
    }

    void AutoDelayCheck()
    {
        if (skillAutoOn == false || player.Us == UnitState.RebirthSkill || player.Us == UnitState.Die)
            return;

        autoDelay += Time.deltaTime;

        if (autoDelay < 1)
            return;

        autoDelay = 0;

        //공격타겟(몹)이 없다면 
        if (player.Findlist.Count == 0)
            return;
        
        int rebirthID = PlayerDataManager.ins.p_data.rebirthID;

        for (int i = skillBtnList.Count-1; i >= 0; i--)
        {
            if (i < rebirthID &&
                skillBtnList[i].gameObject.activeSelf == true &&
                skillBtnList[i].cullTimeOn == false)
            {
                SkillBtnClickOn(i);
                break;
            }   
        }
    }
}
