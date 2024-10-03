using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public int X_START;
    public int Y_START;
    public int NUMBER_OF_COLUMNS;

        
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory container;

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath(
            "Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
#else
        database = Resources.Load<ItemDatabaseObject>("Database");
#endif
    }

    private InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        foreach (var t in container.items)
        {
            if (t.item.id <= -1)
            {
                t.UpdateSlot(_item, _amount);
                return t;
            }
        }
        return null;
    }


    public bool AddItem(ItemObject _item, int _amount)
    {
        var item = _item.CreateItem();
        if (_item.stackable)
        {
            var slots = FindItemsOnInventory(item);
            foreach (var slot in slots)
            {
                if (_amount <= 0) break;

                int availableSpace = _item.stackSize - slot.amount;
                if (availableSpace > 0)
                {
                    int amountToAdd = Mathf.Min(_amount, availableSpace);
                    slot.AddAmount(amountToAdd);
                    _amount -= amountToAdd;
                }
            }

            // If there is still some amount left to add and we have stackable items, try to place it in empty slots
            while (_amount > 0)
            {
                var amountToAdd = Mathf.Min(_amount, _item.stackSize);
                var newSlot = SetEmptySlot(item, amountToAdd);
                if (newSlot == null)
                {
                    // If we couldn't find an empty slot, return false indicating not all items were added
                    return false;
                }
                _amount -= amountToAdd;
            }

            return true;
        }

        // If item is not stackable, try to place each item in an empty slot
        for (int i = 0; i < _amount; i++)
        {
            if (SetEmptySlot(item, 1) == null)
            {
                // If we couldn't find an empty slot for each item, return false
                return false;
            }
        }

        return true;
    }

    public void RemoveItem(Item item, int amount)
    {
        var slot = FindItemOnInventory(item);
        if (slot == null) return;
        slot.amount -= amount;
        if (slot.amount <= 0)
        {
            slot.RemoveItem();
        }
    }
    
    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        var temp = new InventorySlot(item1.item, item1.amount);
        item1.UpdateSlot(item2.item, item2.amount);
        item2.UpdateSlot(temp.item, temp.amount);
    }

    public InventorySlot FindItemOnInventory(Item _item)
    {
        for (int i = container.items.Length - 1; i > -1; i--)
        {
            if (container.items[i].item.id == _item.id)
            {
                return container.items[i];
            }
        }
        return null;
    }

    public bool FindItemOnInventoryBool(Item _item)
    {
        for (int i = container.items.Length - 1; i > -1; i--)
        {
            if (container.items[i].item.id == _item.id)
            {
                return true;
            }
        }
        return false;
    }

    public List<InventorySlot> FindItemsOnInventory(Item _item)
    {
        List<InventorySlot> foundItems = new List<InventorySlot>();

        foreach (var slot in container.items)
        {
            if (slot.item.id == _item.id)
            {
                foundItems.Add(slot);
            }
        }

        return foundItems;
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        var bf = new BinaryFormatter();
        var fileStream = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(fileStream, saveData);
        fileStream.Close();
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            var bf = new BinaryFormatter();
            var fileStream = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(fileStream).ToString(), this);
            fileStream.Close();
        }
    }
    
    public void Clear()
    {
        container.Clear();
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] items = new InventorySlot[24];
    
    public void Clear()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].RemoveItem();
        }
    }
    
    public int GetEmptySlot()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item.id <= -1)
            {
                return i;
            }
        }
        return -1;
    }

}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;
    
    public InventorySlot()
    {
        item = new Item();
        amount = 0;
    }
    public InventorySlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    public void UpdateSlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    public void RemoveItem()
    {
        item = new Item();
        amount = 0;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}