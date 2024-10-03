using UnityEngine;

public class Debris : Interactable
{
    [SerializeField] private ItemSO remover;
    public override string GetInteractText()
    {
        return $"Remove debris ({remover.itemName})";
    }

    public override void Interact(PlayerSubsystem player)
    {
        if (!player.TryGetComponent<InventoryComponent>(out var inventory))
        {
            Debug.LogError("Player does not have an InventoryComponent");
            return;
        }

        if(inventory.InventorySystem.GetItemCount(remover) > 0)
        {
            inventory.InventorySystem.RemoveItem(remover, 1);
            Destroy(gameObject);
            UIManager.GetUI<InteractableUI>()?.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("You don't have the required item to remove this debris");
        }
    }

    public override void StopInteract(PlayerSubsystem player)
    {
        // Do nothing
    }
}
