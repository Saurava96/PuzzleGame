using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;


namespace DarkDemon
{

    public enum GameProfiles { P1, P2, P3}
    public enum GamePlayers { Dummb, MainPlayer}
    public enum LevelsEnum {FirstSceneAndDemo, TheBeginning, LetterToW, ThreeDoors, LetterThief, SeaThief, StolenRing, GuessTheWord
    ,TheWeb,BankHeist,CrossTheBridge}

    public enum GameMode { MainStory, LevelSelect}

    public abstract class GameLevel : MonoBehaviour
    {
        [SerializeField] protected GameProfiles CurrentProfile = GameProfiles.P1;
        [SerializeField] protected LevelsEnum CurrentLevel = LevelsEnum.TheBeginning;
        [SerializeField] protected Transform[] HeavyGameObjects;

        protected Dictionary<LevelsEnum, TheLevel> LevelsDictionary = new Dictionary<LevelsEnum, TheLevel>();

        protected TheLevel CurrentMainLevel = null;

        public LevelsEnum SetCurrentLevel { set { CurrentLevel = value; } }
        public LevelsEnum GetCurrentLevel { get { return CurrentLevel; } }

        public GameProfiles GetCurrentProfile { get { return CurrentProfile; } }

        public GameProfiles SetGameProfile { set { CurrentProfile = value; } }

        public GameMode CurrentGameMode { get; set; } = GameMode.MainStory;
        //Getters
        public TheLevel GetCurrentMainLevel { get { return CurrentMainLevel; } }

        protected GameController GameController;

        protected SaveSystem Save;

        public Dictionary<LevelsEnum, TheLevel> GetLevelsDic { get { return LevelsDictionary; } }

        public bool IsLevelChanging { get; private set; } = false;
        
        protected virtual void Start()
        {

            TheLevel[] states = GetComponentsInChildren<TheLevel>(true);

            for (int i = 0; i < states.Length; i++)
            {
                if (states[i] != null && !LevelsDictionary.ContainsKey(states[i].GetStateType())) //making sure that if the state is not null
                                                                                                     // and the state is already not in the dict, then adding the new state to the dictionary. 
                {
                    LevelsDictionary[states[i].GetStateType()] = states[i]; //adding the state to the dict here.. The key is the currentbehaviour
                                                                               //and the value is the state itself.
                }
            }

            GameController = Instancer.GameControllerInstance;
            Save = new SaveSystem();

            CurrentMainLevel = LevelsDictionary[CurrentLevel];

            CurrentMainLevel.SetLevel = this;



            StartCoroutine(CurrentMainLevel.OnLevelEntered());

        }

        protected virtual void Update()
        {
            if (CurrentMainLevel == null) { return; }
            if (IsLevelChanging) return;
        //  if (GameController.GamePaused) return; commented because when game is paused, player
        //might want to change level or go back to the main menu in the game which requires level change.

            LevelsEnum newType = CurrentMainLevel.GetStateType();

            if (CurrentLevel != newType)
            {
                StartCoroutine(LevelUpdateIE());

            }

            if (CurrentMainLevel != null)
            {
                CurrentMainLevel.LevelUpdate();
                CurrentMainLevel.IsGameOver();

            }

        }

        

        public virtual IEnumerator LevelUpdateIE()
        {
            if (CurrentMainLevel == null) { yield break; }
            if (IsLevelChanging) yield break;
            

            IsLevelChanging = true;

            Debug.Log("About to exit level");
            
            yield return StartCoroutine(CurrentMainLevel.OnLevelExit());
            
            CurrentMainLevel = LevelsDictionary[CurrentLevel]; //setting the new behaviour here 
            
            CurrentMainLevel.SetLevel = this;
            
            
            yield return StartCoroutine(CurrentMainLevel.OnLevelEntered());
            
            IsLevelChanging = false;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (CurrentMainLevel == null) return;

            if (IsDummb(other))
            {
               // CurrentMainLevel.OnTriggerEntered(GamePlayers.Dummb);
            }

            if (IsPlayer(other))
            {
               // CurrentMainLevel.OnTriggerEntered(GamePlayers.MainPlayer);
            }

            
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (CurrentMainLevel == null) return;

            if (IsPlayer(other))
            {
                //CurrentMainLevel.OnTriggerStayed();
            }

            
        }


        protected virtual void OnTriggerExit(Collider other)
        {
            if (CurrentMainLevel == null) return;

            if (IsPlayer(other))
            {
                //CurrentMainLevel.OnTriggerExited();
            }
            
        }

        protected virtual bool IsPlayer(Collider other)
        {
            if (other.GetComponent<MainPlayerBodyParts>())
            {
                if (other.GetComponent<MainPlayerBodyParts>().ThisPart == MainPlayerBodyParts.BodyPart.HeadInventory)
                {
                    return true;
                }
            }

            return false;

        }

        protected virtual bool IsDummb(Collider other)
        {
            if (Instancer.GetHumanFromHeadCollider(other))
            {
                return true;
            }

            return false;
        }

    }
}

