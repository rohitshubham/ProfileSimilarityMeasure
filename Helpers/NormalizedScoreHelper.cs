using System;

namespace SimilarityApp.Helpers
{
    public static class NormalizedScoreHelper {

        private const int rectangles = 100000; // more rectangles = more precise, less rectangles = quicker execution

        public static double GetNormalizedValue(int value1, int value2, int mean = 15) // We take 15 as a deafult mean value
        {
            double z1 = ((double)value2 - (double)value1) / mean;
            double z2 = 0.0;

            var normalizedValue = Math.Abs(GetAreaUnderNormalCurve(z1, z2));

            //Convert to Percentage and inverse the area as we need the similarity
            var percentArea = (1.0 - 2 * normalizedValue) * 100;

            return Math.Round(percentArea, 2);
        }

        private static double GetAreaUnderNormalCurve(double z1, double z2)
        {
            double area = 0.0;            
            double width = (z2 - z1) / rectangles;

            for (int i = 0; i < rectangles; i++)
                area += width * GetNormalProbabilityAtZ(width * i + z1);
            return area;
        }

        /* 
         Returns the height of the normal distribution at the specified z-score
        */
        private static double GetNormalProbabilityAtZ(double z)
        {
            return Math.Exp(-Math.Pow(z, 2) / 2) / Math.Sqrt(2 * Math.PI);
        }
    }
}