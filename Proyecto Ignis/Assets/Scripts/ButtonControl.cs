using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    //el boton en su estado nnormal esta puesto en el sprite del boton basico

    //boton para los dragones que se pulse y despulse
    private Button button;
    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //para que emita el boton pulsado
    public void OverrideButtonImage(Sprite imageToChange)
    {
        button.image.overrideSprite = imageToChange;

    }
    //para que pare de emitir el boton pulsado
    public void StopOverrideButtonImage()
    {
        button.image.overrideSprite = null;
    }
}
