namespace EA4S
{
    /// <summary>
    /// Add inside this class your TextIDs
    /// </summary>
    public class TextID
    {
        public static readonly TextID GAME_RESULT_RETRY = new TextID("game_result_retry");
        public static readonly TextID GAME_RESULT_FAIR = new TextID("game_result_fair");
        public static readonly TextID GAME_RESULT_GOOD = new TextID("game_result_good");
        public static readonly TextID GAME_RESULT_GREAT = new TextID("game_result_great");

        // Assessments
        public static readonly TextID ASSESSMENT_START_1 = new TextID("Assessment_Start_1");
        public static readonly TextID ASSESSMENT_START_2 = new TextID("Assessment_Start_2");
        public static readonly TextID ASSESSMENT_START_3 = new TextID("Assessment_Start_3");

        public static readonly TextID ASSESSSMENT_UPSET_1 = new TextID("Assessment_Upset_1");
        public static readonly TextID ASSESSSMENT_UPSET_2 = new TextID("Assessment_Upset_2");
        public static readonly TextID ASSESSSMENT_UPSET_3 = new TextID("Assessment_Upset_3");

        public static readonly TextID ASSESSSMENT_PUSH_1 = new TextID("Assessment_Push_Dog_1");
        public static readonly TextID ASSESSSMENT_PUSH_2 = new TextID("Assessment_Push_Dog_2");
        public static readonly TextID ASSESSSMENT_PUSH_3 = new TextID("Assessment_Push_Dog_3");

        public static readonly TextID ASSESSSMENT_DOG_GONE_1 = new TextID("Assessment_Dog_Gone_1");
        public static readonly TextID ASSESSSMENT_DOG_GONE_2 = new TextID("Assessment_Dog_Gone_2");
        public static readonly TextID ASSESSSMENT_DOG_GONE_3 = new TextID("Assessment_Dog_Gone_3");

        public static readonly TextID ASSESSSMENT_COMPLETE_1 = new TextID("Assessment_Complete_1");
        public static readonly TextID ASSESSSMENT_COMPLETE_2 = new TextID("Assessment_Complete_2");
        public static readonly TextID ASSESSSMENT_COMPLETE_3 = new TextID("Assessment_Complete_3");

        public static readonly TextID ASSESSSMENT_WRONG_1 = new TextID("Assessment_Wrong_1");
        public static readonly TextID ASSESSSMENT_WRONG_2 = new TextID("Assessment_Wrong_2");
        public static readonly TextID ASSESSSMENT_WRONG_3 = new TextID("Assessment_Wrong_3");

        /// <summary>
        /// Used to get a random TextID file among the given set
        /// </summary>
        /// <param name="ids">Set of TextID files</param>
        /// <returns>A random TextID</returns>
        public static TextID Random( params TextID[] ids)
        {
            return ids[UnityEngine.Random.Range( 0, ids.Length)];
        }

        
        public static readonly TextID ASSESSMENT_RESULT_INTRO = new TextID("assessment_result_intro");
        public static readonly TextID ASSESSMENT_RESULT_GOOD = new TextID("assessment_result_good");
        public static readonly TextID ASSESSMENT_START_A1 = new TextID("assessment_start_A1");
        public static readonly TextID ASSESSMENT_START_A2 = new TextID("assessment_start_A2");
        public static readonly TextID ASSESSMENT_START_A3 = new TextID("assessment_start_A3");

        // TODO: these files are unused currently
        //public static readonly TextID ASSESSMENT_RESULT_VERYGOOD = new TextID("assessment_result_verygood");
        //public static readonly TextID ASSESSMENT_RESULT_RETRY = new TextID("assessment_result_retry");

        public static readonly TextID WELL_DONE = new TextID("comment_welldone");
        public static readonly TextID TIMES_UP = new TextID("game_generic_timeup");

        // Egg
        
        public static readonly TextID EGG_TITLE = new TextID("Egg_Title");
        public static readonly TextID EGG_INTRO = new TextID("Egg_Intro");
        public static readonly TextID EGG_TUTO_BUTTON = new TextID("Egg_Tuto_Button");
        public static readonly TextID EGG_TUTO_SEQUENCE = new TextID("Egg_Tuto_Sequence");

        // ...

        public static TextID GetTextIDFromStars(int stars)
        {
            if (stars < 0)
                return GAME_RESULT_RETRY;

            switch (stars)
            {
                case 0:
                    return GAME_RESULT_RETRY;
                case 1:
                    return GAME_RESULT_FAIR;
                case 2:
                    return GAME_RESULT_GOOD;
                default:
                    return GAME_RESULT_GREAT;
            }
        }

        string id;

        /// <summary>
        /// Use TextID.{NAME} to provide text to other components
        /// </summary>
        private TextID(string id)
        {
            this.id = id;
        }

        public override string ToString()
        {
            return id;
        }
    }
}
