using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Laser : Object_Parent
{
    public bool is_temp_laser = false;  //자신이 임시 레이저인지 체크
    public bool is_init = false;        //초기화 했는지 체크

    public GameObject LaserObject;      //임시 레이저 생성용 레이저 프리팹

    public float Laser_Length;

    public Laser_Direction_Value now_laser_dir;

    public Color_State now_laser_color;

    private LineRenderer Laser_line;    //레이저 표현용 라인렌더러

    GameObject temp_go_laser = null;    //임시 레이저 오브젝트 저장용

    public Vector3 ray_direction;  //레이저 방향 오브젝트 체크용 레이의 방향

    // Use this for initialization
    void Start () {
        //임시로 만들어진놈이 아니면 init함수 돌려줌
        if (!is_temp_laser) Laser_init();
    }
	
	// Update is called once per frame
	void Update () {
        //초기화 함수 전에는 업데이트 돌려주면 안됨
        
        switch (now_laser_dir)
        {
            case Laser_Direction_Value.left:
                ray_direction = Vector3.left;
                break;
            case Laser_Direction_Value.right:
                ray_direction = Vector3.right;
                break;
            case Laser_Direction_Value.up:
                ray_direction = Vector3.up;
                break;
            case Laser_Direction_Value.down:
                ray_direction = Vector3.down;
                break;
            case Laser_Direction_Value.back:
                ray_direction = Vector3.back;
                break;
            case Laser_Direction_Value.forward:
                ray_direction = Vector3.forward;
                break;
        }
        
        if (!is_init) return;

        //레이저 과정 나중에 다 함수화 할것
        //레이 만들어서
        Ray ray = new Ray(transform.position, ray_direction);
        RaycastHit hit;

        //디버그용
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);

        //레이저 방향에 물체가 있을 경우
        if (Physics.Raycast(ray, out hit, Laser_Length))
        {
            //레이저 길이 체크하는 부분 만들것
            Object_Laser temp_laser_script;
            LineRenderer temp_laser;
            
            switch (hit.transform.gameObject.tag)
            {
                //레이에 플레이어 맞았을때
                case "Player":

                    //새 레이저 생성해서
                    if (temp_go_laser == null)
                    {
                        Vector3 temp_go_laser_pos = Vector3.zero;
                        switch (now_laser_dir)
                        {
                            case Laser_Direction_Value.left:
                            case Laser_Direction_Value.right:
                                temp_go_laser_pos = new Vector3(hit.transform.position.x, transform.position.y, transform.position.z);
                                break;
                            case Laser_Direction_Value.up:
                            case Laser_Direction_Value.down:
                                temp_go_laser_pos = new Vector3(transform.position.x, hit.transform.position.y, transform.position.z);
                                break;
                            case Laser_Direction_Value.forward:
                            case Laser_Direction_Value.back:
                                temp_go_laser_pos = new Vector3(transform.position.x, transform.position.y, hit.transform.position.z);
                                break;
                        }
                        temp_go_laser = Instantiate(LaserObject, temp_go_laser_pos , Quaternion.identity);
                    }
                    else
                    {
                        Vector3 temp_go_laser_pos = Vector3.zero;
                        switch (now_laser_dir)
                        {
                            case Laser_Direction_Value.left:
                            case Laser_Direction_Value.right:
                                temp_go_laser_pos = new Vector3(hit.transform.position.x, transform.position.y, transform.position.z);
                                break;
                            case Laser_Direction_Value.up:
                            case Laser_Direction_Value.down:
                                temp_go_laser_pos = new Vector3(transform.position.x, hit.transform.position.y, transform.position.z);
                                break;
                            case Laser_Direction_Value.forward:
                            case Laser_Direction_Value.back:
                                temp_go_laser_pos = new Vector3(transform.position.x, transform.position.y, hit.transform.position.z);
                                break;
                        }
                        temp_go_laser.transform.position = temp_go_laser_pos;
                    }

                    //레이저 그려주기
                    temp_laser_script = temp_go_laser.GetComponent<Object_Laser>();
                    temp_laser = temp_go_laser.GetComponent<LineRenderer>();

                    //임시 레이저 오브젝트에 변수들 정해주고
                    float dist_length = 0;
                    switch (now_laser_dir)
                    {
                        case Laser_Direction_Value.left:
                            dist_length = -(transform.position.x - hit.transform.position.x);
                            break;
                        case Laser_Direction_Value.right:
                            dist_length = transform.position.x - hit.transform.position.x;
                            break;
                        case Laser_Direction_Value.up:
                            dist_length = -(transform.position.y - hit.transform.position.y);
                            break;
                        case Laser_Direction_Value.down:
                            dist_length = transform.position.y - hit.transform.position.y;
                            break;
                        case Laser_Direction_Value.forward:
                            dist_length = -(transform.position.z - hit.transform.position.z);
                            break;
                        case Laser_Direction_Value.back:
                            dist_length = transform.position.z - hit.transform.position.z;
                            break;
                    }

                    temp_laser_script.now_laser_dir = now_laser_dir;
                    temp_laser_script.Laser_Length = Laser_Length - dist_length;

                    temp_laser.SetPosition(0, temp_go_laser.transform.position);
                    temp_laser.SetPosition(1, temp_go_laser.transform.position + temp_laser_script.ray_direction * temp_laser_script.Laser_Length);

                    //색 설정
                    //플레이어 색상값 체크해서 그쪽 색으로 변경
                    Color_State now_player_color_state = hit.transform.gameObject.GetComponent<Player>().now_Color_State;

                    Color temp_color = color_state_to_color(now_player_color_state);

                    temp_laser_script.now_laser_color = now_player_color_state;

                    temp_laser.startColor = temp_color;
                    temp_laser.endColor = temp_color;

                    //임시 레이저 오브젝트 init돌려줌
                    temp_laser_script.Laser_init();

                    //원래 레이저는 새 레이저 있는곳까지만
                    Laser_line.SetPosition(1, temp_go_laser.transform.position);
                break;
                
                //거울 오브젝트
                case "obj_mirror":
                    //반사 연출
                    //이부분 거울 오브젝트 쪽으로 옮길예정
                    Object_Mirror temp_mirror = hit.transform.GetComponent<Object_Mirror>();

                    if (temp_go_laser == null)
                    {
                        Vector3 temp_go_laser_pos = hit.transform.position;
                        temp_go_laser = Instantiate(LaserObject, temp_go_laser_pos, Quaternion.identity);
                    }
                    else
                    {
                        temp_go_laser.transform.position = hit.transform.position;
                    }

                    temp_mirror.reflect_laser(temp_go_laser,now_laser_color);
                    /*
                    temp_laser_script = temp_go_laser.GetComponent<Object_Laser>();
                    temp_laser = temp_go_laser.GetComponent<LineRenderer>();

                    temp_laser_script.now_laser_dir = temp_mirror.mirror_dir;
                    temp_laser_script.Laser_Length = temp_mirror.mirror_Laser_Length;

                    temp_laser.SetPosition(0, temp_go_laser.transform.position);
                    temp_laser.SetPosition(1, temp_go_laser.transform.position + temp_laser_script.ray_direction * temp_laser_script.Laser_Length);

                    
                    temp_laser_script.now_laser_color = now_laser_color;

                    temp_color = color_state_to_color(now_laser_color);

                    temp_laser.startColor = temp_color;
                    temp_laser.endColor = temp_color;
                    */
                    //임시 레이저 오브젝트 init돌려줌
                    temp_laser_script = temp_go_laser.GetComponent<Object_Laser>();

                    temp_laser_script.Laser_init();

                    //원래 레이저는 새 레이저 있는곳까지만
                    Laser_line.SetPosition(1, temp_go_laser.transform.position);

                    break;

                //블록 오브젝트
                case "obj_block":
                    Object_Block temp_block = hit.transform.gameObject.GetComponent<Object_Block>();
                    //색 체크해서
                    if (now_laser_color == temp_block.now_color_state)
                    {
                        //색이 같으면 블록 파괴
                        temp_block.obj_destroy();
                    }

                break;
            }
        }
        else
        {
            if (temp_go_laser != null) Laser_destroy(temp_go_laser);
            Laser_line.SetPosition(1, transform.position + ray_direction * Laser_Length);
        }
    }

    void Laser_init()
    {
        Laser_line = gameObject.GetComponent<LineRenderer>();

        //레이져 방향이랑 레이 방향이랑 맞춰줌
        switch (now_laser_dir)
        {
            case Laser_Direction_Value.left:
                ray_direction = Vector3.left;
                break;
            case Laser_Direction_Value.right:
                ray_direction = Vector3.right;
                break;
            case Laser_Direction_Value.up:
                ray_direction = Vector3.up;
                break;
            case Laser_Direction_Value.down:
                ray_direction = Vector3.down;
                break;
            case Laser_Direction_Value.back:
                ray_direction = Vector3.back;
                break;
            case Laser_Direction_Value.forward:
                ray_direction = Vector3.forward;
                break;
        }

        //라인렌더러 처음위치 끝위치 설정
        Laser_line.SetPosition(0, transform.position);
        Laser_line.SetPosition(1, transform.position + ray_direction * Laser_Length);

        //init 돌려준거 체크
        is_init = true;
    }

    void Laser_destroy(GameObject go_laser)
    {
        Object_Laser temp_laser_script = go_laser.GetComponent<Object_Laser>();
        
        //임시 레이저 설정된게 없으면
        if(temp_laser_script.temp_go_laser == null)
        {
            Destroy(go_laser);
            return;
        }

        Destroy(temp_laser_script.temp_go_laser);

        Laser_destroy(temp_laser_script.temp_go_laser);
    }
}
