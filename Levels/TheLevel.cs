using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;

namespace DarkDemon
{
    public enum CheckPointEnum { C0, C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12, C13, C14, C15 }

    public abstract class TheLevel : MonoBehaviour
    {

        [SerializeField] TheCheckPoint2 CurrentActiveCheckpoint;

        //[SerializeField] protected CheckPointEnum CurrentActiveCheckpoint = CheckPointEnum.C0;

        //protected Dictionary<CheckPointEnum, TheCheckPoint> CheckPointDictionary = new Dictionary<CheckPointEnum, TheCheckPoint>();

        //protected TheCheckPoint CurrentMainCheckPoint = null;

       // public CheckPointEnum SetCurrentCheckPoint { set { CurrentActiveCheckpoint = value; } }
        //public CheckPointEnum GetCurrentCheckPoint { get { return CurrentActiveCheckpoint; } }
        //Getters
        //public TheCheckPoint GetCurrentMainCheckPoint { get { return CurrentMainCheckPoint; } }

        protected GameController Controller;

        public TheCheckPoint2 SetCurrentCheckPoint { set { CurrentActiveCheckpoint = value; } }

        public TheCheckPoint2 GetCurrentCheckpoint { get { return CurrentActiveCheckpoint; } }

        protected TheCheckPoint2[] AllCheckpoints;

        public int TotalCheckpoints { get { return AllCheckpoints.Length; } }

        protected List<GameObject> LevelInstantiatedObjects;

        public CheckPointEnum GetCurrentCheckpointEnum
        {
            get { return CurrentActiveCheckpoint.GetThisCheckpointNum; } 
        }

        protected virtual void Start()
        {
            Controller = Instancer.GameControllerInstance;
            Player = Controller.GetPlayer;
            VRPlayer = (VRMainPlayer)Player;   
           // Dummb = Controller.Get3DPlayer;
            Save = new SaveSystem();
           
           
        }


        


        


        //------------------------------------------------------------------------------------------------------------------------------------------------------------------

        
        [SerializeField] Transform LevelChangerTilePos;

        [Header("Meshes and Models")]

        [SerializeField] Transform LevelMeshesParent;
        [SerializeField] GameObject[] LevelPrefabs;

        [Header("Checkpoint")]
        [SerializeField] Transform CheckpointsParent;

        protected GameLevel ThisGameLevel;

        

        protected MainPlayerBase Player;
        protected VRMainPlayer VRPlayer;
        protected VR3DCharacterBase Dummb;
        SaveSystem Save;


        private bool GameOverIERunning = false;
        private bool LevelReloading = false;


        public virtual GameLevel SetLevel { set { ThisGameLevel = value; } }

        public virtual GameLevel GetGameLevel { get { return ThisGameLevel; } }

        public abstract LevelsEnum GetStateType();

        protected bool IsLevelEntered = false;

       
       

        public abstract void SetWeather();

        protected virtual void LevelStart() { }

        public abstract void SetLevelMusic();

        public virtual void IsGameOver()
        {
            if (!Dummb) return;
            if (!Player) return;

            

            if (GameOverIERunning) return;

            
            if (Dummb.DummbOutOfBounds || Player.PlayerOutOfBounds)
            {
                Debug.Log("game over");
                StartCoroutine(Controller.GetLevelController.LevelUpdateIE());
                
            }

        }

        public virtual void ReloadLevel()
        {
            StartCoroutine(ReloadLevelIE());
        }

        protected virtual IEnumerator ReloadLevelIE()
        {
            if (LevelReloading) yield break;

            if (!CurrentActiveCheckpoint) yield break;

            LevelReloading = true;

            Player.AllowControlShift = false;

           // Controller.GetUIScreen.HideGameMenu();

            yield return StartCoroutine(Controller.GetEnvironmentEffects.ChangeEnvironmentTransition(2, 0, 1));

            yield return StartCoroutine(CurrentActiveCheckpoint.CheckPointLoadedIE());

            yield return StartCoroutine(Controller.GetEnvironmentEffects.ChangeEnvironmentTransition(2, 1, 0));

            Player.AllowControlShift = true;

            LevelReloading = false;


        }

        protected virtual IEnumerator GameOver()
        {
            GameOverIERunning = true;

            if (!CurrentActiveCheckpoint) yield break;

            Player.AllowControlShift = false;

           // Controller.GetUIScreen.HideGameMenu();

            yield return StartCoroutine(Controller.GetEnvironmentEffects.ChangeEnvironmentTransition(2, 0, 1));

            yield return StartCoroutine(CurrentActiveCheckpoint.CheckPointLoadedIE());

            yield return StartCoroutine(Controller.GetEnvironmentEffects.ChangeEnvironmentTransition(2, 1, 0));

            ResetGameOverVariables();

            Player.AllowControlShift = true;

            GameOverIERunning = false;
        }

        protected virtual void ResetGameOverVariables()
        {
            Dummb.DummbOutOfBounds = false;
            Player.PlayerOutOfBounds = false;
        }

        public virtual IEnumerator OnLevelEntered() 
        {
            CheckpointInitialization();


            SaveSystem save = new SaveSystem();

            if(GetStateType() != LevelsEnum.FirstSceneAndDemo)
            {
                int maxCheckpointForThisLevel;
                if (Instancer.GameControllerInstance.GetLevelController.CurrentGameMode != GameMode.MainStory)
                {   //if the game is not being continued, it means the level was selected manually
                    //and we dont want to load any checkpoint because we want 
                    maxCheckpointForThisLevel = 0;
                }
                else
                {
                    maxCheckpointForThisLevel = save.LoadLevelHighestCheckpoint((int)GetStateType());
                }

                SetCurrentCheckPoint = GetCheckpointFromEnumInt(maxCheckpointForThisLevel);
            }

            if (LevelChangerTilePos) { SetLevelChangerPositionInLevel(LevelChangerTilePos); }
            

            if (LevelMeshesParent) { yield return StartCoroutine(LevelToggler(LevelMeshesParent, true)); }

            VariableReference();
            
            yield return StartCoroutine(VariableReferenceIE());

            ControlledAIInstantiate();

            // yield return StartCoroutine(ControlledAICharactersUpdate(true));

            if (!Player) { Debug.LogError("No player"); }

            //if (CurrentMainCheckPoint) { CurrentMainCheckPoint.OnCheckPointLoaded(); } else { Debug.Log("No currentmaincheckpoint"); }

            if (CurrentActiveCheckpoint) 
            {
                Debug.Log("current active checkpoinmt");
                //CurrentActiveCheckpoint.OnCheckpointLoaded();
                yield return StartCoroutine(CurrentActiveCheckpoint.CheckPointLoadedIE());
            }

            SetWeather();

            SetLevelMusic();

            Player.AllowControlShift = true;

            yield return StartCoroutine(Controller.GetEnvironmentEffects.ChangeEnvironmentTransition(2, 1, 0));



            IsLevelEntered = true;

            LevelStart();

            Debug.Log("Level entered");

        }

        private void CheckpointInitialization()
        {
            AllCheckpoints = CheckpointsParent.GetComponentsInChildren<TheCheckPoint2>();
        }

        protected virtual void VariableReference()
        {
            //overriden
        }

        protected virtual void ControlledAIInstantiate() { }

        protected virtual IEnumerator VariableReferenceIE()
        {
            yield return null;
        }

        protected virtual TheCheckPoint2 GetCheckpointFromEnumInt(int value)
        {
            for(int i = 0; i < AllCheckpoints.Length; i++)
            {
                TheCheckPoint2 checkpoint = AllCheckpoints[i];

                if((int)checkpoint.GetThisCheckpointNum == value)
                {
                    return checkpoint;
                }

            }

            return null;

        }

        public virtual void LevelUpdate()
        {
           
        }
        

        private IEnumerator ControlledAICharactersUpdate(bool value)
        {
            yield break;
            /*
            if(AllCheckpoints == null) { AllCheckpoints = CheckpointsParent.GetComponentsInChildren<TheCheckPoint2>(); }

            for(int i = 0; i < AllCheckpoints.Length; i++)
            {
                TheCheckPoint2 checkpoint = AllCheckpoints[i];

                ControlledAI[] ais = checkpoint.ControlledAI;

                for(int j = 0; j < ais.Length; j++)
                {
                    ais[j].gameObject.SetActive(value);
                    yield return null;
                }

                yield return null;

            }


            */
            
        }

        
        
        public virtual IEnumerator OnLevelExit() 
        {
            Player.AllowControlShift = false;
            
           // Controller.GetUIScreen.HideGameMenu();
            
            yield return StartCoroutine(Controller.GetEnvironmentEffects.ChangeEnvironmentTransition(2, 0, 1));

            yield return StartCoroutine(MovePlayerAndDummbToDefaultTile());

            yield return StartCoroutine(ControlledAICharactersUpdate(false));

            

            
            if (LevelMeshesParent) { yield return StartCoroutine(LevelToggler(LevelMeshesParent, false)); }



            
        } 

        
        
        protected virtual IEnumerator MovePlayerAndDummbToDefaultTile()
        {
            
            Transform playerpos = Controller.GetDefaultLevelChangerPos.PlayerPos;
            Transform dummbpos = Controller.GetDefaultLevelChangerPos.DummbPos;

            if (!Controller.GetDefaultLevelChangerPos) yield break;

            yield return StartCoroutine(Player.MovePlayer(playerpos));

            //yield return StartCoroutine(Dummb.MoveDummb(dummbpos));

           // StartCoroutine(Instancer.GameControllerInstance.MoveDummb(dummbpos));

        }

        

        protected virtual void SetLevelChangerPositionInLevel(Transform destination)
        {
            GameController controller = Instancer.GameControllerInstance;
            if (!controller) return;

            LevelChangerTile tile = controller.GetLevelChangerPlatform;
            if (!tile) return;

            tile.transform.position = destination.position;
        }

/*
        protected virtual IEnumerator SimpleLevelToggler(bool enable)
        {
            

            for(int j = 0; j < SimpleLevelMeshes.Length; j++)
            {
                Transform parent = SimpleLevelMeshes[j].transform;


                for (int i = 0; i < parent.childCount; i++)
                {
                    Transform child = parent.GetChild(i);
                    if (!child.GetComponent<DontEnable>())
                    {
                        child.gameObject.SetActive(enable);
                        yield return new WaitForEndOfFrame();
                    }
                    
                }

            }

            

        }
        */
       
        protected virtual IEnumerator LevelToggler(Transform parent, bool enable)
        {

            if (enable)
            {
                for (int i = 0; i < LevelPrefabs.Length; i++)
                {
                    GameObject levelObject = Instantiate(LevelPrefabs[i]);
                    levelObject.transform.parent = parent;
                    LevelInstantiatedObjects ??= new List<GameObject>();
                    LevelInstantiatedObjects.Add(levelObject);

                    yield return new WaitForEndOfFrame();
                }





            }
            else
            {
                for(int i = 0; i < LevelInstantiatedObjects.Count; i++)
                {
                    Destroy(LevelInstantiatedObjects[i]);
                    yield return new WaitForEndOfFrame();
                }

                LevelInstantiatedObjects.Clear();
            }

            
            

            /*
           
            Transform[] LevelObjects = parent.GetComponentsInChildren<Transform>();
            

            yield return new WaitForSeconds(0.1f);
            const int adder = 3;
            int num = adder;

            if (enable)
            {
                int i = 0;

                while (i < LevelObjects.Length)
                {
                    if (!LevelObjects[i].GetComponent<DontEnable>())
                    {
                        LevelObjects[i].gameObject.SetActive(enable);

                        if (i >= num) { yield return new WaitForEndOfFrame(); num += adder; }

                    }

                    i++;
                }
            }
            else
            {
                for(int k = 0; k < LevelObjects.Length; k++)
                {
                    if (!LevelObjects[k].GetComponent<DontEnable>())
                    {
                        LevelObjects[k].gameObject.SetActive(enable);

                    }
                }
            }
            */


        }

        protected virtual void OnDisable()
        {
            StopAllCoroutines();
        }



       

    }
}

