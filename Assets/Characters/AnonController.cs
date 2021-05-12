using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnonController : MonoBehaviour
{
    private PhotonView PV;


    public float runSp;

    Rigidbody2D rb;
    public Collider2D coll;

    AnonAnimator aAnim;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        aAnim = GetComponent<AnonAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PV.IsMine)
        {
            Vector2 mov = new Vector2 (Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
            rb.velocity = mov.normalized * runSp;
            aAnim.mov = mov.normalized;
        }
    }
}
