using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class GuessTheWordParts : MonoBehaviour
    {
        public enum Parts { None,MainPersonPos,Person1Pos,Person2Pos,Person3Pos, BlackBoard, CorrectWord,IncorrectWord }

        public Parts ThisPart = Parts.None;
        
    }
}

