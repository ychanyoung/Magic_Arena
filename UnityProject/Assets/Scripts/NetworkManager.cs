using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text Status;
    public Text welcome;
    public GameManager gameManager;
    public GameObject button;
    public InputField roomInput;
    public GameObject room;
    public InputField ConnectroomInput;
    public int Players;
    public Text wait;
    public Text count;
    float countA;
    public Text Character;

    void Start()
    {
        Character.text = "Dark";
    }

    public void Connect() //로그인 접속
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster() //로비 접속
    {
        PhotonNetwork.LocalPlayer.NickName = gameManager.nickname;
        welcome.text = PhotonNetwork.LocalPlayer.NickName + "님이 로비에 접속";
        PhotonNetwork.JoinLobby();
        room.SetActive(false);
    }
    void Update()
    {
        Players = PhotonNetwork.PlayerList.Length;
        gameManager.character = Character.text;
        if (PhotonNetwork.PlayerList.Length == 2) //방이 찼을때
        {
            if(PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.PlayerList[1].NickName)
            {
                PhotonNetwork.PlayerList[1].NickName = PhotonNetwork.PlayerList[1].NickName + "1";
            }
            button.SetActive(false);
            wait.text = "";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                wait.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : "\n" + " vs " + "\n");
        }
        else //방이 안찼을때
        {
            wait.text = "대기중";
            wait.text += "\n" + PhotonNetwork.LocalPlayer.NickName;
        }
        Status.text = PhotonNetwork.NetworkClientState.ToString();

    }
    public void JoinRoom() //방 들어가기
    {
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    public void Disconnect() => PhotonNetwork.Disconnect(); //방 나가기

    public override void OnDisconnected(DisconnectCause cause) //방에서 나갔을때
    {
        if (PhotonNetwork.NetworkClientState.ToString() != "PeerCreated")
        {
            room.SetActive(false);
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.JoinLobby();
            CancelInvoke("Count");
            button.SetActive(true);
        }
    }

    public override void OnCreatedRoom() //방을 만들었을때
    {
        button.SetActive(false);
        welcome.text = "";
    }

    public override void OnJoinedRoom() //방을 들어왔을때
    {
        button.SetActive(false);
        room.SetActive(true);
        welcome.text = PhotonNetwork.CurrentRoom.Name + "방에 참가";
        countA = 5;
        InvokeRepeating("Count", 0, 0.1f);
    }

    public void JoinRandomRoom() //랜덤 방 들어가기
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { roomInput.text = ""; CreateRoom(); }

    public override void OnJoinRandomFailed(short returnCode, string message) { roomInput.text = ""; CreateRoom(); }

    public void CreateRoom() //방 만들기
    {
        if (roomInput.text == "")
        {
            roomInput.text = "Room" + Random.Range(0, 100);
            PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
        }
        else
            PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
    }
    public void ConnectRoom() //방 들어가기
    {
        PhotonNetwork.JoinRoom(ConnectroomInput.text);
    }

    public void ChooseCharacter(string charname) //캐릭터 선택하기
    {
        Character.text = charname;
    }

    public void Count() //레디 카운트 다운
    {
        if (PhotonNetwork.PlayerList.Length == 2 && countA >= 0) //PlayerList.Length == 2임
        {
            countA = countA - 0.1f;
            count.text = System.Math.Ceiling(countA).ToString();
        }
        else
        {
            countA = 5;
            count.text = "";
        }
        if(countA < 0)
        {
            CancelInvoke("Count");
            gameManager.GameStart();
            countA = 5;
        }

    }
}
