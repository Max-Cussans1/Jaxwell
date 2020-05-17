using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    Animator animator;
    public static bool updateHealthUI = false;
    public static bool lifeGained = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (lifeGained)
        {
            animator.SetTrigger("GainedLife");
            lifeGained = false;
        }
        if (updateHealthUI)
        {
            animator.SetInteger("Lives", Health.lives);
        }
    }
}
