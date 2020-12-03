using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour, IManager
{
    public static MenuManager instance;
    private NetworkLobbyManager networkManager;
    public List<IMenuCodeBehind> menuCodeBehinds = new List<IMenuCodeBehind>();

    public void Awake()
    {
        instance = this;
        networkManager = GetComponent<NetworkLobbyManager>();
        menuCodeBehinds = FindObjectsOfType<MonoBehaviour>().OfType<IMenuCodeBehind>().ToList();
        if (menuCodeBehinds.GroupBy(m => m.MenuIndex).Any(g => g.Count() > 1)) {
            Debug.LogError("Menu's with the same index exist. Fix or transition errors will occur");
        }
        Initialize();
    }

    public void Start() {
        TransitionMenus(0);
        InitializePlayerNameField();
    }

    #region Player Name

    private void InitializePlayerNameField() {
        InputField nameInput = GameObject.Find("Input_PlayerName").GetComponent<InputField>();
        playerName = PlayerPrefs.GetString("PlayerName");
        if (nameInput != null)
        {
            nameInput.onEndEdit.AddListener(delegate { PlayerNameSet(nameInput.textComponent.text); });
            nameInput.textComponent.text = playerName;
        }
    }
    public string playerName {get; private set;}
    private void PlayerNameSet(string name)
    {
        playerName = name;
        PlayerPrefs.SetString("PlayerName", playerName);
    }

    #endregion

    #region Multiplayer

    public void JoinLobbyTransition(bool enabled) {
        if (enabled)
        {
            NetworkLobbyManager.OnClientConnected += HanddleClientConnected;
            NetworkLobbyManager.OnClientDisconnected += HanddleClientDisconnected;
        }
        else
        {
            NetworkLobbyManager.OnClientConnected -= HanddleClientConnected;
            NetworkLobbyManager.OnClientDisconnected -= HanddleClientDisconnected;
        }
    }

    public void ConnectToLobby(Text ipInput) {
        networkManager.networkAddress = ipInput.text;
        networkManager.StartClient();
    }

    public void HostLobbyTransition() {
        networkManager.StartHost();
    }

    private void HanddleClientConnected() { 
        
    }
    private void HanddleClientDisconnected()
    {

    }

    #endregion

    #region General

    public void TransitionMenus(int toIndex)
    {
        menuCodeBehinds.ForEach(m =>
        {
            m.GameObject.SetActive(m.MenuIndex == toIndex);
        });
    }

    public void TransitionToLevel(int toIndex)
    {
        SceneManager.LoadScene(toIndex);
    }

    #endregion

    public bool Initialize()
    {
        menuCodeBehinds.ForEach(m => m.Initialize());
        return true;
    }

    public IEnumerator Routine()
    {
        yield return null;
    }
   
}
