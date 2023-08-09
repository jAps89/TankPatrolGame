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

using Assets.Scripts.Player;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemContainer : MonoBehaviour
{
    private Item _item;
    public Item Item { get => _item; set => _item = value; }

    [SerializeField] private SpriteRenderer _iconSpirteRenderer;
    [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;

    [SerializeField] private Color _weaponColor;
    [SerializeField] private Color _overdriveColor;
    [SerializeField] private Color _allColor;

    [SerializeField] private Color _commonColor;
    [SerializeField] private Color _rareColor;
    [SerializeField] private Color _epicColor;
    [SerializeField] private Color _legendaryColor;

    public void SetupItem(Item item)
    {
        _item = item;

        if (_iconSpirteRenderer != null) _iconSpirteRenderer.sprite = _item.ItemSprite;
        if (_iconSpirteRenderer!= null) _iconSpirteRenderer.color = ItemColor(_item.ItemType);
        if (_backgroundSpriteRenderer != null) _backgroundSpriteRenderer.color = BackgroundColor(_item.ItemRarity);
    }

    private Color ItemColor(eItemType itemType)
    {
        switch (itemType)
        {           
            case eItemType.WEAPON:
                return _weaponColor;
            case eItemType.CHIP:
                return _overdriveColor;
            default:
                return _allColor;
        }
    }

    private Color BackgroundColor(eItemRarity itemRarity)
    {
        switch (itemRarity)
        {
            case eItemRarity.Common:
                return _commonColor;
            case eItemRarity.Rare:
                return _rareColor;
            case eItemRarity.Epic:
                return _epicColor;
            case eItemRarity.Legendary:
                return _legendaryColor;
            default:
                return _commonColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)    {
        if (_item == null) return;
        PlayerController2 pContrl = collision.gameObject.GetComponent<PlayerController2>();
        if (pContrl != null)
            if(RefLib.sInventory.PickupItem(_item, _item.ItemSprite, ItemColor(_item.ItemType), _backgroundSpriteRenderer.sprite, _backgroundSpriteRenderer.color, _item.ItemType))
                Destroy(gameObject);
        //if usable > pContrl.stats.Changestats()
    }
}

public enum eItemType
{
    ALL,
    WEAPON,
    CHIP,
    PICKUP
}
