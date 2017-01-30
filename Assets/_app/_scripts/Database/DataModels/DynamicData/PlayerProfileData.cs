﻿using EA4S.Core;
using EA4S.Helpers;
using SQLite;

namespace EA4S.Database
{
    /// <summary>
    /// Serialized information about the player. Used by the Player Profile.
    /// </summary>
    [System.Serializable]
    public class PlayerProfileData : IData
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string PlayerKey { get; set; }
        public int PlayerId { get; set; }
        public int AvatarId { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public int ProfileCompletion { get; set; }
        public int TotalNumberOfBones { get; set; }
        public int CreationTimestamp { get; set; }
        public int MaxJourneyPosition_Stage { get; set; }
        public int MaxJourneyPosition_LearningBlock { get; set; }
        public int MaxJourneyPosition_PlaySession { get; set; }
        public int CurrentJourneyPosition_Stage { get; set; }
        public int CurrentJourneyPosition_LearningBlock { get; set; }
        public int CurrentJourneyPosition_PlaySession { get; set; }

        public PlayerProfileData()
        {
        }

        public PlayerProfileData(string _PlayerKey, int _PlayerId, int _AvatarId, int _Age, string _Name, int _TotalNumberOfBones)
        {
            Id = 1;  // Only one record
            PlayerKey = _PlayerKey;
            PlayerId = _PlayerId;
            AvatarId = _AvatarId;
            Age = _Age;
            Name = _Name;
            ProfileCompletion = 0;
            TotalNumberOfBones = _TotalNumberOfBones;
            SetMaxJourneyPosition(JourneyPosition.InitialJourneyPosition);
            SetCurrentJourneyPosition(JourneyPosition.InitialJourneyPosition);
            CreationTimestamp = GenericHelper.GetTimestampForNow();
        }

        #region Journey Position

        public void SetMaxJourneyPosition(JourneyPosition pos)
        {
            MaxJourneyPosition_Stage = pos.Stage;
            MaxJourneyPosition_LearningBlock = pos.LearningBlock;
            MaxJourneyPosition_PlaySession = pos.PlaySession;
        }

        public void SetCurrentJourneyPosition(JourneyPosition pos)
        {
            CurrentJourneyPosition_Stage = pos.Stage;
            CurrentJourneyPosition_LearningBlock = pos.LearningBlock;
            CurrentJourneyPosition_LearningBlock = pos.PlaySession;
        }

        public JourneyPosition GetMaxJourneyPosition()
        {
            return new JourneyPosition(MaxJourneyPosition_Stage, MaxJourneyPosition_LearningBlock, MaxJourneyPosition_PlaySession);
        }

        public JourneyPosition GetCurrentJourneyPosition()
        {
            return new JourneyPosition(CurrentJourneyPosition_Stage, CurrentJourneyPosition_LearningBlock, CurrentJourneyPosition_PlaySession);
        }

        #endregion

        #region Database API
        
        public string GetId()
        {
            return Id.ToString();
        }

        public override string ToString()
        {
            return string.Format("ID{0},P{1},Ts{2}",
                Id,
                PlayerId,
                CreationTimestamp
            );
        }

        #endregion

    }
}