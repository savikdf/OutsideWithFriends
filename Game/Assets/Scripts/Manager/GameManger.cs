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
    private InputManager inputManager;
    private List<IManager> managers = new List<IManager>();

    public GameManger() : this(new ResourceLoader(), new SpawnManager(), new InputManager()) { }

    public GameManger(IResourceLoader newResourceLoader, ISpawnManager newSpawnManager, InputManager newInputManager)
    {
        gameManger = this;
        resourceLoader = newResourceLoader;
        this.spawnManager = newSpawnManager;
        this.inputManager = newInputManager;
    }

    private void Awake()
    {
        managers.Add(spawnManager);
        managers.Add(inputManager);
        Initialize();        
    }

    public bool Initialize()
    {
        managers.ForEach(m => m.Initialize());
        
        // running manager's routine's
        managers.ForEach(m => StartCoroutine(m.Routine()));

        if (isDebug)
            StartCoroutine(DetectDebugInputs());

        return true;
    }

    public IEnumerator Routine(){
        yield return null;
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
