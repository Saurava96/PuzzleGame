using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPanda
{
    public class ObjectMovement : EnvironmentBase
    {
        public enum Direction { X, Y, Z, None}

        [SerializeField] Direction ThisDirection = Direction.Y;



        [SerializeField] bool AllowObjectMovement = true;
        [SerializeField] bool OneTouchMovement = false;

        [SerializeField] float FinalMaxPos;
        [SerializeField] float FinalMinPos;

        [SerializeField] float MovementSpeed = 1;
        [SerializeField] bool AutoInitialPosition = false;
        [SerializeField] bool RigidbodyMovePosition = false;


        Rigidbody Rigid;

        public Direction GetThisDirection { get { return ThisDirection; } }

        public float GetFinalMaxPos { get { return FinalMaxPos; } }

        public float GetFinalMinPos { get { return FinalMinPos; } }


        public override void InteractionStart()
        {
            Rigid = GetComponent<Rigidbody>();
        }

        public override void InteractionEntered(bool value)
        {
            if (!AllowObjectMovement) return;
            if (RoutineStarted) return;
            if (!OneTouchMovement) return;
            
            StartCoroutine(MovementController(value));
        }

        public override void ActivateEnvironmentBase(bool value)
        {
            if (value) { AllowObjectMovement = true; }
            else { AllowObjectMovement = false; }
        }

        private IEnumerator MovementController(bool add)
        {
            RoutineStarted = true;
            MoveToInitial = false;

            switch (ThisDirection)
            {
                case Direction.X:

                    if (add)
                    {
                        while (transform.position.x <= FinalMaxPos)
                        {
                            transform.position += new Vector3(MovementSpeed * Time.deltaTime, 0, 0);

                            yield return new WaitForEndOfFrame();
                        }
                    }
                    else
                    {
                        while (transform.position.x >= FinalMinPos)
                        {
                            transform.position -= new Vector3(MovementSpeed * Time.deltaTime, 0, 0);

                            yield return new WaitForEndOfFrame();
                        }
                    }

                    break;


                case Direction.Y:

                    if (add)
                    {
                        while (transform.position.y <= FinalMaxPos)
                        {
                            transform.position += new Vector3(0, MovementSpeed * Time.deltaTime, 0);

                            yield return new WaitForEndOfFrame();
                        }
                    }
                    else
                    {
                        while (transform.position.y >= FinalMinPos)
                        {
                            transform.position -= new Vector3(0, MovementSpeed * Time.deltaTime, 0);

                            yield return new WaitForEndOfFrame();
                        }
                    }

                    break;


                case Direction.Z:

                    if (add)
                    {
                        while (transform.position.z <= FinalMaxPos)
                        {
                            transform.position += new Vector3(0, 0, MovementSpeed * Time.deltaTime);

                            yield return new WaitForEndOfFrame();
                        }
                    }
                    else
                    {
                        while (transform.position.z >= FinalMinPos)
                        {
                            transform.position -= new Vector3(0, 0, MovementSpeed * Time.deltaTime);

                            yield return new WaitForEndOfFrame();
                        }
                    }

                    break;


            }
            
        }

        Vector3 Iniposition;
        Vector3 LastPosition;
        bool MoveToInitial = false;
        float threshold = 0.005f;

        protected override void Start()
        {
            base.Start();

            Iniposition = transform.position;
            LastPosition = transform.position;
        }

        protected override void Update()
        {
            if (!AutoInitialPosition) return;
            if (!MoveToInitial) return;


            transform.position = Vector3.MoveTowards(transform.position, Iniposition, Time.deltaTime * MovementSpeed);
        }

        public override void InteractionStay(bool add)
        {
            if (!AllowObjectMovement) return;
            if (OneTouchMovement) return;
            

            MoveToInitial = false;

            switch (ThisDirection)
            {
                case Direction.X:
                    
                    Vector3 tempVect = new Vector3(1, 0, 0);
                    tempVect = tempVect.normalized * MovementSpeed * Time.deltaTime;
                    
                    if (add)
                    {
                        if (transform.localPosition.x <= FinalMaxPos) //changed from position to localposition because of the car in the castle level.
                        {
                            MovementImplementation(true,tempVect);
                            Debug.Log("moving in positive x");
                            
                        }
                    }
                    else
                    {
                        if (transform.localPosition.x >= FinalMinPos)
                        {
                            MovementImplementation(false, tempVect);
                            Debug.Log("moving in negetive x");
                        }
                    }

                    break;

                case Direction.Y:
                    
                    Vector3 tempVectY = new Vector3(0, 1, 0);
                    tempVectY = tempVectY.normalized * MovementSpeed * Time.deltaTime;

                    if (add)
                    {
                        if (transform.position.y <= FinalMaxPos)
                        {
                            MovementImplementation(true, tempVectY);
                        }
                    }
                    else
                    {
                        if (transform.position.y >= FinalMinPos)
                        {
                            MovementImplementation(false, tempVectY);
                        }
                    }

                    break;

                case Direction.Z:
                    
                    Vector3 tempVectZ = new Vector3(0, 0, 1);
                    tempVectZ = tempVectZ.normalized * MovementSpeed * Time.deltaTime;
                    
                    if (add)
                    {
                        if (transform.position.z <= FinalMaxPos)
                        {
                            MovementImplementation(true, tempVectZ);
                        }
                    }
                    else
                    {
                        if (transform.position.z >= FinalMinPos)
                        {
                            MovementImplementation(false, tempVectZ);
                        }
                    }

                    break;

            }

        }

        private void MovementImplementation(bool add, Vector3 tempVect)
        {
            if (add)
            {
                if (AllowRigidMovement())
                {
                    Rigid.MovePosition(transform.position + tempVect);
                }
                else
                {
                    transform.position += tempVect;
                }
            }
            else
            {
                if (AllowRigidMovement())
                {
                    Rigid.MovePosition(transform.position - tempVect);
                }
                else
                {
                    transform.position -= tempVect;
                }
            }

            
        }

        private bool AllowRigidMovement()
        {
            if (Rigid)
            {
                if (RigidbodyMovePosition)
                {
                    return true;
                }
            }

            return false;

        }

        public override void InteractionEnd()
        {
            if (!AutoInitialPosition) return;
            MoveToInitial = true;
        }

        private bool IsChangingPosition()
        {
            Vector3 offset = transform.position - LastPosition;

            //just for X axis for now..

            switch (ThisDirection)
            {
                case Direction.X:

                    if (offset.x > threshold)
                    {
                        LastPosition = transform.position;

                        return true;

                    }
                    else
                    if (offset.x < -threshold)
                    {
                        LastPosition = transform.position;

                        return true;

                    }

                    break;
                
                case Direction.Y:
                    Debug.Log(offset.y);
                    if (offset.y > threshold)
                    {
                        LastPosition = transform.position;

                        return true;

                    }
                    else
                    if (offset.y < -threshold)
                    {
                        LastPosition = transform.position;

                        return true;

                    }
                    break;

                case Direction.Z:

                    if (offset.z > threshold)
                    {
                        LastPosition = transform.position;

                        return true;

                    }
                    else
                    if (offset.z < -threshold)
                    {
                        LastPosition = transform.position;

                        return true;

                    }

                    break;

            }

            

            return false;
        }




    }

}
