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

public class InvBackground : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] Inventory _inventory;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData != null)
        {
            ItemDragDrop item = eventData.pointerDrag.GetComponent<ItemDragDrop>();
            if (item != null)
            {
                //item.IsDropSuccessful = true;
                //_inventory.DiscardItem(item._currentSlot);
            }
        }
    }
}
