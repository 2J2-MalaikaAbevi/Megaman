using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilit� g�n�rale du script:
   Gestion de l'animation d'explosion et de la destruction des clones de la balle
   Par : Mala�ka Abevi
   Derni�re modification : 30/04/2024
*/

public class Balle : MonoBehaviour
{
    //Fonction pour la detection de collision avec les balles
    //Les balles explosent au contact d'un �l�ment du d�cor ou d'un ennemi
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
            //On active l'animation de la balle qui explose
            GetComponent<Animator>().enabled = true;
            //Puis on d�truit la balle apr�s 0,15sec, soit le temps de son animation
            Destroy(gameObject, 0.15f);
    }
}
