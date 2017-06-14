﻿using EA4S.Audio;
using System.Collections;
using System.Collections.Generic;
using EA4S.Core;
using UnityEngine;

namespace EA4S.LivingLetters
{
    public class TouchToDance : MonoBehaviour
    {
        public void OnMouseDown()
        {
            var view = GetComponent<LetterObjectView>();
            view.ToggleDance();
            var letter = (AppManager.Instance as AppManager).Teacher.GetRandomTestLetterLL(useMaxJourneyData: true);
            view.Initialize(letter);

            AudioManager.I.PlayLetter(letter.Data, true);
        }
    }
}