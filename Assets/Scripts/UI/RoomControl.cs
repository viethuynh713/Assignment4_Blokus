using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BotLevel
{
    EASY,
    MEDIUM,
    HARD
}

public class RoomControl : MonoBehaviour
{
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadSceneAsync("Loading");
    }

    public void AddBot(int level)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Add bot " + (BotLevel) level);
        }
    }

    public void PlayGame()
    {
        Debug.Log("Playyyyyyyy");
    }
}
