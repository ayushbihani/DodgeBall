using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class FallingBlocksScript : MonoBehaviour
{
    public float speed = 7f;
    float xVisibility;
    float yVisibility;
    void Start()
    {
        yVisibility = -Camera.main.orthographicSize - transform.localScale.y;
        xVisibility = -Camera.main.orthographicSize/2 - transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector2.up * speed * Time.deltaTime);
        if(transform.position.x < xVisibility || transform.position.x > -xVisibility || transform.position.y < yVisibility)
            Destroy(gameObject);
    }

    void destroy()
    {
        Destroy(gameObject);
    }
} 
