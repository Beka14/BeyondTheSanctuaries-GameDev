
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    [SerializeField] protected InventorySystemDO inventorySystemDO = null;
    [SerializeField] protected int inventorySize;

    public InventorySystemDO InventorySystem => inventorySystemDO;

    private void Awake()
    {
        inventorySystemDO = new InventorySystemDO(inventorySize);
    }
}