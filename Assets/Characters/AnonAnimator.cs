using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnonAnimator : MonoBehaviourPun, IPunObservable
{
    public Vector2 mov;
    public int currentIndex;

    public float stretch;
    public float squash;
    public float faceOffset;

    public GameObject body;
    public GameObject face;
    public Transform facePosition;
    void Start()
    {

        //Global gl = GameObject.FindWithTag("Global").GetComponent<Global>();
        //body.GetComponent<SpriteRenderer>().sprite = gl.shapes[Random.Range(0,4)];
        //face.GetComponent<SpriteRenderer>().sprite = gl.faces[Random.Range(2,5)];
    }

    void Update()
    {
        AnimationValues();

        if(photonView.IsMine)
        {
            transform.position = new Vector3 (transform.position.x,transform.position.y,transform.position.y);
        }
    }

    void AnimationValues(){
        if (mov.x!=0f){
            body.transform.localScale = new Vector3(1f+Mathf.Abs(mov.x)*stretch,1f-Mathf.Abs(mov.x)*squash,1f);
        } else {
            body.transform.localScale = new Vector3(1f,1f,1f);
        }
        if (mov.magnitude>0f){
            face.transform.position = facePosition.position +  new Vector3 (mov.x, mov.y, 0f)*faceOffset;
        } else {
            face.transform.position = facePosition.position;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Esto no le gusta, mejor hacerlo en AnonNPC (y AnonController) o pasarle el movType y actualizar el skin con el index
        if (stream.IsWriting)
        {
            stream.SendNext(currentIndex);
            stream.SendNext(mov);
            //stream.SendNext(body.transform.localScale);
        }
        else
        {
            currentIndex = (int)stream.ReceiveNext();
            mov = (Vector2)stream.ReceiveNext();
        }
    }
}
