using CatFactory.Markdown;
using CatFactory.SqlServer.Mocking;
using CatFactory.SqlServer.Tests.Models;
using Xunit;

namespace CatFactory.SqlServer.Tests;

public class MockingTests
{
    private int counter = 0;

    public int GetId()
    {
        counter++;

        return counter;
    }

    public DateTime GetBirthDate()
    {
        var date = DateTime.Now;
        var random = new Random();

        date = date.AddYears(random.Next(-40, -21));
        date = date.AddMonths(random.Next(-12, -1));
        date = date.AddDays(random.Next(-365, -1));
        date = date.AddHours(random.Next(-12, -1));
        date = date.AddMinutes(random.Next(-60, -1));
        date = date.AddSeconds(random.Next(-60, -1));

        return date;
    }

    [Fact]
    public void TestMockingPeopleFromClass()
    {
        var mocker = new EntityMocker<Person>();
        var count = 20;

        mocker
            .MockProperty(p => p.PersonId, GetId)
            .MockProperty(p => p.Gender, PersonMocks.Genders)
                .When("M", p => p.GivenName, PersonMocks.MaleGivenNames)
                .When("M", p => p.MiddleName, PersonMocks.MaleMiddleNames)
                .When("F", p => p.GivenName, PersonMocks.FemaleGivenNames)
                .When("F", p => p.MiddleName, PersonMocks.FemaleMiddleNames)
            .MockProperty(p => p.FamilyName, PersonMocks.FamilyNames)
            .MockProperty(p => p.BirthDate, GetBirthDate);

        var people = mocker
            .CreateMocks(count)
            .ToList()
            .OrderBy(item => item.Gender)
            .ThenBy(item => item.GivenName)
            .ThenBy(item => item.MiddleName)
            .ThenBy(item => item.FamilyName);

        var readme = new MdDocument();

        readme.H1("People");

        readme.WriteLine("This file contains people from mocking action!:");

        var table = new MdTable
        {
            Header = new MdTableHeader("ID", "Given Name", "Middle Name", "Family Name", "Gender", "Birth Date")
        };

        foreach (var person in people)
            table.Rows.Add(new MdTableRow(person.PersonId.ToString(), person.GivenName, person.MiddleName, person.FamilyName, person.Gender, person.BirthDate.ToString()));

        readme.Write(table);

        File.WriteAllText(@"C:\Temp\CatFactory.SqlServer\people.entitymocker.md", readme.ToString());

        Assert.True(people.Count() == count);
    }

    // todo: Fix this test
    //[Fact]
    //public void TestMockingPeopleFromAnonymous()
    //{
    //    var mocker = EntityMocker.Create(new
    //    {
    //        PersonId = default(int?),
    //        GivenName = "",
    //        MiddleName = "",
    //        FamilyName = "",
    //        BirthDate = default(DateTime?),
    //        Gender = ""
    //    });

    //    var count = 20;

    //    mocker
    //        .MockProperty(p => p.PersonId, GetId)
    //        .MockProperty(p => p.Gender, PersonMocks.Genders)
    //            .When("M", p => p.GivenName, PersonMocks.MaleGivenNames)
    //            .When("M", p => p.MiddleName, PersonMocks.MaleMiddleNames)
    //            .When("F", p => p.GivenName, PersonMocks.FemaleGivenNames)
    //            .When("F", p => p.MiddleName, PersonMocks.FemaleMiddleNames)
    //        .MockProperty(p => p.FamilyName, PersonMocks.FamilyNames)
    //        .MockProperty(p => p.BirthDate, GetBirthDate);

    //    var people = mocker
    //        .CreateAnonymousMocks(count)
    //        .ToList()
    //        .OrderBy(item => item.Gender)
    //        .ThenBy(item => item.GivenName)
    //        .ThenBy(item => item.MiddleName)
    //        .ThenBy(item => item.FamilyName);

    //    var readme = new MdDocument();

    //    readme.H1("People");

    //    readme.WriteLine("This file contains people from mocking action!:");

    //    var table = new MdTable
    //    {
    //        Header = new MdTableHeader("ID", "Given Name", "Middle Name", "Family Name", "Gender", "Birth Date")
    //    };

    //    foreach (var person in people)
    //        table.Rows.Add(new MdTableRow(person.PersonId.ToString(), person.GivenName, person.MiddleName, person.FamilyName, person.Gender, person.BirthDate.ToString()));

    //    readme.Write(table);

    //    File.WriteAllText(@"C:\Temp\CatFactory.SqlServer\anonymous.entitymocker.md", readme.ToString());

    //    Assert.True(people.Count() == count);
    //}
}
