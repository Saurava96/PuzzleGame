using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DarkDemon
{
    public class CrossTheBridge : TheLevel
    {
        [SerializeField] GameObject CharSpeed1Pre;
        [SerializeField] GameObject CharSpeed2Pre;
        [SerializeField] GameObject CharSpeed5Pre;
        [SerializeField] GameObject CharSpeed10Pre;

        public VehicleTransport Vehicle;

        public Transform Speed1PointR1 { get; private set; }
        public Transform Speed2PointR1 { get; private set; }
        public Transform Speed5PointR1 { get; private set; }
        public Transform Speed10PointR1 { get; private set; }

        public Transform Speed1PointR2 { get; private set; }
        public Transform Speed2PointR2 { get; private set; }
        public Transform Speed5PointR2 { get; private set; }
        public Transform Speed10PointR2 { get; private set; }

        public Humanoid CharSpeed1 { get; set; }
        public Humanoid CharSpeed2 { get; set; }
        public Humanoid CharSpeed5 { get; set; }
        public Humanoid CharSpeed10 { get; set; }

        public HumansAI CurrentHighlightedAI { get; set; }

        public override LevelsEnum GetStateType()
        {
            return LevelsEnum.CrossTheBridge;
        }


        protected override void VariableReference()
        {
            StartCoroutine(VRPlayer.DisablePlayerMovement(true));

            for (int i = 0; i < LevelInstantiatedObjects.Count; i++)
            {
                GameObject game = LevelInstantiatedObjects[i];

                CrossTheBridgeParts[] parts = game.GetComponentsInChildren<CrossTheBridgeParts>();

                for (int k = 0; k < parts.Length; k++)
                {
                    CrossTheBridgeParts part = parts[k];

                    if (part)
                    {
                        switch (part.ThisType)
                        {
                            case CrossTheBridgeParts.Type.Speed1PointR1:
                                Speed1PointR1 = part.transform; break;

                            case CrossTheBridgeParts.Type.Speed2PointR1:
                                Speed2PointR1 = part.transform; break;

                            case CrossTheBridgeParts.Type.Speed5PointR1:
                                Speed5PointR1 = part.transform;   break;

                            case CrossTheBridgeParts.Type.Speed10PointR1:
                                Speed10PointR1 = part.transform; break;

                            case CrossTheBridgeParts.Type.Speed1PointR2:
                                Speed1PointR2 = part.transform; break;

                            case CrossTheBridgeParts.Type.Speed2PointR2:
                                Speed2PointR2 = part.transform; break;

                            case CrossTheBridgeParts.Type.Speed5PointR2:
                                Speed5PointR2 = part.transform; break;

                            case CrossTheBridgeParts.Type.Speed10PointR2:
                                Speed10PointR2 = part.transform; break;
                        }
                    }

                    
                }

            }
        }

        protected override void LevelStart()
        {
            //Set raycast length and layer.
            
        }

        public void SelectCharacter(LineRenderer lineRenderer, RaycastHit hit)
        {
            if(hit.collider!= null)
            {
                HumansAI ai = Controller.GetPeopleFromCollider(hit.collider);
                
                lineRenderer.enabled = ai != null;
                
                if (ai != null)
                {
                    if(CurrentHighlightedAI != null)
                    {
                        if (ai != CurrentHighlightedAI)
                        {
                            CurrentHighlightedAI.MeshOutline.enabled = false;
                            CurrentHighlightedAI = ai;
                            CurrentHighlightedAI.MeshOutline.enabled = true;

                        }
                    }
                    else
                    {
                        CurrentHighlightedAI = ai;
                        CurrentHighlightedAI.MeshOutline.enabled = true;
                    }

                }
            }
            
        }


        protected override void ControlledAIInstantiate()
        {
            GameObject c1 = Instantiate(CharSpeed1Pre,Speed1PointR1.position, Speed1PointR1.rotation);
            CharSpeed1 = c1.GetComponentInChildren<Humanoid>();
           

            GameObject c2 = Instantiate(CharSpeed2Pre,Speed2PointR1.position,Speed2PointR1.rotation);
            CharSpeed2 = c2.GetComponentInChildren<Humanoid>();
            

            GameObject c5 = Instantiate(CharSpeed5Pre,Speed5PointR1.position,Speed5PointR1.rotation);
            CharSpeed5 = c5.GetComponentInChildren<Humanoid>();
           

            GameObject c10 = Instantiate(CharSpeed10Pre,Speed10PointR1.position,Speed10PointR1.rotation);
            CharSpeed10 = c10.GetComponentInChildren<Humanoid>();
            
        }


        public override void SetLevelMusic()
        {
            
        }

        public override void SetWeather()
        {

           
        }
    }
}

