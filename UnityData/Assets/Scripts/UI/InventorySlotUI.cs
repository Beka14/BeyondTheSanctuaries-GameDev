using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] InventoryUI inventoryUI;

    public bool IsEmpty => icon.sprite == null;

    public void SetParent(InventoryUI ui)
    {
        inventoryUI = ui;
    }
    public void SetItem(ItemSO item = null, int amount = -1)
    {
        if(item == null)
        {
            icon.enabled = false;
            icon.sprite = null;
            amountText.enabled = false;
        }
        else
        {
            icon.enabled = true;
            icon.sprite = item.itemSprite;
            amountText.enabled = true;
            amountText.text = amount.ToString();
        }
    }
    public void SetItemAmount(int amount)
    {
        if(IsEmpty)
            return;

        if (amount <= 0)
        {
            icon.color = Color.clear;
            amountText.enabled = false;
        }
        else
        {
            amountText.enabled = true;
            amountText.text = amount.ToString();
        }
    }

    public void OnClicked()
    {
        OnSlotEvent(EventType.MouseDown);
    }

    public void OnSlotEvent(EventType type)
    {
        inventoryUI.OnSlotEventProxy(this, type);
    }
}