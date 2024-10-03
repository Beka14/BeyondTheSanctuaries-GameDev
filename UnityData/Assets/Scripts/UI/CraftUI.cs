using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftUI : UISubsystem
{
    [Header("Craft UI")]
    [SerializeField] private GameObject recipePrefab;
    [SerializeField] private Transform slotAnchor;

    InventoryComponent inventoryComponent;
    GameObject table;

    public override void Bind(PlayerSubsystem playerSubsystem)
    {
        inventoryComponent = playerSubsystem.GetComponent<InventoryComponent>();
    }


    public override void Show()
    {
        base.Show();
        var inv = UIManager.GetUI<InventoryUI>();
        if(inv != null)
        {
            inv.gameObject.SetActive(true);
            inv.DisableClose();
        }
    }

    public override void Close()
    {
        base.Close();
        var inv = UIManager.GetUI<InventoryUI>();
        if (inv != null)
        {
            inv.gameObject.SetActive(false);
            inv.EnableClose();
        }
    }

    public void SetRecipes(List<CraftingRecipe> recipes)
    {
        foreach (var recipe in recipes)
        {
            var recipeUI = Instantiate(recipePrefab, slotAnchor).GetComponent<CraftSlotUI>();
            recipeUI.SetRecipe(recipe);
            recipeUI.OnSlotClicked += OnSlotClicked;
        }
    }

    public bool OrphanAll(GameObject table)
    {
        if (this.table != table)
        {
            foreach (Transform child in slotAnchor)
                Destroy(child.gameObject);
            this.table = table;
            return true;
        }
        return false;
    }

    private void OnSlotClicked(CraftingRecipe slot)
    {
        var inv_backend = inventoryComponent.InventorySystem;
        bool success = true;
        foreach (var ingredient in slot.ingredients)
        {
            var count = inv_backend.GetItemCount(ingredient.item);
            if (count < ingredient.amount)
            {
                success = false;
                break;
            }
        }

        if (!success)
            return;

        foreach (var ingredient in slot.ingredients)
            inv_backend.RemoveItem(ingredient.item, ingredient.amount);

        inv_backend.AddItem(slot.result.item, slot.result.amount, out _);
    }
}