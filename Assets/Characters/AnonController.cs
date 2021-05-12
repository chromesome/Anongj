using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnonController : MonoBehaviour
{
    public float runSp;

    Rigidbody2D rb;
    public Collider2D coll;

    AnonAnimator aAnim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        aAnim = GetComponent<AnonAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mov = new Vector2 (Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
    	rb.velocity = mov.normalized * runSp;
    	aAnim.mov = mov.normalized;
    }
}
