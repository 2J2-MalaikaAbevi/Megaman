using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemarrerJeu : MonoBehaviour
{
    //D�CLARATIONS DES VARIABLES
    public TextMeshProUGUI texteIntro; //Variable pour enregistrer le texte de l'intro qui clignote
    public TextMeshProUGUI textMort; //Variable pour enregistrer le texte qui fera le compte � rebord � la mort de M�gaman

    public GameObject megaman; //Variable pour enregister M�gaman

    float departTemps; //Variable pour enregistrer la valeur de d�part de le compte � rebour pour la fin avec la mort

    // Update is called once per frame
    void Update()
    {
        //Si la barre d'espace est appuy�e, une fonction pour d�marrer le jeu est appel�e  
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CommencerJeu();
        }

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

        //Gestion du compte � rebour pour le texte de fin
        if (megaman.GetComponent<ControleMegaman>().megamanMort)
        {
            departTemps -= Time.deltaTime;
            textMort.text = "�a recommence dans : " + departTemps.ToString();
        }

    }

    //Fonction pour d�marrer le jeu, c'est-�-dire jouer la sc�ne 4 (sc�ne de jeu pour M�gaman IV)
    void CommencerJeu()
    {
        SceneManager.LoadScene(4);
    }
}
