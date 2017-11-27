using Antura.LivingLetters;
using Antura.LivingLetters.Sample;
using Antura.Teacher;

namespace Antura.Minigames.FastCrowd
{
    public enum FastCrowdVariation
    {
        LetterInWord = MiniGameCode.FastCrowd_letterinword,
        Word = MiniGameCode.FastCrowd_word,
        Letter = MiniGameCode.FastCrowd_letter,
        LetterForm = MiniGameCode.FastCrowd_letterform,
        Counting = MiniGameCode.FastCrowd_counting,
        Alphabet = MiniGameCode.FastCrowd_alphabet
    }

    public class FastCrowdConfiguration : IGameConfiguration
    {
        // Game configuration
        public IGameContext Context { get; set; }
        public IQuestionProvider Questions { get; set; }

        #region Game configurations

        public float Difficulty { get; set; }
        public bool TutorialEnabled { get; set; }
        public FastCrowdVariation Variation { get; set; }

        public void SetMiniGameCode(MiniGameCode code)
        {
            Variation = (FastCrowdVariation)code;
        }

        #endregion


        /////////////////
        // Singleton Pattern
        static FastCrowdConfiguration instance;
        public static FastCrowdConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FastCrowdConfiguration();
                }
                return instance;
            }
        }
        /////////////////

        private FastCrowdConfiguration()
        {
            // Default values
            // THESE SETTINGS ARE FOR SAMPLE PURPOSES, THESE VALUES MUST BE SET BY GAME CORE

            Questions = new SampleQuestionProvider();
            //Variation = FastCrowdVariation.Letter;
            //Variation = FastCrowdVariation.Alphabet;
            Variation = FastCrowdVariation.LetterInWord;

            //Questions = new SampleQuestionWithWordsProvider();
            //Variation = FastCrowdVariation.Counting;

            //Questions = new SampleQuestionWordsVariationProvider();
            //Variation = FastCrowdVariation.Words;
            TutorialEnabled = true;

            Context = new MinigamesGameContext(MiniGameCode.FastCrowd_letterinword, System.DateTime.Now.Ticks.ToString());
            Difficulty = 0.5f;
        }

        #region external configuration call
        public static void SetConfiguration(float _difficulty, int _variation)
        {
            instance = new FastCrowdConfiguration()
            {
                Difficulty = _difficulty,
                Variation = (FastCrowdVariation)_variation,
            };
        }

        public IQuestionBuilder SetupBuilder()
        {
            IQuestionBuilder builder = null;

            int nPacks = 10;
            int nCorrect = 4;
            int nWrong = 4;

            var builderParams = new Teacher.QuestionBuilderParameters();

            switch (Variation)
            {
                case FastCrowdVariation.Alphabet:
                    builder = new AlphabetQuestionBuilder();
                    break;
                case FastCrowdVariation.Counting:
                    builder = new OrderedWordsQuestionBuilder(Database.WordDataCategory.Number, builderParams, true);
                    break;
                case FastCrowdVariation.Letter:
                    builder = new RandomLettersQuestionBuilder(nPacks, 1, nWrong, firstCorrectIsQuestion: true);
                    break;
                case FastCrowdVariation.LetterForm:
                    // TODO
                    throw new System.Exception("FastCrowdVariation.LetterForm to be done");
                    builder = new RandomLettersQuestionBuilder(nPacks, 1, nWrong, firstCorrectIsQuestion: true);
                    break;
                case FastCrowdVariation.LetterInWord:
                    builderParams.wordFilters.excludeColorWords = true;
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new LettersInWordQuestionBuilder(nPacks, nWrong: nWrong, useAllCorrectLetters: true, parameters: builderParams);
                    break;
                case FastCrowdVariation.Word:
                    builderParams.wordFilters.excludeColorWords = true;
                    builderParams.wordFilters.requireDrawings = true;
                    builder = new RandomWordsQuestionBuilder(nPacks, nCorrect, nWrong, parameters: builderParams);
                    break;
            }

            return builder;
        }

        public MiniGameLearnRules SetupLearnRules()
        {
            var rules = new MiniGameLearnRules();
            // example: a.minigameVoteSkewOffset = 1f;
            return rules;
        }

        #endregion
    }
}
