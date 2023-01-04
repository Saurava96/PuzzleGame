using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBodyParts : MonoBehaviour
{
    public enum BodyParts { None, Hip, Head, LHand, RHand}

    [SerializeField] BodyParts ThisPart = BodyParts.None;
    
}
