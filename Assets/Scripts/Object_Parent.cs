using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Parent : MonoBehaviour {

    //색 상태 구분
    public enum Color_State
    {
        normal = 0,
        red = 1,
        blue = 2,
        green = 3,
        magenta = 4,
        yellow = 5,
        cyan = 6,
        white = 7,
    }
    
    //방향 구분
    public enum Direction_Value
    {
        left = 0,
        right = 1,
        front = 2,
        back = 3,
        none = 4,
        goal = 5,
    }

    //축 구분
    public enum Axis_Value
    {
        x = 1,
        y = 2,
        z = 3,
    }

    //레이저용 방향 구분
    public enum Laser_Direction_Value
    {
        left = 1,
        right = 2,
        up = 3,
        down = 4,
        forward = 5,
        back = 6,
    }

    //색 상태를 색으로 반환해주는 함수
    protected Color color_state_to_color(Color_State state)
    {
        Color temp_color = Color.white;

        switch (state)
        {
            case Color_State.normal:
                temp_color = Color.white;
                break;
            case Color_State.red:
                temp_color = Color.red;
                break;
            case Color_State.blue:
                temp_color = Color.blue;
                break;
            case Color_State.green:
                temp_color = Color.green;
                break;
            case Color_State.magenta:
                temp_color = Color.magenta;
                break;
            case Color_State.yellow:
                temp_color = Color.yellow;
                break;
            case Color_State.cyan:
                temp_color = Color.cyan;
                break;
            case Color_State.white:
                temp_color = Color.white;
                break;
        }

        return temp_color;
    }
}
