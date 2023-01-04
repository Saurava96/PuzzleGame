using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;

namespace DarkDemon
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class TheCheckPoint2 : MonoBehaviour
    {
        [SerializeField] protected LevelsEnum LevelOfThisCheckPoint = LevelsEnum.TheBeginning;
        [SerializeField] protected CheckPointEnum ThisCheckPointNum = CheckPointEnum.C0;

        [SerializeField] protected Transform PlayerPos;
       // [SerializeField] protected Transform DummbPos;

        [SerializeField] protected bool MainPlayerDetectCheckpoint = true;
        [SerializeField] protected bool ControlledAIDetectCheckpoint = false;
       // [SerializeField] protected bool DummbDetectCheckpoint = false;
        

        [Header("Interactables")]
        [SerializeField] Transform[] ThisCheckPointInteractables;

       // [Header("Characters")]
       // public ControlledAI[] ControlledAI;

        
        

        protected GameController GController;
        protected GameLevelController GameLevelController;
        SaveSystem Save; 
        protected MainPlayerBase Player;
       // protected VR3DCharacterBase Dummb;

        public CheckPointEnum GetThisCheckpointNum { get { return ThisCheckPointNum; } }

        public LevelsEnum GetLevelOfThisCheckPoint { get { return LevelOfThisCheckPoint; } }

        
        


        protected virtual void Start()
        {
            GController = Instancer.GameControllerInstance;
            GameLevelController = GController.GetLevelController;
            Save = new SaveSystem();
            GetComponent<BoxCollider>().isTrigger = true;

            Player = GController.GetPlayer;

           // Dummb = GController.Get3DPlayer;

           // int Layer = LayerMask.NameToLayer("VRPlayerAndDummbOnly");

           // gameObject.layer = Layer;
        }

        /// <summary>
        /// This is called when the game starts at a particular checkpoint.
        /// </summary>
       /* public virtual void OnCheckpointLoaded()
        {
            StartCoroutine(CheckPointLoadedIE());
        }
        */


        public virtual IEnumerator CheckPointLoadedIE()
        {
            int currentCheckpointvalue = (int)ThisCheckPointNum;
            
            int currentGameLevel = GetCurrentLevel();

            Save.LoadGame(currentGameLevel, currentCheckpointvalue);

            //maybe stop player and dummb movement from controller when transporting
            //player to another location
            
            Debug.Log("abvout to change player pos");

           // yield return StartCoroutine(PlaceCharacters());

            yield return StartCoroutine(Player.MovePlayer(PlayerPos));

            // yield return StartCoroutine(Dummb.MoveDummb(DummbPos));

          //  StartCoroutine(Instancer.GameControllerInstance.MoveDummb(DummbPos));




        }
        
        protected virtual IEnumerator PlaceCharacters()
        {
            yield return null;

            /*
            for(int i = 0; i < ControlledAI.Length; i++)
            {
                CheckpointAssociation association = ControlledAI[i].GetComponent<CheckpointAssociation>();

                Transform movePoint = association.DetermineMovePoint(LevelOfThisCheckPoint, ThisCheckPointNum);


                if (association && movePoint !=null)
                {
                    yield return StartCoroutine(Instancer.GameControllerInstance.MoveControlledAI(ControlledAI[i], movePoint));
                }
                

            }*/

        }
        

        

        protected int GetCurrentLevel()
        {
            return (int)GameLevelController.GetCurrentLevel;
        }

        protected TheLevel GetCurrentActiveMainLevel()
        {
            return GameLevelController.GetCurrentMainLevel;
        }


        protected virtual void OnTriggerEnter(Collider other)
        {
            if (GetCurrentLevel() != (int)LevelOfThisCheckPoint) return;

            //if (IsDummb(other))
            {
                
                //if (DummbDetectCheckpoint) { OnCheckPointEntered(); }

            }

            if (MainPlayerDetectCheckpoint)
            {
                if (GController.IsPlayer(other)) { OnCheckPointEntered(); }
            }

            if (ControlledAIDetectCheckpoint)
            {
                if (GController.IsControlledAI(other))
                {
                    OnCheckPointEntered();
                    
                }
            }

        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (GetCurrentLevel() != (int)LevelOfThisCheckPoint) return;


            if (MainPlayerDetectCheckpoint)
            {
                if (GController.IsPlayer(other)) { OnCheckPointExit(); }

            }

            if (ControlledAIDetectCheckpoint)
            {
                if (GController.IsControlledAI(other))
                {
                    OnCheckPointExit();
                }
            }
        }

        

        /// <summary>
        /// This is called when the players hits the trigger while playing the game.
        /// This is not called in the beginning of the game or when the checkpoint is loaded.
        /// </summary>
        public virtual void OnCheckPointEntered()
        {
            if (GController.GetLevelController.CurrentGameMode != GameMode.MainStory) return;

            int currentCheckpointvalue = (int)ThisCheckPointNum;

            int currentGameLevel = GetCurrentLevel();

            int maxcheckpointforLevel = Save.LoadLevelHighestCheckpoint(currentGameLevel);

            if (currentCheckpointvalue <= maxcheckpointforLevel) return;

            //if (!Save.IsKeyExist(currentGameLevel, currentCheckpointvalue - 1)) return;

            Debug.Log("currentcheckpointvalue: " + currentCheckpointvalue);

            Save.SaveAtCheckpoint(currentGameLevel, currentCheckpointvalue, ThisCheckPointInteractables);

            GetCurrentActiveMainLevel().SetCurrentCheckPoint = this;

        }

        public virtual void OnCheckPointExit()
        {

        }


        

        protected virtual void OnDisable()
        {
            StopAllCoroutines();
        }

        protected virtual void Update() { }
    }
}

