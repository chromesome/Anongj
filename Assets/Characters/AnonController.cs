using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnonController : MonoBehaviour
{
    private PhotonView PV;
    
    float runSp;
    Rigidbody2D rb;
    Collider2D coll;

    AnonAnimator aAnim;

    int shape;

    void Start()
    {
        PV = GetComponent<PhotonView>();
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
        if(PV.IsMine)
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
}
