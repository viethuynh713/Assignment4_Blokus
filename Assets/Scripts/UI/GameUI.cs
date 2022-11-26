using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance; 
    private List<GameObject> playerPanelList;

    [SerializeField] private Image playerPanelSample;
    [SerializeField] private Color[] playerPanelColorList = new Color[4];
    [SerializeField] private Color darkColor;

    // Start is called before the first frame update
    private void Awake() {
        instance = this;
    }
    void Start()
    {
        playerPanelList = new List<GameObject>();
    }

    // Update is called once per frame


    public void initPlayerPanelList(int nPlayer)
    {
        for (int i = 0; i < nPlayer; i++)
        {
            GameObject playerPanel = Instantiate(playerPanelSample.gameObject);
            playerPanel.transform.SetParent(transform, false);
            playerPanel.GetComponent<Image>().color = darkColor;
            playerPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Player " + (i + 1).ToString();
            playerPanel.transform.position = new Vector2(765 + 135 * i, 635);
            playerPanelList.Add(playerPanel);
        }
        playerPanelList[0].GetComponent<Image>().color = playerPanelColorList[0];
    }

    public void switchPlayerUI(int turn)
    {
        if (turn == 0)
        {
            playerPanelList[playerPanelList.Count - 1].GetComponent<Image>().color = darkColor;
        }
        else
        {
            playerPanelList[turn - 1].GetComponent<Image>().color = darkColor;
        }
        playerPanelList[turn].GetComponent<Image>().color = playerPanelColorList[turn];
    }

    public void printResult(List<int> rank)

    {
        for (int i = 0; i < playerPanelList.Count; i++)
        {
            playerPanelList[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = rank[i].ToString();
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadSceneAsync("Loading");
    }
}
