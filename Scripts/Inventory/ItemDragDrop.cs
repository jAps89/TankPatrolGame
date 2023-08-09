/*****************************************************************************
* Project: CMN5201gpr-0322-Game
* File   : Rotateable.cs
* Date   : 17.04.22
* Author : Jan Apsel (JA)
*
* These coded instructions, statements, and computer programs contain
* proprietary information of the author and are protected by Federal
* copyright law. They may not be disclosed to third parties or copied
* or duplicated in any form, in whole or in part, without the prior
* written consent of the author.
*
* History:
*   25.4.22 JA created 
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Item _item;
    [SerializeField] private Image _image, _image1;

    public CanvasGroup CanvasGroup { get { return _canvasGroup; } }
    public Canvas Canvas { set { _canvas = value; } }
    public Image Image { get { return _image; } set { _image = value; } }
    public Image Image1 { get { return _image1; } set { _image1 = value; } }
    public Item Item { get { return _item; } set { _item = value; } }
    public InvSlot _currentSlot { get; set; }
    public bool IsDropSuccessful { private get; set; }
    public eItemType _itemType { get; set; }
    public RectTransform RectTransform { get { return _rectTransform; } }

    private Vector2 _lastAnchorPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        _lastAnchorPosition = _rectTransform.anchoredPosition;
        //_canvasGroup.blocksRaycasts = false;
        SetItemsBlockRaycast(false);
        IsDropSuccessful = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData) 
    {
        //_canvasGroup.blocksRaycasts = true;
        SetItemsBlockRaycast(true);
        if (!IsDropSuccessful) ResetPosition(); 
    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void ResetPosition()
    {
        _rectTransform.anchoredPosition = _lastAnchorPosition;
    }

    void SetItemsBlockRaycast(bool isBlock)
    {
        foreach(ItemSlot itemSl in RefLib.sInventory.ItemSlots)
        {
            ItemDragDrop itemDD = itemSl.ItemDD;
            if(itemDD != null) itemDD.CanvasGroup.blocksRaycasts = isBlock;

        }
    }
}
public struct InvSlot
{
    public int SlotIndex;
    public eItemType SlotType;
}
