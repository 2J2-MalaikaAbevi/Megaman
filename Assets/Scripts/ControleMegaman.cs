using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Fonctionnement et utilité générale du script:
   Gestion des déplacements horizontaux et du saut de Megaman à l'aide des touches : Left (ou A), Right (ou D) et Up (ou W).
   Gestion des détections des collisions entre le personnage et les objets du jeu.
   Gestion des animations
   Gestion des attaques de Mégaman (attaque classique et tire)
   Gestion des fins de partie
   Par : Malaïka Abevi
   Dernière modification : 30/04/2024
*/

public class ControleMegaman : MonoBehaviour
{
    /******DÉCLARATIONS DES VARIABLES******/                               
    float vitesseX; //Variable pour la vitesse horizontale de Megaman
    float vitesseY; //Variable pour la vitesse verticale de Megaman

    public float vitesseXMax; //Variable pour la vitesse de déplacement désirée pour Megaman (modifiable dans l'inspecteur)
    public float vitesseYMax; //Variable pour la vitesse de saut désirée pour Megaman (modifiable dans l'inspecteur)

    public float vitesseMaximale;  //La vitesse maximale que Mégaman peut atteindre

    public bool partieTerminee = false; //Variable pour déterminer si la partie est terminée ou non

    bool peutAttaquer = true; //Variable pour déterminer si Mégaman peut attaquer ou non en vérifiant s'il y a une attaque en cours

    public static int pointage; //Variable non purgée par la mémoire pour enregistrer le pointage du joueur, c'est-à-dire le nombre de balle d'énergie qu'il amasse
    public static int meilleurPointage; //Variable non purgée par la mémoire pour enregistrer le meilleur pointage du joueur
    public static bool tropheePoint; //Variable non purgée pour enregistrer si le pointage obtenu par le joueur à bien eu un meilleur pointage à la fin de la partie

    public TextMeshProUGUI textePointage; //Variable pour le texte du pointage

    public GameObject BalleOriginale; //Variable pour la balle de Mégaman

    public AudioClip sonMort; //Variable pour le clip du son de la mort de Mégaman
    public AudioClip sonsArme; //Variable pour le clip de son de tire de Mégaman

    //Fonction Update qui gère les déplacements et le saut de Megaman et qui gère les animations de Megaman
    void Update()
    {
        //----------------Gestion du déplacement---------------//
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



            /*******GESTION DES ANIMATIONS DE MARCHE ET D'ATTAQUES DE MÉGAMAN******************************************/

            //-------------------------------------------------Gestion de l'animation de marche de Mégaman
            if(Mathf.Abs(vitesseX) > 0.9f)
            {
                //On rend la condition pour l'animation de la marche vraie pour qu'elle joue
                GetComponent<Animator>().SetBool("marche", true);
            }
            else
            {
                //Sinon, on rend la condition pour l'animation de marche fausse pour que Mégaman arrête de marcher
                GetComponent<Animator>().SetBool("marche", false);
            }


            //--------------------------------------------------Gestion de l'attaque classique
            //Si la barre d'espace est appuyé
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Et seulement si Mégaman peut attaquer et qu'il n'est pas entrain de sauter
                if(peutAttaquer && !(GetComponent<Animator>().GetBool("saute")))
                {
                    //On active l'animation d'attaque en rendant le parametre d'attaque vrai
                    GetComponent<Animator>().SetBool("attaque", true);
                    //Et on enregistre que Mégaman ne peut pas attaquer
                    peutAttaquer = false;

                    //Puis on appelle une fonction qui rend la possibilité d'attaque après un délai de 0,5f 
                    Invoke("ActivationAttaque", 0.5f);
                }
            }


            //-----------------------------------------------Gestion de l'attaque avec la balle de Mégaman
            //Si on appuie sur la touche Entrée
            if (Input.GetKeyDown(KeyCode.Return))
            {   
                //Seulement si l'attaque et le saut ne sont pas en cours
                if (!(GetComponent<Animator>().GetBool("attaque")) && !(GetComponent<Animator>().GetBool("saute")))
                {
                    //Rendre la variable qui permettra les animations de tire de Mégaman à true
                    GetComponent<Animator>().SetBool("tireBalle", true);
                    //Cloner la balle et enregistrer les clones de la balle
                    GameObject balleClone = Instantiate(BalleOriginale);
                    //Rendre les clones actives
                    balleClone.SetActive(true);
                    //Faire jouer le son de tire
                    GetComponent<AudioSource>().PlayOneShot(sonsArme);
                    
                    //Gestion de la direction des balles selon la direction de Mégaman
                    //Si Mégaman est vers la gauche
                    if(GetComponent<SpriteRenderer>().flipX)
                    {
                        //On met les balles du coté gauche de Mégaman
                        balleClone.transform.position = transform.position + new Vector3(-1f, 1);
                        //On propulse les balles vers la gauche en changeant leur vélocités
                        balleClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-25, 0);
                    }
                    //Si Mégaman est vers la droite
                    else
                    {
                        //On met les balles du coté droit de Mégaman
                        balleClone.transform.position = transform.position + new Vector3(1f, 1);
                        //On propulse les balles vers la droite en changeant leur vélocités
                        balleClone.GetComponent<Rigidbody2D>().velocity = new Vector2(25, 0);
                    }
                }
            }
            //Si la touche Entrée est relachée
            else if (Input.GetKeyUp(KeyCode.Return))
            {
                //On rend fausse le paramètre pour le tire de balle
                GetComponent<Animator>().SetBool("tireBalle", false);
            }
        }
    }

    //Fonction pour la détection des collisions
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //On ajoute une condition avec Physics2D pour que le personnage arrète son saut seulement lorsqu'il touche un objet "avec ses pieds" 
        if(Physics2D.OverlapCircle(transform.position, 0.25f))
        {
            GetComponent<Animator>().SetBool("saute", false);
        }

        //Si on touche un ennemi 
        if(infoCollision.gameObject.tag == "ennemi")
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

                //On fait rejouer la partie après un délai de 2 sec
                Invoke("sceneMegamanMort", 2f);
            }
        }

        //Pour désactiver le capsule collider de l'abeille lors d'une collision pour éviter que l'abeille pousse Mégaman
        if(infoCollision.gameObject.name == "Abeille")
        {
            infoCollision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    //Fonction pour la gestion des collisions avec les colliders de type Trigger
    private void OnTriggerEnter2D(Collider2D infoCollider)
    {
        //Si Mégaman touche à des balles d'énergie, le pointage est incrémenté
        if(infoCollider.gameObject.tag == "Point petit")
        {
            //On détruit les balles touchées
            Destroy(infoCollider.gameObject);
            
            //On incrémente le pointage
            pointage++;
            
            //On transcrit la valeur du pointage dans le texte pour le pointage
            textePointage.text = pointage.ToString();
        }

        if(infoCollider.gameObject.name == "LeVideMort")
        {
            //On active l'animation de mort de Mégaman et on recommence la partie
            GetComponent<Animator>().SetBool("mort", true);

            //On fait jouer le son de la mort
            GetComponent<AudioSource>().PlayOneShot(sonMort);

            //On rend la variable de la partie terminée vraie
            partieTerminee = true;

            //On appelle la fonction qui s'occupe du chargement de la scène de mort
            Invoke("sceneMegamanMort", 2f);
        }

        //Si Mégaman touche au trophée, c'est la victoire
        if (infoCollider.gameObject.name == "Trophee")
        {
            //On appelle la fonction qui s'occupe du chargement de la scène de victoire
            sceneMegamanVictoire();

            //Si Mégaman touche le trophée, alors on compare le pointage obtenu avec avec le meilleur pointage réalisé
            if (pointage > meilleurPointage)
            {
                //Si le pointage actuel est plus élevé que le meilleur pointage obtenu, alors on donne nouvelle valeur au meilleur pointage
                meilleurPointage = pointage;

                //On rend la variable booléenne statique true (utile dans un autre script dans la scène de victoire)
                tropheePoint = true;
            }
            else
            {
                //On rend la variable booléenne statique false (utile dans un autre script dans la scène de victoire)
                tropheePoint = false;
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

    //Fonction pour charger la scène de la mort de Mégaman
    void sceneMegamanMort()
    {
        SceneManager.LoadScene("FinaleMort");
    }

    //Fonction pour charger la scène de la victoire de Mégaman
    void sceneMegamanVictoire()
    {
        SceneManager.LoadScene("FinaleVictoire");
    }
}
