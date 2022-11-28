
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class NetworkCallBacks : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMPro.TMP_InputField m_NameIpF;
    [SerializeField] private GameObject m_LoadingPnl;
    [SerializeField] private GameObject m_LobbyPnl;
    [SerializeField] private GameObject m_InitPnl;
    [SerializeField] private GameObject m_roomUIPrefab;
    [SerializeField] private Transform m_roomParent;

private void Start() {
    if(PhotonNetwork.IsConnected)
    {
        m_LobbyPnl.SetActive(true);
        m_InitPnl.SetActive(false);
        m_NameIpF.text = PhotonNetwork.LocalPlayer.NickName;
        
        var myColor = -1;
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add("MyColor", myColor);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    else
    {
        m_LobbyPnl.SetActive(false);
        m_InitPnl.SetActive(true);
    }
}
    private Dictionary<string, GameObject> listRoom = new Dictionary<string, GameObject>();
    public void Play()
    {
        if(m_NameIpF.text == "")return;
        m_LoadingPnl.SetActive(true);
        m_InitPnl.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        m_LoadingPnl.SetActive(false);
        Debug.Log("Connect successful");
    if(PhotonNetwork.IsConnected)
    {
        PhotonNetwork.LocalPlayer.NickName = m_NameIpF.text;
    }
        PhotonNetwork.JoinLobby();
        Debug.Log("Hello "+PhotonNetwork.LocalPlayer.NickName);
    }
    public override void OnJoinedLobby()
    {
        m_LobbyPnl.SetActive(true);
    }
    #region Room
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room List Updated");
        foreach (var room in roomList)
        {
            
            if (!listRoom.ContainsKey(room.Name))
            {
                GameObject roomInfoUI = Instantiate(m_roomUIPrefab, m_roomParent);
                roomInfoUI.GetComponent<RoomInfoUI>().SetRoomInfo(room.Name);
                listRoom.Add(room.Name, roomInfoUI);
            }
            else
            {
                if (room.PlayerCount == 0)
                {
                    Destroy(listRoom[room.Name]);
                    listRoom.Remove(room.Name);
                }
            }
        }
    }

    public void CreateRoom(TMPro.TMP_InputField nameInF)
    {
        RoomOptions option = new RoomOptions()
        {
            MaxPlayers = 4,
            IsVisible = true,
            IsOpen = true,

        };
        PhotonNetwork.CreateRoom(nameInF.text, option);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Create Room successful");
        PhotonNetwork.LoadLevel("Main (khanh)");
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Main (khanh)");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }
    #endregion
}
