﻿using ArabicSupport;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace EA4S.Egg
{
    public class EggButton : MonoBehaviour
    {
        public TextMeshProUGUI buttonText;
        public Image buttonImage;
        public Button button;

        public Color colorStandard;
        public Color colorLightUp;

        Tween colorTweener;

        public ILivingLetterData livingLetterData { get; private set; }

        Action<ILivingLetterData> onButtonPressed;

        public int positionIndex { get; set; }

        Action playButtonAudioCallback;
        IAudioManager audioManager;

        bool inputEnabled = false;

        Action endMoveCallback;
        Action startMoveCallback;
        Action endScaleCallback;

        Tween scaleTweener;
        Sequence moveSequence;

        public void Initialize(IAudioManager audioManager, Action<ILivingLetterData> onButtonPressed)
        {
            button.onClick.AddListener(OnButtonPressed);

            this.audioManager = audioManager;
            this.onButtonPressed = onButtonPressed;
        }

        public void SetAnswer(ILivingLetterData livingLetterData)
        {
            this.livingLetterData = livingLetterData;

            if (livingLetterData.DataType == LivingLetterDataType.Letter)
            {
                buttonText.gameObject.SetActive(true);

                buttonText.text = ArabicAlphabetHelper.GetLetterFromUnicode(((LL_LetterData)livingLetterData).Data.Isolated_Unicode);
            }
            else if (livingLetterData.DataType == LivingLetterDataType.Letter)
            {
                buttonText.gameObject.SetActive(true);

                buttonText.text = ArabicFixer.Fix(((LL_WordData)livingLetterData).Data.Arabic, false, false);
            }
        }

        public float PlayButtonAudio(bool lightUp, float delay = 0f, Action callback = null)
        {
            playButtonAudioCallback = callback;

            IAudioSource audioSource = audioManager.PlayLetterData(livingLetterData);
            audioSource.Stop();

            float duration = audioSource.Duration;

            if (colorTweener != null)
                colorTweener.Kill();

            Color newColor = lightUp ? colorLightUp : colorStandard;

            colorTweener = DOTween.To(() => buttonImage.color, x => buttonImage.color = x, newColor, duration / 2f).OnComplete(delegate ()
            {
                colorTweener = DOTween.To(() => buttonImage.color, x => buttonImage.color = x, colorStandard, duration / 2f).OnComplete(delegate ()
                {
                    if (playButtonAudioCallback != null)
                    {
                        playButtonAudioCallback();
                    }
                });
            }).OnStart(delegate ()
            {
                audioSource = audioManager.PlayLetterData(livingLetterData);
            }).SetDelay(delay);

            return duration;
        }

        void OnButtonPressed()
        {
            if (inputEnabled)
            {
                ChangeColorOnButtonPressed();

                if (onButtonPressed != null)
                {
                    onButtonPressed(livingLetterData);
                }
            }
        }

        void ChangeColorOnButtonPressed()
        {
            if (colorTweener != null)
                colorTweener.Kill();

            buttonImage.color = colorLightUp;

            colorTweener = DOTween.To(() => buttonImage.color, x => buttonImage.color = x, colorStandard, 1f);
        }

        public void SetOnPressedColor()
        {
            if (colorTweener != null)
                colorTweener.Kill();

            buttonImage.color = colorLightUp;
        }

        public void SetOnStandardColor()
        {
            if (colorTweener != null)
                colorTweener.Kill();

            buttonImage.color = colorStandard;
        }

        public void EnableInput()
        {
            inputEnabled = true;
        }

        public void DisableInput()
        {
            inputEnabled = false;
        }

        public void ScaleTo(float scale, float duration, float delay = 0f, Action endCallback = null)
        {
            endScaleCallback = endCallback;

            if (scaleTweener != null)
            {
                scaleTweener.Kill();
            }

            scaleTweener = transform.DOScale(scale, duration).SetDelay(delay).OnComplete(delegate () { if (endScaleCallback != null) endScaleCallback(); });
        }

        public void MoveTo(Vector3 position, float duration, AnimationCurve animationCurve, float delay = 0f, float delayFromStart = 0f, Action startCallback = null, Action endCallback = null)
        {
            endMoveCallback = endCallback;
            startMoveCallback = startCallback;

            if (moveSequence != null)
            {
                moveSequence.Kill();
            }

            moveSequence = DOTween.Sequence();

            moveSequence.AppendInterval(delay);

            if (startMoveCallback != null)
            {
                moveSequence.AppendCallback(delegate () { if (startMoveCallback != null) startMoveCallback(); });
                moveSequence.AppendInterval(delayFromStart);
                moveSequence.Append(transform.DOLocalJump(position, 100f, 1, duration));
            }
            else
            {
                moveSequence.Append(transform.DOLocalMove(position, duration).SetEase(animationCurve));
            }

            moveSequence.OnComplete(delegate () { if (endMoveCallback != null) endMoveCallback(); });

            moveSequence.Play();
        }

        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }
    }
}