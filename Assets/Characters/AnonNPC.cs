using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnonNPC : MonoBehaviourPun, IPunObservable, IKillable
{
    float runSp;

    Rigidbody2D rb;
	Collider2D coll;
    AnonAnimator aAnim;

    public int movType;
    Vector2 mov;

    public bool isAlive = true;

    // Start is called before the first frame update
    void Awake()
    {
        movType = Random.Range(0,4);

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        aAnim = GetComponent<AnonAnimator>();

        Global gl = GameObject.FindWithTag("Global").GetComponent<Global>();
        aAnim.body.GetComponent<SpriteRenderer>().sprite = gl.shapes[movType];
        aAnim.face.GetComponent<SpriteRenderer>().sprite = gl.faces[Random.Range(0, 5)];
        runSp = gl.runSp;
        
    }

    private void Start()
    {
        if(photonView.IsMine)
        {
            EmpezarMovimiento();
            TurnRandom();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //aAnim.mov = mov.normalized;
        if(!isAlive)
        {
            rb.velocity = Vector2.zero;
            Global gl = GameObject.FindWithTag("Global").GetComponent<Global>();
            aAnim.face.GetComponent<SpriteRenderer>().sprite = gl.faces[0];
        }
    }

    [PunRPC]
    void EmpezarMovimiento(){
    	int variar = 0;
    	int intentos = 0;
    	do {
    		Random.Range(-1,2);
    		intentos++;
    		if (intentos>10){
    			variar = 1;
    		}
    	}while(variar == 0);

    	switch(movType){
    		case 0:{
    			mov = new Vector2 (variar,0f) * runSp;
    			break;
    		}
    		case 1:{
    			mov = new Vector2 (0f,variar) * runSp;
    			break;
    		}
    		case 2:{
    			mov = new Vector2 (-1f,1f) * variar;
    			mov = mov.normalized*runSp;
    			break;
    		}
    		case 3:{
    			mov = new Vector2 (1f,1f) * variar;
    			mov = mov.normalized*runSp;
    			break;
    		}
    	}
    	rb.velocity = mov;
    }

    void Voltear(){
        if(rb.velocity == Vector2.zero)
        {
    	    mov = mov*-1;
    	    rb.velocity = mov;
            aAnim.mov = mov.normalized;
        }
        else
        {
            rb.velocity = Vector2.zero;
            aAnim.mov = Vector2.zero.normalized;
        }
    }
    void OnCollisionEnter2D(){
    	Voltear();
    }
    void TurnRandom(){
        if (isAlive)
        {
    	    Voltear();
    	    Invoke("TurnRandom",Random.Range(0f,3f));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if(stream.IsReading)
        {
            movType = (int)stream.ReceiveNext();
            Global gl = GameObject.FindWithTag("Global").GetComponent<Global>();
            aAnim.body.GetComponent<SpriteRenderer>().sprite = gl.shapes[movType];
            //aAnim.face.GetComponent<SpriteRenderer>().sprite = gl.faces[Random.Range(2, 5)];
        }
        else
        {
            stream.SendNext(movType);
        }
    }

    [PunRPC]
    public void Kill()
    {
        isAlive = false;
    }
}
