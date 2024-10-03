using System;
using UnityEngine;

public class CraftSlotUI : MonoBehaviour
{
    public event Action<CraftingRecipe> OnSlotClicked;

    [SerializeField] InventorySlotUI result;
    [SerializeField] GameObject slotPrefab;

    CraftingRecipe recipe;

    public void SetRecipe(CraftingRecipe recipe)
    {
        result.SetItem(recipe.result.item, recipe.result.amount);
        for (int i = 0; i < recipe.ingredients.Count; i++)
        {
            var slot = Instantiate(slotPrefab, transform).GetComponent<InventorySlotUI>();
            slot.SetItem(recipe.ingredients[i].item, recipe.ingredients[i].amount);
        }
        this.recipe = recipe;
    }

    public void OnClicked()
    {
        OnSlotClicked?.Invoke(recipe);
    }
}
