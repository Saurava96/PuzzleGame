using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RootMotion.Demos.Turret;

namespace DarkDemon
{
    public class LetterThief : TheLevel
    {
       

        [Header("Manual Reference")]
        public GameObject BoyControlledPre;
        public GameObject GirlControlledPre;
        public GameObject BoyShopPrefab;
        public GameObject GirlShopPrefab;
        
        public GameObject[] AutoAIPrefabs;
        

        [Header("Auto Reference")]
       
        public Transform BoyRoomDefaultT;
        public Transform GirlRoomDefaultT;
        public Transform BoyShopControlledPos;
        public Transform GirlShopControlledPos;

        public List<EnvironmentGeneral> UIs;

        
        
        
        public Transform BoyPlayerPos;
        public Transform GirlPlayerPos;
        public Transform BoyShopPlayerPos;
        public Transform GirlShopPlayerPos;
        public Transform GirlRoomChestPos;
        public Transform BoyRoomChestPos;
        public Transform BoyShopChestPos;
        public GameObject BoyRoom;
        public GameObject GirlRoom;
        public GameObject BoyShop;
        public GameObject GirlShop;
        public GameObject LetterBox;

        public ControlledAI BoyControlled;
        public ControlledAI GirlControlled;
        

        public GameObject[] AutoAIs;
        

        public enum LevelStates {None, BoyRoom,GirlRoom,BoyStore,GirlStore}

        public LevelStates CurrentLevelState = LevelStates.BoyRoom;

        private LevelStates CurrentState = LevelStates.None;

        public bool BoxBought { get; set; } = false;
        public bool BoyBoughtKey { get; set; } = false;
        
        public bool GirlBoughtKey { get; set; } = false;

        public List<LockKey> BoyBoughtKeyCodes;
        public List<LockKey> GirlBoughtKeyCodes;


        public void AddToBoyBoughtKeys(LockKey item)
        {
            BoyBoughtKeyCodes ??= new List<LockKey>();

            BoyBoughtKeyCodes.Add(item);

            Debug.Log("boy bough key and added to list");

        }

        public void AddToGirlBoughtKeys(LockKey item)
        {
            GirlBoughtKeyCodes ??= new List<LockKey>();

            GirlBoughtKeyCodes.Add(item);

        }



        public override LevelsEnum GetStateType()
        {
            return LevelsEnum.LetterThief;
        }



        public override void LevelUpdate()
        {
            if (!IsLevelEntered) return;

            if (CurrentState == CurrentLevelState) return;

            switch (CurrentLevelState)
            {
                case LevelStates.BoyRoom:

                    if (GirlShop) { GirlShop.SetActive(false); }
                    if (BoyShop) { BoyShop.SetActive(false); }
                    GirlControlled.gameObject.SetActive(false);
                    GirlRoom.SetActive(false);

                    if (BoxBought) 
                    {
                        UIViewer ui = LetterBox.GetComponentInChildren<UIViewer>();
                        ui.UIs.Clear();

                        ui.UpdateButtonFunctionality(ChestParts.Parts.CenterUI,ButtonGeneral.LetterThiefButtonState.SendLetter, "Send Letter");
                        ui.UpdateButtonFunctionality(ChestParts.Parts.RightUI,ButtonGeneral.LetterThiefButtonState.BoyLockLetterBox, "Lock Letter Box");
                        ui.UpdateButtonFunctionality(ChestParts.Parts.LeftUI,ButtonGeneral.LetterThiefButtonState.BoyUnlockLetterBox, "Unlock Letter Box");

                        LetterBox.transform.parent = BoyRoom.transform;
                        LetterBox.transform.SetPositionAndRotation(BoyRoomChestPos.position, BoyRoomChestPos.rotation);
                        LetterBox.SetActive(true); 
                    }

                    BoyRoom.SetActive(true);
                    BoyControlled.gameObject.SetActive(true);
                    StartCoroutine(Controller.MoveControlledAI(BoyControlled, BoyRoomDefaultT));
                    StartCoroutine(Player.MovePlayer(BoyPlayerPos));
                    CurrentState = CurrentLevelState;

                    break;

                case LevelStates.GirlRoom:

                  
                    BoyRoom.SetActive(false);
                    BoyControlled.gameObject.SetActive(false);
                    if (GirlShop) { GirlShop.SetActive(false); }
                    if (BoyShop) { BoyShop.SetActive(false); }

                    if (BoxBought)
                    {
                        UIViewer ui = LetterBox.GetComponentInChildren<UIViewer>();
                        ui.UIs.Clear();

                        ui.UpdateButtonFunctionality(ChestParts.Parts.CenterUI,ButtonGeneral.LetterThiefButtonState.SendLetter, "Send Letter");
                        ui.UpdateButtonFunctionality(ChestParts.Parts.RightUI,ButtonGeneral.LetterThiefButtonState.GirlLockLetterBox, "Lock Letter Box");
                        ui.UpdateButtonFunctionality(ChestParts.Parts.LeftUI,ButtonGeneral.LetterThiefButtonState.GirlUnlockLetterBox, "Unlock Letter Box");

                        LetterBox.transform.parent = GirlRoom.transform;
                        LetterBox.transform.SetPositionAndRotation(GirlRoomChestPos.position, GirlRoomChestPos.rotation);
                        LetterBox.SetActive(true);
                    }

                    GirlControlled.gameObject.SetActive(true);
                    GirlRoom.SetActive(true);
                    StartCoroutine(Controller.MoveControlledAI(GirlControlled, GirlRoomDefaultT));
                    StartCoroutine(Player.MovePlayer(GirlPlayerPos));
                    CurrentState = CurrentLevelState;

                    break;

                case LevelStates.GirlStore:

                    BoyRoom.SetActive(false);
                    if (BoyShop) { BoyShop.SetActive(false); }
                    BoyControlled.gameObject.SetActive(false);
                    GirlRoom.SetActive(false);

                    GirlShop.SetActive(true);   
                    GirlControlled.gameObject.SetActive(true);
                    StartCoroutine(Controller.MoveControlledAI(GirlControlled, GirlShopControlledPos));
                    StartCoroutine(Player.MovePlayer(GirlShopPlayerPos));
                    CurrentState = CurrentLevelState;
                    break;

                case LevelStates.BoyStore:

                    BoyRoom.SetActive(false);

                    if (GirlShop) { GirlShop.SetActive(false); }
                    GirlRoom.SetActive(false);
                    GirlControlled.gameObject.SetActive(false);
                    BoyShop.SetActive(true);

                    if (BoxBought)
                    {
                        LetterBox.SetActive(false);
                    }
                    else
                    {
                        UIViewer ui = LetterBox.GetComponentInChildren<UIViewer>();
                        ui.UIs.Clear();
                        
                        ui.UpdateButtonFunctionality(ChestParts.Parts.CenterUI,ButtonGeneral.LetterThiefButtonState.BuyBox, "Buy Letter Box");

                    }

                    BoyControlled.gameObject.SetActive(true);
                    StartCoroutine(Controller.MoveControlledAI(BoyControlled, BoyShopControlledPos));
                    StartCoroutine(Player.MovePlayer(BoyShopPlayerPos));
                    CurrentState = CurrentLevelState;
                    break;

            }

            
            
        }

        

        protected override void VariableReference()
        {
            UIs = new List<EnvironmentGeneral>();

            for (int i = 0; i < LevelInstantiatedObjects.Count; i++)
            {
                GameObject game = LevelInstantiatedObjects[i];

                LetterThiefParts[] parts = game.GetComponentsInChildren<LetterThiefParts>();

                for(int k = 0; k < parts.Length; k++)
                {
                    LetterThiefParts part = parts[k];   

                    if (part)
                    {
                        switch (part.ThisPart)
                        {
                            case LetterThiefParts.Parts.BoyDefaultPos:
                                BoyRoomDefaultT = part.transform; break;

                            case LetterThiefParts.Parts.GirlDefaultPos:
                                GirlRoomDefaultT = part.transform; break;

                            case LetterThiefParts.Parts.BoyRoom:
                                BoyRoom = part.gameObject; break;

                            case LetterThiefParts.Parts.GirlRoom:
                                GirlRoom = part.gameObject; break;

                            case LetterThiefParts.Parts.BoyShop:
                                BoyShop = part.gameObject; break;

                            case LetterThiefParts.Parts.GirlShop:
                                GirlShop = part.gameObject;
                                break;

                            case LetterThiefParts.Parts.BoyPlayerPos:
                                BoyPlayerPos = part.transform; break;

                            case LetterThiefParts.Parts.GirlPlayerPos:
                                GirlPlayerPos = part.transform; break;

                            case LetterThiefParts.Parts.BoyShopPlayerPos:
                                BoyShopPlayerPos = part.transform; break;

                            case LetterThiefParts.Parts.GirlShopPlayerPos:
                                GirlShopPlayerPos = part.transform;
                                break;

                            case LetterThiefParts.Parts.BoyShopControlledPos:
                                BoyShopControlledPos = part.transform; break;

                            case LetterThiefParts.Parts.GirlShopControlledPos:
                                GirlShopControlledPos = part.transform; break;

                            case LetterThiefParts.Parts.LetterBox:
                                LetterBox = part.gameObject; break;

                            case LetterThiefParts.Parts.GirlRoomChestPos:
                                GirlRoomChestPos = part.transform; break;

                            case LetterThiefParts.Parts.BoyRoomChestPos:
                                BoyRoomChestPos = part.transform; break;


                            case LetterThiefParts.Parts.BoyShopChestPos:
                                BoyShopChestPos = part.transform;
                                break;


                        }
                    }
                }

            }

        }

        protected override void ControlledAIInstantiate()
        {


            GameObject boy = SimplePool.Spawn(BoyControlledPre,BoyRoomDefaultT.position,BoyRoomDefaultT.rotation);
            BoyControlled = boy.GetComponent<ControlledAI>();
            //boy.SetActive(false);

            GameObject girl = SimplePool.Spawn(GirlControlledPre, GirlRoomDefaultT.position, GirlRoomDefaultT.rotation);
            GirlControlled = girl.GetComponent<ControlledAI>();
            //girl.SetActive(false);

        }



        public override void SetLevelMusic()
        {
            
        }

        public override void SetWeather()
        {
            
        }
    }
}

