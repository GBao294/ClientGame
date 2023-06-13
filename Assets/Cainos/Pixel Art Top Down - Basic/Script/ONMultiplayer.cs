using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ONMultiplayer : MonoBehaviour
{
    //public TopDownCharacterController gameManager;

    public void OnClick()
    {
        //gameManager.SetOnline(true);
        SceneManager.LoadScene(1);
    }
    
}
