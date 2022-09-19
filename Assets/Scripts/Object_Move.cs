using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Move : Object_Map
{

    public bool iswork = false; //작동중인지 여부

    public float move_range_value;  //움직이는 범위
    public float move_speed;

    public Axis_Value Move_Axis;   //움직이는 축

    Vector3 obj_start_pos;  //움직이기 시작하는 위치
    Vector3 obj_end_pos;    //도착 위치

    public GameObject start_pos_connect_map;  //시작위치와 연결된 맵 오브젝트
    public Direction_Value start_pos_connect_dir;
    public GameObject end_pos_connect_map;    //끝 위치와 연결된 맵 오브젝트
    public Direction_Value end_pos_connect_dir;

    float moveValue = 0;    //움직임 계산용

    bool is_end_pos = false;    //위치 체크용

    GameObject player;
    Player player_script;
    bool player_move_ok = false;    //플레이어가 움직임을 시작했는지
    bool player_move_end = false;   //플레이어가 도착했는지

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        player_script = player.GetComponent<Player>();

        set_move_pos();
    }
	
	// Update is called once per frame
	void Update () {
        //작동하면 움직이게
        if (player_move_ok) player_move_end_check();

        if (iswork) move_routine();
	}
    
    void set_move_pos()
    {
        //시작 위치를 잡아주고
        obj_start_pos = transform.position;

        Object_Map temp_move_script = start_pos_connect_map.GetComponent<Object_Map>();

        go_move_dir[(int)start_pos_connect_dir] = start_pos_connect_map;
        Direction_Value reverse_dir = Direction_Value.none;
        switch (start_pos_connect_dir)
        {
            case Direction_Value.left:
                reverse_dir = Direction_Value.right;
                break;
            case Direction_Value.right:
                reverse_dir = Direction_Value.left;
                break;
            case Direction_Value.front:
                reverse_dir = Direction_Value.back;
                break;
            case Direction_Value.back:
                reverse_dir = Direction_Value.front;
                break;
        }

        temp_move_script.go_move_dir[(int)reverse_dir] = gameObject;

        //방향에 맞춰서 끝 위치도 만들어줌
        switch (Move_Axis)
        {
            case Axis_Value.x:
                obj_end_pos = new Vector3(obj_start_pos.x + move_range_value, obj_start_pos.y, obj_start_pos.z);
                break;
            case Axis_Value.y:
                obj_end_pos = new Vector3(obj_start_pos.x, obj_start_pos.y + move_range_value, obj_start_pos.z);
                break;
            case Axis_Value.z:
                obj_end_pos = new Vector3(obj_start_pos.x, obj_start_pos.y, obj_start_pos.z + move_range_value);
                break;
        }
    }

    void move_routine()
    {
        //실제 움직임 표현해줌
        if (is_end_pos) moveValue -= Time.deltaTime * move_speed;
        else moveValue += Time.deltaTime * move_speed;

        player.transform.position = Vector3.Lerp(obj_start_pos + Vector3.up, obj_end_pos + Vector3.up, moveValue);
        transform.position = Vector3.Lerp(obj_start_pos, obj_end_pos, moveValue);

        //이동 끝났는지 체크
        if (move_end_check()) move_end();
    }

    //이 스크립트 밖에서 실행시켜줄 움직임 시작용 내부함수
    public void move_start()
    {
        //iswork = true;
        player_move_ok = player_script.player_move_start(gameObject);

        //이동처리가 불가능했으면
        if (!player_move_ok) return;

        //이동 시작하면 움직일수있는 곳 다 끊어줌
        for (int i = 0; i < 4; i++)
        {
            go_move_dir[i] = null;
        }

        if (!is_end_pos)
        {
            //start_pos_connect_map.GetComponent<Object_Map>().go_move_dir[(int)start_pos_connect_dir] = null;
        }
        else
        {
            //end_pos_connect_map.GetComponent<Object_Map>().go_move_dir[(int)start_pos_connect_dir] = null;
        }
    }

    //끝내는거 체크하는거긴한데 좀 구조가....
    bool move_end_check()
    {
        if (!is_end_pos)
        {
            if (moveValue >= 1) return true;
            else return false;
        }
        else
        {
            if (moveValue <= 0) return true;
            else return false;
        }
    }

    //움직임 끝내주는 함수
    void move_end()
    {
        //소수점값 날리기(임시) (다른 방법 알아보기)
        moveValue = (int)moveValue;

        //움직이는거 끝났으니 변수 초기화
        player_move_ok = false;
        player_move_end = false;

        iswork = false;
        is_end_pos = !is_end_pos;

        crossroads_count_calc();
        start_pos_connect_map.GetComponent<Object_Map>().crossroads_count_calc();
        end_pos_connect_map.GetComponent<Object_Map>().crossroads_count_calc();

        //도착했으니까 연결된 맵 바꿔줘야됨
        if (!is_end_pos)
        {
            Object_Map temp_move_script = start_pos_connect_map.GetComponent<Object_Map>();

            go_move_dir[(int)start_pos_connect_dir] = start_pos_connect_map;
            Direction_Value reverse_dir = Direction_Value.none;
            switch (start_pos_connect_dir)
            {
                case Direction_Value.left:
                    reverse_dir = Direction_Value.right;
                    break;
                case Direction_Value.right:
                    reverse_dir = Direction_Value.left;
                    break;
                case Direction_Value.front:
                    reverse_dir = Direction_Value.back;
                    break;
                case Direction_Value.back:
                    reverse_dir = Direction_Value.front;
                    break;
            }

            temp_move_script.go_move_dir[(int)reverse_dir] = gameObject;
        }
        else
        {
            Object_Map temp_move_script = end_pos_connect_map.GetComponent<Object_Map>();

            go_move_dir[(int)end_pos_connect_dir] = end_pos_connect_map;
            Direction_Value reverse_dir = Direction_Value.none;
            switch (end_pos_connect_dir)
            {
                case Direction_Value.left:
                    reverse_dir = Direction_Value.right;
                    break;
                case Direction_Value.right:
                    reverse_dir = Direction_Value.left;
                    break;
                case Direction_Value.front:
                    reverse_dir = Direction_Value.back;
                    break;
                case Direction_Value.back:
                    reverse_dir = Direction_Value.front;
                    break;
            }

            temp_move_script.go_move_dir[(int)reverse_dir] = gameObject;
        }
    }

    //플레이어가 오브젝트 탑승 위치에 도착 했는지 판단해주는 함수
    void player_move_end_check()
    {
        //플레이어가 이동중이면 끝내줌
        if (player_script.isMove) return;

        if (player_script.go_dest_pos != gameObject) return;

        player_move_end = true;
        iswork = true;
    }
}
