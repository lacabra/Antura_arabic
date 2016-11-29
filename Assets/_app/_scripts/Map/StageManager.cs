﻿using UnityEngine;
using System.Collections;
using EA4S;
using DG.Tweening;
namespace EA4S
{
    public class StageManager : MonoBehaviour
    {
        public Color[] colorMaps;
        public GameObject[] stages;
        public GameObject[] cameras;
        public GameObject[] miniMaps;
        public GameObject letter;

        [Header("UIButtons")]
        public GameObject leftStageButton;
        public GameObject rightStageButton;
        public GameObject rightMovementButton;
        public GameObject leftMovementButton;
        public GameObject playButton;
        public GameObject bookButton;
        public GameObject anturaButton;

        int s, i,previousStage;
        bool inTransition;
        void Awake()
        {
            /*AppManager.I.Player.MaxJourneyPosition.Stage = 2;
            AppManager.I.Player.MaxJourneyPosition.LearningBlock = 3;
            AppManager.I.Player.MaxJourneyPosition.PlaySession = 1;*/

            s = AppManager.I.Player.MaxJourneyPosition.Stage;
            for (i = 1; i <= (s - 1); i++)
            {
                stages[i].SetActive(false);
                miniMaps[i].GetComponent<MiniMap>().isAvailableTheWholeMap = true;
                miniMaps[i].GetComponent<MiniMap>().CalculateSettingsStageMap();
            }
            miniMaps[i].GetComponent<MiniMap>().CalculateSettingsStageMap();

            FirstOrLastMap();

            stages[AppManager.I.Player.CurrentJourneyPosition.Stage].SetActive(true);
            Camera.main.backgroundColor = colorMaps[AppManager.I.Player.CurrentJourneyPosition.Stage];
            Camera.main.GetComponent<CameraFog>().color = colorMaps[AppManager.I.Player.CurrentJourneyPosition.Stage];
            letter.GetComponent<LetterMovement>().miniMapScript = miniMaps[AppManager.I.Player.CurrentJourneyPosition.Stage].GetComponent<MiniMap>();

            StartCoroutine("ResetPosLetter");  
        }
        void Start()
        {
            /* FIRST CONTACT FEATURE */
            if (AppManager.I.Player.IsFirstContact())
            {
                FirstContactBehaviour();
            }
            /* --------------------- */
            else ActivateUI();
        }

        void Update()
        {
            // Remove this with First Contact Temp Behaviour
            UpdateTimer();
        }

        #region First Contact Session        
        /// <summary>
        /// Firsts the contact behaviour.
        /// Put Here logic for first contact only situations.
        /// </summary>
        void FirstContactBehaviour()
        {

            if (AppManager.I.Player.IsFirstContact(1))
            {
                // First contact step 1:
                DesactivateUI();
                #region Temp Behaviour (to be deleted)
                countDown.Start();
                #endregion
                // ..and set first contact done.
                AppManager.I.Player.FirstContactPassed();
                anturaButton.SetActive(true);
                Debug.Log("First Contact Step1 finished! Go to Antura Space!");
            }
            else if (AppManager.I.Player.IsFirstContact(2))
            {
                // First contact step 2:

                // ..and set first contact done.
                AppManager.I.Player.FirstContactPassed(2);
                Debug.Log("First Contact Step2 finished! Good Luck!");
            }

        }
        #region Temp Behaviour (to be deleted)
        CountdownTimer countDown = new CountdownTimer(1f);
        void OnEnable() { countDown.onTimesUp += CountDown_onTimesUp; }
        void OnDisable() { countDown.onTimesUp -= CountDown_onTimesUp; }
        private void CountDown_onTimesUp() { NavigationManager.I.GoToScene(AppScene.Rewards); }
        private void UpdateTimer() { countDown.Update(Time.deltaTime); }
        #endregion

        #endregion


        public void StageLeft()
        {
            int numberStage = AppManager.I.Player.CurrentJourneyPosition.Stage;
            if ((numberStage < s) && (!inTransition))
            {
                previousStage = numberStage;
                inTransition = true;
                stages[numberStage + 1].SetActive(true);
                ChangeCamera(cameras[numberStage + 1]);

                ChangePinDotToBlack();
                AppManager.I.Player.CurrentJourneyPosition.Stage++;
                ChangeCameraFogColor(AppManager.I.Player.CurrentJourneyPosition.Stage);
                letter.GetComponent<LetterMovement>().miniMapScript = miniMaps[numberStage + 1].GetComponent<MiniMap>();
                letter.GetComponent<LetterMovement>().ResetPosLetterAfterChangeStage();

                FirstOrLastMap();

                StartCoroutine("DesactivateMap");
            }
        }
        public void StageRight()
        {
            int numberStage = AppManager.I.Player.CurrentJourneyPosition.Stage;
            if ((numberStage > 1)&& (!inTransition))
            {
                previousStage = numberStage;
                inTransition = true;
                stages[numberStage - 1].SetActive(true);
                ChangeCamera(cameras[numberStage - 1]);

                ChangePinDotToBlack();
                AppManager.I.Player.CurrentJourneyPosition.Stage--;
                ChangeCameraFogColor(AppManager.I.Player.CurrentJourneyPosition.Stage);
                letter.GetComponent<LetterMovement>().miniMapScript = miniMaps[numberStage - 1].GetComponent<MiniMap>();
                letter.GetComponent<LetterMovement>().ResetPosLetterAfterChangeStage();

                FirstOrLastMap();

                StartCoroutine("DesactivateMap");
            }
        }
        public void ChangeCamera(GameObject ZoomCameraGO)
        {

            CameraGameplayController.I.MoveToPosition(ZoomCameraGO.transform.position, ZoomCameraGO.transform.rotation, 0.6f);

        }
        IEnumerator ResetPosLetter()
        {
            yield return new WaitForSeconds(0.2f);
            letter.GetComponent<LetterMovement>().ResetPosLetter();
            letter.SetActive(true);
            CameraGameplayController.I.transform.position = cameras[AppManager.I.Player.CurrentJourneyPosition.Stage].transform.position;
        }
        void ChangePinDotToBlack()
        {
            if (AppManager.I.Player.CurrentJourneyPosition.PlaySession == 100)//change color pin to black
            {
                letter.GetComponent<LetterMovement>().miniMapScript.posPines[AppManager.I.Player.CurrentJourneyPosition.LearningBlock].GetComponent<MapPin>().ChangeMaterialPinToBlack();
                letter.GetComponent<LetterMovement>().miniMapScript.posPines[AppManager.I.Player.CurrentJourneyPosition.LearningBlock].transform.GetChild(0).gameObject.SetActive(true); //activate pin
            }
            else
                letter.GetComponent<LetterMovement>().ChangeMaterialDotToBlack(letter.GetComponent<LetterMovement>().miniMapScript.posDots[letter.GetComponent<LetterMovement>().pos]);
        }
        void ChangeCameraFogColor(int c)
        {
            Camera.main.DOColor(colorMaps[c], 1);
            Camera.main.GetComponent<CameraFog>().color = colorMaps[c];
        }
        IEnumerator DesactivateMap()
        {
            yield return new WaitForSeconds(0.8f);
            stages[previousStage].SetActive(false);
            inTransition = false;
        }
        void FirstOrLastMap()
        {
            if (AppManager.I.Player.CurrentJourneyPosition.Stage == 1)
                rightStageButton.SetActive(false);
            else if (AppManager.I.Player.CurrentJourneyPosition.Stage == 6)
                leftStageButton.SetActive(false);
            else
            {
                rightStageButton.SetActive(true);
                leftStageButton.SetActive(true);
            }

        }
        void DesactivateUI()
        {
            leftStageButton.SetActive(false);
            rightStageButton.SetActive(false);
            rightMovementButton.SetActive(false);
            leftMovementButton.SetActive(false);
            playButton.SetActive(false);
            bookButton.SetActive(false);
            anturaButton.SetActive(false);
        }
        void ActivateUI()
        {
            leftStageButton.SetActive(true);
            rightStageButton.SetActive(true);
            rightMovementButton.SetActive(true);
            leftMovementButton.SetActive(true);
            playButton.SetActive(true);
            bookButton.SetActive(true);
            anturaButton.SetActive(true);
        }

        /*void DesactivateMap()
        {
            stages[previousStage].SetActive(false);
            inTransition = false;
        }*/


    }
}