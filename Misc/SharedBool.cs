using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPanda
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Shared/Shared Boolean")]
    public class SharedBool : ScriptableObject
    {
        public bool Value = false;
    }
}

