using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    // a que velocidad va a ir el enemigo
    public float Speed;
    // velocidad para la bala 2, esta es la velocidad que tendran los enemigos cuando la bala imacte contra ellos

    public float Speed2 = 0.3f;

    //cuanta vida tienen los enemigos, las vidas que tienen
    // cada personaje le restara un daño o otro asi seran diferentes
    public int Damage, Life;//<---------- el daño que hace el enemigo que al ser publico se pone fuera.  //el Life es = cuanta vida tienen los enemigos, las vidas que tienen

    // a que posicion tengo que ir, mi sigueinte posicion en X.
    private float NextPosX;

    // le doy acceso a la lista de enemigos con esta variable, los enemigos al crearse se quedavan en la lista creados como missing al destruirse, me dejan un hueco vacio en la lista.
    // para eliminarlos de la lista le doy acceso con esta variable a la lista
    public GameManager Manager;

    // la torreta cuando se rellene significara que hay una torreta delante
    public GameObject Turret;

    //  un ratio de ataque para cuando el enemigo se tope con una torreta
    private float RateAttack;

    public Animator Elemental;

    public GameObject DeadElemental;

    






    // Use this for initialization
    void Start()
    { 

    
        // aqui le digo que en mi siguiente posicion x, vaya restando 1 para que vaya hacia la izquierda desde la posicion que inicio.
        NextPosX = transform.position.x - 1;

    }
    // Update is called once per frame
    void Update()
    {
        // el movimiento de mi personaje sera desde mi posicion hasta la siguiente posicion , mi posicion inicial sera mi posicion transform.position,
        // luego me movere en la X con NextPosX que arriba le dijo que reste  -1 , y la Y sera ya la posicion que tiene mi posicion Y. luego a la velocidad del speed por el tiempo.
        //la velocidad esta fuera dentro del script enemycontrol.
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(NextPosX, transform.position.y), Speed * Time.deltaTime);

        




        // aqui le digo que cunado llege a esa posicion  NextPosX vuelva  restarme otro -1 para que siga moviendose
        if (transform.position.x == NextPosX)
        {
            // aqui le digo proporcioname uan torreta, 
            Turret = Manager.TurretForward(new Vector2(transform.position.x - 1, transform.position.y));
            // si la torreta es igual a nulo es decir que no hay nadie
            if (Turret == null)
            {
                // el enemigo puede seguir avanzando
                NextPosX = transform.position.x - 1;
            }
        }
       // aqui le voy a decir que el enemigo me ataque , cuando? si el turret es diferente a nulo, significa que hay torreta delante
        if( Turret!= null)
        {
            
            // me tiene que atacar
            // aqui el RateAttack que es la variable de arriba va a ser un contador
            RateAttack += Time.deltaTime;
            // cuando llege a 1 segundo me quitara vida, la vida que yo asigne fuera
            Elemental.SetBool("atackfire", true);
            Elemental.SetBool("atackwater", true);
            if (RateAttack > 1)
            {
                // acedo a la torreta que acede al script turretcontrol que acede a la funcion de removelife, si el daño es menor o igual a 0 significa que la torreta a muerto
                // le pregunto el daño que me va a devolver segun el daño que le he echo yo
                // el daño esta en la funcion RemoveLife que es una funcion del script EnemyControl 
                //alli estan los Hits que al ser publica los hits que tiene que recibir cada torreta para ser destruia deben ponerse desde fuera.
                if(Turret.GetComponent<TurretControl>().RemoveLife(Damage) <= 0)
                {
                    // aqui primero le pregunto si hay un enemigo delante
                    Turret = Manager.TurretForward(new Vector2(transform.position.x - 1, transform.position.y));
                    // si no hay una torreta delante seguire avanzando
                    if (Turret == null)
                    {
                        // el enemigo sigue avanzando
                        NextPosX = transform.position.x - 1;
                        Elemental.SetBool("atackfire", false);
                        Elemental.SetBool("atackwater", false);
                    }
                }
                // reseteo contador
                RateAttack = 0;
            }
        }



        // cuando mi posicion es decir la del enemigo sea -1.5 me eliminas.
        if(transform.position.x == -1.5f)
        {
            // AQUI ANTES DE MORIR hace un removeLife y me guarda la puntuacion
            Manager.RemoveLife();
            // aqui llamo a la funcion de mas abajo
            DeleteEnemy();
            
            
        }
       
    }
    // funcion de borrar enemigos
    private void DeleteEnemy()
    {
        // antes de que se destruya el enemigo lo quitas de la lista accedo al manager que este accede al GameManager que este tiene acceso a la lista de enemigos
        //y eliminar gameobject es decir el enemigo.
        Manager.SavedEnemies.Remove(gameObject);
        //comprueba si ha de desactivar esa torreta
        Manager.DesactiveTurrets();
        // destruirme a mi mismo es decir al enemigo 
        Destroy(gameObject);

        GameObject NewDeadElemental = (GameObject)Instantiate(DeadElemental, transform.position, Quaternion.Euler(0, 0, 0));
        NewDeadElemental.transform.position = new Vector3(NewDeadElemental.transform.position.x, NewDeadElemental.transform.position.y, -5);
    }

    //cuando le enter una bala le restara vida cuando sea suficiente para moriri lo elimino.

    private void OnTriggerEnter2D(Collider2D Col)
    {
        // si el tag es bullet ... o bullet 2 ara este col.tag el doble || sireve para que haga lo mismo a una que la otra
        if(Col.tag == "Bullet" || Col.tag == "Bala2")
        {
            // me vas a eliminar la bala que me a tocado
            Destroy(Col.gameObject);
            // ademas me vas a restar vida, el numero es la cantidad de golpes
            Life -= Col.GetComponent<BulletControl>().Damage;
            //si la vida es igual o menor a 0
            if( Life <=0)
            {// me destruyo de la lista y luego yo

                //  cada enemigo me suma 5 puntos de score
                Manager.GetScore(5);

                DeleteEnemy(); // acedo a esta funcion que ya hace el eliminar de la lista y a mi mismo.
            }
            // me vas a eliminar la bala que me a tocado
            Destroy(Col.gameObject);

           


        }
        // tag para la bala 2 que realintiza al enemigo
        if(Col.tag == "Bala2")
        {
            Speed = Speed2;
        }
    }

    


}
