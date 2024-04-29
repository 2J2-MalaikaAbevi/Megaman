using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GererScene : MonoBehaviour
{
    //DÉCLARATIONS DES VARIABLES
    public TextMeshProUGUI texteIntro; //Variable pour enregistrer le texte de l'intro qui clignote
    public TextMeshProUGUI texteMort; //Variable pour enregistrer le texte qui fera le compte à rebord à la mort de Mégaman
    public TextMeshProUGUI textePointage; //Variable pour enregistrer le texte pour le pointage 

    public GameObject megaman; //Variable pour enregister Mégaman

    float compteRebours = 10; //Variable pour enregistrer le décompte pour redémarrer le jeu (scène de Megaman5)

    public AudioClip sonVictoire;
                            
    void Start()
    {
        //Gestion du clignotement de l'introduction
        if (SceneManager.GetActiveScene().name == "Introduction")
        {
            InvokeRepeating("GererTexteIntro", 0, 0.3f);
        }

        //Gestion du compte à rebours dans Start pour éviter les problemes de fonction appel trop de fois 
        if (SceneManager.GetActiveScene().name == "FinaleMort")
        {
            InvokeRepeating("GererCompteRebours", 1, 1); //J'ai mis un délai de 1sec avant l'appel pour qu'on puisse voir le "10" du compte à rebours
            textePointage.text = ControleMegaman.pointage.ToString() + " points!";

        }
    }


    // Update is called once per frame
    void Update()
    {
        //Gestion du changement de scène lorsque la partie est terminée
        if (SceneManager.GetActiveScene().name == "Introduction" || SceneManager.GetActiveScene().name == "FinaleMort")
        {
            //Si la barre d'espace est appuyée, une fonction pour démarrer le jeu est appelée 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(7);
            }
        }

        if(SceneManager.GetActiveScene().name == "FinaleVictoire")
        {
            GererSceneVictoire();
            print("coucou");
        }
    }

    //Fonction pour diminuer la valeur du compte à rebours
    void GererCompteRebours()
    {
        compteRebours = compteRebours - 1;

        if (compteRebours <= 0)
        {
            SceneManager.LoadScene(7);
        }

        texteMort.text = "Ça recommence dans : " + compteRebours.ToString();
    }

    //
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
            texteIntro.enabled = false;
        }
    }

    void GererSceneVictoire()
    {
        //Gestion du clignotement du texte d'intro
        if (!GetComponent<AudioSource>().isPlaying)
        {
            SceneManager.LoadScene("Introduction");
        }
    }
}
