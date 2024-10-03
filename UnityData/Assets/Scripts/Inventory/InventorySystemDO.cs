using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySystemDO
{
    [SerializeField] private List<InventorySlotDO> inventory;
    private Dictionary<Guid, int> inventorySlots; // For faster access

    public Action<int> OnInventorySlotsChanged;

    public List<InventorySlotDO> Inventory => inventory;
    public int InventorySize => inventory.Count;



    public InventorySystemDO(int size)
    {
        inventory = new List<InventorySlotDO>(size);
        inventorySlots = new Dictionary<Guid, int>(size);
        for (int i = 0; i < size; i++)
        {
            inventory.Add(new InventorySlotDO());
        }
    }

    public InventorySlotDO SlotAt(int idx)
    {
        return idx >= 0 && idx < inventory.Count ? inventory[idx] : null;
    }

    public bool ContainsItem(ItemSO item, out InventorySlotDO slot, out int slot_idx)
    {
        slot_idx = -1;
        slot = null;

        if (item == null)
            return false;

        bool contains = inventorySlots.ContainsKey(item.id);
        slot_idx = contains ? inventorySlots[item.id] : -1;
        slot = contains ? inventory[slot_idx] : null;
        return contains;
    }

    public int RemoveItem(ItemSO item, int amount)
    {
        if (ContainsItem(item, out InventorySlotDO slot, out int idx))
        {
            int extracted = Math.Min(amount, slot.Amount);
            slot.RemoveAmount(extracted);
            OnInventorySlotsChanged?.Invoke(idx);
            if(slot.Item == null)
                inventorySlots.Remove(item.id);

            return extracted;
        }
        return 0;
    }

    public bool AddItem(ItemSO item, int amount, out int remaining)
    {
        remaining = amount;
        if (ContainsItem(item, out InventorySlotDO slot, out int idx))
        {
            if (slot.IsFull)
                return false;

            // Wolfenstein logic
            remaining = slot.AddAmount(amount); // Add the amount to the slot, clamp to maxStack and do nothing
            OnInventorySlotsChanged?.Invoke(idx);
            return true;
        }
        else if (FreeSlot(out InventorySlotDO freeSlot, out idx))
        {
            freeSlot.CreateSlot(item, amount);
            inventorySlots.Add(item.id, idx);
            OnInventorySlotsChanged?.Invoke(idx);
            return true;
        }
        return false;
    }

    public bool FreeSlot(out InventorySlotDO slot, out int slot_idx)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].IsEmpty)
            {
                slot = inventory[i];
                slot_idx = i;
                return true;
            }
        }

        slot = null;
        slot_idx = -1;
        return false;
    }

    public int GetItemCount(ItemSO item)
    {
        if(!ContainsItem(item, out InventorySlotDO slot, out int idx))
        {
            return 0;
        }
        return slot.Amount;
    }
}
