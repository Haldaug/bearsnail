using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_PlayerControls : MonoBehaviour
{

    // Stats
    public float crawlSpeed;
    public float rollSpeed;
    public float jumpPower;
    public float jumpDuration;



    // Set variables
    public int playerMoveType = 1;
    private bool onGround;
    private Vector2 groundHitPosition;

    // Controls
    public float horizontalKeys;
    private bool jumpKey;

    // Components
    public Rigidbody2D shellRb2d;
    public Rigidbody2D bearRb2d;
    private LayerMask groundMask;
    private Animator animator;
    public Collider2D shellCollider;
    public GameObject bearParent;
    public GameObject velocityPointer;
    public Rigidbody2D jumper;
    private Vector2 jumperPosition;
    private SliderJoint2D jumperJoint;

    private List<Collider2D> bearParts = new List<Collider2D>();

    private void Start()
    {
        jumperJoint = jumper.GetComponent<SliderJoint2D>();
        groundMask = LayerMask.GetMask("Ground");
        animator = GetComponent<Animator>();
        bearParts.AddRange(bearParent.GetComponentsInChildren<Collider2D>());

        playerMoveType = 1;
        jumperPosition = jumper.transform.localPosition;
    }



    private void Update()
    {
        //Controls
        horizontalKeys = Input.GetAxis("Horizontal Movement");
        jumpKey = Input.GetButtonDown("Jump");
    }


    void FixedUpdate()
    {
        var playerPosition = new Vector2(transform.position.x, transform.position.y);
        
        /*
        // Ground Detection
        var groundHit = Physics2D.Raycast(transform.position, -Vector2.up, 1.9f, groundMask, 1, 1);

        print(groundHit.transform);

        if(groundHit.transform != null)
        {
            onGround = true;
        } else
        {
            onGround = false;
        }*/



        // Snail bear's types of movement
        switch (playerMoveType)
        {


            // CRAWLING
            case 1:
                /*
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    foreach (var part in bearParts)
                    {
                        part.GetComponent<HingeJoint2D>().enabled = true;
                    }
                }*/



                bearRb2d.AddForce(Vector3.right * crawlSpeed * horizontalKeys + Vector3.up * shellRb2d.velocity.y);

                if (onGround == true && horizontalKeys != 0)
                {
                    //transform.LookAt(Vector2.Lerp(transform.position, playerPosition + rb2d.velocity.normalized * 2, 0.5f), Vector3.up);

                    
                }
                
                // Entering shell
                if(jumpKey == true)
                {
                    playerMoveType = 2;
                    animator.Play("EnterShell");

                    foreach(var part in bearParts)
                    {
                        part.GetComponent<Rigidbody2D>().isKinematic = true;
                        part.GetComponent<Collider2D>().enabled = false;
                        part.GetComponent<HingeJoint2D>().enabled = false;
                        part.GetComponent<Rigidbody2D>().Sleep();
                    }

                    
                }

                /*
                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
                {

                    foreach (var part in bearParts)
                    {
                        part.GetComponent<Collider2D>().enabled = true;
                    }
                }
                */

                /*

                    var setRotationEuler = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.right, rb2d.velocity.normalized), 0.5f).ToEuler();

                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, setRotationEuler.z));
                }*/
                break;


            // ROLLING
            case 2:
                //velocityPointer.transform.rotate(Vector3.forward);///*transform.rotation.ToEuler() + Quaternion.FromToRotation(*/velocityPointer.transform.right, rb2d.velocity*100/*).ToEuler()*/);



                shellRb2d.AddForce(Vector3.right * rollSpeed * horizontalKeys);

                // Jumping
                /*if(onGround == true && jumpKey)
                {
                    var velocityPerp = Vector2.Perpendicular(shellRb2d.velocity.normalized) * Mathf.Sign(shellRb2d.velocity.x);

                    shellRb2d.velocity = shellRb2d.velocity + velocityPerp * jumpPower;

                }*/


                // Exiting shell
                if (jumpKey == true)//(shellRb2d.velocity.magnitude < 0.5f && onGround == true)
                {
                    playerMoveType = 1;
                    animator.Play("ExitShell");




                    foreach (var part in bearParts)
                    {
                        part.GetComponent<Collider2D>().enabled = false;
                        part.GetComponent<Rigidbody2D>().isKinematic = false;
                        part.GetComponent<Rigidbody2D>().WakeUp();
                        
                        part.GetComponent<HingeJoint2D>().enabled = true;
                    }
                    print("Exiting shell");

                    jumpDuration = 5;
                    StartCoroutine(ExitingTimer());

                }

                break;
        }


        if(jumpDuration > 0)
        {
            print("jumping");

            jumperJoint.useLimits = false;
            jumpDuration -= 1;
            jumperJoint.GetComponent<Rigidbody2D>().mass = 1;
        } else
        {
            jumperJoint.useLimits = true;
            jumperJoint.GetComponent<Rigidbody2D>().mass = 0;
        }

        if (jumpDuration == 1)
        {

        }

    }



    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            onGround = true;
            groundHitPosition = collision.GetContact(0).point;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            onGround = false;
        }
    }


    IEnumerator ExitingTimer()
    {
        yield return new WaitForSeconds(3);
        foreach (var part in bearParts)
        {
            part.GetComponent<Collider2D>().enabled = true;
        }
    }
}
