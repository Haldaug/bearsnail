using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        jumperPosition = jumper.transform.localPosition;

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Intro")
        {
            playerMoveType = 2;
            animator.Play("EnterShell");

            foreach (var part in bearParts)
            {
                part.GetComponent<Rigidbody2D>().isKinematic = true;
                part.GetComponent<Collider2D>().enabled = false;
                part.GetComponent<HingeJoint2D>().enabled = false;
                part.GetComponent<Rigidbody2D>().Sleep();
            }
        }
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

        if (shellRb2d.transform.position.y < -12)
        {
            Debug.Log("RESTART");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

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

                if (horizontalKeys != 0)
                {
                    animator.Play("Crawling", 1);
                    animator.SetFloat("crawlSpeed", horizontalKeys);

                    foreach (var part in bearParts)
                    {
                        if (part.name.Contains("Torso"))
                        {
                            var rb2d = part.GetComponent<Rigidbody2D>();

                            rb2d.AddForce(Vector2.up * 10);

                            //part.transform.RotateAround(part.GetComponent<HingeJoint2D>().anchor, Vector2.Angle(transform.up, transform.right)/ 100);
                        }
                    }
                }



                bearRb2d.AddForce(Vector3.right * crawlSpeed * horizontalKeys + Vector3.up * shellRb2d.velocity.y);

                if (onGround == true && horizontalKeys != 0)
                {
                    //transform.LookAt(Vector2.Lerp(transform.position, playerPosition + rb2d.velocity.normalized * 2, 0.5f), Vector3.up);


                }

                // Entering shell
                if (jumpKey == true)
                {
                    playerMoveType = 2;
                    animator.Play("EnterShell");

                    foreach (var part in bearParts)
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
                        var old = part.GetComponent<HingeJoint2D>();

                        if (!part.name.Contains("Arm"))
                        {
                            var joint = part.gameObject.AddComponent<HingeJoint2D>();
                            joint.anchor = old.anchor;
                            joint.connectedAnchor = old.connectedAnchor;
                            joint.useLimits = old.useLimits;
                            joint.limits = old.limits;
                            joint.connectedBody = old.connectedBody;
                            Destroy(old);
                        }



                        /*var oldMin = part.GetComponent<HingeJoint2D>().limits.min;
                        //var oldMax = part.GetComponent<HingeJoint2D>().limits.max;
                        //var refAngle = part.GetComponent<HingeJoint2D>().referenceAngle;
                        //var jointAngle = part.GetComponent<HingeJoint2D>().jointAngle;

                        //print(part.name + " oldMin: " + oldMin + "(" + WrapAngle(oldMin) + "). oldMax: " + oldMax + "(" + WrapAngle(oldMax) + "). refAngle: " + refAngle + ". jointAngle: " + jointAngle);

                        //JointAngleLimits2D limits = new JointAngleLimits2D
                        //{
                        //    min = jointAngle + WrapAngle(oldMin),
                        //    max = jointAngle + WrapAngle(oldMax)
                        //};

                        //part.GetComponent<HingeJoint2D>().limits = limits;*/



                        //part.GetComponent<HingeJoint2D>().limits.max = jointAngle + oldMin;

                        //print("JointAngle: " + part.GetComponent<HingeJoint2D>().jointAngle + ". wrap: " + WrapAngle(part.GetComponent<HingeJoint2D>().jointAngle));
                        //print("ReferenceAngle: " + part.GetComponent<HingeJoint2D>().referenceAngle);
                        //print("euler: " + part.transform.eulerAngles.z);
                        //part.transform.eulerAngles = new Vector3(part.transform.eulerAngles.x, part.transform.eulerAngles.y, part.transform.eulerAngles.z);
                        //.RotateAround(part.transform.parent.position, part.transform.eulerAngles.z);
                        //part.GetComponent<HingeJoint2D>().jointAngle = WrapAngle(part.GetComponent<HingeJoint2D>().jointAngle);
                    }
                    print("Exiting shell");

                    jumpDuration = 5;
                    StartCoroutine(ExitingTimer());

                }

                break;
        }


        if (jumpDuration > 0)
        {
            print("jumping");

            jumperJoint.useLimits = false;
            jumpDuration -= 1;
            jumperJoint.GetComponent<Rigidbody2D>().mass = 1;
        }
        else
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
        yield return new WaitForSeconds(0.3f);
        foreach (var part in bearParts)
        {
            part.GetComponent<Collider2D>().enabled = true;
        }
    }

    private static float WrapAngle(float angleInDegrees)
    {
        if (angleInDegrees >= 360f)
        {
            return angleInDegrees - (360f * (int)(angleInDegrees / 360f));
        }
        else if (angleInDegrees >= 0f)
        {
            return angleInDegrees;
        }
        else
        {
            float f = angleInDegrees / -360f;
            int i = (int)f;
            if (f != i)
                ++i;
            return angleInDegrees + (360f * i);
        }
    }
}
