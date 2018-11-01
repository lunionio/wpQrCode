using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WpQrCode.Infrastructure.Exceptions
{
    public class QrCodeException : Exception
    {
        public QrCodeException()
        {
        }

        public QrCodeException(string message) : base(message)
        {
        }

        public QrCodeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QrCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
