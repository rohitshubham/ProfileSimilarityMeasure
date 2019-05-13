using System;
using PropertyInfo = System.Reflection.PropertyInfo;
using SimilarityApp.Helpers;
using System.Collections.Generic;

namespace SimilarityApp.Services
{
    [Flags]
    public enum ExcludeTypeComparision { Name , Hobbies }

    //ToDO should we make it a thread safe singleton ?
    public class ProfileSimilarityService
    {
        public readonly Dictionary<string, int> NormalDistributionMeanValuesForProfileMeasures = new Dictionary<string, int>();

        public ProfileSimilarityService()
        {
            NormalDistributionMeanValuesForProfileMeasures.Add("Age", 15);
            NormalDistributionMeanValuesForProfileMeasures.Add("Income", 50000);
        }

        public void GetProfileSimilarity()
        {
            var profile_1 = new Profile();
            var profile_2 = new Profile();
            profile_1.Age = 23;
            profile_1.Gender = "Male";
            profile_1.Name = "Rohit Raj";
            profile_1.Income = 100000;
            profile_2.Age = 38;
            profile_2.Gender = "Male";
            profile_2.Name = "Rohit Raj";
            profile_2.Income = 110000;
            GetPrimitiveProfileProptiesSimilarity(profile_1, profile_2);
        }

        //Gets the profile similarity on all the properties except hobbies.
        private void GetPrimitiveProfileProptiesSimilarity(Profile sourceProfile, Profile destinationProfile)
        {
            Type sourceType = sourceProfile.GetType();
            Type destinationType = destinationProfile.GetType();

            if (sourceType == destinationType)
            {
                try
                {
                    PropertyInfo[] sourceProperties = sourceType.GetProperties();
                    foreach (PropertyInfo pi in sourceProperties)
                    {
                        if ((sourceType.GetProperty(pi.Name).GetValue(sourceProfile, null) == null && destinationType.GetProperty(pi.Name).GetValue(destinationProfile, null) == null))
                        {
                            // if both are null, don't try to compare  (throws exception!)
                            continue;
                        }
                        if (typeof(string).IsAssignableFrom(pi.PropertyType) && !Enum.IsDefined(enumType: typeof(ExcludeTypeComparision), value: pi.Name))
                        {
                            var sourcePropertyValue = sourceType.GetProperty(pi.Name).GetValue(sourceProfile, null) as string;
                            var destinationPropertyValue = destinationType.GetProperty(pi.Name).GetValue(destinationProfile, null) as string;
                            //ToDo Use levenstein distance
                            if (String.Equals(sourcePropertyValue, destinationPropertyValue, StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine($"{pi.Name} matches 100%");
                            }
                        }
                        if (typeof(int?).IsAssignableFrom(pi.PropertyType) && !Enum.IsDefined(enumType: typeof(ExcludeTypeComparision), value: pi.Name))
                        {
                            var sourcePropertyValue = (int)sourceType.GetProperty(pi.Name).GetValue(sourceProfile, null);
                            var destinationPropertyValue = (int)destinationType.GetProperty(pi.Name).GetValue(destinationProfile, null);

                            //Get the value of Normalized mean for this property if exists
                            NormalDistributionMeanValuesForProfileMeasures.TryGetValue(pi.Name, out int meanValue);


                            var percentMatch = NormalizedScoreHelper.GetNormalizedValue(sourcePropertyValue, destinationPropertyValue, meanValue);
                            Console.WriteLine($"{pi.Name} matches {percentMatch}%");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                throw new ArgumentException("Comparison object must be of the same type.", "comparisonObject");
            }

            //return true;
        }
    }
}
