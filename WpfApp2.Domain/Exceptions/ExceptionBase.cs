using System;

namespace WpfApp2.Domain.Exceptions
{
    public abstract class ExceptionBase : Exception
    {
        public abstract ExceptionKind Kind { get; }

        public enum ExceptionKind
        {
            Info,
            Warning,
            Error,
        }

        public ExceptionBase(string message)
            : base(message) { }

        public ExceptionBase(string message, Exception exception)
            : base(message, exception) { }
    }
}
