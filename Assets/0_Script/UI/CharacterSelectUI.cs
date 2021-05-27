using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UniRx;

namespace A_Script
{
    public class CharacterSelectUI : InitBase
    {
        public CharacterWeapon CW;
        public int slotcount = 5;
        Transform Slots;
        Dictionary<int, CharacterSelectData> Characters = new Dictionary<int, CharacterSelectData>();
        Dictionary<int, Toggle> Toggles = new Dictionary<int, Toggle>();
  
        CharacterUI cu;

        /// <summary> 선택된 토클 인덱스 </summary>
        int selectindex = -1;
        /// <summary> 이전 선택된 토클 인덱스 </summary>
        int beforeSelectindex = -1;
        /// <summary> 삭제 확인 팝업 </summary>
        public RectTransform deleteCheckPopup;

        public void Init(CharacterUI _cu)
        {
            cu = _cu;
            SetEvent();
            SetSaveData();
            deleteCheckPopup.gameObject.SetActive(false);
        }

        /// <summary>
        /// 이벤트 등록
        /// </summary>
        void SetEvent()
        {
            Slots = transform.Find("Left/Slots");
            for (int i = 0; i < slotcount; i++)
            {
                Characters[i] = (new CharacterSelectData(i));

                int index = i;

                Toggles[i] = Slots.GetChild(i).GetComponent<Toggle>();

                //슬롯 토클이 클릭되었을때 실행
                Toggles[i]
                    .OnValueChangedAsObservable()
                    .Where(changeFlag => changeFlag)
                    .Select(_=> index)
                    .Subscribe(SlotsEvnet)
                    .AddTo(gameObject);
            }

            Button enterBtn = transform.Find("BtnPanel/EnterBtn").GetComponent<Button>();
            enterBtn
                .OnClickAsObservable()
                .Subscribe(_ => GameStartEvent())
                .AddTo(gameObject);

            Button deleteCheckBtn = transform.Find("BtnPanel/DeleteCheckBtn").GetComponent<Button>();
            deleteCheckBtn
                .OnClickAsObservable()
                .Subscribe(_ => DeleteCheckEvent())
                .AddTo(gameObject);

            Button deleteOnBtn = deleteCheckPopup.transform.Find("BtnPanel/DeleteOnBtn").GetComponent<Button>();
            deleteOnBtn
                .OnClickAsObservable()
                .Subscribe(_ => DeleteSlotEvent())
                .AddTo(gameObject);
        }

        /// <summary>
        /// 생성창에서 뒤로가기 버튼 클릭시 호출
        /// </summary>
        public void CreateBack()
        {
            Toggles[beforeSelectindex].isOn = true;
        }

        /// <summary>
        /// 슬롯 클릭 이벤트
        /// </summary>
        /// <param name="index"></param>
        void SlotsEvnet(int index)
        {
            beforeSelectindex = selectindex;
            selectindex = index;
            if (!Characters[index].isit)
            {
                cu.ShowCreate();
            }
            else
            {
                CW.SetPrefab(Characters[selectindex].WeaponType);
            }
        }

        /// <summary>
        /// 저장된 데이타 로드
        /// </summary>
        void SetSaveData()
        {
            for (int i = 0; i < slotcount; i++)
            {
                string keyValue = string.Format("{0}{1}", PlayerDataManager.ins.chaStateDataKey, i);

                CharacterSelectData obj = PlayerPrefsUtils.GetObject<CharacterSelectData>(keyValue);

                if (obj != null)
                {
                    selectindex = i;
                    Created(obj);
                }
            }

            if (selectindex != -1)
            {
                Toggles[selectindex].isOn = true;
            }
        }

        /// <summary>
        /// 캐릭터 생성
        /// </summary>
        /// <param name="csd"></param>
        public void Created(CharacterSelectData csd)
        {
            CW.gameObject.SetActive(true);
            CW.SetPrefab(csd.WeaponType);

            csd.key = selectindex;
            Characters[selectindex] = csd;
            Characters[selectindex].isit = true;
            UpdateChracterinfo(csd);
            Toggles[selectindex].isOn = true;

            string keyValue = string.Format("{0}{1}", PlayerDataManager.ins.chaStateDataKey, selectindex);
            PlayerPrefsUtils.SetObject(keyValue, Characters[selectindex]);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 슬롯 정보 세팅
        /// </summary>.
        /// <param name="csd"></param>
        void UpdateChracterinfo(CharacterSelectData csd)
        {
            var target = transform.Find("Left/Slots/" + csd.key.ToString());
            target.Find("Space/Name").GetComponent<Text>().text = csd.isit ? csd.nickName : string.Empty;
            target.Find("Space/Lv").GetComponent<Text>().text = csd.isit ? "Lv :" + csd.currentData.lv.ToString() : string.Empty ;
        }

        /// <summary>
        /// 슬롯 삭제 확인
        /// </summary>
        public void DeleteCheckEvent()
        {
            Text titleText = deleteCheckPopup.transform.Find("Title").GetComponent<Text>();
            titleText.text = string.Format("{0} 캐릭터를\n삭제하시겠습니까?", Characters[selectindex].nickName);
            deleteCheckPopup.gameObject.SetActive(true);
        }

        /// <summary>
        /// 슬롯 삭제 
        /// </summary>
        public void DeleteSlotEvent()
        {
            deleteCheckPopup.gameObject.SetActive(false);

            Characters[selectindex] = (new CharacterSelectData(selectindex));
            UpdateChracterinfo(Characters[selectindex]);

            DeleteSaveData();
            
            bool allDeleteOn = true;
            foreach (var character in Characters)
            {
                if (character.Value.isit)
                {
                    selectindex = character.Key;
                    allDeleteOn = false;
                    break;
                }
            }

            if (allDeleteOn)
            {
                selectindex = 0;
                Toggles[0].isOn = false;
            }

            Toggles[selectindex].isOn = true;
        }

        /// <summary>
        /// 저장된 정보를 삭제
        /// </summary>
        void DeleteSaveData()
        {
            PlayerDataManager.ins.DataDelete(selectindex);
        }

        /// <summary>
        /// 게임 시작
        /// </summary>
        public void GameStartEvent()
        {
            if (Characters[selectindex].isit == false)
                return;

            PlayerDataManager.ins.Init(selectindex, Characters[selectindex]);
            cu.GameStart(Characters[selectindex]);
        }
    }
} 