using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public abstract class CatchTheCulprit : TheLevel
    {
        [Header("Manual Reference")]

        [SerializeField] protected GameObject PersonWihProblemPre;
        [SerializeField] protected GameObject Person1Pre;
        [SerializeField] protected GameObject Person2Pre;
        [SerializeField] protected GameObject Person3Pre;


        [Header("Auto Reference")]
        public GameObject PersonWithProb;
        public GameObject Person1;
        public GameObject Person2;
        public GameObject Person3;

        public Transform PlayerDefaultPos;
        public Transform ControlledAIDefaultPos;
        public Transform PersonWithProbPos;
        public Transform Person1Pos;
        public Transform Person2Pos;
        public Transform Person3Pos;


        //

        public bool CulpritSelected { get; set; } = false;

        public bool WrongPersonSelected { get; set; } = false;

        public bool PlayerMadeDecision { get; set; } = false;


        protected virtual void VariableInitializer()
        {
            for (int i = 0; i < LevelInstantiatedObjects.Count; i++)
            {
                GameObject game = LevelInstantiatedObjects[i];

                CatchTheCulpritParts[] parts = game.GetComponentsInChildren<CatchTheCulpritParts>();

                for (int k = 0; k < parts.Length; k++)
                {
                    CatchTheCulpritParts part = parts[k];

                    if (part)
                    {
                        switch (part.ThisPart)
                        {
                            case CatchTheCulpritParts.Parts.PersonWithprobPos:
                                PersonWithProbPos = part.transform; break;

                            case CatchTheCulpritParts.Parts.Person1Pos:
                                Person1Pos = part.transform; break;

                            case CatchTheCulpritParts.Parts.Person2Pos:
                                Person2Pos = part.transform; break;

                            case CatchTheCulpritParts.Parts.Person3Pos:
                                Person3Pos = part.transform; break;

                            case CatchTheCulpritParts.Parts.ControlledDefaultPos:
                                ControlledAIDefaultPos = part.transform; break;

                            case CatchTheCulpritParts.Parts.PlayerDefaultPos:
                                PlayerDefaultPos = part.transform; break;

                        }
                    }
                }

            }
        }

        


        public override void LevelUpdate()
        {
            if (PlayerMadeDecision) return;

            
            if (CulpritSelected)
            {
                //level complete

                Debug.Log("culprit selected");
                PlayerMadeDecision = true;
            }
            
            if(WrongPersonSelected)
            {
                //wrong person, decrease points and continue..
                //game over
                Debug.Log("Worng person selected");
                PlayerMadeDecision = true;
            }
            
        }

    }
}

