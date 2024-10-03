using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting Recipe", menuName = "Item/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<ItemAmount> ingredients;
    public ItemAmount result;
}