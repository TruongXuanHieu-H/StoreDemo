using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailPanelController : MonoBehaviour
{
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected TMP_Text itemTitle;
    [SerializeField] protected TMP_Text itemDescription;


    public void SetupDetailDisplayer(StoreItem storeItem)
    {
        if (storeItem != null) {
            itemIcon.sprite = storeItem.itemIcon.sprite;
            itemTitle.text = storeItem.itemTitle.text;
            itemDescription.text = storeItem.itemInfo.desc;
        }
    }
}
