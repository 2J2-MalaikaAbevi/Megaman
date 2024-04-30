using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilit� g�n�rale du script:
   Gestion des d�placements de l'abeille
   Gestion de l'explosion de l'abeille
   Par : Mala�ka Abevi
   Derni�re modification : 30/04/2024
*/

public class ExplosionAbeille : MonoBehaviour
{
    //D�claration de variable
    public AudioClip sonExplosion; //Variable pour le son d'explosion

    //Fonction pour la d�tection de collision entre la roue dentel�e et M�gaman
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //Si l'abeille entre en collision avec M�gaman en attaque ou si elle entre en collision avec une balle 
        if ((infoCollision.gameObject.name == "Megaman" && infoCollision.gameObject.GetComponent<Animator>().GetBool("attaque")) || infoCollision.gameObject.tag == "balle")
        {
            //On active l'animation de l'explosion de l'abeille
            GetComponent<Animator>().SetBool("explosion", true);

            //On d�sactive l'animation de d�placement de gauche � droite du parent (AbeilleDeplacement)
            gameObject.transform.parent.GetComponent<Animator>().enabled = false;

            //On d�sactive son collider
            GetComponent<CapsuleCollider2D>().enabled = false;

            //On fait jouer le son d'explosion
            GetComponent<AudioSource>().PlayOneShot(sonExplosion);

            //Puis on d�truit l'abeille avec un d�lai de 1sec pour avoir de le temps de voir l'animation de l'explosion de l'abeille
            Destroy(gameObject, 0.5f);
        }
    }
}
