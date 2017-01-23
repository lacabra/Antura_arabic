﻿using System.Collections.Generic;
using EA4S.MinigamesAPI;

namespace EA4S.Minigames.Egg
{
    public class QuestionManager
    {
        List<ILivingLetterData> lLetterDataSequence = new List<ILivingLetterData>();

        bool sequence;

        public void StartNewQuestion(float difficulty, bool onlyLetter)
        {
            sequence = false;

            lLetterDataSequence.Clear();

            IQuestionPack questionPack = EggConfiguration.Instance.Questions.GetNextQuestion();

            List<ILivingLetterData> correctAnswers = new List<ILivingLetterData>();
            List<ILivingLetterData> wrongAnswers = new List<ILivingLetterData>();
            
            correctAnswers.AddRange(questionPack.GetCorrectAnswers());
            wrongAnswers.AddRange(questionPack.GetWrongAnswers());

            sequence = EggConfiguration.Instance.Variation == EggConfiguration.EggVariation.Sequence;

            int numberOfLetters = 2; 

            numberOfLetters += ((int)(difficulty * 5) + 1);

            if (numberOfLetters > 8)
            {
                numberOfLetters = 8;
            }

            if (!sequence)
            {
                lLetterDataSequence.Add(correctAnswers[0]);

                numberOfLetters -= 1;

                if (numberOfLetters > wrongAnswers.Count)
                {
                    numberOfLetters = wrongAnswers.Count;
                }

                for (int i = 0; i < numberOfLetters; i++)
                {
                    lLetterDataSequence.Add(wrongAnswers[i]);
                }
            }
            else
            {
                if (numberOfLetters > correctAnswers.Count)
                {
                    numberOfLetters = correctAnswers.Count;
                }

                for (int i = 0; i < numberOfLetters; i++)
                {
                    lLetterDataSequence.Add(correctAnswers[i]);
                }
            }
        }

        public List<ILivingLetterData> GetlLetterDataSequence()
        {
            return lLetterDataSequence;
        }

        public bool IsSequence()
        {
            return sequence;
        }
    }
}