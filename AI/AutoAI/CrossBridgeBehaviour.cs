using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DarkDemon
{
    public class CrossBridgeBehaviour : Behaviours
    {
        public enum CharacterType { None, Char1,Char2,Char5,Char10}
        public enum LocationState { OnRock1, OnVehicle, OnRock2 }

        public CharacterType ThisType = CharacterType.None;

        public VehiclePoint CurrentVehiclePoint { get; set; } = null;

        public LocationState CurrentLocationState { get; private set; } = LocationState.OnRock1;

        CrossTheBridge CurrentLevel;
        GameLevelController LevelController;
        const int RunSpeed = 2;
        public override PeopleBehavioursEnum GetStateType()
        {
            return PeopleBehavioursEnum.CrossBridge;
        }

        public override void OnStateEnter()
        {
            LevelController = Instancer.GameControllerInstance.GetLevelController;

            //Change to idle when not in the correct level.
            if(LevelController.GetCurrentLevel != LevelsEnum.CrossTheBridge)
            {
                People.SetCurrentBehaviour = PeopleBehavioursEnum.Idle;
                return;
            }

            CurrentLevel = (CrossTheBridge)LevelController.GetCurrentMainLevel;

            
        }


        public void MoveToPoint()
        {
            switch (CurrentLocationState)
            {
                case LocationState.OnRock1:

                    if(CurrentLevel.Vehicle.LocationState == VehicleTransport.VehicleLocationState.BesideRock1)
                    {
                        GoToVehiclePoint();
                    }

                    break;


                case LocationState.OnRock2:

                    if (CurrentLevel.Vehicle.LocationState == VehicleTransport.VehicleLocationState.BesideRock2)
                    {
                        GoToVehiclePoint();
                    }

                    break;

                case LocationState.OnVehicle:

                    Transform pointToGo;

                    switch(CurrentLevel.Vehicle.LocationState)
                    {
                        case VehicleTransport.VehicleLocationState.BesideRock1:

                            pointToGo = GetToRockPoint(VehicleTransport.VehicleLocationState.BesideRock1);
                            CurrentLevel.Vehicle.NumOfCharOnVeh -= 1;
                            CurrentVehiclePoint.Occupied = false;
                            MoveToTarget(pointToGo);
                            CurrentLocationState = LocationState.OnRock1;
                            break;

                        case VehicleTransport.VehicleLocationState.BesideRock2:
                            
                            pointToGo = GetToRockPoint(VehicleTransport.VehicleLocationState.BesideRock2);
                            CurrentLevel.Vehicle.NumOfCharOnVeh -= 1;
                            CurrentVehiclePoint.Occupied = false;
                            MoveToTarget(pointToGo);
                            CurrentLocationState = LocationState.OnRock2;
                            break;


                    }

                    break;

            }
        }

        public override void BehaviourUpdate()
        {
            if (!People) return;
            if (!People.Agent) return;
            if(!People.Agent.enabled) return;   

            if ((!People.Agent.hasPath && !People.Agent.pathPending) ||
                People.Agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                People.Animator.SetInteger(SpeedHash, 0);
                People.Agent.enabled = false;
            }
                
        }


        private void MoveToTarget(Transform point)
        {
            People.Agent.enabled = true;
            People.Agent.destination = point.position;
            People.Animator.SetInteger(SpeedHash, RunSpeed);

        }

        private void GoToVehiclePoint()
        {
            CurrentLevel.Vehicle.NumOfCharOnVeh += 1;
            CurrentVehiclePoint = CurrentLevel.Vehicle.GetAvailablePoint();
            CurrentVehiclePoint.Occupied = true;
            Transform point = CurrentVehiclePoint.transform;
            MoveToTarget(point);
            CurrentLocationState = LocationState.OnVehicle;
        }

        private Transform GetToRockPoint(VehicleTransport.VehicleLocationState vehiclestate)
        {
            Transform pointToGo = null;

            switch (vehiclestate)
            {
                case VehicleTransport.VehicleLocationState.BesideRock1:
                    switch (ThisType)
                    {
                        case CharacterType.Char1:
                            pointToGo = CurrentLevel.Speed1PointR1; break;

                        case CharacterType.Char2:
                            pointToGo = CurrentLevel.Speed2PointR1; break;

                        case CharacterType.Char5:
                            pointToGo = CurrentLevel.Speed5PointR1; break;

                        case CharacterType.Char10:
                            pointToGo = CurrentLevel.Speed10PointR1; break;
                    }

                    break;

                case VehicleTransport.VehicleLocationState.BesideRock2:

                    switch (ThisType)
                    {
                        case CharacterType.Char1:
                            pointToGo = CurrentLevel.Speed1PointR2; break;

                        case CharacterType.Char2:
                            pointToGo = CurrentLevel.Speed2PointR2; break;

                        case CharacterType.Char5:
                            pointToGo = CurrentLevel.Speed5PointR2; break;

                        case CharacterType.Char10:
                            pointToGo = CurrentLevel.Speed10PointR2; break;
                    }

                    break;
            }

            

            return pointToGo;
        }

        
        public override void OnStateExit()
        {
            
        }

    }
}

