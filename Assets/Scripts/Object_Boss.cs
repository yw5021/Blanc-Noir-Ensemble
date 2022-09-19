using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Boss : Object_Parent
{
    //컴포넌트 창에 띄워줌
    [System.Serializable]
    public struct boss_pattern
    {
        public string test_text;

        public float start_delay;
        public float end_delay;

        public GameObject attack_range;



        /* 코드 안쪽에서 쓸때 용의 생성자
        public boss_pattern(string test_text)
        {
            this.test_text = test_text;
        }
        */
    }
    public boss_pattern[] boss_Patterns;

    Queue<boss_pattern> pattern_queue = new Queue<boss_pattern>();

	// Use this for initialization
	void Start () {
        StartCoroutine(Active_pattern());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void pattern_create()
    {
        int size = boss_Patterns.Length;

        for (int i = 0; i < size; i++)
        {
            pattern_queue.Enqueue(gameObject.GetComponent<Object_Boss>().boss_Patterns[i]);

            Debug.Log("보스 패턴 추가");
            Debug.Log("임시 대사 " + boss_Patterns[i].test_text);
            Debug.Log("선딜 " + boss_Patterns[i].start_delay);
            Debug.Log("후딜 " + boss_Patterns[i].end_delay);
        }

        Debug.Log("");
        Debug.Log("");
        Debug.Log("");
        Debug.Log("");
    }

    IEnumerator Active_pattern()
    {
        if (pattern_queue.Count == 0) pattern_create();

        //패턴 가져오고
        boss_pattern now_pattern;

        now_pattern = pattern_queue.Dequeue();
        
        //선딜 기다림
        yield return new WaitForSeconds(now_pattern.start_delay);

        //패턴 실행
        Debug.Log(now_pattern.test_text);
        Instantiate(now_pattern.attack_range);

        //후딜 기다림
        yield return new WaitForSeconds(now_pattern.end_delay);

        //다음 패턴
        yield return StartCoroutine(Active_pattern());
    }
}
