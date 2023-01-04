using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DarkDemon
{
    public class BankHeist : TheLevel
    {
        public override LevelsEnum GetStateType()
        {
            return LevelsEnum.BankHeist;
        }


        public override IEnumerator OnLevelEntered()
        {
            yield return StartCoroutine(OnLevelEntered());

            //Add player weapon.

        }

        public override void SetLevelMusic()
        {
            
        }

        public override void SetWeather()
        {
            
        }
    }
}

