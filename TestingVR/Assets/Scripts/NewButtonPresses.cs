using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

//[RequireComponent(typeof(Rigidbody))]        //This is if it's using a rigidbody, the Hurricane uses a charactercontroller, so this needs to be off

public class NewButtonPresses : MonoBehaviour
{
    [SerializeField]
    InputActionReference AButton;
    [SerializeField]
    InputActionReference BButton;
    [SerializeField]
    InputActionReference XButton;
    [SerializeField]
    InputActionReference YButton;

    [SerializeField]
    InputActionReference ToggleWeapon;
    [SerializeField]
    InputActionReference MenuButton;

    [SerializeField]
    float JumpForce = 0.17f;


    XROrigin XRRigRef;
    CapsuleCollider XRCollider;
    //Rigidbody RBody;

    [SerializeField]
    public GameObject GroundChecker;

    /*public LayerMask GroundedMask;
    bool IsGrounded => Physics.CheckSphere(GroundChecker.transform.position, 0.1f, GroundedMask);*/

    /*bool IsGrounded => Physics.Raycast(                                //the => just makes it that, whenever this variable is called, it does this stuff, casting a ray and whatnot
    new Vector2(transform.position.x, transform.position.y + 2f),
    Vector3.down, 2f);*/

    bool IsGrounded = true;

    public Ray GroundRay;
    public RaycastHit RayHit;
    //---------------------------

    [System.NonSerialized]
    public CharacterController CharController;

    [System.NonSerialized]
    public Vector3 ToMove;

    private Vector3 DownDir;

    [SerializeField]
    public LayerMask GroundLayer;



    private void Start()
    {
        XRRigRef = GetComponent<XROrigin>();
        //RBody = GetComponent<Rigidbody>();                           //This is if it's using a rigidbody, the Hurricane uses a charactercontroller, so this needs to be off
        CharController = GetComponent<CharacterController>();
        AButton.action.performed += AButtonMethod;
        BButton.action.performed += BButtonMethod;
        XButton.action.performed += XButtonMethod;
        YButton.action.performed += YButtonMethod;
        ToggleWeapon.action.performed += ToggleWeaponMethod;
        MenuButton.action.performed += MenuButtonMethod;
    }

    void Awake()
    {

    }

    void AButtonMethod(InputAction.CallbackContext obj)
    {
        
    }
    void BButtonMethod(InputAction.CallbackContext obj)
    {
        Debug.Log("bbutton");
    }
    void XButtonMethod(InputAction.CallbackContext obj)
    {
        Debug.Log("xbutton");
    }
    void YButtonMethod(InputAction.CallbackContext obj)
    {
        Debug.Log("ybutton");
    }

    void ToggleWeaponMethod(InputAction.CallbackContext obj)
    {
        //Debug.Log(obj.action.ReadValue<Vector2>());
        if (obj.action.ReadValue<Vector2>().y > 0.5)
        {
            Debug.Log("right stick +Y");
            CheckGrounding();
            if (!IsGrounded) return;                         //return thing, it's saying if IsGrounded is NOT true, then return, meaning, don't do the rest of the method
            //RBody.AddForce(Vector3.up * JumpForce);        //This is if it's using a rigidbody, the Hurricane uses a charactercontroller, so this needs to be off
            ToMove.Set(0f, JumpForce, 0f);
            //CharController.Move(ToMove);
        }
    }
    void MenuButtonMethod(InputAction.CallbackContext obj)
    {
        Debug.Log("menubutton");
    }




    private void CheckGrounding()
    {
        //Debug.Log(this.transform.forward);
        DownDir.Set(0, -1, 0);
        GroundRay = new Ray(this.transform.position, (DownDir));
        if (Physics.Raycast(GroundRay, out RayHit, 0.5f, GroundLayer))
        {
            IsGrounded = true;
            //Debug.Log("hit " + RayHit.transform.gameObject.name);
        }
        else
        {
            IsGrounded = false;
        }  //*/
        //IsGrounded = Physics.Raycast(new Vector2(transform.position.x, transform.position.y + 2f), Vector3.down, 2f);
    }

    private void Update()
    {
        CharController.Move(ToMove);
        //CharController.Move(ToMove);
        //ToMove.y -= 0.003f;
        CheckGrounding();
        if (IsGrounded)
        {
            //ToMove.y = 0;
        }
        else
        {
            ToMove.y -= 0.003f;

        }
    }


}
