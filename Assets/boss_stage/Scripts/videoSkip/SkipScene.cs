using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipScene : MonoBehaviour
{
    
    public void Changng()
    {
        StartCoroutine(NextScene());
    }


    IEnumerator NextScene()
    {
          yield return new WaitForSeconds(0.3f);
          SceneManager.LoadScene("SampleScene");
    }

  
}
