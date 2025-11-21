using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;


public class Attack : MonoBehaviourPunCallbacks
{
    public int Player;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Sprite DarkQ;
    public Sprite PoisonQ;
    public Sprite HydraQ;
    public Sprite DarkW1;
    public Sprite DarkW2;
    public Sprite DarkW3;
    public Sprite PoisonW;
    public float PoisonWSpeed = 1;
    public Sprite HydraW;
    public float PlusDamage;
    public AudioSource AS;
    public int IsMine;




    void Start()
    {
        PoisonWSpeed = 1;
        if (PV.IsMine && PhotonNetwork.PlayerList[0].NickName != PhotonNetwork.LocalPlayer.NickName)
            PV.RPC("FlipY", RpcTarget.AllBuffered);

        if (PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.LocalPlayer.NickName)
            Player = 1;
        //Player[0]
        else
            Player = -1;
        //Player[1]
        if (PV.IsMine)
            IsMine = 1;
        else
            IsMine = -1;


        if (gameObject.name == "DarkQ(Clone)")
        {
            SR.sprite = DarkQ;
        }
        if (gameObject.name == "PoisonQ(Clone)")
        {
            SR.sprite = PoisonQ;
        }
        if (gameObject.name == "HydraQ(Clone)")
        {
            transform.localScale = new Vector3(3 + (GameObject.Find("GameManager").GetComponent<GameManager>().gameManager.HydraCount * 0.35f), 3 + (GameObject.Find("GameManager").GetComponent<GameManager>().gameManager.HydraCount * 0.35f), 1);
            SR.sprite = HydraQ;
        }
        if (gameObject.name == "DarkW(Clone)")
        {
            SR.sprite = DarkW1;
            InvokeRepeating("DarkW", 0.5f, 0.5f);

        }
        if (gameObject.name == "PoisonW(Clone)")
        {
            SR.sprite = PoisonW;
        }
        if (gameObject.name == "HydraW(Clone)")
        {
            SR.sprite = HydraW;
        }



    }

    void DarkW()
    {
        if (SR.sprite == DarkW1)
        {
            SR.sprite = DarkW2;
            transform.localScale = new Vector3(4, 4, 1);
        }
        if (SR.sprite == DarkW2)
        {
            SR.sprite = DarkW3;
            transform.localScale = new Vector3(5, 5, 1);
        }
        if (SR.sprite == DarkW3)
        {
            CancelInvoke("DarkW");
        }
    }
    [PunRPC]
    void PlusDamaged()
    {
        PlusDamage = 2;
    }

    [PunRPC]
    void NotPlusDamaged()
    {
        PlusDamage = 1;
    }

    void Update()
    {
        if (gameObject.name == "PoisonW(Clone)" && transform.position.y < -3.2f)
        {
            PoisonWSpeed = 0;
            Invoke("Destroy", 2.5f);
            transform.localScale = new Vector3(6, 6, 1);
        }
        if (gameObject.name == "PoisonW(Clone)" && transform.position.y > 4.2f)
        {
            PoisonWSpeed = 0;
            Invoke("Destroy", 2.5f);
            transform.localScale = new Vector3(6, 6, 1);
        }


        if (gameObject.name == "DarkQ(Clone)")
            transform.Translate(0, 12 * Player * IsMine * Time.deltaTime, 0);
        if (gameObject.name == "PoisonQ(Clone)")
            transform.Translate(0, 20 * Player * IsMine * Time.deltaTime, 0);
        if (gameObject.name == "HydraQ(Clone)")
        {
            transform.Translate(0, 10 * Player * IsMine * Time.deltaTime, 0);
        }
        if (gameObject.name == "DarkW(Clone)")
        {
            transform.Translate(0, 10 * Player * IsMine * Time.deltaTime, 0);
        }
        if (gameObject.name == "PoisonW(Clone)")
        {
            transform.Translate(0, 16 * PoisonWSpeed * Player * IsMine * Time.deltaTime, 0);
        }
        if (gameObject.name == "HydraW(Clone)")
        {
            transform.Translate(0, 12 * Player * IsMine * Time.deltaTime, 0);
        }

        if (GameObject.Find("GameManager").GetComponent<GameManager>().gameManager.IsDarkR && PV.IsMine)
        {
            PV.RPC("PlusDamaged", RpcTarget.AllBuffered);
        }
        else
            PV.RPC("NotPlusDamaged", RpcTarget.AllBuffered);


    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PV.IsMine && collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine && !collision.GetComponent<PlayerScript>().IsDarkE)
        {
            if (gameObject.name == "DarkQ(Clone)")
            {
                collision.GetComponent<PlayerScript>().Damaged(10 * PlusDamage);
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
            if (gameObject.name == "PoisonQ(Clone)")
            {
                collision.GetComponent<PlayerScript>().InvokeRepeating("Poison", 0, 3);
                collision.GetComponent<PlayerScript>().Damaged(5);
                collision.GetComponent<PlayerScript>().Slow();
                collision.GetComponent<PlayerScript>().Invoke("NotSlow", 3);
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
            if (gameObject.name == "HydraQ(Clone)")
            {
                collision.GetComponent<PlayerScript>().Damaged(10 + (GameObject.Find("GameManager").GetComponent<GameManager>().gameManager.HydraCount * 2));
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
            if (gameObject.name == "DarkW(Clone)")
            {
                collision.GetComponent<PlayerScript>().Damaged(15 * PlusDamage);
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
            if (gameObject.name == "PoisonW(Clone)")
            {
                collision.GetComponent<PlayerScript>().InvokeRepeating("Poison", 0, 3);
                collision.GetComponent<PlayerScript>().Damaged(10);
                collision.GetComponent<PlayerScript>().Slow();
                PoisonWSpeed = 0;
                transform.localScale = new Vector3(6, 6, 1);
                Invoke("Destroy", 2.5f);
            }
            if (gameObject.name == "HydraW(Clone)")
            {
                collision.GetComponent<PlayerScript>().Damaged(15);
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
            if (gameObject.name == "PoisonE(Clone)")
            {
                collision.GetComponent<PlayerScript>().PoisonE();
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
            if (gameObject.name == "PoisonR(Clone)")
            {
                collision.GetComponent<PlayerScript>().PoisonDamage = true;
                collision.GetComponent<PlayerScript>().Invoke("NotPoison", 8);
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }

        }
        if (!PV.IsMine && collision.tag == "Wall" && gameObject.name != "PoisonE(Clone)" && gameObject.name != "PoisonR(Clone)" && gameObject.name != "PoisonW(Clone)")
        {
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }

    void Destroy()
    {
        PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!PV.IsMine && collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            if (gameObject.name == "PoisonW(Clone)")
            {
                collision.GetComponent<PlayerScript>().Invoke("NotSlow", 1);
                PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }
    [PunRPC]
    void FlipY()
    {
        if (gameObject.name == "PoisonQ(Clone)")
        {
            SR.flipY = true;
        }
    }
}
