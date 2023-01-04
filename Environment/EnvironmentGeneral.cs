using LazyPanda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class EnvironmentGeneral : EnvironmentBase
    {
        public enum GeneralEnvironmentType {None, Toggler, UI}

        public GeneralEnvironmentType ThisType = GeneralEnvironmentType.Toggler;

        [Header("Toggler")]
        [SerializeField] GameObject ToEnable;

        [Header("UI")]
        CanvasGroup CanvasGroup;
        bool EnableRoutineRunning = false;
        bool DisableRoutineRunning = false;
        public override void InteractionEntered()
        {
            switch (ThisType)
            {
                case GeneralEnvironmentType.Toggler:
                    ToEnable.SetActive(true); break;

                


            }
        }

        public override IEnumerator InteractionEnteredIE(bool value)
        {
            switch (ThisType)
            {
                case GeneralEnvironmentType.UI:
                    if (value) { yield return StartCoroutine(EnableUISmooth()); }
                    else {yield return StartCoroutine(DisableUISmooth()); }

                    break;
            }
        }


        public override void InteractionEntered(bool value)
        {
            switch (ThisType)
            {
                case GeneralEnvironmentType.UI:
                    if (value) { CanvasGroup.alpha = 1; }
                    else { CanvasGroup.alpha = 0; }

                    break;
            }
        }

        private IEnumerator EnableUISmooth()
        {
            if (EnableRoutineRunning) yield break;

            EnableRoutineRunning = true;

            if(CanvasGroup == null) { CanvasGroup = GetComponentInChildren<CanvasGroup>(); }

            float val = CanvasGroup.alpha;
            float elapsedTime = 0;
            float waitTime = 1;

            while (elapsedTime < waitTime)
            {
                CanvasGroup.alpha = Mathf.Lerp(val, 1, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                // Yield here
                yield return null;
            }

            CanvasGroup.alpha = 1;
            CanvasGroup.interactable = true;
            EnableRoutineRunning = false;
        }



        private IEnumerator DisableUISmooth()
        {
            Debug.Log("Disalbing UI start");

            if (DisableRoutineRunning) yield break;

            DisableRoutineRunning = true;

            if (CanvasGroup == null) { CanvasGroup = GetComponentInChildren<CanvasGroup>(); }

            float val = CanvasGroup.alpha;
            float elapsedTime = 0;
            float waitTime = 1;
            CanvasGroup.interactable = false;
            while (elapsedTime < waitTime)
            {
                CanvasGroup.alpha = Mathf.Lerp(val, 0, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                // Yield here
                yield return null;
            }

            CanvasGroup.alpha = 0;
            Debug.Log("disabling ui");
            DisableRoutineRunning = false;

        }

        


    }
}

