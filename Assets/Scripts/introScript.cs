using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introScript : MonoBehaviour
{

    private Animator animator;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("EnterShell");
        audioManager = GetComponent<AudioManager>();
        audioManager.Play("themesong");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
