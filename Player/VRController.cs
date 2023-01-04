using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using RootMotion.Demos;
using BNG;
using DarkDemon;

namespace LazyPanda
{
    public class VRController : MonoBehaviour
    {

        //GameObject Player;
        // InputBridge VRInputs;

        
       
        UserControlThirdPerson VRThirdPersonController;

        VR3DPlayer Player3D;

        private Transform HipBone;

        public Rigidbody HipRigid { get; set; }

        InputBridge Bridge;
        GameController Controller;
        private void Start()
        {

            Controller = Instancer.GameControllerInstance;
           // Player = Instancer.GetPlayer;
            Player3D = GetComponentInParent<VR3DPlayer>();

            if (Player3D)
            {
                HipBone = Player3D.GetHipBone;

                if (!HipBone)
                {
                    Player3D.BodyPartsSetup();
                    HipBone = Player3D.GetHipBone;

                    
                }

                HipRigid = HipBone.GetComponent<Rigidbody>();

            }


            // VRInputs = Player.GetComponent<InputBridge>();

            Bridge = InputBridge.Instance;
            VRThirdPersonController = GetComponent<UserControlThirdPerson>();

        }

        private void Update()
        {
            if (!Controller) return;
            if (Controller.GamePaused) return;

          //  if (!VRInputs) { Debug.LogError("no vr input"); return; };
            if (!VRThirdPersonController) { Debug.LogError("no third person"); return; };
            if (!VRThirdPersonController.AllowVRControl) { Debug.Log("Vr control not active"); return; }
            if (!Player3D) { Debug.LogError("Humansbase not found");return; }
            

            ThisCharacterInControl();
            //Interact();

            
        }


        private void ThisCharacterInControl()
        {
            if (Player3D.CurrentState == VR3DCharacterBase.CharacterState.InControl)
            {
                if (Controller.DebugMode)
                {
                    float h = Input.GetAxisRaw("Horizontal");
                    float v = Input.GetAxisRaw("Vertical");
                    bool j = Input.GetButton("Jump");

                    VRThirdPersonController.VectorMove = new Vector2(h, v);
                    VRThirdPersonController.Jumping = j;
                }
                else
                {
                    VRThirdPersonController.VectorMove = Bridge.LeftThumbstickAxis;

                    VRThirdPersonController.Jumping = Bridge.AButtonDown;
                }

                
            }
            else
            {
                VRThirdPersonController.VectorMove = Vector3.zero;

                VRThirdPersonController.Jumping = false;
            }
        }

        private void Interact()
        {
            if (Player3D.CurrentState != VR3DCharacterBase.CharacterState.InControl) return;

            if (InputBridge.Instance.RightTriggerDown)
            {
               // Player3D.InteractionDecider();
               
                // this is for lifting the objects which we are not doing at
               //the moment but if we do, then enable then uncomment it 
            }


        }

        //not in use...
        private void Player3DInControl()
        {
            bool val = ButtonInput.Instance.BButtonDown;

            if (val)
            {
                if (Player3D.CurrentState != VR3DCharacterBase.CharacterState.InControl)
                {
                   // Player3D.ToInControlState();

                }
            }
        }
        public IEnumerator JumpFromHanging()
        {
            

            if (Player3D.IsHanging)
            {
                //if jump is pressed while hanging, this is called;

                Player3D.JumpWhileHanging();

                VRThirdPersonController.ControlInAir = true;

                yield return new WaitForSeconds(2);

                VRThirdPersonController.ControlInAir = false;

                Player3D.AliveFromHanging();
                
                //Player3D.AliveFromLowPin();
            }
          
            

        }

    }
}

