using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    // vida torretas int porque son vidas enteras
    public int Life;

    //haceos igual que los enemigos pero en este caso para poder eliminar las torretas de la lista
    // voy al script GameManager y pongo esto -----> CurrentTurret.GetComponent<TurretControl>().Manager = GetComponent<GameManager>(); 
    // asi con el Manager puedo aceder a las torretas de la lista
    public GameManager Manager;

    //cuando la torreta sabe si ha de atacar o no
    public bool ActiveAttack;

    //vas a recibir un gameobject torreta que seran las balas
    public GameObject Bullet;

    

    //cadencia de disparo publica asi cada torreta puede tener su cadencia

    public float FireRate;

    // contador de tiempo que necesita para cada bala

    private float TimeRate = 0f;

    public int TurretDamage;

    //precio torreta
    public int Price;

    // contador para los soles

    
    private float DestroyAnimDead = 0;

    public GameObject Sol;

    // boleana para activar los soles al dejar la torreta en el tablero
    private bool SolActived = true;

    public Animator Dragon;



    public GameObject AnimDead;
    

    

    





    // Use this for initialization
    void Start()
    {
        // con esto puedo acceder a otros scripts

        Manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            SolActived = false;
        }
        if (this.tag == "Torreta4")
        {
            ActiveAttack = true;
        }
        // esto es lo que vamos a activar segun si la torreta esta o no atacando
        if (ActiveAttack == true && this.tag != "Torreta4")// el && this.tag != "Torreta4") es para que no haga el disparo.
        {

            // aqui la torreta esta disparando
            TimeRate += Time.deltaTime;
            if (TimeRate > FireRate)
            {
                if (this.tag != "Torreta4")
                {



                    //Creo la bala, la instancio
                    GameObject NewBullet = (GameObject)Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, 0));
                    // esta bala tiene tanto daño, le digo que la bala instanciada newbullet acceda desde getcomponent al script Bulletcontrol y haga daño el daño que le asigne a la torreta desde fuera.
                    NewBullet.GetComponent<BulletControl>().Damage = TurretDamage;
                    TimeRate = 0;


                }

                //dragon  activa animacion de ataque
                Dragon.SetBool("atack", true);
                Dragon.SetBool("atackamarillo", true);
                Dragon.SetBool("AtackAzul", true);



            }
           

        }
        if (ActiveAttack == false)
        {
            Dragon.SetBool("atack", false);
            Dragon.SetBool("atackamarillo", false);
            Dragon.SetBool("AtackAzul", false);
        }
        if (Manager.SavedEnemies != null)
        {
            if (SolActived == false)
            {
                TimeRate += Time.deltaTime;
                if (TimeRate > FireRate)
                {

                    if (this.tag == "Torreta4")
                    {




                        GameObject NewSol = (GameObject)Instantiate(Sol, transform.position, Quaternion.Euler(0, 0, 0));
                        NewSol.transform.position = new Vector3(NewSol.transform.position.x, NewSol.transform.position.y, -5);
                        TimeRate = 0;
                        //SolActived = false;

                    }
                }

            }
        }

    }     
        
        
        
        //aqui le digo que cuando suelte el boton izquierdo del raton me active el SolActived
 
    // si he matado a los enemigos pongo int porque los enemigos le van a pegar cuando le pegen le devolvera la vida que le queda
    // restar vida
    public int RemoveLife(int Hits)
    {
        // mi vida es igual a mi vida menos los golpes que me restes
        Life = Life - Hits;
        // si mi vida es menor o igual a 0
        if(Life <= 0 )
        {
           
            // me elimino de la lista para que no se quede en missing dentro de la lista
            Manager.SavedTurrets.Remove(gameObject);
           
            
            // voy a eliminarme
            Destroy(gameObject);
            if (this.tag != "Torreta4")
            {
                GameObject NewAnimDead = (GameObject)Instantiate(AnimDead, transform.position, Quaternion.Euler(0, 0, 0));
                NewAnimDead.transform.position = new Vector3(NewAnimDead.transform.position.x, NewAnimDead.transform.position.y, -5);
            }

            





        }
        // me devuelve la vida para saber cuanta vida me queda, si lo que le queda es menor de 0 es que el enemigo a muerto

        return Life;
    }

}
