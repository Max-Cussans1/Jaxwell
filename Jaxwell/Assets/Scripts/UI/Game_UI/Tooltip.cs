using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tooltip : MonoBehaviour
{
    //TODO: ERROR HANDLING FOR IF THESE ARE EMPTY
    public Text tooltip;
    public GameObject player;

    public string TooltipText = "";
    bool textChanged = false;

    public float timeToShow = 10.0f;
    float tempTimeToShow;

    public bool showOnlyOnce = false;
    bool hasBeenShown = false;
    bool showToolTip = false;

    void Start()
    {
        tempTimeToShow = timeToShow;
    }

    void Update()
    {
        if(showToolTip)
        {            
            if(!tooltip.enabled)
            {
                tooltip.enabled = true;
            }
            tempTimeToShow -= Time.deltaTime;

            if(tempTimeToShow <= 0)
            {
                hasBeenShown = true;
                showToolTip = false;
                tooltip.enabled = false;

                if(!showOnlyOnce)
                {
                    tempTimeToShow = timeToShow;
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //check if whatever we are hitting isn't null
        if (other.gameObject != null)
        {
            if (other.gameObject == player.gameObject)
            {
                if(!textChanged)
                {
                    tooltip.text = TooltipText;
                }

                if (showOnlyOnce)
                {
                    if (!hasBeenShown)
                    {
                        showToolTip = true;
                    }
                }
                else
                {
                    showToolTip = true;
                }
            }
        }
    }


}
