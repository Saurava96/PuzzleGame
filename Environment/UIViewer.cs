using DarkDemon;
using LazyPanda;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.UI;

namespace DarkDemon
{
    [RequireComponent(typeof(BoxCollider))]
    public class UIViewer : MonoBehaviour
    {
        public List<EnvironmentGeneral> UIs;
        [SerializeField] float TimeGap = 1;
        

        GameController GController;
        public bool MainPlayerDetectCheckpoint = false;
        public bool ControlledAIDetectCheckpoint = true;

        public bool AllowTriggerExecution = true;

        private bool UIListShowRoutineRunning = false;
        private bool UIShowRoutineRunning = false;

        Collider ColliderInTrigger = null;

        private void Start()
        {
            GController = Instancer.GameControllerInstance;
            GetComponent<Collider>().isTrigger = true;
        }

        public void UpdateButtonFunctionality(ChestParts.Parts part,ButtonGeneral.LetterThiefButtonState newstate, string text)
        {
            UIs ??= new List<EnvironmentGeneral>();

            EnvironmentGeneral item = GetItemFromPartEnum(part);

            if (item.TryGetComponent<ButtonGeneral>(out var b)) { b.State = newstate; }
           
            ButtonManagerBasic button = item.GetComponent<ButtonManagerBasic>();
            

            button.normalText.text = text;

            UIs.Add(item);

        }


        private EnvironmentGeneral GetItemFromPartEnum(ChestParts.Parts part)
        {
            ChestParts[] parts = GetComponentsInChildren<ChestParts>();

            for(int i = 0; i < parts.Length; i++)
            {
                if(part == parts[i].ThisPart)
                {
                    return parts[i].GetComponent<EnvironmentGeneral>(); 
                }
            }

            return null;
           

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!AllowTriggerExecution) return;

            if (MainPlayerDetectCheckpoint)
            {
                if (GController.IsPlayer(other)) { ColliderInTrigger = other; TriggerEntered();  }
            }

            if (ControlledAIDetectCheckpoint)
            {
                if (GController.IsControlledAI(other))
                {
                    ColliderInTrigger = other;
                    TriggerEntered();  
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!AllowTriggerExecution) return;

            if (other == ColliderInTrigger)
            {
                TriggerExit();
                
            }
        }

        private void TriggerEntered()
        {
            StartCoroutine(UIShow(UIs, true));
        }

        private void TriggerExit()
        {
            StartCoroutine(UIShow(UIs, false));
        }

        public void UIToggleImmediate(bool value)
        {
            for (int i = 0; i < UIs.Count; i++)
            {
                UIs[i].InteractionEntered(value);

            }
        }

        
        public IEnumerator UIShow(List<EnvironmentGeneral> UIs, bool value)
        {
            if (UIListShowRoutineRunning) yield break;

            //makes sure that it runs only once.
            UIListShowRoutineRunning = true;

            if (!value) { Debug.Log("Disabling in routine"); }

            for (int i = 0; i < UIs.Count; i++)
            {
                yield return StartCoroutine(UIs[i].InteractionEnteredIE(value));
                
            }

            UIListShowRoutineRunning = false;
            
        }

        public IEnumerator UIShow(EnvironmentGeneral UI, bool value, float timegap)
        {
            if (UIShowRoutineRunning) yield break;

            //makes sure that it runs only once.
            UIShowRoutineRunning = true;

            yield return new WaitForSeconds(timegap);

            UI.InteractionEntered(value);

            UIShowRoutineRunning = false;

        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

    }
}

