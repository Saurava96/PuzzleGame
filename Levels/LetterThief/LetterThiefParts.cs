using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterThiefParts : MonoBehaviour
{
    public enum Parts { None, LeftUI,CenterUI,RightUI, BoyUIpos, GirlUIPos, GoToShop, BoyDefaultPos, GirlDefaultPos, DoorOptionCanvas, LetterCanvas,BoyUIDoorPos
    , BoyRoom, GirlRoom, BoyShop,GirlShop,GirlUIDoorPos, BoyPlayerPos,GirlPlayerPos,BoyShopPlayerPos,GirlShopPlayerPos,BoyShopControlledPos,GirlShopControlledPos
    ,LetterBox, GirlRoomChestPos, BoyRoomChestPos, BoyShopChestPos}

    public Parts ThisPart = Parts.None;
}
