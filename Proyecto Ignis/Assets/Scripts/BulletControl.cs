using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    //la bala se va a move a una cierta velocidad
    public float Speed;
    // la bala tiene un daño pero se lo asignamos a las torretas desde fuera
    public int Damage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
        if (transform.position.x > 13.5f)
        {
            Destroy(gameObject);
        }
    }
    
}
