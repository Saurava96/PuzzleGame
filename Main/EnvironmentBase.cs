using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkDemon;


namespace LazyPanda
{
    

    public abstract class EnvironmentBase : MonoBehaviour
    {
        public enum Types { Start, Stay, End }
        public enum DirectionEnum { X, Y, Z }

        
        
        protected MainPlayerBase Player;

        protected VR3DCharacterBase Player3D;

        public bool CurrentlyLooking { get; set; } = false;

        public Transform ObjectMesh { get; private set; }

        public enum EnvironBaseType { None, Heli, AI}

        //set from script VRMailPlayer.cs. Mainly used for those objects which can be controlled by the player..
        public EnvironBaseType EType { get; set; } = EnvironBaseType.None;

        public bool AllowTobeControlledAI = false;

        protected bool RoutineStarted = false;

        

        public bool StartImplemented { get; set; } = false;

        protected virtual void Start()
        {
            Player = Instancer.GameControllerInstance.GetPlayer;

            Player3D = Instancer.GetPlayer3D;

            if (GetComponent<ControlledAI>()) { InteractionStart(); } //others are controlled from button or Ingame joystick
            

            PartsInitializer();

            if (Application.isEditor) { if (gameObject.isStatic) { Debug.Log("GAMEOBJECT IS STATICCCCCCCC: " + gameObject.name); } }

        }

        private void PartsInitializer()
        {
            EnvironmentBasePart[] parts = GetComponentsInChildren<EnvironmentBasePart>();

            for (int i = 0; i < parts.Length; i++)
            {
                switch (parts[i].ThisPart)
                {
                    case EnvironmentBasePart.EnvironmentParts.MeshRenderer:

                        ObjectMesh = parts[i].transform;

                        break;

                }
            }

            
        }

        public virtual void InteractionStart() {}

        public virtual void InteractionStart(bool add) { }

        public virtual void InteractionStart(Vector3 pos) { }

        public virtual void InteractionEntered() { }

        public virtual void InteractionEntered(bool value) { }

        public virtual IEnumerator InteractionEnteredIE(bool value) { yield return null; }

        public virtual void InteractionEntered(LevelsEnum CurrentLevel, CheckPointEnum CurentCheckpoint) { }

        public virtual void InteractionSlider(bool value) { }

        public virtual void InteractionEnd() { }

        public virtual void InteractionStay() { }

        public virtual void ActivateEnvironmentBase(bool value) { }

        public virtual void InteractionStay(bool val) { }

        public virtual void InteractionStay(float value) { }

        public virtual void TimedAction() { }

        public virtual void InteractionStay(float val1 = 50, float val2 = 50, bool inGameJoystick = false) { }

        public virtual void InstantInteraction(bool value) { }

        public virtual void VectorInteraction(Vector2 LeftJoystickVal, Vector2 RightJoystickVal) { }

        public virtual void TriggerInteraction(float LeftTrigger, float RightTrigger) { }

        public virtual void TriggerButtonInteraction(bool LeftTrigger, bool RightTrigger) { }

        public virtual void XkeyInteraction(bool value) { }
        
        protected virtual void Update() { }

        

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { StopAllCoroutines(); }

    }
}

