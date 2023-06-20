using DynamicScroll;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DynamicShelf : DynamicScrollObject<StoreShelfData>
{
    public override float CurrentHeight { get; set; }
    public override float CurrentWidth { get; set; }

    public void Awake()
    {
        CurrentHeight = GetComponent<RectTransform>().rect.height;
        CurrentWidth = GetComponent<RectTransform>().rect.width;
    }

    [SerializeField] private List<StoreItem> itemDisplayers;

    public override void UpdateScrollObject(StoreShelfData shelfData, int index)
    {
        base.UpdateScrollObject(shelfData, index);

        for (int i = 0; i < shelfData.itemsInfo.Count; i++)
        {
            itemDisplayers[i].InitButton(shelfData.itemsInfo[i]);
            itemDisplayers[i].gameObject.SetActive(true);
        }

        for (int i = shelfData.itemsInfo.Count; i < itemDisplayers.Count; i++)
        {
            itemDisplayers[i].gameObject.SetActive(false);
        }
    }
}
