using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerSubsystem : MonoBehaviour
{
    [HideInInspector] public PlayerController player;

    public ControlWrap Controls => player.controls;
    public PlayerEvents Events => player.events;

    protected virtual void Awake()
    {
        player = transform.root.GetComponentInChildren<PlayerController>();
        if (!player.initialized)
        {
            player.Init();           
        }
    }
}