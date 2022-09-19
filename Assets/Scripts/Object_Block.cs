using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Block : Object_Parent
{

    public GameObject pos_map;  //맵에서 자신이 있는 위치

    public Direction_Value block_dir;   //플레이어가 오는 방향 (막아야될 방향)

    public Color_State now_color_state;

    Object_Map now_pos_script;
    Object_Map come_in_root_pos_script;

	// Use this for initialization
	void Start () {
        now_pos_script = pos_map.GetComponent<Object_Map>();
        
        
    }
	
	// Update is called once per frame
	void Update () {
        //임시 버그처리
        if (now_pos_script.go_move_dir[(int)block_dir] == null) return;

        //플레이어가 오는 방향쪽에 블럭 가져와줌
        come_in_root_pos_script = now_pos_script.go_move_dir[(int)block_dir].GetComponent<Object_Map>();

        //들어오는 길을 막아줌
        switch (block_dir)
        {
            case Direction_Value.left:
                come_in_root_pos_script.dir_check_ok[1] = true;
                break;
            case Direction_Value.right:
                come_in_root_pos_script.dir_check_ok[0] = true;
                break;
            case Direction_Value.front:
                come_in_root_pos_script.dir_check_ok[3] = true;
                break;
            case Direction_Value.back:
                come_in_root_pos_script.dir_check_ok[2] = true;
                break;
        }

    }

    public void obj_destroy()
    {
        //임시 버그처리
        if (now_pos_script.go_move_dir[(int)block_dir] != null)
        {
            //막아준 길 풀어줌
            switch (block_dir)
            {
                case Direction_Value.left:
                    come_in_root_pos_script.dir_check_ok[1] = false;
                    break;
                case Direction_Value.right:
                    come_in_root_pos_script.dir_check_ok[0] = false;
                    break;
                case Direction_Value.front:
                    come_in_root_pos_script.dir_check_ok[3] = false;
                    break;
                case Direction_Value.back:
                    come_in_root_pos_script.dir_check_ok[2] = false;
                    break;
            }
        }

        Destroy(gameObject);
    }
}
