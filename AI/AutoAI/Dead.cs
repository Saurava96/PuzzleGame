using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class Dead : Behaviours
    {
        public override PeopleBehavioursEnum GetStateType()
        {
            return PeopleBehavioursEnum.Dead;
        }

        public override void OnStateEnter()
        {
            People.Agent.enabled = false;
            People.Puppet.state = RootMotion.Dynamics.PuppetMaster.State.Dead;
           
        }
    }
}

