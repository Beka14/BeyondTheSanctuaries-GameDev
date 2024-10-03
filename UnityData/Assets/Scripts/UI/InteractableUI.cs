using System;
using UnityEngine;
using TMPro;

public class InteractableUI : UISubsystem
{
    [Header("Inspector")]
    [SerializeField] private TextMeshProUGUI interactableText;

    PlayerInteract interact;
    bool enabledUse = true;

    public override void Bind(PlayerSubsystem playerSubsystem)
    {
        interact = playerSubsystem.GetComponent<PlayerInteract>();
        if(!interact)
            return;

        interact.OnInteractionObjectChange += OnInteractionObjectChange;
        UIManager.OnUIOpen += OnUIOpen;
        UIManager.OnUIClose += OnUIClose;
    }

    private void OnDestroy()
    {
        if (interact)
            interact.OnInteractionObjectChange -= OnInteractionObjectChange;

        UIManager.OnUIOpen -= OnUIOpen;
        UIManager.OnUIClose -= OnUIClose;
    }


    private void OnUIClose()
    {
        interact.enabled = true;
        enabledUse = true;

        if (interact.Item)
            gameObject.SetActive(true);
    }

    private void OnUIOpen()
    {
        interact.enabled = false;
        enabledUse = false;
        gameObject.SetActive(false);
    }


    private void OnInteractionObjectChange(Interactable interactable)
    {
        if(interactable == null || !enabledUse)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        interactableText.text = interactable.GetInteractText();
    }

}
