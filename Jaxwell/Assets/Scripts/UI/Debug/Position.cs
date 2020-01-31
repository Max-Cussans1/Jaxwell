using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Position : MonoBehaviour
{
    GameObject player;
    Text positionText;
    Vector2 playerPos;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        positionText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        positionText.text = "Position: " + playerPos.ToString();
    }
}
