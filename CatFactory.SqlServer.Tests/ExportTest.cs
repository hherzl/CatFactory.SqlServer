using CatFactory.SqlServer.CodeFactory;
using CatFactory.SqlServer.ObjectRelationalMapping;
using CatFactory.SqlServer.Tests.Models;
using Xunit;

namespace CatFactory.SqlServer.Tests;

public class ExportTest
{
    [Fact]
    public void ExportBloggingScript()
    {
        // Arrange
        var database = Databases.Blogging;

        // Act
        SqlServerDatabaseScriptCodeBuilder.CreateScript(database, @"C:\Temp\CatFactory.SqlServer", true, true);

        // Assert
    }

    [Fact]
    public void ExportCollegeScript()
    {
        // Arrange
        var database = SqlServerDatabase.CreateWithDefaults("College");

        database.AddDefaultTypeMapFor(typeof(string), "nvarchar");

        var student = database
            .DefineEntity(new { Id = 0, GivenName = "", MiddleName = "", FamilyName = "" })
            .SetNaming("Student")
            .SetColumnFor(p => p.GivenName, 15)
            .SetColumnFor(p => p.MiddleName, 15, true)
            .SetColumnFor(p => p.FamilyName, 15)
            .SetIdentity(p => p.Id)
            .SetPrimaryKey(e => e.Id)
            ;

        student
            .AddExtendedProp(p => p.GivenName, "MS_Description", "Given name")
            .AddExtendedProp(p => p.MiddleName, "MS_Description", "Middle name")
            .AddExtendedProp(p => p.FamilyName, "MS_Description", "Family name")
            ;

        student.Data.Add(new { Id = 0, GivenName = "Carlo", MiddleName = "H", FamilyName = "Herzl" });

        // TODO: add sql transcriber => translate results from select to code (c#)

        var course = database
            .DefineEntity(new { Id = 0, Name = "", Description = "" })
            .SetNaming("Course")
            .SetColumnFor(e => e.Name, 255)
            .SetIdentity(e => e.Id, 1000, 1000)
            .SetPrimaryKey(e => e.Id)
            .AddUnique(e => e.Name)
            ;

        var courseStudent = database
            .DefineEntity(new
            {
                Id = 0,
                CourseId = 0,
                StudentId = 0
            })
            .SetNaming("CourseStudent")
            .SetIdentity(p => p.Id)
            .SetPrimaryKey(p => p.Id)
            .AddUnique(p => new { p.CourseId, p.StudentId })
            .AddForeignKey(p => p.CourseId, course.Table)
            .AddForeignKey(p => p.StudentId, student.Table)
            ;

        // Act

        SqlServerDatabaseScriptCodeBuilder.CreateScript(database, @"C:\Temp\CatFactory.SqlServer", true, true);

        // Assert
    }

    [Fact]
    public void ExportDefinitionForRothschildHouseEntities()
    {
        // Arrange
        var database = SqlServerDatabase.CreateWithDefaults("RothschildHouse");

        database.ExtendedProperties.Add(new("MS_Description", "Database to storage RothschildHouse payments"));

        database.AddDefaultTypeMapFor(typeof(string), "nvarchar");

        var person = database
            .DefineEntity(new
            {
                PersonId = Guid.Empty,
                GivenName = "",
                MiddleName = "",
                FamilyName = "",
                FullName = "",
                BirthDate = DateTime.Now
            })
            .SetNaming("Person", "People")
            .SetColumnFor(p => p.GivenName, 15)
            .SetColumnFor(p => p.MiddleName, 15, true)
            .SetColumnFor(p => p.FamilyName, 15)
            .SetColumnFor(p => p.FullName, 45)
            .SetPrimaryKey(p => p.PersonId)
            ;

        person
            .AddExtendedProp("MS_Description", "Person catalog")
            .AddExtendedProp(p => p.GivenName, "MS_Description", "Given name")
            .AddExtendedProp(p => p.MiddleName, "MS_Description", "Middle name")
            .AddExtendedProp(p => p.FamilyName, "MS_Description", "Family name")
            .AddExtendedProp(p => p.FullName, "MS_Description", "Full name")
            .AddExtendedProp(p => p.BirthDate, "MS_Description", "Birth date")
            ;

        var creditCard = database
            .DefineEntity(new
            {
                CreditCardId = Guid.Empty,
                PersonId = Guid.Empty,
                CardType = "",
                CardNumber = "",
                Last4Digits = "",
                ExpirationDate = DateTime.Now,
                Cvv = ""
            })
            .SetNaming("CreditCard", "Payment")
            .SetColumnFor(p => p.CardType, 20)
            .SetColumnFor(p => p.CardNumber, 20)
            .SetColumnFor(p => p.Last4Digits, 4)
            .SetColumnFor(p => p.Cvv, 4)
            .SetPrimaryKey(p => p.CreditCardId)
            .AddUnique(p => p.CardNumber)
            .AddForeignKey(p => p.PersonId, person.Table)
            ;

        creditCard
            .AddExtendedProp(p => p.PersonId, "MS_Description", "Person Identifier")
            .AddExtendedProp(p => p.CardType, "MS_Description", "Card type")
            .AddExtendedProp(p => p.CardNumber, "MS_Description", "Card number")
            .AddExtendedProp(p => p.Last4Digits, "MS_Description", "Last 4 Digits")
            .AddExtendedProp(p => p.ExpirationDate, "MS_Description", "Expiration Date")
            .AddExtendedProp(p => p.Cvv, "MS_Description", "Card Verification Value")
            ;

        var paymentTransaction = database
            .DefineEntity(new
            {
                PaymentTransactionId = Guid.Empty,
                CreditCardId = Guid.Empty,
                ConfirmationId = Guid.Empty,
                Amount = 0m,
                CreatedOn = DateTime.Now
            })
            .SetNaming("PaymentTransaction", "Payment")
            .SetColumnFor(p => p.Amount, 10, 4)
            .SetPrimaryKey(p => p.CreditCardId)
            .AddForeignKey(p => p.CreditCardId, creditCard.Table)
            .AddDefault(p => p.CreatedOn, "GETDATE()")
            .AddCheck(p => p.Amount, "Amount > 0")
            ;

        paymentTransaction
            .AddExtendedProp(p => p.PaymentTransactionId, "MS_Description", "Payment Transaction Identifier")
            .AddExtendedProp(p => p.CreditCardId, "MS_Description", "Credit Card Identifier")
            .AddExtendedProp(p => p.ConfirmationId, "MS_Description", "Confirmation Identifier")
            .AddExtendedProp(p => p.Amount, "MS_Description", "Transaction Amount")
            .AddExtendedProp(p => p.CreatedOn, "MS_Description", "Payment Date time")
            ;

        // Act

        SqlServerDatabaseScriptCodeBuilder.CreateScript(database, @"C:\Temp\CatFactory.SqlServer", true, true);
    }
}
