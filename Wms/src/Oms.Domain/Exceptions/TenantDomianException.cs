namespace Huayu.Oms.Domain.Exceptions
{
    public class TenantDomianException : Exception
    {
        public TenantDomianException() { }

        public TenantDomianException(string message) : base(message) { }

        public TenantDomianException(string message, Exception innerException) : base(message, innerException) { }
    }
}
