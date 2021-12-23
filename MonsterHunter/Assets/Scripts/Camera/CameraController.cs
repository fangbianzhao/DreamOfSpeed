using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Const var
    private const float minSpeedDamp = 0f;
    private const float maxSpeedDamp = 1f;
    private const float minRotation = 0f;
    private const float maxRotation = 100f;
    
    
    //Serializable var
    [SerializeField]
    private Transform target = null;//用来存放摄像机要跟随的物体

    [SerializeField] [Range(minSpeedDamp, maxSpeedDamp)]
    private float speedDamp = minSpeedDamp;

    [SerializeField] [Range(minRotation, maxRotation)]
    //摄像头转动的速度
    private float rotation = 60f;

    public Quaternion controlRotation;

    private Transform cameraRig;    //相机追踪物体 
    private Transform cameraPivot;  //相机旋转的轴心
    private Quaternion pivotTargetLocalRotation; // Controls the X Rotation (Tilt Rotation)
    private Quaternion rigTargetLocalRotation; // Controlls the Y Rotation (Look Rotation)
    private Vector3 cameraVelocity; // The velocity at which the camera moves
    private PlayerHealth playerHealth;

    protected void Awake()
    {
        this.cameraPivot = this.transform.parent;
        this.cameraRig = this.cameraPivot.parent;

        this.transform.localRotation = Quaternion.identity;
        this.playerHealth = target.GetComponent<PlayerHealth>();
    }
    
    protected virtual void FixedUpdate()
    {
        controlRotation = PlayerInput.GetMouseRotationInput();
        this.UpdateRotation(controlRotation);
    }

    protected virtual void LateUpdate()
    {
        this.FollowTarget();
    }

    private void FollowTarget()
    {
        if (this.target == null)
        {
            return;
        }

        this.cameraRig.position = Vector3.SmoothDamp(this.cameraRig.position, this.target.transform.position, ref this.cameraVelocity, this.speedDamp);
    }

    private void UpdateRotation(Quaternion controlRotation)
    {
        if (this.target != null)
        {
            // Y Rotation (Look Rotation)
            this.rigTargetLocalRotation = Quaternion.Euler(0f, controlRotation.eulerAngles.y, 0f);

            // X Rotation (Tilt Rotation)
            this.pivotTargetLocalRotation = Quaternion.Euler(controlRotation.eulerAngles.x, 0f, 0f);

            if (this.rotation > 0.0f)
            {
                this.cameraPivot.localRotation =
                    Quaternion.Slerp(this.cameraPivot.localRotation, this.pivotTargetLocalRotation, this.rotation * Time.deltaTime);

                this.cameraRig.localRotation =
                    Quaternion.Slerp(this.cameraRig.localRotation, this.rigTargetLocalRotation, this.rotation * Time.deltaTime);
            }
            else
            {
                this.cameraPivot.localRotation = this.pivotTargetLocalRotation;
                this.cameraRig.localRotation = this.rigTargetLocalRotation;
            }
            // if player is still alive
            if (this.playerHealth.currentHealth > 0)
            {
                // update player rotation
                target.rotation = this.rigTargetLocalRotation * this.pivotTargetLocalRotation;
            }
        }
    }
    
}