using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public SpriteRenderer SR;
    public PhotonView PV;
    public Image HealthBar;
    bool canWalk = true;
    bool canAttack = true;
    public float Health = 100;
    int flipX = 1;
    public GameManager gameManager;
    public float slow;
    public int Player;
    bool IsPoison;
    public int Hydracount = 0;
    public Sprite Hydra1;
    public Sprite Hydra2;
    public Sprite Hydra3;
    public Sprite Hydra4;
    public Sprite Hydra5;
    public bool IsDarkE = false;
    float Speed = 1;
    public bool IsDarkR = false;
    public bool PoisonDamage = false;
    public GameObject DarkR;
    public AudioSource AS;
    public GameObject Q;
    public GameObject W;
    public GameObject E;
    public GameObject R;
    Vector3 curPos;


    void Start()
    {
        PoisonDamage = false;
        IsDarkR = false;
        Speed = 1;
        IsDarkE = false;
        if (gameObject.name == "PlayerHydra(Clone)")
        {
            Hydracount = 1;
        }
        IsPoison = false;
        CancelInvoke("Poison");
        Health = 100;
        slow = 1;
        if (PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.LocalPlayer.NickName)
        {
            Player = 1; //Player[0]
        }
        else
            Player = -1; //Player[1]
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EndGame")
            InvokeRepeating("EndGame", 0, 0.5f);
    }

    public void EndGame()
    {
        Health--;
        if (Health == 0)
            CancelInvoke("EndGame");
    }

    public void Damaged(float Damage)
    {
        Health = Health - Damage;
    }
    public void Poison()
    {
        IsPoison = true;
        Health--;
        if (PoisonDamage == true)
            Health--;
    }

    public void NotPoison()
    {
        PoisonDamage = false;
    }

    public void Slow()
    {
        slow = slow - 0.4f;
    }
    public void NotSlow()
    {
        slow = slow + 0.4f;
    }
    [PunRPC]
    void ColorRPCSlow()
    {
        SR.color = Color.blue;
    }
    [PunRPC]
    void ColorRPCPoison()
    {
        SR.color = Color.green;
    }
    [PunRPC]
    void ColorRPCWhite()
    {
        SR.color = Color.white;
    }
    [PunRPC]
    void ColorRPCBlack()
    {
        SR.color = Color.black;
    }


    void CanAttack()
    {
        canAttack = true;
    }
    void CanWalk()
    {
        canWalk = true;
    }

    void DarkEOff()
    {
        IsDarkE = false;
        Speed = 1;
    }

    void DarkROff()
    {
        IsDarkR = false;
        PV.RPC("DarkROffRPC", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void DarkROffRPC()
    {
        DarkR.SetActive(false);
    }

    public void PoisonE()
    {
        slow = slow - 0.5f;
        Invoke("NotSlow", 3);
    }


    void HydraWCount()
    {
        canAttack = false;
        if (Hydracount == 1)
        {
            Invoke("HydraWCounT", 0);
            Invoke("CanAttack", 0.1f);
        }
        if (Hydracount == 2)
        {
            Invoke("HydraWCounT", 0);
            Invoke("HydraWCounT", 0.1f);
            Invoke("CanAttack", 0.2f);
        }
        if (Hydracount == 3)
        {
            Invoke("HydraWCounT", 0);
            Invoke("HydraWCounT", 0.1f);
            Invoke("HydraWCounT", 0.2f);
            Invoke("CanAttack", 0.3f);
        }
        if (Hydracount == 4)
        {
            Invoke("HydraWCounT", 0);
            Invoke("HydraWCounT", 0.1f);
            Invoke("HydraWCounT", 0.2f);
            Invoke("HydraWCounT", 0.3f);
            Invoke("CanAttack", 0.4f);
        }
        if (Hydracount == 5)
        {
            Invoke("HydraWCounT", 0);
            Invoke("HydraWCounT", 0.1f);
            Invoke("HydraWCounT", 0.2f);
            Invoke("HydraWCounT", 0.3f);
            Invoke("HydraWCounT", 0.4f);
            Invoke("CanAttack", 0.5f);
        }
    }
    void HydraWCounT()
    {
        PhotonNetwork.Instantiate("HydraW", new Vector3(transform.position.x, transform.position.y + Player, transform.position.z), Quaternion.identity);
    }



    void FixedUpdate()
    {
        PV.RPC("Game", RpcTarget.AllBuffered);
        if (gameObject.name == "PlayerDark(Clone)")
        {
            flipX = 1;
        }
        else
        {
            flipX = -1;
        }
        if (PV.IsMine)
        {
            HealthBar.fillAmount = (Health / 100);
            gameManager.health = Health;
            gameManager.HydraCount = Hydracount;
            gameManager.IsDarkR = IsDarkR;
            if (Input.GetKey(KeyCode.Q))
            {
                if (gameObject.name == "PlayerDark(Clone)" && GameObject.Find("DarkQ").GetComponent<Image>().fillAmount == 1 && canAttack == true)
                {
                    PhotonNetwork.Instantiate("DarkQ", new Vector3(transform.position.x, transform.position.y + Player, transform.position.z), Quaternion.identity);
                    GameObject.Find("DarkQ").GetComponent<Image>().fillAmount = 0;
                    canAttack = false;
                    canWalk = false;
                    Invoke("CanAttack", 0.1f);
                    Invoke("CanWalk", 0.2f);
                    PV.RPC("SoundQ", RpcTarget.AllBuffered);
                }
                if (gameObject.name == "PlayerPoison(Clone)" && GameObject.Find("PoisonQ").GetComponent<Image>().fillAmount == 1 && canAttack == true)
                {
                    PhotonNetwork.Instantiate("PoisonQ", new Vector3(transform.position.x, transform.position.y + Player, transform.position.z), Quaternion.identity);
                    GameObject.Find("PoisonQ").GetComponent<Image>().fillAmount = 0;
                    canAttack = false;
                    canWalk = false;
                    Invoke("CanAttack", 0.1f);
                    Invoke("CanWalk", 0.2f);
                    PV.RPC("SoundQ", RpcTarget.AllBuffered);
                }
                if (gameObject.name == "PlayerHydra(Clone)" && GameObject.Find("HydraQ").GetComponent<Image>().fillAmount == 1 && canAttack == true)
                {
                    PhotonNetwork.Instantiate("HydraQ", new Vector3(transform.position.x, transform.position.y + Player, transform.position.z), Quaternion.identity);
                    GameObject.Find("HydraQ").GetComponent<Image>().fillAmount = 0;
                    canAttack = false;
                    canWalk = false;
                    Invoke("CanAttack", 0.1f);
                    Invoke("CanWalk", 0.2f);
                    PV.RPC("SoundQ", RpcTarget.AllBuffered);
                }
            }
            if (Input.GetKey(KeyCode.W))
            {
                if (gameObject.name == "PlayerDark(Clone)" && GameObject.Find("DarkW").GetComponent<Image>().fillAmount == 1 && canAttack == true)
                {
                    PhotonNetwork.Instantiate("DarkW", new Vector3(transform.position.x, transform.position.y + Player, transform.position.z), Quaternion.identity);
                    GameObject.Find("DarkW").GetComponent<Image>().fillAmount = 0;
                    canAttack = false;
                    canWalk = false;
                    Invoke("CanAttack", 0.1f);
                    Invoke("CanWalk", 0.2f);
                    PV.RPC("SoundW", RpcTarget.AllBuffered);
                }
                if (gameObject.name == "PlayerPoison(Clone)" && GameObject.Find("PoisonW").GetComponent<Image>().fillAmount == 1 && canAttack == true)
                {
                    PhotonNetwork.Instantiate("PoisonW", new Vector3(transform.position.x, transform.position.y + Player, transform.position.z), Quaternion.identity);
                    GameObject.Find("PoisonW").GetComponent<Image>().fillAmount = 0;
                    canAttack = false;
                    canWalk = false;
                    PV.RPC("SoundW", RpcTarget.AllBuffered);
                    Invoke("CanAttack", 0.1f);
                    Invoke("CanWalk", 0.2f);
                }
                if (gameObject.name == "PlayerHydra(Clone)" && GameObject.Find("HydraW").GetComponent<Image>().fillAmount == 1 && canAttack == true)
                {
                    GameObject.Find("HydraW").GetComponent<Image>().fillAmount = 0;
                    HydraWCount();
                    PV.RPC("SoundW", RpcTarget.AllBuffered);
                }
            }
            if (Input.GetKey(KeyCode.E))
            {
                if (gameObject.name == "PlayerDark(Clone)" && GameObject.Find("DarkE").GetComponent<Image>().fillAmount == 1)
                {
                    GameObject.Find("DarkE").GetComponent<Image>().fillAmount = 0;
                    canAttack = false;
                    IsDarkE = true;
                    Speed = 2f;
                    Invoke("DarkEOff", 3);
                    Invoke("CanAttack", 3);
                    PV.RPC("Sound", RpcTarget.AllBuffered);
                }
                if (gameObject.name == "PlayerPoison(Clone)" && GameObject.Find("PoisonE").GetComponent<Image>().fillAmount == 1)
                {
                    GameObject.Find("PoisonE").GetComponent<Image>().fillAmount = 0;
                    canWalk = false;
                    if(Player == 1)
                        PhotonNetwork.Instantiate("PoisonE", new Vector3(0, 2.75f, 0), Quaternion.identity);
                    if (Player == -1)
                        PhotonNetwork.Instantiate("PoisonE", new Vector3(0, -1.75f, 0), Quaternion.identity);
                    Invoke("CanWalk", 0.2f);
                }
                if (gameObject.name == "PlayerHydra(Clone)" && GameObject.Find("HydraE").GetComponent<Image>().fillAmount == 1 && Hydracount > 1)
                {
                    GameObject.Find("HydraE").GetComponent<Image>().fillAmount = 0;
                    Hydracount--;
                    Health = Health + 40;
                }
            }
            if (Input.GetKey(KeyCode.R))
            {
                if (gameObject.name == "PlayerDark(Clone)" && GameObject.Find("DarkR").GetComponent<Image>().fillAmount == 1)
                {
                    GameObject.Find("DarkR").GetComponent<Image>().fillAmount = 0;
                    IsDarkR = true;
                    PV.RPC("DarkRRPC", RpcTarget.AllBuffered);
                    Invoke("DarkROff", 8);
                }
                if (gameObject.name == "PlayerPoison(Clone)" && GameObject.Find("PoisonR").GetComponent<Image>().fillAmount == 1)
                {
                    GameObject.Find("PoisonR").GetComponent<Image>().fillAmount = 0;
                    if (Player == 1)
                        PhotonNetwork.Instantiate("PoisonR", new Vector3(0, 2.75f, 0), Quaternion.identity);
                    if (Player == -1)
                        PhotonNetwork.Instantiate("PoisonR", new Vector3(0, -1.75f, 0), Quaternion.identity);
                }
                if (gameObject.name == "PlayerHydra(Clone)" && GameObject.Find("HydraR").GetComponent<Image>().fillAmount == 1 && Hydracount < 5)
                {
                    GameObject.Find("HydraR").GetComponent<Image>().fillAmount = 0;
                    Hydracount++;
                    PV.RPC("Sound", RpcTarget.AllBuffered);
                }
            }



            if (gameObject.name == "PlayerHydra(Clone)" && Hydracount == 1)
                PV.RPC("HydraCount1RPC", RpcTarget.AllBuffered);
            if (gameObject.name == "PlayerHydra(Clone)" && Hydracount == 2)
                PV.RPC("HydraCount2RPC", RpcTarget.AllBuffered);
            if (gameObject.name == "PlayerHydra(Clone)" && Hydracount == 3)
                PV.RPC("HydraCount3RPC", RpcTarget.AllBuffered);
            if (gameObject.name == "PlayerHydra(Clone)" && Hydracount == 4)
                PV.RPC("HydraCount4RPC", RpcTarget.AllBuffered);
            if (gameObject.name == "PlayerHydra(Clone)" && Hydracount == 5)
                PV.RPC("HydraCount5RPC", RpcTarget.AllBuffered);
            if (IsDarkE == true)
            {
                PV.RPC("ColorRPCBlack", RpcTarget.AllBuffered);
            }
            else if (slow < 1)
            {
                PV.RPC("ColorRPCSlow", RpcTarget.AllBuffered);
            }
            else if (IsPoison == true)
            {
                PV.RPC("ColorRPCPoison", RpcTarget.AllBuffered);
            }
            else
                PV.RPC("ColorRPCWhite", RpcTarget.AllBuffered);



            float axis = Input.GetAxisRaw("Horizontal") * flipX;
            if (axis != 0)
            {
                PV.RPC("FlipX", RpcTarget.AllBuffered, axis);
            }
            if (Input.GetButton("Horizontal") && canWalk == true)
            {
                transform.Translate(Input.GetAxisRaw("Horizontal") * Time.deltaTime * 7 * slow * Speed, 0, 0);
            }
            if (Input.GetButton("Vertical") && canWalk == true)
            {
                transform.Translate(0, Input.GetAxisRaw("Vertical") * Time.deltaTime * 7 * slow * Speed, 0);
            }
        }
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else
        {
            HealthBar.fillAmount = 0;
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 50);
        }
    }
    [PunRPC]
    void Sound()
    {
        AS.Play();
    }


    [PunRPC]
    void Game()
    {
        if (PhotonNetwork.PlayerList.Length == 1)
        {
            gameManager.Winner = PhotonNetwork.LocalPlayer.NickName;
            gameManager.IsEnd = true;
            PhotonNetwork.Destroy(gameObject);
        }
        if (Health <= 0 | gameManager.IsEnd == true)
        {
            if (PhotonNetwork.PlayerList.Length != 1)
            {
                if (PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.LocalPlayer.NickName)
                {
                    gameManager.Winner = PhotonNetwork.PlayerList[1].NickName;
                    Debug.Log("Player 1 승리");
                    PhotonNetwork.Destroy(gameObject);
                    gameManager.IsEnd = true;
                }
                else
                {
                    gameManager.Winner = PhotonNetwork.PlayerList[0].NickName;
                    Debug.Log("Player 0 승리");
                    PhotonNetwork.Destroy(gameObject);
                    gameManager.IsEnd = true;
                }
            }
        }
    }
    [PunRPC]
    void FlipX(float axis)
    {
        if (axis == -1)
            SR.flipX = true;
        else
            SR.flipX = false;
    }

    [PunRPC]
    void HydraCount1RPC()
    {
        SR.sprite = Hydra1;
    }
    [PunRPC]
    void HydraCount2RPC()
    {
        SR.sprite = Hydra2;
    }
    [PunRPC]
    void HydraCount3RPC()
    {
        SR.sprite = Hydra3;
    }
    [PunRPC]
    void HydraCount4RPC()
    {
        SR.sprite = Hydra4;
    }
    [PunRPC]
    void HydraCount5RPC()
    {
        SR.sprite = Hydra5;
    }
    [PunRPC]
    void DarkRRPC()
    {
        DarkR.SetActive(true);
        DarkR.GetComponent<AudioSource>().Play();
    }

    [PunRPC]
    void SoundQ()
    {
        Q.GetComponent<AudioSource>().Play();
    }
    [PunRPC]
    void SoundW()
    {
        W.GetComponent<AudioSource>().Play();
    }
    [PunRPC]
    void SoundE()
    {
        E.GetComponent<AudioSource>().Play();
    }
    [PunRPC]
    void SoundR()
    {
        R.GetComponent<AudioSource>().Play();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }
}
