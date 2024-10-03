using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : UISubsystem
{
    GameObject[] inventorySpace;
    InventoryComponent inventoryComponent;
    PlayerSubsystem playerSubsystem;
    Dictionary<InventorySlotUI, InventorySlotDO> itemsDisplayed;

    [Header("Inventory UI")]
    [SerializeField] int itemRowMaxCount = 5;
    [SerializeField] GameObject itemUIPrefab;
    [SerializeField] float spacing = 5;
    [SerializeField] Transform slotAnchor;
    [SerializeField] Button closeButton;

    public event Action OnClose;

    public override void Bind(PlayerSubsystem playerSubsystem)
    {
        this.playerSubsystem = playerSubsystem;
        inventoryComponent = playerSubsystem.gameObject.GetComponent<InventoryComponent>();
        playerSubsystem.Controls.OnInventoryPressed += Toggle;
        CreateSlots();
    }

    private void OnEnable()
    {
        if (inventoryComponent != null)
        {
            inventoryComponent.InventorySystem.OnInventorySlotsChanged += UpdateSlot;
            UpdateInventory();
        }
    }
    private void OnDisable()
    {
        if (inventoryComponent != null)
            inventoryComponent.InventorySystem.OnInventorySlotsChanged -= UpdateSlot;
    }

    private void UpdateSlot(int obj)
    {
        if (obj < 0 || obj >= inventorySpace.Length)
        {
            Debug.LogError("Invalid slot index");
            return;
        }

        var inv_backend = inventoryComponent.InventorySystem.Inventory;
        var slot = inventorySpace[obj].GetComponent<InventorySlotUI>();

        slot.SetItem(inv_backend[obj].Item, inv_backend[obj].Amount);
    }

    private void UpdateInventory()
    {
        var inv_backend = inventoryComponent.InventorySystem.Inventory;
        for (int i = 0; i < inventorySpace.Length; i++)
        {
            var slot = inventorySpace[i].GetComponent<InventorySlotUI>();
            if (i < inv_backend.Count)
            {
                slot.SetItem(inv_backend[i].Item, inv_backend[i].Amount);
            }
            else
            {
                slot.SetItem();
            }
        }
    }

    private void CreateSlots()
    {
        if (inventoryComponent == null)
        {
            Debug.LogError("InventoryComponent is null");
            return;
        }

        var size = inventoryComponent.InventorySystem.InventorySize;
        itemsDisplayed = new Dictionary<InventorySlotUI, InventorySlotDO>(size);
        inventorySpace = new GameObject[size];
        Vector3 delta_position = Vector3.zero;
        var rect = itemUIPrefab.GetComponent<RectTransform>().rect;

        for (int i = 0; i < inventorySpace.Length; i++)
        {
            if (i % itemRowMaxCount == 0 && i != 0)
            {
                delta_position.x = 0;
                delta_position.y -= rect.height + spacing;
            }
            else if (i != 0)
            {
                delta_position.x += rect.width + spacing;
            }

            var instance = Instantiate(itemUIPrefab, slotAnchor);
            instance.transform.localPosition = delta_position;
            var slot = instance.GetComponent<InventorySlotUI>();
            slot.SetItem(); // Clear inventory slot
            slot.SetParent(this);
            inventorySpace[i] = instance;
            itemsDisplayed[slot] = inventoryComponent.InventorySystem.Inventory[i];
        }
    }

    public void OnSlotEventProxy(InventorySlotUI slot, EventType type)
    {
        if (slot.IsEmpty)
            return;

        if (type == EventType.MouseDown)
        {
            var item = itemsDisplayed[slot];
            if (item.Item is UsableItemSO usable && inventoryComponent.InventorySystem.GetItemCount(usable) >= usable.useAmount)
            {
                if (usable.Use(playerSubsystem))
                {
                    inventoryComponent.InventorySystem.RemoveItem(usable, usable.useAmount);
                }
            }
        }
    }

    public void DisableClose()
    {
        closeButton.interactable = false;
    }

    public void EnableClose()
    {
        closeButton.interactable = true;
    }
}