using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuiviCamera : MonoBehaviour
{
    //DÉCLARATION DES VARIABLES
    public GameObject laCible; //Variable pour enregistrer le gameObject qui sera suivi (dans ce cas, c'est Mégaman)

    public float limiteHaut; //Variable pour la limite du haut de la caméra
    public float limiteBas; //Variable pour la limite du bas de la caméra
    public float limiteGauche; //Variable pour la limite de la gauche de la caméra
    public float limiteDroite; //Variable pour la limite de la droite de la caméra

    // Update is called once per frame
    void Update()
    {
        //Déclaration locale d'une variable vecteur à 3 chiffres pour la position de la caméra
        Vector3 positionActuelle = laCible.transform.position;

        //Définir la position de la caméra selon les limites imposées (pour ne pas sortir du champ de vue désiré)
        if (positionActuelle.x < limiteGauche) positionActuelle.x = limiteGauche; //limite de la gauche

        if (positionActuelle.x > limiteDroite) positionActuelle.x = limiteDroite; //limite de la droite

        if (positionActuelle.y < limiteBas) positionActuelle.y = limiteBas; //limite du bas

        if (positionActuelle.y > limiteHaut) positionActuelle.y = limiteHaut; //limite du haut

        //On s'assure que la caméra demeure à la même ""profondeur"" (sinon, elle peut être remise à 0 et on ne verra plus la scène)
        positionActuelle.z = -10;

        //Puis on applique à la position de la caméra la position actuelle
        transform.position = positionActuelle;
    }
}
