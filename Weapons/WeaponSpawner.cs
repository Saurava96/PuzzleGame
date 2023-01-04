using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;

namespace DarkDemon
{
    public class WeaponSpawner : EnvironmentBase
    {

        [SerializeField] HomingGrenade GrenadePrefab;
        
        [SerializeField] Transform SpawnPos;

        [SerializeField] float BulletTravelTime = 2;
        


        public Transform Target;


        public override void InteractionEntered(LevelsEnum CurrentLevel, CheckPointEnum CurentCheckpoint)
        {
            if(CurrentLevel == LevelsEnum.LetterToW)
            {
                LetterToW level = (LetterToW)Instancer.GameControllerInstance.GetLevelController.GetCurrentMainLevel;

                if (!level.CorrectPassword)
                {
                    SpawnGrenade();
                }
            }
           
        }

        public void SpawnGrenade()
        {
            GameObject grenade = SimplePool.Spawn(GrenadePrefab.gameObject, SpawnPos.position, Quaternion.identity);
            HomingGrenade homing = grenade.GetComponent<HomingGrenade>();
            homing.Target = Target;
            homing.BulletTravelTime = BulletTravelTime;
            homing.ThrowGrenade(0.05f, SpawnPos,true);

           
        }



    }
}

