using System;

namespace CrowdTag.Model
{
    public class BusinessException : Exception
    {
        public BusinessException() : base() { }

        public BusinessException(string message) : base(message) { }

        public BusinessException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class DbItemNotFoundException : BusinessException
    {
        public Type Type { get; set; }

        public object Id { get; set; }

        private static string FormatExceptionMessage(Type type, object id)
        {
            return FormatExceptionMessage(type.ToString(), id);
        }

        private static string FormatExceptionMessage(string type, object id)
        {
            return String.Format("Could not find item of type '{0}' with id '{1}'", type, id);
        }

        public DbItemNotFoundException(Type type, int id)
            : base(FormatExceptionMessage(type, id))
        {
            this.Type = type;
            this.Id = id;
        }
    }
}