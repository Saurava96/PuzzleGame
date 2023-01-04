using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPanda
{
    public class VR3DJumpDetector : MonoBehaviour
    {
        Joint CurrentJoint;
        private bool HangingHere = false;
        IEnumerator AllowHanging;
        private VR3DCharacterBase CurrentHangingPlayer;

        EnvironmentBase EnvironmentComponent;

        private void Start()
        {
            CurrentJoint = GetComponent<Joint>();       //gets the joint
            
        }

        private void OnTriggerEnter(Collider other)
        {
            //if the other collider is a character, then..
            if (other.GetComponent<VR3DBodyParts>())
            {
                

                VR3DBodyParts part = other.GetComponent<VR3DBodyParts>();

                

                //if the left hand collided or the right hand collided with this trigger, then....
                if (part.ThisPart == VR3DBodyParts.RigidBodyParts.LeftHand || part.ThisPart == VR3DBodyParts.RigidBodyParts.RightHand)
                {
                    
                    //if the collided part of the body has a rigidbody, basically it means 
                    //if they can be ragdolled, then....
                    if (part.GetComponent<Rigidbody>())
                    {
                        //Hangs the character at this detector...
                        MakeCharacterHang(other, part);
                        


                    }
                }

            }
        }
        /// <summary>
        /// Hangs the character at this point by making them completely ragdolled.
        /// One of the character's hand is joint to this point by a fixed joint. The joint cannot be 
        /// broken currently..
        /// </summary>
        /// <param name="other"></param>
        /// <param name="part"></param>
        private void MakeCharacterHang(Collider other,VR3DBodyParts part)
        {
            //gets the human from the collider (collider which collided with this point.)
            VR3DCharacterBase player = Instancer.GetHumanFromCollider(other);

            //If not human, then we return..
            if (!player) { Debug.LogError("human base not found"); return; }

            //If something in hand, then do not hang the player to this jump detector;
            //if (player.ObjectInHand != null) return;

            //checking to make sure if someone is hanging there already..
            //If currenthangingplayer is null, no one is hanging so can try to hang..
            if (CurrentHangingPlayer != null)
            {
                //If someone is hanging there which is not the current character, so do not allow currentplayer to hang...
                if (CurrentHangingPlayer.IsHanging)
                {
                    //and the current hanging player is not the player trying to hang, then we
                    //make sure that this player cannot replace the current hanging player. So we return...
                    if(CurrentHangingPlayer != player) { Debug.Log("Differnt"); return; }


                }
                

            }
           

            //if the current player is hanging and collider touches again, return;
            if (player.IsHanging) return;

            //if no other character hanging, and this player not hanging as well, then proceed to start 
            //the hanging process..
            if (!HangingHere)
            {
                //actually hangs the player at this spot..
                player.HangingDeadSimulation();

                // player.LowPinDeadSimulation();

                //connects the hand to the joint of this point..
                CurrentJoint.connectedBody = part.GetComponent<Rigidbody>();

                //setting the connected joint variable so we can null it when the character actually 
                //stops hanging..
                player.ConnectedJoint = CurrentJoint;

                
                //is hanging at this point.
                HangingHere = true;

                //who is hanging at the point.
                CurrentHangingPlayer = player;

                
                
            }

            //it starts the process of allowing other player to hang because the currenthanging player 
            //has left..
            else
            {
                //making sure the coroutine does not run mltiple times.
                if(AllowHanging == null)
                {
                    AllowHanging = Hanging();
                    StartCoroutine(AllowHanging);
                }
            }

            
        }

        private IEnumerator Hanging()
        {
            

            //after 1.5s, this point will be available for hanging again after the previous character
            //has stopped hanging.
            yield return new WaitForSeconds(1.5f);

            HangingHere = false;

            AllowHanging = null;
        }

    }
}

