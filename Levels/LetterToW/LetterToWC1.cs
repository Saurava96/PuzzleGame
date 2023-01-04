using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LazyPanda;

namespace DarkDemon
{
    public class LetterToWC1 : TheCheckPoint2
    {

        private bool SequencePlayed = false;

        TextMeshProUGUI Text;
        WeaponSpawner HomingLauncher;

        public override void OnCheckPointEntered()
        {
            if (SequencePlayed) return;
            if(Text == null || HomingLauncher == null)
            {
                LetterToW level = (LetterToW)GetCurrentActiveMainLevel();
                Text = level.Text;
                HomingLauncher = level.HomingGrenadeSpawner;
            }
            

            StartCoroutine(Sequence());
        }

        private IEnumerator Sequence()
        {
            SequencePlayed = true;

            //update screen to show wrong password..
            Text.text = "WRONG PASSWORD";

            //screen flickering..


            //wait for few seconds and then,
            yield return new WaitForSeconds(3);

            //shoot the grenade aluncher from here.
            HomingLauncher.SpawnGrenade();

            

        }

    }
}

