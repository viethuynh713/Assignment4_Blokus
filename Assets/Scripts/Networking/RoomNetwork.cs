using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomNetwork : MonoBehaviourPunCallbacks
{
    
    [SerializeField] private Transform m_ParentTrans;
    [SerializeField]private PlayerUI[] _listPlayerUIDisable;
    private Dictionary<string,PlayerUI> _listPlayerUIEnable = new Dictionary<string, PlayerUI>();
    private void Awake() {

    }
    private void Start() {
        var numOfPlayer = PhotonNetwork.CurrentRoom.PlayerCount -1;
        if(PhotonNetwork.IsMasterClient)
        {
            _listPlayerUIDisable[numOfPlayer].gameObject.SetActive(true);
            _listPlayerUIDisable[numOfPlayer].SetInfos(PhotonNetwork.LocalPlayer.NickName,(BrickColor)(numOfPlayer+1),true);
            _listPlayerUIEnable.Add(PhotonNetwork.LocalPlayer.NickName,_listPlayerUIDisable[numOfPlayer]);
            
        }
        else
        {
            _listPlayerUIDisable[numOfPlayer].gameObject.SetActive(true);
            _listPlayerUIDisable[numOfPlayer].SetInfos(PhotonNetwork.LocalPlayer.NickName,(BrickColor)(numOfPlayer+1),false);
            _listPlayerUIEnable.Add(PhotonNetwork.LocalPlayer.NickName,_listPlayerUIDisable[numOfPlayer]);
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
         var numOfPlayer = PhotonNetwork.CurrentRoom.PlayerCount -1;

            _listPlayerUIDisable[numOfPlayer].gameObject.SetActive(true);
            _listPlayerUIDisable[numOfPlayer].SetInfos(PhotonNetwork.LocalPlayer.NickName,(BrickColor)(numOfPlayer+1),false);
            _listPlayerUIEnable.Add(PhotonNetwork.LocalPlayer.NickName,_listPlayerUIDisable[numOfPlayer]);
        
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // foreach(var item in _listPlayerUI)
        // {
        //     if(item.GetComponent<PlayerUI>().nameTxt.text == otherPlayer.NickName)
        //     {
        //         _listPlayerUI.Remove(item);
        //         Destroy(item.gameObject);
        //         break;
        //     }
        // }
    }

}
