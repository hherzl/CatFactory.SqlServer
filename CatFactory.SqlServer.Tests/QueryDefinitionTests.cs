using CatFactory.SqlServer.Features;
using Xunit;

namespace CatFactory.SqlServer.Tests;

public class QueryDefinitionTests
{
    [Fact]
    public void SelectAllTest()
    {
        // Arrange
        var model = new
        {
            Id = 0,
            Name = "",
            Email = "",
            Phone = ""
        };

        // Act
        var queryDefinition = model.SelectAll("Student");

        // Assert
        Assert.True(queryDefinition.Columns.Count == 4);
        Assert.True(queryDefinition.Source == "Student");
    }

    [Fact]
    public void SelectByKeyTest()
    {
        // Arrange
        var model = new
        {
            Id = 0,
            Name = "",
            Email = "",
            Phone = ""
        };

        // Act
        var queryDefinition = model.SelectByKey(e => e.Id, model.Id, "Teacher");

        // Assert
        Assert.True(queryDefinition.Columns.Count == 4);
        Assert.True(queryDefinition.Source == "Teacher");
        Assert.True(queryDefinition.Conditions.Count == 1);
    }

    [Fact]
    public void InsertTest()
    {
        // Arrange
        var model = new
        {
            Id = Guid.Empty,
            Name = "",
            Email = "",
            Phone = ""
        };

        // Act
        var queryDefinition = model.Insert("Student");

        // Assert
        Assert.True(queryDefinition.Columns.Count == 4);
        Assert.True(queryDefinition.Source == "Student");
        Assert.True(queryDefinition.Conditions.Count == 0);
    }

    [Fact]
    public void InsertWithIdentityTest()
    {
        // Arrange
        var model = new
        {
            Id = 0,
            Name = "",
            Email = "",
            Phone = ""
        };

        // Act
        var queryDefinition = model.Insert(e => e.Id, "Employee");

        // Assert
        Assert.True(queryDefinition.Columns.Count == 4);
        Assert.True(queryDefinition.Source == "Employee");
        Assert.True(queryDefinition.Conditions.Count == 0);
    }

    [Fact]
    public void UpdateByKeyTest()
    {
        // Arrange
        var model = new
        {
            Id = 0,
            Name = "",
            Email = "",
            Phone = ""
        };

        // Act
        var queryDefinition = model.Update(e => e.Id, model.Id, "User");

        // Assert
        Assert.True(queryDefinition.Columns.Count == 4);
        Assert.True(queryDefinition.Source == "User");
        Assert.True(queryDefinition.Conditions.Count == 1);
    }

    [Fact]
    public void DeleteByKeyTest()
    {
        // Arrange
        var model = new
        {
            Id = 0,
            Name = "",
            Email = "",
            Phone = ""
        };

        // Act
        var queryDefinition = model.Delete(e => e.Id, model.Id, "Teacher");

        // Assert
        Assert.True(queryDefinition.Columns.Count == 0);
        Assert.True(queryDefinition.Source == "Teacher");
        Assert.True(queryDefinition.Conditions.Count == 1);
    }
}
