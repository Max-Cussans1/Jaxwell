using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    //time before we clean up projectiles (in seconds)
    public float projectileCleanupTime = 3.0f;

    // Use this for initialization
    void Awake()
    {
        //destroy projectile after a certain amount of time
        Destroy(this.gameObject, projectileCleanupTime);
    }
}
