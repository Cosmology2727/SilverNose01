using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference JumpActionReference;

    [SerializeField]
    private float JumpForce = 500f;

    private Rigidbody RBody;

    

    private bool IsGrounded => Physics.Raycast(
        new Vector2(transform.position.x, transform.position.y + 1.8f),
        Vector3.down, 1.8f);

    void Start()
    {
        RBody = GetComponent<Rigidbody>();
        JumpActionReference.action.performed += OnJump;
    }


    void Update()
    {
        
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        if (!IsGrounded) return;
        RBody.AddForce(Vector3.up * JumpForce);
    }
}
