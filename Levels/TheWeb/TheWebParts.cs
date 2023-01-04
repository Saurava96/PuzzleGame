using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class TheWebParts : MonoBehaviour
    {
        public enum Parts { None, RunnerStartingTile, CatcherStartingTile, PlayerPlatformPos, PlayerPosOnTile,Wheel }
        
        public Parts ThisPart = Parts.None;
    }
}

