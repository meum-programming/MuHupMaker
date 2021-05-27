using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    public Button btnClass;
    public Image bgImage;
    public Image fillImage;

    public bool cullTimeOn = false;

    float currentCullTime = 0;
    float maxCullTime = 0;

    public bool activeOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BtnActiveSet(bool _activeOn)
    {
        activeOn = _activeOn;
        bgImage.gameObject.SetActive(activeOn);
    }

    public bool CullTimeSet(float cullTime)
    {
        if (activeOn == false || cullTimeOn == true)
            return false;
        
        maxCullTime = cullTime;
        currentCullTime = 0;
        cullTimeOn = true;

        return true;
    }

    private void Update()
    {
        CullTimeCheck();
    }

    void CullTimeCheck()
    {
        if (cullTimeOn == false)
            return;

        currentCullTime += Time.deltaTime;

        //쿨타임 끝
        if (currentCullTime >= maxCullTime)
        {
            currentCullTime = maxCullTime;
            cullTimeOn = false;
        }

        float fillAmountValue = currentCullTime / maxCullTime;
        fillImage.fillAmount = fillAmountValue;
    }
}
