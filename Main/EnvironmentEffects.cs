using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkDemon;
using UnityEngine.TextCore.Text;

namespace LazyPanda
{
    public class EnvironmentEffects : MonoBehaviour
    {

        GameController Controller;
        private bool MakeAliveRoutineRunning = false;
        private void Start()
        {
            Controller = Instancer.GameControllerInstance;
        }

        public void Show3DViewer()
        {
            if (Controller.DebugMode) return;

            GameObject screen = Controller.GetScreen3D;

            screen.SetActive(true);
            /*
            Orientation orientation = new Orientation(Controller.EditUpOffset,
                Controller.EditforwardOffset,
                Controller.EditScaleTime, Controller.IniScale);

            screen.transform.position = orientation.Position;
            screen.transform.rotation = orientation.Rotation;
            screen.transform.localScale = orientation.Scale;
            */

        }

        public void Hide3DViewer()
        {
           // GameObject screen = Controller.GetScreen3D;
           // screen.SetActive(false);
           // screen.transform.position = Vector3.zero;

        }

        public void Viewer3dEnvironmentChange(bool show3dviewer)
        {

            if (Controller.DebugMode) return;
            
            Camera playerHead = Controller.GetPlayer.PlayerCamera;
            GameObject screen3d = Controller.GetScreen3D;

            if (show3dviewer)
            {
                //Make scene dark..


                playerHead.cullingMask = 1 << screen3d.layer | 1 << LayerMask.NameToLayer("UI");
                playerHead.clearFlags = CameraClearFlags.SolidColor;
                playerHead.backgroundColor = Color.black;

            }
            else
            {
                playerHead.cullingMask = -1;
                playerHead.clearFlags = CameraClearFlags.Skybox;
            }

        }

        public void MakeEnvironmentNormal()
        {
            if (Controller.DebugMode) return;

            Camera playerHead = Controller.GetPlayer.PlayerCamera;
            playerHead.cullingMask = -1;
            playerHead.clearFlags = CameraClearFlags.Skybox;

        }

        public IEnumerator SmoothMakeEnvironmentNormal()
        {
            MakeEnvironmentNormal();
            
            yield return StartCoroutine(ChangeEnvironmentTransition(1, 1, 0));

            
        }

        public void MakeEnvironmentDark()
        {
            
            Camera playerHead = Controller.GetPlayer.PlayerCamera;

            playerHead.cullingMask = ~-1;
            playerHead.clearFlags = CameraClearFlags.SolidColor;
            playerHead.backgroundColor = Color.black;

        }

        public IEnumerator SmoothMakeEnvironmentDark()
        {
            yield return StartCoroutine(ChangeEnvironmentTransition(1, 0, 1));

            MakeEnvironmentDark();

        }

        

        public IEnumerator ChangeEnvironmentTransition(float duration, float alphaIN, float alphaOut)
        {
            MeshRenderer renderer = Controller.GetPlayer.Fader.GetComponent<MeshRenderer>();

            Material mat = renderer.material;
            

            float timer = 0;
            Color matcolor = mat.color;

            while (timer <= duration)
            {
                matcolor.a = Mathf.Lerp(alphaIN, alphaOut, timer / duration);

                renderer.material.SetColor("_Color", matcolor);
                timer += Time.deltaTime;

                yield return new WaitForEndOfFrame();

            }

            Debug.Log("reaching end");
            matcolor.a = alphaOut;
            renderer.material.SetColor("_Color", matcolor);
           
        }

        

        

      
        public void Explode(GameObject game, GameObject explosionprefab)
        {
            if (explosionprefab) { SimplePool.Spawn(explosionprefab, game.transform.position, game.transform.rotation); }

            SimplePool.Despawn(game);
            
            //sound effect
        }

        public void ExplodeWithForce(GameObject game, GameObject explosionPrefab, float ForceValue, float AreaRadius)
        {
            ApplyForce(game.transform.position, ForceValue, AreaRadius);

            if (explosionPrefab) { SimplePool.Spawn(explosionPrefab, game.transform.position, Quaternion.identity); }

            SimplePool.Despawn(game);

        }

        public void ExplodeWithForce(Vector3 pos, GameObject explosionPrefab, float ForceValue, float AreaRadius)
        {
            ApplyForce(pos, ForceValue, AreaRadius);

            if (explosionPrefab) { SimplePool.Spawn(explosionPrefab, pos, Quaternion.identity); }

        }

        

        private void ApplyForce(Vector3 pos, float ForceValue, float AreaRadius)
        {
            Vector3 explosionPos = pos;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, AreaRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                HumansAI character = Instancer.GameControllerInstance.GetPeopleFromCollider(hit);

                if (character)
                {
                    StartCoroutine(MakeCharacterDead(character));

                }


                if (rb != null)
                {
                    rb.AddExplosionForce(ForceValue, explosionPos, AreaRadius, 3f);
                    Debug.Log("force applied");
                }
                    

            }
        }

        private IEnumerator MakeCharacterDead(HumansAI character)
        {
            yield return new WaitForSeconds(1);
            character.SetCurrentBehaviour = PeopleBehavioursEnum.Dead;

        }

        

        public void ExplodeTimed(GameObject game, GameObject explosionprefab, float time)
        {
            StartCoroutine(ExplodeTimedIE(game, explosionprefab, time));
        }

        private IEnumerator ExplodeTimedIE(GameObject game, GameObject explosionprefab, float time)
        {
            yield return new WaitForSeconds(time);

            Explode(game, explosionprefab);
        }


        
    }
}

