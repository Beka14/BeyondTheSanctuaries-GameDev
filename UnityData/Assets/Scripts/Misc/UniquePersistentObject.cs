using UnityEngine;

class UniquePersistentObject : MonoBehaviour
{
    public static UniquePersistentObject Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameManager.DontDestroyOnLoadEx(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}