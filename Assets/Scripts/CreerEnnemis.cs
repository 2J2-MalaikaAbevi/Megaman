using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreerEnnemis : MonoBehaviour
{
    //D�clarations des variables
    public GameObject ennemiACreer; //La roue dentel�e � dupliquer
    public GameObject personnage; //Pour la position de Megaman
    public float limiteGauche; //D�terminer la zone de reproduction du cot� gauche
    public float limiteDroite; //D�terminer la zone de reproduction du cot� droit

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DupliqueRoue", 0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Variable pour cloner la roue dentel�e
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
