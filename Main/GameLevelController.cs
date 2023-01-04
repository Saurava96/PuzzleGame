using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkDemon
{
    public class GameLevelController : GameLevel
    {
        [SerializeField] bool RunHeavyGameObjects = true;

        protected override void Start()
        {
            

            if (RunHeavyGameObjects)
            {
                StartCoroutine(HeavyObjectsLoader());
            }
            else
            {
                base.Start();
            }
            

        }

        protected virtual void UpdateProfile(GameProfiles profile)
        {
            SetGameProfile = profile;
        }



        public void ContinueGame()
        {
            SaveSystem save = new SaveSystem();

            int maxLevel = save.LoadHighestLevel();

            if (maxLevel == -1) return;

            SetCurrentLevel = (LevelsEnum)maxLevel;

        }

        /// <summary>
        /// This method is only used when the player has reached the end of the level and
        /// is on the levelchangerTile.
        /// </summary>
        public void ChangeToNextLevel()
        {
            if(CurrentGameMode == GameMode.MainStory)
            {
                //int currentlevel = (int)GetCurrentLevel;

                //currentlevel++;

                LevelsEnum nextLevel = NextLevel();

                int next = (int)nextLevel;
                
                //saving when the level is exited at the first checkpoint of the next level
                //so if game quits while transitioning, the player does not have to replay from the 
                //last checkpoint, but play the next level.
                Save.SaveAtCheckpoint(next, 0);

                SetCurrentLevel = nextLevel;
            }
            else
            {
                //bring the menu at the right place with pause menu on.
                LoadSpecificLevel(LevelsEnum.FirstSceneAndDemo);

                //back to the demo scene with the menu to selected another level.
            }

            
        }

        /// <summary>
        /// Decides the next level to start after a certain level is over.
        /// </summary>
        /// <returns></returns>
        private LevelsEnum NextLevel()
        {
            switch (GetCurrentLevel)
            {
                case LevelsEnum.FirstSceneAndDemo:
                    return LevelsEnum.TheBeginning;
                case LevelsEnum.TheBeginning:
                    break;
            }

            return LevelsEnum.FirstSceneAndDemo;
        }


        public void LoadSpecificLevel(LevelsEnum level)
        {
            Save.DeleteDirectoryLS();

            CurrentGameMode = GameMode.LevelSelect;

            SetCurrentLevel = level;
        }

        

        //not being used
        public void ReloadLastCheckpoint()
        {
            //currently reload will basically disable and reenable the objects, but 
            //if there is a weird problem, then taht would not be solved by this.
            //the objects need to be deleted and then instantiated, but we currently,
            //dont do that. Make prefab of every object which is in the scenes and then
            //destroy and instantiate again. We will see..

            //reload level needs to be implemented too.

           // if (!CurrentMainLevel) { Debug.LogError("current level not defined"); return; }

            //CurrentMainLevel.GetCurrentCheckpoint.OnCheckpointLoaded();

        }

        private IEnumerator HeavyObjectsLoader()
        {
            yield return StartCoroutine(LoadHeavyObject(true));

            yield return new WaitForSeconds(1);

            yield return StartCoroutine(LoadHeavyObject(false));

            yield return new WaitForSeconds(1);

            base.Start();
        }

        private IEnumerator LoadHeavyObject(bool value)
        {
            for (int i = 0; i < HeavyGameObjects.Length; i++)
            {
                Transform heavy = HeavyGameObjects[i];

                if (heavy.childCount > 0)
                {
                    for (int j = 0; j < heavy.childCount; j++)
                    {
                        Transform child = heavy.GetChild(j);
                        child.gameObject.SetActive(value);
                        yield return new WaitForEndOfFrame();
                    }
                }

            }
        }

        protected override void Update()
        {
            base.Update();


            
        }


        //max level and max checkpoint for continuing the game when the player returns to play the game.
        
        // Max Checkpoint in the current level the player has crossed to.
        
        //You save in the file when the player has crossed the max level and max checkpoint
        //we can have a system of max checkpoint saved for each level. 
        //so when the player wants to player another level again, then they can contnue later on as well from the same checkpoint
        //in that level even if it not the max level.

        //Continue game looks for max level and max checkpoint
        //if a level is selected which is less than max level, then  "continue level" button can be pressed to continue the game from that checkpoint
        //in the same level.

        //maxlevel, maxcheckpoint

        //levelname, maxcheckpoint
        //level2, maxcheckpoint

        


        //NOT BEING USED______________________________________________________________________________________________________________

        /*

        private void CheckSceneChange()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                int curr = SceneManager.GetActiveScene().buildIndex;
                int maxScene = SceneManager.sceneCountInBuildSettings;

                curr++;

                if (curr > maxScene - 2) { curr = 0; SceneToGoTo = curr; }



                StartCoroutine(MoveToActualScene(curr)); //taking it to the empty scene.. maxScene in build index is the last scene...


            }
        }

        private IEnumerator MoveToEmptyScene(int num)
        {
            yield return StartCoroutine(SceneChange(num));

            Debug.Log("Empty Scene Loaded");
            yield return new WaitForSeconds(0.5f);

            StartCoroutine(MoveToActualScene(SceneToGoTo));

        }

        private IEnumerator MoveToActualScene(int num)
        {
            yield return StartCoroutine(SceneChange(num));

            Debug.Log("Actual level loaded");

            LevelUpdated(num);
        }

        private IEnumerator SceneChange(int num)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(num, LoadSceneMode.Single);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        */

    }
}

