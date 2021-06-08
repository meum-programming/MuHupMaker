using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace A_Script
{
    public class GameManager : MonoBehaviour
    {
        public Dictionary<int, bool> MouseUIDic = new Dictionary<int, bool>();
        public LayerMask mask;

        private static GameManager _i;
        public static GameManager i
        {
            get
            {
                if (_i == null)
                {
                    _i = FindObjectOfType<GameManager>();

                    if (_i == null)
                    {
                        string path = "UI/Ingame/Manager";

                        if (Resources.Load(path) == null)
                        {
                            Debug.LogWarning(string.Format("해당 위치에 오브젝트 없음\npath = Resources/{0}", path));
                        }
                        else
                        {
                            GameObject obj = Instantiate(Resources.Load(path) as GameObject);
                            _i = obj.GetComponent<GameManager>();
                        }
                    }
                }
                return _i;
            }
        }

        // Start is called before the first frame update
        public List<Player> Players = new List<Player>();
        public Player player {
            get {
                return Players[0];
            }
        }
        public List<Transform> Waypoints = new List<Transform>();
        StageCamera _ActiveStageCamera;
        public StageCamera ActiveStageCamera {
            get {
                return _ActiveStageCamera;
            }
            set {
                _ActiveStageCamera = value;
                camera = _ActiveStageCamera.GetComponent<Camera>();
            }
        }
        Camera camera;
        public List<Transform> UILookAt = new List<Transform>();
        public static int MaxInvenEquipCount = 300;
        public static int MaxInvenSkillCount = 100;
        public static int MaxInvenConsumCount = 100;

        public static int MaxInvenAllCount = MaxInvenEquipCount + MaxInvenSkillCount + MaxInvenConsumCount;
        public CharacterData CD = new CharacterData();
        public InventoryUI IUI;
        public SkillUI SIU;
        public bool Smooth = false;
        public float SmoothTime = 0.1f;
        public float shake = 0.05f;
        public float shakeAmount = 0.1f;
        public float decreaseFactor = 0.1f;
        public Action<Transform> TargetEvent = null;
        public Action<Vector3> PointEvent = null;

        public GameObject Resurrection;
        public Text ResurrectionText;
        public CharacterSelectData csd;
        bool first = false;
        public string beforeScene;
        public string activeScene;

        public Dictionary<string, FieldBossData> fieldBossDataDic = new Dictionary<string, FieldBossData>();
        public UnityAction<FieldBossData> fieldBossCntChange;
        public UnityAction bossKillEvent;

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
        public void Init(CharacterSelectData _csd)
        {
            csd = _csd;
            if (Players.Count == 0)
            {
                var p = Instantiate(Resources.Load("Player/Woman") as GameObject);

                Players.Add(p.GetComponent<Player>());
            }

            foreach (var data in DataManager.i.fieldBossTable.dataDic)
            {
                fieldBossDataDic.Add(data.Key, data.Value);
            }
        }

        private void Update()
        {
            if (ActiveStageCamera == null)
                return;
            if (MouseUIDic.ContainsValue(true))
                return;
            if (Input.GetMouseButtonDown(0)) { 
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit,float.MaxValue, mask))
                {
                    Debug.Log(hit.collider.name);
                        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
                        {
                            PointEvent?.Invoke(hit.point);
                        }
                        else {
                            TargetEvent?.Invoke(hit.transform);
                        }
                }
            }
        }

        private void LateUpdate()
        {
            if (ActiveStageCamera != null)
            {
                for (int i = 0; i < UILookAt.Count; i++)
                {
                    UILookAt[i].rotation = ActiveStageCamera.transform.rotation;
                }
            }
        }
        
        public void InitScene()
        {
            if (first)
                return;
            first = true;
            transform.Find("PlayerCanvas").gameObject.SetActive(true);
            player.UnitInitDirect(DataManager.i.characterTable.charInfos[100101], CD.inventory);
            IUI.Init();
            SIU.Init();

            SetItem();
            SetSkill();
        }

        public void LoadBeforeScene()
        {
            LoadScene(beforeScene);
        }

        public void LoadScene(string sceneName)
        {
            beforeScene = activeScene;
            activeScene = sceneName;
            SceneManager.LoadScene(activeScene);
            player.Reset();
        }

        public void LoadNextScene()
        {
            string beforeSceneName = beforeScene;
            beforeScene = activeScene;
            
            string[] sceneNameSplit = beforeSceneName.Split('_');

            string newSceneName = beforeSceneName;

            if (sceneNameSplit.Length == 3)
            {
                int typeNum = int.Parse(sceneNameSplit[1]);
                int stageNum = int.Parse(sceneNameSplit[2]);

                stageNum += 1;

                if (stageNum > 4)
                {
                    stageNum = 1;
                    typeNum += 1;

                    if (typeNum > 8)
                    {
                        typeNum = 1;
                    }
                }

                newSceneName = string.Format("Stage_{0:d2}_{1:d2}", typeNum, stageNum);
            }

            activeScene = newSceneName;
            SceneManager.LoadScene(activeScene);
            player.Reset();
        }


        /// <summary>
        /// 아이템 세팅
        /// </summary>
        private void SetItem()
        {
            player.EquipInit();

            ItemSaveData itemSaveData = PlayerDataManager.ins.itemSaveData;

            //저장 데이타가 없다면
            if (itemSaveData == null)
            {
                var index = 2100000 + csd.WeaponType;
                ItemData weaponItem = CD.inventory.Equip.AddReceive(DataManager.i.totalTable.totalInfo[index]);
                player.Weapon.Equip(weaponItem);
                IUI.ShowTap(InventoryType.Equipment);
                return;   
            }

            //장착 아이템 데이타 획득
            List<int> saveEquipItemIdList = new List<int>()
            {
                itemSaveData.weaponId,
                itemSaveData.helmetId,
                itemSaveData.armorId,
                itemSaveData.pantsId,
                itemSaveData.bootsId,
                itemSaveData.guardId,
            };

            //ID가 0인 아이템 제외
            saveEquipItemIdList = saveEquipItemIdList
                                    .Where(id => id != 0)
                                    .ToList();

            //저장된 장착 아이템 적용
            foreach (int itemId in saveEquipItemIdList)
            {
                ItemData tempItemData = DataManager.i.totalTable.totalInfo[itemId];

                foreach (ItemData itemData in player.Inventory.Equip.GetList(tempItemData))
                {
                    if (itemData.id == tempItemData.id)
                    {
                        player.GetEquipData(tempItemData.itemtype).Equip(itemData);
                        break;
                    }
                }
            }

            IUI.ShowTap(InventoryType.Equipment);
        }

        /// <summary>
        /// 스킬 세팅
        /// </summary>
        private void SetSkill()
        {
            SkillSaveData skillSaveData = PlayerDataManager.ins.skillSaveData;

            if (skillSaveData == null)
            {
                var index = 1 + (csd.WeaponType * 100000);
                var skill = DataManager.i.skillTable.skillInfos[index];
                var selectskill = CD.skillinventory.Nomal.AddReceive(skill);
                player.Nomal.Set(selectskill,true);
                return;
            }

            SetSaveDataSkill(SkillType.Nomal);
            SetSaveDataSkill(SkillType.Speed);
            SetSaveDataSkill(SkillType.Special);
        }

        /// <summary>
        /// 저장된 무공 데이타 설정
        /// </summary>
        /// <param name="skillType"></param>
        private void SetSaveDataSkill(SkillType skillType)
        {
            SkillSaveData skillSaveData = PlayerDataManager.ins.skillSaveData;

            //저장된 무공 리스트
            List<int> saveSkillList = new List<int>();
            //현재 무공 리스트
            BasicSkillInventory skillInventory = null;

            //저장된 활성화 무공 데이터
            int subSkillSaveId = 0;
            //현재 활성화 무공 데이터
            SubSkillData subSkillData = null;
            
            switch (skillType)
            {
                case SkillType.Nomal:
                    saveSkillList = skillSaveData.nomalSkillIdList;
                    skillInventory = CD.skillinventory.Nomal;
                    subSkillSaveId = skillSaveData.subSkillNomalId;
                    subSkillData = player.Nomal;
                    break;
                case SkillType.Speed:
                    saveSkillList = skillSaveData.speedSkillIdList;
                    skillInventory = CD.skillinventory.Speed;
                    subSkillSaveId = skillSaveData.subSkillSpeedId;
                    subSkillData = player.Speed;
                    break;
                case SkillType.Special:
                    saveSkillList = skillSaveData.speedSkillIdList;
                    skillInventory = CD.skillinventory.Special;
                    subSkillSaveId = skillSaveData.subSkillSpecialId;
                    subSkillData = player.Special;
                    break;
            }

            foreach (var itemId in saveSkillList)
            {
                var skill = DataManager.i.skillTable.skillInfos[itemId];
                var selectskill = skillInventory.AddReceive(skill);

                if (subSkillSaveId != 0 && itemId == selectskill.id)
                {
                    subSkillData.Set(selectskill, true);
                }
            }
        }

        public void FieldBossCntReset()
        {
            FieldBossData data = null;
            if (fieldBossDataDic.TryGetValue(activeScene, out data))
            {
                data.currnetFieldBossCount = 0;
            }
        }

        public void FieldBossCntAdd(int addValue)
        {
            FieldBossData data = null;
            if (fieldBossDataDic.TryGetValue(activeScene, out data))
            {
                int currentValue = Mathf.Min(data.currnetFieldBossCount + addValue, data.maxFieldBossCount);
                data.currnetFieldBossCount = currentValue;
            }

            if (fieldBossCntChange != null)
            {
                fieldBossCntChange(data);
            }
        }

        public FieldBossData GetFieldBossData()
        {
            if (fieldBossDataDic.ContainsKey(activeScene))
            {
                return fieldBossDataDic[activeScene];
            }
            else if (fieldBossDataDic.ContainsKey(beforeScene))
            {
                return fieldBossDataDic[beforeScene];
            }

            return null;
        }
    }

    [System.Serializable]
    public class CharacterData {
        public Inventory inventory = new Inventory();
        public ActiveSkillInventory skillinventory = new ActiveSkillInventory();
        int _Cash;
        public int Cash {
            get {
                return _Cash;
            }
            set {
                _Cash = value;
                if (CashText != null)
                    CashText.text = _Cash.ToString();
            }
        }
        public Text CashText;
        int _Gold;
        public int Gold
        {
            get
            {
                return _Gold;
            }
            set
            {
                _Gold = value;
             //   Debug.LogError(_Gold);
               if (GoldText != null)
                    GoldText.text = _Gold.ToString();
            }
        }
     
        public Text GoldText;
    }
   

}
