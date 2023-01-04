using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class DemoLevel : TheLevel
    {
        
        public override LevelsEnum GetStateType()
        {
            return LevelsEnum.FirstSceneAndDemo;
        }


        public override IEnumerator OnLevelEntered()
        {
           // GameMenu uiscreen = Controller.GetUIScreen;

           // uiscreen.transform.position = GameMenuTransform.position;
           // uiscreen.transform.rotation = GameMenuTransform.rotation;
            //uiscreen.ShowGameMenu();
           // uiscreen.EnableMenu(MenuParts.MenuPart.FirstMenu);

            yield return StartCoroutine(base.OnLevelEntered());


        }

       

        public override void SetLevelMusic()
        {
            
        }

        public override void SetWeather()
        {
            
        }
    }
}

