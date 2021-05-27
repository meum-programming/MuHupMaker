using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A_Script
{
    [System.Serializable]
    public class ItemDropGroupInfoTable : DataTable
    {
      public Dictionary<int, List<ItemDropGroupInfoData>> itemdginfo = new Dictionary<int, List<ItemDropGroupInfoData>>();
      //  public DicIDGI itemdginfo = new DicIDGI();
#if UNITY_EDITOR
        public List<ItemDropGroupInfoData> itemdginfoView = new List<ItemDropGroupInfoData>();
#endif
        public ItemDropGroupInfoTable()
        {

            
        }

        public override void Load(List<Dictionary<string, object>> _datas)
        {
            var dataEnumerator = _datas.GetEnumerator();
            int index = 1;
            List<ItemDropGroupInfoData> datas = new List<ItemDropGroupInfoData>();
            while (dataEnumerator.MoveNext())
            {
                var data = dataEnumerator.Current;

                ItemDropGroupInfoData info = new ItemDropGroupInfoData();
             
                info.itemdropgruopid = int.Parse(data["DropGruopItemID"].ToString());
                info.itemid = int.Parse(data["ItemID"].ToString());
               // Debug.LogError(info.itemid);
                info.itemid_rate = int.Parse(data["ItemID_Rate"].ToString());
                
         
#if UNITY_EDITOR
                itemdginfoView.Add(info);
#endif
                datas.Add(info);
                if (index != info.itemdropgruopid)
                {
                    itemdginfo.Add(index, datas);
                    datas = new List<ItemDropGroupInfoData>();
                    index = info.itemdropgruopid;
                }
            }
            itemdginfo.Add(index, datas);
        }
    }

	[System.Serializable]
	public class ItemDropGroupInfoData
	{
		public int index;
		public int itemdropgruopid;
		public int _itemid;
		public int itemid
		{
			get
			{
				return _itemid;
			}
			set
			{
				_itemid = value;
				if (_itemid > 999999)
				{
					droptype = DropType.Item;
				}
				else
				{
					droptype = DropType.Skill;
				}
			}
		}
		public DropType droptype;
		public int itemid_rate;
	}

	[System.Serializable]
	public enum DropType
	{
		Item, Skill
	}
}
