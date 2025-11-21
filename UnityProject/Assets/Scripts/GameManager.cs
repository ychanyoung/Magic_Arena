using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Threading;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject login;
    public GameObject lobby;
    public string nickname;
    public Text Nickname;
    public Text Error;
    public NetworkManager networkManager;
    public string character;
    public GameObject game;
    public GameObject Dark_Skill, Poison_Skill, Hydra_Skill;
    public Text StartCount;
    public int SC;
    public Text time;
    public int gameTime;
    public GameObject EndGame;
    public float health;
    public GameManager gameManager;
    bool CanUpdate = true;
    public Text GameEnd;
    public GameObject gameEnd;
    public string Winner;
    public bool IsEnd = false;
    public int HydraCount;
    public bool IsDarkR;
    public AudioSource AS;

    void Start()
    {
        login.SetActive(true);
        lobby.SetActive(false);
        game.SetActive(false);
    }

    void Update()
    {
        if (gameManager.IsEnd == true && CanUpdate == true)
        {
            CanUpdate = false;
            gameEnd.SetActive(true);
            GameEnd.text = gameManager.Winner + "\n" + "승리"; 
            Invoke("GameOver", 3);
        }
        Dark_Skill.transform.GetChild(0).GetComponent<Image>().fillAmount = Dark_Skill.transform.GetChild(0).GetComponent<Image>().fillAmount + (0.4f * Time.deltaTime); //Q 스킬4초쯤
        Poison_Skill.transform.GetChild(0).GetComponent<Image>().fillAmount = Poison_Skill.transform.GetChild(0).GetComponent<Image>().fillAmount + (0.55f * Time.deltaTime); //Q 스킬3초쯤
        Hydra_Skill.transform.GetChild(0).GetComponent<Image>().fillAmount = Hydra_Skill.transform.GetChild(0).GetComponent<Image>().fillAmount + (0.25f * (gameManager.HydraCount * 0.2f + 1) * Time.deltaTime); //Q 스킬7-a초쯤
        Dark_Skill.transform.GetChild(1).GetComponent<Image>().fillAmount = Dark_Skill.transform.GetChild(1).GetComponent<Image>().fillAmount + (0.25f * Time.deltaTime); //W 스킬4초쯤
        Poison_Skill.transform.GetChild(1).GetComponent<Image>().fillAmount = Poison_Skill.transform.GetChild(1).GetComponent<Image>().fillAmount + (0.25f * Time.deltaTime); //W 스킬5초쯤
        Hydra_Skill.transform.GetChild(1).GetComponent<Image>().fillAmount = Hydra_Skill.transform.GetChild(1).GetComponent<Image>().fillAmount + (0.15f * (gameManager.HydraCount * 0.2f + 1) * Time.deltaTime); //W 스킬6초쯤
        Dark_Skill.transform.GetChild(2).GetComponent<Image>().fillAmount = Dark_Skill.transform.GetChild(2).GetComponent<Image>().fillAmount + (0.05f * Time.deltaTime); //E
        Poison_Skill.transform.GetChild(2).GetComponent<Image>().fillAmount = Poison_Skill.transform.GetChild(2).GetComponent<Image>().fillAmount + (0.05f * Time.deltaTime); //E
        Hydra_Skill.transform.GetChild(2).GetComponent<Image>().fillAmount = Hydra_Skill.transform.GetChild(2).GetComponent<Image>().fillAmount + (0.1f * Time.deltaTime); //E
        Dark_Skill.transform.GetChild(3).GetComponent<Image>().fillAmount = Dark_Skill.transform.GetChild(3).GetComponent<Image>().fillAmount + (0.02f * Time.deltaTime); //R
        Poison_Skill.transform.GetChild(3).GetComponent<Image>().fillAmount = Poison_Skill.transform.GetChild(3).GetComponent<Image>().fillAmount + (0.02f * Time.deltaTime); //R
        Hydra_Skill.transform.GetChild(3).GetComponent<Image>().fillAmount = Hydra_Skill.transform.GetChild(3).GetComponent<Image>().fillAmount + (0.05f * Time.deltaTime); //R


    }


    public void Login() //로그인
    {
        if (System.Text.Encoding.Default.GetBytes(Nickname.text).Length < 10 && System.Text.Encoding.Default.GetBytes(Nickname.text).Length > 3)
        {
            nickname = Nickname.text;
            login.SetActive(false);
            lobby.SetActive(true);
            networkManager.Connect();
        }

        if (System.Text.Encoding.Default.GetBytes(Nickname.text).Length >= 10)
        {
            Error.text = "닉네임이 너무 길어요";
        }
        if (System.Text.Encoding.Default.GetBytes(Nickname.text).Length <= 3)
        {
            Error.text = "닉네임이 너무 짧아요";
        }
    }
    public void GameStart() //게임 시작
    {
        gameManager.IsEnd = false;
        lobby.SetActive(false);
        login.SetActive(false);
        game.SetActive(true);
        SC = 3;
        InvokeRepeating("GameStartCount", 0, 1);
        gameTime = 0; ;
        InvokeRepeating("GameTime", 3, 1);
        CanUpdate = true;
    }
    public void GameOver() //게임 끝나고 다시시작
    {
        PhotonNetwork.Disconnect();
        gameEnd.SetActive(false);
        CancelInvoke("GameTime");
        lobby.SetActive(true);
        Dark_Skill.SetActive(false);
        Poison_Skill.SetActive(false);
        Hydra_Skill.SetActive(false);
        game.SetActive(false);
    }
    
    void GameStartCount()
    {
        time.text = "Start";
        if (SC == 0)
        {
            CancelInvoke("GameStartCount");
            if (character == "Dark")
                Dark_Skill.SetActive(true);
            if (character == "Poison")
                Poison_Skill.SetActive(true);
            if (character == "Hydra")
                Hydra_Skill.SetActive(true);
            if (PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.LocalPlayer.NickName)
            {
                GameObject player = PhotonNetwork.Instantiate("Player" + character, new Vector3(0, -3, 0), Quaternion.identity) as GameObject;
                Dark_Skill.transform.GetChild(3).GetComponent<Image>().fillAmount = 0;
                Poison_Skill.transform.GetChild(3).GetComponent<Image>().fillAmount = 0;
                Hydra_Skill.transform.GetChild(3).GetComponent<Image>().fillAmount = 0;
            }
            else
            {
                GameObject player = PhotonNetwork.Instantiate("Player" + character, new Vector3(0, 5, 0), Quaternion.identity) as GameObject;
                Dark_Skill.transform.GetChild(3).GetComponent<Image>().fillAmount = 0;
                Poison_Skill.transform.GetChild(3).GetComponent<Image>().fillAmount = 0;
                Hydra_Skill.transform.GetChild(3).GetComponent<Image>().fillAmount = 0;
            }
            StartCount.text = "";
        }
        else
        {
            StartCount.text = SC.ToString();
            SC--;
            AS.Play();
        }
    }

    void GameTime()
    {
        if(gameTime > 120)
        {
            time.text = gameTime.ToString();
            time.color = Color.red;
            gameTime++;
            EndGame.SetActive(true);
        }
        else
        {
            time.text = gameTime.ToString();
            gameTime++;
            EndGame.SetActive(false);
            time.color = Color.black;
        }
    }



}
