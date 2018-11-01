using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace WpQrCode.Entities
{
    public class Token : Base
    {
        public Token()
        { }

        public Token(TokenOptions options, bool hasExpiration, string descricao, bool ativo, int status)
            : base(descricao, status, ativo, options.IdUsuario, options.IdCliente)
        {
            HasExpiration = hasExpiration;
            ExpirationTime = HasExpiration ? options.Expiration : new TimeSpan(0);

            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            this._salt = Convert.ToBase64String(salt);
            Nome = GetHash(options.Nome, salt);
        }

        public TimeSpan ExpirationTime { get; set; }
        public bool HasExpiration { get; set; }

        [Column("Salt")]
        private string _salt { get; set; }

        [NotMapped]
        public byte[] QrCode { get; set; }

        public bool VerifyHash(string token)
        {          
            return Nome.Equals(token);
        }

        private string GetHash(string token, byte[] sBytes)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: token,
            salt: sBytes,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        }
    }
}
