using System;
using CatFactory.SqlServer.CodeFactory;
using CatFactory.SqlServer.DocumentObjectModel;
using CatFactory.SqlServer.ObjectRelationalMapping;
using CatFactory.SqlServer.Tests.Models;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class ExportTest
    {
        [Fact]
        public void TestBloggingExportScript()
        {
            // Arrange
            var database = Databases.Blogging;

            // Act
            SqlServerDatabaseScriptCodeBuilder.CreateScript(database, "C:\\Temp\\CatFactory.SqlServer", true);

            // Assert
        }

        [Fact]
        public void TestCollegeExportScript()
        {
            // Arrange
            var database = SqlServerDatabaseFactory.CreateWithDefaults("College");

            var student = database
                .DefineEntity(new { StudentId = 0, FirstName = "", MiddleName = "", LastName = "" })
                .SetNaming("Student")
                .SetColumnFor(p => p.FirstName, type: "nvarchar", length: 10)
                .SetColumnFor(p => p.MiddleName, type: "nvarchar", length: 10, nullable: true)
                .SetColumnFor(p => p.LastName, type: "nvarchar", length: 10)
                .SetIdentity(p => p.StudentId)
                .SetPrimaryKey(e => e.StudentId)
                .AddExtendedProperty(p => p.FirstName, "MS_Description", "First name")
                .AddExtendedProperty(p => p.MiddleName, "MS_Description", "Middle name")
                .AddExtendedProperty(p => p.LastName, "MS_Description", "Last name");

            var course = database
                .DefineEntity(new { CourseId = 0, Name = "" })
                .SetNaming("Course")
                .SetColumnFor(e => e.Name, type: "nvarchar", length: 255)
                .SetIdentity(e => e.CourseId, seed: 1000, increment: 1000)
                .SetPrimaryKey(e => e.CourseId)
                .AddUnique(e => e.Name);

            var courseStudent = database
                .DefineEntity(new { CourseStudentId = 0, CourseId = 0, StudentId = 0 })
                .SetNaming("CourseStudent")
                .SetIdentity(p => p.CourseStudentId)
                .SetPrimaryKey(p => p.CourseStudentId)
                .AddUnique(p => new { p.CourseId, p.StudentId })
                .AddForeignKey(p => p.CourseId, course.Table)
                .AddForeignKey(p => p.StudentId, student.Table);

            // Act

            SqlServerDatabaseScriptCodeBuilder.CreateScript(database, "C:\\Temp\\CatFactory.SqlServer", true);

            // Assert
        }

        [Fact]
        public void TestDefinitionForRothschildHouseEntities()
        {
            // Arrange
            var database = SqlServerDatabaseFactory.CreateWithDefaults("RothschildHouse");

            database.ExtendedProperties.Add(new ExtendedProperty("MS_Description", "Database to storage RothschildHouse payments"));

            var person = database
                .DefineEntity(new
                {
                    PersonID = Guid.Empty,
                    GivenName = "",
                    MiddleName = "",
                    FamilyName = "",
                    FullName = "",
                    BirthDate = DateTime.Now
                })
                .SetNaming("Person")
                .SetColumnFor(p => p.GivenName, type: "nvarchar", length: 10)
                .SetColumnFor(p => p.FamilyName, type: "nvarchar", length: 10)
                .SetColumnFor(p => p.FullName, type: "nvarchar", length: 30)
                .SetPrimaryKey(p => p.PersonID)
                .SetColumnFor(p => p.MiddleName, type: "nvarchar", length: 10, nullable: true)
                .AddExtendedProperty("MS_Description", "Person catalog")
                .AddExtendedProperty(p => p.GivenName, "MS_Description", "Given name")
                .AddExtendedProperty(p => p.MiddleName, "MS_Description", "Middle name")
                .AddExtendedProperty(p => p.FamilyName, "MS_Description", "Family name")
                .AddExtendedProperty(p => p.FullName, "MS_Description", "Full name")
                .AddExtendedProperty(p => p.BirthDate, "MS_Description", "Birth date");

            var creditCard = database
                .DefineEntity(new
                {
                    CreditCardID = Guid.Empty,
                    PersonID = Guid.Empty,
                    CardType = "",
                    CardNumber = "",
                    Last4Digits = "",
                    ExpirationDate = DateTime.Now,
                    Cvv = DateTime.Now
                })
                .SetNaming("CreditCard")
                .SetColumnFor(p => p.CardType, type: "nvarchar", length: 20)
                .SetColumnFor(p => p.CardNumber, type: "nvarchar", length: 20)
                .SetColumnFor(p => p.Last4Digits, type: "nvarchar", length: 4)
                .SetColumnFor(p => p.Cvv, type: "nvarchar", length: 4)
                .SetPrimaryKey(p => p.CreditCardID)
                .AddUnique(p => p.CardNumber)
                .AddForeignKey(p => p.PersonID, person.Table)
                .AddExtendedProperty(p => p.PersonID, "MS_Description", "Person Identifier")
                .AddExtendedProperty(p => p.CardType, "MS_Description", "Card type")
                .AddExtendedProperty(p => p.CardNumber, "MS_Description", "Card number")
                .AddExtendedProperty(p => p.Last4Digits, "MS_Description", "Last 4 Digits")
                .AddExtendedProperty(p => p.ExpirationDate, "MS_Description", "Expiration Date")
                .AddExtendedProperty(p => p.Cvv, "MS_Description", "Card Verification Value");

            var paymentTransaction = database
                .DefineEntity(new
                {
                    PaymentTransactionID = Guid.Empty,
                    CreditCardID = Guid.Empty,
                    ConfirmationID = Guid.Empty,
                    Amount = 0m,
                    PaymentDateTime = DateTime.Now
                })
                .SetNaming("PaymentTransaction")
                .SetColumnFor(p => p.Amount, prec: 10, scale: 4)
                .SetPrimaryKey(p => p.CreditCardID)
                .AddForeignKey(p => p.CreditCardID, creditCard.Table)
                .AddExtendedProperty(p => p.PaymentTransactionID, "MS_Description", "Payment Transaction Identifier")
                .AddExtendedProperty(p => p.CreditCardID, "MS_Description", "Credit Card Identifier")
                .AddExtendedProperty(p => p.ConfirmationID, "MS_Description", "Confirmation Identifier")
                .AddExtendedProperty(p => p.Amount, "MS_Description", "Transaction Amount")
                .AddExtendedProperty(p => p.PaymentDateTime, "MS_Description", "Payment Date time");

            // Act

            SqlServerDatabaseScriptCodeBuilder.CreateScript(database, "C:\\Temp\\CatFactory.SqlServer", true);
        }
    }
}
