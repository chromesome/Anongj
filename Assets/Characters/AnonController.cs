using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class AnonController : MonoBehaviour
{
    [SerializeField] GameObject playerCanvas;
    [SerializeField] Text gamerTag;
    [SerializeField] PhotonView photonView;
    
    float runSp;
    Rigidbody2D rb;
    Collider2D coll;

    AnonAnimator aAnim;

    int shape;

    private void Awake()
    {
        setGameTag(PhotonNetwork.NickName);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        aAnim = GetComponent<AnonAnimator>();

        Global gl = GameObject.FindWithTag("Global").GetComponent<Global>();
        aAnim.body.GetComponent<SpriteRenderer>().sprite = gl.shapes[Random.Range(0,4)];
        aAnim.face.GetComponent<SpriteRenderer>().sprite = gl.faces[Random.Range(2,5)];
        runSp = gl.runSp;
    }

    void Update()
    {
        if(photonView.IsMine)
        {
            Vector2 mov = new Vector2 (Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
            rb.velocity = mov.normalized * runSp;
            aAnim.mov = mov.normalized;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeShape();
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

    void setGameTag(string nickName)
    {
        if(photonView.IsMine)
        {
            if(gamerTag)
                gamerTag.text = nickName;

            if(playerCanvas)
                playerCanvas.SetActive(true);

        }
        else
        {
            if(gamerTag)
                gamerTag.text = photonView.Owner.NickName;

            if(playerCanvas)
                playerCanvas.SetActive(true); // TODO ocultar en la versión final
        }
    }
}
