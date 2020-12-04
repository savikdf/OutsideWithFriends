using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuCodeBehind : MenuCodeBehind
{
    List<string> playerList = new List<string>();
    public List<string> PlayerList {
        get { return playerList; }
        set {
            PlayerList = value;
            RefreshPlayerList();
        }
    }
    StringBuilder stringBuilder = new StringBuilder();
    public Text playerListText;
    public Text ipAddressText;

    //public void AddPlayerToLobbyList(string playerName) {
    //    playerList.Add(playerName);
    //    RefreshPlayerList();
    //}

    //public void RemovePlayerToLobbyList(string playerName)
    //{
    //    playerList.Add(playerName);
    //    RefreshPlayerList();
    //}

    void RefreshPlayerList() { 
        if(playerListText != null)
        {
            playerList.ForEach(p => stringBuilder.AppendLine(" " + p)); //space is because im too lazy to space the UI correctly
            playerListText.text = stringBuilder.ToString();
            stringBuilder.Clear();
        }
    }
}
