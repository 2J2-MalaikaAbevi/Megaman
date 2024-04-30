using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilité générale du script:
   Génération d'ennemis (roue dentelée)
   Gestion des clones d'ennemis
   Par : Malaïka Abevi
   Dernière modification : 30/04/2024
*/

public class CreerEnnemis : MonoBehaviour
{
    //Déclarations des variables
    public GameObject ennemiACreer; //La roue dentelée à dupliquer
    public GameObject personnage; //Pour la position de Megaman
    public float limiteGauche; //Déterminer la zone de reproduction du coté gauche
    public float limiteDroite; //Déterminer la zone de reproduction du coté droit

    //Fonction pour executer qu'une seule fois l'instruction d'invoquer à une certaine cadence la fonction pour le clonage d'ennemi 
    void Start()
    {
        InvokeRepeating("DupliqueRoue", 0, 3);
    }

    //Fonction pour le clonage et la gestion des clones
    void DupliqueRoue()
    {
        //Si Mégaman est à l'interieur des limites définies
        if (personnage.transform.position.x > limiteGauche && personnage.transform.position.x < limiteDroite)
        {
            //Enregistrement du clone l'ennemi dans une variable 
            GameObject laCopie = Instantiate(ennemiACreer);

            //Rendre le clone actif
            laCopie.SetActive(true);

            //Positionner au hazard sur l'axe des X l'ennemi généré (et à une hauteur de 8f)
            laCopie.transform.position = new Vector3(Random.Range(personnage.transform.position.x - 8f, personnage.transform.position.x + 8f), 8f, 0);
        }
    }
}
