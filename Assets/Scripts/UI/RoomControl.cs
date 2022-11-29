using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BotLevel
{
    EASY,
    MEDIUM,
    HARD,
    NULL
}

public class RoomControl : MonoBehaviour
{
    [SerializeField] RoomNetwork roomNetwork;
    [SerializeField] GameObject passBtn;
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
            GameManager.instance.StartGameUI(roomNetwork._listPlayerUIEnable.Count);
            roomNetwork.photonView.RPC("OnEnablePanel",RpcTarget.All);
        }
        
    }
    [PunRPC]
    private void OnEnablePanel() {
        this.gameObject.SetActive(false);
        passBtn.SetActive(true);
    }
    
}
