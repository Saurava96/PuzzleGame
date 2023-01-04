using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidBodyParts : MonoBehaviour
{
    public enum HumanoidBodyPartsEnum { None, Head, Mesh}

    public HumanoidBodyPartsEnum ThisPart = HumanoidBodyPartsEnum.None;
}
