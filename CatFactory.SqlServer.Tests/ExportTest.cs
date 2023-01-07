using System;
using CatFactory.SqlServer.CodeFactory;
using CatFactory.SqlServer.DatabaseObjectModel;
using CatFactory.SqlServer.ObjectRelationalMapping;
using CatFactory.SqlServer.Tests.Models;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
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
                .DefineEntity(new { StudentId = 0, GivenName = "", MiddleName = "", FamilyName = "" })
                .SetNaming("Student")
                .SetColumnFor(p => p.GivenName, length: 15)
                .SetColumnFor(p => p.MiddleName, length: 15, nullable: true)
                .SetColumnFor(p => p.FamilyName, length: 15)
                .SetIdentity(p => p.StudentId)
                .SetPrimaryKey(e => e.StudentId)
                ;

            student
                .AddExtendedProperty(p => p.GivenName, "MS_Description", "Given name")
                .AddExtendedProperty(p => p.MiddleName, "MS_Description", "Middle name")
                .AddExtendedProperty(p => p.FamilyName, "MS_Description", "Family name")
                ;

            student.Data.Add(new { StudentId = 0, GivenName = "Carlo", MiddleName = "H", FamilyName = "Herzl" });

            // todo: add sql transcriber => translate results from select to code (c#)

            var course = database
                .DefineEntity(new { CourseId = 0, Name = "", Description = "" })
                .SetNaming("Course")
                .SetColumnFor(e => e.Name, length: 255)
                .SetIdentity(e => e.CourseId, seed: 1000, increment: 1000)
                .SetPrimaryKey(e => e.CourseId)
                .AddUnique(e => e.Name)
                ;

            var courseStudent = database
                .DefineEntity(new { CourseStudentId = 0, CourseId = 0, StudentId = 0 })
                .SetNaming("CourseStudent")
                .SetIdentity(p => p.CourseStudentId)
                .SetPrimaryKey(p => p.CourseStudentId)
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

            database.ExtendedProperties.Add(new ExtendedProperty("MS_Description", "Database to storage RothschildHouse payments"));

            database.AddDefaultTypeMapFor(typeof(string), "nvarchar");

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
                .SetNaming("Person", "People")
                .SetColumnFor(p => p.GivenName, length: 15)
                .SetColumnFor(p => p.MiddleName, length: 15, nullable: true)
                .SetColumnFor(p => p.FamilyName, length: 15)
                .SetColumnFor(p => p.FullName, length: 45)
                .SetPrimaryKey(p => p.PersonID)
                ;

            person
                .AddExtendedProperty("MS_Description", "Person catalog")
                .AddExtendedProperty(p => p.GivenName, "MS_Description", "Given name")
                .AddExtendedProperty(p => p.MiddleName, "MS_Description", "Middle name")
                .AddExtendedProperty(p => p.FamilyName, "MS_Description", "Family name")
                .AddExtendedProperty(p => p.FullName, "MS_Description", "Full name")
                .AddExtendedProperty(p => p.BirthDate, "MS_Description", "Birth date")
                ;

            var creditCard = database
                .DefineEntity(new
                {
                    CreditCardID = Guid.Empty,
                    PersonID = Guid.Empty,
                    CardType = "",
                    CardNumber = "",
                    Last4Digits = "",
                    ExpirationDate = DateTime.Now,
                    Cvv = ""
                })
                .SetNaming("CreditCard", "Payment")
                .SetColumnFor(p => p.CardType, type: "nvarchar", length: 20)
                .SetColumnFor(p => p.CardNumber, type: "nvarchar", length: 20)
                .SetColumnFor(p => p.Last4Digits, type: "nvarchar", length: 4)
                .SetColumnFor(p => p.Cvv, type: "nvarchar", length: 4)
                .SetPrimaryKey(p => p.CreditCardID)
                .AddUnique(p => p.CardNumber)
                .AddForeignKey(p => p.PersonID, person.Table)
                ;

            creditCard
                .AddExtendedProperty(p => p.PersonID, "MS_Description", "Person Identifier")
                .AddExtendedProperty(p => p.CardType, "MS_Description", "Card type")
                .AddExtendedProperty(p => p.CardNumber, "MS_Description", "Card number")
                .AddExtendedProperty(p => p.Last4Digits, "MS_Description", "Last 4 Digits")
                .AddExtendedProperty(p => p.ExpirationDate, "MS_Description", "Expiration Date")
                .AddExtendedProperty(p => p.Cvv, "MS_Description", "Card Verification Value")
                ;

            var paymentTransaction = database
                .DefineEntity(new
                {
                    PaymentTransactionID = Guid.Empty,
                    CreditCardID = Guid.Empty,
                    ConfirmationID = Guid.Empty,
                    Amount = 0m,
                    PaymentDateTime = DateTime.Now
                })
                .SetNaming("PaymentTransaction", "Payment")
                .SetColumnFor(p => p.Amount, prec: 10, scale: 4)
                .SetPrimaryKey(p => p.CreditCardID)
                .AddForeignKey(p => p.CreditCardID, creditCard.Table)
                .AddDefault(p => p.PaymentDateTime, "GETDATE()")
                .AddCheck(p => p.Amount, "Amount > 0")
                ;

            paymentTransaction
                .AddExtendedProperty(p => p.PaymentTransactionID, "MS_Description", "Payment Transaction Identifier")
                .AddExtendedProperty(p => p.CreditCardID, "MS_Description", "Credit Card Identifier")
                .AddExtendedProperty(p => p.ConfirmationID, "MS_Description", "Confirmation Identifier")
                .AddExtendedProperty(p => p.Amount, "MS_Description", "Transaction Amount")
                .AddExtendedProperty(p => p.PaymentDateTime, "MS_Description", "Payment Date time")
                ;

            // Act

            SqlServerDatabaseScriptCodeBuilder.CreateScript(database, @"C:\Temp\CatFactory.SqlServer", true, true);
        }
    }
}
