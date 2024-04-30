using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Fonctionnement et utilit� g�n�rale du script:
   Gestion des changements de sc�nes
   Gestion du texte affich�s dans les diff�rentes sc�nes
   Gestion de l'affichage du pointage
   Par : Mala�ka Abevi
   Derni�re modification : 30/04/2024
*/

public class GererScene : MonoBehaviour
{
    //D�CLARATIONS DES VARIABLES
    public TextMeshProUGUI texteIntro; //Variable pour enregistrer le texte de l'intro qui clignote
    public TextMeshProUGUI texteMort; //Variable pour enregistrer le texte qui fera le compte � rebord � la mort de M�gaman
    public TextMeshProUGUI textePointage; //Variable pour enregistrer le texte pour le pointage 

    //public GameObject megaman; //Variable pour enregister M�gaman
    public GameObject trophee; //Variable pour enregistrer le troph�e de la sc�ne de victoire

    float compteRebours = 10; //Variable pour enregistrer le d�compte pour red�marrer le jeu (sc�ne de Megaman5)
    

    //Gestion des diff�rente sc�nes et des instructions qui doivent �tre appeler une seule fois (Comme le compte � rebours)
    void Start()
    {
        //Gestion du clignotement de l'introduction
        if (SceneManager.GetActiveScene().name == "Introduction")
        {
            //Appel de la fonction qui g�re le texte de la sc�ne d'introduction
            InvokeRepeating("GererTexteIntro", 0, 0.3f);
            textePointage.text = "Pointage � battre : "+ ControleMegaman.meilleurPointage.ToString();
        }

        //Gestion du compte � rebours dans Start pour �viter les problemes de fonction appel trop de fois 
        if (SceneManager.GetActiveScene().name == "FinaleMort")
        {
            InvokeRepeating("GererCompteRebours", 1, 1); //J'ai mis un d�lai de 1sec avant l'appel pour qu'on puisse voir le "10" du compte � rebours
            //On affiche le pointage de la partie faite avec un message
            textePointage.text = ControleMegaman.pointage.ToString() + " points!";
        }

        //Gestion du pointage 
        if(SceneManager.GetActiveScene().name == "FinaleVictoire")
        {
            //On affiche le pointage de la partie faite avec un message
            textePointage.text = ControleMegaman.pointage.ToString() + " points!";
        }
    }


    //Gestion dans Update des touches appuy�es et des instructions qui doivent �tre en constante execution
    void Update()
    {
        //Gestion du changement de sc�ne lorsque que le joueur n'est pas dans la sc�ne de jeu
        //Dans le cas o� si le joueur est � la sc�ne d'intro ou � la sc�ne de mort
        if (SceneManager.GetActiveScene().name == "Introduction" || SceneManager.GetActiveScene().name == "FinaleMort")
        {
            //Si la barre d'espace est appuy�e, une fonction pour d�marrer le jeu est appel�e 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Megaman5");
                
                //On r�initialise le pointage � 0
                ControleMegaman.pointage = 0;
            }
        }

        //Dans le cas o� si le joueur est � la sc�ne de victoire
        if (SceneManager.GetActiveScene().name == "FinaleVictoire")
        {   
            //On appelle la fonction pour la gestion de la sc�ne de victoire
            GererSceneVictoire();
        }
    }

    //Fonction pour diminuer la valeur du compte � rebours
    void GererCompteRebours()
    {
        //On d�cremente le compte � rebours 
        compteRebours--;

        //Quand le compte arebours est a z�ro, on red�marre la sc�ne de jeu
        if (compteRebours <= 0)
        {
            SceneManager.LoadScene("Megaman5");
        }

        //On affiche le message de compte � rebours avec la valeur du compte � rebours 
        texteMort.text = "�a recommence dans : " + compteRebours.ToString();
    }

    //Fonction pour g�rer le texte afficher � la sc�ne d'intro
    void GererTexteIntro()
    {
        //Gestion du clignotement du texte d'intro
        if (!texteIntro.enabled)
        {
            //On r�active le texte pour le message de fin 
            texteIntro.enabled = true;
        }
        else if (texteIntro.enabled)
        {
            //On d�sactive le texte pour le message de fin
            texteIntro.enabled = false;
        }
    }

    //Fonction pour la gestion de la sc�ne de victoire
    void GererSceneVictoire()
    {
        //On affiche l'image du troph�e seulemeent si le pointage de la partie compl�t�e est plus �lev� que le meilleur pointage enregistr�
        if(ControleMegaman.tropheePoint)
        {
            trophee.SetActive(true);
        }

        //Si la musique ne joue plus, on passe � la sc�ne d'introduction
        if (!GetComponent<AudioSource>().isPlaying)
        {
            SceneManager.LoadScene("Introduction");
            
            //On r�initialise le pointage � 0
            ControleMegaman.pointage = 0;
        }
    }
}
