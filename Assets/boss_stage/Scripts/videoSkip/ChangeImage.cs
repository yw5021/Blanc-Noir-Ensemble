using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public Image TesImage; //바뀌어질 이미지
    int a = 0;

    void Start()
    {
        
    }

    void Change()
    {
        //TestImage.sprite = TestSprite; //TestImage에 SourceImage를 TestSprite에 존제하는 이미지로 바꾸어줍니다
        if (Input.GetKeyUp(KeyCode.Space))
        {
            a = a + 1;
            Debug.Log(a = a + 1);
            if (a == 2)
            {
                Debug.Log("눌럿냐 ?");
                TesImage.gameObject.SetActive(true);

            }
            else if (a == 4)
            {
                Debug.Log("2번째 눌러짐");
                TesImage.gameObject.SetActive(false);
                a = 0;
            }
        }

    }
    public void Reset()
    {
        TesImage.gameObject.SetActive(false);
    }
    void Update()
    {
        Change();

    }
}
