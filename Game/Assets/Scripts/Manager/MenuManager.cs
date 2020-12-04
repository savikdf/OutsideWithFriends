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
    private LobbyMenuCodeBehind lobbyCodeBehind;

    private bool isLobbyHost = false;
    private Button joinButton = null;

    public void Awake()
    {
        instance = this;
        networkManager = GetComponent<NetworkLobbyManager>();
        menuCodeBehinds = FindObjectsOfType<MonoBehaviour>().OfType<IMenuCodeBehind>().ToList();
        if (menuCodeBehinds.GroupBy(m => m.MenuIndex).Any(g => g.Count() > 1)) {
            Debug.LogError("Menu's with the same index exist. Fix or transition errors will occur");
        }
        lobbyCodeBehind = (LobbyMenuCodeBehind)menuCodeBehinds.FirstOrDefault(cb => cb.GetType() == typeof(LobbyMenuCodeBehind)); 
        if(lobbyCodeBehind == null)
        {
            Debug.LogError("No Lobby Codebehind detected");
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

    public void JoinLobbyAttempt(Button button) {
        //disable button and grab input ip
        string ip = GameObject.Find("Input_IP").GetComponent<InputField>().text; //only input is IP field
        joinButton = button;
        joinButton.enabled = false;

        //sub to callback events
        NetworkLobbyManager.OnClientConnected += HanddleClientConnected;
        NetworkLobbyManager.OnClientDisconnected += HanddleClientDisconnected;

        //attempt connection
        networkManager.networkAddress = ip;
        networkManager.StartClient();
    }

    public void ConnectToLobby(Text ipInput) {
        
    }

    public void HostLobbyStart() {
        networkManager.StartHost();
    }

    //Callback event
    private void HanddleClientConnected() {
        joinButton.enabled = true;
        TransitionMenus(3);
    }
    //Callback event
    private void HanddleClientDisconnected()
    {
        joinButton.enabled = true;
    }

    public void LobbyCancel() {
        if (isLobbyHost)
            networkManager.StopHost();
        lobbyCodeBehind.PlayerList = new List<string>();
        //unsub to callback events
        NetworkLobbyManager.OnClientConnected -= HanddleClientConnected;
        NetworkLobbyManager.OnClientDisconnected -= HanddleClientDisconnected;
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
