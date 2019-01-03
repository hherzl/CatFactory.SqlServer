using System.Collections.Generic;

namespace CatFactory.SqlServer.Tests.Models
{
    public static class Mock
    {
        public static IEnumerable<string> Genders
            => new List<string>
            {
                "M", "F"
            };

        public static IEnumerable<string> MaleGivenNames
            => new List<string>
            {
                "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas"
            };

        public static IEnumerable<string> MaleMiddleNames
            => new List<string>
            {
                "", "Lee", "Alexander", "Edward", "Alphonse"
            };

        public static IEnumerable<string> FemaleGivenNames
            => new List<string>
            {
                "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Susan", "Margaret", "Lisa", "Nancy"
            };

        public static IEnumerable<string> FemaleMiddleNames
            => new List<string>
            {
                "", "Ann", "Bella", "Catherine", "Claire", "Ellen"
            };

        public static IEnumerable<string> FamilyNames
            => new List<string>
            {
                "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson"
            };
    }
}
