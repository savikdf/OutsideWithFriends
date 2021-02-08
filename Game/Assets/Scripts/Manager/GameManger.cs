using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManger : MonoBehaviour, IManager
{
    public static GameManger singleton;
    public static IResourceLoader resourceLoader;
    public bool isDebug;
    [HideInInspector] public ISpawnManager spawnManager;
    private IInputManager inputManager;
    private List<IManager> managers = new List<IManager>();
    public IInputManager InputManager{get{return this.inputManager;} set{}}
    public GameManger() : this(new ResourceLoader(), new InputManager()) { }

    public GameManger(IResourceLoader newResourceLoader, InputManager newInputManager)
    {
        singleton = singleton == null? this: singleton;
        
        resourceLoader = newResourceLoader;
        this.inputManager = newInputManager;
        this.spawnManager = null; //set by the network
    }

    private void Awake()
    {
        if (singleton != this)
        {
            Destroy(this);
        }
        managers.Add(inputManager);
        Initialize();

        DontDestroyOnLoad(this.gameObject);
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
            if (Input.GetKeyDown(KeyCode.Delete)) {
                Debug.Log("Resetting Scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            yield return null;
        }
    
    }

    
}
