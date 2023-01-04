using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledAIBodyPart : MonoBehaviour
{
    public enum BodyPart { None, Head, CharacterMesh, CharacterCamera, HipBone, SphereCastPos, CharacterController}

    public BodyPart ThisPart = BodyPart.None;

    
}
