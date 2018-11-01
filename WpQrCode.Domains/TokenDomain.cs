using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpQrCode.Domains.Generics;
using WpQrCode.Entities;
using WpQrCode.Infrastructure;
using WpQrCode.Infrastructure.Exceptions;

namespace WpQrCode.Domains
{
    public class TokenDomain : IDomain<Token>
    {
        private readonly TokenRepository _repository;

        public TokenDomain(TokenRepository repository)
        {
            _repository = repository;
        }

        public void Delete(Token entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Token> GetAll(int idCliente)
        {
            throw new NotImplementedException();
        }

        public Token GetById(int entityId, int idCliente)
        {
            try
            {
                var token = _repository.GetSingle(t => t.ID.Equals(entityId) && t.IdCliente.Equals(idCliente) &&t.Ativo);
                return token;
            }
            catch(Exception e)
            {
                throw new TokenException("Não foi possível recuperar o token.", e);
            }
        }

        public Token Save(Token entity)
        {
            try
            {
                entity.DataCriacao = DateTime.UtcNow;
                entity.DataEdicao = DateTime.UtcNow;
                entity.Ativo = true;

                var id = _repository.Add(entity);
                entity.ID = id;

                return entity;
            }
            catch(Exception e)
            {
                throw new TokenException("Não foi possível salvar o token gerado.", e);
            }
        }

        public Token Update(Token entity)
        {
            throw new NotImplementedException();
        }

        public bool GetLast(int idCliente, out Token token)
        {
            var tokenResult = _repository.GetList(t => t.IdCliente.Equals(idCliente) 
                    && t.HasExpiration)?.OrderBy(t => t.ExpirationTime).FirstOrDefault();
            bool result;

            switch (tokenResult)
            {
                case Token t
                    when t.HasExpiration && t.ExpirationTime > DateTime.UtcNow.TimeOfDay:
                    {
                        result = true;
                        token = t;
                    }
                    break;
                default:
                    result = false;
                    token = null;
                    break;
            }

            return result;
        }
    }
}
