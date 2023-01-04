using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;


namespace DarkDemon
{
    public class CatchMeTile : MonoBehaviour
    {
        public CatchMeTile[] Neighbours;

        public int X = 0;

        public int Y = 0;

        public bool AllowTriggerDetection = true;

        public Transform CameraViewPosition;

        public bool UniqueTile = false;
        public bool NeighbourToUniqueTile = false;

        
        GameController Controller;
        Dictionary<LevelsEnum, TheLevel> LevelDic;
        TheWeb Web;
        CatchMeBehaviour RunnerBehaviour;
        CatchMeBehaviour CatcherBehaviour;

        
        BoxCollider BoxTrigger;

        
        const float BigTriggerY = 1f;

        
        const float SmallTriggerY = 0.05f;

        const float MinDistanceThreshold = 1;
        const float MaxDistanceThreshold = 5;


        private bool DummbCloseToTile = false;
        private bool ToboCloseToTile = false;

        Transform ToboHipBone;
        Transform DummbHipBone;
        GameLevelController LevelController;

        private void Start()
        {
            Controller = Instancer.GameControllerInstance;
            BoxTrigger = GetComponent<BoxCollider>(); //10 0.5

            LevelController = Controller.GetLevelController;
           // ToboHipBone = Controller.GetOrangeTobo.GetRootBone;
           // DummbHipBone = Controller.Get3DPlayer.GetHipBone;
        }

        private void Update()
        {
           // if (LevelController.GetCurrentLevel != LevelsEnum.CatchMeIfYouCan) return;
            if (!AllowTriggerDetection) return;
            if (!Controller) return;
            if (!ToboHipBone) return;
            if (!DummbHipBone) return;

            //LookAtDummbY();

          //  DistanceFromDummb();
          //  DistanceFromTobo();
        }

        private void DistanceFromDummb()
        {
            float sqLDummb = GetSquaredDistance(DummbHipBone);

            if (DummbCloseToTile)
            {
                if (sqLDummb > MaxDistanceThreshold * MaxDistanceThreshold)
                {
                    DummbCloseToTile = false;
                }
            }
            else
            {   //it means dummb landed on this tile. 
                if (sqLDummb < MinDistanceThreshold * MinDistanceThreshold)
                {
                    CatcherOnTile();
                    DummbCloseToTile = true;
                }
            }

        }

        private void DistanceFromTobo()
        {
            float sqTobo = GetSquaredDistance(ToboHipBone);

            if (ToboCloseToTile)
            {
                if (sqTobo > MaxDistanceThreshold * MaxDistanceThreshold)
                {
                    ToboCloseToTile = false;
                }
            }
            else
            {   //it means dummb landed on this tile. 
                if (sqTobo < MinDistanceThreshold * MinDistanceThreshold)
                {
                    RunnerOnTile();
                    ToboCloseToTile = true;
                }
            }

        }

        private float GetSquaredDistance(Transform target)
        {
            Vector3 offset = transform.position - target.position;
            float sqLDummb = offset.sqrMagnitude;
            return sqLDummb;
        }

        

        private void OnTriggerEnter(Collider other)
        {
          //  if (LevelController.GetCurrentLevel != LevelsEnum.CatchMeIfYouCan) return;

            if (!AllowTriggerDetection) return;

            Initializer();


            if (IsCatcher(other) && CatcherBehaviour.MoveToTile == this)
            {
                UpdateBoxTrigger(BigTriggerY);
                Debug.Log("dummai trigger");
                CatcherOnTile();

            }

            if (IsRunner(other) && RunnerBehaviour.MoveToTile == this)
            {
                UpdateBoxTrigger(BigTriggerY);

                RunnerOnTile();
            }
        }

        private void CatcherOnTile()
        {
            Debug.Log("character head after return: " + gameObject.name);

            CatcherBehaviour.CurrentTile = this;
            CatcherBehaviour.MoveToTile = null;
            
            CatcherBehaviour.StandAndLaugh();
            
            Web.SwitchTurn(TheWeb.Turn.Runn);
        }

        private void RunnerOnTile()
        {
            Debug.Log("human head after return: " + gameObject.name);

            RunnerBehaviour.CurrentTile = this;
            RunnerBehaviour.MoveToTile = null;
            
            RunnerBehaviour.StandAndLaugh();
            Web.SwitchTurn(TheWeb.Turn.Catch);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!AllowTriggerDetection) return;

            if (IsCatcher(other) || IsRunner(other))
            {
                UpdateBoxTrigger(SmallTriggerY);
            }
        }


        private void UpdateBoxTrigger(float sizeZ)
        {
            if (!BoxTrigger) { Debug.LogError("No box collider"); }
            if (!BoxTrigger.isTrigger) { Debug.LogError("Change radius of trigger only, this box collider is not trigger"); }


            BoxTrigger.size = new Vector3(BoxTrigger.size.x, BoxTrigger.size.y, sizeZ);

        }
        

        private void Initializer()
        {
            if (!Web)
            {
                Web = (TheWeb)Controller.GetLevelController.GetCurrentMainLevel;
                
            }

            if (!RunnerBehaviour)
            {
                RunnerBehaviour = Web.RunnerCurrentBehaviour;
            }

            if (!CatcherBehaviour)
            {
                CatcherBehaviour = Web.CatcherCurrentBehaviour;
            }

           
        }

      

        protected virtual bool IsCatcher(Collider other)
        {
            HumansAI humanhead = Controller.GetPeopleHeadFromCollider(other);

            if (!humanhead) return false;

            if (humanhead.GetHumanoidType == HumanoidType.ControlledAI)
            {
                return true;
            }

            return false;

            
        }

        protected virtual bool IsControlledAI(Collider other)
        {
            ControlledAI ai = Instancer.GetControlledAIFromHeadCollider(other);

            if (ai) { return true; }

            return false;

        }

        protected virtual bool IsRunner(Collider other)
        {
            HumansAI humanhead = Controller.GetPeopleHeadFromCollider(other);

            if (!humanhead) return false;

            if(humanhead.GetHumanoidType == HumanoidType.AI)
            {
                return true;
            }

            

            return false;
        }

    }
}

