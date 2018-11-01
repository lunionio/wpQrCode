using System;
using System.Collections.Generic;
using System.Text;

namespace WpQrCode.Entities
{
    public class TokenOptions
    {
        private DateTime _date;
        private string _guid;
        private string _expiration;
        public string Nome { get; }
        public TimeSpan Expiration { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }

        public TokenOptions()
        { }

        public TokenOptions(DateTime date, string guid, string expiration, int idUsuario, int idCliente)
        {
            _date = date;
            _guid = guid;
            IdUsuario = idUsuario;
            IdCliente = idCliente;
            _expiration = expiration;
            Expiration = string.IsNullOrEmpty(_expiration) ? new TimeSpan(0) : _date.AddMilliseconds(Convert.ToDouble(_expiration)).TimeOfDay;
            Nome = $"{ IdUsuario }{ IdCliente }{ _date }{ _guid }";
        }
    }
}
