using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject FuegoObject;
    private float FuegoRate;
    private RaycastHit2D Hitinfo2;
    public Animator FireMenu;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Hitinfo2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
            if(Hitinfo2 == true)
            {
                if(Hitinfo2.transform.tag == "Fuego")
                {
                    Destroy(Hitinfo2.transform.gameObject);
                }
            }
        }
        RespawnFuego();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        Application.Quit();
    }
    private void RespawnFuego()
    {
        FuegoRate += Time.deltaTime;
        if(FuegoRate >1.5f)
        {
            GameObject NewFuego = (GameObject)Instantiate(FuegoObject, new Vector2(Random.Range(1, 16) + 0.5f, 13f), Quaternion.Euler(0, 0, 0));
            FuegoRate = 0;
        }

        
    }
}
