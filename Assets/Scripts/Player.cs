using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Object_Parent
{
    public bool isActive = true;

    public Material[] obj_player_color;        //플레이어 색상 오브젝트(Material로 되있는거 나중에 바꿀것)

    public float moveSpeed = 3f;
    public float jumpPower = 3f;

    //움직임 계산용 값
    float moveValue = 0f;
    float jumpValue = 0f;

    //플레이어 색 상태 저장용
    Color_State prev_Color_State = Color_State.normal;
    public Color_State now_Color_State = Color_State.normal;

    //플레이어 상태 저장용
    public bool isMove = false;
    public bool isJump = false;

    public bool isNextPosSet = false;

    //go_now_pos는 게임 시작 시점에 첫 위치를 가져오는걸로
    public GameObject go_now_pos;
    public GameObject go_next_pos;
    public GameObject go_dest_pos;

    public GameObject go_goal_pos;

    public string next_scene_name;

    public LayerMask Now_Layer;
    public GameObject skill_fade;
    public Image skill_image;

    public enum Player_State
    {
        normal = 0,
        active_skill = 1,
    }

    public Player_State now_player_state;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

        if (!isActive) return;

        player_move();

        //레이 쏘는 부분은 나중에 레이어로 필터처리하는 방식으로 스킬쪽과 이동쪽 나눠줌
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 99999f ,Now_Layer))
            {
                switch (now_player_state) {
                    case Player_State.normal:
                        Debug.Log("클릭됨 " + hit.transform.gameObject.name);
                        //이 부분은 나중에 오브젝트마다 함수화 시킨 부분으로 처리
                        switch (hit.transform.gameObject.tag)
                        {
                            case "obj_map":
                                player_move_start(hit.transform.gameObject);
                                break;
                            case "obj_move":
                                hit.transform.gameObject.GetComponent<Object_Move>().move_start();
                                break;
                            case "obj_mirror":
                                hit.transform.gameObject.GetComponent<Object_Mirror>().rotate_mirror();
                                break;
                        }
                        break;
                    case Player_State.active_skill:
                        //색상 변경(나중에 스킬쪽으로 뺼것)
                        switch_color(hit.transform.gameObject.tag);
                        break;
                }
            }
        }
    }
    public void change_state()
    {
        if (!isActive) return;

        switch (now_player_state)
        {
            case Player_State.normal:
                now_player_state = Player_State.active_skill;
                Now_Layer = 1 << LayerMask.NameToLayer("ColorLayer");
                skill_fade.SetActive(true);
                break;
            case Player_State.active_skill:
                now_player_state = Player_State.normal;
                now_Color_State = Color_State.normal;
                set_color();
                Now_Layer = 1 << LayerMask.NameToLayer("MoveLayer");
                skill_fade.SetActive(false);
                break;
        }
       
    }
    
    //목표 위치 설정
    public bool player_move_start(GameObject go_pos)
    {
        go_dest_pos = go_pos;

        if (isJump)
        {
            isNextPosSet = true;
            return false;
        }

        bool move_ok = player_move_set_pos(go_dest_pos);

        return move_ok;
    }

    public bool player_move_set_pos(GameObject go_pos)
    {
        Object_Map temp_map_script = go_pos.GetComponent<Object_Map>();

        //방향 체크 쭉 해줌
        bool move_ok = temp_map_script.set_des(go_now_pos, go_dest_pos);

        //길 찾기 체크가 실패했으면
        if (!move_ok)
        {
            Debug.Log("플레이어쪽에서 이동 불가 처리함");
            return false;
        }

        //플레이어는 움직이는 상태로
        isMove = true;

        //첫 이동 위치 설정해줌
        set_next_pos();

        //버그 임시처리
        if (!move_ok) isMove = false;

        //이동 처리 성공했으니까 참값 반환
        return true;
    }

    //플레이어의 실제 움직임
    void player_move()
    {
        //이동중이 아니면 종료
        if (!isMove) return;

        //플레이어가 움직이는 행동루틴 실행
        player_move_routine();

        //도착체크 해줘서 도착했으면 다음 목표 오브젝트 끌어옴
        if (!player_move_routine_end_check()) return;

        //도착했으면 isjump꺼줌
        isJump = false;

        //도착했으면 위치값 계산을 위해 계산용 값들을 0으로 돌려줌
        moveValue = 0;
        jumpValue = 0;

        //약간 틀어져있는 좌표값 맞춰줌
        transform.position = go_next_pos.transform.position + Vector3.up;

        //플레이어의 현재 위치 바꿔줌
        go_now_pos = go_next_pos;

        if (go_now_pos == go_goal_pos) goal_event();

        if (isNextPosSet) player_move_set_pos(go_dest_pos);

        //다음 방향 정해준거 가져와서 다음 위치 설정해줌
        set_next_pos();
    }

    void goal_event()
    {
        FadeOutScript Fade = GameObject.Find("Fade").GetComponent<FadeOutScript>();

        Fade.ChangeScene(next_scene_name);
        Fade.StartFadeAnim();

    }

    //현재 위치에 정해진 다음 움직여야될 장소 가져와서 목적지로 설정해주는 함수
    void set_next_pos()
    {
        Object_Map temp_map_script = go_now_pos.GetComponent<Object_Map>();

        Direction_Value next_dir = temp_map_script.next_dir;

        switch (next_dir)
        {
            case Direction_Value.left:
            case Direction_Value.right:
            case Direction_Value.front:
            case Direction_Value.back:
                go_next_pos = temp_map_script.go_move_dir[(int)next_dir];
                break;

            case Direction_Value.none:
                Debug.Log("다음 움직일 장소를 찾지 못함");
                break;
            case Direction_Value.goal:
                //현재 위치가 도착장소면 이동 끝내줌
                player_move_end();
                break;
        }

        //플레이어 회전 임시로 만듬
        switch (next_dir)
        {
            case Direction_Value.left:
                //270
                gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case Direction_Value.right:
                //90
                gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Direction_Value.front:
                //0
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction_Value.back:
                //180
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
        }
    }

    void player_move_end()
    {
        go_dest_pos = go_now_pos;

        isMove = false;
    }

    //플레이어의 움직임 표현
    void player_move_routine()
    {
        //이동하는 루틴 짜주기
        moveValue += Time.deltaTime * moveSpeed;

        jumpValue += Time.deltaTime;

        // 9.8 = 중력가속도
        float height = (jumpValue * jumpValue * (-9.8f) / 2f) + (jumpValue * jumpPower);

        //y축 움직임을 따로 계산해서 점프하는것처럼 보이게 만듬
        Vector3 y_vector = new Vector3(0,height,0);

        //맵보다 밑으로 가면 안됨
        if (height < 0) y_vector = Vector3.zero;

        //플레이어 실제 움직임
        transform.position = Vector3.Lerp(go_now_pos.transform.position, go_next_pos.transform.position, moveValue);
        //플레이어 점프 표현 넣어줌
        transform.position += y_vector + Vector3.up;

        //점프중인 상태로 바꿔줌
        isJump = true;
    }
    
    bool player_move_routine_end_check()
    {
        //포지션으로 도착했는지 확인
        if (((go_next_pos.transform.position + Vector3.up)-transform.position).sqrMagnitude > 0.001f) return false;

        //위쪽에서 안걸렸으면 도착 한거니까 true 반환해줌
        return true;
    }

    //플레이어 색 바꿔주는 함수(인자로 태그 받아서 처리하는부분 바꿔줄것)
    void switch_color(string tag)
    {
        //바꾸기 전의 색 저장
        prev_Color_State = now_Color_State;

        switch (prev_Color_State)
        {
            case Color_State.normal:
                switch (tag)
                {
                    case "obj_red":
                        now_Color_State = Color_State.red;
                    break;
                    case "obj_green":
                        now_Color_State = Color_State.green;
                    break;
                    case "obj_blue":
                        now_Color_State = Color_State.blue;
                    break;
                }
            break;

            case Color_State.red:
                switch (tag)
                {
                    case "obj_green":
                        now_Color_State = Color_State.yellow;
                    break;
                    case "obj_blue":
                        now_Color_State = Color_State.magenta;
                    break;
                }
            break;

            case Color_State.blue:
                switch (tag)
                {
                    case "obj_red":
                        now_Color_State = Color_State.magenta;
                    break;
                    case "obj_green":
                        now_Color_State = Color_State.cyan;
                    break;
                }
            break;

            case Color_State.green:
                switch (tag)
                {
                    case "obj_red":
                        now_Color_State = Color_State.yellow;
                    break;
                    case "obj_blue":
                        now_Color_State = Color_State.cyan;
                    break;
                }
            break;

            case Color_State.yellow:
                switch (tag)
                {
                    case "obj_blue":
                        now_Color_State = Color_State.white;
                        break;
                }
                break;

            case Color_State.magenta:
                switch (tag)
                {
                    case "obj_green":
                        now_Color_State = Color_State.white;
                        break;
                }
                break;

            case Color_State.cyan:
                switch (tag)
                {
                    case "obj_red":
                        now_Color_State = Color_State.white;
                        break;
                }
                break;
        }



        //바뀐 색에 따라서 material 바꿔주기
        set_color();
    }

    //색 리셋 함수
    public void reset_color()
    {
        //원래상태로 돌려주고
        prev_Color_State = Color_State.normal;
        now_Color_State = Color_State.normal;

        //색 적용
        set_color();
    }

    //실제 색 적용시켜줌 (플레이어 색을 오브젝트로 처리하게되면 이부분 수정)
    void set_color()
    {
        GameObject.Find("cha").GetComponent<SkinnedMeshRenderer>().material = obj_player_color[(int)now_Color_State];
        skill_image.color = color_state_to_color(now_Color_State);

        now_player_state = Player_State.normal;
        skill_fade.SetActive(false);
        Now_Layer = 1 << LayerMask.NameToLayer("MoveLayer");
    }


}

