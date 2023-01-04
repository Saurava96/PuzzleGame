using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class SeaThief : CatchTheCulprit
    {
        public override LevelsEnum GetStateType()
        {
            return LevelsEnum.SeaThief;
        }

        protected override void VariableReference()
        {
            VariableInitializer();

        }


        public override void SetLevelMusic()
        {

        }

        public override void SetWeather()
        {

        }

    }
}

