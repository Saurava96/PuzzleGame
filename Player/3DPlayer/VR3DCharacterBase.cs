using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using RootMotion.Demos;

namespace LazyPanda
{
    public abstract class VR3DCharacterBase : MonoBehaviour
    {
        public enum CharacterState { None, Idle, InControl }

        private bool Hanging = false;

        public CharacterState CurrentState { get; set; } = CharacterState.None;

        protected PuppetMaster Puppet;

       // protected PickUp CharacterPickDualHand;
       // protected MainMeshTrigger CharacterTriggers;
        protected Animator AnimatorController;
        protected CharacterAnimationThirdPerson AnimationThirdPerson;
        


        protected BehaviourBase[] Behaviours;

        private const float  DeadMuscleDamper = 2;
        private readonly bool InternalCollision = true;
        protected  float DefaultPin = 1;
        protected  float DefaultMuscleWeight = 1;
        protected  float DefaultMuscleDamper = 0;

        public bool CurrentlyLooking { get; set; } = false;
        public bool IsSuctioned { get; set; } = false;



        protected Transform Head;
        protected Transform LeftHand;
        protected Transform RightHand;
        protected Transform HipBone;
        protected Transform MeshMain;
        protected Transform MeshRendererCharacter;
        protected Transform CharacterController;
        protected CharacterPuppet CharacterPuppet;
        protected BehaviourPuppet PuppetCollisionFall;
        protected BehaviourFall PuppetFall;
        protected VRController VRController;
        protected Camera CharacterCamera;
        
       // public InteractionBase ObjectInHand { get; set; } = null;

        public Transform GetCharacterHead { get { return Head; } }
        public Transform GetCharacterLeftHand { get { return LeftHand; } }
        public Transform GetCharacterRightHand { get { return RightHand; } }

        public Transform GetHipBone { get { return HipBone; } }

        public Transform GetMeshRendererCharacter { get { return MeshRendererCharacter; } }

        public Transform GetMeshMain { get { return MeshMain; } }

        public Transform GetCharacterController { get { return CharacterController; } }

        public CharacterPuppet GetCharacterPuppet { get { return CharacterPuppet; } }

        public VRController GetVRController { get { return VRController; } }

        public Camera GetcharacterCamera { get { return CharacterCamera; } }

        public PuppetMaster GetPuppet { get { if (!Puppet) { Puppet = GetComponentInChildren<PuppetMaster>();return Puppet; } else { return Puppet; } } }

        public bool IsHanging { get { return Hanging; } set { Hanging = value; } }

        public bool IsAlive { get; protected set; }

        

        public bool AllowToBeControlled { get; set; } = true;

        public bool DummbOutOfBounds { get; set; } = false;

        public Joint ConnectedJoint { get; set; }

        protected VRMainPlayer Player;

        protected int PickUpHash;
        
        protected int DropHash;

        

        protected GameController Controller;
        protected virtual void Awake()
        {
            Puppet = GetComponentInChildren<PuppetMaster>();

            Behaviours = GetComponentsInChildren<BehaviourBase>();

            if (!Puppet) { Debug.LogError("No pupper");return; }

            Collider[] colliders = Puppet.GetComponentsInChildren<Collider>();

            for(int i = 0; i < colliders.Length; i++)
            {
                Instancer.AddToHumansDic(colliders[i], this);
            }

            DefaultPin = Puppet.pinWeight;
            DefaultMuscleWeight = Puppet.muscleWeight;
            DefaultMuscleDamper = Puppet.muscleDamper;

            BodyPartsSetup();
            
            

        }

        protected virtual void Start()
        {
            MainPlayerBase player = Instancer.GameControllerInstance.GetPlayer;

            Player = player.GetComponent<VRMainPlayer>();

            Controller = Instancer.GameControllerInstance;
        }

        protected void DefaultPuppetStateSettingsValues()
        {
            if (!Puppet) return;

            Puppet.stateSettings.deadMuscleDamper = DeadMuscleDamper;
            Puppet.stateSettings.enableInternalCollisionsOnKill = InternalCollision;
            
        }

        protected void DefaultAlivePinValues()
        {
            if (!Puppet) return;

            Puppet.pinWeight = DefaultPin;
            Puppet.muscleWeight = DefaultMuscleWeight;
            Puppet.muscleDamper = DefaultMuscleDamper;
        }

        

        public virtual IEnumerator MoveDummb(Transform pos)
        {
            Puppet.mode = PuppetMaster.Mode.Disabled;
            CharacterController.position = pos.position;
            yield return new WaitForEndOfFrame();
            Puppet.mode = PuppetMaster.Mode.Active;
            
        }

        /// <summary>
        /// sets up all the body parts for this character..
        /// </summary>
        public void BodyPartsSetup()
        {

            VR3DBodyParts[] parts = GetComponentsInChildren<VR3DBodyParts>();

            for (int i = 0; i < parts.Length; i++)
            {
                switch (parts[i].ThisPart)
                {
                    case VR3DBodyParts.RigidBodyParts.Head:

                        Head = parts[i].transform;
                        Collider headC = Head.GetComponent<Collider>();
                        Instancer.AddToHeadHumanDic(headC, this);
                        
                        break;

                    case VR3DBodyParts.RigidBodyParts.LeftHand:

                        LeftHand = parts[i].transform; break;

                    case VR3DBodyParts.RigidBodyParts.RightHand:

                        RightHand = parts[i].transform; break;

                    case VR3DBodyParts.RigidBodyParts.Hip:

                        HipBone = parts[i].transform;
                        


                        break;

                    case VR3DBodyParts.RigidBodyParts.CharacterMeshMain:

                        MeshMain = parts[i].transform;

                       // CharacterPickDualHand = MeshMain.GetComponent<PickUp>();

                      //  CharacterTriggers = MeshMain.GetComponent<MainMeshTrigger>();

                        break;

                    case VR3DBodyParts.RigidBodyParts.CharacterMeshRenderer:

                        MeshRendererCharacter = parts[i].transform; break;

                    case VR3DBodyParts.RigidBodyParts.CharacterController:

                        CharacterController = parts[i].transform;
                        CharacterPuppet = parts[i].GetComponent<CharacterPuppet>();
                        VRController = parts[i].GetComponent<VRController>();

                        break;

                    case VR3DBodyParts.RigidBodyParts.AnimatorController:

                        AnimatorController = parts[i].GetComponent<Animator>();
                        AnimationThirdPerson = parts[i].GetComponent<CharacterAnimationThirdPerson>();
                        PickUpHash = Animator.StringToHash("PickUpTrigger");
                        DropHash = Animator.StringToHash("DropTrigger");

                        break;

                    case VR3DBodyParts.RigidBodyParts.PuppetCollisionFall:

                        PuppetCollisionFall = parts[i].GetComponent<BehaviourPuppet>(); break;

                    case VR3DBodyParts.RigidBodyParts.PuppetFall:

                        PuppetFall = parts[i].GetComponent<BehaviourFall>(); break;

                    case VR3DBodyParts.RigidBodyParts.CharacterCamera:

                        CharacterCamera = parts[i].GetComponent<Camera>(); break;

                    


                }
            }

        }


        /// <summary>
        /// Updates the pin weight of this character..
        /// </summary>
        /// <param name="val"></param>
        public virtual void UpdatePinWeight(float val)
        {
            if (!Puppet) return;

            Puppet.pinWeight = Mathf.Clamp01(val);
        }


        /// <summary>
        /// Updates the muscle weight of this target..
        /// </summary>
        /// <param name="val"></param>
        public virtual void UpdateMuscleWeight(float val)
        {
            if (!Puppet) return;

            Puppet.muscleWeight = Mathf.Clamp01(val);
        }

        
        public virtual void EnableBehaviors(bool value) { }

        public virtual void Dead()
        {
            Puppet.state = PuppetMaster.State.Dead;
            IsAlive = false;
        }

        public virtual void Alive()
        {
            Puppet.state = PuppetMaster.State.Alive;
            IsAlive = true;
        }

        /// <summary>
        /// Makes the character hang to the point. called by points when they want to hang the player
        /// to a point...
        /// </summary>
        public virtual void HangingDeadSimulation()
        {
            LowPinDeadSimulation();

            IsHanging = true;
            Debug.Log("Hanging dead simulation");
        }

        /// <summary>
        /// Character dead simulation by decreasing pin weight..
        /// </summary>
        public virtual void LowPinDeadSimulation()
        {
            Puppet.pinWeight = 0;
            Puppet.muscleWeight = 0;
            Puppet.muscleDamper = 40;
        }

        public virtual void AliveFromLowPin()
        {
            DefaultAlivePinValues();
        }

        /// <summary>
        /// Disconnect the hanging character..
        /// </summary>
        public virtual void AliveFromHanging()
        {
            if (IsHanging) return;

            DefaultAlivePinValues();
            ConnectedJoint.connectedBody = null;

            IsHanging = false;
        }

        public virtual void JumpWhileHanging() { }

        public virtual void ToIdleState() { }

        public virtual void ToInControlState (bool show3dviewer) { }

        public virtual void ToInControlState (float xVal,float zVal) { }

        //protected virtual void ObjectPickUp (InteractionBase interaction) { }

        protected virtual void DropObject() { }

        public virtual void InteractionDecider() { }

        protected virtual void Onupdate() { }

        protected virtual void Update() { Onupdate(); }

    }

}

