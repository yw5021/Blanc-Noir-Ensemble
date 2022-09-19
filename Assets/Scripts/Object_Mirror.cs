using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Mirror : Object_Parent
{

    public Laser_Direction_Value now_mirror_dir;
    public Laser_Direction_Value prev_mirror_dir;

    public float mirror_Laser_Length;

    public float rotate_speed;

    public float rotate_value = 0;

    Vector3 prev_pos;
    Vector3 now_pos;

    Vector3 v3Dest;

    bool isrotate = false;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        //원점에서 거리랑 방향으로 벡터값 구하는 식 
        //밑의 식 이용해서 레이저 빛 돌아가는것처럼 보이게 만들기
        /*
        Vector3 v3Source = Vector3.zero;    // 중심이 되는 오브젝트
        Vector3 v3Distance = Vector3.forward * 10f;    // 거리벡터(forward는 Y축 기준으로 앞을 보고있는 벡터. 필요에따라 수정 필요)
        Quaternion qRotate = Quaternion.Euler(0f, test, 0f);  // 회전할 각도(Y축 기준 회전. 필요에따라 수정 필요)
        Vector3 v3TargetPoint = qRotate * v3Distance;    // 원점을 기준으로 거리와 각도를 연산한 후, 벡터
        v3Dest = v3Source + v3TargetPoint;    // 중심이 되는 오브젝트에서 해당 거리와 각도만큼 이동한 곳의 좌표.

        Debug.Log(v3Dest);

        test++;
        */
        /*
        switch (now_mirror_dir)
        {
            case Laser_Direction_Value.left:
                now_pos = new Vector3(0, 270, 0);
                break;
            case Laser_Direction_Value.right:
                now_pos = new Vector3(0, 90, 0);
                break;
            case Laser_Direction_Value.forward:
                now_pos = new Vector3(0, 0, 0);
                break;
            case Laser_Direction_Value.back:
                now_pos = new Vector3(0, 180, 0);
                break;
            case Laser_Direction_Value.up:
                now_pos = new Vector3(270, 0, 0);
                break;
            case Laser_Direction_Value.down:
                now_pos = new Vector3(90, 0, 0);
                break;
        }
        switch (prev_mirror_dir)
        {
            case Laser_Direction_Value.left:
                prev_pos = new Vector3(0, 270, 0);
                break;
            case Laser_Direction_Value.right:
                prev_pos = new Vector3(0, 90, 0);
                break;
            case Laser_Direction_Value.forward:
                prev_pos = new Vector3(0, 0, 0);
                break;
            case Laser_Direction_Value.back:
                prev_pos = new Vector3(0, 180, 0);
                break;
            case Laser_Direction_Value.up:
                prev_pos = new Vector3(270, 0, 0);
                break;
            case Laser_Direction_Value.down:
                prev_pos = new Vector3(90, 0, 0);
                break;
        }
        */

        //if (prev_mirror_dir != now_mirror_dir)laser_rotate();
    }
    /*
    void laser_rotate()
    {
        //라인렌더러 위치 설정
        Vector3 v3Source = transform.position;    // 중심이 되는 오브젝트
        Vector3 v3Distance = Vector3.forward * mirror_Laser_Length;    // 거리벡터(forward는 Y축 기준으로 앞을 보고있는 벡터. 필요에따라 수정 필요)
        Quaternion qRotate = Quaternion.Euler(Vector3.Lerp(prev_pos,now_pos,rotate_value));  // 회전할 각도(Y축 기준 회전. 필요에따라 수정 필요)
        Vector3 v3TargetPoint = qRotate * v3Distance;    // 원점을 기준으로 거리와 각도를 연산한 후, 벡터
        v3Dest = v3Source + v3TargetPoint;    // 중심이 되는 오브젝트에서 해당 거리와 각도만큼 이동한 곳의 좌표.

        if (rotate_value < 1) rotate_value += Time.deltaTime * rotate_speed;
        else
        {
            rotate_value = 0;
            prev_mirror_dir = now_mirror_dir;
        }
    }
    */
    public void rotate_mirror()
    {
        switch (now_mirror_dir)
        {
            case Laser_Direction_Value.left:
                now_mirror_dir = Laser_Direction_Value.right;
                break;
            case Laser_Direction_Value.right:
                now_mirror_dir = Laser_Direction_Value.left;
                break;
            case Laser_Direction_Value.up:
                now_mirror_dir = Laser_Direction_Value.down;
                break;
            case Laser_Direction_Value.down:
                now_mirror_dir = Laser_Direction_Value.up;
                break;
            case Laser_Direction_Value.back:
                now_mirror_dir = Laser_Direction_Value.forward;
                break;
            case Laser_Direction_Value.forward:
                now_mirror_dir = Laser_Direction_Value.back;
                break;
        }
        if(isrotate)
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 90f, 0);
        else
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y - 90f, 0);

        isrotate = !isrotate;
    }

    public void reflect_laser(GameObject go_laser,Color_State now_color_state)
    {
        Object_Laser temp_laser_script = go_laser.GetComponent<Object_Laser>();
        LineRenderer temp_laser = go_laser.GetComponent<LineRenderer>();

        temp_laser_script.now_laser_dir = now_mirror_dir;
        temp_laser_script.Laser_Length = mirror_Laser_Length;

        temp_laser.SetPosition(0, go_laser.transform.position);
        temp_laser.SetPosition(1, go_laser.transform.position + temp_laser_script.ray_direction * temp_laser_script.Laser_Length); // v3Dest);

        Vector3 ray_direction = Vector3.zero;

        switch (now_mirror_dir)
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

        temp_laser_script.ray_direction = ray_direction;//v3Dest - go_laser.transform.position;

        temp_laser_script.now_laser_color = now_color_state;

        Color temp_color = color_state_to_color(temp_laser_script.now_laser_color);

        temp_laser.startColor = temp_color;
        temp_laser.endColor = temp_color;
    }
}
