using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;

namespace DarkDemon
{
    public class SaveInfo
    {
        public SaveInfo(int level, int checkpoint)
        {
            CurrentLevel = level;
            CurrentCheckPoint = checkpoint;
        }

        public int CurrentLevel;
        public int CurrentCheckPoint;
    }

    public class SaveSystem
    {
        

        public void SaveAtCheckpoint(int currentLevel, int currentCheckpoint, Transform[] game = null)
        {
            GameController controller = Instancer.GameControllerInstance;
            GameLevelController levelcontroller = controller.GetLevelController;

            string finalfilelocation = "";
            
            if (controller.GetLevelController.CurrentGameMode == GameMode.MainStory)
            {
                finalfilelocation = controller.GetFinalFileLocation
                (levelcontroller.GetCurrentProfile, currentLevel);
            }
            else if(controller.GetLevelController.CurrentGameMode == GameMode.LevelSelect)
            {
                finalfilelocation = controller.GetFinalFileLocationLS(currentLevel);

            }
            
            
            string keyname = controller.GetBaseString(currentLevel) + "_" + currentCheckpoint.ToString();

            if (ES3.KeyExists(finalfilelocation))
            {
                int loadedCheckpoint = GetCheckPointNum(keyname);

                if (currentCheckpoint <= loadedCheckpoint) return;

                ES3.Save(keyname, game, finalfilelocation);

                return;
            }



            ES3.Save(keyname, game, finalfilelocation);

        }

        

        public void LoadGame(int level, int currentCheckpoint)
        {
            string finalfilelocation = Instancer.GameControllerInstance.GetFinalFileLocation
                (Instancer.GameControllerInstance.GetLevelController.GetCurrentProfile, level);

            if (!ES3.FileExists(finalfilelocation)) return;

            for(int i = 0; i <= currentCheckpoint; i++)
            {
                string finalKey = Instancer.GameControllerInstance.GetBaseString(level) 
                    + "_" + i.ToString();

                if (ES3.KeyExists(finalKey,finalfilelocation))
                {
                    ES3.Load(finalKey,finalfilelocation);
                }

            }

            

        }

        

        private int GetLevelNum(string key)
        {
            return int.Parse(key.Substring(key.IndexOf("_") - 1, 1));
        }

        private int GetCheckPointNum(string key)
        {
            return int.Parse(key.Substring(key.IndexOf("_") + 1, 1));
        }

        public bool IsSaveDirectoryExistsLS()
        {
            GameController controller = Instancer.GameControllerInstance;

            string savedirectory = controller.GetSaveDirectoryLS();

            if (!ES3.DirectoryExists(savedirectory))
            {
                return false;
            }

            return false;
        }

        public bool IsSaveDirectoryExists()
        {
            GameController controller = Instancer.GameControllerInstance;

            GameProfiles profile = controller.GetLevelController.GetCurrentProfile;

            string savedirectory = controller.GetSaveDirectory(profile);

            if (!ES3.DirectoryExists(savedirectory))
            {
                return false;
            }
            else
            {
                if (ES3.FileExists(controller.GetFinalFileLocation(profile, 0)))
                {
                    return true;
                }
            }

            return false;



        }

        
        

        public int LoadHighestLevel()
        {
            GameController controller = Instancer.GameControllerInstance;

            GameProfiles profile = controller.GetLevelController.GetCurrentProfile;

            string savedirectory = controller.GetSaveDirectory(profile);

            int maxlevel = 0;

            if (!ES3.DirectoryExists(savedirectory))
            {
                return maxlevel;
            }

            

            for (int i = 0; i < controller.MaxGameLevel; i++)
            {
                
                if (ES3.FileExists(controller.GetFinalFileLocation(profile, i)))
                {
                    maxlevel = i;
                }
            }

            return maxlevel;





           /*if (!ES3.FileExists()) { return -1; }

            string[] allSavedLevels = ES3.GetKeys();

            if (allSavedLevels.Length <= 0) return -1;

            int[] alllevels = new int[allSavedLevels.Length];

            for (int i = 0; i < allSavedLevels.Length; i++)
            {
                alllevels[i] = GetLevelNum(allSavedLevels[i]);
            }

            return MaxFromList(alllevels);*/
        }

        

        private int MaxFromList(int[] list1)
        {
            int max = int.MinValue;

            for(int i = 0; i < list1.Length; i++)
            {
                if(list1[i] > max) { max = list1[i]; }
            }

            return max;
        }

        public int LoadHighestCheckpointLS(int level)
        {
            string finalfilelocation = Instancer.GameControllerInstance.GetFinalFileLocationLS
                (level);



            if (!ES3.FileExists(finalfilelocation)) { return 0; }

            return GetMaxCheckpoint(level, finalfilelocation);


        }

        public void DeleteDirectoryLS()
        {
            GameController controller = Instancer.GameControllerInstance;

            string savedirectory = controller.GetSaveDirectoryLS();

            if (IsSaveDirectoryExistsLS())
            {
                ES3.DeleteDirectory(savedirectory);
            }

            //if(ES3.DirectoryExists())

            //ES3.DeleteDirectory()
            
        }

        public int LoadLevelHighestCheckpoint(int level)
        {
            //Level:1_2

            string finalfilelocation = Instancer.GameControllerInstance.GetFinalFileLocation
                (Instancer.GameControllerInstance.GetLevelController.GetCurrentProfile, level);

            return GetMaxCheckpoint(level, finalfilelocation);


        }

        private int GetMaxCheckpoint(int level, string finalfilelocation)
        {
            int maxCheckpoint = 0;


            for (int i = 0; i < Instancer.GameControllerInstance.MaxCheckpointPerLevel; i++)
            {
                string key = Instancer.GameControllerInstance.GetBaseString(level) + "_" + i.ToString();

                if (ES3.KeyExists(key, finalfilelocation))
                {
                    maxCheckpoint = i;
                }

            }

            return maxCheckpoint;
        }
        

        public bool IsKeyExist(int currentLevel, int currentCheckpoint)
        {
            return false;
            /*
            string finalKey = LevelKey + currentLevel.ToString() + "_" + currentCheckpoint.ToString();

            if (!ES3.FileExists()) return true;

            if (ES3.KeyExists(finalKey)) { return true; }

            return false;
            */
        }

        /*

        public int LoadMaxLevelCheckPoint(int level)
        {
            string finalKey = LevelKey + level;

            int maxCheckpoint;
            
            if (ES3.KeyExists(finalKey))
            {
                maxCheckpoint = Load(finalKey);
            }
            else
            {
                maxCheckpoint = 0;

               
            }


            return maxCheckpoint;

        }

        public void SaveGame(int currentLevel, int currentCheckpoint)
        {
            string finalKey = LevelKey + currentLevel.ToString();

            //check if allowed to save because we only want to save max checkpoint of a level or max checkpoint of max level.

            if (ES3.KeyExists(finalKey))
            {
                int loadedCheckpoint = Load(finalKey);

                if (currentCheckpoint <= loadedCheckpoint) return;

                ES3.Save(finalKey, currentCheckpoint);
            }
            else
            {
                ES3.Save(finalKey, currentCheckpoint);
            }


        }

        public int LoadMaxLevel()
        {
            if (!ES3.FileExists()) { return -1; }

            string[] allSavedLevels = ES3.GetKeys();

            if (allSavedLevels.Length <= 0) return -1;

            int[] alllevels = new int[allSavedLevels.Length];

            for (int i = 0; i < allSavedLevels.Length; i++)
            {
                alllevels[i] = Load(allSavedLevels[i]);
            }


            return MaxFromList(alllevels);

        }

        public int LoadCheckPointFromLevel(int level)
        {
            string finalKey = LevelKey + level.ToString();

            if (!ES3.KeyExists(finalKey)) return int.MinValue;

            return Load(finalKey);
        }

        private int Load(string levelvalue)
        {
            string a = levelvalue;
            string b = string.Empty;


            for (int i = 0; i < a.Length; i++)
            {
                if (Char.IsDigit(a[i]))
                {
                    b += a[i];
                    return int.Parse(b);
                }

            }

            return -1;


        }

    */
    }
}



