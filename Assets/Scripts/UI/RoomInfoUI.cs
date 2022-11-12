using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RoomInfoUI : MonoBehaviour
{
    public TMPro.TMP_Text NameTxt;
    
    public void SetRoomInfo(string roomName)
    {
        NameTxt.text = roomName;
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(NameTxt.text);
    }


}
