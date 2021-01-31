using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGamePlayerLobby : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Default";

    private NetworkManagerLobby room;
    public NetworkManagerLobby Room {
        get { 
            if(room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartClient()
    {
        //enable this if going between scenes mid game: DontDestroyOnLoad(GameObject);
        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }

}


