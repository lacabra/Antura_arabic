﻿using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace EA4S {

    public class AnturaModelManager : MonoBehaviour {

        public static AnturaModelManager Instance;

        [Header("Bones Attach")]
        public Transform Dog_head;
        public Transform Dog_spine01;
        public Transform Dog_jaw;
        public Transform Dog_Tail3;
        public Transform Dog_R_ear04;
        public Transform Dog_L_ear04;

        /// <summary>
        /// The dog head pointer
        /// </summary>
        Transform Dog_head_pointer, Dog_spine01_pointer, Dog_jaw_pointer, Dog_Tail3_pointer, dog_R_ear04_pointer, dog_L_ear04_pointer;

        /// <summary>
        /// Pointer to transform used as parent for add reward model (and remove if already mounted yet).
        /// </summary>
        [HideInInspector]
        public Transform transformParent;

        #region Life cycle

        void Awake() {
            Instance = this;
            chargeCategoryList();
        }

        #endregion



        #region Rewards

        /// <summary>
        /// The category list
        /// </summary>
        List<string> categoryList = new List<string>();
        /// <summary>
        /// Charges the category list.
        /// </summary>
        void chargeCategoryList() {
            foreach (var reward in RewardSystemManager.GetConfig().Antura_rewards) {
                if (!categoryList.Contains(reward.Category))
                    categoryList.Add(reward.Category);
            } 
        }

        /// <summary>
        /// The actual rewards for place positions.
        /// </summary>
        List<Reward> actualRewardsForCategory = new List<Reward>();

        /// <summary>
        /// Adds reward to active rewards list and if already exist reward for same category substitute it.
        /// </summary>
        /// <param name="_reward">The reward.</param>
        void AddRewardActiveRewardsList(Reward _reward) {
            Reward oldRewardInList = actualRewardsForCategory.Find(r => r.Category == _reward.Category);
            if (oldRewardInList != null) {
                actualRewardsForCategory.Remove(oldRewardInList);
            }
            actualRewardsForCategory.Add(_reward);
        }

        #endregion

        #region API

        /// <summary>
        /// Loads the reward on model.
        /// </summary>
        /// <param name="_id">The identifier.</param>
        /// <returns></returns>
        public GameObject LoadRewardOnAntura(string _id) {
            Reward reward = RewardSystemManager.GetConfig().Antura_rewards.Find(r => r.ID == _id);
            if (reward == null) {
                Debug.LogFormat("Reward {0} not found!", _id);
                return null;
            }
            AddRewardActiveRewardsList(reward);
            string boneParent = reward.BoneAttach;
            Transform transformParent = transform;
            GameObject rewardModel = null;
            switch (boneParent) {
                case "dog_head":
                    transformParent = Dog_head;
                    if (Dog_head_pointer)
                        Destroy(Dog_head_pointer.gameObject);
                    Dog_head_pointer = ModelsManager.MountModel(reward.ID, transformParent).transform;
                    break;
                case "dog_spine01":
                    transformParent = Dog_spine01;
                    if (Dog_spine01_pointer)
                        Destroy(Dog_spine01_pointer.gameObject);
                    Dog_spine01_pointer = ModelsManager.MountModel(reward.ID, transformParent).transform;
                    break;
                case "dog_jaw":
                    transformParent = Dog_jaw;
                    if (Dog_jaw_pointer)
                        Destroy(Dog_jaw_pointer.gameObject);
                    Dog_jaw_pointer = ModelsManager.MountModel(reward.ID, transformParent).transform;
                    break;
                case "dog_Tail4":
                    transformParent = Dog_Tail3;
                    if (Dog_Tail3_pointer)
                        Destroy(Dog_Tail3_pointer.gameObject);
                    Dog_Tail3_pointer = ModelsManager.MountModel(reward.ID, transformParent).transform;
                    break;
                case "dog_R_ear04":
                    transformParent = Dog_R_ear04;
                    if (dog_R_ear04_pointer)
                        Destroy(dog_R_ear04_pointer.gameObject);
                    dog_R_ear04_pointer = ModelsManager.MountModel(reward.ID, transformParent).transform;
                    break;
                case "dog_L_ear04":
                    transformParent = Dog_L_ear04;
                    if (dog_L_ear04_pointer)
                        Destroy(dog_L_ear04_pointer.gameObject);
                    dog_L_ear04_pointer = ModelsManager.MountModel(reward.ID, transformParent).transform;
                    break;
                default:
                    break;
            }
            return rewardModel;
        }


        
        #endregion
    }

    #region reward structures

    [Serializable]
    public class RewardConfig {
        public List<Reward> Antura_rewards;
    }

    [Serializable]
    public class Reward {
        //public string Type;
        public string ID;
        public string RewardName;
        //public string Priority;
        //public string Done;
        public string BoneAttach;
        public string Material1;
        public string Material2;
        public string Category;
        public string RemTongue;
    }

    #endregion
}