﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    // Components
    public Rigidbody2D shellRb2d;
    private LayerMask groundMask;
    private AudioManager audioManager;

    // Variables
    private bool onGround;


    // Start is called before the first frame update
    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        groundMask = LayerMask.GetMask("Ground");

    }

    // Update is called once per frame
    void Update()
    {
        if(shellRb2d.velocity.magnitude > 1 && onGround)
        {
            print("sound");
            audioManager.Play("roll");
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