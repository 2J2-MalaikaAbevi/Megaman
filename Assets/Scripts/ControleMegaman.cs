using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Fonctionnement et utilit� g�n�rale du script:
   Gestion des d�placements horizontaux et du saut de Megaman � l'aide des touches : Left (ou A), Right (ou D) et Up (ou W).
   Gestion des d�tections des collisions entre le personnage et les objets du jeu.
   Gestion des animations
   Gestion des attaques de M�gaman (attaque classique et tire)
   Gestion des fins de partie
   Par : Mala�ka Abevi
   Derni�re modification : 30/04/2024
*/

public class ControleMegaman : MonoBehaviour
{
    /******D�CLARATIONS DES VARIABLES******/                               
    float vitesseX; //Variable pour la vitesse horizontale de Megaman
    float vitesseY; //Variable pour la vitesse verticale de Megaman

    public float vitesseXMax; //Variable pour la vitesse de d�placement d�sir�e pour Megaman (modifiable dans l'inspecteur)
    public float vitesseYMax; //Variable pour la vitesse de saut d�sir�e pour Megaman (modifiable dans l'inspecteur)

    public float vitesseMaximale;  //La vitesse maximale que M�gaman peut atteindre

    public bool partieTerminee = false; //Variable pour d�terminer si la partie est termin�e ou non

    bool peutAttaquer = true; //Variable pour d�terminer si M�gaman peut attaquer ou non en v�rifiant s'il y a une attaque en cours

    public static int pointage; //Variable non purg�e par la m�moire pour enregistrer le pointage du joueur, c'est-�-dire le nombre de balle d'�nergie qu'il amasse
    public static int meilleurPointage; //Variable non purg�e par la m�moire pour enregistrer le meilleur pointage du joueur
    public static bool tropheePoint; //Variable non purg�e pour enregistrer si le pointage obtenu par le joueur � bien eu un meilleur pointage � la fin de la partie

    public TextMeshProUGUI textePointage; //Variable pour le texte du pointage

    public GameObject BalleOriginale; //Variable pour la balle de M�gaman

    public AudioClip sonMort; //Variable pour le clip du son de la mort de M�gaman
    public AudioClip sonsArme; //Variable pour le clip de son de tire de M�gaman

    //Fonction Update qui g�re les d�placements et le saut de Megaman et qui g�re les animations de Megaman
    void Update()
    {
        //----------------Gestion du d�placement---------------//
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



            /*******GESTION DES ANIMATIONS DE MARCHE ET D'ATTAQUES DE M�GAMAN******************************************/

            //-------------------------------------------------Gestion de l'animation de marche de M�gaman
            if(Mathf.Abs(vitesseX) > 0.9f)
            {
                //On rend la condition pour l'animation de la marche vraie pour qu'elle joue
                GetComponent<Animator>().SetBool("marche", true);
            }
            else
            {
                //Sinon, on rend la condition pour l'animation de marche fausse pour que M�gaman arr�te de marcher
                GetComponent<Animator>().SetBool("marche", false);
            }


            //--------------------------------------------------Gestion de l'attaque classique
            //Si la barre d'espace est appuy�
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Et seulement si M�gaman peut attaquer et qu'il n'est pas entrain de sauter
                if(peutAttaquer && !(GetComponent<Animator>().GetBool("saute")))
                {
                    //On active l'animation d'attaque en rendant le parametre d'attaque vrai
                    GetComponent<Animator>().SetBool("attaque", true);
                    //Et on enregistre que M�gaman ne peut pas attaquer
                    peutAttaquer = false;

                    //Puis on appelle une fonction qui rend la possibilit� d'attaque apr�s un d�lai de 0,5f 
                    Invoke("ActivationAttaque", 0.5f);
                }
            }


            //-----------------------------------------------Gestion de l'attaque avec la balle de M�gaman
            //Si on appuie sur la touche Entr�e
            if (Input.GetKeyDown(KeyCode.Return))
            {   
                //Seulement si l'attaque et le saut ne sont pas en cours
                if (!(GetComponent<Animator>().GetBool("attaque")) && !(GetComponent<Animator>().GetBool("saute")))
                {
                    //Rendre la variable qui permettra les animations de tire de M�gaman � true
                    GetComponent<Animator>().SetBool("tireBalle", true);
                    //Cloner la balle et enregistrer les clones de la balle
                    GameObject balleClone = Instantiate(BalleOriginale);
                    //Rendre les clones actives
                    balleClone.SetActive(true);
                    //Faire jouer le son de tire
                    GetComponent<AudioSource>().PlayOneShot(sonsArme);
                    
                    //Gestion de la direction des balles selon la direction de M�gaman
                    //Si M�gaman est vers la gauche
                    if(GetComponent<SpriteRenderer>().flipX)
                    {
                        //On met les balles du cot� gauche de M�gaman
                        balleClone.transform.position = transform.position + new Vector3(-1f, 1);
                        //On propulse les balles vers la gauche en changeant leur v�locit�s
                        balleClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-25, 0);
                    }
                    //Si M�gaman est vers la droite
                    else
                    {
                        //On met les balles du cot� droit de M�gaman
                        balleClone.transform.position = transform.position + new Vector3(1f, 1);
                        //On propulse les balles vers la droite en changeant leur v�locit�s
                        balleClone.GetComponent<Rigidbody2D>().velocity = new Vector2(25, 0);
                    }
                }
            }
            //Si la touche Entr�e est relach�e
            else if (Input.GetKeyUp(KeyCode.Return))
            {
                //On rend fausse le param�tre pour le tire de balle
                GetComponent<Animator>().SetBool("tireBalle", false);
            }
        }
    }

    //Fonction pour la d�tection des collisions
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //On ajoute une condition avec Physics2D pour que le personnage arr�te son saut seulement lorsqu'il touche un objet "avec ses pieds" 
        if(Physics2D.OverlapCircle(transform.position, 0.25f))
        {
            GetComponent<Animator>().SetBool("saute", false);
        }

        //Si on touche un ennemi 
        if(infoCollision.gameObject.tag == "ennemi")
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

                //On fait rejouer la partie apr�s un d�lai de 2 sec
                Invoke("sceneMegamanMort", 2f);
            }
        }

        //Pour d�sactiver le capsule collider de l'abeille lors d'une collision pour �viter que l'abeille pousse M�gaman
        if(infoCollision.gameObject.name == "Abeille")
        {
            infoCollision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    //Fonction pour la gestion des collisions avec les colliders de type Trigger
    private void OnTriggerEnter2D(Collider2D infoCollider)
    {
        //Si M�gaman touche � des balles d'�nergie, le pointage est incr�ment�
        if(infoCollider.gameObject.tag == "Point petit")
        {
            //On d�truit les balles touch�es
            Destroy(infoCollider.gameObject);
            
            //On incr�mente le pointage
            pointage++;
            
            //On transcrit la valeur du pointage dans le texte pour le pointage
            textePointage.text = pointage.ToString();
        }

        if(infoCollider.gameObject.name == "LeVideMort")
        {
            //On active l'animation de mort de M�gaman et on recommence la partie
            GetComponent<Animator>().SetBool("mort", true);

            //On fait jouer le son de la mort
            GetComponent<AudioSource>().PlayOneShot(sonMort);

            //On rend la variable de la partie termin�e vraie
            partieTerminee = true;

            //On appelle la fonction qui s'occupe du chargement de la sc�ne de mort
            Invoke("sceneMegamanMort", 2f);
        }

        //Si M�gaman touche au troph�e, c'est la victoire
        if (infoCollider.gameObject.name == "Trophee")
        {
            //On appelle la fonction qui s'occupe du chargement de la sc�ne de victoire
            sceneMegamanVictoire();

            //Si M�gaman touche le troph�e, alors on compare le pointage obtenu avec avec le meilleur pointage r�alis�
            if (pointage > meilleurPointage)
            {
                //Si le pointage actuel est plus �lev� que le meilleur pointage obtenu, alors on donne nouvelle valeur au meilleur pointage
                meilleurPointage = pointage;

                //On rend la variable bool�enne statique true (utile dans un autre script dans la sc�ne de victoire)
                tropheePoint = true;
            }
            else
            {
                //On rend la variable bool�enne statique false (utile dans un autre script dans la sc�ne de victoire)
                tropheePoint = false;
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

    //Fonction pour charger la sc�ne de la mort de M�gaman
    void sceneMegamanMort()
    {
        SceneManager.LoadScene("FinaleMort");
    }

    //Fonction pour charger la sc�ne de la victoire de M�gaman
    void sceneMegamanVictoire()
    {
        SceneManager.LoadScene("FinaleVictoire");
    }
}
