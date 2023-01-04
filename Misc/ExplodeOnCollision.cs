using BNG;
using LazyPanda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnCollision : MonoBehaviour
{
    [SerializeField] float ExplosionRadius = 5;
    [SerializeField] float ExplosionForce = 50;

    [SerializeField] GameObject ExplosionEffectprefab;

    private void OnCollisionEnter(Collision collision)
    {
        GameController controller = Instancer.GameControllerInstance;
        EnvironmentEffects effect = controller.GetEnvironmentEffects;
        effect.ExplodeWithForce(gameObject, ExplosionEffectprefab, ExplosionForce, ExplosionRadius);

    }

    private void MissileExplode()
    {
        GameController controller = Instancer.GameControllerInstance;
        EnvironmentEffects effect = controller.GetEnvironmentEffects;
        
    }

}
