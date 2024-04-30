using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

/* Fonctionnement et utilit� g�n�rale du script:
   Gestion de la cam�ra utilis� par la detection de touche
   Par : Mala�ka Abevi
   Derni�re modification : 21/04/2024
*/

public class GestionCam : MonoBehaviour
{
    //D�CLARATION DES VARIABLES
    public GameObject camera1; //Variable pour enregister une premi�re cam�ra � contr�ler
    public GameObject camera2; //Variable pour enregister une deuxi�me cam�ra � contr�ler

    //On d�marre le jeu avec la cam�ra 1 active et la camera 2 d�sactiv�e
    void Start()
    {
        camera1.SetActive(true);
        camera2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Si on appuie sur "1", on rend la premi�re cam�ra active et la deuxi�me inactive
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Comme la fonction Start() rend d�ja la premi�re cam�ra active et la deuxi�me inactive, on l'a rappelle tout simplement (au lieu de r�ecrire le code) 
            Start();
        }
        //Si on appuie sur "2", on rend la premi�re inactive et la deuxi�me cam�ra active 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            camera1.SetActive(false);
            camera2.SetActive(true);
        }
    }
}
