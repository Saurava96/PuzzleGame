using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;
using DarkDemon;

public class Instancer : MonoBehaviour
{
    private static GameObject Player;

    private static Dictionary<Collider, VR3DCharacterBase> HumansDic;

    private static Dictionary<Collider, VR3DCharacterBase> HeadHumanDic;

    private static Dictionary<Collider, ControlledAI> HeadControlledAIDic;

   


    public static VR3DCharacterBase CharacterInControl { get; set; }

    public static VR3DPlayer Player3D;
    
    private static GameController gameControllerInstance;

    /// <summary>
    /// Gets the Main player from the hierarchy..
    /// </summary>
    public static GameObject GetPlayer
    {


        get
        {
            if (!gameControllerInstance.DebugMode)
            {
                if (Player == null)
                {
                    Player = GameObject.FindWithTag("MainPlayer");
                }

                return Player;
            }
            else
            {
                if(Player == null)
                {
                    Player = GameObject.FindWithTag("PlayerDebug");
                }

                return Player;
            }

            
        }
    }

    public static GameController GameControllerInstance
    {
        get
        {
            if (gameControllerInstance == null)
            {
                gameControllerInstance = FindObjectOfType<GameController>();

            }
            return gameControllerInstance;
        }
    }

    public static VR3DPlayer GetPlayer3D
    {
        get
        {
            if(Player3D == null)
            {
                Player3D = FindObjectOfType<VR3DPlayer>();
                
                return Player3D;
            }

            return Player3D;
        }
    }

    /// <summary>
    /// Adds to the dictionary a collider of a particular human using which we can 
    /// get the HumanBase..
    /// </summary>
    /// <param name="c"></param>
    /// <param name="h"></param>
    public static void AddToHumansDic(Collider c, VR3DCharacterBase h)
    {
        if (HumansDic == null)
        {
            HumansDic = new Dictionary<Collider, VR3DCharacterBase>();
        }

        HumansDic.Add(c, h);


    }

    public static void AddToHeadHumanDic(Collider c, VR3DCharacterBase h)
    {
        if (HeadHumanDic == null)
        {
            HeadHumanDic = new Dictionary<Collider, VR3DCharacterBase>();
        }

        HeadHumanDic.Add(c, h);


    }

    public static void AddToHeadControlledAIDic(Collider c, ControlledAI h)
    {
        if (HeadControlledAIDic == null)
        {
            HeadControlledAIDic = new Dictionary<Collider, ControlledAI>();
        }

        HeadControlledAIDic.Add(c, h);


    }



    /// <summary>
    /// Gets a human from the collider which was stored in the awake of the HumanBase Script.
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static VR3DCharacterBase GetHumanFromCollider(Collider c)
    {
        if(HumansDic == null) { Debug.Log("Dictionary is empty");return null; }

        if(HumansDic.TryGetValue(c,out VR3DCharacterBase val))
        {
            return val;
        }

        return null;

    }

    public static VR3DCharacterBase GetHumanFromHeadCollider(Collider c)
    {
        if (HeadHumanDic == null) { Debug.Log("Dictionary is empty"); return null; }

        if (HeadHumanDic.TryGetValue(c, out VR3DCharacterBase val))
        {
            return val;
        }

        return null;

    }



    public static ControlledAI GetControlledAIFromHeadCollider(Collider c)
    {
        if (HeadControlledAIDic == null) { Debug.Log("Dictionary is empty"); return null; }

        if (HeadControlledAIDic.TryGetValue(c, out ControlledAI val))
        {
            return val;
        }

        return null;

    }

}
