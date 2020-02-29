using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image health1;
    public Image health2;
    public Image health3;
    public Image health4;

    public static bool updateHealthUI = false;

    void Start()
    {
        switch (Health.lives)
        {
            case 4:
                health1.enabled = true;
                health2.enabled = true;
                health3.enabled = true;
                health4.enabled = true;

                updateHealthUI = false;

                break;

            case 3:
                health1.enabled = true;
                health2.enabled = true;
                health3.enabled = true;
                health4.enabled = false;

                updateHealthUI = false;

                break;

            case 2:
                health1.enabled = true;
                health2.enabled = true;
                health3.enabled = false;
                health4.enabled = false;

                updateHealthUI = false;

                break;

            case 1:
                health1.enabled = true;
                health2.enabled = false;
                health3.enabled = false;
                health4.enabled = false;

                updateHealthUI = false;

                break;

            case 0:
                health1.enabled = false;
                health2.enabled = false;
                health3.enabled = false;
                health4.enabled = false;

                updateHealthUI = false;

                break;

            default:
                health1.enabled = true;
                health2.enabled = true;
                health3.enabled = true;
                health4.enabled = true;

                updateHealthUI = false;

                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (updateHealthUI)
        {
            switch (Health.lives)
            {
                case 4:
                    health1.enabled = true;
                    health2.enabled = true;
                    health3.enabled = true;
                    health4.enabled = true;

                    updateHealthUI = false;

                    break;

                case 3:
                    health1.enabled = true;
                    health2.enabled = true;
                    health3.enabled = true;
                    health4.enabled = false;

                    updateHealthUI = false;

                    break;

                case 2:
                    health1.enabled = true;
                    health2.enabled = true;
                    health3.enabled = false;
                    health4.enabled = false;

                    updateHealthUI = false;

                    break;

                case 1:
                    health1.enabled = true;
                    health2.enabled = false;
                    health3.enabled = false;
                    health4.enabled = false;

                    updateHealthUI = false;

                    break;

                case 0:
                    health1.enabled = false;
                    health2.enabled = false;
                    health3.enabled = false;
                    health4.enabled = false;

                    updateHealthUI = false;

                    break;

                default:
                    health1.enabled = true;
                    health2.enabled = true;
                    health3.enabled = true;
                    health4.enabled = true;

                    updateHealthUI = false;

                    break;
            }
        }
    }
}
