using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class IdleBehaviour : Behaviours
    {
        public override PeopleBehavioursEnum GetStateType()
        {
            return PeopleBehavioursEnum.Idle;
        }

        public override void OnStateEnter()
        {
            People.Agent.enabled = false;

            People.Animator.SetInteger(SpeedHash, 0);

            

            Debug.Log("working");
        }

    }

}
