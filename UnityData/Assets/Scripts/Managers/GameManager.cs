using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static Dictionary<Scene, GameObject> _spawnPoints = new();
    static List<GameObject> _ddolObjects = new();
    public static GameManager instance;

    [SerializeField] bool cursorVisibleOnStart = false;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            CursorState.SetVisible(cursorVisibleOnStart);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void RegisterSpawnPoint(GameObject spawnPoint)
    {
        _spawnPoints[spawnPoint.scene] = spawnPoint;
    }

    public static void DontDestroyOnLoadEx(GameObject go)
    {
        Object.DontDestroyOnLoad(go);
        _ddolObjects.Add(go);
    }

    public static void DestroyAll()
    {
        foreach (var go in _ddolObjects)
            if (go != null)
                Destroy(go);

        _ddolObjects.Clear();
    }

    public void LoadLevel(int level, PlayerSubsystem player = null)
    {
        UIManager.EnableUI();
        var scene = SceneManager.GetSceneByBuildIndex(level);
        if (scene.isLoaded)
        {
            SceneManager.SetActiveScene(scene);
            if (player)
            {
                var controller = player.GetComponent<PlayerMovement>().Controller;
                controller.enabled = false;

                controller.gameObject.transform.position = _spawnPoints[scene].transform.position;
                controller.enabled = true;
            }
            return;
        }
        LoadSceneMode mode = SceneManager.GetActiveScene().buildIndex == 0
            ? LoadSceneMode.Single
            : LoadSceneMode.Additive;
        SceneManager.LoadScene(level, mode);
        CallAfterDelay.Create(0, () =>
        {
            var scene = SceneManager.GetSceneByBuildIndex(level);

            if (SceneManager.SetActiveScene(scene) && player)
            {
                var controller = player.GetComponent<PlayerMovement>().Controller;
                controller.enabled = false;

                controller.gameObject.transform.position = _spawnPoints[scene].transform.position;
                controller.enabled = true;
            }
        });
        CursorState.SetVisible(false);
    }

    public static void ToMainMenu()
    {
        DestroyAll();
        UIManager.DisableUI();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        CursorState.SetVisible(true);
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}