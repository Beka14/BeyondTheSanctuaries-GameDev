using UnityEngine;

public enum ItemType
{
    Default,
    Food,
    Health,
    Weapon,
    Crafting,
    AmericanRifleAmmo
}

public abstract class ItemObject : ScriptableObject
{
    public int id;
    public Sprite uiDisplay;
    public ItemType type;
    public bool stackable;
    public int stackSize;
    public Item data = new Item();
    [TextArea(15, 20)] public string description;
    
    public Item CreateItem()
    {
        var newItem = new Item(this);
        return newItem;
    }
    
    public void Awake()
    {
        data = new Item(this);
    }
}

[System.Serializable]
public class Item
{
    public string name;
    public int id;

    public Item(ItemObject _item)
    {
        name = _item.name;
        id = _item.id;
    }
    
    public Item()
    {
        name = "";
        id = -1;
    }
}