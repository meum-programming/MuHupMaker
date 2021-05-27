using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStateUI :MonoBehaviour
{
    public Camera target;
    float maxhp;
    float _hp;
    public float hp
    {
        set
        {
            _hp = value;
            UpdateHpValue();
        }
    }
    float maxap;
    float _ap;
    public float ap
    {
        set
        {
            _ap = value;
            UpdateApValue();
        }
    }
    float maxmp;
    float _mp;
    public float mp
    {
        set
        {
            _mp = value;
            UpdateMpValue();
        }
    }
    float maxexp;
    float _exp;
    public float exp
    {
        set
        {
            _exp = value;
            UpdateExpValue();
        }
    }
    float maxhpsize = 1;
    RectTransform hpgage;
    float hpgagey;
    float maxapsize = 1;
    RectTransform apgage;
    float apgagey;
    float maxmpsize = 1;
    RectTransform mpgage;
    float mpgagey;
    float maxexpsize = 1;
    RectTransform expgage;
    float expgagey;
    Text hptext;
    Text aptext;
    Text mptext;
    Text exptext;
    Text lvtext;
    bool showhp = false;
    bool showap = false;
    bool showmp = false;
    bool showexp = false;
    bool showlv = false;
    public void UpdateData(float MaxHP, float MaxAP, float MaxMP, float HP, float AP, float MP, float MaxEXP=0, float EXP=0) {
        maxhp = MaxHP;
        maxap = MaxAP;
        maxmp = MaxMP;
        maxexp = MaxEXP;
        hp = HP;
        ap = AP;
        mp = MP;
        exp = EXP;
    }
    protected bool IsTarget = false;
    public void Init(Camera _target, float MaxHP, float MaxAP,float MaxMP, float HP, float AP,float MP, float MaxEXP =0, float EXP=0, bool ShowText = true, bool rotation = true) {
        if (_target != null)
        {
            target = _target;
            GetComponent<Canvas>().worldCamera = target;
            if (rotation)
                IsTarget = true;
        }
        else {
            IsTarget = false;
        }

        /*
        if (ShowText)
        {
            var hptexttran = transform.Find("hp/Text");
            if (hptexttran)
            {
                showhp = true;
                hptext = hptexttran.GetComponent<Text>();

            }
            var aptexttran = transform.Find("ap/Text");
            if (aptexttran)
            {
                showap = true;
                aptext = aptexttran.GetComponent<Text>();

            }
            var mptexttran = transform.Find("mp/Text");
            if (mptexttran)
            {
                showmp = true;
                mptext = mptexttran.GetComponent<Text>();

            }
            var exptexttran = transform.Find("exp/Text");
            if (exptexttran)
            {
                showexp = true;
                exptext = exptexttran.GetComponent<Text>();

            }
        }
        hpgage = transform.Find("hp/gage").GetComponent<RectTransform>();
        hpgagey = hpgage.sizeDelta.y;
        maxhpsize = hpgage.sizeDelta.x;
        maxhp = MaxHP;
        hp = HP;
        UpdateHpValue();
        var apgagetran = transform.Find("ap/gage");
        if (apgagetran != null)
        {

            apgage = apgagetran.GetComponent<RectTransform>();

            apgagey = apgage.sizeDelta.y;
            maxapsize = apgage.sizeDelta.x;
            ap = AP;
            maxap = MaxAP;
            UpdateApValue();
        }
        var mpgagetran = transform.Find("mp/gage");
        if (mpgagetran != null)
        {

            mpgage = mpgagetran.GetComponent<RectTransform>();

            mpgagey = mpgage.sizeDelta.y;
            maxmpsize = mpgage.sizeDelta.x;
            mp = MP;
            maxmp = MaxMP;
            UpdateMpValue();
        }

        if (transform.Find("exp"))
        {
            maxexp = MaxEXP;
            exp = EXP;
            //UpdateExpValue();
        }
        */
        /*
        var expgagetran = transform.Find("exp/gage");
        if (expgagetran != null)
        {

            expgage = expgagetran.GetComponent<RectTransform>();

            //expgagey = expgage.sizeDelta.y;
            //maxexpsize = expgage.sizeDelta.x;
            exp = EXP;
            maxexp = MaxEXP;
            UpdateExpValue();
        }

        //var lv = transform.Find("lv/Text");
        if (lv != null)
        {
            showlv = true;
            lvtext = lv.GetComponent<Text>();
        }
        */

    }
    public void UpdateLV(int i) {
        if (showlv) {
        }
    }
  
  
    void UpdateHpValue()
    {
        //Debug.LogWarning("_hp = " + _hp + " maxhp = "+ maxhp);
        //Debug.LogWarning("data HP = " + PlayerDataManager.ins.OriginData.hp);
        /*
        hpgage.GetComponent<Image>().fillAmount = (_hp / maxhp);
        hpgage.sizeDelta = new Vector2((_hp/maxhp) * maxhpsize, hpgagey);
        if (showhp)
            hptext.text = Mathf.Ceil(_hp).ToString() + "/" + Mathf.Ceil(maxhp).ToString();
            */
    }
    void UpdateApValue()
    {
        //apgage.sizeDelta = new Vector2((_ap / maxap) * maxapsize, apgagey);
        //if (showap)
        //    aptext.text = Mathf.Ceil(_ap).ToString() + "/" + Mathf.Ceil(maxap).ToString();
    }
    void UpdateMpValue()
    {
        //mpgage.sizeDelta = new Vector2((_mp / maxmp) * maxmpsize, mpgagey);
        //if (showap)
        //    mptext.text = Mathf.Ceil(_mp).ToString() + "/" + Mathf.Ceil(maxmp).ToString();
    }
    void UpdateExpValue()
    {
        /*
        if (transform.Find("exp"))
        {
            Slider slider = transform.Find("exp").GetComponent<Slider>();
            slider.value = _exp / maxexp;
        }

        if (showexp)
            exptext.text = Mathf.Ceil(_exp).ToString() + "/" + Mathf.Ceil(maxexp).ToString() + "("+ ((int)(_exp / maxexp *100f))+ "%)";
            */
    }

    // Update is called once per frame

}
