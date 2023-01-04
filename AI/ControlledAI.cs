using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Demos;
using LazyPanda;
using RootMotion.Dynamics;
//using StarterAssets;
//using UnityEngine.InputSystem.XR;

namespace DarkDemon
{
    public class ControlledAI : EnvironmentBase
    {
        public enum ControlledType { None, KillableByDummb, Golem}

        public ControlledType ThisType = ControlledType.None;

        UserControlThirdPerson ThirdPerson;
        GameController Controller;

        
        

        private const int ThresholdVal = 2;
        public Transform Head { get; private set; }
        public PuppetMaster Puppet { get; private set; }

        public Transform HipBone { get; private set; }

        public Transform CharacterController { get; private set; }

        float RadiusOfRay = 3f;
        LayerMask CharactersDetectHitLayer;
        const int ActualLength = 3;
        int LenghtOfRay = ActualLength;
        Transform SphereCastPos;

        float XValueWhenClose = 0;
        float ZValueWhenClose = 0;
        bool updateXandZ = true;

        protected float DefaultPin = 1;
        protected float DefaultMuscleWeight = 1;
        protected float DefaultMuscleDamper = 0;

        public bool IsSuctioned { get; set; } = false;

        public bool AIOutOfBounds { get; set; } = false;

        public Transform CharacterCamera { get; private set; }

        

        public override void InteractionStart()
        {
            Controller = Instancer.GameControllerInstance;
            ThirdPerson = GetComponentInChildren<UserControlThirdPerson>();


            CharactersDetectHitLayer = 1 << LayerMask.NameToLayer("Default");

            PartsInitializer();

        }


        protected override void Update()
        {
            if (!Puppet) return;


        }


        public override void InteractionStay(float xVal = 50f, float zVal = 50f, bool ingameJoystick = false)
        {
            if (ThirdPerson) 
            {
                if (ingameJoystick)
                {
                    float finalX = xVal - 50f;
                    float finalZ = zVal - 50f;

                    if (CloseToZero(finalX)) { finalX = 0; }
                    if (CloseToZero(finalZ)) { finalZ = 0; }

                    Vector2 v = new Vector2(finalX, finalZ);

                    ThirdPerson.VectorMove = v;
                }
                else
                {
                    if (ThisType == ControlledType.Golem)
                    {

                        Debug.DrawRay(SphereCastPos.position, SphereCastPos.up * LenghtOfRay, Color.white);
                        if (Physics.SphereCast(SphereCastPos.position, RadiusOfRay, SphereCastPos.up, out RaycastHit hit, LenghtOfRay, CharactersDetectHitLayer))
                        {
                            Debug.DrawRay(SphereCastPos.position, SphereCastPos.up * LenghtOfRay, Color.green);

                            if (updateXandZ)
                            {
                                XValueWhenClose = xVal;
                                ZValueWhenClose = zVal;
                            }

                            updateXandZ = false;

                        }
                        else
                        {
                            updateXandZ = true;
                            XValueWhenClose = 0;
                            ZValueWhenClose = 0;
                        }

                        if (!updateXandZ)
                        {
                            if (ZValueWhenClose > 0)
                            {
                                if (zVal >= 0)
                                {
                                    zVal = 0;
                                }
                            }

                            if (ZValueWhenClose < 0)
                            {
                                if (zVal <= 0)
                                {
                                    zVal = 0;
                                }
                            }

                            if (XValueWhenClose > 0)
                            {
                                if (xVal >= 0)
                                {
                                    xVal = 0;
                                }
                            }

                            if (XValueWhenClose < 0)
                            {
                                if (xVal <= 0)
                                {
                                    xVal = 0;
                                }
                            }
                        }


                    }

                    Vector2 v = new Vector2(xVal, zVal);

                    ThirdPerson.VectorMove = v;
                }
            }

        }

        

        public override void InstantInteraction(bool value)
        {
            if (ThirdPerson)
            {
                ThirdPerson.Jumping = value;
            }
            
        }


        private bool CloseToZero(float val)
        {
            if (val >= -ThresholdVal && val <= ThresholdVal)
            {
                return true;
            }

            return false;

        }


        public void ImplementDeath()
        {
            if (!Puppet) { Debug.LogError("NO puppet found"); return; }

            Puppet.state = PuppetMaster.State.Dead;
            


        }

        public void Dead()
        {
            Puppet.state = PuppetMaster.State.Dead;
        }

        public void Alive()
        {
            Puppet.state = PuppetMaster.State.Alive;
        }

        public void LowPinDeadSimulation()
        {
            Puppet.pinWeight = 0;
            Puppet.muscleWeight = 0;
            Puppet.muscleDamper = 40;
        }

        public virtual void AliveFromLowPin()
        {
            DefaultPinValues();
        }

        protected virtual void DefaultPinValues()
        {
            if (!Puppet) return;

            Puppet.pinWeight = DefaultPin;
            Puppet.muscleWeight = DefaultMuscleWeight;
            Puppet.muscleDamper = DefaultMuscleDamper;
        }

        private void OutOfBoundsInquiry()
        {
            if (!CharacterController) return;

            if (CharacterController.transform.position.y <= Instancer.GameControllerInstance.MinElevation || 
                CharacterController.transform.position.y >= Instancer.GameControllerInstance.MaxElevation)
            {
                
                AIOutOfBounds = true;
                
                gameObject.SetActive(false);
                Debug.Log("Out Of Bounds AI Bot");
                return;
            }

            AIOutOfBounds = false;


        }

        protected virtual void FixedUpdate()
        {
            OutOfBoundsInquiry();
        }

        

        protected void PartsInitializer()
        {
            if (transform.parent) { transform.parent = null; }

            Puppet = GetComponentInChildren<PuppetMaster>();

            if (Puppet)
            {
                DefaultPin = Puppet.pinWeight;
                DefaultMuscleWeight = Puppet.muscleWeight;
                DefaultMuscleDamper = Puppet.muscleDamper;
            }
            
            ControlledAIBodyPart[] parts = GetComponentsInChildren<ControlledAIBodyPart>();

            for (int i = 0; i < parts.Length; i++)
            {

                switch (parts[i].ThisPart)
                {
                    case ControlledAIBodyPart.BodyPart.Head:

                        Head = parts[i].transform;
                        Instancer.AddToHeadControlledAIDic(Head.GetComponent<Collider>(), this);

                        break;


                    case ControlledAIBodyPart.BodyPart.CharacterCamera:

                        CharacterCamera = parts[i].transform;
                        break;

                    case ControlledAIBodyPart.BodyPart.HipBone:

                        HipBone = parts[i].transform; break;

                    case ControlledAIBodyPart.BodyPart.SphereCastPos:

                        SphereCastPos = parts[i].transform; break;

                    case ControlledAIBodyPart.BodyPart.CharacterController:

                        CharacterController = parts[i].transform; break;
                }

            }
        }
    }
}

