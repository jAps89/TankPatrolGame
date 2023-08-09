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

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform _rectTransform;
    public InvSlot SlotData { get; set; }
    public ItemDragDrop ItemDD { get; set; }
    public RectTransform RectTransform { get { return _rectTransform; } }

    //private void Awake()
    //{
    //    Slot = new InvSlot() { SlotIndex = _slotIndex, SlotType = SlotType };
    //}

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData != null)
        {
            ItemDragDrop item = eventData.pointerDrag.GetComponent<ItemDragDrop>();

            if(item != null)
                if(RefLib.sInventory.MoveItem(item._currentSlot, SlotData))
                {
                    item.IsDropSuccessful = true;
                    item.RectTransform.anchoredPosition = _rectTransform.anchoredPosition;
                }
        }
    }

    //public void UpdateInventory(ItemDragDrop item)
    //{
    //    if (RefLib.sInventory.MoveItem2(item._currentSlot, SlotData))
    //        item._currentSlot = SlotData;

    //    else item.ResetPosition();
    //}
}
