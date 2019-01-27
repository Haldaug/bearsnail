using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    public Transitions transitions;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = transitions.GetComponent<Animator>();

        animator.Play("EnterScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        transitions.switchScene = true;

    }
}
