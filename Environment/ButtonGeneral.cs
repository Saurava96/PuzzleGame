using LazyPanda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

namespace DarkDemon
{
    public class ButtonGeneral : MonoBehaviour
    {
        GameController Controller;
        GameLevelController LevelController;

        public enum LetterThiefButtonState { None,GoToBoyShop,GoToGirlRoom,GoToBoyRoom,GoToGirlShop,SendLetter,BuyBox,BoyBuyLockKey,GirlBuyLockKey,BoyLockLetterBox,
        BoyUnlockLetterBox,GirlLockLetterBox,GirlUnlockLetterBox}

        public LetterThiefButtonState State = LetterThiefButtonState.None;

        private void Start()
        {
            Controller = Instancer.GameControllerInstance;
            LevelController = Controller.GetLevelController;
        }


        //_______________________________Guess The Word_______________________________________

        public void PlayerGuessTheWordDecision()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.GuessTheWord) { Debug.LogError("Not Guess the word level"); return; }

            GuessTheWord level = (GuessTheWord)LevelController.GetCurrentMainLevel;

            GuessTheWordParts part = GetComponent<GuessTheWordParts>();

            if(part.ThisPart == GuessTheWordParts.Parts.CorrectWord)
            {
                level.CorrectWordSelected = true;
            }

            if(part.ThisPart == GuessTheWordParts.Parts.IncorrectWord)
            {
                level.InCorrectWordSelected = true;
            }




        }




        //_______________________________Sea Thief____________________________________________


        public void PlayerCulpritSeaThief()
        {
            CatchTheCulprit level = GetSeaThief();

            if(level == null) { Debug.LogError("Not sea thief Level"); return; }


            CatchTheCulpritParts part = GetComponentInParent<CatchTheCulpritParts>();

            if (!part) { Debug.LogError("no sea part attached");return; }

            if(part.ThisPart == CatchTheCulpritParts.Parts.Culprit)
            {
                level.CulpritSelected = true;
            }
            else if(part.ThisPart == CatchTheCulpritParts.Parts.NotCulprit)
            {
                level.WrongPersonSelected = true;
            }

            

        }


        private CatchTheCulprit GetSeaThief()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.SeaThief) return null;

            return (CatchTheCulprit)LevelController.GetCurrentMainLevel;
        }


        //_______________________________Letter Thief_________________________________________


        /// <summary>
        /// This is used for cases where I have to change the functionality of the same button during the level.
        /// </summary>
        public void ButtonClicked()
        {
            switch (State)
            {
                case LetterThiefButtonState.SendLetter:
                    SendLetter(); break;

                case LetterThiefButtonState.BoyUnlockLetterBox:
                    BoyUnlockLetterBox(); break;

                case LetterThiefButtonState.GirlUnlockLetterBox:
                    GirlUnlockLetter(); break;

                case LetterThiefButtonState.GoToGirlShop:
                    GoToGirlShop(); break;

                case LetterThiefButtonState.GoToGirlRoom:
                    GoToGirlRoom(); break;

                case LetterThiefButtonState.GoToBoyShop:
                    GoToBoyShop(); break;

                case LetterThiefButtonState.GoToBoyRoom:
                    GoToBoyRoom();
                    break;

                case LetterThiefButtonState.GirlLockLetterBox:
                    GirlLockLetter(); break;

                case LetterThiefButtonState.GirlBuyLockKey:
                    GirlBuyLockKey();
                    break;

                case LetterThiefButtonState.BuyBox:
                    BuyBox();
                    break;

                case LetterThiefButtonState.BoyBuyLockKey:
                    BoyBuyLockKey(); break;

                case LetterThiefButtonState.BoyLockLetterBox:
                    BoyLockLetterBox(); break;

                case LetterThiefButtonState.None:
                    Debug.Log("None state"); break;

               
            }
        }


        private LetterThief GetLetterThief()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return null;

            return (LetterThief)LevelController.GetCurrentMainLevel;

        }

        
        
        public void GoToBoyShop()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return;

            GetLetterThief().CurrentLevelState = LetterThief.LevelStates.BoyStore;

        }

        public void GoToGirlRoom()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return;

            GetLetterThief().CurrentLevelState = LetterThief.LevelStates.GirlRoom;
        }

        public void GoToBoyRoom()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return;

            GetLetterThief().CurrentLevelState = LetterThief.LevelStates.BoyRoom;
        }

        public void GoToGirlShop()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return;

            GetLetterThief().CurrentLevelState = LetterThief.LevelStates.GirlStore;
        }

        public void SendLetter()
        {

            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return;

            LetterThief level = GetLetterThief();

            GameObject letterbox = level.LetterBox;
            ChestBox box = letterbox.GetComponentInChildren<ChestBox>();

            Debug.Log("Send letter clicked");

            if (box.NumLocks <= 0)
            {
                //then letter gets stolen.
                Debug.Log("No Locks bought");
                return;
            }

            switch (GetLetterThief().CurrentLevelState)
            {
                case LetterThief.LevelStates.BoyRoom:
                    GoToGirlRoom();
                    Debug.Log("Go To Girl Room");
                    break;

                case LetterThief.LevelStates.GirlRoom:
                    GoToBoyRoom();
                    Debug.Log("Go To Boy Room");
                    break;
                   
            }

        }

        

        public void BuyBox()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return;

            GetLetterThief().BoxBought = true;

            //Disable the box..
            GetLetterThief().LetterBox.SetActive(false);


        }

        public void BoyBuyLockKey()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return;

            LockKey key = GetComponentInParent<LockKey>();

            if (!key) { Debug.LogError("no LockKey component found");return; }

            LetterThief level = GetLetterThief();

            level.AddToBoyBoughtKeys(key);

            key.gameObject.SetActive(false);

            GetLetterThief().BoyBoughtKey = true;
        }

        public void GirlBuyLockKey()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return;

            LockKey key = GetComponentInParent<LockKey>();

            if (!key) { Debug.LogError("no LockKey component found"); return; }

            LetterThief level = GetLetterThief();

            level.AddToGirlBoughtKeys(key);

            GetLetterThief().GirlBoughtKey = true;
        }

        public void BoyLockLetterBox()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return;

            LetterThief level = GetLetterThief();

            if(level.BoxBought && level.BoyBoughtKey)
            {
                Debug.Log("Lock letter boy");
                //Lock Box;
                ChestBox box = level.LetterBox.GetComponent<ChestBox>();
                box.LockBox(LetterThief.LevelStates.BoyRoom);
            }
        }

        public void BoyUnlockLetterBox()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return;

            LetterThief level = GetLetterThief();

            if (level.BoxBought && level.BoyBoughtKey)
            {
                Debug.Log("UnLock letter boy");
                //Unlock Box;
                ChestBox box = level.LetterBox.GetComponent<ChestBox>();
                box.UnLockBox(LetterThief.LevelStates.BoyRoom);
            }
        }

        public void GirlLockLetter()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return;

            LetterThief level = GetLetterThief();

            if (level.BoxBought && level.GirlBoughtKey)
            {
                
                //Unlock Box;
                ChestBox box = level.LetterBox.GetComponent<ChestBox>();
                box.LockBox(LetterThief.LevelStates.GirlRoom);
            }
        }

        public void GirlUnlockLetter()
        {
            if (LevelController.GetCurrentLevel != LevelsEnum.LetterThief) return;

            LetterThief level = GetLetterThief();

            if (GetLetterThief().BoxBought && GetLetterThief().GirlBoughtKey)
            {
                //Unlock Box;
                ChestBox box = level.LetterBox.GetComponent<ChestBox>();
                box.UnLockBox(LetterThief.LevelStates.GirlRoom);
            }
        }



    }
}

