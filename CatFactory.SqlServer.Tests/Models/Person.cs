using System;

namespace CatFactory.SqlServer.Tests.Models
{
    public class Person
    {
        public int? PersonId { get; set; }

        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string FamilyName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Gender { get; set; }
    }
}
