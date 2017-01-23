﻿using System;
using EA4S.Audio;
using EA4S.MinigamesCommon;
using UnityEngine;

namespace EA4S.Scanner
{
	public class ScannerResultState : IGameState
	{
		ScannerGame game;

		float timer = 2;

		public ScannerResultState(ScannerGame game)
		{
			this.game = game;
		}

		public void EnterState()
		{
			AudioManager.I.PlayMusic(Music.Relax);
		}

		public void ExitState()
		{
		}

		public void Update(float delta)
		{
			timer -= delta;

			if (timer < 0)
			{
				game.EndGame(game.CurrentStars, game.CurrentScoreRecord);

                if (game.CurrentStars == 0)
                    AudioManager.I.PlayDialogue("Reward_0Star");
                else
                    AudioManager.I.PlayDialogue("Reward_" + game.CurrentStars + "Star_" + UnityEngine.Random.Range(1, 4));
            }
		}

		public void UpdatePhysics(float delta)
		{
		}
	}
}