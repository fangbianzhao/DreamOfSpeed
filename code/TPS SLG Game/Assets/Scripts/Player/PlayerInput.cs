using UnityEngine;

public static class PlayerInput
{
    private static float lookAngle = 0f;
    private static float tiltAngle = 0f;


    public static Vector3 GetMovementInput(Camera relativeCamera)
    {
        Vector3 moveVector;//内置单位向量结构体
        float horizontalAxis = Input.GetAxis("Horizontal");//get mouse input
        float verticalAxis = Input.GetAxis("Vertical");

        if (relativeCamera != null)
        {
            // Calculate the move vector relative to camera rotation
            Vector3 scalerVector = new Vector3(1f, 0f, 1f);
            Vector3 cameraForward = Vector3.Scale(relativeCamera.transform.forward, scalerVector).normalized;
            Vector3 cameraRight = Vector3.Scale(relativeCamera.transform.right, scalerVector).normalized;

            moveVector = (cameraForward * verticalAxis + cameraRight * horizontalAxis);
        }
        else
        {
            // Use world relative directions
            moveVector = (Vector3.forward * verticalAxis + Vector3.right * horizontalAxis);
        }

        if (moveVector.magnitude > 1f)
        {
            moveVector.Normalize();
        }

        return moveVector;
    }

    public static Quaternion GetMouseRotationInput(float mouseSensitivity = 4f, float minTiltAngle = -75f, float maxTiltAngle = 60f)
    {

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Adjust the look angle (Y Rotation)
        lookAngle += mouseX * mouseSensitivity;
        lookAngle %= 360f;

        // Adjust the tilt angle (X Rotation) used by camera 
        tiltAngle += mouseY * mouseSensitivity;
        tiltAngle %= 360f;
        tiltAngle = MathfExtensions.ClampAngle(tiltAngle, minTiltAngle, maxTiltAngle);

        var controlRotation = Quaternion.Euler(-tiltAngle, lookAngle, 0f);
        return controlRotation;
    }
    //using Input class to chect equipment input
    public static bool GetJumpInput(
    {
    //jump control
        return Input.GetButtonDown("Jump");
    }

    public static bool GetFire1()
    {
    //fire 1 control
        return Input.GetButton("Fire1");
    }

    public static bool GetFire2()
    {
    //fire 2 control
        return Input.GetButton("Fire2");
    }

    public static bool GetQ()
    {
    //key q control
        return Input.GetKeyDown(KeyCode.Q);
    }

    public static bool GetE()
    {
    //key e down control
        return Input.GetKeyDown(KeyCode.E);
    }

    public static bool GetShirt()
    {
    //get shirt control
        return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
    }

    public  static bool GetOne()
    {
    //get one control
        return Input.GetKeyDown(KeyCode.Alpha1);
    }

    public static bool GetTwo()
    {
    //get 2 control
        return Input.GetKeyDown(KeyCode.Alpha2);
    }

    public static bool GetThree()
    {
    //get 3 control
        return Input.GetKeyDown(KeyCode.Alpha3);
    }

    public static bool GetR()
    {
    //key r control
        return Input.GetKeyDown(KeyCode.R);
    }

}
