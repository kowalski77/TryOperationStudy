using System.Collections.Generic;

namespace TryOperationStudy
{
    public class SomeOperation
    {
        private SomeOperation(string context)
        {
            this.Context = context;
        }

        public string? Logger { get; set; }

        public string Context { get; }

        public ICollection<ExceptionPredicate?> ExceptionPredicateCollection { get; } = new List<ExceptionPredicate?>();

        public static SomeOperation With(string context)
        {
            return new(context);
        }
    }
}