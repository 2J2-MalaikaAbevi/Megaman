using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilité générale du script:
   Gestion des déplacements de l'abeille
   Gestion de l'explosion de l'abeille
   Par : Malaïka Abevi
   Dernière modification : 30/04/2024
*/

public class ExplosionAbeille : MonoBehaviour
{
    //Déclaration de variable
    public AudioClip sonExplosion; //Variable pour le son d'explosion

    //Fonction pour la détection de collision entre la roue dentelée et Mégaman
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //Si l'abeille entre en collision avec Mégaman en attaque ou si elle entre en collision avec une balle 
        if ((infoCollision.gameObject.name == "Megaman" && infoCollision.gameObject.GetComponent<Animator>().GetBool("attaque")) || infoCollision.gameObject.tag == "balle")
        {
            //On active l'animation de l'explosion de l'abeille
            GetComponent<Animator>().SetBool("explosion", true);

            //On désactive l'animation de déplacement de gauche à droite du parent (AbeilleDeplacement)
            gameObject.transform.parent.GetComponent<Animator>().enabled = false;

            //On désactive son collider
            GetComponent<CapsuleCollider2D>().enabled = false;

            //On fait jouer le son d'explosion
            GetComponent<AudioSource>().PlayOneShot(sonExplosion);

            //Puis on détruit l'abeille avec un délai de 1sec pour avoir de le temps de voir l'animation de l'explosion de l'abeille
            Destroy(gameObject, 0.5f);
        }
    }
}
