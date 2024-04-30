using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

/* Fonctionnement et utilité générale du script:
   Gestion de la caméra utilisé par la detection de touche
   Par : Malaïka Abevi
   Dernière modification : 21/04/2024
*/

public class GestionCam : MonoBehaviour
{
    //DÉCLARATION DES VARIABLES
    public GameObject camera1; //Variable pour enregister une première caméra à contrôler
    public GameObject camera2; //Variable pour enregister une deuxième caméra à contrôler

    //On démarre le jeu avec la caméra 1 active et la camera 2 désactivée
    void Start()
    {
        camera1.SetActive(true);
        camera2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Si on appuie sur "1", on rend la première caméra active et la deuxième inactive
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Comme la fonction Start() rend déja la première caméra active et la deuxième inactive, on l'a rappelle tout simplement (au lieu de réecrire le code) 
            Start();
        }
        //Si on appuie sur "2", on rend la première inactive et la deuxième caméra active 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            camera1.SetActive(false);
            camera2.SetActive(true);
        }
    }
}
