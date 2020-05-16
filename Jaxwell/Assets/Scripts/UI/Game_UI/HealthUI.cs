using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    Animator animator;
    public static bool updateHealthUI = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if(updateHealthUI)
        {
            animator.SetInteger("Lives", Health.lives);
        }
    }
}
