using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : Elements
{
    //change to the element we want to start with in editor
    public elements element = 0;
    elements platformElement;

    SpriteRenderer p_spriteRenderer;
    bool changedSprite = false;

    void Start()
    {
        //temp solution changing colour until we get sprites/anims
        p_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            element = elements.fire;
            Debug.Log("Enabled Fire");
            changedSprite = false;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            element = elements.water;
            Debug.Log("Enabled Water");
            changedSprite = false;
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            element = elements.earth;
            Debug.Log("Enabled Earth");
            changedSprite = false;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            element = elements.air;
            Debug.Log("Enabled Air");
            changedSprite = false;
        }

        if(element == elements.fire && !changedSprite)
        {
            p_spriteRenderer.color = Color.red;
            changedSprite = true;
        }

        if (element == elements.water && !changedSprite)
        {
            p_spriteRenderer.color = Color.blue;
            changedSprite = true;
        }

        if (element == elements.earth && !changedSprite)
        {
            p_spriteRenderer.color = Color.green;
            changedSprite = true;
        }

        if (element == elements.air && !changedSprite)
        {
            p_spriteRenderer.color = Color.gray;
            changedSprite = true;
        }
    }
}
