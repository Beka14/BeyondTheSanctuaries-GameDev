using System;
using TMPro;
using UnityEngine;

class Cleaner : IDisposable
{
    TMP_InputField input;
    public Cleaner(TMP_InputField input)
    {
        this.input = input;
    }

    public void Dispose()
    {
        input.text = "";
    }
}



public class ConsoleUI : UISubsystem
{
    PlayerSubsystem playerSubsystem;
    [SerializeField] TMP_Text output;
    [SerializeField] TMP_InputField input;
    [SerializeField] ItemSO[] ammo;
    [SerializeField] ItemSO[] impulse102;

    public override void Bind(PlayerSubsystem playerSubsystem)
    {
        this.playerSubsystem = playerSubsystem;
        playerSubsystem.Controls.OnConsolePressed += Toggle;
    }

    private void OnEnable()
    {
        input.onEndEdit.AddListener(OnEndEdit);
    }

    private void OnDisable()
    {
        input.onEndEdit.RemoveListener(OnEndEdit);
    }

    public void OnEndEdit(string cmd)
    {
        using var x = new Cleaner(input);

        if (cmd == "clear")
        {
            output.text = "";
            return;
        }

        if (cmd == "exit")
        {
            GameManager.QuitGame();
            return;
        }

        if (cmd == "help")
        {
            output.text += "Commands:\n";
            output.text += "clear - Clear the console\n";
            output.text += "exit - Exit the game\n";
            output.text += "help - Show this help message\n";
            output.text += "god - god mode\n";
            output.text += "allammo - adds 100 ammo to your gun\n";
            output.text += "impulse 102 - adds dynamite and shovel to the inventory\n";
            return;
        }

        if (cmd == "god")
        {
            var playerHealth = playerSubsystem.GetComponent<PlayerHealth>();
            if (!playerHealth)
            {
                output.text += "PlayerHealth component not found\n";
                return;
            }

            if (playerHealth.ToggleGodMode())
                output.text += "God mode on\n";
            else
                output.text += "God mode off\n";
            return;
        }

        if (cmd == "allammo")
        {
            var playerInventory = playerSubsystem.GetComponent<InventoryComponent>();
            if (!playerInventory)
            {
                output.text += "PlayerInventory component not found\n";
                return;
            }

            foreach (var i in ammo)
            {
                playerInventory.InventorySystem.AddItem(i, i.maxStack, out _);
                output.text += $"Added {i.maxStack} of {i.itemName}\n";
            }

            return;
        }

        if (cmd == "impulse 102")
        {
            var playerInventory = playerSubsystem.GetComponent<InventoryComponent>();
            if (!playerInventory)
            {
                output.text += "PlayerInventory component not found\n";
                return;
            }

            foreach (var i in impulse102)
            {
                playerInventory.InventorySystem.AddItem(i, i.maxStack, out _);
                output.text += $"Added {i.maxStack} of {i.itemName}\n";
            }
            return;
        }

        output.text += "Unknown command\n";
    }
}