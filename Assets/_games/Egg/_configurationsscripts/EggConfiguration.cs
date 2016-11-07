﻿namespace EA4S.Egg
{
    public class EggConfiguration : IGameConfiguration
    {
        // Game configuration
        public IGameContext Context { get; set; }
        public IQuestionProvider Questions { get; set; }
        public float Difficulty { get; set; }

        /////////////////
        // Singleton Pattern
        static EggConfiguration instance;
        public static EggConfiguration Instance
        {
            get
            {
                if (instance == null)
                    instance = new EggConfiguration();
                return instance;
            }
        }
        /////////////////

        private EggConfiguration()
        {
            // Default values
            // THESE SETTINGS ARE FOR SAMPLE PURPOSES, THESE VALUES MUST BE SET BY GAME CORE
            Context = new SampleGameContext();
            Difficulty = 0.1f;
            Questions = new SampleEggQuestionProvider(Difficulty);
        }

        public IQuestionBuilder SetupBuilder() {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nCorrect = 5;
            int nWrong = 0;

            builder = new RandomLettersQuestionBuilder(nPacks, nCorrect, nWrong);

            return builder;
        }
    }
}