using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct ItemRandom
{
    [Range(0, 100)]
    public int chance;
    public ItemSO item;

    public int minAmount;
    public int lowAmount;
    public int highAmount;
}

public class RandomLoot : Loot
{
    [SerializeField] private List<ItemRandom> itemSpawn;

    private void Start()
    {
        items = new();
        foreach (var item in itemSpawn)
        {
            var itemAmount = item.minAmount;

            if (Random.value * 100 > item.chance)
            {
                if (itemAmount != 0)
                    items.Add(new InventorySlotDO(item.item, itemAmount));
                continue;
            }

            // Randomize the amount of items
            itemAmount += Random.Range(item.lowAmount, item.highAmount + 1);
            items.Add(new InventorySlotDO(item.item, itemAmount));
        }
    }
}