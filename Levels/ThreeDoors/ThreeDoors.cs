using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace DarkDemon
{
    public class ThreeDoors : TheLevel
    {
        private EnvironmentGeneral LeftInstruction;
        private EnvironmentGeneral RightInstruction;
        private EnvironmentGeneral CenterInstruction;
        private EnvironmentGeneral MainQ;
        
        public List<EnvironmentGeneral> UIs;

        public override LevelsEnum GetStateType()
        {
            return LevelsEnum.ThreeDoors;
        }


        protected override void VariableReference()
        {
            UIs = new List<EnvironmentGeneral>();

            for (int i = 0; i < LevelInstantiatedObjects.Count; i++)
            {
                GameObject game = LevelInstantiatedObjects[i];

                ThreeDoorsParts part = game.GetComponentInChildren<ThreeDoorsParts>();

                if (part)
                {
                    switch (part.Thispart)
                    {
                        case ThreeDoorsParts.Parts.Center:
                            
                            CenterInstruction = part.GetComponentInChildren<EnvironmentGeneral>();
                            UIs.Add(CenterInstruction);
                            break;
                        
                        case ThreeDoorsParts.Parts.Left:
                            
                            LeftInstruction = part.GetComponentInChildren<EnvironmentGeneral>();
                            UIs.Add(LeftInstruction);
                            break;

                        case ThreeDoorsParts.Parts.Right:
                           
                            RightInstruction = part.GetComponentInChildren<EnvironmentGeneral>();
                            UIs.Add(RightInstruction);
                            break;

                        case ThreeDoorsParts.Parts.MainQ:
                            
                            MainQ = part.GetComponentInChildren<EnvironmentGeneral>();
                            UIs.Add(MainQ);
                            break;
                        
                    }
                }

            }


        }

        public override void SetLevelMusic()
        {
            
        }

        public override void SetWeather()
        {
            
        }
    }

}
