using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class TalkBehaviour : Behaviours
    {
        public bool AutoStartTalk = true;
        [SerializeField] bool ShowOptionsAfterTalk = false;

        private bool Talking = false;

        public override PeopleBehavioursEnum GetStateType()
        {
            return PeopleBehavioursEnum.Talk;
        }

        public override void OnStateEnter()
        {
            //play talk animation.

            if (!People.DialogueBox) 
            {
                Debug.LogError("NO dialogue box attached");
                
                People.SetCurrentBehaviour = PeopleBehavioursEnum.Idle;
                    
                return; 
            }

            if (AutoStartTalk) { StartCoroutine(Talk()); }

        }

        public override void OnStateExit()
        {
            People.DialogueBox.GetViewer().UIToggleImmediate(false);
            
        }

        public override void OnTriggerExited(Collider other)
        {
            if(People.AllowControlledAITrigger || People.AllowPlayerTrigger)
            {
                StopCoroutine(Talk());
            }

            AITriggerExited(other); 

        }

        public IEnumerator Talk(float time)
        {
            People.DialogueBox.GetViewer().UIToggleImmediate(true);
            People.DialogueBox.Speak();
            
            yield return new WaitForSeconds(time);

            People.DialogueBox.GetViewer().UIToggleImmediate(false);
        }

        private IEnumerator Talk()
        {
            if (Talking) yield break;

            Talking = true;

            People.DialogueBox.GetViewer().UIToggleImmediate(true);
            People.DialogueBox.Speak();

            yield return new WaitForSeconds(3);

            Talking = false;

            if (ShowOptionsAfterTalk)
            {
                People.SetCurrentBehaviour = PeopleBehavioursEnum.Options;
            }
            else
            {
                People.SetCurrentBehaviour = PeopleBehavioursEnum.Idle;
            }
            
        }



    }
}

