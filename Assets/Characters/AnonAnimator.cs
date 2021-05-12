using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnonAnimator : MonoBehaviour
{
    public Vector2 mov;

    public float stretch;
    public float squash;
    public float faceOffset;

    public GameObject body;
    public GameObject face;
    public Transform facePosition;
    void Start()
    {

        Global gl = GameObject.FindWithTag("Global").GetComponent<Global>();
        body.GetComponent<SpriteRenderer>().sprite = gl.shapes[Random.Range(0,4)];
        face.GetComponent<SpriteRenderer>().sprite = gl.faces[Random.Range(2,5)];
    }

    void Update()
    {
        AnimationValues();
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
}
