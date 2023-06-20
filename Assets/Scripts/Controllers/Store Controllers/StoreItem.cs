using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class StoreItem : MonoBehaviour
{
    [SerializeField] public Image itemIcon;
    [SerializeField] public TMP_Text itemTitle;
    [SerializeField] public TMP_Text itemPrice;

    public StoreItemInfo itemInfo;

    public void InitButton(StoreItemInfo itemInfo)
    {
        this.itemInfo = itemInfo;
        this.itemTitle.text = itemInfo.title;
        this.itemPrice.text = itemInfo.price + "";
        StoreUIController.Instance.RequestItemIcon(itemInfo.id, itemInfo.icon, (iconImg) => SetIcon(iconImg));
    }

    public void SetIcon(Sprite icon)
    {
        this.itemIcon.sprite = icon;
    }


    public void OnClick_ShowItemDetail()
    {
        PopupController.Instance.OnObjectClicked(this.GetComponent<Selectable>());
        StoreUIController.Instance.ShowItemDetail(this);
    }
}
