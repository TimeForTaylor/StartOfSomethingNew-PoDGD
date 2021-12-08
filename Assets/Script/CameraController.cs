using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject Target;
    
    [Header("Position Locking")]
    public bool UsePositionLocking = false;
    public Vector3 PositionOffset = new Vector3(0f, 0f, -10f);
    
    [Header("Curb Motion")]
    public bool UseCurbMotion = false;
    public Bounds CurbMotionBounds = new Bounds(Vector3.zero, Vector3.one);
    public Vector3 CurbPositionOffset = new Vector2(0f, 0f);

    private Vector3 currentDampingVelocity = Vector3.zero;
    public bool damping;
    public float SmoothTime = 0.5f;

    [Header("Velocity Zoom")] 
    public bool UseVelocityZoom = false;
    public Rigidbody2D TargetBody;
    public Camera myCamera;
    
    public float CameraMinSize = 5f;
    public float CameraMaxSize = 10f;    
    
    public float VelocityMin = 0.5f;
    public float VelocityMax = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        TargetBody = Target.GetComponent<Rigidbody2D>();
        myCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (UsePositionLocking)
        {
            PositionLocking();
        }

        if (UseCurbMotion)
        {
            CurbMotion();
        }

        if (UseVelocityZoom)
        {
            VelocityZoom();
        }
    }

    void VelocityZoom()
    {
        if (TargetBody == null)
        {
            return;
        }
        var velocity = TargetBody.velocity.magnitude;
        var factor = Mathf.InverseLerp(VelocityMin, VelocityMax, velocity);

        myCamera.orthographicSize = Mathf.SmoothStep(CameraMinSize, CameraMaxSize, factor);

    }
    void CurbMotion()
    {
        CurbMotionBounds.center = transform.position + CurbPositionOffset;
        
        // if the target is outside of the box:
        // find the distance between the nearest edge and the target
        Vector3 targetPosition = Target.transform.position;

        var xOffset = 0f;
        var yOffset = 0f;
        
        if (targetPosition.x < CurbMotionBounds.min.x)
        {
            xOffset = targetPosition.x - CurbMotionBounds.min.x;
        }
        else if (targetPosition.x > CurbMotionBounds.max.x)
        {
            xOffset = targetPosition.x - CurbMotionBounds.max.x;
        }
        
        if (targetPosition.y < CurbMotionBounds.min.y)
        {
            yOffset = targetPosition.y - CurbMotionBounds.min.y;
        }
        else if (targetPosition.y > CurbMotionBounds.max.y)
        {
            yOffset = targetPosition.y - CurbMotionBounds.max.y;
        }
        
        // then add that to the camera position to move the camera with the target
        //transform.position = transform.position + new Vector3(xOffset,yOffset);
        if (damping)
        {
            transform.position = Vector3.SmoothDamp(transform.position, transform.position + new Vector3(xOffset, yOffset),
                ref currentDampingVelocity, SmoothTime);
        }
        else
        {
            transform.position = transform.position + new Vector3(xOffset,yOffset);
        }
    }
    
    void PositionLocking()
    {
        if (Target != null)
        {
            transform.position = Target.transform.position + PositionOffset;
        }
    }
    
    private void OnDrawGizmos()
    {
       
        if (Target && UsePositionLocking)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(Target.transform.position, new Vector3(.1f, .1f));
            Gizmos.color = Color.green;
            Gizmos.DrawCube(Target.transform.position + (Vector3) (Vector2) PositionOffset, new Vector3(.1f, .1f));
            Gizmos.color = Color.yellow;
            var planarOffset = PositionOffset;
            planarOffset.z = Target.transform.position.z;
            Gizmos.DrawLine(Target.transform.position + planarOffset, Target.transform.position);
        }

        if (Target && UseCurbMotion)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(Target.transform.position, new Vector3(.1f, .1f));
            
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube((Vector2)CurbMotionBounds.center,CurbMotionBounds.size);
        }
    }
}
