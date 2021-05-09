using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnonAnimator : MonoBehaviour
{
    public Vector2 mov;
    Animator anim;
    SpriteRenderer spr;
    void Start()
    {
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (mov.x!=0f){
        	if (mov.x>0f){
        		spr.flipX = false;
        	} else {
        		spr.flipX = true;	
        	}
        }
        if (mov.magnitude>0f){
        	anim.SetBool("moving", true);
    	} else {
    		anim.SetBool("moving", false);
    	}
    }
}
