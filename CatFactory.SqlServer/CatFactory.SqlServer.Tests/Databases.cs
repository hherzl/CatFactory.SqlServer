using System.Collections.Generic;
using CatFactory.Mapping;

namespace CatFactory.SqlServer.Tests
{
    public static class Databases
    {
        public static Database Store
        {
            get
            {
                var database = new Database()
                {
                    Name = "StoreMock",
                    Tables = new List<Table>
                    {
                        new Table
                        {
                            Schema = "dbo",
                            Name = "EventLog",
                            Columns = new List<Column>
                            {
                                new Column { Name = "EventLogID", Type = "int" },
                                new Column { Name = "EventType", Type = "int" },
                                new Column { Name = "Key", Type = "varchar", Length = 255 },
                                new Column { Name = "Message", Type = "varchar" },
                                new Column { Name = "EntryDate", Type = "datetime" }
                            },
                            Identity = new Identity { Name = "EventLogID", Seed = 1, Increment = 1 }
                        },
                        new Table
                        {
                            Schema = "HumanResources",
                            Name = "Employee",
                            Columns = new List<Column>
                            {
                                new Column { Name = "EmployeeID", Type = "int" },
                                new Column { Name = "FirstName", Type = "varchar", Length = 25 },
                                new Column { Name = "MiddleName", Type = "varchar", Length = 25, Nullable = true },
                                new Column { Name = "LastName", Type = "varchar", Length = 25 },
                                new Column { Name = "BirthDate", Type = "datetime" }
                            },
                            Identity = new Identity { Name = "EmployeeID", Seed = 1, Increment = 1 }
                        },
                        new Table
                        {
                            Schema = "Production",
                            Name = "ProductCategory",
                            Columns = new List<Column>
                            {
                                new Column { Name = "ProductCategoryID", Type = "int" },
                                new Column { Name = "ProductCategoryName", Type = "varchar", Length = 100 },
                            },
                            Identity = new Identity { Name = "ProductCategoryID", Seed = 1, Increment = 1 }
                        },
                        new Table
                        {
                            Schema = "Production",
                            Name = "Product",
                            Columns = new List<Column>
                            {
                                new Column { Name = "ProductID", Type = "int" },
                                new Column { Name = "ProductName", Type = "varchar", Length = 100 },
                                new Column { Name = "ProductCategoryID", Type = "int" },
                                new Column { Name = "Description", Type = "varchar", Length = 255, Nullable = true }
                            },
                            Identity = new Identity { Name = "ProductID", Seed = 1, Increment = 1 }
                        },
                        new Table
                        {
                            Schema = "Production",
                            Name = "ProductInventory",
                            Columns = new List<Column>
                            {
                                new Column { Name = "ProductInventoryID", Type = "int" },
                                new Column { Name = "ProductID", Type = "int" },
                                new Column { Name = "EntryDate", Type = "datetime" },
                                new Column { Name = "Quantity", Type = "int" }
                            },
                            Identity = new Identity { Name = "ProductInventoryID", Seed = 1, Increment = 1 }
                        },
                        new Table
                        {
                            Schema = "Sales",
                            Name = "Customer",
                            Columns = new List<Column>
                            {
                                new Column { Name = "CustomerID", Type = "int" },
                                new Column { Name = "CompanyName", Type = "varchar", Length = 100, Nullable = true },
                                new Column { Name = "ContactName", Type = "varchar", Length = 100, Nullable = true }
                            },
                            Identity = new Identity { Name = "CustomerID", Seed = 1, Increment = 1 }
                        },
                        new Table
                        {
                            Schema = "Sales",
                            Name = "Shipper",
                            Columns = new List<Column>
                            {
                                new Column { Name = "ShipperID", Type = "int" },
                                new Column { Name = "CompanyName", Type = "varchar", Length = 100, Nullable = true },
                                new Column { Name = "ContactName", Type = "varchar", Length = 100, Nullable = true }
                            },
                            Identity = new Identity { Name = "ShipperID", Seed = 1, Increment = 1 }
                        },
                        new Table
                        {
                            Schema = "Sales",
                            Name = "Order",
                            Columns = new List<Column>
                            {
                                new Column { Name = "OrderID", Type = "int" },
                                new Column { Name = "OrderDate", Type = "datetime" },
                                new Column { Name = "CustomerID", Type = "int" },
                                new Column { Name = "EmployeeID", Type = "int" },
                                new Column { Name = "ShipperID", Type = "int" },
                                new Column { Name = "Comments", Type = "varchar", Length = 255, Nullable = true }
                            },
                            Identity = new Identity { Name = "OrderID", Seed = 1, Increment = 1 }
                        },
                        new Table
                        {
                            Schema = "Sales",
                            Name = "OrderDetail",
                            Columns = new List<Column>
                            {
                                new Column { Name = "OrderID", Type = "int" },
                                new Column { Name = "ProductID", Type = "int" },
                                new Column { Name = "ProductName", Type = "varchar", Length = 255 },
                                new Column { Name = "UnitPrice", Type = "decimal", Prec = 8, Scale = 4 },
                                new Column { Name = "Quantity", Type = "int" },
                                new Column { Name = "Total", Type = "decimal", Prec = 8, Scale = 4 }
                            },
                            PrimaryKey = new PrimaryKey(new string[] { "OrderID", "ProductID" })
                        }
                    },
                    Views = new List<View>
                    {
                        new View
                        {
                            Schema = "Sales",
                            Name = "OrderSummary",
                            Columns = new List<Column>
                            {
                                new Column { Name = "OrderID", Type = "int" },
                                new Column { Name = "OrderDate", Type = "datetime" },
                                new Column { Name = "CustomerName", Type = "varchar", Length = 100 },
                                new Column { Name = "EmployeeName", Type = "varchar", Length = 100 },
                                new Column { Name = "ShipperName", Type = "varchar", Length = 100 },
                                new Column { Name = "OrderLines", Type = "int" }
                            }
                        }
                    }
                };

                database.AddDbObjectsFromTables();
                database.AddDbObjectsFromViews();
                database.SetPrimaryKeyToTables();

                database.AddColumnsForTables(new Column[]
                {
                    new Column { Name = "CreationUser", Type = "varchar", Length = 50 },
                    new Column { Name = "CreationDate", Type = "datetime" },
                    new Column { Name = "LastUpdateUser", Type = "varchar", Length = 50, Nullable = true },
                    new Column { Name = "LastUpdateDate", Type = "datetime", Nullable = true },
                    new Column { Name = "RowVersionID", Type = "rowversion", Nullable = true }
                });

                database.LinkTables();

                return database;
            }
        }
    }
}
