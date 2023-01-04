using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;
using UnityEngine.InputSystem.XR;
using UnityEditor.Experimental.GraphView;
using BNG;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;
using Newtonsoft.Json.Serialization;
using UnityEngine.InputSystem.HID;

namespace DarkDemon
{
    public class TheWeb : TheLevel
    {
       
        [Header("Level Specific")]
        
        public GameObject RunnerPrefab;

        public GameObject CatcherPrefab;
        
        [SerializeField] Transform CatchmePlayerPlatformPre;
        [SerializeField] Material SkyboxMaterial;
        
        Transform CatchmePlayerPlatform;


        CatchMeTile DummbStartingTile;
        CatchMeTile ToboStartingTile;
        Transform CatchMePlatformPos;
        Transform Wheel;

        public CatchMeBehaviour RunnerCurrentBehaviour { get; set; } = null;
        public CatchMeBehaviour CatcherCurrentBehaviour { get; set; } = null;
        
        public enum Turn { Catch, Runn }

        private Turn CurrentTurn = Turn.Catch;

        public bool LevelInitialized { get; private set; } = false;
        
        private bool IsLevelPassed = false;
        DarkDemon.Outline OutlineD;

        public CatchMeTile GetDummbStartingTile { get { return DummbStartingTile; } }
        public Transform GetCatchMePlayerplatform { get { return CatchmePlayerPlatform; } }
        public CatchMeTile GetToboStartingTile { get { return ToboStartingTile; } }

        public Humanoid Runner { get; set; }

        public Humanoid Catcher { get; set; }

        protected override void ControlledAIInstantiate()
        {
            GameObject c = Instantiate(CatcherPrefab);
            Catcher = c.GetComponentInChildren<Humanoid>();
            c.SetActive(false);

            GameObject r = Instantiate(RunnerPrefab);
            Runner = r.GetComponentInChildren<Humanoid>();
            r.SetActive(false);

        }

        protected override void VariableReference()
        {
            for (int i = 0; i < LevelInstantiatedObjects.Count; i++)
            {
                GameObject game = LevelInstantiatedObjects[i];

                TheWebParts[] parts = game.GetComponentsInChildren<TheWebParts>();

                for (int k = 0; k < parts.Length; k++)
                {
                    TheWebParts part = parts[k];

                    if (part)
                    {
                        switch (part.ThisPart)
                        {
                            case TheWebParts.Parts.CatcherStartingTile:
                                DummbStartingTile = part.GetComponent<CatchMeTile>();   break;

                            case TheWebParts.Parts.RunnerStartingTile:
                                ToboStartingTile = part.GetComponent<CatchMeTile>(); break;

                            case TheWebParts.Parts.PlayerPlatformPos:
                                CatchMePlatformPos = part.transform;
                                CatchmePlayerPlatform = Instantiate(CatchmePlayerPlatformPre);
                                CatchmePlayerPlatform.transform.position = CatchMePlatformPos.position;
                                break;

                            case TheWebParts.Parts.Wheel:
                                Wheel = part.transform;
                                EasyQuaternion.Instance.ConstantRotation(Wheel,2,AngleType.X);
                                break;
                            


                        }
                    }
                }

            }

            

        }

        protected override void LevelStart()
        {
            StartCoroutine(TheWebInitializer());

        }

        private IEnumerator TheWebInitializer()
        {


            //after some condition met, the game has started..

            //Initialization..

            GameLevelController GController = Instancer.GameControllerInstance.GetLevelController;
            UIPointer pointer = Instancer.GameControllerInstance.GetPlayer.Pointer;
            pointer.ShootRaycast = true;
            pointer.Length = 200;

            EnvironmentEffects effects = Instancer.GameControllerInstance.GetEnvironmentEffects;

            yield return StartCoroutine(effects.SmoothMakeEnvironmentDark());
            yield return StartCoroutine(VRPlayer.DisablePlayerMovement(true));
            //other objects disabled. We can do it after this level is combined.

            CatchmePlayerPlatform.gameObject.SetActive(true);

            CatchmePlayerPlatform.position = CatchMePlatformPos.position;

          

            Runner.GetHumanoidParent.gameObject.SetActive(true);
            
            Catcher.GetHumanoidParent.gameObject.SetActive(true);

            Catcher.SetCurrentBehaviour = PeopleBehavioursEnum.CatchMe;

            Runner.SetCurrentBehaviour = PeopleBehavioursEnum.CatchMe;

            yield return new WaitForEndOfFrame();

            RunnerCurrentBehaviour = (CatchMeBehaviour)Runner.GetCurrentMainBehaviour;

            CatcherCurrentBehaviour = (CatchMeBehaviour)Catcher.GetCurrentMainBehaviour;

            RunnerCurrentBehaviour.CurrentTile = ToboStartingTile;

            CatcherCurrentBehaviour.CurrentTile = DummbStartingTile;

            

            yield return StartCoroutine(Instancer.GameControllerInstance.MoveHumanoid(Catcher,DummbStartingTile.transform));

            yield return StartCoroutine(Instancer.GameControllerInstance.MoveHumanoid(Runner, ToboStartingTile.transform));

            Player.ControlDummb(false);

            Player.AllowControlShift = false;
            
            
            //SwitchTurn(CurrentTurn);

            RunnerCurrentBehaviour.TurnToMove = false;
            CatcherCurrentBehaviour.TurnToMove = true;

            

            yield return StartCoroutine(effects.SmoothMakeEnvironmentNormal());

            LevelInitialized = true;

        }

        public override void LevelUpdate()
        {
            if (!IsLevelEntered) return;
            if (!LevelInitialized) return;

        }

        public void SelectTile(LineRenderer lineRenderer, RaycastHit hit)
        {
            CatchMeTile tile = hit.transform.GetComponentInChildren<CatchMeTile>();

            lineRenderer.enabled = tile != null;
            DarkDemon.Outline outline = hit.transform.GetComponent<DarkDemon.Outline>();

            if (outline != null)
            {
                if (outline != OutlineD)
                {
                    if (OutlineD == null)
                    {
                        OutlineD = outline;
                    }
                    else
                    {
                        OutlineD.enabled = false;
                        OutlineD = outline;
                    }

                }
            }
            else
            {
                ToggleOutline(false);
            }

            if (tile)
            {

                if (Catcher.GetCurrentBehaviour != PeopleBehavioursEnum.CatchMe) return;


                if (!CatcherCurrentBehaviour.TurnToMove) return;

                CatchMeTile currentStandingTile = CatcherCurrentBehaviour.CurrentTile;
                CatchMeTile[] neighbours = currentStandingTile.Neighbours;



                if (InNeighbour(tile, neighbours))
                {
                    lineRenderer.useWorldSpace = false;
                    lineRenderer.SetPosition(0, Vector3.zero);
                    lineRenderer.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, //change the transform.
                        hit.transform.position) * 1));


                    ToggleOutline(true);

                    if (Player.SelectCatchMeTile())
                    {
                        CatcherCurrentBehaviour.MoveNowToSelectedTile(tile);
                        ToggleOutline(false);
                    }

                }
                else
                {
                    if (lineRenderer)
                    {
                        lineRenderer.enabled = false;

                    }
                }


            }


        }

        
        public void ToggleOutline(bool value)
        {
            if (OutlineD != null)
            {
                OutlineD.enabled = value;
                if (value == false) { OutlineD = null; }

            }
        }

        private bool InNeighbour(CatchMeTile tile, CatchMeTile[] neighbours)
        {
            for (int i = 0; i < neighbours.Length; i++)
            {
                if (neighbours[i] == tile)
                {
                    return true;
                }
            }

            return false;


        }

        public void SwitchTurn(Turn turn)
        {
            if (IsLevelPassed) return;

            switch (turn)
            {
                case Turn.Catch:

                    //Dummb.ToInControlState(false);
                    RunnerCurrentBehaviour.TurnToMove = false;
                    CatcherCurrentBehaviour.TurnToMove = true;
                    if (!ToboStartingTile.AllowTriggerDetection) { ToboStartingTile.AllowTriggerDetection = true; }
                    break;

                case Turn.Runn:

                    //Dummb.ToIdleState();
                    RunnerCurrentBehaviour.TurnToMove = true;
                    CatcherCurrentBehaviour.TurnToMove = false;
                    if (!DummbStartingTile.AllowTriggerDetection) { DummbStartingTile.AllowTriggerDetection = true; }

                    break;
            }
        }

        public IEnumerator LevelPassed()
        {
            IsLevelPassed= true;

            yield return new WaitForSeconds(1);

            Catcher.Animator.speed = 0.2f; //effect of slow motion on the catcher.
            //particle effects.
            //environment change..
            //UI show with scores and time taken.


            //Level passed effects and transition to another level.

        }

        public override LevelsEnum GetStateType()
        {
            return LevelsEnum.TheWeb;
        }

        

        public override IEnumerator OnLevelExit()
        {
            UIPointer pointer = Instancer.GameControllerInstance.GetPlayer.Pointer;
            pointer.ShootRaycast = false;
            pointer.Length = 0;

            VRPlayer.MainPlayerInControl();
            yield return StartCoroutine(base.OnLevelExit());
        }

        public override void SetWeather()
        {
            RenderSettings.skybox = SkyboxMaterial;
        }

        public override void SetLevelMusic()
        {
            
        }
    }
}

