using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace A_Script
{
    public class CharacterUI : InitBase 
    {
        CharacterCreateUI ccu;
        CharacterSelectUI csu;
        private void Start()
        {
            localinit();
        }
        public override void initactive()
        {
            ccu = GetInstantiate("CharacterCreate").GetComponent<CharacterCreateUI>();
            csu = GetInstantiate("CharacterSelect").GetComponent<CharacterSelectUI>();
            ccu.Init(this);
            csu.Init(this);

            ccu.gameObject.SetActive(false);
        }

        GameObject GetInstantiate(string name)
        {
            string path = string.Format("UI/CharacterCreation/{0}", name); 
            return Instantiate(Resources.Load(path) as GameObject, transform);
        }

        public void ShowCreate()
        {
            ccu.gameObject.SetActive(true);
            csu.gameObject.SetActive(false);
            ccu.Show();
        }
        public void CreateClose()
        {
            ccu.gameObject.SetActive(false);
            csu.gameObject.SetActive(true);
            csu.CreateBack();

        }

        public void GameStart(CharacterSelectData SelectCharacter) {
            if (SelectCharacter != null) {
                //load proto
                GameManager.i.Init(SelectCharacter);
                string firstSceneName = DataManager.i.mapTable.mapDataList[0].sceneName;
                GameManager.i.LoadScene(firstSceneName);
            }
        }
        public void Created(CharacterSelectData csd) {
            csu.Created(csd.Copy());
            ccu.gameObject.SetActive(false);
            csu.gameObject.SetActive(true);
        }
    }

    public class CharacterSelectData
    {
        public string nickName;
        public int key;
        public bool isit = false;
        public PlayerData currentData = new PlayerData();
        public int Jap = 1;
        public int Gender = 0;
        public int WeaponType = 0;
        public int rebirthID = 0;
        [SerializeField]
        public List<int> rebirthWeaponList = new List<int>();

        public CharacterSelectData(int _key)
        {
            key = _key;
        }
        public CharacterSelectData()
        {

        }
        
        public CharacterSelectData Copy()
        {
            CharacterSelectData temp = new CharacterSelectData();
            temp.nickName = nickName;
            temp.key = key;
            temp.isit = isit;
            temp.currentData.lv = currentData.lv;
            temp.Jap = Jap;
            temp.Gender = Gender;
            temp.WeaponType = WeaponType;
            return temp;

        }
    }
}
