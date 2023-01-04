using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkDemon
{
    public class GuessTheWord : TheLevel
    {
        [Header("Manual Reference")]

        [SerializeField] protected GameObject PersonWithQuestionPre;
        [SerializeField] protected GameObject Person1Pre;
        [SerializeField] protected GameObject Person2Pre;
        [SerializeField] protected GameObject Person3Pre;



        [Header("Auto Reference")]
        public Humanoid PersonWithQuestion;
        public Humanoid Person1;
        public Humanoid Person2;
        public Humanoid Person3;
        public UIViewer BlackBoard;

        public Transform PersonWithQuestionPos;
        public Transform Person1Pos;
        public Transform Person2Pos;
        public Transform Person3Pos;

        public bool CorrectWordSelected { get; set; } = false;

        public bool InCorrectWordSelected { get; set; } = false;

        private bool LetPlayerSelect = false;


        public override LevelsEnum GetStateType()
        {
            return LevelsEnum.GuessTheWord;
        }


        protected override void ControlledAIInstantiate()
        {


            GameObject mainperson = SimplePool.Spawn(PersonWithQuestionPre, PersonWithQuestionPos.position, PersonWithQuestionPos.rotation);
            PersonWithQuestion = mainperson.GetComponentInChildren<Humanoid>();
            SetTalkBehaviour(PersonWithQuestion);


            GameObject person1 = SimplePool.Spawn(Person1Pre, Person1Pos.position, Person1Pos.rotation);
            Person1 = person1.GetComponentInChildren<Humanoid>();
            SetTalkBehaviour(Person1);

            GameObject person2 = SimplePool.Spawn(Person2Pre, Person2Pos.position, Person2Pos.rotation);
            Person2 = person2.GetComponentInChildren<Humanoid>();
            SetTalkBehaviour(Person2);

            GameObject person3 = SimplePool.Spawn(Person3Pre, Person3Pos.position, Person3Pos.rotation);
            Person3 = person3.GetComponentInChildren<Humanoid>();
            SetTalkBehaviour(Person3);



        }

        public override void LevelUpdate()
        {
            if (!LetPlayerSelect) return;

            if (CorrectWordSelected)
            {
                //level complete..
                Debug.Log("Level Complete");
            }

            if (InCorrectWordSelected)
            {
                //level failed..
                Debug.Log("Level Failed as wrong word selected");
            }
            
        }

        private TalkBehaviour GetTalkBehaviour(Humanoid human)
        {
            //talk behaviour should already be activated in ControlledAIInstantiate method.

            return (TalkBehaviour)human.GetCurrentMainBehaviour;

        }

        private void SetTalkBehaviour(Humanoid human)
        {
            human.AllowTalkBehaviour = true;
            TalkBehaviour talkB = human.GetComponent<TalkBehaviour>();
            talkB.AutoStartTalk = false;

            human.SetCurrentBehaviour = PeopleBehavioursEnum.Talk;
        }

        protected override void VariableReference()
        {
            for (int i = 0; i < LevelInstantiatedObjects.Count; i++)
            {
                GameObject game = LevelInstantiatedObjects[i];

                GuessTheWordParts[] parts = game.GetComponentsInChildren<GuessTheWordParts>();

                for (int k = 0; k < parts.Length; k++)
                {
                    GuessTheWordParts part = parts[k];

                    if (part)
                    {
                        switch (part.ThisPart)
                        {
                            case GuessTheWordParts.Parts.MainPersonPos:
                                PersonWithQuestionPos = part.transform; break;

                            case GuessTheWordParts.Parts.Person1Pos:
                                Person1Pos = part.transform; break;

                            case GuessTheWordParts.Parts.Person2Pos:
                                Person2Pos = part.transform; break;

                            case GuessTheWordParts.Parts.Person3Pos:
                                Person3Pos = part.transform; break;

                            case GuessTheWordParts.Parts.BlackBoard:
                                
                                BlackBoard = part.GetComponent<UIViewer>(); 
                                break;


                        }
                    }
                }

            }
        }


        public IEnumerator LevelSequence()
        {
            LetPlayerSelect = false;

            //Main person speaks "There are six words written on the board."
            yield return StartCoroutine(GetTalkBehaviour(PersonWithQuestion).Talk(2));

            //"Cat, Dog, Has, Max,Dim, Tag"
            yield return StartCoroutine(GetTalkBehaviour(PersonWithQuestion).Talk(2));

            //"I have given you each a piece of paper with different letter written on it"
            yield return StartCoroutine(GetTalkBehaviour(PersonWithQuestion).Talk(3));

            //"Each letter is included in the secret word and the secret word is one of the words on the board.".
            yield return StartCoroutine(GetTalkBehaviour(PersonWithQuestion).Talk(4));

            //"You need to figure out the secret word"
            yield return StartCoroutine(GetTalkBehaviour(PersonWithQuestion).Talk(2));

            //"I will ask each one of you a question."
            yield return StartCoroutine(GetTalkBehaviour(PersonWithQuestion).Talk(2));

            //"Sam, Do you know the secret word from the letter you have ?
            yield return StartCoroutine(GetTalkBehaviour(PersonWithQuestion).Talk(3));

            //Sam replies: "Yes".
            yield return StartCoroutine(GetTalkBehaviour(Person1).Talk(2));

            //Caitlin, Do you know the secret word from the letter you have ?
            yield return StartCoroutine(GetTalkBehaviour(PersonWithQuestion).Talk(3));

            //Yes
            yield return StartCoroutine(GetTalkBehaviour(Person2).Talk(2));

            //Leo, do you know the secret word from the letter you have ?
            yield return StartCoroutine(GetTalkBehaviour(PersonWithQuestion).Talk(3));

            //Yes
            yield return StartCoroutine(GetTalkBehaviour(Person3).Talk(2));

            //What is the secret word ??
            yield return StartCoroutine(BlackBoard.UIShow(BlackBoard.UIs, true));

            LetPlayerSelect = true;

        }



        public override void SetLevelMusic()
        {
            
        }

        public override void SetWeather()
        {
            
        }
    }
}

