using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elements : MonoBehaviour
{
    public enum elements
    {
        fire,
        water,
        earth,
        air
    };

    //returns true if the same element
    public static bool ElementCheck(elements thisElement, elements otherElement)
    {
        if (otherElement == thisElement)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
