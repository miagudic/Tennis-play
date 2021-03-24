using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform aimTarget;
    float speed = 3f;
    float force = 13;

    bool hitting;

    public Transform ball;
    Animator animator;

    Vector3 aimTargetInitialPosition;

    ShotManager shotManager;
    Shot currentShot;

   private void Start()
    {
        animator = GetComponent<Animator>();
        aimTargetInitialPosition = aimTarget.position;
        shotManager = GetComponent<ShotManager>();
        currentShot = shotManager.topSpin;
    }

   

    
    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        

        if( Input.GetKeyDown(KeyCode.F))
        {
            hitting = true;  
            currentShot = shotManager.topSpin;
        }else if (Input.GetKeyUp(KeyCode.F))
        {
            hitting = false;
        }

        if( Input.GetKeyDown(KeyCode.E))
        {
            hitting = true;  
            currentShot = shotManager.flat;
        }else if (Input.GetKeyUp(KeyCode.E))
        {
            hitting = false;
        }

        if( Input.GetKeyDown(KeyCode.R))
        {
            hitting = true;  
            currentShot = shotManager.flatServe;
        }else if (Input.GetKeyUp(KeyCode.R))
        {
            hitting = false;
            ball.transform.position = transform.position + new Vector3(0.2f, 1, 0);
            Vector3 dir = aimTarget.position - transform.position;
            ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
        }

        if(hitting)
        {
            aimTarget.Translate( new Vector3(h, 0, 0 ) * speed * 2 * Time.deltaTime );
        }

        if( (h != 0 || v != 0) && !hitting)
        {
            transform.Translate( new Vector3(h, 0, v) * speed * Time.deltaTime );
        }

        Vector3 ballDir = ball.position - transform.position;
            if(ballDir.x >= 0)
            {
                Debug.Log("forehand");
            }
            else
            {   
                 Debug.Log("backhand");
            } 
            Debug.DrawRay(transform.position, ballDir);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            Vector3 dir = aimTarget.position - transform.position;
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);

            Vector3 ballDir = ball.position - transform.position;
            if(ballDir.x >= 0)
            {
                animator.Play("forehand");
                //Debug.Log("forehand");
            }
            else
            {   
                 animator.Play("backhand");
                 //Debug.Log("backhand");
            }    

            aimTarget.position = aimTargetInitialPosition;
        }

    }
}
