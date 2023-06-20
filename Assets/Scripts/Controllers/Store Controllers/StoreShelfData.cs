using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreShelfData 
{
    public List<StoreItemInfo> itemsInfo = new();
    
    public StoreShelfData(List<StoreItemInfo> itemList)
    {
        this.itemsInfo = itemList;
    }
}
