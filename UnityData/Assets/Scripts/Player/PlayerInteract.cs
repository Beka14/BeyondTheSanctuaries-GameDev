using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class PlayerInteract : PlayerSubsystem
{
    [SerializeField] private float maxInteractionDistance = 1;
    [SerializeField] private Camera playerCamera;

    private Interactable item;
    public Action<Interactable> OnInteractionObjectChange;

    public Interactable Item
    {
        get { return item; }
        private set
        {
            if (item == value)
                return;

            item = value;
            OnInteractionObjectChange?.Invoke(item);
        }
    }
    private void OnEnable()
    {
        Controls.OnUsePressed += OnUsePressed;
        Controls.OnUseReleased += OnUseReleased;
    }

    private void OnDisable()
    {
        Controls.OnUsePressed -= OnUsePressed;
        Controls.OnUseReleased -= OnUseReleased;
    }

    void OnUsePressed()
    {
        if (item == null)
            return;

        item.Interact(this);
    }
    private void OnUseReleased()
    {
        if (item == null)
        {
            return;
        }
        item.StopInteract(this);
        item = null;
    }

    private void Update()
    {
        ScanInteractable();
    }

    private bool ScanInteractable()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f));
        var items = Physics.RaycastAll(ray, maxInteractionDistance);

        foreach (var hit in items)
        {
            if (hit.transform.gameObject.TryGetComponent(out Interactable interactable))
            {
                Item = interactable;
                return true;
            }
        }

        Item = null;
        return false;
    }

}
