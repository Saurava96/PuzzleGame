using LazyPanda;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class Rotater : EnvironmentBase
    {
        private bool AllowRotation = true;

        public override void InteractionStart()
        {
            
        }

        

        public override void InteractionEntered(LevelsEnum CurrentLevel, CheckPointEnum CurentCheckpoint)
        {
            if(CurrentLevel == LevelsEnum.LetterToW)
            {
                LetterToW level = (LetterToW)Instancer.GameControllerInstance.GetLevelController.GetCurrentMainLevel;

                if (!level.CorrectPassword) return;

                if (!AllowRotation) return;

                EasyQuaternion.Instance.RotateTransformByAngle(transform, 2, new Vector3(70, 0, 0));

                AllowRotation = false;
            }
        }



    }
}

