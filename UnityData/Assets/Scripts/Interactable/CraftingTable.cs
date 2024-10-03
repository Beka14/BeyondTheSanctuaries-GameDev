using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : Interactable
{
    [Header("Crafting Table")]
    [SerializeField] private List<CraftingRecipe> recipes;

    public override string GetInteractText()
    {
        return "Craft";
    }

    public override void Interact(PlayerSubsystem player)
    {
        var ui = UIManager.GetUI<CraftUI>();
        if(ui.OrphanAll(gameObject)) // If the table is different from the last table
            ui.SetRecipes(recipes);
        ui.Show();
    }

    public override void StopInteract(PlayerSubsystem player)
    {

    }
}