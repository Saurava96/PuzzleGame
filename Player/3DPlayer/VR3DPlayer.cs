using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;
using RootMotion.FinalIK;
using RootMotion.Demos;
using RootMotion.Dynamics;


/// <summary>
/// VR3DPlayer is the 
/// </summary>
public class VR3DPlayer : VR3DCharacterBase
{
    [SerializeField] SharedBool PlayerInControl;
    [SerializeField] SharedBool LookToChangePlayer;
    private const int UpperLayerIndex = 2;

  
    
    [Header("Footsteps")]
    
    
    [SerializeField]
    private float footstepInterval;
    [SerializeField]
    private Vector3 footOffset;
    private int foot = 1;
    private Vector3 previousPosition;
    private float distanceTravelled;
    private bool isSneaking;
    private bool isGrounded;

    const float DefaultResistance = 500;
    const float ObjectHoldResistance = 1000;

    
    public bool IsLookToChangePlayer { get { return LookToChangePlayer.Value; } }


    

    protected override void Awake()
    {
        base.Awake();
        if (transform.parent) { transform.parent = null; }
    }


    /// <summary>
    /// Enable or Disable behaviours....
    /// </summary>
    /// <param name="value"></param>
    public override void EnableBehaviors(bool value)
    {
        for( int i = 0; i < Behaviours.Length; i++)
        {
            Behaviours[i].enabled = false;
        }
    }

    


    /// <summary>
    /// Makes the character jump while hanging.. Uses velocity..
    /// </summary>
    public override void JumpWhileHanging()
    {
        ConnectedJoint.connectedBody = null;

        Rigidbody hip = HipBone.GetComponent<Rigidbody>();

        hip.velocity = 45 * Vector3.up;

        IsHanging = false;

    }

    /// <summary>
    /// Makes the player in control of the player. The player can now control this character..
    /// </summary>
    /// 
    /*
    public override void ToInControlState(bool show3dviewer)
    {
        GameController controller = Instancer.GameControllerInstance;

        //if false, character cannot be controlled even if B is pressed. 
        if (!AllowToBeControlled) return;

        if (CurrentState == CharacterState.InControl) return;


        //if other character is in control, then make that charcater to idle state because the player
        //can control only one at a time..
        if (Instancer.CharacterInControl)
        {
            Instancer.CharacterInControl.ToIdleState();
        }


        //updating the enum because this character is in control now...
        CurrentState = CharacterState.InControl;
        
        //Updating the character to let instancer know that this character is in control..
        Instancer.CharacterInControl = this;

       // StartCoroutine(DisablePlayerMovement(!show3dviewer));

        //Since character is in control, it means the player is not looking anymore
        Player.NotLooking();

        if (!IsAlive && !IsSuctioned)
        {
            Alive();
        }

        if (!IsSuctioned) { AliveFromLowPin(); }
       

        VR3DCharacterBase character = controller.Get3DPlayer;
        Transform characterController = character.GetCharacterController;
        UserControlThirdPerson userControl = characterController.GetComponent<UserControlThirdPerson>();

        if (show3dviewer) 
        { 
            controller.GetEnvironmentEffects.Show3DViewer();

            Camera cam = character.GetcharacterCamera;
            cam.enabled = true;

            
            if (!controller.DebugMode)
            {
                if (!cam.targetTexture) { cam.targetTexture = controller.GetRenderTexture; }
            }
            
            userControl.cam = cam.transform;

        }
        else
        {
            userControl.cam = controller.GetPlayer.GetHead;
        }

        Debug.Log("Character in control");
        
        
    }

    */
    

    /// <summary>
    /// MOves the chracter to idle state..
    /// </summary>
    public override void ToIdleState()
    {
        CurrentState = CharacterState.Idle;
        
    }

    /// <summary>
    /// Decides how to interact with the object when near the object or when holding something in hand...
    /// </summary>
    public override void InteractionDecider()
    {
        /*

        if (ObjectInHand)
        {
            DropObject();
            return;
        }

        if (!CharacterTriggers) { Debug.LogError("No character trigger found"); return; }

        if (!CharacterTriggers.IsInteractableAround) return;

        */

        /*

        switch (CharacterTriggers.GetCurrentInteractionType)
        {
            case InteractionBase.InteractionTypes.ButtonPress:

                break;

            case InteractionBase.InteractionTypes.Door:

                break;

            case InteractionBase.InteractionTypes.ObjectPickUpDualHand:

                ObjectPickUp(CharacterTriggers.GetInteractableObject);

                break;

            case InteractionBase.InteractionTypes.None:

                break;
        }

        */

    }

   

    private void OutOfBoundsInquiry()
    {
        if (!Controller) return;

        if (!CharacterController) return;

        if (CharacterController.transform.position.y <= Controller.MinElevation || CharacterController.transform.position.y >= Controller.MaxElevation)
        {
            Debug.Log("out of bounds");
            DummbOutOfBounds = true;
            return;
        }

        DummbOutOfBounds = false;


    }

    protected virtual void FixedUpdate()
    {
        UpdateGrounded();

        OutOfBoundsInquiry();
    }

    protected virtual void LateUpdate()
    {
        Vector3 currentPosition = transform.position;
        float distanceTravelledThisFrame = Vector3.Distance(previousPosition, currentPosition);

        distanceTravelled += distanceTravelledThisFrame;

        if (isGrounded && distanceTravelled > footstepInterval)
        {
            distanceTravelled = 0;
            foot = -foot;

          //  TriggerFootstep();
        }

        previousPosition = currentPosition;
    }

    /*
    private void TriggerFootstep()
    {
        //Velocity is a 0-1 value used to scale the volume of the footstep sounds
        float velocity = isSneaking ? 0.25f : 1;

        RaycastHit hit;
        Ray r = new Ray(transform.position + transform.rotation * footOffset, Vector3.down);
        if (Physics.Raycast(r, out hit))
        {
            InteractionData data = new InteractionData()
            {
                Velocity = Vector3.down * velocity,
                CompositionValue = 1,
                PriorityOverride = 100,
                ThisObject = this.gameObject
            };

            if (foot > 0)
                data.TagMask = footstepRightTag.GetTagMask();
            else if (foot < 0)
                data.TagMask = footstepLeftTag.GetTagMask();

            ImpactRaycastTrigger.Trigger(data, hit, true);
        }
    }

    */
    private void UpdateGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, 2, 1 << 0);


    }

    /*
    /// <summary>
    /// The character will pick up the interactable object in front of it.
    /// </summary>
    /// <param name="interactable"></param>
    protected override void ObjectPickUp(InteractionBase interactable)
    {
        if (!interactable) { Debug.LogError("no interactable object"); }
        if (!AnimatorController) { Debug.LogError("No AnimatorController"); return; ; }
        if (!AnimationThirdPerson) { Debug.LogError("No third person animation");return; }

        InteractionObject interaction = interactable.GetComponent<InteractionObject>();

        if (!interaction) { Debug.LogError("No interaction found"); return; }

        

        if (Vector3.Angle(interactable.transform.forward, MeshMain.transform.forward) > 90 && Vector3.Dot(interactable.transform.forward, MeshMain.transform.forward) < 0)
        {
            interactable.transform.forward = -interactable.transform.forward;
        }

      

        CharacterPickDualHand.SetPickUpTime(0.1f);
        CharacterPickDualHand.SetCurrentIO(interaction);

        CharacterPickDualHand.PickUpObject(interaction);

        ObjectInHand = interactable;

        AnimatorController.SetTrigger(PickUpHash);
        AnimationThirdPerson.AnimationLayerIndex = UpperLayerIndex;

        Weight weight = new Weight(ObjectHoldResistance);

        if (PuppetCollisionFall)
        {
            PuppetCollisionFall.collisionResistance = weight;
        }
        
        interactable.InteractionBegin();
    }

    */
    /// <summary>
    /// The character will drop a picked object from the hand...
    /// </summary>
    /// 

    /*
    protected override void DropObject()
    {
        if (!CharacterTriggers) return;

        if (!ObjectInHand) return;

        CharacterPickDualHand.LetGo();

        AnimatorController.SetTrigger(DropHash);
        AnimationThirdPerson.AnimationLayerIndex = 0;

        ObjectInHand.InteractionEnd();

        ObjectInHand = null;

        Weight weight = new Weight(DefaultResistance);

        if (PuppetCollisionFall)
        {
            PuppetCollisionFall.collisionResistance = weight;
        }


    }

    */
    
    
}
