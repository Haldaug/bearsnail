﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    // Components
    public Rigidbody2D shellRb2d;

    private AudioManager audioManager;

    // Variables
    private bool onGround;
    private bool jumpKey;


    // Start is called before the first frame update
    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        //audioManager.Play("themesong");
    }

    // Update is called once per frame
    void Update()
    {

        jumpKey = Input.GetButtonDown("Jump");

        if (shellRb2d.velocity.magnitude > 1.5 && onGround)
        {
            print("sound");
            audioManager.Play("roll");
        }

        if(jumpKey)
        {
            audioManager.Play("vocal");
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 2)
        {
            audioManager.Play("bump");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            onGround = false;
        }
    }
}
