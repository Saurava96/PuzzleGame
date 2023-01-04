using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DarkDemon
{
    public class ButtonLetter : MonoBehaviour
    {
        public KeyCode key;
        TextMeshProUGUI Text;
        

        public void PrintLetter()
        {
            if(Instancer.GameControllerInstance.GetLevelController.GetCurrentLevel != LevelsEnum.LetterToW) { return; }

            if (Text == null)
            {
                LetterToW level = (LetterToW)Instancer.GameControllerInstance.GetLevelController.GetCurrentMainLevel;
                Text = level.Text;
            }
            
            if (key == KeyCode.Backspace)
            {
                Text.text = "";
                return;
            }

            if(key == KeyCode.Space)
            {
                Text.text += " ";
                return;
            }

            if(key == KeyCode.Tab)
            {
                return;
            }


            Text.text += key.ToString();
        }
        
    }
}

