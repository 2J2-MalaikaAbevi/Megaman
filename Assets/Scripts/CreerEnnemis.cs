using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilit� g�n�rale du script:
   G�n�ration d'ennemis (roue dentel�e)
   Gestion des clones d'ennemis
   Par : Mala�ka Abevi
   Derni�re modification : 30/04/2024
*/

public class CreerEnnemis : MonoBehaviour
{
    //D�clarations des variables
    public GameObject ennemiACreer; //La roue dentel�e � dupliquer
    public GameObject personnage; //Pour la position de Megaman
    public float limiteGauche; //D�terminer la zone de reproduction du cot� gauche
    public float limiteDroite; //D�terminer la zone de reproduction du cot� droit

    //Fonction pour executer qu'une seule fois l'instruction d'invoquer � une certaine cadence la fonction pour le clonage d'ennemi 
    void Start()
    {
        InvokeRepeating("DupliqueRoue", 0, 3);
    }

    //Fonction pour le clonage et la gestion des clones
    void DupliqueRoue()
    {
        //Si M�gaman est � l'interieur des limites d�finies
        if (personnage.transform.position.x > limiteGauche && personnage.transform.position.x < limiteDroite)
        {
            //Enregistrement du clone l'ennemi dans une variable 
            GameObject laCopie = Instantiate(ennemiACreer);

            //Rendre le clone actif
            laCopie.SetActive(true);

            //Positionner au hazard sur l'axe des X l'ennemi g�n�r� (et � une hauteur de 8f)
            laCopie.transform.position = new Vector3(Random.Range(personnage.transform.position.x - 8f, personnage.transform.position.x + 8f), 8f, 0);
        }
    }
}
