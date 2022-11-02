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

namespace HurricaneVR.TechDemo.Scripts
{
    public class DemoCodeGrabbing : MonoBehaviour
    {
        public HVRHandGrabber Grabber { get; set; }
        public HVRGrabbable Grabbable;
        public HVRGrabTrigger GrabTrigger;
        public HVRPosableGrabPoint GrabPoint;

        [SerializeField]
        public bool IsRight;
        public HVRGrabbable ToDeactivate;

        //My stuff I've added
        [SerializeField]
        InputActionReference ToggleWeapon;

        public void Start()
        {
            //THIS IS THE ORIGINAL CODE FOR IT, HELP FINDING IT, puts the object in the right hand
            Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);

            //This one has LAST instead, which makes it the left hand
            //Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);

            //My stuff I've added
            if (ToggleWeapon != null)
            ToggleWeapon.action.performed += ToggleWeaponMethod;
        }

        public void ToggleWeaponMethod(InputAction.CallbackContext obj) //My method I've added
        {
            Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);
            Grabber.ForceRelease();
            Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);

            if (GrabTrigger == HVRGrabTrigger.ManualRelease && Grabber.GrabbedTarget == Grabbable)
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

            if (FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld != null)
            {
                FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld.gameObject.SetActive(false);
            }
            Grabbable.gameObject.SetActive(true);
            Grabber.Grab(Grabbable, GrabTrigger, GrabPoint);
            FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld = Grabbable.gameObject;
        }




        public void Grab()
        {
            if (IsRight)
            {
                Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);
                Grabber.ForceRelease();
                Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);
            }
            else
            {
                Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);
                Grabber.ForceRelease();
                Grabber = GameObject.FindObjectsOfType<HVRHandGrabber>().Last(e => e.gameObject.activeInHierarchy);
            }

            //if (Grabbable && Grabber)    THIS WAS THE ORIGINAL CODE, HELP TO FIND THIS
            if (Grabber)
            {
                if (GrabTrigger == HVRGrabTrigger.ManualRelease && Grabber.GrabbedTarget == Grabbable)
                {
                    ToDeactivate = Grabbable;
                    Grabber.ForceRelease();
                    ToDeactivate.gameObject.SetActive(false);
                    FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld.gameObject.SetActive(false);
                    return;
                }

                //grabber needs to have it's release sequence completed if it's holding something
                if(Grabber.IsGrabbing)
                {
                    ToDeactivate = Grabbable;
                    Grabber.ForceRelease();
                    ToDeactivate.gameObject.SetActive(false);
                    FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld.gameObject.SetActive(false);
                }

                if (FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld != null)
                {
                    FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld.gameObject.SetActive(false);
                }
                Grabbable.gameObject.SetActive(true);
                Grabber.Grab(Grabbable, GrabTrigger, GrabPoint);
                FindObjectOfType<ListVariables>().GetComponent<ListVariables>().CurrentlyHeld = Grabbable.gameObject;
            }
        }
    }
}
