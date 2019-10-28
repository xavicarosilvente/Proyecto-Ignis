using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolControl : MonoBehaviour
{
    public float Speed;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
       
        if (transform.position.y > 8.5f)
        {
            Destroy(gameObject);
        }
    }
}
