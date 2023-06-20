using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using System;
using DynamicScroll;
using UnityEngine.UI;

public class StoreUIController : MonoBehaviour
{
    private static StoreUIController instance;
    public static StoreUIController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null || instance == this) {
            
        } else
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    [SerializeField] private AssetReferenceT<TextAsset> storeItemsInfoAR;
    [SerializeField] private SpriteAtlas[] storeItemAtlases;
    [SerializeField] private List<AssetReferenceAtlasedSprite> storeItemAtlasesAR;
    [SerializeField] private int maxSpritesPerAtlas;


    public DynamicScrollRect shelvesScroll;
    public GameObject shelfPrefap;
    private DynamicScroll<StoreShelfData, DynamicShelf> mVerticalDynamicScroll = new DynamicScroll<StoreShelfData, DynamicShelf>();
    public int numberItemPerShelf;

    // Start is called before the first frame update
    void Start()
    {
        storeItemAtlases = new SpriteAtlas[storeItemAtlasesAR.Count];

        for (int i = 0; i < storeItemAtlasesAR.Count; i++)
        {
            int currentIndex = i;
            storeItemAtlasesAR[currentIndex].LoadAssetAsync<SpriteAtlas>().Completed += (atlasHandler) =>
            {
                if (atlasHandler.Status == AsyncOperationStatus.Succeeded)
                {
                    storeItemAtlases[currentIndex] = atlasHandler.Result;
                    Debug.Log("Store_Items_" + currentIndex + " is loaded.");
                }
                else
                {
                    Debug.LogWarning("Can't load Store_Items_" + currentIndex + ".");
                }
            };
        }

        OnStoreOpened();
    }

    public void RequestItemIcon(int itemId, string itemIconName, Action<Sprite> callback)
    {
        int atlasIndex = (itemId - 1) / maxSpritesPerAtlas;
        StartCoroutine(WaitAtlasLoaded(atlasIndex, itemId, itemIconName, callback));
    }
    
    private IEnumerator WaitAtlasLoaded(int atlasIndex, int itemId, string itemIconName, Action<Sprite> callback)
    {
        yield return new WaitUntil(() => storeItemAtlases[atlasIndex] != null);
        Sprite itemIconSprite = storeItemAtlases[atlasIndex].GetSprite(itemIconName);
        callback.Invoke(itemIconSprite == null ? null : itemIconSprite);
    }

    
    public class StoreItemList
    {
        public StoreItemInfo[] items;
    }
    public void OnStoreOpened()
    {
        storeItemsInfoAR.LoadAssetAsync<TextAsset>().Completed += (textAssetHandler) =>
        {
            if (textAssetHandler.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Store items' info is loaded successfully.");
                StoreItemList itemList = JsonUtility.FromJson<StoreItemList>(textAssetHandler.Result.text);
                InitStoreItems(itemList.items);
            }
            else
            {
                Debug.Log("Cannot load store items' info.");
            }
        };
    }


    private void InitStoreItems(StoreItemInfo[] items)
    {

        int numberShelves = (int)Math.Ceiling((float)items.Length / numberItemPerShelf);
        List<StoreShelfData> storeData = new List<StoreShelfData>();

        for (int i = 0; i < numberShelves; i++)
        {
            List<StoreItemInfo> shelfContentData = new List<StoreItemInfo>();
            for (int j = 0; j < numberItemPerShelf; j++)
            {
                int itemIndex = i * numberItemPerShelf + j;
                if (itemIndex < items.Length)
                {
                    shelfContentData.Add(items[itemIndex]);
                }
            }
            StoreShelfData shelfData = new StoreShelfData(shelfContentData);
            storeData.Add(shelfData);
        }
        mVerticalDynamicScroll.Initiate(shelvesScroll, storeData, 0, shelfPrefap);
    }

    [SerializeField] private GameObject itemDetailUI;
    [SerializeField] private RectTransform itemDetailPanel;
    [SerializeField] private List<Selectable> itemDetailSelectableElements;


    [SerializeField] private ItemDetailPanelController itemDetailPanelController;
    public void ShowItemDetail(StoreItem item)
    {
        itemDetailPanelController.SetupDetailDisplayer(item);
        PopupController.Instance.ShowPanel(itemDetailUI, itemDetailPanel, itemDetailSelectableElements);
    }
    public void HideItemDetail()
    {
        PopupController.Instance.HidePanel(itemDetailUI, itemDetailPanel, itemDetailSelectableElements);
    }
    public void OnClick_HideItemDetail()
    {
        HideItemDetail();
    }
}
