using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour, IManager
{
    public List<IMenuCodeBehind> menuCodeBehinds = new List<IMenuCodeBehind>();
    private IMenuCodeBehind activeMenu = null;

    [HideInInspector] public LobbyMenuCodeBehind lobbyCodeBehind;
    private NetworkManagerLobby networkLobbyManager;
    public NetworkManagerLobby NetworkLobbyManager
    {
        get { return networkLobbyManager; }
    }
    private Button joinButton = null;
    [HideInInspector] public bool isLobbyHost = false;
    [HideInInspector] public string playerDisplayName = string.Empty;

    public void Awake()
    {
        if (SceneManager.GetActiveScene().name.Contains("Menu"))
        {
            networkLobbyManager = GetComponent<NetworkManagerLobby>();
            menuCodeBehinds = FindObjectsOfType<MonoBehaviour>().OfType<IMenuCodeBehind>().ToList();
            if (menuCodeBehinds.GroupBy(m => m.MenuIndex).Any(g => g.Count() > 1))
            {
                Debug.LogError("Menu's with the same index exist. Fix or transition errors will occur");
            }
            lobbyCodeBehind = (LobbyMenuCodeBehind)menuCodeBehinds.FirstOrDefault(cb => cb.GetType() == typeof(LobbyMenuCodeBehind));
            if (lobbyCodeBehind == null)
            {
                Debug.LogError("No Lobby Codebehind detected");
            }
            Initialize();
        }
        
    }

    public void Start() {
        if (SceneManager.GetActiveScene().name.Contains("Menu"))
            TransitionMenus(-1);
    }

    #region Multiplayer

    public void JoinLobby(Button button) {
        //disable button and grab input ip
        string ip = GameObject.Find("Input_IP").GetComponent<InputField>().text;    //only input is IP field
        ip = string.IsNullOrWhiteSpace(ip) ? "localhost" : ip;                      //default to localhost if empty input

        joinButton = button;
        joinButton.enabled = false;

        //sub to callback events
        NetworkManagerLobby.OnClientConnected += HanddleClientConnected;
        NetworkManagerLobby.OnClientDisconnected += HanddleClientDisconnected;

        //attempt connection
        networkLobbyManager.networkAddress = ip;
        networkLobbyManager.StartClient();
    }

    public void HostLobbyStart() {
        networkLobbyManager.StartHost();
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
        TransitionMenus(1);
    }

    public void LobbyCancel() {
        if (isLobbyHost)
            networkLobbyManager.StopHost();

        TransitionMenus(1);
        isLobbyHost = false;
        //unsub to callback events
        NetworkManagerLobby.OnClientConnected -= HanddleClientConnected;
        NetworkManagerLobby.OnClientDisconnected -= HanddleClientDisconnected;
    }

    #endregion

    #region General

    public void SetPlayerName(InputField nameInput)
    {
        playerDisplayName = nameInput.text;
        GameObject.Find("PlayerName").GetComponent<Text>().text = playerDisplayName;
    }

    public void TransitionMenus(int toIndex)
    {
        menuCodeBehinds.ForEach(m =>
        {
            m.GameObject.SetActive(m.MenuIndex == toIndex);
        });
        activeMenu = menuCodeBehinds.First(m => m.MenuIndex == toIndex);
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
