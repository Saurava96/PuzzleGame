using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class StolenRing : CatchTheCulprit
    {
        public override LevelsEnum GetStateType()
        {
            return LevelsEnum.StolenRing;
        }

        protected override void VariableReference()
        {
            VariableInitializer();
        }

        public override void SetLevelMusic()
        {
            throw new System.NotImplementedException();
        }

        public override void SetWeather()
        {
            throw new System.NotImplementedException();
        }
    }
}

