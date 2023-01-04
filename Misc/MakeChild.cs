using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;

namespace DarkDemon
{
    [RequireComponent(typeof(BoxCollider))]
    public class MakeChild : MonoBehaviour
    {
        BoxCollider Trigger;

        [SerializeField] bool OnlyMainPlayer = false;

        public bool PlayerOnTile { get; set; } = false;

        public bool DummbOnTile { get; private set; } = false;

        private void Start()
        {
            Trigger = GetComponent<BoxCollider>();

            Trigger.isTrigger = true;

        }


        private void OnTriggerEnter(Collider other)
        {
            
                if (other.GetComponent<MainPlayerBodyParts>())
                {
                    if (other.GetComponent<MainPlayerBodyParts>().ThisPart == MainPlayerBodyParts.BodyPart.HeadInventory)
                    {
                        Instancer.GameControllerInstance.GetPlayer.transform.parent = transform;

                        Debug.Log("Main player child");

                        PlayerOnTile = true;

                    }

                }



            if (Instancer.GetHumanFromHeadCollider(other))
            {
                VR3DCharacterBase character = Instancer.GetHumanFromHeadCollider(other);

                character.transform.parent = transform;

                DummbOnTile = true;

                Debug.Log("Character child");

            }

            

            if (Instancer.GetControlledAIFromHeadCollider(other))
            {
                ControlledAI ai = Instancer.GetControlledAIFromHeadCollider(other);

                ai.transform.parent = transform;

                
            }

        }

        private void OnTriggerExit(Collider other)
        {
            
            
                if (other.GetComponent<MainPlayerBodyParts>())
                {
                    if (other.GetComponent<MainPlayerBodyParts>().ThisPart == MainPlayerBodyParts.BodyPart.HeadInventory)
                    {
                        PlayerOutOfTile();
                    }


                }

                
            if (Instancer.GetHumanFromHeadCollider(other))
            {
                DummbOutOfTile();
            }

            

            if (Instancer.GetControlledAIFromHeadCollider(other))
            {
                ControlledAI ai = Instancer.GetControlledAIFromHeadCollider(other);

                ai.transform.parent = null;

            }
        }

        public void PlayerOutOfTile()
        {
            Instancer.GetPlayer.transform.parent = null;

            Debug.Log("player parent null");

            PlayerOnTile = false;
        }

        public void DummbOutOfTile()
        {
            VR3DCharacterBase character = Instancer.GetPlayer3D;

            character.transform.parent = null;

            Debug.Log("Character parent null");

            DummbOnTile = false;
        }

    }
}

