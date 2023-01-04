using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

public class AutoAIInstantiator : MonoBehaviour
{
    [Header("Manual Reference")]

    public GameObject[] AutoAIPrefabs;

    public GameObject[] InstantiatePoints;

    public bool MakePuppetKinematic = true;

    [Header("Auto Reference")]

    public List<GameObject> AutoAIs;



    public void InstantiateAutoAIs()
    {

        AutoAIs ??= new List<GameObject>();

        if (InstantiatePoints.Length == 0)
        {
            InstantiatePoints = new GameObject[transform.childCount];

            for (int i = 0; i < InstantiatePoints.Length; i++)
            {
                InstantiatePoints[i] = transform.GetChild(i).gameObject;
            }

        }


        if (AutoAIPrefabs.Length != InstantiatePoints.Length) { Debug.LogError("Number of Points and Prefabs are different"); return; }

        for (int i = 0; i < AutoAIPrefabs.Length; i++)
        {
            GameObject ai = Instantiate(AutoAIPrefabs[i]);
            
            if (MakePuppetKinematic) { ai.GetComponentInChildren<PuppetMaster>().mode = PuppetMaster.Mode.Kinematic; }
            
            AutoAIs.Add(ai);

        }

    }

    private void OnDisable()
    {
        if (AutoAIs == null) return;

        for (int i = 0; i <AutoAIs.Count; i++)
        {
            AutoAIs[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (AutoAIs == null) return;

        for (int i = 0; i < AutoAIs.Count; i++)
        {
            AutoAIs[i].SetActive(true);
        }

    }

}
