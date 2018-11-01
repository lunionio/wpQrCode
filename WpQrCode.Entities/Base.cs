using System;
using System.Collections.Generic;
using System.Text;

namespace WpQrCode.Entities
{
    public class Base
    {
        public Base()
        { }

        public Base(string descricao, int status, bool ativo, int idUsuario, int idCliente)
        {
            Descricao = descricao;
            Ativo = ativo;
            Status = status;
            IdCliente = idCliente;
            UsuarioCriacao = idUsuario;
            UsuarioEdicao = idUsuario;
        }

        public int ID { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataEdicao { get; set; }
        public int UsuarioCriacao { get; set; }
        public int UsuarioEdicao { get; set; }
        public bool Ativo { get; set; }
        public int Status { get; set; }
        public int IdCliente { get; set; }
    }
}
