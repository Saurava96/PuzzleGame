using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPanda
{
    public class VR3DBodyParts : MonoBehaviour
    {
        public enum RigidBodyParts { None, Head, RightHand,LeftHand,Hip,CharacterMeshMain,CharacterMeshRenderer,CharacterController,
        AnimatorController,PuppetCollisionFall,PuppetFall,CharacterCamera}

        public RigidBodyParts ThisPart = RigidBodyParts.None;
    }
}

