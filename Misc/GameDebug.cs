using BNG;

using LazyPanda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DarkDemon
{
    public class GameDebug : MonoBehaviour
    {
        [SerializeField] GameObject MainPlayer;
        [SerializeField] FirstPersonMovement PlayerDebug;


        GameController GController;
        GameLevelController LevelController;

        bool DebugOn = false;

        public EnvironmentBase CurrentEnvironmentObject { get; set; }
        public Button CurrentActiveButton { get; set; }

        public Lever CurrentActiveLever { get; set; }

        Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f); // center of the screen
        float rayLength = 500f;

        private void Start()
        {
            GController = Instancer.GameControllerInstance;
            LevelController = GController.GetLevelController;

            if (!GController.DebugMode) return;

            MainPlayer.SetActive(false);
            PlayerDebug.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;

        }

        private void Update()
        {
            if (!GController) return;

            DebugOn = GController.DebugMode;

            if (!DebugOn) return;

            if (!Camera.main) return;

            // actual Ray
            Ray ray = Camera.main.ViewportPointToRay(rayOrigin);

            // debug Ray
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayLength))
            {
                if (hit.transform != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        CurrentClickedEnvironmentBase(hit.transform.gameObject);
                        CurrentClickedHandButton(hit.transform.gameObject);
                        CurrentClickedLever(hit.transform.gameObject);
                    }
                    
                }
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                PlayerDebug.GodMode();
            }


        }

        private void CurrentClickedHandButton(GameObject g)
        {
            Button b = g.GetComponentInParent<Button>();

            if(b != null)
            {
                AllNull();
                CurrentActiveButton = b;
                Debug.Log("Button Clicked");
                return;
            }

            CurrentActiveButton = null;
        }

        private void CurrentClickedEnvironmentBase(GameObject g)
        {
            EnvironmentBase environment = g.GetComponentInParent<EnvironmentBase>();

            Debug.Log("Current Clicked Call: " + g.name);

            if (environment != null)
            {
                AllNull();
                CurrentEnvironmentObject = environment;
                
                Debug.Log("object getting set.");
                return;
            }

            CurrentEnvironmentObject = null;

        }

        private void CurrentClickedLever(GameObject g)
        {
            Lever b = g.GetComponentInParent<Lever>();
            if(b == null)
            {
                b = g.GetComponentInChildren<Lever>();
            }

            if (b != null)
            {
                AllNull();
                CurrentActiveLever = b;
                Debug.Log("Lever Clicked");
                return;
            }

            CurrentActiveLever = null;
        }

        private void AllNull()
        {
            CurrentEnvironmentObject = null;
            CurrentActiveButton = null;
            CurrentActiveLever = null;
        }

    }




    }


