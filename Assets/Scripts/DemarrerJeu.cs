using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemarrerJeu : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CommencerJeu();
        }    
    }

    void CommencerJeu()
    {
        SceneManager.LoadScene(4);
    }
}
