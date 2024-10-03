using System;
using UnityEngine;


[Serializable]
public struct ItemAmount
{
    public ItemSO item;
    public int amount;

    public ItemAmount(ItemSO item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}

[Serializable]
public class InventorySlotDO 
{
    [SerializeField] private ItemSO item;
    [SerializeField] private int amount;

    public ItemSO Item => item;
    public int Amount => amount;
    public bool IsEmpty => item == null || amount <= 0;
    public bool IsFull => item != null && amount == item.maxStack;

    public Action<int> OnSlotChanged;

    public InventorySlotDO(ItemSO item = null, int amount = -1)
    {
        CreateSlot(item, amount);
    }

    /// <summary>
    /// Add an amount of items to the slot
    /// </summary>
    /// <param name="amount">Amount to add</param>
    /// <returns>Overflow amount, if more then max amount</returns>
    public int AddAmount(int amount)
    {
        this.amount = System.Math.Min(this.amount + amount, item.maxStack);
        return System.Math.Max(this.amount + amount - item.maxStack, 0);
    }

    /// <summary>
    /// Remove an amount of items from the slot
    /// Does not check if amount is less then 0
    /// </summary>
    /// <param name="amount"> Amount to remove </param>
    public void RemoveAmount(int amount) {
        this.amount -= amount;
        if (this.amount == 0)
            ClearSlot();
    }

    /// <summary>
    /// Clears the slot
    /// </summary>
    public void ClearSlot()
    {
        CreateSlot(null, -1);
    }

    /// <summary>
    /// Create a slot with an item and amount
    /// </summary>
    /// <param name="item"> Item to add to the slot </param>
    /// <param name="amount"> Amount of the item </param>
    public void CreateSlot(ItemSO item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}
