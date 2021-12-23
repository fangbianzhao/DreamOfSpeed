using UnityEngine;

public static class PlayerInput
{
    private static float lookAngle = 0f;    //保持相机注视目标
    private static float tiltAngle = 0f;    //相机倾斜角

    
    public static float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360f || angle > 360f)
        {
            if (angle < -360f)
            {
                angle += 360f;
            }
            else if (angle > 360f)
            {
                angle -= 360f;
            }
        }
        //Scripting API: Mathf.Clamp - Unity - Manual
        return Mathf.Clamp(angle, min, max);
    }
    public static Quaternion GetMouseRotationInput(float mouseSensitivity = 4f, float minTiltAngle = -75f, float maxTiltAngle = 60f)
    {

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Adjust the look angle (Y Rotation)
        lookAngle += mouseX * mouseSensitivity;
        lookAngle %= 360f;

        // Adjust the tilt angle (X Rotation)
        tiltAngle += mouseY * mouseSensitivity;
        tiltAngle %= 360f;
        tiltAngle = ClampAngle(tiltAngle, minTiltAngle, maxTiltAngle);

        var controlRotation = Quaternion.Euler(-tiltAngle, lookAngle, 0f);
        return controlRotation;
    }
    //获取键盘输入
    public  static bool GetOne()
    {
        return Input.GetKeyDown(KeyCode.Alpha1);
    }

    public static bool GetTwo()
    {
        return Input.GetKeyDown(KeyCode.Alpha2);
    }

    public static bool GetThree()
    {
        return Input.GetKeyDown(KeyCode.Alpha3);
    }

    public static bool GetR()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    
}