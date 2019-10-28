using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDeadControl : MonoBehaviour
{
    private float Contador = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Contador += Time.deltaTime;
        // me elimino de la lista para que no se quede en missing dentro de la lista


        if (Contador > 0.9f)
        {

            Destroy(gameObject);
        }
        
    }
}
