using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class AnonController : MonoBehaviour, IPunObservable, IKillable
{
    [SerializeField] GameObject playerCanvas;
    [SerializeField] Text gamerTag;
    [SerializeField] PhotonView photonView;
    [SerializeField] GameObject playerCamera;

    public bool isAlive = true;
    
    float runSp;
    Rigidbody2D rb;
    Collider2D coll;

    AnonAnimator aAnim;

    int shape;

    public delegate void KilledAnomie(string gamerTag);
    public static event KilledAnomie OnKillPlayer;

    private void Awake()
    {
        setGameTag(PhotonNetwork.NickName);
    }

    void Start()
    {
        if(photonView.IsMine)
        {
            playerCamera.SetActive(true);
        }

        shape = Random.Range(0,4);

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        aAnim = GetComponent<AnonAnimator>();

        Global gl = GameObject.FindWithTag("Global").GetComponent<Global>();
        aAnim.body.GetComponent<SpriteRenderer>().sprite = gl.shapes[shape];
        aAnim.face.GetComponent<SpriteRenderer>().sprite = gl.faces[Random.Range(2,5)];
        runSp = gl.runSp;
    }

    void Update()
    {
        if(photonView.IsMine && isAlive)
        {
            Vector2 mov = new Vector2 (Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
            rb.velocity = mov.normalized * runSp;
            aAnim.mov = mov.normalized;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeShape();
            }

            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeShape(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeShape(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeShape(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangeShape(3);
            }
        }
        else
        {
            if(!isAlive)
            {
                Global gl = GameObject.FindWithTag("Global").GetComponent<Global>();
                aAnim.face.GetComponent<SpriteRenderer>().sprite = gl.faces[0];
            }

        }
    }
    void ChangeShape()
    {
        Global gl = GameObject.FindWithTag("Global").GetComponent<Global>();
        shape++;
        if (shape>3)
        {
            shape = 0;
        }
        aAnim.body.GetComponent<SpriteRenderer>().sprite = gl.shapes[shape];
    }

    void ChangeShape(int skinId)
    {
        Global gl = GameObject.FindWithTag("Global").GetComponent<Global>();
        shape = skinId;
        aAnim.body.GetComponent<SpriteRenderer>().sprite = gl.shapes[shape];
    }

    void setGameTag(string nickName)
    {
        if(photonView.IsMine)
        {
            if(gamerTag){
                //gamerTag.text = nickName;
            }

            if(playerCanvas){
                //playerCanvas.SetActive(true);
            }

        }
        else
        {
            if(gamerTag){
                //gamerTag.text = photonView.Owner.NickName;
            }

            if(playerCanvas){
                //playerCanvas.SetActive(true); // TODO ocultar en la versión final
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if(stream.IsReading)
        {
            shape = (int)stream.ReceiveNext();
            Global gl = GameObject.FindWithTag("Global").GetComponent<Global>();
            aAnim.body.GetComponent<SpriteRenderer>().sprite = gl.shapes[shape];
            //aAnim.face.GetComponent<SpriteRenderer>().sprite = gl.faces[Random.Range(2, 5)];
        }
        else
        {
            stream.SendNext(shape);
        }
    }

    [PunRPC]
    public void Kill()
    {
        if(isAlive)
        {
            Debug.Log("Killed player " + photonView.Owner.NickName);
            isAlive = false;
            OnKillPlayer(PhotonNetwork.NickName);
        }
    }
}
