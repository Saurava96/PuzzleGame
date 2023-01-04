using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyPanda;

namespace DarkDemon
{
    public class LevelChangerTile : MonoBehaviour
    {
        [SerializeField] float Speed = 3f;

        public Transform PlayerPos;
        public Transform DummbPos;

        public MakeChild MakeChild { get; private set; }

        Vector3 FinalAdder = new Vector3(0, 0, 100);

        float Timer = 0;
        bool ChangedLevel = false;
        GameController Controller;

        private void Start()
        {
            MakeChild = GetComponent<MakeChild>();
            Controller = Instancer.GameControllerInstance;
        }

        private void Update()
        {
            if (!MakeChild) return;

            if (!MakeChild.PlayerOnTile) return;

            if (!MakeChild.DummbOnTile) return;


            MoveTile();
            ChangeLevel();
        }

        private void MoveTile()
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + FinalAdder, Time.deltaTime * Speed);
            Timer += Time.deltaTime;
        }

        private void ChangeLevel()
        {
            if (Timer <= 10) return;

            Controller.GetLevelController.ChangeToNextLevel();
            
            Timer = 0;

            MakeChild.DummbOutOfTile();
            MakeChild.PlayerOutOfTile();
        }


        


        //Start moving the tile using moveTowards to show that the tile is taking the player to the new world.


    }
}

