using System.Collections;
using System.Collections.Generic;
using A_Script;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public PlayerStateUI psu;

    public Text nickNameText;
    public Text lvText;

    public Text expText;
    public Slider expSlider;

    public Text hpText;
    public Slider hpSlider;

    public Text mpText;
    public Slider mpSlider;

    public Text apText;
    public Slider apSlider;

    public Text fieldBossCntText;
    public Slider fieldBossCntSlider;
    public Button fieldBossBtn;

    public void Start()
    {
        PlayerDataManager.ins.updateOn += UpdatePlayerStatus;
        GameManager.i.fieldBossCntChange += FieldBossCntChange;
        GameManager.i.bossKillEvent += BossKillOn;
        UpdatePlayerStatus();
    }

    /// <summary>
    /// 플레이어 스텟을 업데이트
    /// </summary>
    void UpdatePlayerStatus()
    {
        //플레이어 스텟 데이타
        PlayerData currentData = PlayerDataManager.ins.p_data.currentData;
        PlayerData maxData = PlayerDataManager.ins.maxData;

        //닉네임 세팅
        nickNameText.text = A_Script.GameManager.i.csd.nickName;

        //환생 데이타
        int rebirthID = PlayerDataManager.ins.p_data.rebirthID;
        RebirthInfo rebirthInfo = PlayerDataManager.ins.GetRebirthInfo(rebirthID);

        //레벨 세팅
        string lvRebirthStr = rebirthInfo == null ? string.Empty : string.Format("\n{0}", rebirthInfo.rebirthName);
        lvText.text = string.Format("  LV: {0}{1}", currentData.lv, lvRebirthStr);

        //경험치 세팅
        expSlider.value = (float)currentData.exp / maxData.exp;
        expText.text = string.Format("{0}/{1}({0,1:P1})", currentData.exp, maxData.exp); //{0,1:P1} => 0값을 1로 나누고 %기호 표시

        //HP 세팅
        hpSlider.value = (float)currentData.hp / maxData.hp;
        hpText.text = string.Format("{0}/{1}", (int)currentData.hp, (int)maxData.hp);

        //MP 세팅
        mpSlider.value = (float)currentData.mp / maxData.mp;
        mpText.text = string.Format("{0}/{1}", (int)currentData.mp, (int)maxData.mp);

        //AP 세팅
        apSlider.value = (float)currentData.ap / maxData.ap;
        apText.text = string.Format("{0}/{1}", (int)currentData.ap, (int)maxData.ap);
    }

    public void FieldBossCntChange(FieldBossData data)
    {
        if (data == null)
        {
            fieldBossCntSlider.gameObject.SetActive(false);
            fieldBossCntText.gameObject.SetActive(false);
            fieldBossBtn.gameObject.SetActive(false);

            return;
        }

        fieldBossCntSlider.value = (float)data.currnetFieldBossCount / data.maxFieldBossCount;
        fieldBossCntText.text = data.currnetFieldBossCount.ToString();

        fieldBossCntSlider.gameObject.SetActive(true);
        fieldBossCntText.gameObject.SetActive(true);

        bool iconActiveOn = data.currnetFieldBossCount >= data.maxFieldBossCount;

        if (iconActiveOn)
        {
            fieldBossBtn.gameObject.SetActive(iconActiveOn);
            BossActiveOn();
        }
    }

    public void BossActiveOn()
    {
        GameManager.i.FieldBossCntReset();
        GameManager.i.LoadScene("Stage_Boss");
    }

    public void BossKillOn()
    {
        StartCoroutine("BackToMap");
    }
    public IEnumerator BackToMap()
    {
        GameManager.i.player.Wait();

        yield return new WaitForSeconds(5);

        //GameManager.i.LoadBeforeScene();
        GameManager.i.LoadNextScene();
    }

    /// <summary>
    /// 레벨 강제 업 (임시 사용) 
    /// </summary>
    public void TempLvUp()
    {
        PlayerDataManager.ins.TemplvUp();
    }

}
