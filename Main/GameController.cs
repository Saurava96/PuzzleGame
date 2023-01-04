using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkDemon;
using RootMotion.Dynamics;
using BNG;


namespace LazyPanda
{
    public class GameController : MonoBehaviour
    {


      //  public Vector3 IniScale { private set; get; }
      //  public Vector3 MenuIniScale { private set; get; }

       

        [SerializeField] VRMainPlayer Player;

        [Header("Environment")]
        [SerializeField] EnvironmentEffects Effects;
        [SerializeField] RenderTexture CamTexture;

        [Header("Devices")]

        [SerializeField] GameObject Screen3D;

        [SerializeField] LevelChangerTile LevelChangerPlatform;
        [SerializeField] DefaultTile DefaultPosTile;

        private static Dictionary<Collider, HumansAI> PeopleDic;

        private static Dictionary<Collider, HumansAI> PeopleHeadDic;
        


      

        [Header("Scripts")]
        [SerializeField] GameLevelController LevelController;

        [Header("Tobos")]
        //[SerializeField] Humanoid OrangeTobo;
        //[SerializeField] Humanoid GreenTobo;
        //[SerializeField] Humanoid BlueTobo;

       // [Header("Dummb Just AI")]
        //[SerializeField] Humanoid DummbJustAI;

       // [Header("MenuScreen")]
       // [SerializeField] GameMenu UIScreen;
      //  public float X = 90; //was 60
       // public float y = 180;//was 0
      //  public float z = 0; //was -90
        
       

        [Header("Variables")]
        
        [SerializeField] bool InstantPause = true;
        [SerializeField] LayerMask DefaultGroundLayers;

        [Header("Debug")]
        public bool DebugMode = false;
        [SerializeField] GameDebug Debugger;
        [SerializeField] MainPlayerBase PlayerDebug;



      //  public VR3DCharacterBase Get3DPlayer { get { return Dummb; } }

        public MainPlayerBase GetPlayer { get { if (DebugMode) { return PlayerDebug; } else { return Player; } } }

        public EnvironmentEffects GetEnvironmentEffects { get { return Effects; } }

        public GameObject GetScreen3D { get { return Screen3D; } }

        public RenderTexture GetRenderTexture { get { return CamTexture; } }

        

        public GameLevelController GetLevelController { get { return LevelController; } }

        public DefaultTile GetDefaultLevelChangerPos { get { return DefaultPosTile; } }

        public LevelChangerTile GetLevelChangerPlatform { get { return LevelChangerPlatform; } }

        public ControlledAI CurrentControlledAI { get; set; }

        public string GetFinalFileLocation(GameProfiles profile, int currentLevel)
        {
            string filename = GetFileName(currentLevel);

            return GetSaveDirectory(profile) + "/" + filename;
        }

        public string GetFinalFileLocationLS(int currentlevel)
        {
            string filename = GetFileName(currentlevel);

            return GetSaveDirectoryLS() + "/" + filename;
        }
        public string GetSaveDirectory(GameProfiles profile)
        {
            return Application.persistentDataPath + "/" + profile.ToString() + "Profile";
        }


        public string GetSaveDirectoryLS()
        {
            return Application.persistentDataPath + "/" + "LevelSelect" + "Profile";
        }

        public string LevelKey()
        {
            return "Level-";
        }

        public string GetFileName(int currentLevel)
        {
            return GetBaseString(currentLevel) + ".es3";
        }

        public string GetBaseString(int currentLevel)
        {
            return LevelKey() + currentLevel.ToString(); //Level-0
        }

        public int MaxCheckpointPerLevel { get { return 20; } }

        public int MaxGameLevel { get { return 30; } }

        public GameDebug GetDebugger { get { return Debugger; } }

        public bool IsPlayer(Collider other)
        {
            if (other.GetComponent<MainPlayerBodyParts>())
            {
                if (other.GetComponent<MainPlayerBodyParts>().ThisPart == MainPlayerBodyParts.BodyPart.Head)
                {
                    return true;
                }
            }

            return false;

        }

        public bool IsDummb(Collider other)
        {
            if (Instancer.GetHumanFromHeadCollider(other))
            {
                return true;
            }

            return false;
        }

        public bool IsControlledAI(Collider other)
        {
            ControlledAI ai = Instancer.GetControlledAIFromHeadCollider(other);

            if (ai != null)
            {

                if (ai == CurrentControlledAI)
                {

                    return true;
                }
            }

            return false;

        }

        public void AddPeopleToDic(Collider c, HumansAI ai)
        {
            if (PeopleDic == null)
            {
                PeopleDic = new Dictionary<Collider, HumansAI>();
            }

            PeopleDic.Add(c, ai);

        }



        public HumansAI GetPeopleFromCollider(Collider c)
        {
            if (PeopleDic.TryGetValue(c, out HumansAI val))
            {
                return val;
            }

            return null;
        }

        public void AddPeopleHeadToDic(Collider c, HumansAI ai)
        {
            if (PeopleHeadDic == null)
            {
                PeopleHeadDic = new Dictionary<Collider, HumansAI>();
            }

            PeopleHeadDic.Add(c, ai);
        }

        public HumansAI GetPeopleHeadFromCollider(Collider c)
        {
            if (PeopleHeadDic.TryGetValue(c, out HumansAI val))
            {
                return val;
            }

            return null;
        }

        public virtual IEnumerator MoveControlledAI(ControlledAI controlledAI, Transform finalPos)
        {

            PuppetMaster puppet = controlledAI.Puppet;

            puppet.state = RootMotion.Dynamics.PuppetMaster.State.Frozen;
            yield return new WaitForEndOfFrame();
            controlledAI.gameObject.SetActive(false);
            controlledAI.CharacterController.transform.localPosition = Vector3.zero;

            controlledAI.transform.position = finalPos.position;

            controlledAI.transform.rotation = finalPos.rotation;

            controlledAI.gameObject.SetActive(true);

            puppet.state = RootMotion.Dynamics.PuppetMaster.State.Alive;
        }

        public IEnumerator MoveHumanoid(HumansAI ai, Transform pos)
        {
            PuppetMaster puppet = ai.Puppet;

            puppet.mode = RootMotion.Dynamics.PuppetMaster.Mode.Disabled;
            yield return new WaitForEndOfFrame();
            ai.transform.position = pos.position;
            ai.transform.rotation = pos.rotation;

            //yield return new WaitForEndOfFrame();
            puppet.mode = RootMotion.Dynamics.PuppetMaster.Mode.Active;
        }

        /*

         



         

         public Humanoid GetOrangeTobo { get { return OrangeTobo; } }

         public Humanoid GetBlueTobo { get { return BlueTobo; } }

         public Humanoid GetGreenTobo { get { return GreenTobo; } }

         public Humanoid GetDummbJustAI { get { return DummbJustAI; } }

         public GameMenu GetUIScreen { get { return UIScreen; } }

         */

        public bool GamePaused { get; private set; } = false;

        public float MinElevation { get { return -200f; } }
        public float MaxElevation { get { return 200f; } }

        /*
        public IEnumerator MoveDummb(Transform finalPos)
        {
            Dummb.GetPuppet.state = RootMotion.Dynamics.PuppetMaster.State.Frozen;
            yield return new WaitForEndOfFrame();
            Dummb.gameObject.SetActive(false);
            Dummb.GetCharacterController.transform.localPosition = Vector3.zero;

            Dummb.transform.position = finalPos.position;

            Dummb.transform.rotation = finalPos.rotation;

            Dummb.gameObject.SetActive(true);

            Dummb.GetPuppet.state = RootMotion.Dynamics.PuppetMaster.State.Alive;

        }
        */


        /*

       


        public void PauseGame()
        {
            if (!InstantPause) { StartCoroutine(PauseIE()); }
            else
            {
                GetEnvironmentEffects.MakeEnvironmentDark();
                PauseImplementation();
            }

        }

        

        private void PauseImplementation()
        {
            GameMenu menu = GetUIScreen;
            menu.ShowGameMenu();

            menu.EnableMenu(MenuParts.MenuPart.PauseMenu);

            GetPlayer.PlayerCamera.cullingMask = 1 << menu.gameObject.layer | 1 << LayerMask.NameToLayer("UI");

            GamePaused = true;

            Time.timeScale = 0;
        }

        private void UnpauseImplement()
        {
            GetUIScreen.HideGameMenu();

            GamePaused = false;

            Time.timeScale = 1;
        }

        private IEnumerator PauseIE()
        {
            yield return StartCoroutine(GetEnvironmentEffects.SmoothMakeEnvironmentDark());

            yield return StartCoroutine(GetEnvironmentEffects.ChangeEnvironmentTransition(1, 1, 0));

            PauseImplementation();



        }

        public void UnPauseGame()
        {
            if (!InstantPause) { StartCoroutine(UnPauseIE()); }
            else
            {
                GetEnvironmentEffects.MakeEnvironmentNormal();
                UnpauseImplement();
            }

            
        }

        private IEnumerator UnPauseIE()
        {
            UnpauseImplement();

            yield return StartCoroutine(GetEnvironmentEffects.ChangeEnvironmentTransition(1, 0, 1));

            yield return StartCoroutine(GetEnvironmentEffects.SmoothMakeEnvironmentNormal());

        }

        





        
        
        

        */

        //constants

        private void Start()
        {
            if (DebugMode) { Player.gameObject.SetActive(false); PlayerDebug.gameObject.SetActive(true); }
            else { Player.gameObject.SetActive(true); PlayerDebug.gameObject.SetActive(false); }
           // if (Screen3D) { IniScale = Screen3D.transform.localScale; }
           // if(UIScreen) { MenuIniScale = UIScreen.transform.localScale; }

          //  IBInstance = InputBridge.Instance;
        }

        /*
        protected virtual void Update()
        {
            if (IBInstance.RightThumbstickDown && !Player.Show3DViewer && !GetLevelController.IsLevelChanging)
            {
                if (!GamePaused) { PauseGame(); }
                else { UnPauseGame(); }
            }
        }
        */

        //public float EditScaleTime { get; } = 15; //was 1
        //public float EditUpOffset { get; } = 0.3f;
        //public float EditforwardOffset { get; } = 3; //was 0.55


        public float EditScaleTime
        {
            get
            {
                if (DebugMode) { return 1; }
                else { return 15; }
            }
        }
        
        public float EditUpOffset { get
            {
                if (DebugMode) { return 0.3f; }
                else { return 0.3f; }

            } }


        public float EditforwardOffset
        {
            get
            {
                if (DebugMode) { return 0.45f; }
                else { return 3f; }
            }
        }

        public float MenuScaleTime { get; } = 3;
        public float MenuUpOffset { get; } = -7f;
        public float MenuForwardOffset { get; } = 15;


    }
}

