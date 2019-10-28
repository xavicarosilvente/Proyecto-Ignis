using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //guardo la torreta en una variable global porque una vez creada tengo que saber que torreta es para poder desplazarala por todo el escenario. 
    //esta en privado porque no hace falta desde fuera, en publico puedo ver que funciona.
    // esta variable es cuando pulso en el boton  el tiempo que tengo esta torreta cogida es el currentTurret.
    public GameObject CurrentTurret;

    // esta es una lista donde guardo todas las torretas como añadire torretas por eso es GameObject.
    //añadimos las torretas a la lista para tener un control exacto de las torretas que necesitamos.
    public List<GameObject> SavedTurrets;

    // Objecto enemigo que asigno desde fuera, asigno en el  script gamemanager el enemy prefab en la casilla enemy.
    public GameObject Enemy;

    public GameObject Enemy2;

    // como imprimo enemigos? con un contador , cadencia de enemigos.
    private float RateEnemies;

    private float RateEnemies2;
    // guardo mis enemigos en una lista
    public List<GameObject> SavedEnemies;

    //en el gamemanager controlamos el dinero que llevo
    private int Money = 100;
    // visualizo el dinero en un texto en el canvas
    public Text UI_Money;

    // puntuacion donde consigo puntos
    private int Score;
    // visualizo el score en un texto en el canvas
    public Text UI_Score;

    // puntuacion global
    private int MaxScore;

    // para imprimir en pantalla la puntuacion global que es el maxscore
    public Text UI_MaxScore;


    // yo como usiario tengo vidas, las vidas se restaran cada vez que el enemigo llege a X posicion.
    public int Lifes = 3;

    // me devuelve informacion al tocar un objeto con el cursor, necesitamos crear un rayo el profe le llama Hitinfo
    //me informa que he tocado un objeto a traves de ese objeto puedo hacer cualquier cosa

    private RaycastHit2D HitInfo;

    // cada cuanto aparece un sol de la torreta

    public Animator Torch, Torch2, Torch3, Torch4;

    public GameObject SolCieloObject;

    private float SolCieloRate;













    // Use this for initialization
    void Start()
    {
        // nada más empezar me actualiza el dinero y me aparece desde el principio
        UpdateMoney();
        // me muestre el score desde 0
        GetScore(0);
        // guarda la puntuacion total que ha habido en el juego
        GetMaxScore();
    }

    // Update is called once per frame
    void Update()
    {

        // cuando pulse con el boton izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            // tengo que hacer un rayo fissico que salga desde la camara pero desde el cursor , y la direccion siempre es  -Vector2.up);
            //con esto me coge la direccion hacia adelante , hacia donde voy
            HitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
            // le pregunto si estoy colisionando con algo
            if (HitInfo == true)
            {
                // si el hit info es sol.. que ara?
                if (HitInfo.transform.tag == "Sol")
                {
                    // me sumara dineo
                    Money += 25;
                    //me destruira el gameobject instanciado
                    Destroy(HitInfo.transform.gameObject);
                    // actualizara el dinero actual
                    UpdateMoney();
                }
                

            }
        }
        // reposiciono el objeto siempre en la posicion del mouse y el objeto es CurrentTurrent.
        if (Input.GetMouseButton(0))//<--------- el (0) es el boton izquierdo del raton.
        {
            // Aqui le estoy diciendo que puedo mover la torreta cuando el currentturret este relleno. por eso diferente a nulo es decir nada es relleno.
            if (CurrentTurret != null)//<---- Si el CurrentTurrent es diferente de nulo...
            {
                //nueva posicion para la torreta que seria la posicion del mouse de X más 0.5 y la Y + 0.5 lo del 0.5 es para que se me adapte de cuadrado en cuadrado en el grid.
                CurrentTurret.transform.position = new Vector2((int)MousePosition().x + 0.5f, (int)MousePosition().y + 0.5f);//<------------------ el mouse position es numero decimal para que sea un numero entero lo convierto en intpor eso el (int).

            }
        }
        // cuando sueltes el boton del mouse no puedes cojer la torreta otra vez
        if (Input.GetMouseButtonUp(0))
        {
            if (CurrentTurret != null)//<---- Si el CurrentTurrent es diferente de nulo...
            {//aqui le indico los limites donde si se pueden construir torretas
             // si es mayor a 0 puedo contruir la torreta y si es menor de 10 la podre construir tamb igual con la Y si es mayor de 0 y menor de 6, en estos margenes podre construir
                if ((int)MousePosition().x > 0 && (int)MousePosition().x < 10 && (int)MousePosition().y > 0 && (int)MousePosition().y < 6 && OverlapTurret(CurrentTurret) == false && Money >= CurrentTurret.GetComponent<TurretControl>().Price)//<------------si no hay torreta me dejara guardar una torreta de la lista, tambien le pregunto si tengo el dinero
                {
                    // guardo la torreta dentro de la lista y estoy guardando las torretas del CurrentTurrets , añadimos las torretas a la lista para tener un control exacto de las torretas que necesitamos.
                    SavedTurrets.Add(CurrentTurret);

                    CurrentTurret.GetComponent<TurretControl>().Manager = GetComponent<GameManager>();
                    // comprueba a traves de la funcion ActiveTurretNewTurre para saber si a de atacar o no. teiene que comprobar la currenturret.
                    ActiveTurretNewTurret(CurrentTurret);

                    //Restar precio de la torreta al dinero del usuario, este es el dinero que le voya  restar a las monedas
                    //saber si tengo el dinero suficiente y estar en los limites no estar por debajo de 0 dineros
                    Money -= CurrentTurret.GetComponent<TurretControl>().Price;// si mi dinero es menor al precio de la torreta.
                    //cuando compro la torerta que es cuando la arrastro actualizo el dinero que tengo, siempre debajo de la linea de gastar el dinero me dara el resultado anterior sino.
                    UpdateMoney();




                }
                //sino esta en los limites mencionados anteriormente la torreta no se construira la destruira automaticamente porque no esta en los margenes y sino tengo el dinero suficiente me las destruye no me deja poner torretas sin el dinero
                else
                {
                    Destroy(CurrentTurret);
                }
                CurrentTurret = null;//<--------------aqui le estoy diciendo que al levantar el boton del raton el currentturrent sera nulo entonces
                                     // ya no la podre volver a cojer la torreta porque la condicion de arriba no se cumple.( la de...  if(CurrentTurret != null)... porque ya no sera diferente a nulo.

            }

        }
        // esta funcion hace lo que esta escrito abajo dentro del RespawnEnemies es como un acceso directo a este.
        RespawnEnemies();
        RespawnSolCielo();

    }
    private void RespawnSolCielo()
    {
        SolCieloRate += Time.deltaTime;
        if(SolCieloRate > 5f)
        {
            GameObject NewSolCielo = (GameObject)Instantiate(SolCieloObject, new Vector2(Random.Range(4, 9) + 0.5f, 7.5f),
                Quaternion.Euler(0, 0, 0));
            SolCieloRate = 0;
        }
    }
    private void RespawnEnemies()
    {
        RateEnemies += Time.deltaTime;

        // si el rate es mayor de 1.5f
        if (RateEnemies > 20f)
        {
            // instancio el enemigo  Enemy en NewEnemy  
            GameObject NewEnemy = (GameObject)Instantiate(Enemy, new Vector2(13.5f, Random.Range(1, 6) + 0.5f), Quaternion.Euler(0, 0, 0));//<---------- Aqui le doy la posicion desde donde empieza a salir el enemigo 13.5 en la X y que salga en la Y Random
                                                                                                                                           // y que salga en la Y Random entre 1 y 6.
                                                                                                                                           // aqui guardo mis enemigos en la lista.
            SavedEnemies.Add(NewEnemy);
            // accedo al enemigo le digo el componente que quiero cojer es decir el script del enemigo enemycontrol accedo a la variable Manager y lo igualo a un script aun Gamemanager
            NewEnemy.GetComponent<EnemyControl>().Manager = GetComponent<GameManager>();//<---- lo que consigo esque el Manager que esta en el otro script sea el Game manager asi en el otro script podre usar Manager como si fuera el GameManager.
                                                                                        // aqui activo la funcion  ActiveTurretNewEnemy pongo aqui esta funcion porque es donde estan guardados los enemigos
            ActiveTurretNewEnemy(NewEnemy);

            // reseteo el contador
            RateEnemies = 2;
           
        }
        
        RateEnemies2 += Time.deltaTime;
        if (RateEnemies2 > 60f)
        {
            // instancio el enemigo  Enemy en NewEnemy  
            GameObject NewEnemy2 = (GameObject)Instantiate(Enemy2, new Vector2(13.5f, Random.Range(1, 6) + 0.5f), Quaternion.Euler(0, 0, 0));//<---------- Aqui le doy la posicion desde donde empieza a salir el enemigo 13.5 en la X y que salga en la Y Random
                                                                                                                                             // y que salga en la Y Random entre 1 y 6.
                                                                                                                                             // aqui guardo mis enemigos en la lista.
            SavedEnemies.Add(NewEnemy2);
            // accedo al enemigo le digo el componente que quiero cojer es decir el script del enemigo enemycontrol accedo a la variable Manager y lo igualo a un script aun Gamemanager
            NewEnemy2.GetComponent<EnemyControl>().Manager = GetComponent<GameManager>();//<---- lo que consigo esque el Manager que esta en el otro script sea el Game manager asi en el otro script podre usar Manager como si fuera el GameManager.
                                                                                         // aqui activo la funcion  ActiveTurretNewEnemy pongo aqui esta funcion porque es donde estan guardados los enemigos
            ActiveTurretNewEnemy2(NewEnemy2);


            // reseteo el contador
            RateEnemies2 = 50;



        }

    }

    // esta funcion esta asignada al boton de fuera, el boton tiene que importar una torreta al script entre parentesis pongo lo que entrara, una Turret que es un Gameobject.
    // el prefab de la torreta esta metido en el " OnCLICK" donde tambien esta el script del GameManager.Dentro del Onclick busco la funcion del CreateTurret.
    public void CreateTurret(GameObject Turret)
    {
        // si tengo el dinero para crear la torreta me dejaras arrastar una torreta( con este if ni siquiera se vera la torreta cuando la arrastre ademas de no dejarme ponerla en el tablero.
        if (Money >= Turret.GetComponent<TurretControl>().Price)
        //instancio la torreta que me guardo la que hay que desplazar , la que entrea por parametros Turret, la posicion seria la del mouse que seria el Camera.main...
        {
            GameObject NewTurrent = (GameObject)Instantiate(Turret, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.Euler(0, 0, 0));

            // aqui guardo la nueva instancia , oye currentturret tu eres la nueva instancia que es el newturret.
            //(en el script de GameManager  fuera veras la variable currentturret y cuando pulses a uno de los botones te saldra la torreta)
            CurrentTurret = NewTurrent;
        }
    }


    // esta funcion hace que al pulsar en la torreta la torreta siga en el mouse asta que suelte
    // no pongo despues de private el void porque quiero que me devuelva la posicion  por eso pongo Vector2  porque son dos direcciones X y Y del mouse arriba iz derecha y abajo, 
    //quiero que me devuelva la posicion.
    private Vector2 MousePosition()
    {
        //Aqui obtengo la posicion del mouse con vector2, la guardo
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Aqui la posicion del mouse.
        return MousePos;

        //aqui adentro podria haber usado return Camera.main.ScreenToWorldPoint(Input.mousePosition); que seria lo mismo.
    }
    // aqui le digo que si no hay torreta en esa posicion ya creada que me deje crear una torreta , si ya hay una torreta no podre crearla.
    //recibe parametros y devuelve parametros, devolvera un verdadero o falso si hay torreta true si no hay falso, podre guardar este elemento SavedTurrets.Add(CurrentTurret);siempre que sea falso.
    //por eso aqui la variable es bool
    private bool OverlapTurret(GameObject Turret)//<-----------------se llama OverlapTurret porque significa solapar torreta. entre parentesis pongo que torreta pongo
    {
        // Aqui le digo que si no hay torreta puedo poner torreta
        bool Overlap = false;//<---------Hay torreta? = No
        //Aqui recorro todas las torretas con un For, siempre inicia desde 0 el For.
        //Quiero que el for mire todas las torretas que tengo guardadas en el campo de batalla que serian las que no quiero solapar, las de la lista que serian las que estan puestas.
        for (int i = 0; i < SavedTurrets.Count; i++)//<---- el i++ es para que revise las torres de la lista de 1 en una sumando 1.
        {
            // aqui le pregunto si hay una torreta guardada exactamente en esa posicion, si no hay torreta me dejara.
            //significa si la posición de esa torreta que tengo en la lista es igual que la posición de la torreta que tengo en la función...
            if (SavedTurrets[i].transform.position == Turret.transform.position)
            {
                // no me deja poner torreta.
                Overlap = true; //hay torreta? = si
            }
        }

        return Overlap;//<-------------devuelve el resultado de la operacion de dentro del for si no hay torreta devolvera false si hay torreta devolvera true. este return es asi porque es una booleana.
    }
    // vamos a crer un public gameobject porque me va a devolver esa torreta la que tengo delante si no hay torreta me devolvera nulo 
    // si me devuelve nulo en el script de enemycontrol significara que no tiene ninguna torreta delante .... if (Turret == null)...
    public GameObject TurretForward(Vector2 EnemyPos)//<-------- Turret forward torreta en frente, necesito enviarle una posicion para saber si hay una torreta.
    {
        // recorro todas la torretas creadas en pantalla las de la lista
        for (int i = 0; i < SavedTurrets.Count; i++)
        {
            // si hay alguna torreta en la posicion del enemigo
            if ((Vector2)SavedTurrets[i].transform.position == EnemyPos)//<---------esta convertido a vector 2 por eso el parentesis del vectro2
            {
                //devuelvo la torreta que esta en esa posicion  la savedturret i
                return SavedTurrets[i];
            }
        }
        // no a encontrado ninguna torreta
        return null;
    }

    //cuando tenga una torreta en el escenario decide si se ha de activar a disparar o no
    private void ActiveTurretNewTurret(GameObject Turret)
    {
        //la torreta recorre todos los enemigos para saber si hay algun enemigo, revisa todos los enemigos.
        for (int i = 0; i < SavedEnemies.Count; i++)
        {
            //si esta torreta esta en la posicion i de este enemigo y ademas la X de la torreta es menor que la del enemigo entonces la torreta ataca.
            // si el el saved enemies I posicion y es exactamente igual que el de la torreta (significa que esta en la misma fila) ademas que la torreta este a la izquierda del enemigo.
            //conclusion este if detecta si tengo un enemigo en la misma fila y ademas todavia no em a sobrepasado.
            if (SavedEnemies[i].transform.position.y == Turret.transform.position.y &&
                SavedEnemies[i].transform.position.x > Turret.transform.position.x)
            {
                // si llega a entrar en este parentesis es que hay enemigos delante de la torreta, asi que activo el turret desde el script turret control y la boleana ActiveAttack
                // a traves de un getcomponent y acede al script Turretcontrol, seguidamente accede a la boleana ActiveAttack
                Turret.GetComponent<TurretControl>().ActiveAttack = true;

            }


        }


    }
    //activo la torreta cuando se crea un nuevo enemigo (GameObject NewEnemy)" le paso el enemigo"
    private void ActiveTurretNewEnemy(GameObject NewEnemy)
    {
        //voy a recorrer todas las torretas
        for (int i = 0; i < SavedTurrets.Count; i++)
        {
            // si esta torreta esta en la misma linea y ademas que la x sea mas a la izquierda  que del enemigo
            if (SavedTurrets[i].transform.position.y == NewEnemy.transform.position.y)
            {
                // activar el savedturret I
                SavedTurrets[i].GetComponent<TurretControl>().ActiveAttack = true;// seguidamente llamo esta funcion donde tengo al enemigo guardado. cuando instancio al enemigo


            }

        }




    }

    private void ActiveTurretNewEnemy2(GameObject NewEnemy2)
    {
        //voy a recorrer todas las torretas
        for (int i = 0; i < SavedTurrets.Count; i++)
        {
            // si esta torreta esta en la misma linea y ademas que la x sea mas a la izquierda  que del enemigo
            if (SavedTurrets[i].transform.position.y == NewEnemy2.transform.position.y)
            {
                // activar el savedturret I
                SavedTurrets[i].GetComponent<TurretControl>().ActiveAttack = true;// seguidamente llamo esta funcion donde tengo al enemigo guardado. cuando instancio al enemigo


            }
        }
    }
    // descativa las torretas para que dejen de disparar, desde el enemycontrol y la funcion DeletEnemys.
    public void DesactiveTurrets()
    {
        // recorro todas la torretas de la lista
        for (int i = 0; i < SavedTurrets.Count; i++)
        {
            // calcular todas las torretas de la lista y mirar todos los enemigos
            //cada torreta tendra su boleana para saber que torretas descativar
            bool ContinueAttack = false; // inicio en falso
            //recorro todos los enemigos
            for (int j = 0; j < SavedEnemies.Count; j++)
            {
                //ultilo la j porque la i ya esta en uso, si el enemigo esta en mi fila y ademas superior en x si esta pondremos la boleana ContinueAttack a true
                if (SavedEnemies[j].transform.position.y == SavedTurrets[i].transform.position.y && SavedEnemies[j].transform.position.x > SavedTurrets[i].transform.position.x)
                {
                    ContinueAttack = true;
                }
            }
            SavedTurrets[i].GetComponent<TurretControl>().ActiveAttack = ContinueAttack;


        }



    }
    // funcion para saber cuando he de actualizar el dinero

    public void UpdateMoney()
    {
        UI_Money.text = " " + Money.ToString();
    }

    // funcion para que me sume puntuacion

    public void GetScore(int score)// el score en minuscula es el que le sumo al total que es el Score con S mayus.
    {
        // Score= cantidad total , score cantidad que le sumo.
        Score += score;
        UI_Score.text = " " + Score.ToString();
    }

    // me imprime la puntuacion final
    private void GetMaxScore()
    {
        //si tengo esta puntuacion
        if (PlayerPrefs.HasKey("PvZ_MaxScore") == true)
        {
            MaxScore = PlayerPrefs.GetInt("PvZ_MaxScore");
        }
        // sino tengo el valor el valor es 0 porque no hay partida guardada
        else
        {
            MaxScore = 0;
        }
        // el Ui max scoretext es igual al max score string
        UI_MaxScore.text = " " + MaxScore.ToString();
    }

    //restamos una vida al player , a la persona que juega el juego.
    public void RemoveLife()
    {
        Lifes -= 1;
        // si el life es menor o igual a 0 significa que he muerto
        if (Lifes <= 0)
        {// que lo guarde siempre y cuando sea menor que el guardado
            if (Score > MaxScore)
            {

                // aqui le digo que me guarde la puntuacion, donde lo quiero guardar en "PvZ_MaxScore" que valor? EL SCORE.
                PlayerPrefs.SetInt("PvZ_MaxScore", Score);

            }
            // el juego se para en seco al perder
            
            SceneManager.LoadScene(0);
        }
    }

    // si quisiera resetear el parametro que me guarda el score se haria lo siguiente
    // private void RemoveGame()
    //{
    //   PlayerPrefs.DeleteKey("PvZ_MaxScore"); <--------este sirev para eliminar uno si quiero eliminarlo todo PlayerPrefs.DeleteAll
    //}

    //para animar el boton si pulso en el boton se enciende la animacion si lo vuelvo a pulsar se apaga.
    public void TorchAnim()
    {


        // recibir , preguntar sobre la vairable
        if (Torch.GetBool("Torch"))//<-------------con el get.bool da por echo que esta encendido
        {
            Torch.SetBool("Torch", false);

        }
        else
        {
            // para darle el valor
            Torch.SetBool("Torch", true);
        }

    }
    public void TorchAnim2()
    {
        // recibir , preguntar sobre la vairable
        if (Torch2.GetBool("Torch2"))//<-------------con el get.bool da por echo que esta encendido
        {
            Torch2.SetBool("Torch2", false);

        }
        else
        {
            // para darle el valor
            Torch2.SetBool("Torch2", true);
        }
    }
    public void TorchAnim3()
    {
        // recibir , preguntar sobre la vairable
        if (Torch3.GetBool("Torch3"))//<-------------con el get.bool da por echo que esta encendido
        {
            Torch3.SetBool("Torch3", false);

        }
        else
        {
            // para darle el valor
            Torch3.SetBool("Torch3", true);
        }
    }
    public void TorchAnim4()
    {
        // recibir , preguntar sobre la vairable
        if (Torch4.GetBool("Torch4"))//<-------------con el get.bool da por echo que esta encendido
        {
            Torch4.SetBool("Torch4", false);

        }
        else
        {
            // para darle el valor
            Torch4.SetBool("Torch4", true);
        }




    }
}
