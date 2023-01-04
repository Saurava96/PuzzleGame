using LazyPanda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class VehicleTransport : MonoBehaviour
    {
        public VehiclePoint[] StandingPoints;
       
        GameController GController;
        GameLevelController LevelController;

        public enum VehicleLocationState { BesideRock1,BesideRock2 };

        public VehicleLocationState LocationState { get; private set; } = VehicleLocationState.BesideRock1;

        public int NumOfCharOnVeh { get; set;} = 0;


        private void Start()
        {
            GController = Instancer.GameControllerInstance;
            LevelController = GController.GetLevelController;
        }

        

        private void VehicleToleranceCheck()
        {
            if (NumOfCharOnVeh < 3) return;

            VehicleDestroyed();

        }

        public void MoveVehicle()
        {

        }

        private void VehicleDestroyed()
        {

        }

        public VehiclePoint GetAvailablePoint()
        {
            for(int i = 0; i < StandingPoints.Length; i++)
            {
                if (!StandingPoints[i].Occupied)
                {
                    return StandingPoints[i];
                }
            }

            return null;
        }

        private HumansAI IsCharacter(Collider other)
        {
            HumansAI ai = GController.GetPeopleHeadFromCollider(other);

            if(ai != null) return ai;

            return null;

            
        }



    }
}

