using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltoFila : MonoBehaviour
{
    private float Salto, Fila, SaltoPos;

    private int SaltoDir, FilRand;

    public float Speed;


    // Use this for initialization
    void Start()
    {
        // seleccion de fila aleatoria
        FilRand = Random.Range(0, 2);
        if (FilRand == 0)
        {
            Fila = 6.5f;
        }
        if (FilRand == 1)
        {
            Fila = 8.5f;
        }

        // fila inferior y superior que solo puede subir si es inf y bajar si es superior la fila
        if (transform.position.y == 1.5)
        {
            Salto = 1f;

        }
        if (transform.position.y == 5.5)
        {
            Salto = -1f;
        }
        // filas centrales salto arriba y abajo
        if (transform.position.y == 2.5f || transform.position.y == 3.5f || transform.position.y == 4.5f)
        {
            SaltoDir = Random.Range(0, 2);

            if (SaltoDir == 0)
            {
                Salto = -1f;
            }
            if (SaltoDir == 1)
            {
                Salto = 1f;
            }

        }
        
      
        SaltoPos = transform.position.y + Salto;
    }

    // Update is called once per frame
    void Update()
    {
        //la fila se eligue de manera aleatoria( Fila) desde start y la direccion de salto tambien(SaltoPos)
        if (transform.position.x <= Fila && transform.position.x >= 5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, SaltoPos), Speed * Time.deltaTime);
            // transform.position = new Vector2(transform.position.x, transform.position.y + Salto);
        }
    }
}
