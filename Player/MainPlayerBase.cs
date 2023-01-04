using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;



namespace LazyPanda
{
    public abstract class MainPlayerBase : MonoBehaviour
    {
        protected Transform Head;
        protected Transform PlayerController;
        

        protected PlayerTeleport Teleport;
        protected SmoothLocomotion Locomotion;
        protected PlayerRotation Rotation;
        protected BNGPlayerController ControllerPlayer;
        protected CharacterController CharacterController;
        protected Collider CharacterControllerCollider;
       // protected MobilePostProcessing MobilePost;

        private float Stepoffset = 0.3f;
        private Vector3 Center = new Vector3(0, -0.25f, 0);
        private float Radius = 0.1f;
        private float Height = 1.5f;

        public bool DummbInRange { get; set; } = false;
       
        
        public GameObject Fader { get; private set; }
        public Camera PlayerCamera { get; private set; }
        public Transform GetHead { get { return Head; } }

        public Transform GetPlayerController { get { return PlayerController; } }
        public SmoothLocomotion GetLocomotion { get { return Locomotion; } }
        public PlayerTeleport GetTeleport { get { return Teleport; } }

        public PlayerRotation GetRotation { get { return Rotation; } }

        public BNGPlayerController GetControllerPlayer { get { return ControllerPlayer; } }

        public CharacterController GetCharacter { get { return CharacterController; } }

       // public MobilePostProcessing GetMobilePostProcessing { get { return MobilePost; } }

        public Collider GetCharacterControllerCollider { get { return CharacterControllerCollider; } }

        
        public bool PlayerInVehicle { get; set; } = false;

        public bool AllowControlShift { get; set; } = false;

        public bool PlayerOutOfBounds { get; set; } = false;

        public UIPointer Pointer { get; private set; } = null;

        protected virtual void Awake()
        {

            BodyPartsSetup();

        }

        protected virtual void Update() { }

        protected virtual void FixedUpdate() { OnFixedUpdate(); }


        protected virtual void OnFixedUpdate() { }

        private void BodyPartsSetup()
        {
            MainPlayerBodyParts[] parts = GetComponentsInChildren<MainPlayerBodyParts>();

            for (int i = 0; i < parts.Length; i++)
            {
                switch (parts[i].ThisPart)
                {
                    case MainPlayerBodyParts.BodyPart.Head:

                        Head = parts[i].transform;
                        PlayerCamera = Head.GetComponent<Camera>();
                       // MobilePost = GetComponent<MobilePostProcessing>();
                        break;

                    case MainPlayerBodyParts.BodyPart.PlayerController:

                        PlayerController = parts[i].transform;

                        if (!PlayerController) { Debug.LogError("No player controller");return; }

                        Teleport = PlayerController.GetComponent<PlayerTeleport>();
                        Locomotion = PlayerController.GetComponent<SmoothLocomotion>();
                        Rotation = PlayerController.GetComponent<PlayerRotation>();
                        ControllerPlayer = PlayerController.GetComponent<BNGPlayerController>();
                        CharacterController = PlayerController.GetComponent<CharacterController>();
                        CharacterControllerCollider = PlayerController.GetComponent<Collider>();

                        Stepoffset = CharacterController.stepOffset;
                        Center = CharacterController.center;
                        Radius = CharacterController.radius;
                        Height = CharacterController.height;//height can be tricky.. check with VR.


                        break;

                    case MainPlayerBodyParts.BodyPart.Fader:

                        Fader = parts[i].gameObject; break;

                    case MainPlayerBodyParts.BodyPart.HandPointer:
                        Pointer = parts[i].GetComponent<UIPointer>(); break; 




                }
            }


        }

        public void SetPlayerDefaultValues()
        {

            if (Instancer.GameControllerInstance.DebugMode)
            {
               // FirstPersonMovement firstperson = GetComponent<FirstPersonMovement>();
               // firstperson.enabled = true;
            }

            ControllerPlayer.enabled = true;
           
            Teleport.enabled = false;

            Locomotion.enabled = true;
            //Locomotion.MovementSpeed = 3;
            //Locomotion.JumpForce = 3;
            //Locomotion.StrafeSpeed = 1;
            //Locomotion.StrafeSprintSpeed = 1.25f;
            //Locomotion.SprintSpeed = 1.5f;
            //Locomotion.AirControl = true;

            Rotation.enabled = true;
            
            ControllerPlayer.MoveCharacterWithCamera = true;
            
            ControllerPlayer.RotateCharacterWithCamera = true;

            PlayerCamera.enabled = true;

            // CharacterController.stepOffset = Stepoffset;
            /// CharacterController.center = Center;
            // CharacterController.radius =  Radius;
            /// CharacterController.height = Height; //height can be tricky.. take a lookkk
        }

        


        protected void StopPlayerMovement(bool rotationEnabled)
        {
            if (Instancer.GameControllerInstance.DebugMode)
            {
                //FirstPersonMovement firstperson = GetComponent<FirstPersonMovement>();
                //firstperson.enabled = false;
            }

            
            ControllerPlayer.enabled = false;
            
            Teleport.enabled = false;
            Locomotion.enabled = false;
            
            //Locomotion.MovementSpeed = 0;
            //Locomotion.JumpForce = 0;
            //Locomotion.StrafeSpeed = 0;
            //Locomotion.StrafeSprintSpeed = 0;
            //Locomotion.SprintSpeed = 0;
            //Locomotion.AirControl = false;

            
            ControllerPlayer.MoveCharacterWithCamera = false;
            ControllerPlayer.RotateCharacterWithCamera = false;

            if (rotationEnabled)
            {
                Rotation.enabled = true;
               
            }
            else
            {
                Rotation.enabled = false;

            }
            //if debug mode, we are disabling the player camera because it is not needed
            //as we are using 3rd person and can use the dummb camera to control dummb.
            //There is no need of vr camera.
            if (Instancer.GameControllerInstance.DebugMode)
            {
                PlayerCamera.enabled = false;
            }


            Debug.Log("Stopped");

        }

        public IEnumerator MovePlayer(Transform Destination)
        {
            yield return StartCoroutine(DisablePlayerMovement(false));

            Debug.Log("about to change position");

            transform.position = Destination.position;
            transform.rotation = Destination.rotation;
            GetPlayerController.position = transform.position;

            yield return new WaitForEndOfFrame();

            MainPlayerInControl();

            


        }

        public void AddPlayerWeapon() { }

        public abstract IEnumerator DisablePlayerMovement(bool rotation);

        public virtual void MainPlayerInControl() { }
        public virtual void ControlEnvironmentBaseObject(EnvironmentBase environmentObject) { }

        protected virtual void LookingAtEnvironmentBase(EnvironmentBase controllable) { }

        public virtual void ControlDummb(bool show3dviewer) { }

        public virtual bool SelectCatchMeTile() { return false; }



    }
}

