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
    [SerializeField] RoomNetwork roomNetwork;
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadSceneAsync("Loading");
    }

    public void AddBot(int level)
    {
        roomNetwork.AddBot((BotLevel)level);
    }

    public void PlayGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
        Debug.Log("Playyyyyyyy");
        }
        
    }
}
