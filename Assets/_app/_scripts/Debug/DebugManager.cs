﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using EA4S.API;

namespace EA4S
{
    public enum DifficulyLevels
    {
        VeryEasy = 0,
        Easy = 1,
        Normal = 2,
        Hard = 3,
        VeryHard = 4
    }

    public class DebugManager : MonoBehaviour
    {
        public static DebugManager I;

        public bool CheatMode = false;

        private bool _ignoreJourneyData = false;
        public bool IgnoreJourneyData {
            get { return _ignoreJourneyData; }
            set {
                _ignoreJourneyData = value;
                Teacher.ConfigAI.forceJourneyIgnore = _ignoreJourneyData;
            }
        }

        private DifficulyLevels _difficultyLevel = DifficulyLevels.Normal;
        public DifficulyLevels DifficultyLevel {
            get { return _difficultyLevel; }
            set {
                _difficultyLevel = value;
                switch (_difficultyLevel) {
                    case DifficulyLevels.VeryEasy:
                        Difficulty = 0.1f;
                        break;
                    case DifficulyLevels.Easy:
                        Difficulty = 0.3f;
                        break;
                    case DifficulyLevels.Normal:
                        Difficulty = 0.5f;
                        break;
                    case DifficulyLevels.Hard:
                        Difficulty = 0.7f;
                        break;
                    case DifficulyLevels.VeryHard:
                        Difficulty = 1.0f;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private bool _firstContactPassed;
        /// <summary>
        /// Gets or sets a value indicating whether [first contact passed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [first contact passed]; otherwise, <c>false</c>.
        /// </value>
        public bool FirstContactPassed {
            get { return !AppManager.Instance.Player.IsFirstContact(); }
            set {
                if (value) {
                    AppManager.Instance.Player.FirstContactPassed(2);
                } else {
                    AppManager.Instance.Player.ResetPlayerProfileCompletion();
                }
                _firstContactPassed = value;
            }
        }

        public float Difficulty = 0.5f;
        public int Stage = 1;
        public int LearningBlock = 1;
        public int PlaySession = 1;

        void Awake()
        {
            I = this;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("DEBUG - SPACE : skip");
            }

            if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0)) {
                Debug.Log("DEBUG - 0");
            }

            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)) {
                Debug.Log("DEBUG - 1");
            }

            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)) {
                Debug.Log("DEBUG - 2");
            }

            if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)) {
                Debug.Log("DEBUG - 3");
            }
        }

        public void LaunchMinigGame(MiniGameCode miniGameCodeSelected)
        {
            Debug.Log("LaunchMinigGame " + miniGameCodeSelected.ToString());
            AppManager.Instance.Player.CurrentJourneyPosition.Stage = Stage;
            AppManager.Instance.Player.CurrentJourneyPosition.LearningBlock = LearningBlock;
            AppManager.Instance.Player.CurrentJourneyPosition.PlaySession = PlaySession;

            // We must force this or the teacher won't use the correct data
            AppManager.Instance.Teacher.InitialiseCurrentPlaySession();

            // Call start game with parameters
            NavigationManager.I.CurrentScene = AppScene.GameSelector;
            MiniGameAPI.Instance.StartGame(
                miniGameCodeSelected,
                new GameConfiguration(Difficulty)
            );

        }

    }
}
