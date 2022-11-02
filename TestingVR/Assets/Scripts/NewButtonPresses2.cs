using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.HandPoser;
using HurricaneVR.Framework.Shared;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;
using HurricaneVR.Framework.Core.Player;
using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.ControllerInput;

namespace HurricaneVR.TechDemo.Scripts
{
    public class NewButtonPresses2 : MonoBehaviour
    {
        public HVRHandGrabber Grabber { get; set; }
        public HVRGrabbable Grabbable;
        public HVRGrabbable GrabbableBow;
        public HVRGrabbable GrabbableSword;

        public HVRGrabbable GrabbableKnife;
        public HVRGrabbable GrabbablePistol;
        public HVRGrabbable GrabbablePistolAmmo;
        [System.NonSerialized] public HVRGrabbable GrabbablePistolAmmoInstance;
        public HVRGrabbable GrabbableSMG;
        public HVRGrabbable GrabbableSMGAmmo;
        [System.NonSerialized] public HVRGrabbable GrabbableSMGAmmoInstance;
        public HVRGrabbable GrabbableShotgun;
        public HVRGrabbable GrabbableShotgunAmmo;
        [System.NonSerialized] public HVRGrabbable GrabbableShotgunAmmoInstance;

        public HVRGrabTrigger GrabTrigger;
        public HVRPosableGrabPoint GrabPoint;

        [SerializeField]
        public bool IsRight;
        public HVRGrabbable ToDeactivate;

        //My stuff I've added
        [SerializeField]
        InputActionReference AButton;
        [SerializeField]
        InputActionReference BButton;
        [SerializeField]
        InputActionReference XButton;
        [SerializeField]
        InputActionReference YButton;

        [SerializeField]
        InputActionReference LGButton;
        private float LGValue = 0;
        /*[SerializeField]
        InputActionReference LTButton;
        private float LTValue = 0;
        [SerializeField]
        InputActionReference RGButton;
        private float RGValue = 0;
        [SerializeField]
        InputActionReference RTButton;
        private float RTValue = 0;*/


        [SerializeField]
        InputActionReference RightStickTilt;
        [SerializeField]
        InputActionReference LGRightStickTilt;
        [SerializeField]
        InputActionReference MenuButton;

        [SerializeField]
        float JumpForce = 0.17f;

        [SerializeField]
        int RightStickDelayFull = 40;
        int RightStickDelay;
        public int CurrentEquipped = 0;
        public int CurrentArrow = 0;

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

        [SerializeField]
        public bool Guns;
        //---------------------------

        [System.NonSerialized]
        public CharacterController CharController;

        [System.NonSerialized]
        public Vector3 ToMove;

        private Vector3 DownDir;

        [SerializeField]
        public LayerMask GroundLayer;


        public void Start()
        {
            //THIS IS THE ORIGINAL CODE FOR IT, HELP FINDING IT, puts the object in the right hand
            Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);

            //This one has LAST instead, which makes it the left hand
            //Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);

            //My stuff I've added
            XRRigRef = GetComponent<XROrigin>();
            //RBody = GetComponent<Rigidbody>();                           //This is if it's using a rigidbody, the Hurricane uses a charactercontroller, so this needs to be off
            CharController = GetComponent<CharacterController>();
            AButton.action.performed += AButtonMethod;
            BButton.action.performed += BButtonMethod;
            XButton.action.performed += XButtonMethod;
            YButton.action.performed += YButtonMethod;
            LGButton.action.performed += LGButtonMethod;
            RightStickTilt.action.performed += RightStickTiltMethod;
            LGRightStickTilt.action.performed += LGRightStickTiltMethod;
            MenuButton.action.performed += MenuButtonMethod;

            RightStickDelay = RightStickDelayFull;
            CurrentEquipped = 0;
        }

        

        void AButtonMethod(InputAction.CallbackContext obj)
        {
            //Debug.Log("abutton " + CurrentEquipped);
            if (Guns)
            {
                CurrentEquipped += 1;
                if (CurrentEquipped >= 24)
                {
                    CurrentEquipped = 0;
                }
                else if ((CurrentEquipped > 0) & (CurrentEquipped < 20))
                {
                    CurrentEquipped = 20;
                }

                if (CurrentEquipped == 0) //Empty Hand
                {
                    Grabber.ForceRelease();
                    GrabbableShotgun.gameObject.SetActive(false);
                }
                else if (CurrentEquipped == 20) //Knife Equipped
                {
                    //This is if you want it to go in the right hand
                    Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);
                    Grabber.ForceRelease();
                    Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);

                    Grabber.ForceRelease();
                    GrabbableKnife.gameObject.SetActive(true);
                    Grabber.Grab(GrabbableKnife, GrabTrigger, GrabPoint);
                }
                else if (CurrentEquipped == 21) //Pistol Equipped
                {
                    //This is if you want it to go in the right hand
                    Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);
                    Grabber.ForceRelease();
                    Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);

                    Grabber.ForceRelease();
                    GrabbableKnife.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                    GrabbableKnife.gameObject.SetActive(false);
                    GrabbablePistol.gameObject.SetActive(true);
                    Grabber.Grab(GrabbablePistol, GrabTrigger, GrabPoint);
                }
                else if (CurrentEquipped == 22) //SMG Equipped
                {
                    //This is if you want it to go in the right hand
                    Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);
                    Grabber.ForceRelease();
                    Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);

                    Grabber.ForceRelease();
                    GrabbablePistol.gameObject.SetActive(false);
                    GrabbableSMG.gameObject.SetActive(true);
                    Grabber.Grab(GrabbableSMG, GrabTrigger, GrabPoint);
                }
                else if (CurrentEquipped == 23) //Shotgun Equipped
                {
                    //This is if you want it to go in the right hand
                    Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);
                    Grabber.ForceRelease();
                    Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);

                    Grabber.ForceRelease();
                    GrabbableSMG.gameObject.SetActive(false);
                    GrabbableShotgun.gameObject.SetActive(true);
                    Grabber.Grab(GrabbableShotgun, GrabTrigger, GrabPoint);
                }
            }

        }
        void BButtonMethod(InputAction.CallbackContext obj)
        {
            //Debug.Log("bbutton");
            if (Guns)
            {
                if (CurrentEquipped == 21) //Pistol ammo
                {
                    HVRGrabbable NewAmmo = Instantiate(GrabbablePistolAmmo, GrabbablePistol.transform.position, GrabbablePistol.transform.rotation);

                    //This is if you want to let go of left hand, instantiate something, and put it in the left hand
                    Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);
                    Grabber.ForceRelease();
                    GrabbablePistolAmmoInstance = NewAmmo;
                    Grabber.Grab(GrabbablePistolAmmoInstance, GrabTrigger, GrabPoint);
                }
                else if (CurrentEquipped == 22) //SMG ammo
                {
                    HVRGrabbable NewAmmo = Instantiate(GrabbableSMGAmmo, GrabbablePistol.transform.position, GrabbablePistol.transform.rotation);

                    //This is if you want to let go of left hand, instantiate something, and put it in the left hand
                    Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);
                    Grabber.ForceRelease();
                    GrabbableSMGAmmoInstance = NewAmmo;
                    Grabber.Grab(GrabbableSMGAmmoInstance, GrabTrigger, GrabPoint);
                }
                else if (CurrentEquipped == 23) //Shotgun ammo
                {
                    HVRGrabbable NewAmmo = Instantiate(GrabbableShotgunAmmo, GrabbablePistol.transform.position, GrabbablePistol.transform.rotation);

                    //This is if you want to let go of left hand, instantiate something, and put it in the left hand
                    Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);
                    Grabber.ForceRelease();
                    GrabbableShotgunAmmoInstance = NewAmmo;
                    Grabber.Grab(GrabbableShotgunAmmoInstance, GrabTrigger, GrabPoint);
                }
            }
        }
        void XButtonMethod(InputAction.CallbackContext obj)
        {
            //Debug.Log("xbutton");
        }
        void YButtonMethod(InputAction.CallbackContext obj)
        {
            if (LGValue <= 0)
            {
                //Debug.Log("ybutton");
            }
        }

        void LGButtonMethod(InputAction.CallbackContext obj)
        {
            LGValue = obj.action.ReadValue<float>();
            FindObjectOfType<HVRPlayerController>().GetComponent<HVRPlayerController>().LGValue = LGValue;
            FindObjectOfType<ListVariables>().GetComponent<ListVariables>().LGValue = LGValue;
            //Debug.Log("LGbutton " + obj.action.ReadValue<float>());
        }

        void MenuButtonMethod(InputAction.CallbackContext obj)
        {
            //Debug.Log("menubutton");
        }


        public void RightStickTiltMethod(InputAction.CallbackContext obj) //My method I've added
        {
            //Debug.Log("right stick " + obj.action.ReadValue<Vector2>());
            if (obj.action.ReadValue<Vector2>().y > 0.7 && LGValue <= 0.2 && obj.action.ReadValue<Vector2>().x < 0.3 && obj.action.ReadValue<Vector2>().x > -0.3)
            {
                CheckGrounding();
                if (!IsGrounded) return;                         //return thing, it's saying if IsGrounded is NOT true, then return, meaning, don't do the rest of the method
                //RBody.AddForce(Vector3.up * JumpForce);        //This is if it's using a rigidbody, the Hurricane uses a charactercontroller, so this needs to be off
                ToMove.Set(0f, JumpForce, 0f);
                //CharController.Move(ToMove);
            }
        }

        public void LGRightStickTiltMethod(InputAction.CallbackContext obj)
        {
            if (!Guns)
            {
                //Debug.Log(CurrentEquipped + " " + RightStickDelay + " " + obj.action.ReadValue<Vector2>().x);
                if (RightStickDelay >= RightStickDelayFull)
                {
                    RightStickDelay = 0;

                    if (obj.action.ReadValue<Vector2>().x >= 0.01)
                    {
                        CurrentEquipped += 1;
                        if (CurrentEquipped >= 3)
                        {
                            CurrentEquipped = 0;
                        }
                    }
                    else if (obj.action.ReadValue<Vector2>().x <= -0.01)
                    {
                        CurrentEquipped -= 1;
                        if (CurrentEquipped < 0)
                        {
                            CurrentEquipped = 2;
                        }
                    }

                    if (CurrentEquipped == 0) //Empty Hand
                    {
                        Grabber.ForceRelease();
                        GrabbableBow.gameObject.SetActive(false);
                        GrabbableSword.gameObject.SetActive(false);
                    }
                    else if (CurrentEquipped == 1) //Bow Equipped
                    {
                        //This is if you want it to go in the left hand
                        Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);
                        Grabber.ForceRelease();
                        Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);

                        Grabber.ForceRelease();
                        GrabbableSword.gameObject.SetActive(false);
                        GrabbableBow.gameObject.SetActive(true);
                        Grabber.Grab(GrabbableBow, GrabTrigger, GrabPoint);
                    }
                    else if (CurrentEquipped == 2) //Sword Equipped
                    {
                        //This is if you want it to go in the right hand
                        Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);
                        Grabber.ForceRelease();
                        Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);

                        Grabber.ForceRelease();
                        GrabbableBow.gameObject.SetActive(false);
                        GrabbableSword.gameObject.SetActive(true);
                        Grabber.Grab(GrabbableSword, GrabTrigger, GrabPoint);
                    }




                    //This whole area is the old system
                    /*if (GrabTrigger == HVRGrabTrigger.ManualRelease && Grabber.GrabbedTarget == Grabbable)
                    {
                        ToDeactivate = Grabbable;
                        Grabber.ForceRelease();
                        ToDeactivate.gameObject.SetActive(false);
                        FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld.gameObject.SetActive(false);
                        return;
                    }

                    //grabber needs to have it's release sequence completed if it's holding something
                    if (Grabber.IsGrabbing)
                    {
                        ToDeactivate = Grabbable;
                        Grabber.ForceRelease();
                        ToDeactivate.gameObject.SetActive(false);
                        FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld.gameObject.SetActive(false);
                    }

                    if (FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld != null)  //This whole if statement seems really stupid, if it doesn't exist then disable it?
                    {
                        FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld.gameObject.SetActive(false);
                    }

                    Grabbable.gameObject.SetActive(true);
                    Grabber.Grab(Grabbable, GrabTrigger, GrabPoint);
                    FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld = Grabbable.gameObject;
                    */
                }
            }
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
            if (!FindObjectOfType<HVRPlayerController>().GetComponent<HVRPlayerController>().IsClimbing)
            {
                CharController.Move(ToMove);
            }
            //CharController.Move(ToMove);
            //ToMove.y -= 0.003f;
            CheckGrounding();
            if (IsGrounded)
            {
                //ToMove.y = 0;
            }
            else
            {
                if (!FindObjectOfType<HVRPlayerController>().GetComponent<HVRPlayerController>().IsClimbing)
                {
                    ToMove.y -= 0.003f;
                }
            }

            //RightStickDelay stops constant changes if you hold the stick, making weapon changes crazy
            if (RightStickDelay < RightStickDelayFull)
            {
                RightStickDelay += 1;
            }
        }

    }
}
