﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EA4S.Db;

namespace EA4S.Teacher
{
    public class LogIntelligence
    {
        // References
        DatabaseManager db;

        public LogIntelligence(DatabaseManager db)
        {
            this.db = db;
        }

        #region Mood

        public void LogMood(int mood)
        {
            float realMood = Mathf.InverseLerp(ConfigAI.minimumMoodValue, ConfigAI.maximumMoodValue, mood);
            var data = new LogMoodData(realMood);
            db.Insert(data);
        }

        #endregion

        #region Info

        public void LogInfo(string session, InfoEvent infoEvent, string parametersString = "")
        {
            var data = new LogInfoData(session, infoEvent, parametersString);
            db.Insert(data);
        }

        #endregion

        #region Play

        /// <summary>
        /// Parameters for the results of a play session related to a specific skill.
        /// </summary>
        public struct PlayResultParameters
        {
            public PlayEvent playEvent;
            public PlaySkill skill;
            public float score;
        }

        public void LogPlay(string session, string playSession, MiniGameCode miniGameCode, List<PlayResultParameters> resultsList)
        {
            // The teacher receives a score for each play skill the minigame deems worthy of analysis
            foreach (var result in resultsList)
            {
                var data = new LogPlayData(session, playSession, miniGameCode, result.playEvent, result.skill, result.score);
                db.Insert(data);
            }
        }

        #endregion

        #region Learn

        /// <summary>
        /// General parameters used to define the learning result for each minigame instance
        /// </summary>
        public struct LearnResultParameters
        {
            public DbTables table;
            public string elementId;
            public int nCorrect;
            public int nWrong;
        }

        /// <summary>
        /// Specific rules per mini game
        /// </summary>
        public class MiniGameLearnRules
        {
            public enum VoteLogic
            {
                Threshold,
                SuccessRatio
            }

            public VoteLogic voteLogic;
            public float logicParameter;                // for example, success threshold 
            public float minigameVoteSkewOffset;        // takes into account that some minigames are skewed
            public float minigameImportanceWeight;      // takes into account that some minigames are more important on learning in respect to others

            public MiniGameLearnRules()
            {
                voteLogic = VoteLogic.SuccessRatio;
                logicParameter = 0f;
                minigameVoteSkewOffset = 0f;
                minigameImportanceWeight = 1f;
            }
        }

        public void LogLearn(string session, string playSession, MiniGameCode miniGameCode, List<LearnResultParameters> resultsList)
        {
            var learnRules = GetLearnRules(miniGameCode);

            foreach (var result in resultsList)
            {
                float score = 0f;
                float successRatio = result.nCorrect * 1f / (result.nCorrect + result.nWrong);
                switch (learnRules.voteLogic)
                {
                    case MiniGameLearnRules.VoteLogic.Threshold:
                        // Uses a binary threshold
                        float threshold = learnRules.logicParameter;
                        score = successRatio > threshold ? 1f : -1f;
                        break;
                    case MiniGameLearnRules.VoteLogic.SuccessRatio:
                        // Uses directly the success ratio to drive the vote
                        score = Mathf.InverseLerp(-1f, 1f, successRatio);
                        break;
                }
                score *= learnRules.minigameImportanceWeight;
                score += learnRules.minigameVoteSkewOffset;

                var data = new LogLearnData(session, playSession, miniGameCode, result.table, result.elementId, score);
                db.Insert(data);

                // We also update the score for that data element
                UpdateScoreDataWithMovingAverage(result.table, result.elementId, score, 5);
            }
        }

        private MiniGameLearnRules GetLearnRules(MiniGameCode code)
        {
            MiniGameLearnRules rules = new MiniGameLearnRules();
            switch (code)
            {
                case MiniGameCode.Balloons_letter:  // @todo: set correct ones per each minigame
                    rules.voteLogic = MiniGameLearnRules.VoteLogic.Threshold;
                    rules.logicParameter = 0.5f;
                    break;

                default:
                    rules.voteLogic = MiniGameLearnRules.VoteLogic.SuccessRatio;
                    break;
            }
            return rules;
        }

        #endregion

        #region Journey Scores

        public void LogMiniGameScore(MiniGameCode miniGameCode, float score)
        {
            UpdateScoreDataWithMaximum(DbTables.MiniGames, ((int)miniGameCode).ToString(), score);
        }

        public void LogPlaySessionScore(string playSessionId, float score)
        {
            UpdateScoreDataWithMaximum(DbTables.PlaySessions, (playSessionId).ToString(), score);
        }

        public void LogLearningBlockScore(int learningBlock, float score)
        {
            UpdateScoreDataWithMaximum(DbTables.LearningBlocks, (learningBlock).ToString(), score);
        }

        #endregion

        #region Score Utilities

        private void UpdateScoreDataWithMaximum(DbTables table, string elementId, float newScore)
        {
            string query = string.Format("SELECT * FROM ScoreData WHERE TableName = '{0}' AND ElementId = '{1}'", table.ToString(), elementId);
            List<ScoreData> scoreDataList = db.FindScoreDataByQuery(query);
            float previousMaxScore = 0;
            if (scoreDataList.Count > 0)
            {
                previousMaxScore = scoreDataList[0].Score;
            }
            float newMaxScore = Mathf.Max(previousMaxScore, newScore);
            db.UpdateScoreData(table, elementId, newMaxScore);
        }

        private void UpdateScoreDataWithMovingAverage(DbTables table, string elementId, float newScore, int movingAverageSpan)
        {
            string query = string.Format("SELECT * FROM ScoreData WHERE TableName = '{0}' AND ElementId = '{1}'", table.ToString(), elementId);
            List<ScoreData> scoreDataList = db.FindScoreDataByQuery(query);
            float previousAverageScore = 0;
            if (scoreDataList.Count > 0)
            {
                previousAverageScore = scoreDataList[0].Score;
            }
            // @note: for the first movingAverageSpan values, this won't be accurate
            float newAverageScore = previousAverageScore - previousAverageScore / movingAverageSpan + newScore / movingAverageSpan;
            db.UpdateScoreData(table, elementId, newAverageScore);
        }

        #endregion

    }

}