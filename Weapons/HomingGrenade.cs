using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingGrenade : MonoBehaviour
{
    

    public float BulletTravelTime { get; set; }

    
    public Transform Target { get; set; }


    public void ThrowGrenade(float angularDrag, Transform shootingPoint, bool randomness = false)
    {
        if(Target == null) { Debug.LogError("No target for homing grenade"); return; }

        GameObject grenade = gameObject;



        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 projectingVelocity = CalculateProjectileVelocity(BulletTravelTime, Target, shootingPoint);

            if (randomness)
            {
                projectingVelocity += new Vector3(Random.Range(-200, 200) / 100f, 0, Random.Range(-200, 200) / 100f);
            }

            rb.velocity = projectingVelocity;
            rb.angularDrag = angularDrag;
        }


    }

    

    protected virtual Vector3 CalculateProjectileVelocity(float bulletTravelTime, Transform Target, Transform projectingPoint)
    {


        Vector3 displacementXZ = new Vector3(Target.position.x - projectingPoint.position.x, 0, Target.position.z - projectingPoint.position.z);

        float VelocityXZ = displacementXZ.magnitude / bulletTravelTime;

        //d=v(delta)t+1/2*(a OR gravity)*(delta)t^2
        float distanceY = Target.position.y - projectingPoint.position.y;

        float VelocityY = distanceY / bulletTravelTime + 0.5f * Mathf.Abs(Physics.gravity.magnitude) * bulletTravelTime;


        Vector3 finalVelocity = displacementXZ.normalized;
        finalVelocity *= VelocityXZ;
        finalVelocity.y = VelocityY;
        return finalVelocity;


    }

   
}
