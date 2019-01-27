using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitions : MonoBehaviour
{
    public bool switchScene;
    private Animator animator;
    private SceneManager scene;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        animator.Play("EnterScene");
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("NextScene") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            SceneManager.LoadScene("Level 1");
        }

        if (switchScene == true)
        {
            animator.Play("NextScene");
        }
    }
}
