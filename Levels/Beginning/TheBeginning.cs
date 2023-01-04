using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class TheBeginning : TheLevel
    {

        public override IEnumerator OnLevelEntered()
        {
            yield return StartCoroutine(base.OnLevelEntered());

            Debug.Log("beginning level");
        }




        public override LevelsEnum GetStateType()
        {
            return LevelsEnum.TheBeginning;
        }

        

        public override void SetLevelMusic()
        {
            //throw new System.NotImplementedException();
        }

        

        public override void SetWeather()
        {
            //throw new System.NotImplementedException();
        }

       
    }
}

