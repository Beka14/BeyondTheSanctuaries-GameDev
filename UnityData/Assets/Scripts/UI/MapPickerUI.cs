using UnityEngine;

public class MapPickerUI : UISubsystem
{
    PlayerSubsystem playerSubsystem;
    public override void Bind(PlayerSubsystem playerSubsystem)
    {
        this.playerSubsystem = playerSubsystem;
    }

    public void OnMap(int mapIndex)
    {
        Close();
        GameManager.instance.LoadLevel(mapIndex, playerSubsystem);
    }
}
