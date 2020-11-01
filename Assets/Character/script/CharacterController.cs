using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class CharacterController : MonoBehaviour
{
    public Transform followCamera;
    PlayerMovement controller;

    [Header("speed")]
    public float walk = 0.5f;
    public float run = 1.0f;

    void Start()
    {
        controller = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
        float speed = InputControl(moveInput);
        controller.LookAround(ref followCamera, Input.GetAxis("Mouse X"));
        controller.Move(moveInput.normalized, speed);
        controller.SetAnimatorParameters(speed, moveInput);
    }

    float InputControl(Vector3 veloctity)
    {
        if (veloctity.x == 0 && veloctity.z == 0)
        {
            return 0;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return run;
        }
        return walk;
    }
}