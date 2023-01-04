using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPanda
{
    public class MainPlayerBodyParts : MonoBehaviour
    {
        public enum BodyPart { None, Head, LeftHand, RightHand, PlayerController, HeadInventory, HeadCollision, Fader,HandPointer }

        public BodyPart ThisPart = BodyPart.None;
    }
}

