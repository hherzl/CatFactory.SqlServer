namespace CatFactory.SqlServer.Features
{
#pragma warning disable CS1591
    public class QueryCondition
    {
        public QueryCondition()
        {
        }

        public QueryCondition(string column, ComparisonOperator comparisonOperator, object value)
        {
            Column = column;
            ComparisonOperator = comparisonOperator;
            Value = value;
        }

        public QueryCondition(LogicOperator logicOperator, string column, ComparisonOperator comparisonOperator, object value)
        {
            LogicOperator = logicOperator;
            Column = column;
            ComparisonOperator = comparisonOperator;
            Value = value;
        }

        public LogicOperator LogicOperator { get; set; }
        public string Column { get; set; }
        public ComparisonOperator ComparisonOperator { get; set; }
        public object Value { get; set; }
    }
}
