using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarMovement : MonoBehaviour
{
    public float speed; // Adjust the speed of the movement

    void Update()
    {
        transform.Translate (new Vector3(0,1,0) * speed * Time.deltaTime);
    }
}
