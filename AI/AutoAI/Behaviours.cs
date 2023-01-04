using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;

namespace DarkDemon
{
    public abstract class Behaviours : MonoBehaviour
    {

        protected HumansAI People;
        protected GameController Controller;

       


        public virtual HumansAI setPeople { set { People = value; } }
        public abstract PeopleBehavioursEnum GetStateType();

        public bool LookAt { get; set; } = false;
        
        
        public Transform LookTarget { get; set; }

        public virtual void OnStateEnter() { }

        public virtual void OnStateExit() { }

        public virtual void OnTriggerEntered(Collider other)
        {
            //talk Behaviour

            if (!People.AllowTalkBehaviour) return;

            if (People.AllowPlayerTrigger)
            {
                if (Instancer.GameControllerInstance.IsPlayer(other))
                {
                    People.SetCurrentBehaviour = PeopleBehavioursEnum.Talk;
                }
            }

            if (People.AllowControlledAITrigger)
            {
                if (Instancer.GameControllerInstance.IsControlledAI(other))
                {
                    People.SetCurrentBehaviour = PeopleBehavioursEnum.Talk;
                }
            }
        }


        public virtual void OnTriggerExited(Collider other) { }
       
        protected virtual void AITriggerExited(Collider other)
        {
            if (People.AllowPlayerTrigger)
            {
                if (Instancer.GameControllerInstance.IsPlayer(other))
                {
                    People.SetCurrentBehaviour = PeopleBehavioursEnum.Idle;
                }
            }

            if (People.AllowControlledAITrigger)
            {
                if (Instancer.GameControllerInstance.IsControlledAI(other))
                {
                    People.SetCurrentBehaviour = PeopleBehavioursEnum.Idle;
                }
            }
        }



        public virtual void OnTriggerStayed() { }

        

        public virtual void BehaviourUpdate() { }

        public void StopLooking()
        {
            LookAt = false;

            People.LookAt.solver.SetIKPositionWeight(0);

            People.LookAt.solver.target = null;
        }

        protected virtual void LookAtTarget()
        {
            if (!LookAt) return;

            if (!LookTarget) return;

            People.LookAt.solver.target = LookTarget;

            StartCoroutine(LookInterpolate());


        }

        private IEnumerator LookInterpolate()
        {
            float lookVal = 0;

            while (lookVal <= 1)
            {
                lookVal += 0.01f;

                People.LookAt.solver.SetIKPositionWeight(lookVal);

                if (lookVal >= 1f)
                {
                    People.LookAt.solver.SetIKPositionWeight(1);
                }


                yield return new WaitForEndOfFrame();
            }

            LookAt = false;

            Debug.Log("loooo");

        }


        public virtual void UpdatingPositionAndAngle()
        {
            if (!People) return;

            if (!People.Animator) return;

            if (!People.Agent) return;

            if (!People.Agent.enabled) return;

            Vector3 LocalDesiredVelocity = transform.InverseTransformVector(People.Agent.desiredVelocity);

            float Angle = Mathf.Atan2(LocalDesiredVelocity.x, LocalDesiredVelocity.z) * Mathf.Rad2Deg; //finding the angle in which the agent wants to go



            SmoothAngle = Mathf.MoveTowardsAngle(SmoothAngle, Angle, 30.0f * Time.deltaTime);//smoothly moving in the desired angle. 

            // float Speed = LocalDesiredVelocity.z;

            People.Animator.SetFloat(AngleHash, SmoothAngle); //sending the final angle to the animator and then the animator turns the NPC accordingly. 
                                                              //Angle is one of the parameter of the animator controller.
            
           

            // AnimatorController.SetFloat(SpeedHash, Speed, 0.2f, Time.deltaTime);


            if (People.Agent.desiredVelocity.sqrMagnitude > Mathf.Epsilon)
            {
                Quaternion Look = Quaternion.LookRotation(People.Agent.desiredVelocity, Vector3.up);

                transform.rotation = Quaternion.Slerp(transform.rotation, Look, 5.0f * Time.deltaTime);
            }

            
        }

        protected virtual void Start()
        {
            Controller = Instancer.GameControllerInstance;

            SpeedHash = Animator.StringToHash("Speed");
            AngleHash = Animator.StringToHash("Angle");
           
        }

        public virtual void OnAnimatorUpdated()
        {
            if (!People) return;

            if (!People.Agent) return;

            if (Time.deltaTime != 0)
            {
                //setting the velocity of agent according to the animator
                People.Agent.velocity = People.Animator.deltaPosition / Time.deltaTime;
            }
        }




        //small humanoid
        private float SmoothAngle;
        protected int AngleHash;
        protected int SpeedHash;
        

    }
}

