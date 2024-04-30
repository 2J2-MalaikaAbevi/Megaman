using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilité générale du script:
   Gestion de l'animation d'explosion et de la destruction des clones de la balle
   Par : Malaïka Abevi
   Dernière modification : 30/04/2024
*/

public class Balle : MonoBehaviour
{
    //Fonction pour la detection de collision avec les balles
    //Les balles explosent au contact d'un élément du décor ou d'un ennemi
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
            //On active l'animation de la balle qui explose
            GetComponent<Animator>().enabled = true;
            //Puis on détruit la balle après 0,15sec, soit le temps de son animation
            Destroy(gameObject, 0.15f);
    }
}
