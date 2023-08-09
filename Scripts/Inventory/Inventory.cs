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

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] int _inventorySize;
    public int InvSize { get; private set; }
    private int _wpnCount;
    private int _chipCount;

    WeaponManager _wpnManager;
    OverdriveManager _overdriveManager;
    [SerializeField] GameObject _canvasPfab;
    [SerializeField] Transform _canvas;
    Canvas _canvasComp;

    [SerializeField] GameObject _itemDDpFab;
    [SerializeField] ItemSlot[] _itemSlots;
    [SerializeField] bool _isActive = false;

    public ItemSlot[] ItemSlots { get { return _itemSlots; } }


    private void Awake()
    {
        _isMenu = false;

        if (!_isActive) return;

        RefLib.sInventory = this;

        _wpnManager = GetComponent<WeaponManager>();
        _overdriveManager = GetComponent<OverdriveManager>();
        _wpnCount = 2;//_wpnManager.WeaponsSlots.Length;
        _chipCount = 1; //_overdriveManager.OverdriveSlots.Length;
        InvSize = _inventorySize;

        if (_canvas == null)
        {
            SetupCanvas();
        }
        if (_itemSlots.Length==0) SetupSlots();
        _canvasComp = _canvas.GetComponent<Canvas>();

        if (_itemSlots.Length != InvSize + _wpnCount + _chipCount) print("wrong invSize...guess");
        //_itemSlots = new ItemSlot[InvSize + _wpnCount + _ovdrCount];
        //for(int i = 0; i < _itemSlots.Length; i++)
        //{
        //    _itemSlots[i] = new ItemSlot();
        //}
        if (_itemSlots.Length==0) return;
        InitSlots(eItemType.ALL, InvSize);
        InitSlots(eItemType.WEAPON, _wpnCount);
        InitSlots(eItemType.CHIP, _chipCount);

        AddSlotImages();
        SwitchShowInv();
    }

    static public bool _isMenu = false;
    bool _isOpen = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) _isMenu = !_isMenu;
        if (Input.GetKeyDown(KeyCode.Tab) && !_isMenu)
        {
            _isOpen = !_isOpen;
            if (_isOpen == true) Time.timeScale = 0f; 
            else Time.timeScale = 1f;
            SwitchShowInv();
        }
        if (_isOpen && _isMenu)
        {
            _isOpen = !_isOpen;
            SwitchShowInv();
        }

    }
    List<Image> _allImages = new List<Image>();
    bool _showAllImages = true;

    void AddSlotImages()
    {
        foreach(Image image in _canvas.GetComponentsInChildren<Image>())
            _allImages.Add(image);

        //foreach (var item in _itemSlots)
        //    _allImages.Add(item.gameObject.GetComponent<Image>());
        //_allImages.Add(_canvas.GetChild(0).gameObject.GetComponent<Image>());
    }
    void SwitchShowInv()
    {
        foreach(Image image in _allImages)
            if(image!=null) image.enabled = !_showAllImages;
        _showAllImages = !_showAllImages;

        for (int k = 0; k < 2; k++)
            _canvas.GetChild(k).gameObject.SetActive(_showAllImages);
    }

    void SetupCanvas()
    {
        GameObject canvas = Instantiate(_canvasPfab);
        canvas.transform.SetParent(null);
        _canvas = canvas.transform;
    }
    void SetupSlots()
    {
        int count = 0;
        _itemSlots = new ItemSlot[_inventorySize + _wpnCount + _chipCount];
        foreach(Transform child in _canvas.transform)
        {
            if (count >= _inventorySize + _wpnCount + _chipCount) return;
            ItemSlot slot = child.GetComponent<ItemSlot>();
            if(slot!=null)
                _itemSlots[count++] = slot;
        }
    }

    [SerializeField] bool isColorize;
    int slotIndexCounter = 0;
    void InitSlots(eItemType type, int slotNum)
    {
        for (int i = 0; i < slotNum; i++) 
        {
            _itemSlots[slotIndexCounter].SlotData = 
                new InvSlot() { SlotIndex = slotIndexCounter, SlotType = type };

            if(isColorize)
            {
                _itemSlots[slotIndexCounter].gameObject.transform.GetChild(1).GetComponent<Image>().color = TypeColor(type);
                _itemSlots[slotIndexCounter++].gameObject.name = type.ToString() + "slot" + slotIndexCounter.ToString();
            }
        }
    }

    Color TypeColor(eItemType type)
    {
        switch (type)
        {
            case eItemType.ALL:
                return Color.white;
            case eItemType.WEAPON:
                return Color.red;
            case eItemType.CHIP:
                return Color.blue;
            default:
                return Color.black;
        }
    }

    public bool PickupItem(Item item, Sprite sprite, Color color, Sprite sprite1, Color color1, eItemType type)
    {
        int slot = 0;

        if(type == eItemType.WEAPON)
            for (int i = 0; i < _wpnCount; i++)
                if (_itemSlots[_inventorySize + i].ItemDD == null)
                    slot = _inventorySize + i;

        else if(type == eItemType.CHIP)
            for (int k = 0; k < _chipCount; k++)
                if (_itemSlots[_inventorySize + _wpnCount + k].ItemDD == null)
                    slot = _inventorySize + _wpnCount + k;

        if(slot == 0)
            while (_itemSlots[slot].ItemDD != null)
            {
                slot++;
                if (slot >= InvSize)
                {
                    //InventoryFull();
                    return false;
                }
            }

        GameObject newItemDD = Instantiate(_itemDDpFab);
        newItemDD.transform.SetParent(_canvas);

        ItemDragDrop itemDD = newItemDD.GetComponent<ItemDragDrop>();

        itemDD.Item = item;
        itemDD.Image.sprite = sprite;
        itemDD.Image.color = color;

        itemDD.Image1.sprite = sprite1;
        itemDD.Image1.color = color1;

        itemDD.Canvas = _canvasComp;
        itemDD._currentSlot = _itemSlots[slot].SlotData;
        itemDD.RectTransform.anchoredPosition = 
            _itemSlots[slot].RectTransform.anchoredPosition;
        itemDD.RectTransform.localScale = new Vector3(1, 1, 1);
        itemDD.RectTransform.transform.localPosition -= 
            new Vector3(0, 0, itemDD.RectTransform.transform.localPosition.z);
        itemDD._itemType = type;
        _itemSlots[slot].ItemDD = itemDD;

        foreach(Image img in newItemDD.GetComponentsInChildren<Image>())
        {
            _allImages.Add(img);
            img.enabled = _showAllImages;
        }

        if (_itemSlots[slot].SlotData.SlotType != eItemType.ALL)
            EquipItem(itemDD, slot);
        return true;
    }

    public bool MoveItem(InvSlot slotFrom, InvSlot slotTo)
    {
        ItemDragDrop itemDDfrom = _itemSlots[slotFrom.SlotIndex].ItemDD;
        if (itemDDfrom == null || (itemDDfrom._itemType != slotTo.SlotType 
            && slotTo.SlotType != eItemType.ALL)) 
            return false;

        ItemDragDrop itemDDto = _itemSlots[slotTo.SlotIndex].ItemDD;
        if(itemDDto != null)
            if (itemDDto._itemType != slotFrom.SlotType && slotFrom.SlotType != eItemType.ALL)
                return false;

        itemDDfrom._currentSlot = slotTo;
        if(itemDDto!=null)
        {
            itemDDto._currentSlot = slotFrom;
            itemDDto.RectTransform.anchoredPosition = 
                ItemSlots[slotFrom.SlotIndex].RectTransform.anchoredPosition;
        }

        _itemSlots[slotFrom.SlotIndex].ItemDD = itemDDto;
        _itemSlots[slotTo.SlotIndex].ItemDD = itemDDfrom;


        if(slotTo.SlotType != eItemType.ALL)
            if(!EquipItem(itemDDfrom, slotTo.SlotIndex))
                return false;
        if (itemDDto != null)
            if (slotFrom.SlotType != eItemType.ALL)
                if(!EquipItem(itemDDto, slotFrom.SlotIndex))
                    return false;
        if (itemDDto == null) UnequipSlot(slotFrom);

        return true;
    }

    public bool EquipItem(ItemDragDrop itemDD, int slot)
    {
        switch (itemDD._itemType)
        {
            case eItemType.WEAPON:
                Weapon wpn = itemDD.Item as Weapon;
                if (wpn == null) return false;
                if (!_wpnManager.EquipWeapon(wpn, slot - InvSize)) 
                    return false;
                return true;

            case eItemType.CHIP:
                OverdriveChip chip = itemDD.Item as OverdriveChip;
                if (chip == null) return false;
                if (!_overdriveManager.EquipOverdriveChip(chip, slot - InvSize - _wpnCount)) 
                    return false;
                return true;
            default:
                return false;
        }
    }
    public bool UnequipSlot(InvSlot itemSl)
    {
        switch (itemSl.SlotType)
        {
            case eItemType.WEAPON:
                if (!_wpnManager.UnequipWeapon(itemSl.SlotIndex - InvSize)) 
                    return false;
                return true;

            case eItemType.CHIP:
                if (!_overdriveManager.UnequipOverdriveChip(itemSl.SlotIndex - InvSize - _wpnCount)) 
                    return false;
                return true;
            default:
                return false;
        }
    }
    public bool DiscardItem(InvSlot slot)
    {
        switch (slot.SlotType)
        {
            case eItemType.ALL:
                break;

            case eItemType.WEAPON:
                _wpnManager.WeaponsSlots[slot.SlotIndex - InvSize] = null;
                break;

            case eItemType.CHIP:
                _overdriveManager.OverdriveSlots[slot.SlotIndex - InvSize- _wpnCount] = null;
                break;

            default:
                return false;

        }
        ItemDragDrop item = _itemSlots[slot.SlotIndex].ItemDD;
        _allImages.Remove(item.gameObject.GetComponent<Image>());
        _itemSlots[slot.SlotIndex].ItemDD = null;
        Destroy(item);
        return true;
    }
}

