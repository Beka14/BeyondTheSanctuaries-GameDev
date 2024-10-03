using System.Collections.Generic;
using UnityEngine;

public class Loot : Interactable
{
    [SerializeField] protected List<InventorySlotDO> items;

    public override string GetInteractText()
    {
        return "Pick up";
    }

    public override void Interact(PlayerSubsystem player)
    {
        if (!player.TryGetComponent<InventoryComponent>(out var playerInventory))
        {
            Debug.LogError("Player does not have an InventoryComponent");
            return;
        }

        // Transfer all items from this inventory to the player's inventory
        foreach (var item in items)
        {
            if (playerInventory.InventorySystem.AddItem(item.Item, item.Amount, out _))
            {
                item.RemoveAmount(item.Amount);
            }
        }
        items.RemoveAll(item => item.Item == null);
        if (items.Count == 0)
        {
            UIManager.GetUI<InteractableUI>()?.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public override void StopInteract(PlayerSubsystem player)
    {
        // Do nothing
    }
}
