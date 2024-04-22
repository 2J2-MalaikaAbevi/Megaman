using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemarrerJeu : MonoBehaviour
{
    //DÉCLARATIONS DES VARIABLES
    public TextMeshProUGUI texteIntro; //Variable pour enregistrer le texte de l'intro qui clignote
    public TextMeshProUGUI textMort; //Variable pour enregistrer le texte qui fera le compte à rebord à la mort de Mégaman

    public GameObject megaman; //Variable pour enregister Mégaman

    float departTemps; //Variable pour enregistrer la valeur de départ de le compte à rebour pour la fin avec la mort

    // Update is called once per frame
    void Update()
    {
        //Si la barre d'espace est appuyée, une fonction pour démarrer le jeu est appelée  
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CommencerJeu();
        }

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

        //Gestion du compte à rebour pour le texte de fin
        if (megaman.GetComponent<ControleMegaman>().megamanMort)
        {
            departTemps -= Time.deltaTime;
            textMort.text = "Ça recommence dans : " + departTemps.ToString();
        }

    }

    //Fonction pour démarrer le jeu, c'est-à-dire jouer la scène 4 (scène de jeu pour Mégaman IV)
    void CommencerJeu()
    {
        SceneManager.LoadScene(4);
    }
}
