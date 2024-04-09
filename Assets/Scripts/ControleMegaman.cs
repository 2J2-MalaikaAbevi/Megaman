using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Fonctionnement et utilit� g�n�rale du script:
   Gestion des d�placements horizontaux et du saut de Megaman � l'aide des touches : Left (ou A), Right (ou D) et Up (ou W).
   Gestion des d�tections des collisions entre le personnage et les objets du jeu.
   Gestion des animations
   Gestion des fins de partie
   Par : Mala�ka Abevi
   Derni�re modification : 09/04/2024
*/

public class ControleMegaman : MonoBehaviour
{
    /******D�CLARATIONS DES VARIABLES******/
    float vitesseX; //Variable pour la vitesse horizontale de Megaman
    public float vitesseXMax; //Variable pour la vitesse de d�placement d�sir�e pour Megaman (modifiable dans l'inspecteur)
    float vitesseY; //Variable pour la vitesse verticale de Megaman
    public float vitesseYMax; //Variable pour la vitesse de saut d�sir�e pour Megaman (modifiable dans l'inspecteur)

    public float vitesseMaximale;  //La vitesse maximale que M�gaman peut atteindre

    public bool partieTerminee = false; //Variable pour d�terminer si la partie est termin�e ou non
       
    bool peutAttaquer = true; //Variable pour d�terminer si M�gaman peut attaquer ou non en v�rifiant s'il y a une attaque en cours

    public AudioClip sonMort; //Variable pour le clip du son de la mort de M�gaman


    //Fonction qui g�re les d�placements et le saut de Megaman et qui g�re les animations de Megaman
    void Update()
    {
        if (!partieTerminee) //Les touches ne marcheront que si la partie n'est pas termin�e
        {
            //On ajuste la variable vitesseX si la touche Left (ou A) ou Right (ou D) est appuy�e.
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                //Avec la variable vitesseXMax qui est public, on peut maintenant modifier la vitesse sans avoir � revenir sans cesse dans le code
                vitesseX = -vitesseXMax;

                //On fait miroiter l'image de Megaman pour qu'il regarde vers la gauche
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                //Avec la variable vitesseXMax qui est public, on peut maintenant modifier la vitesse sans avoir � revenir sans cesse dans le code            
                vitesseX = vitesseXMax;
                //On ram�ne l'image de Megaman � la normal pour qu'il regarde vers la droite
                GetComponent<SpriteRenderer>().flipX = false;

            }
            //On ajuste la variable vitesseX avec la v�locit� en X pour faire un arret plus naturel
            else
            {
                vitesseX = GetComponent<Rigidbody2D>().velocity.x;
            }

            print(Physics2D.OverlapCircle(transform.position, 0.5f) == true);

            //On ajuste la variable vitesseY si la touche Up (ou W) est appuy�e.
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && Physics2D.OverlapCircle(transform.position, 0.25f))
            {
                //Avec la variable vitesseYMax qui est public, on peut maintenant modifier la vitesse sans avoir � revenir sans cesse dans le code
                vitesseY = vitesseYMax;

                //On rend la condition pour l'animation du saut vraie pour qu'elle joue
                GetComponent<Animator>().SetBool("saute", true);
            }

            //On ajuste la variable vitesseY avec la v�locit� en Y pour faire un arr�t plus naturel
            else
            {
                //Avec la variable vitesseYMax qui est public, on peut maintenant modifier la vitesse sans avoir � revenir sans cesse dans le code
                vitesseY = GetComponent<Rigidbody2D>().velocity.y;
            }

            //On fait acc�l�r� Megaman tout en s'assurant qu'il ne d�passe pas la vitesse maximale impos� (pour ne pas qu'il sorte de la zone de jeu)
            if(!peutAttaquer && vitesseX <= vitesseMaximale && vitesseX >= -vitesseMaximale)
            {
                //On multiplie car c'est plus facile a g�rer que d'additionner/soustraire (vitesse n�gative et vitesse positive)
                vitesseX *= 2;
            }

            //On ajuste la v�locit� du personnage en lui attribuant les valeurs des variables de vitesse X et Y
            GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);



            /*******ANIMATIONS DE MARCHE ET D'ATTAQUE DE M�GAMAN******************************************/
            if(/*vitesseX > 0.9f || vitesseX < -0.9f*/ Mathf.Abs(vitesseX) > 0.9f)
            {
                //On rend la condition pour l'animation de la marche vraie pour qu'elle joue
                GetComponent<Animator>().SetBool("marche", true);
            }
            else
            {
                //Sinon, on rend la condition pour l'animation de marche fausse pour que M�gaman arr�te de marcher
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

    /*Fonction pour la d�tection des collisions*/
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //On ajoute une condition avec Physics2D pour que le personnage arr�te son saut seulement lorsqu'il touche un objet "avec ses pieds" 
        if(Physics2D.OverlapCircle(transform.position, 0.25f))
        {
            GetComponent<Animator>().SetBool("saute", false);
        }

        /*Si on touche la roue dentel�e*/
        if(infoCollision.gameObject.name == "RoueDentelee")
        {
            //On fait mourir M�gaman seulement s'il n'est pas en attaque
            if (!(GetComponent<Animator>().GetBool("attaque")))
            {
            //On rend la condition pour l'animation de la mort vraie pour qu'elle joue
            GetComponent<Animator>().SetBool("mort", true);

            //On fait jouer le son de la mort
            GetComponent<AudioSource>().PlayOneShot(sonMort);

            //On rend la variable de la partie termin�e vraie
            partieTerminee = true;

            //On fait rejouer la partie avec un d�lai de 2 sec
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

                //On d�sactive l'animation de d�placement de gauche � droite du parent (AbeilleDeplacement)
                infoCollision.gameObject.transform.parent.GetComponent<Animator>().enabled = false;

                //On d�sactive son collider
                infoCollision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

                //Puis on d�truit l'abeille avec un d�lai de 1sec pour avoir de le temps de voir l'animation de l'explosion de l'abeille
                Destroy(infoCollision.gameObject, 1f);
            }

            //Si l'animation d'attaque de Megaman n'est pas active
            else if(!(GetComponent<Animator>().GetBool("attaque")))
            {
                //On active l'animation de mort de M�gaman et on recommence la partie
                GetComponent<Animator>().SetBool("mort", true);

                //On fait jouer le son de la mort
                GetComponent<AudioSource>().PlayOneShot(sonMort);

                //On d�sactive le collider de l'abeille
                infoCollision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

                //On rend la variable de la partie termin�e vraie
                partieTerminee = true;

                //On fait rejouer la partie avec un d�lai de 2 sec
                Invoke("recommencerJeu", 2f);
            }
        }
    }

    //Fonction pour redonner la possibilit� d'attaquer
    void ActivationAttaque()
    {
        //On remet la variable � true
        peutAttaquer = true;
        //Puis on remet la condition pour l'animation d'attaque � false
        GetComponent<Animator>().SetBool("attaque", false);
    }

    void recommencerJeu()
    {
        //Charger la sc�ne 2 car il s'agit de M�gaman 3 (dans Build Setting)
        SceneManager.LoadScene(2);
    }
}
