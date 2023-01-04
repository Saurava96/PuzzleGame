using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;

namespace DarkDemon
{
    public class CheckpointAssociation : MonoBehaviour
    {   
        [Header("Point 1")]
        public LevelsEnum LevelForPosition1 = LevelsEnum.TheBeginning;
        public CheckPointEnum CheckpointForPosition1 = CheckPointEnum.C0;
        [Tooltip("Move to this Position if the level is LevelForPosition1 and the Checkpoint is CheckpointForPosition1.")]
        public Transform Position1;

        [Header("Point 2")]
        public LevelsEnum LevelForPosition2 = LevelsEnum.TheBeginning;
        public CheckPointEnum CheckpointForPosition2 = CheckPointEnum.C0;
        [Tooltip("Move to this Position if the level is LevelForPosition2 and the Checkpoint is CheckpointForPosition2.")]
        public Transform Position2;



        public Transform DetermineMovePoint(LevelsEnum currentLevel, CheckPointEnum currentCheckpoint)
        {

            if(currentLevel == LevelForPosition1 && currentCheckpoint == CheckpointForPosition1)
            {
                return Position1;
            }

            if (currentLevel == LevelForPosition2 && currentCheckpoint == CheckpointForPosition2)
            {
                return Position2;
            }

            return null;

        }

    }
}

