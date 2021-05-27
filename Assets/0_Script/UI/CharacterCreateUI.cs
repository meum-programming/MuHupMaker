using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
namespace A_Script
{
    public class CharacterCreateUI : InitBase
    {
        CharacterSelectData data;
        CharacterUI cu;
        Button Next;
        public CharacterWeapon CW;

        public InputField nickNameInputField;

        public void Init(CharacterUI _cu) {
            cu = _cu;
            localinit();
            data = new CharacterSelectData();
            var job = transform.Find("Left/Bg/Job");
            for (int i = 0; i < job.childCount; i++)
            {
                var toggle = job.transform.GetChild(i).GetComponent<Toggle>();
                toggle.onValueChanged.RemoveAllListeners();
                toggle.onValueChanged.AddListener(JobEvent(i));

            }
            var gender = transform.Find("Left/Bg/Gender");
            for (int i = 0; i < gender.childCount; i++)
            {
                var toggle = gender.transform.GetChild(i).GetComponent<Toggle>();
                toggle.onValueChanged.RemoveAllListeners();
                toggle.onValueChanged.AddListener(GenderEvent(i));

            }
            var grade = transform.Find("Right/Grade/TogglePanel");
            for (int i = 0; i < grade.childCount; i++)
            {
                var toggle = grade.transform.GetChild(i).GetComponent<Toggle>();
                toggle.onValueChanged.RemoveAllListeners();
                toggle.onValueChanged.AddListener(WeaponType(i));

            }
            transform.Find("BtnPanel/BackBtn").GetComponent<Button>().onClick.AddListener(Back);
        }
        public void Show() {
            transform.Find("Left/Bg/Job").GetChild(0).GetComponent<Toggle>().isOn = true;
            transform.Find("Left/Bg/Job").GetChild(0).GetComponent<Toggle>().onValueChanged.Invoke(true);
           transform.Find("Left/Bg/Gender").GetChild(0).GetComponent<Toggle>().isOn = true;
            transform.Find("Left/Bg/Gender").GetChild(0).GetComponent<Toggle>().onValueChanged.Invoke(true);
            transform.Find("Right/Grade/TogglePanel").GetChild(0).GetComponent<Toggle>().isOn = true;
            transform.Find("Right/Grade/TogglePanel").GetChild(0).GetComponent<Toggle>().onValueChanged.Invoke(true);
            nickNameInputField.text = string.Empty;

        }
        public void Back() {
            //캐선창 더해야함
            cu.CreateClose();
        }
        public Slider HP;
        public Slider MP;
        public Slider AP;
        public Text HPtext;
        public Text MPtext;
        public Text APtext;
        UnityEngine.Events.UnityAction<bool> JobEvent(int i)
        {
            return (b) => {

                if (b == false)
                    return;
                
                data.Jap = i + 1;

                var p_data = DataManager.i.charlvTable.charlvinfos[data.Jap][1];

                float maxValue = 15;

                HP.value = p_data.addhp / maxValue;
                MP.value = p_data.addmp / maxValue;
                AP.value = p_data.addap / maxValue;

            };
        }
        UnityEngine.Events.UnityAction<bool> GenderEvent(int i)
        {
            return (b) => {
                data.Gender = i;
            };
        }
        UnityEngine.Events.UnityAction<bool> WeaponType(int i)
        {
            return (b) => {
                data.WeaponType = i+1;
                CW.SetPrefab(i + 1);
            };
        }
        public void Create() {

            string checkStr = nickNameInputField.text.Trim();
            if (checkStr == string.Empty)
            {
                Debug.LogWarning("닉네임 공백임");
                return;
            }

            //특수문자 추출
            string otherStr = Regex.Replace(checkStr, @"[ ^0-9a-zA-Z가-힣 ]", "", RegexOptions.Singleline);
            if (otherStr.Trim() != string.Empty)
            {
                Debug.LogWarning("특수문자 있음");
                return;
            }

            data.nickName = checkStr;

            cu.Created(data);
        }
        public override void initactive()
        {
            Next = transform.Find("BtnPanel/NextBtn").GetComponent<Button>();
            Next.onClick.AddListener(Create);
        }
    }
}
