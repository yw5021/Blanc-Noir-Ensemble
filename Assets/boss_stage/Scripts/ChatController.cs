using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{

    public Text ChatText; // 실제 채팅이 나오는 텍스트
    public Text CharacterName; // 캐릭터 이름이 나오는 텍스트




    public string writerText = "";

    bool isButtonClicked = false;

    void Start()
    {
        StartCoroutine(TextPractice());
    }

    void Update()
    {

    }

    
    IEnumerator NormalChat(string narrator, string narration)
    {
        int a = 0;
        CharacterName.text = narrator;
        writerText = "";

        //텍스트 타이핑 효과
        for (a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            ChatText.text = writerText;
            yield return null;
        }

        //키를 다시 누를 떄 까지 무한정 대기
        while (true)
        {
            if (isButtonClicked)
            {
                isButtonClicked = false;
                break;
            }
            yield return null;
        }
    }

    IEnumerator TextPractice()
    {
        yield return StartCoroutine(NormalChat("렌즈", "안녕 난렌즈야"));
        yield return StartCoroutine(NormalChat("캐릭터2", "안녕하세요, 반갑습니다."));
    }
}


//
//            if (isButtonClicked)
//            {
//                ChatText.text = narration;
//                a = narration.Length; // 버튼 눌리면 그냥 다 출력하게 함
//                isButtonClicked = false;
//            }