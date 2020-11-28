using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour, IManager
{
    public static GameManger gameManger;
    public static IResourceLoader resourceLoader;

    public bool isDebug;
    private ISpawnManager spawnManager;
    private List<IManager> managers = new List<IManager>();

    public GameManger() : this(new ResourceLoader(), new SpawnManager()) { }

    public GameManger(IResourceLoader newResourceLoader, ISpawnManager newSpawnManager)
    {
        gameManger = this;
        resourceLoader = newResourceLoader;
        this.spawnManager = newSpawnManager;
    }

    private void Awake()
    {
        managers.Add(spawnManager);
        Initialize();        
    }

    public bool Initialize()
    {
        managers.ForEach(m => m.Initialize());

        if (isDebug)
            StartCoroutine(DetectDebugInputs());

        return true;
    }

    private IEnumerator DetectDebugInputs() {
        while (isDebug) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Debug.Log("Resetting Scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            yield return null;
        }
    
    }

    
}
