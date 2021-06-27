namespace Eventive.ApplicationLogic.Common
{
    public class Constants
    {
        public const double TrendingApplicationWeight = 3;
        public const double TrendingFollowWeight = 2;
        public const double TrendingCommentWeight = 1.5;
        public const double TrendingClickWeight = 0.5;
        public const double TrendingGravity = 0.05;
        public const int NumberOfTrendingEventsShown = 4;

        public const double ProximityScoreMaximumDistanceInMeters = 1000000;
        public const double RecommendationRatingWeight = 5;
        public const double RecommendationApplicationWeight = 3;
        public const double RecommendationFollowWeight = 1;
        public const double RecommendationClickWeight = 0.1;

        public const double RecommendationCategoryWeight = 5;
        public const double RecommendationProximityWeight = 5;
        public const int NumberOfRecommendedEventsShown = 8;
    }
}
