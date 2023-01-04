using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DarkDemon
{
    public class OptionsBehaviour : Behaviours
    {
       [SerializeField] UIViewer UIControl;

        public override PeopleBehavioursEnum GetStateType()
        {
            return PeopleBehavioursEnum.Options;
            
        }

        public override void OnStateEnter()
        {
            //idle animation..

            UIControl.UIToggleImmediate(true);
        }

        public override void OnStateExit()
        {
            UIControl.UIToggleImmediate(false);
        }

        public override void OnTriggerExited(Collider other)
        {
            AITriggerExited(other); 
        }

        
    }
}

