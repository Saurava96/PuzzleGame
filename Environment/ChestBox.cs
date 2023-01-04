using DarkDemon;
using LazyPanda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class ChestBox : MonoBehaviour
    {
        

        public int NumLocks { get; set; } = 0;

        public bool DDBox = true;
        public bool LPBox = true;
        public bool KPBox = false;

        public LevelsEnum CurrentLevel;

        private bool LockedWithDD = false;
        private bool LockedWithLP = false;
        private bool LockedWIthKP = false;


        public void LockBox(LetterThief.LevelStates state)
        {
           // if (state != LetterThief.LevelStates.GirlRoom && state != LetterThief.LevelStates.GirlRoom) return;

            if (state == LetterThief.LevelStates.BoyRoom)
            {
               // if (!AllowLockForBoy()) return;
            }

            if(state == LetterThief.LevelStates.GirlRoom)
            {
               // if(!AllowLockForGirl()) return;
            }

            
            ChestParts[] parts = GetComponentsInChildren<ChestParts>();

            for (int i = 0; i < parts.Length; i++)
            {
                switch (parts[i].ThisPart)
                {
                    case ChestParts.Parts.LeftLock:
                    case ChestParts.Parts.RightLock:

                        if (NumLocks == 0)
                        {
                            parts[i].gameObject.SetActive(false);
                        }
                        else if (NumLocks == 1)
                        {
                            parts[i].gameObject.SetActive(true);
                        }

                        break;

                    case ChestParts.Parts.CenterLock:

                        if (NumLocks == 0)
                        {
                            parts[i].gameObject.SetActive(true);
                        }
                        else if (NumLocks == 1)
                        {
                            parts[i].gameObject.SetActive(false);
                        }

                        break;

                }
            }

            NumLocks++;


        }

        private bool AllowLockForBoy()
        {
            if (CurrentLevel != LevelsEnum.LetterThief) return false;

            LetterThief level = GetLetterThiefLevel();

            for(int i = 0; i < level.BoyBoughtKeyCodes.Count; i++)
            {
                if (level.BoyBoughtKeyCodes[i].CompatibleWithDD)
                {
                    LockedWithDD = true;
                    Debug.Log("Key compatible with DD");
                    return true;
                }
            }

            return false;



        }

        private bool AllowLockForGirl()
        {
            if (CurrentLevel != LevelsEnum.LetterThief) return false;

            LetterThief level = GetLetterThiefLevel();

            for (int i = 0; i < level.GirlBoughtKeyCodes.Count; i++)
            {
                if (level.GirlBoughtKeyCodes[i].CompatibleWithLP)
                {
                    Debug.Log("Key compatible with LP");
                    LockedWithLP = true;
                    return true;
                }
            }

            return false;



        }



        private LetterThief GetLetterThiefLevel()
        {
            if (CurrentLevel != LevelsEnum.LetterThief) return null;

            GameController controller = Instancer.GameControllerInstance;

            GameLevelController levelcontroller = controller.GetLevelController;

            return (LetterThief)levelcontroller.GetCurrentMainLevel;

           
        }

        private bool AllowBoyUnlock()
        {
            LetterThief level = GetLetterThiefLevel();

            for(int i = 0; i < level.BoyBoughtKeyCodes.Count; i++)
            {
                if (level.BoyBoughtKeyCodes[i].CompatibleWithDD)
                {
                    return true;
                }
            }

            return false;
            

        }

        private bool AllowGirlUnlock()
        {
            LetterThief level = GetLetterThiefLevel();

            for (int i = 0; i < level.GirlBoughtKeyCodes.Count; i++)
            {
                if (level.GirlBoughtKeyCodes[i].CompatibleWithLP)
                {
                    return true;
                }
            }

            return false;


        }

        public void UnLockBox(LetterThief.LevelStates state)
        {
            if (state != LetterThief.LevelStates.GirlRoom || state != LetterThief.LevelStates.GirlRoom) return;

            if(state == LetterThief.LevelStates.BoyRoom)
            {
                if (!AllowBoyUnlock()) return;
            }

            if(state == LetterThief.LevelStates.GirlRoom)
            {
                if (!AllowGirlUnlock()) return;
            }

            ChestParts[] parts = GetComponentsInChildren<ChestParts>();

            for (int i = 0; i < parts.Length; i++)
            {
                switch (parts[i].ThisPart)
                {

                    case ChestParts.Parts.LeftLock:
                    case ChestParts.Parts.RightLock:

                        if (NumLocks == 0)
                        {

                        }
                        else if (NumLocks == 1)
                        {
                            parts[i].gameObject.SetActive(false);

                        }
                        else if (NumLocks == 2)
                        {
                            parts[i].gameObject.SetActive(false);
                        }

                        break;

                    case ChestParts.Parts.CenterLock:

                        if (NumLocks == 0)
                        {

                        }
                        else if (NumLocks == 1)
                        {
                            parts[i].gameObject.SetActive(false);
                        }
                        else if (NumLocks == 2)
                        {
                            parts[i].gameObject.SetActive(true);
                        }
                        break;

                }
            }

            NumLocks--;
        }

    }
}

