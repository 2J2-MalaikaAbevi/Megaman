using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilité générale du script:
   Gestion des déplacements de la roue dentelée
   Gestion de l'explosion de la roue dentelée
   Par : Malaïka Abevi
   Dernière modification : 01/04/2024
*/

public class ExplosionRoueDentelee: MonoBehaviour
{
    //Fonction pour la détection de collision entre la roue dentelée et Mégaman
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        if(infoCollision.gameObject.name == "Megaman")
        {
            //Réactiver l'animator, ce qui fera jouer l'animation
            GetComponent<Animator>().enabled = true;

            //Désactiver le collider, pour qu'il n'y ait plus de collision
            GetComponent<Collider2D>().enabled = false;

            /*Désactivation de la vélocité, de la vélocité angulaire et de la gravité pour que la roue dentelée reste sur place 
             (puisqu'il n'y a plus de collider, il tomberait dans le vide et continurait son mouvement sinon)*/
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Rigidbody2D>().angularVelocity = 0;
            GetComponent<Rigidbody2D>().gravityScale = 0;

            //Appeler la fonction pour la destruction de la roue dentelée après une demi seconde, soit la durée de son animation d'explosion
            Invoke("DetruireRoueDentelee", 0.5f);
        }  
    }

    //Fonction pour la destruction de la roue dentelée
    void DetruireRoueDentelee()
    {
        Destroy(gameObject);
    }
}
