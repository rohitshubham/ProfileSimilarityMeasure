using System;
using SimilarityApp.Services;

namespace SimilarityApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // using anonymous type for getting profile similarity
            new ProfileSimilarityService().GetProfileSimilarity();
            Console.ReadLine();

        }
    }
}
