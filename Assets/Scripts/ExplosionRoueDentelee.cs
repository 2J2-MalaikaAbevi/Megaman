using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilit� g�n�rale du script:
   Gestion des d�placements de la roue dentel�e
   Gestion de l'explosion de la roue dentel�e
   Par : Mala�ka Abevi
   Derni�re modification : 01/04/2024
*/

public class ExplosionRoueDentelee: MonoBehaviour
{
    //Fonction pour la d�tection de collision entre la roue dentel�e et M�gaman
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        if(infoCollision.gameObject.name == "Megaman")
        {
            //R�activer l'animator, ce qui fera jouer l'animation
            GetComponent<Animator>().enabled = true;

            //D�sactiver le collider, pour qu'il n'y ait plus de collision
            GetComponent<Collider2D>().enabled = false;

            /*D�sactivation de la v�locit�, de la v�locit� angulaire et de la gravit� pour que la roue dentel�e reste sur place 
             (puisqu'il n'y a plus de collider, il tomberait dans le vide et continurait son mouvement sinon)*/
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Rigidbody2D>().angularVelocity = 0;
            GetComponent<Rigidbody2D>().gravityScale = 0;

            //Appeler la fonction pour la destruction de la roue dentel�e apr�s une demi seconde, soit la dur�e de son animation d'explosion
            Invoke("DetruireRoueDentelee", 0.5f);
        }  
    }

    //Fonction pour la destruction de la roue dentel�e
    void DetruireRoueDentelee()
    {
        Destroy(gameObject);
    }
}
