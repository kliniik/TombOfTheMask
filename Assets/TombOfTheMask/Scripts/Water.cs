using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public Vector2 moveDirection = Vector2.up; 
    public float speed = 2f;                   

    void Start()
    { 
        StartCoroutine(MoveWater());
    }

    IEnumerator MoveWater()
    {
        while (true)
        {
            // we move the water in the direction of *moveDirection* at the speed of *speed*
            transform.position += (Vector3)(moveDirection.normalized * speed * Time.deltaTime);

            yield return null;
        }
    }
}
