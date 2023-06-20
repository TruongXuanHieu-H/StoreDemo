using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class PopupController : MonoBehaviour
{
    private static PopupController instance;
    public static PopupController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null || instance == this) 
        {

        }
        else
        {
            Destroy(instance);
        }
        instance = this;
    }

    Action<bool, List<Selectable>> setSelectableStatus = (status, selectableElements) =>
    {
        foreach (Selectable selectable in selectableElements)
        {
            if (selectable != null)
            {
                selectable.interactable = status;
            }
        }
    };

    #region Clicked
    public void OnObjectClicked(Selectable clickedObj)
    {
        StartCoroutine(PlayClickedAnim(clickedObj));
    }
    private IEnumerator PlayClickedAnim(Selectable clickedObj)
    {
        clickedObj.interactable = false;
        RectTransform clickedObjRect = clickedObj.GetComponent<RectTransform>();
        clickedObjRect.DOScale(0.9f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        clickedObjRect.DOScale(1.1f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        clickedObjRect.DOScale(1f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        clickedObj.interactable = true;
    }
    #endregion
    #region Popup
    public void ShowPanel(GameObject firstlyShow, RectTransform overTimeShow, List<Selectable> selectableElements)
    {

        setSelectableStatus.Invoke(false, selectableElements);

        StartCoroutine(PopIn(overTimeShow, setSelectableStatus, selectableElements, firstlyShow));
    }

    public void HidePanel(GameObject lastestHide, RectTransform overTimeHide, List<Selectable> selectableElements)
    {
        setSelectableStatus.Invoke(false, selectableElements);

        StartCoroutine(PopOut(overTimeHide, setSelectableStatus, selectableElements, lastestHide));
    }

    private IEnumerator PopIn(RectTransform popupObj, Action<bool, List<Selectable>> setSelectableStatus, List<Selectable> selectableElements, GameObject firstShow)
    {
        firstShow.SetActive(true);

        popupObj.gameObject.SetActive(true);
        popupObj.localScale = Vector2.zero;
        popupObj.DOScale(1.2f, 0.3f);

        yield return new WaitForSeconds(0.3f);

        popupObj.DOScale(1f, 0.1f);

        yield return new WaitForSeconds(0.1f);

        setSelectableStatus.Invoke(true, selectableElements);
    }

    private IEnumerator PopOut(RectTransform popupObj, Action<bool, List<Selectable>> setSelectableStatus, List<Selectable> selectableElements, GameObject lastestHide)
    {
        popupObj.localScale = Vector2.one;
        popupObj.DOScale(1.2f, 0.1f);
        yield return new WaitForSeconds(0.1f);

        popupObj.DOScale(0, 0.3f);
        yield return new WaitForSeconds(0.3f);

        popupObj.gameObject.SetActive(false);
        setSelectableStatus.Invoke(true, selectableElements);

        lastestHide.SetActive(false);
    }
    #endregion
}
