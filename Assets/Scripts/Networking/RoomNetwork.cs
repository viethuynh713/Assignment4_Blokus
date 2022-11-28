using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RoomNetwork : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform m_ParentTrans;

    [SerializeField]
    private PlayerUI[] _listPlayerUIDisable;

    public Dictionary<string, PlayerUI>
        _listPlayerUIEnable = new Dictionary<string, PlayerUI>();

    [SerializeField]
    PhotonView photonView;


    private void Start()
    {
        var numOfPlayer = _listPlayerUIEnable.Count;
        if (PhotonNetwork.IsMasterClient)
        {
            _listPlayerUIDisable[numOfPlayer].gameObject.SetActive(true);
            _listPlayerUIDisable[numOfPlayer]
                .SetInfos(PhotonNetwork.LocalPlayer.NickName,
                (BrickColor) numOfPlayer,
                true,false);
            _listPlayerUIEnable
                .Add(PhotonNetwork.LocalPlayer.NickName,
                _listPlayerUIDisable[numOfPlayer]);
        }
        GameManager.instance.playerColor = (BrickColor)(PhotonNetwork.CurrentRoom.PlayerCount - 1);
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        var numOfPlayer = _listPlayerUIEnable.Count;

        _listPlayerUIDisable[numOfPlayer].gameObject.SetActive(true);
        _listPlayerUIDisable[numOfPlayer]
            .SetInfos(newPlayer.NickName, (BrickColor) numOfPlayer, false,false);
        Debug.Log(newPlayer.NickName);
        _listPlayerUIEnable
            .Add(newPlayer.NickName, _listPlayerUIDisable[numOfPlayer]);

        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var p in _listPlayerUIEnable)
            {
                photonView.RPC("DisplayUI", newPlayer, p.Value.index, p.Key,p.Value.IsBot);
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        foreach (var item in _listPlayerUIEnable)
        {
            if (item.Key == otherPlayer.NickName)
            {
                _listPlayerUIEnable.Remove(item.Key);
                item.Value.gameObject.SetActive(false);
                break;
            }
        }
    }

    [PunRPC]
    public void DisplayUI(int index, string namePlayer,bool isBot)
    {
        _listPlayerUIDisable[index].gameObject.SetActive(true);
        _listPlayerUIDisable[index]
            .SetInfos(namePlayer, (BrickColor) index, false,isBot);
        _listPlayerUIEnable.Add(namePlayer, _listPlayerUIDisable[index]);
        
    }

    public void AddBot(BotLevel level)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            var numOfPlayer = _listPlayerUIEnable.Count;
            if(numOfPlayer > 3)return;
            photonView.RPC("DisplayUI", RpcTarget.All, numOfPlayer, "Bot"+ Random.Range(0,99)+ level,true);
 

        }
    }
    
}
