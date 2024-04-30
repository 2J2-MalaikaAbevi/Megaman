using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/* Fonctionnement et utilit� g�n�rale du script:
   Gestion des d�placements de la roue dentel�e
   Gestion de l'explosion de la roue dentel�e
   Par : Mala�ka Abevi
   Derni�re modification : 30/04/2024
*/

public class ExplosionRoueDentelee: MonoBehaviour
{
    //D�claration de variable
    public AudioClip sonExplosion; //Variable pour le son d'explosion

    //Fonction pour la d�tection de collision entre la roue dentel�e et M�gaman
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //Si l'abeille entre en collision avec M�gaman ou si elle entre en collision avec une balle 
        if (infoCollision.gameObject.name == "Megaman" || infoCollision.gameObject.tag == "balle")
        {
            //R�activer l'animator, ce qui fera jouer l'animation
            GetComponent<Animator>().enabled = true;

            //D�sactiver le collider, pour qu'il n'y ait plus de collision
            GetComponent<Collider2D>().enabled = false;

            //On fait jouer le son d'explosion
            GetComponent<AudioSource>().PlayOneShot(sonExplosion);

            /*D�sactivation de la v�locit�, de la v�locit� angulaire et de la gravit� pour que la roue dentel�e reste sur place 
             (puisqu'il n'y a plus de collider, il tomberait dans le vide et continurait son mouvement sinon)*/
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Rigidbody2D>().angularVelocity = 0;
            GetComponent<Rigidbody2D>().gravityScale = 0;

            //Destruction de la roue dentel�e apr�s une demi seconde, soit la dur�e de son animation d'explosion
            Destroy(gameObject, 0.5f);
        }  
    }
}
