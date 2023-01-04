using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using RootMotion.Demos;
using DarkDemon;
//using StarterAssets;

namespace LazyPanda
{
    public class VRMainPlayer : MainPlayerBase
    {
        [SerializeField] SharedBool LookToChangePlayer;
        [SerializeField] LayerMask CharactersDetectHitLayer;
        [SerializeField] SharedBool PlayerInControl;

        DarkDemon.Outline OutlineOnCharacter;
        VR3DCharacterBase CurrentCharacter;
        private const int LenghtOfRay = 50;
        const float RadiusOfRay = 1.5f;
     

        
        EnvironmentBase CurrentControllable;
        EnvironmentBase LastHeliControlled;
        EnvironmentBase LastControlledAI;

        EnvironmentBase CurrentEnvironmentBaseControlling;

        InputBridge Bridge;
        public bool Show3DViewer { get; private set; } = false;
        
        GameController Controller;

        private bool AFlip = false;

        private enum InControlSwitch { MainPlayer, Dummb, Heli, ControlledAI }
        InControlSwitch Current = InControlSwitch.MainPlayer;
       
        public bool IsPlayerInControl { get { return PlayerInControl.Value; } }



        //Keyboard key values 
        bool Down1 = false;
        bool Down2 = false;


        protected override void Awake()
        {
            base.Awake();
           
            PlayerInControl.Value = true;

            Bridge = InputBridge.Instance;
            Controller = Instancer.GameControllerInstance;

            CurrentCharacter = Instancer.GetPlayer3D;
        }



        protected override void Update()
        {
            if (!Controller) return;

            if (Controller.GamePaused) return;
            if (!Controller.DebugMode) { if (!AllowControlShift) return; }

            KeyboardInput();

            //if player looking at environmentbase, and button press, then switch control.
            //if player looking in void and dummb's camera looking at environmentbase, then switch control to dummb perspective.

            //Decides if the player is looking at an EnvironmentBase type which we can control...
            if (LookToChangePlayer.Value && !Show3DViewer)
            {
                if(!RayCastHittingEnvironmentBase(Head.position, Head.forward, RadiusOfRay + 2))
                {
                    NotLooking();
                }

            }
            else if(CurrentCharacter.CurrentState == VR3DCharacterBase.CharacterState.InControl && Show3DViewer)
            {
                // Transform cameraT = CurrentCharacter.GetcharacterCamera.transform;

                /*if(!RayCastHittingEnvironmentBase(cameraT.position, cameraT.forward, RadiusOfRay + 8))
                {
                    NotLooking();
                }*/

                NotLooking(); //delete this if uncommenting the above code.
            }
            else
            {
                NotLooking();
            }
            

            
            //if the player is looking at an EnvironmentBase type and then we press B/Y, the control shifts to
            //that character..
            if (CurrentControllable)
            {
                if(CurrentControllable != CurrentEnvironmentBaseControlling)
                {
                    if (Bridge.BButtonDown || Down1) 
                    {
                        //condition ? consequent : alternative

                        //In Debug mode, we do not need Show3dviewer to show up 
                        //because we are using the 3rd person camera.
                        Show3DViewer = true;

                    }
                    if (Bridge.YButtonDown || Down2) 
                    { //if (CurrentControllable.GetComponent<MyHeliController>()) { Show3DViewer = true; }
                        //else { Show3DViewer = false; } 

                        Show3DViewer = false;
                    }

                    if (Bridge.YButtonDown || Bridge.BButtonDown || Down1 || Down2)
                    {
                        ControlEnvironmentBaseObject();
                    }
                }

                
            }
            
            //if we are not looking at any EnvironmentBase character and we press B/Y, the 
            //the controls switches between the mainPlayer and Dummb..
            else
            {
                if (Bridge.YButtonDown || Down2)
                {
                    if (!PlayerInControl.Value)
                    {
                        MainPlayerInControl();
                        Current = InControlSwitch.MainPlayer;
                    }
                    else
                    {
                        ControlDummb();
                        Current = InControlSwitch.Dummb;
                    }

                    //if the heli has been once activated by the player, it is added into auto controls,
                    //so now when the player can switch between mainplayer, dummb and heli. they do not
                    //need to look at heli to actually activate its control..
                }else if (Bridge.BButtonDown || Down1)
                {
                    switch (Current)
                    {
                        case InControlSwitch.MainPlayer:
                            ControlDummb();
                            Current = InControlSwitch.Dummb;
                            break;

                        case InControlSwitch.Dummb:
                            if (LastHeliControlled)
                            {
                                Show3DViewer = true;
                                ControlEnvironmentBaseObject(LastHeliControlled);
                                Current = InControlSwitch.Heli;
                                
                            }
                            else if (LastControlledAI)
                            {
                                Show3DViewer = true;
                                ControlEnvironmentBaseObject(LastControlledAI);
                                Current = InControlSwitch.ControlledAI;
                            }
                            else
                            {
                                MainPlayerInControl();
                                Current = InControlSwitch.MainPlayer;
                            }

                            break;

                        case InControlSwitch.Heli:
                        case InControlSwitch.ControlledAI:
                            MainPlayerInControl();
                            Current = InControlSwitch.MainPlayer;
                            break;
                    }
                }


            }
            
            //if the EnvironmentBase charcater is in control, then we control it from here..
            if (CurrentEnvironmentBaseControlling)
            {
                UpdateEnvironmentBaseObjectValues();
            }


        }




        public override void MainPlayerInControl()
        {
            if (PlayerInControl.Value) return;
            
            PlayerInControl.Value = true;

            if (!PlayerInVehicle)
            {
                SetPlayerDefaultValues();
                
            }


            
            
            Show3DViewer = false;
            Controller.GetEnvironmentEffects.MakeEnvironmentNormal();
            Controller.GetEnvironmentEffects.Hide3DViewer();
            DisableDummbMovement();
            DisableEnvironmentBaseObject();
            


            Debug.Log("Main player in control");

        }

        public override void ControlEnvironmentBaseObject(EnvironmentBase environmentObject)
        {
            CurrentControllable = environmentObject;
            ControlEnvironmentBaseObject();
        }


        protected override void OnFixedUpdate()
        {
            OutOfBoundsInquiry();
        }

        protected virtual void OutOfBoundsInquiry()
        {
            if (!Controller) return;

            if (!CharacterController) return;

            if(CharacterController.transform.position.y <= Controller.MinElevation || CharacterController.transform.position.y >= Controller.MaxElevation)
            {
                Debug.Log("Player Out of bounds");
                PlayerOutOfBounds = true;
                return;
            }

            PlayerOutOfBounds = false;

            

        }


        private void ControlEnvironmentBaseObject()
        {
            CurrentEnvironmentBaseControlling = CurrentControllable;

            bool HeliORAI = false;

            UserControlThirdPerson userControl = CurrentControllable.GetComponentInChildren<UserControlThirdPerson>();

           // ThirdPersonController thirdperson = CurrentControllable.GetComponentInChildren<ThirdPersonController>();

            Camera cam = CurrentControllable.GetComponentInChildren<Camera>();

            /*
            if (CurrentEnvironmentBaseControlling.GetComponent<MyHeliController>())
            {
                CurrentEnvironmentBaseControlling.EType = EnvironmentBase.EnvironBaseType.Heli;
                HeliORAI = true;
                LastHeliControlled = CurrentEnvironmentBaseControlling;
                Current = InControlSwitch.Heli;
            }

            */

            if (CurrentEnvironmentBaseControlling.GetComponent<ControlledAI>())
            {
                CurrentEnvironmentBaseControlling.EType = EnvironmentBase.EnvironBaseType.AI;
                HeliORAI = true;
                LastControlledAI = CurrentEnvironmentBaseControlling;
                Current = InControlSwitch.ControlledAI;
                Controller.CurrentControlledAI = (ControlledAI)CurrentEnvironmentBaseControlling;
            }

            if (!HeliORAI)
            {
                Debug.LogError("NOT one of the defined type. CHECKKKK");
            }

            if (Show3DViewer)
            {
                Controller.GetEnvironmentEffects.Show3DViewer();
                cam.enabled = true;

                if (!Controller.DebugMode)
                {
                    if (!cam.targetTexture) { cam.targetTexture = Controller.GetRenderTexture; }
                }
                
                if (userControl) { userControl.cam = cam.transform; }
               // if (thirdperson) { thirdperson.Camera = cam.transform; }

                

            }
            else
            {
                if (userControl) { userControl.cam = Controller.GetPlayer.GetHead; }
               // if (thirdperson) { thirdperson.Camera = Controller.GetPlayer.GetHead; }
                
            }

            Controller.GetEnvironmentEffects.Viewer3dEnvironmentChange(Show3DViewer);
            NotLooking();
            DisableDummbMovement();
            StartCoroutine(DisablePlayerMovement(!Show3DViewer));
        }

        private void UpdateEnvironmentBaseObjectValues()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            bool j = Input.GetButton("Jump");

            Vector2 vecL = Controller.DebugMode ? new Vector2(h,v): Bridge.LeftThumbstickAxis;
            bool jump = Controller.DebugMode ? j: Bridge.AButtonDown;
            bool XKey = Bridge.XButtonDown;
            Vector2 vecR = Bridge.RightThumbstickAxis;
            float triggerL = Bridge.LeftTrigger;
            float triggerR = Bridge.RightTrigger;
            bool buttonTriggerL = Bridge.LeftTriggerDown;
            bool buttonTriggerR = Bridge.RightTriggerDown;

            if (jump) { AFlip = !AFlip; Debug.Log("Aflipped"); }
            
            CurrentEnvironmentBaseControlling.InteractionStay(vecL.x, vecL.y, false);

            if (CurrentEnvironmentBaseControlling.EType == EnvironmentBase.EnvironBaseType.Heli)
            {
                CurrentEnvironmentBaseControlling.InstantInteraction(AFlip);
            }
            else { CurrentEnvironmentBaseControlling.InstantInteraction(jump); }
            
            
            CurrentEnvironmentBaseControlling.VectorInteraction(vecL, vecR);
            CurrentEnvironmentBaseControlling.TriggerInteraction(triggerL, triggerR);
            CurrentEnvironmentBaseControlling.TriggerButtonInteraction(buttonTriggerL, buttonTriggerR);
        }

        private void ControlDummb()
        {
            if (!CurrentCharacter)
            {
                CurrentCharacter = Instancer.GetPlayer3D;
            }

            if (CurrentCharacter)
            {
                if (Bridge.BButtonDown || Down1) { Show3DViewer = true; }
                if (Bridge.YButtonDown || Down2) { Show3DViewer = false; }

                Controller.GetEnvironmentEffects.Viewer3dEnvironmentChange(Show3DViewer);
                StartCoroutine(DisablePlayerMovement(!Show3DViewer));
                DisableEnvironmentBaseObject();

                CurrentCharacter.ToInControlState(Show3DViewer);

                

            }
        }

        

        public override void ControlDummb(bool show3dviewer)
        {
            Show3DViewer = show3dviewer;

            ControlDummb();
        }

        protected virtual bool RayCastHittingEnvironmentBase(Vector3 origin, Vector3 direction, float radius)
        {
            if (Physics.SphereCast(origin, radius, direction, out RaycastHit hit, LenghtOfRay, CharactersDetectHitLayer))
            {
                EnvironmentBase controllable = hit.transform.GetComponentInParent<EnvironmentBase>();

                if (controllable)
                {
                    LookingAtEnvironmentBase(controllable);
                    return true;
                }

            }

            return false;
        }

        protected override void LookingAtEnvironmentBase(EnvironmentBase controllable)
        {
            if (!controllable) return;

            if (controllable.AllowTobeControlledAI)
            {
                if (controllable != CurrentEnvironmentBaseControlling)
                {
                    if (OutlineOnCharacter != null)
                    {
                        OutlineOnCharacter.enabled = false;
                        OutlineOnCharacter = null;
                    }

                    CurrentControllable = controllable;

                    CurrentControllable.CurrentlyLooking = true;

                    Transform mesh = controllable.ObjectMesh;

                    OutlineOnCharacter = mesh.GetComponent<DarkDemon.Outline>();

                    if (OutlineOnCharacter) { OutlineOnCharacter.enabled = true; }

                }
            }
        }

        private void DisableEnvironmentBaseObject()
        {
            if (CurrentEnvironmentBaseControlling)
            {
                //CurrentEnvironmentBaseControlling.transform.GetComponentInChildren<Camera>().targetTexture = null;
                CurrentEnvironmentBaseControlling.transform.GetComponentInChildren<Camera>().enabled = false;
                CurrentEnvironmentBaseControlling.InteractionStay(0, 0, false);
                if (CurrentEnvironmentBaseControlling.EType == EnvironmentBase.EnvironBaseType.Heli)
                {
                    CurrentEnvironmentBaseControlling.InstantInteraction(AFlip);
                }
                else
                {
                    CurrentEnvironmentBaseControlling.InstantInteraction(false);
                }
                
            }
            Controller.CurrentControlledAI = null;
            CurrentEnvironmentBaseControlling = null;
        }

        private void DisableDummbMovement()
        {
            if (Instancer.CharacterInControl)
            {
                Instancer.CharacterInControl.ToIdleState();
                //Instancer.CharacterInControl.GetcharacterCamera.targetTexture = null;
                Instancer.CharacterInControl.GetcharacterCamera.enabled = false;
            }

            
            Instancer.CharacterInControl = null;
        }

        
       
        public void NotLooking()
        {
            if (OutlineOnCharacter)
            {
                OutlineOnCharacter.enabled = false;

                OutlineOnCharacter = null;
            }

            

            if (CurrentControllable)
            {
                CurrentControllable.CurrentlyLooking = false;
                CurrentControllable = null;
            }

        }

        public override bool SelectCatchMeTile()
        {
            if (Bridge.AButtonDown || Bridge.XButtonDown)
            {
                return true;
            }

            return false;
        }

        private void KeyboardInput()
        {
            if (!Controller.DebugMode) return;

            Down1 = Input.GetKeyDown(KeyCode.Alpha1);
            Down2 = Input.GetKeyDown(KeyCode.Alpha2);

        }

        private void ChangeControlOlder()
        {
            if (LookToChangePlayer.Value)
                        {
                            if (Physics.SphereCast(Head.position, RadiusOfRay, Head.forward, out RaycastHit hit, LenghtOfRay, CharactersDetectHitLayer))
                            {
                                if (Instancer.GetHumanFromCollider(hit.collider))
                                {
                                    CurrentCharacter = Instancer.GetHumanFromCollider(hit.collider);

                                    if (CurrentCharacter == Instancer.CharacterInControl) return;


                                    Transform meshcharacter = CurrentCharacter.GetMeshRendererCharacter;

                                    OutlineOnCharacter = meshcharacter.GetComponent<DarkDemon.Outline>();

                                    OutlineOnCharacter.enabled = true;

                                    CurrentCharacter.CurrentlyLooking = true;



                                }
                                else
                                {
                                    NotLooking();
                                }
                            }
                            else
                            {
                                NotLooking();
                            }

                            //If not looking directly to the character, then....
                            if (!CurrentCharacter)
                            {
                                bool val = InputBridge.Instance.BButton;

                                if (val)
                                {
                                    MainPlayerInControl();
                                }
                            }
                        }
                        else
                        {
                            if (!PlayerInControl.Value)
                            {
                                if (InputBridge.Instance.BButtonDown || InputBridge.Instance.YButtonDown)
                                {
                                    MainPlayerInControl();
                                }

                            }
                            else
                            {
                                if (!CurrentCharacter)
                                {
                                    CurrentCharacter = Instancer.GetPlayer3D;
                                }

                                if (CurrentCharacter)
                                {
                                    if (InputBridge.Instance.BButtonDown)
                                    {
                                        CurrentCharacter.ToInControlState(true);
                                    }

                                    if (InputBridge.Instance.YButtonDown)
                                    {
                                        CurrentCharacter.ToInControlState(false);
                                    }


                                }

                            }
                        }

            /*
             else if (CurrentCharacter)
            {
                if(CurrentCharacter != Instancer.CharacterInControl)
                {
                    if (Bridge.BButtonDown) { Show3DViewer = true; }
                    if (Bridge.YButtonDown) { Show3DViewer = false; }


                    if (Bridge.YButtonDown || Bridge.BButtonDown)
                    {

                        StartCoroutine(DisablePlayerMovement(!Show3DViewer));
                        DisableEnvironmentBaseObject();
                        CurrentCharacter.ToInControlState(Show3DViewer);
                    }


                }

            }
             
             */

        }

        public override IEnumerator DisablePlayerMovement(bool rotation)
        {
            yield return new WaitForEndOfFrame();

            //Disabling the main player movement. 
            PlayerInControl.Value = false;

            StopPlayerMovement(rotation);
        }
    }
}

