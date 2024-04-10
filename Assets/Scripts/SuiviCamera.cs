using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuiviCamera : MonoBehaviour
{
    //D�CLARATION DES VARIABLES
    public GameObject laCible; //Variable pour enregistrer le gameObject qui sera suivi (dans ce cas, c'est M�gaman)

    public float limiteHaut; //Variable pour la limite du haut de la cam�ra
    public float limiteBas; //Variable pour la limite du bas de la cam�ra
    public float limiteGauche; //Variable pour la limite de la gauche de la cam�ra
    public float limiteDroite; //Variable pour la limite de la droite de la cam�ra

    // Update is called once per frame
    void Update()
    {
        //D�claration locale d'une variable vecteur � 3 chiffres pour la position de la cam�ra
        Vector3 positionActuelle = laCible.transform.position;

        //D�finir la position de la cam�ra selon les limites impos�es (pour ne pas sortir du champ de vue d�sir�)
        if (positionActuelle.x < limiteGauche) positionActuelle.x = limiteGauche; //limite de la gauche

        if (positionActuelle.x > limiteDroite) positionActuelle.x = limiteDroite; //limite de la droite

        if (positionActuelle.y < limiteBas) positionActuelle.y = limiteBas; //limite du bas

        if (positionActuelle.y > limiteHaut) positionActuelle.y = limiteHaut; //limite du haut

        //On s'assure que la cam�ra demeure � la m�me ""profondeur"" (sinon, elle peut �tre remise � 0 et on ne verra plus la sc�ne)
        positionActuelle.z = -10;

        //Puis on applique � la position de la cam�ra la position actuelle
        transform.position = positionActuelle;
    }
}
