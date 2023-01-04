using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    //check neighbours and move to that neighbour on which player is not there and is farthest from the player.
    //if two points are same distance from the player then choose one randomly.
    //if no option left and trapped, then move towards the player.
    public class CatchMeBehaviour : Behaviours
    {
        

        public CatchMeTile DummOnTile { get; set; }
        
        public CatchMeTile AIOnTile { get; set; }

        public CatchMeTile CurrentTile { get; set; }

        public CatchMeTile MoveToTile { get; set; }

        Position PlayerPos;
        Position AIPos;

        List<float> positionList;
        List<CatchMeTile> tileList;
        List<int> maxIndexList;
        private PeopleBehavioursEnum DummbCurrentBehaviour;
        private CatchMeBehaviour DummbCatchBehaviour;

        const int RunSpeed = 2;
        Humanoid DummbAI;
        TheWeb Web;

        public bool TurnToMove { get; set; } = false;

        public struct Position
        {
            public int X;
            public int Y;

        }

        

        public override PeopleBehavioursEnum GetStateType()
        {
            return PeopleBehavioursEnum.CatchMe;
        }


        public override void OnStateEnter()
        {
           

            if (People.GetHumanoidType == HumanoidType.AI)
            {
                if (positionList == null) { positionList = new List<float>(); }
                if (tileList == null) { tileList = new List<CatchMeTile>(); }
                if (maxIndexList == null) { maxIndexList = new List<int>(); }

                

                DummbCurrentBehaviour = People.GetCurrentBehaviour;
            }

            StandAndLaugh();

            Web = (TheWeb)Controller.GetLevelController.GetCurrentMainLevel;

        }

        

        public void StandAndLaugh()
        {
            People.Agent.enabled = false;
            People.Animator.SetInteger(SpeedHash, 0);
        }

        public override void BehaviourUpdate()
        {
            //wheneven AI turn to Move, then 
            //check for neighbours and put in the list for distance of neighbours from the player.

            if (!TurnToMove) return;

            switch (People.GetHumanoidType)
            {
                case HumanoidType.AI: //might have to change this.

                    //if (DummbCurrentBehaviour != PeopleBehavioursEnum.CatchMe) return;
                    
                    if (!DummbCatchBehaviour) 
                    {
                        
                        DummbCatchBehaviour = Web.CatcherCurrentBehaviour;
                    
                    }

                    if (!DummbCatchBehaviour) return;

                    if (DummbCatchBehaviour.CurrentTile)
                    {
                        Debug.Log(DummbCatchBehaviour.CurrentTile.transform.parent.gameObject.name);

                        positionList.Clear();
                        tileList.Clear();

                        for (int i = 0; i < CurrentTile.Neighbours.Length; i++)
                        {
                            CatchMeTile neighbour = CurrentTile.Neighbours[i];

                            Vector2 neighbourPos = new Vector2(neighbour.X, neighbour.Y);
                            Vector2 playerPos = new Vector2(DummbCatchBehaviour.CurrentTile.X, DummbCatchBehaviour.CurrentTile.Y);



                            float distance = (playerPos - neighbourPos).sqrMagnitude;

                            

                            if (!neighbour.UniqueTile)
                            {
                                if (DummbCatchBehaviour.CurrentTile.UniqueTile)
                                {
                                    if (!neighbour.NeighbourToUniqueTile)
                                    {
                                        positionList.Add(distance);

                                        tileList.Add(neighbour);
                                    }
                                }
                                else
                                {
                                    positionList.Add(distance);

                                    tileList.Add(neighbour);
                                }

                                
                            }

                        }

                        int maxIndex = GetMaxIndex(positionList);

                        MoveToTile = tileList[maxIndex];

                        MoveToTarget(MoveToTile);

                        TurnToMove = false;

                    }

                    break;

                
            }

        }

        private void MoveToTarget(CatchMeTile tile)
        {
            LookTo(tile);
            People.Agent.enabled = true;
            People.Agent.destination = tile.transform.position;
            People.Animator.SetInteger(SpeedHash, RunSpeed);
            
        }

        public void MoveNowToSelectedTile(CatchMeTile tile)
        {
           // if (People.GetHumanoidType != HumanoidType.DummbAI) return;
           
            MoveToTile = tile;
            MoveToTarget(tile);
            TurnToMove = false;

            if (Web.RunnerCurrentBehaviour.CurrentTile == tile)
            {
                StartCoroutine(Web.LevelPassed());
                
            }
        }

        private void LookTo(CatchMeTile tile)
        {
            var lookPos = People.GetRootBone.position - tile.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            tile.transform.rotation = rotation;
        }

        public override void OnStateExit()
        {
            
        }

        private int GetMaxIndex(List<float> list1)
        {
            float max = float.MinValue;

            maxIndexList.Clear();
            
            for(int i = 0; i < list1.Count; i++)
            {
                if(list1[i] > max)
                {
                    max = list1[i];
                }
            }

            for(int i = 0; i < list1.Count; i++)
            {
                if(list1[i] == max)
                {
                    maxIndexList.Add(i);
                }
            }



            return maxIndexList[Random.Range(0, maxIndexList.Count)];

            
        }
    }
}

