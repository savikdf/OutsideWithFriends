using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManger : MonoBehaviour, IManager
{
    public static GameManger singleton;
    public static IResourceLoader resourceLoader;
    public bool isDebug = false;
    [HideInInspector] public ISpawnManager spawnManager;
    private IInputManager inputManager;
    private List<IManager> managers = new List<IManager>();
    public IInputManager InputManager { get { return this.inputManager; } set { } }

    private void Awake()
    {
        singleton = singleton == null ? this : singleton;
        if (singleton != this)
        {
            Destroy(this);
        }

        resourceLoader = new ResourceLoader();
        inputManager = new InputManager();
        spawnManager = GetComponent<SpawnManager>(); //overriden by the network

        managers.Add(spawnManager);
        managers.Add(inputManager);
        Initialize();

        DontDestroyOnLoad(this.gameObject);
    }

    public bool Initialize()
    {
        if (managers != null && managers.Count > 0)
        {
            managers.ForEach(m => m.Initialize());
            // running manager's routine's
            managers.ForEach(m => StartCoroutine(m.Routine()));
        }

        if (isDebug)
            StartCoroutine(DetectDebugInputs());

        return true;
    }

    public void Start()
    {
    }

    public IEnumerator Routine()
    {
        yield return null;
    }

    private IEnumerator DetectDebugInputs()
    {
        while (isDebug)
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Debug.Log("Resetting Scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            yield return null;
        }

    }


}
