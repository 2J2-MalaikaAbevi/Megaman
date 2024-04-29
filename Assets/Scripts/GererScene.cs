using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GererScene : MonoBehaviour
{
    //D�CLARATIONS DES VARIABLES
    public TextMeshProUGUI texteIntro; //Variable pour enregistrer le texte de l'intro qui clignote
    public TextMeshProUGUI texteMort; //Variable pour enregistrer le texte qui fera le compte � rebord � la mort de M�gaman
    public TextMeshProUGUI textePointage; //Variable pour enregistrer le texte pour le pointage 

    public GameObject megaman; //Variable pour enregister M�gaman

    float compteRebours = 10; //Variable pour enregistrer le d�compte pour red�marrer le jeu (sc�ne de Megaman5)

    public AudioClip sonVictoire;
                            
    void Start()
    {
        //Gestion du clignotement de l'introduction
        if (SceneManager.GetActiveScene().name == "Introduction")
        {
            InvokeRepeating("GererTexteIntro", 0, 0.3f);
        }

        //Gestion du compte � rebours dans Start pour �viter les problemes de fonction appel trop de fois 
        if (SceneManager.GetActiveScene().name == "FinaleMort")
        {
            InvokeRepeating("GererCompteRebours", 1, 1); //J'ai mis un d�lai de 1sec avant l'appel pour qu'on puisse voir le "10" du compte � rebours
            textePointage.text = ControleMegaman.pointage.ToString() + " points!";

        }
    }


    // Update is called once per frame
    void Update()
    {
        //Gestion du changement de sc�ne lorsque la partie est termin�e
        if (SceneManager.GetActiveScene().name == "Introduction" || SceneManager.GetActiveScene().name == "FinaleMort")
        {
            //Si la barre d'espace est appuy�e, une fonction pour d�marrer le jeu est appel�e 
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

    //Fonction pour diminuer la valeur du compte � rebours
    void GererCompteRebours()
    {
        compteRebours = compteRebours - 1;

        if (compteRebours <= 0)
        {
            SceneManager.LoadScene(7);
        }

        texteMort.text = "�a recommence dans : " + compteRebours.ToString();
    }

    //
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
