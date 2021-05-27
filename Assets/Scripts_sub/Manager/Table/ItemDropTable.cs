using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HSMLibrary.Generics;

public class ItemDropTable : Singleton<ItemDropTable>
{
    private Dictionary<int, List<ItemDropData>> itemDropDataDic = null;

    public ItemDropTable()
    {
        itemDropDataDic = new Dictionary<int, List<ItemDropData>>();
        itemDropDataDic.Clear();
    }


}
