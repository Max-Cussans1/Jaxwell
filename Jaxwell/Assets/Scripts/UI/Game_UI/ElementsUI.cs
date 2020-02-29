using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementsUI : Elements
{
    public PlayerState playerState;
    public DashScript dashScript;

    public GameObject movingRightArrow;
    public GameObject movingLeftArrow;

    public GameObject fire;
    public GameObject water;
    public GameObject earth;
    public GameObject air;

    public Image dashCooldown;
    public static bool beginDashCD = false;

    Outline fireActive;
    Outline waterActive;
    Outline earthActive;
    Outline airActive;

    void Start()
    {
        fireActive = fire.GetComponent<Outline>();
        waterActive = water.GetComponent<Outline>();
        earthActive = earth.GetComponent<Outline>();
        airActive = air.GetComponent<Outline>();
    }
    // Update is called once per frame
    void Update()
    {
        if(MoveScript.movingRight && !movingRightArrow.activeSelf)
        {
            movingLeftArrow.SetActive(false);
            movingRightArrow.SetActive(true);
        }
        else if(!MoveScript.movingRight && !movingLeftArrow.activeSelf)
        {
            movingRightArrow.SetActive(false);
            movingLeftArrow.SetActive(true);
        }

        if(playerState.element == elements.fire && !fireActive.enabled)
        {
            waterActive.enabled = false;
            earthActive.enabled = false;
            airActive.enabled = false;
            fireActive.enabled = true;
        }
        else if (playerState.element == elements.water && !waterActive.enabled)
        {

            fireActive.enabled = false;
            earthActive.enabled = false;
            airActive.enabled = false;
            waterActive.enabled = true;
        }
        else if (playerState.element == elements.earth && !earthActive.enabled)
        {

            fireActive.enabled = false;
            waterActive.enabled = false;
            airActive.enabled = false;
            earthActive.enabled = true;
        }
        else if (playerState.element == elements.air && !airActive.enabled)
        {
            fireActive.enabled = false;
            waterActive.enabled = false;
            earthActive.enabled = false;
            airActive.enabled = true;
        }

        if(beginDashCD)
        {
            dashCooldown.fillAmount = 1.0f;
            beginDashCD = false;
        }


        if (!dashScript.canDash)
        {
            dashCooldown.fillAmount -= 1.0f / dashScript.dashCooldown * Time.deltaTime;
        }
    }
}
