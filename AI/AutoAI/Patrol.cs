using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DarkDemon
{
    public enum Looker { None, Looking, StopLook }
    public class Patrol : Behaviours
    {

        [SerializeField] AIWaypointNetwork CurrentNetwork;
        [SerializeField] int SpeedValue = 2;

        [Header("Particles")]
        [SerializeField] GameObject FootStepParticle;
        
        public int SetSpeed { set { SpeedValue = value; } }

        public AIWaypointNetwork WaypointNetwork { set { CurrentNetwork = value; } get { return CurrentNetwork; } }

        

        public bool HasPath = false;
        public bool PathPending = false;
        public bool PathStale = false;
        public NavMeshPathStatus PathStatus = NavMeshPathStatus.PathInvalid;

        //delete
        public bool DisableAgent { get; set; } = false;
        

        public int CurrentIndex { get; private set; } = 4; //7 for sequence2, 9 for sequence3


        int RunCurveHash;
        
        private bool LeftSoundPlay = true;
        protected bool LeftFootOnGround = true;

        public override PeopleBehavioursEnum GetStateType()
        {
            return PeopleBehavioursEnum.Patrol;
        }

        public override void OnStateEnter()
        {
            if (CurrentNetwork == null) return;

            SpeedHash = Animator.StringToHash("Speed");

            RunCurveHash = Animator.StringToHash("RunCurve");

            if (!DisableAgent)
            {
                People.Agent.enabled = true;
            }
            
            
            People.Animator.SetInteger(SpeedHash, SpeedValue);

            SetNextDestination(true);
            
            


        }

        public override void BehaviourUpdate()
        {
           
            
            // Copy NavMeshAgents state into inspector visible variables
            HasPath = People.Agent.hasPath;
            PathPending = People.Agent.pathPending;
            PathStale = People.Agent.isPathStale;
            PathStatus = People.Agent.pathStatus;

            // If we don't have a path and one isn't pending then set the next
            // waypoint as the target, otherwise if path is stale regenerate path
            if ((!HasPath && !PathPending) || PathStatus == NavMeshPathStatus.PathInvalid /*|| PathStatus==NavMeshPathStatus.PathPartial*/)
                SetNextDestination(true);
            else
            if (People.Agent.isPathStale)
                SetNextDestination(false);

            LookAtTarget();
            FootStepParticles();
            //SoundManagement();
        }

        public virtual void SetNextDestination(bool increment)
        {
            if (!People.Agent.enabled) return;

            // If no network return
            if (!CurrentNetwork) return;

            // Calculatehow much the current waypoint index needs to be incremented
            int incStep = increment ? 1 : 0;

            // Calculate index of next waypoint factoring in the increment with wrap-around and fetch waypoint 
            int nextWaypoint = (CurrentIndex + incStep >= CurrentNetwork.Waypoints.Count) ? 0 : CurrentIndex + incStep;
            Transform nextWaypointTransform = CurrentNetwork.Waypoints[nextWaypoint];

            // Assuming we have a valid waypoint transform
            if (nextWaypointTransform != null)
            {
                // Update the current waypoint index, assign its position as the NavMeshAgents
                // Destination and then return
                CurrentIndex = nextWaypoint;
                People.Agent.destination = nextWaypointTransform.position;

                return;
            }

            // We did not find a valid waypoint in the list for this iteration
            CurrentIndex++;


        }

        public override void OnStateExit()
        {
            People.Agent.enabled = false;

            People.Animator.SetInteger(SpeedHash, 0);

            StopLooking();

            LeftSoundPlay = true;

            DisableAgent = false;

        }

        protected virtual void FootStepParticles()
        {
            if (!People) return;

            if (People.Animator.GetFloat(RunCurveHash) <= 0f) return;


            if (LeftFootOnGround)
            {
               // SimplePool.Spawn(FootStepParticle, People.LeftFoot.position, People.LeftFoot.rotation);

               // LeftFootOnGround = !LeftFootOnGround;
            }
            else
            {
              //  SimplePool.Spawn(FootStepParticle, People.RightFoot.position, People.RightFoot.rotation);

              //  LeftFootOnGround = !LeftFootOnGround;
            }

        }

        /*

        protected virtual void SoundManagement()
        {
            if (!People) return;

            if (!People.LeftFootSource || !People.RightFootSource) return;

            if (People.Animator.GetFloat(RunCurveHash) <= 0f) return;

            if (LeftSoundPlay)
            {
                if (People.LeftFootSource.isPlaying) { People.LeftFootSource.Stop(); }

                People.LeftFootSource.clip = GetRandomClip();

                People.LeftFootSource.Play();

                LeftSoundPlay = !LeftSoundPlay;

                
            }
            else
            {
                if (People.RightFootSource.isPlaying) { People.RightFootSource.Stop(); }

                People.RightFootSource.clip = GetRandomClip();

                People.RightFootSource.Play();

                LeftSoundPlay = !LeftSoundPlay;
            }

        }

        */

        

    }

}

