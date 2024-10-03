using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Awake()
    {
        GameManager.RegisterSpawnPoint(gameObject);
    }
}