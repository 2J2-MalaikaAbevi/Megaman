using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Fonctionnement et utilité générale du script:
   Gestion des déplacements horizontaux et du saut de Megaman à l'aide des touches : Left (ou A), Right (ou D) et Up (ou W).
   Gestion des détections des collisions entre le personnage et les objets du jeu.
   Gestion des animations
   Gestion des fins de partie
   Par : Malaïka Abevi
   Dernière modification : 09/04/2024
*/

public class ControleMegaman : MonoBehaviour
{
    /******DÉCLARATIONS DES VARIABLES******/
    float vitesseX; //Variable pour la vitesse horizontale de Megaman
    public float vitesseXMax; //Variable pour la vitesse de déplacement désirée pour Megaman (modifiable dans l'inspecteur)
    float vitesseY; //Variable pour la vitesse verticale de Megaman
    public float vitesseYMax; //Variable pour la vitesse de saut désirée pour Megaman (modifiable dans l'inspecteur)

    public float vitesseMaximale;  //La vitesse maximale que Mégaman peut atteindre

    public bool partieTerminee = false; //Variable pour déterminer si la partie est terminée ou non
       
    bool peutAttaquer = true; //Variable pour déterminer si Mégaman peut attaquer ou non en vérifiant s'il y a une attaque en cours

    public AudioClip sonMort; //Variable pour le clip du son de la mort de Mégaman


    //Fonction qui gère les déplacements et le saut de Megaman et qui gère les animations de Megaman
    void Update()
    {
        if (!partieTerminee) //Les touches ne marcheront que si la partie n'est pas terminée
        {
            //On ajuste la variable vitesseX si la touche Left (ou A) ou Right (ou D) est appuyée.
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                //Avec la variable vitesseXMax qui est public, on peut maintenant modifier la vitesse sans avoir à revenir sans cesse dans le code
                vitesseX = -vitesseXMax;

                //On fait miroiter l'image de Megaman pour qu'il regarde vers la gauche
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                //Avec la variable vitesseXMax qui est public, on peut maintenant modifier la vitesse sans avoir à revenir sans cesse dans le code            
                vitesseX = vitesseXMax;
                //On ramène l'image de Megaman à la normal pour qu'il regarde vers la droite
                GetComponent<SpriteRenderer>().flipX = false;

            }
            //On ajuste la variable vitesseX avec la vélocité en X pour faire un arret plus naturel
            else
            {
                vitesseX = GetComponent<Rigidbody2D>().velocity.x;
            }

            print(Physics2D.OverlapCircle(transform.position, 0.5f) == true);

            //On ajuste la variable vitesseY si la touche Up (ou W) est appuyée.
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && Physics2D.OverlapCircle(transform.position, 0.25f))
            {
                //Avec la variable vitesseYMax qui est public, on peut maintenant modifier la vitesse sans avoir à revenir sans cesse dans le code
                vitesseY = vitesseYMax;

                //On rend la condition pour l'animation du saut vraie pour qu'elle joue
                GetComponent<Animator>().SetBool("saute", true);
            }

            //On ajuste la variable vitesseY avec la vélocité en Y pour faire un arrêt plus naturel
            else
            {
                //Avec la variable vitesseYMax qui est public, on peut maintenant modifier la vitesse sans avoir à revenir sans cesse dans le code
                vitesseY = GetComponent<Rigidbody2D>().velocity.y;
            }

            //On fait accéléré Megaman tout en s'assurant qu'il ne dépasse pas la vitesse maximale imposé (pour ne pas qu'il sorte de la zone de jeu)
            if(!peutAttaquer && vitesseX <= vitesseMaximale && vitesseX >= -vitesseMaximale)
            {
                //On multiplie car c'est plus facile a gérer que d'additionner/soustraire (vitesse négative et vitesse positive)
                vitesseX *= 2;
            }

            //On ajuste la vélocité du personnage en lui attribuant les valeurs des variables de vitesse X et Y
            GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);



            /*******ANIMATIONS DE MARCHE ET D'ATTAQUE DE MÉGAMAN******************************************/
            if(/*vitesseX > 0.9f || vitesseX < -0.9f*/ Mathf.Abs(vitesseX) > 0.9f)
            {
                //On rend la condition pour l'animation de la marche vraie pour qu'elle joue
                GetComponent<Animator>().SetBool("marche", true);
            }
            else
            {
                //Sinon, on rend la condition pour l'animation de marche fausse pour que Mégaman arrête de marcher
                GetComponent<Animator>().SetBool("marche", false);
            }

            /****************Gestion de l'attaque****************/
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(peutAttaquer && !(GetComponent<Animator>().GetBool("saute")))
                {
                    GetComponent<Animator>().SetBool("attaque", true);
                    peutAttaquer = false;

                    Invoke("ActivationAttaque", 0.5f);
                }
            }
        }   
    }

    /*Fonction pour la détection des collisions*/
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //On ajoute une condition avec Physics2D pour que le personnage arrète son saut seulement lorsqu'il touche un objet "avec ses pieds" 
        if(Physics2D.OverlapCircle(transform.position, 0.25f))
        {
            GetComponent<Animator>().SetBool("saute", false);
        }

        /*Si on touche la roue dentelée*/
        if(infoCollision.gameObject.name == "RoueDentelee")
        {
            //On fait mourir Mégaman seulement s'il n'est pas en attaque
            if (!(GetComponent<Animator>().GetBool("attaque")))
            {
            //On rend la condition pour l'animation de la mort vraie pour qu'elle joue
            GetComponent<Animator>().SetBool("mort", true);

            //On fait jouer le son de la mort
            GetComponent<AudioSource>().PlayOneShot(sonMort);

            //On rend la variable de la partie terminée vraie
            partieTerminee = true;

            //On fait rejouer la partie avec un délai de 2 sec
            Invoke("recommencerJeu", 2f);
            }
        }

        /*Si on touche l'abeille*/
        if(infoCollision.gameObject.name == "Abeille")
        {
            //Si l'animation d'attaque de Megaman est active
            if (GetComponent<Animator>().GetBool("attaque"))
            {
                //On active l'animation de l'explosion de l'abeille
                infoCollision.gameObject.GetComponent<Animator>().SetBool("explosion", true);

                //On désactive l'animation de déplacement de gauche à droite du parent (AbeilleDeplacement)
                infoCollision.gameObject.transform.parent.GetComponent<Animator>().enabled = false;

                //On désactive son collider
                infoCollision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

                //Puis on détruit l'abeille avec un délai de 1sec pour avoir de le temps de voir l'animation de l'explosion de l'abeille
                Destroy(infoCollision.gameObject, 1f);
            }

            //Si l'animation d'attaque de Megaman n'est pas active
            else if(!(GetComponent<Animator>().GetBool("attaque")))
            {
                //On active l'animation de mort de Mégaman et on recommence la partie
                GetComponent<Animator>().SetBool("mort", true);

                //On fait jouer le son de la mort
                GetComponent<AudioSource>().PlayOneShot(sonMort);

                //On désactive le collider de l'abeille
                infoCollision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

                //On rend la variable de la partie terminée vraie
                partieTerminee = true;

                //On fait rejouer la partie avec un délai de 2 sec
                Invoke("recommencerJeu", 2f);
            }
        }
    }

    //Fonction pour redonner la possibilité d'attaquer
    void ActivationAttaque()
    {
        //On remet la variable à true
        peutAttaquer = true;
        //Puis on remet la condition pour l'animation d'attaque à false
        GetComponent<Animator>().SetBool("attaque", false);
    }

    void recommencerJeu()
    {
        //Charger la scène 2 car il s'agit de Mégaman 3 (dans Build Setting)
        SceneManager.LoadScene(2);
    }
}
