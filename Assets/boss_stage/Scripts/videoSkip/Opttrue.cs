using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opttrue : MonoBehaviour
{
    public Canvas Opt;


    void Update()
    {

    }
    
    public void Ativeon()
    {
        ative();
       
    }

    void ative()
    {
        Opt.gameObject.SetActive(true);
    }


}
