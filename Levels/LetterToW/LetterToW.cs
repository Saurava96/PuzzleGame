using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DarkDemon
{
    public class LetterToW : TheLevel
    {
        public TextMeshProUGUI Text;
        public WeaponSpawner HomingGrenadeSpawner;

        public bool CorrectPassword { get; set; } = false;

        public override LevelsEnum GetStateType()
        {
            return LevelsEnum.LetterToW;
        }

        protected override void VariableReference()
        {
            for(int i = 0; i < LevelInstantiatedObjects.Count; i++)
            {
                GameObject game = LevelInstantiatedObjects[i];

                LetterToWParts part = game.GetComponentInChildren<LetterToWParts>();

                if (part)
                {
                    switch (part.ThisPart)
                    {
                        case LetterToWParts.Parts.ScreenText:
                            Text = part.GetComponentInChildren<TextMeshProUGUI>(); break;

                        case LetterToWParts.Parts.HomingLauncher:
                            HomingGrenadeSpawner = part.GetComponent<WeaponSpawner>(); break;
                    }
                }

            }

            
        }

        public override void LevelUpdate()
        {
            if (!Text) return;

            if (Text.text == "THE DAY WE MET")
            {
                CorrectPassword = true;
                return;

            }

            CorrectPassword = false;
        }

        public override void SetLevelMusic()
        {
            
        }

        public override void SetWeather()
        {
            
        }
    }
}

