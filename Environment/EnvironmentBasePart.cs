using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBasePart : MonoBehaviour
{
    public enum EnvironmentParts { None, MeshRenderer}

    public EnvironmentParts ThisPart = EnvironmentParts.None;
    
}
