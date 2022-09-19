using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Map : Object_Parent
{
    bool move_ok;   //이동이 가능했는지 여부

    public bool[] dir_check_ok = { false, false, false, false };    //움직일 방향 체크했는지 여부

    public Direction_Value next_dir;  //다음 움직일 방향
    public Direction_Value prev_dir;  //들어온 방향

    Direction_Value move_priority = Direction_Value.left; //움직임의 우선순위

    Stack<GameObject> crossroad = new Stack<GameObject>();  //갈림길 저장용

    public int crossroads_count = 0; //갈림길 갯수

    public GameObject[] go_move_dir;    //갈 방향에 있는 오브젝트

    GameObject start_pos;   //계산 시작한 위치 저장용

    Stack<GameObject> path_go = new Stack<GameObject>();    //체크한 길 저장용 (스택으로 해놓긴했는데 다른걸로 처리해야됨)

	// Use this for initialization
	void Start () {
        crossroads_count_calc();    //갈림길 갯수 체크해서 저장
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //스크립트 바깥에서 실행해줄 길찾기 알고리즘
    public bool set_des(GameObject now_pos,GameObject dest_pos)
    {
        
        //도착지점은 goal으로 바꿔줌
        dest_pos.GetComponent<Object_Map>().move_priority = Direction_Value.goal;
        dest_pos.GetComponent<Object_Map>().next_dir = Direction_Value.goal;

        /*
        now_pos.GetComponent<Object_Map>().move_priority = Direction_Value.left;
        for (int i = 0; i < 4; i++)
        {
            now_pos.GetComponent<Object_Map>().dir_check_ok[i] = false;
        }
        */
        Object_Map now_map_script = now_pos.GetComponent<Object_Map>();

        //시작지점 넣어줌
        start_pos = now_pos.transform.gameObject;

        //시작위치를 돌아올 갈림길 스택에 넣어줌
        now_map_script.crossroad.Push(start_pos);
        Debug.Log(start_pos + "가 시작위치");

        //방향 계산
        calc_dir(now_map_script, now_map_script, 0);

        //계산된 방향 적용
        set_dir(now_map_script, 0);

        //계산용으로 썼던 변수들 초기화
        reset_temp_value(now_pos.transform.gameObject);

        //버그 임시수정
        if (!move_ok)
        {
            dest_pos.GetComponent<Object_Map>().move_priority = Direction_Value.none;
            dest_pos.GetComponent<Object_Map>().next_dir = Direction_Value.none;
        }

        //이동 가능했는지 여부 보내줌
        return move_ok;
    }

    //갈림길 저장용 함수
    void push_crossroad(Object_Map now_pos,GameObject go_crossroad)
    {
        GameObject prev_crossroad = now_pos.crossroad.Pop();

        if(prev_crossroad == go_crossroad)
        {
            //같은거면 그냥 다시 넣어줌
            now_pos.crossroad.Push(prev_crossroad);
            Debug.Log(prev_crossroad + " 이미 저장된 갈림길임");
        }
        else
        {
            //다른거면 둘다 넣어줌
            now_pos.crossroad.Push(prev_crossroad);
            now_pos.crossroad.Push(go_crossroad);
            Debug.Log(go_crossroad +" 새로운 갈림길 저장");
        }
    }

    //방향 계산 함수
    void calc_dir(Object_Map prev_pos, Object_Map now_pos,int cnt)
    {
        if (cnt > 200) return;

        //시작지점도 none까지 체크했으면 못가는길이니까 계산 끝내줌
        if (start_pos == now_pos.gameObject && now_pos.move_priority == Direction_Value.none)
        {
            Debug.Log("막혀있거나 못가는 장소임");
            move_ok = false;
            return;
        }
        //임시로 버그 막음 처리할 방법 생각
        if (now_pos.gameObject == start_pos)
        {
            now_pos.crossroad.Push(start_pos);
        }

        Debug.Log(now_pos.transform.gameObject.name + " 에서" + now_pos.move_priority + "를 체크");

        //도착하면 중지
        if (now_pos.move_priority == Direction_Value.goal)
        {
            Debug.Log("탐색 중지");
            //길찾는데 성공했으니까 true로 반환해줌
            move_ok = true;
            return;
        }

        //갈 곳이 없으면 이전 갈림길로 돌아가서 다시 탐색
        if (now_pos.move_priority == Direction_Value.none)
        {
            GameObject go_Map = now_pos.crossroad.Pop();

            Debug.Log(go_Map + "를 이전 갈림길로 뽑음");
            
            Object_Map cross_now_pos = go_Map.GetComponent<Object_Map>();

            //이전 갈림길로 뽑은게 시작위치면 이전위치가 없음
            if (go_Map == start_pos)
            {
                calc_dir(cross_now_pos, cross_now_pos, ++cnt);
                return;
            }


            Object_Map cross_prev_pos = cross_now_pos.go_move_dir[(int)cross_now_pos.prev_dir].GetComponent<Object_Map>();

            cross_now_pos.dir_check_ok[(int)cross_now_pos.move_priority] = true;

            Debug.Log(now_pos.transform.gameObject.name + "에서 막다른길이라 이전 갈림길로 돌아감");
            calc_dir(cross_prev_pos, cross_now_pos, ++cnt);
        }
        //없는 길이면 다음거 탐색
        else if (now_pos.go_move_dir[(int)now_pos.move_priority] == null)
        {
            //갈 위치 오브젝트가 없으면 다음거 탐색
            now_pos.dir_check_ok[(int)now_pos.move_priority] = true;
            now_pos.move_priority++;

            Debug.Log("갈 위치가 없음");
            calc_dir(prev_pos, now_pos, ++cnt);
        }
        //이미 체크한 길이거나 막힌 길이면 다음거 탐색
        else if (now_pos.dir_check_ok[(int)now_pos.move_priority])
        {
            now_pos.move_priority++;

            Debug.Log("지나온 길 이거나 막힌 길임");
            calc_dir(prev_pos, now_pos, ++cnt);
        }
        //다음 위치를 찾았다면
        else
        {
            //다음 위치 체크해주기
            Object_Map next_pos = now_pos.go_move_dir[(int)now_pos.move_priority].GetComponent<Object_Map>();

            //가는 길,들어온 길은 체크과정에서 꺼줌
            switch (now_pos.move_priority)
            {
                case Direction_Value.left:
                    now_pos.dir_check_ok[0] = true;
                    next_pos.dir_check_ok[1] = true;
                    break;
                case Direction_Value.right:
                    now_pos.dir_check_ok[1] = true;
                    next_pos.dir_check_ok[0] = true;
                    break;
                case Direction_Value.front:
                    now_pos.dir_check_ok[2] = true;
                    next_pos.dir_check_ok[3] = true;
                    break;
                case Direction_Value.back:
                    now_pos.dir_check_ok[3] = true;
                    next_pos.dir_check_ok[2] = true;
                    break;
            }

            //갈림길 저장한거 보내줌
            next_pos.crossroad = now_pos.crossroad;
            //시작 위치도 보내줌
            next_pos.start_pos = now_pos.start_pos;

            //갈림길 스택에 저장
            switch (now_pos.crossroads_count)
            {
                case 0:
                    Debug.Log(now_pos.gameObject.name + " 갈림길 갯수 이상");
                    break;
                case 3:
                case 4:
                    Debug.Log(now_pos.transform.gameObject + "에 갈림길 체크");
                    push_crossroad(now_pos, now_pos.gameObject);
                    break;
            }

            //초기화용으로 위치 저장
            path_go.Push(next_pos.gameObject);

            //재귀로 다음경로 탐색
            Debug.Log(next_pos.transform.gameObject.name + " 찾아서 그쪽으로 감");
            calc_dir(now_pos, next_pos, ++cnt);
        }
        
    }

    //실제 방향 정해주는 함수
    void set_dir(Object_Map now_pos,int cnt)
    {
        //무한루프 방지
        if (cnt > 100) return;

        //이동 불가능 위치 선택했으면 끝내줌
        if (!move_ok) return;

        //도착했으면 끝내줌
        if (now_pos.move_priority == Direction_Value.goal) return;

        //다음 위치 설정
        now_pos.next_dir = now_pos.move_priority;
        Debug.Log(now_pos.gameObject + " 에서" + now_pos.move_priority + " 로 다음 위치 설정");

        Object_Map next_pos = now_pos.go_move_dir[(int)now_pos.next_dir].GetComponent<Object_Map>();

        set_dir(next_pos, ++cnt);
    }

    //길 체크용으로 썼던 오브젝트들 초기화용 함수 (초기화 제대로 됐는지 확인해봐야됨)
    void reset_temp_value(GameObject go)
    {
        Debug.Log(go + "초기화 함");
        //시작위치는 저장 안했으니까 따로 초기화 해줌
        Object_Map start_Object_Map = go.GetComponent<Object_Map>();
        start_Object_Map.move_priority = Direction_Value.left;
        start_Object_Map.crossroad = new Stack<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            start_Object_Map.dir_check_ok[i] = false;
        }

        //나머지 위치 초기화 해줌
        int size = path_go.Count;
        for (int i = 0; i < size; i++)
        {
            GameObject temp_go = path_go.Pop();
            Debug.Log(temp_go + "초기화 함");
            Object_Map temp_Object_Map = temp_go.GetComponent<Object_Map>();
            temp_Object_Map.move_priority = Direction_Value.left;
            temp_Object_Map.crossroad = new Stack<GameObject>();
            for (int j = 0; j < 4; j++)
            {
                temp_Object_Map.dir_check_ok[j] = false;
            }
        }
    }



    public void crossroads_count_calc()
    {
        crossroads_count = 0;

        //갈수있는 방향이 몇개인지 체크
        Object_Map Object_Map = GetComponent<Object_Map>();
        for (int i = 0; i < 4; i++) {
            if (Object_Map.go_move_dir[i] != null)
                crossroads_count++;
        }
    }
}
