using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/* Fonctionnement et utilité générale du script:
   Gestion des déplacements de la roue dentelée
   Gestion de l'explosion de la roue dentelée
   Par : Malaïka Abevi
   Dernière modification : 30/04/2024
*/

public class ExplosionRoueDentelee: MonoBehaviour
{
    //Déclaration de variable
    public AudioClip sonExplosion; //Variable pour le son d'explosion

    //Fonction pour la détection de collision entre la roue dentelée et Mégaman
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //Si l'abeille entre en collision avec Mégaman ou si elle entre en collision avec une balle 
        if (infoCollision.gameObject.name == "Megaman" || infoCollision.gameObject.tag == "balle")
        {
            //Réactiver l'animator, ce qui fera jouer l'animation
            GetComponent<Animator>().enabled = true;

            //Désactiver le collider, pour qu'il n'y ait plus de collision
            GetComponent<Collider2D>().enabled = false;

            //On fait jouer le son d'explosion
            GetComponent<AudioSource>().PlayOneShot(sonExplosion);

            /*Désactivation de la vélocité, de la vélocité angulaire et de la gravité pour que la roue dentelée reste sur place 
             (puisqu'il n'y a plus de collider, il tomberait dans le vide et continurait son mouvement sinon)*/
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Rigidbody2D>().angularVelocity = 0;
            GetComponent<Rigidbody2D>().gravityScale = 0;

            //Destruction de la roue dentelée après une demi seconde, soit la durée de son animation d'explosion
            Destroy(gameObject, 0.5f);
        }  
    }
}
