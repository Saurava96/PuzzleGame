using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RootMotion.Dynamics;
using RootMotion.FinalIK;


namespace DarkDemon
{
    public enum PeopleBehavioursEnum
    {
        None, Idle, Patrol, Dead, Talk, Options, CatchMe, CrossBridge
    };

    public enum HumanoidType { None, AI,ControlledAI}

    public abstract class HumansAI : MonoBehaviour
    {
        [SerializeField] protected HumanoidType CurrentHumanoidType = HumanoidType.None;
        [SerializeField] protected PeopleBehavioursEnum currentBehaviour = PeopleBehavioursEnum.None;


        //Bones
        [SerializeField] Transform rootBone;
        [SerializeField] BehaviourPuppet BehaviourPuppet;
        [SerializeField] BehaviourFall BehaviourFall;
        [SerializeField] Transform HumanoidParent;

        [Header("Talk Behaviour")]
        public bool AllowTalkBehaviour = false;
        public bool AllowPlayerTrigger = false;
        public bool AllowControlledAITrigger = false;
        [SerializeField] DialogueQueue Dialogue;
       

        protected GameObject Player;

        public LookAtIK LookAt { get; set; } = null;

        protected Dictionary<PeopleBehavioursEnum, Behaviours> behaviourDictionary = new Dictionary<PeopleBehavioursEnum, Behaviours>();

        protected Behaviours currentMainBehaviour = null;

        public Animator Animator { get; private set; }
        public NavMeshAgent Agent { get; private set; }

        public PuppetMaster Puppet { get; private set; }

        public Transform GetRootBone { get { return rootBone; } }

        public Rigidbody RigidRootBone { get; private set; }

        

        public BehaviourPuppet GetBehaviourPuppet { get { return BehaviourPuppet; } }

        public BehaviourFall GetBehaviourFall { get { return BehaviourFall; } }

        public Transform GetHumanoidParent { get { return HumanoidParent; } }

        public PeopleBehavioursEnum SetCurrentBehaviour { set { currentBehaviour = value; } }
        public PeopleBehavioursEnum GetCurrentBehaviour { get { return currentBehaviour; } }

        //Getters
        public Behaviours GetCurrentMainBehaviour { get { return currentMainBehaviour; } }

        public HumanoidType GetHumanoidType { get { return CurrentHumanoidType; } }

        public Transform TargetTolook { get; set; }


        public float TotalMass { get; private set; } = 0;

        public Dictionary<PeopleBehavioursEnum, Behaviours> GetBehaviourDic { get { return behaviourDictionary; } }

        public DialogueQueue DialogueBox { get { return Dialogue; } }

        public MeshRenderer CharacterMesh { get; set; }

        public DarkDemon.Outline MeshOutline { get; set; }

        protected virtual void Awake()
        {
            Behaviours[] states = GetComponents<Behaviours>();

            for (int i = 0; i < states.Length; i++)
            {
                if (states[i] != null && !behaviourDictionary.ContainsKey(states[i].GetStateType())) //making sure that if the state is not null
                                                                                                     // and the state is already not in the dict, then adding the new state to the dictionary. 
                {
                    behaviourDictionary[states[i].GetStateType()] = states[i]; //adding the state to the dict here.. The key is the currentbehaviour
                                                                               //and the value is the state itself.
                }
            }

            currentMainBehaviour = behaviourDictionary[currentBehaviour];

            currentMainBehaviour.setPeople = this;

            Animator = GetComponentInChildren<Animator>();

            Agent = GetComponent<NavMeshAgent>();

            Puppet = rootBone.GetComponentInParent<PuppetMaster>();

            RigidRootBone = rootBone.GetComponent<Rigidbody>();

            LookAt = GetComponent<LookAtIK>();

            LookAt.solver.SetIKPositionWeight(0);

            if (Dialogue) { Dialogue.gameObject.SetActive(false); }


            Initializer();

        }

        protected virtual void Start()
        {
            //  Instancer instancer = Instancer.Instance;

            //  instancer.RegisterAIPeople(transform.GetInstanceID(), this);

            Collider[] colliders = Puppet.GetComponentsInChildren<Collider>();
            

            for(int i = 0; i < colliders.Length; i++)
            {
                Instancer.GameControllerInstance.AddPeopleToDic(colliders[i], this);
            }

            Rigidbody[] BodyParts = rootBone.GetComponentsInChildren<Rigidbody>();

            for (int i = 0; i < BodyParts.Length; i++)
            {
                if (BodyParts[i] != null)
                {
                    TotalMass += BodyParts[i].mass;

                     //registering bones with this statemachine,//so we can know that these bones belong to which NPC

                    //BodyParts[i].gameObject.layer = BodyPartLayerNum;
                    //BodyParts[i].gameObject.AddComponent<BodyPartCollision>();
                }
            }

            if (GetComponent<Rigidbody>() != null)
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }

            if (GetComponent<CapsuleCollider>() != null)
            {
                GetComponent<CapsuleCollider>().isTrigger = true;
            }

            currentMainBehaviour.OnStateEnter();

        }

        protected virtual void Update()
        {
            if (currentMainBehaviour == null) { return; };

            PeopleBehavioursEnum newType = currentMainBehaviour.GetStateType(); //checking if the state has changed. for ex: from walking to running

            if (currentBehaviour != newType) //if new type has been detected, then..
            {
                currentMainBehaviour.OnStateExit();
                currentMainBehaviour = behaviourDictionary[currentBehaviour]; //setting the new behaviour here 
                currentMainBehaviour.setPeople = this;
                currentMainBehaviour.OnStateEnter();//calling onstate enter of particulat behaviuor. Each state can override this and OnStateEnter() can be 
                                                    //called when the state first runs or when the state moves from one state to another state
                newType = currentBehaviour;//makes sure currentbehaviuorenum gets updated
            }

            if (currentMainBehaviour != null)
            {

                currentMainBehaviour.UpdatingPositionAndAngle();//this makes sure to run so that NPC goes accoring to the 

                currentMainBehaviour.BehaviourUpdate();

            }



        }

        public void OnAnimatorMove()
        {

            if (currentMainBehaviour != null)
            {
                currentMainBehaviour.OnAnimatorUpdated();
            }

        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (currentMainBehaviour == null) return;

            currentMainBehaviour.OnTriggerEntered(other);

            
        }

        private void OnTriggerStay(Collider other)
        {
            if (currentMainBehaviour == null) return;

            currentMainBehaviour.OnTriggerStayed();
        }


        private void OnTriggerExit(Collider other)
        {
            if (currentMainBehaviour == null) return;

            currentMainBehaviour.OnTriggerExited(other);

           

        }


        

        
       
        private void Initializer()
        {

            HumanoidBodyParts[] bodyParts = Puppet.GetComponentsInChildren<HumanoidBodyParts>();

            for(int i = 0; i < bodyParts.Length; i++)
            {
                switch (bodyParts[i].ThisPart)
                {
                    case HumanoidBodyParts.HumanoidBodyPartsEnum.Head:
                        
                        Instancer.GameControllerInstance.AddPeopleHeadToDic(bodyParts[i].GetComponent<Collider>(), this);

                        break;

                    case HumanoidBodyParts.HumanoidBodyPartsEnum.Mesh:
                        
                        CharacterMesh = bodyParts[i].GetComponent<MeshRenderer>();
                        MeshOutline = bodyParts[i].GetComponent<DarkDemon.Outline>();

                        break;

                    
                }
            }

        }

        

    }
}


