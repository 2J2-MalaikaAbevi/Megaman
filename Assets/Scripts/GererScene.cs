using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Fonctionnement et utilité générale du script:
   Gestion des changements de scènes
   Gestion du texte affichés dans les différentes scènes
   Gestion de l'affichage du pointage
   Par : Malaïka Abevi
   Dernière modification : 30/04/2024
*/

public class GererScene : MonoBehaviour
{
    //DÉCLARATIONS DES VARIABLES
    public TextMeshProUGUI texteIntro; //Variable pour enregistrer le texte de l'intro qui clignote
    public TextMeshProUGUI texteMort; //Variable pour enregistrer le texte qui fera le compte à rebord à la mort de Mégaman
    public TextMeshProUGUI textePointage; //Variable pour enregistrer le texte pour le pointage 

    //public GameObject megaman; //Variable pour enregister Mégaman
    public GameObject trophee; //Variable pour enregistrer le trophée de la scène de victoire

    float compteRebours = 10; //Variable pour enregistrer le décompte pour redémarrer le jeu (scène de Megaman5)
    

    //Gestion des différente scènes et des instructions qui doivent être appeler une seule fois (Comme le compte à rebours)
    void Start()
    {
        //Gestion du clignotement de l'introduction
        if (SceneManager.GetActiveScene().name == "Introduction")
        {
            //Appel de la fonction qui gère le texte de la scène d'introduction
            InvokeRepeating("GererTexteIntro", 0, 0.3f);
            textePointage.text = "Pointage à battre : "+ ControleMegaman.meilleurPointage.ToString();
        }

        //Gestion du compte à rebours dans Start pour éviter les problemes de fonction appel trop de fois 
        if (SceneManager.GetActiveScene().name == "FinaleMort")
        {
            InvokeRepeating("GererCompteRebours", 1, 1); //J'ai mis un délai de 1sec avant l'appel pour qu'on puisse voir le "10" du compte à rebours
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


    //Gestion dans Update des touches appuyées et des instructions qui doivent être en constante execution
    void Update()
    {
        //Gestion du changement de scène lorsque que le joueur n'est pas dans la scène de jeu
        //Dans le cas où si le joueur est à la scène d'intro ou à la scène de mort
        if (SceneManager.GetActiveScene().name == "Introduction" || SceneManager.GetActiveScene().name == "FinaleMort")
        {
            //Si la barre d'espace est appuyée, une fonction pour démarrer le jeu est appelée 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Megaman5");
                
                //On réinitialise le pointage à 0
                ControleMegaman.pointage = 0;
            }
        }

        //Dans le cas où si le joueur est à la scène de victoire
        if (SceneManager.GetActiveScene().name == "FinaleVictoire")
        {   
            //On appelle la fonction pour la gestion de la scène de victoire
            GererSceneVictoire();
        }
    }

    //Fonction pour diminuer la valeur du compte à rebours
    void GererCompteRebours()
    {
        //On décremente le compte à rebours 
        compteRebours--;

        //Quand le compte arebours est a zéro, on redémarre la scène de jeu
        if (compteRebours <= 0)
        {
            SceneManager.LoadScene("Megaman5");
        }

        //On affiche le message de compte à rebours avec la valeur du compte à rebours 
        texteMort.text = "Ça recommence dans : " + compteRebours.ToString();
    }

    //Fonction pour gérer le texte afficher à la scène d'intro
    void GererTexteIntro()
    {
        //Gestion du clignotement du texte d'intro
        if (!texteIntro.enabled)
        {
            //On réactive le texte pour le message de fin 
            texteIntro.enabled = true;
        }
        else if (texteIntro.enabled)
        {
            //On désactive le texte pour le message de fin
            texteIntro.enabled = false;
        }
    }

    //Fonction pour la gestion de la scène de victoire
    void GererSceneVictoire()
    {
        //On affiche l'image du trophée seulemeent si le pointage de la partie complétée est plus élevé que le meilleur pointage enregistré
        if(ControleMegaman.tropheePoint)
        {
            trophee.SetActive(true);
        }

        //Si la musique ne joue plus, on passe à la scène d'introduction
        if (!GetComponent<AudioSource>().isPlaying)
        {
            SceneManager.LoadScene("Introduction");
            
            //On réinitialise le pointage à 0
            ControleMegaman.pointage = 0;
        }
    }
}
