using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreerEnnemis : MonoBehaviour
{
    //Déclarations des variables
    public GameObject ennemiACreer; //La roue dentelée à dupliquer
    public GameObject personnage; //Pour la position de Megaman
    public float limiteGauche; //Déterminer la zone de reproduction du coté gauche
    public float limiteDroite; //Déterminer la zone de reproduction du coté droit

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DupliqueRoue", 0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Variable pour cloner la roue dentelée
    void DupliqueRoue()
    {
        if (personnage.transform.position.x > limiteGauche && personnage.transform.position.x < limiteDroite)
        {
            GameObject laCopie = Instantiate(ennemiACreer);

            laCopie.SetActive(true);

            laCopie.transform.position = new Vector3(Random.Range(personnage.transform.position.x - 8f, personnage.transform.position.x + 8f), 8f, 0);
        }
    }
}
