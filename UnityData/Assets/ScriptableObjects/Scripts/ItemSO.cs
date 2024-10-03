using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item", order = 0)]
public class ItemSO : ScriptableObject
{
    public Guid id = Guid.NewGuid();
    public string itemName;
    public Sprite itemSprite;
    public int maxStack;

    public bool IsStackable => maxStack > 1;
}

public class UsableItemSO : ItemSO
{
    public int useAmount = 1;
    public virtual bool Use(PlayerSubsystem playerSubsystem)
    {
        return false;
    }
}