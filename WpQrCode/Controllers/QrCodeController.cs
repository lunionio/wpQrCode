using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using WpQrCode.Domains;
using WpQrCode.Entities;
using WpQrCode.Helpers;
using WpQrCode.Infrastructure.Exceptions;
using WpQrCode.Services;

namespace WpQrCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrCodeController : ControllerBase
    {
        private readonly SegurancaService _service;
        private readonly TokenDomain _domain;
        private readonly QrCodeHandler _handler;
        private readonly IConfiguration _configuration;

        public QrCodeController(SegurancaService service, 
            TokenDomain domain, QrCodeHandler handler, IConfiguration configuration)
        {
            _service = service;
            _domain = domain;
            _handler = handler;
            _configuration = configuration;
        }

        [HttpGet("{idCliente:int}/{idUsuario:int}/{token}")]
        public async Task<IActionResult> GenerateQrCode([FromRoute]int idCliente, [FromRoute]int idUsuario, [FromRoute]string token)
        {
            try
            {
                await _service.ValidateTokenAsync(token);

                if(_domain.GetLast(idCliente, out var tokenQr))
                {
                    tokenQr.QrCode = _handler.GenerateQr(tokenQr.Nome, 250, 250);
                    return Ok(tokenQr);
                }

                var milliseconds = _configuration.GetValue<string>("TokenExpiration");

                var options = new TokenOptions(DateTime.UtcNow, Guid.NewGuid().ToString(), milliseconds, idUsuario, idCliente);
                tokenQr = new Token(options, true, string.Empty, true, 1);

                var newToken =_domain.Save(tokenQr);
                newToken.QrCode = _handler.GenerateQr(newToken.Nome, 250, 250);

                return Ok(newToken);
            }
            catch(InvalidTokenException e)
            {
                return StatusCode(401, e.Message);
            }
            catch(TokenException e)
            {
                return StatusCode(400, e.Message);
            }
            catch(QrCodeException e)
            {
                return StatusCode(400, e.Message);
            }
            catch(ServiceException e)
            {
                return StatusCode(401, e.Message);
            }
            catch(Exception e)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("GenerateManually/{idCliente:int}/{idUsuario:int}/{token}")]
        public async Task<IActionResult> GenerateQrCodeManually([FromRoute]int idCliente, [FromRoute]int idUsuario, [FromRoute]string token)
        {
            try
            {
                await _service.ValidateTokenAsync(token);

                var options = new TokenOptions(DateTime.UtcNow, Guid.NewGuid().ToString(), string.Empty, idUsuario, idCliente);
                var tokenQr = new Token(options, false, string.Empty, true, 1);

                var newToken = _domain.Save(tokenQr);
                newToken.QrCode = _handler.GenerateQr(newToken.Nome, 250, 250);

                return Ok(newToken);
            }
            catch (InvalidTokenException e)
            {
                return StatusCode(401, e.Message);
            }
            catch (TokenException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (QrCodeException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (ServiceException e)
            {
                return StatusCode(401, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost("{idCliente:int}/{token}")]
        public async Task<IActionResult> ValidateTokenQr([FromRoute]int idCliente, [FromRoute]string token, [FromBody]Token tokenQr)
        {
            try
            {
                await _service.ValidateTokenAsync(token);

                var qrToken = _domain.GetById(tokenQr.ID, idCliente);

                if(qrToken != null && qrToken.VerifyHash(tokenQr.Nome))
                {
                    if (qrToken.HasExpiration)
                    {
                        if (qrToken.ExpirationTime > DateTime.UtcNow.TimeOfDay)
                        {
                            qrToken.QrCode = _handler.GenerateQr(qrToken.Nome, 250, 250);
                            return Ok(qrToken);
                        }
                        else
                        {
                            qrToken.Ativo = false;
                            _domain.Update(qrToken);
                            return Ok("Token expirado.");
                        }
                    }
                    else
                    {
                        qrToken.QrCode = _handler.GenerateQr(qrToken.Nome, 250, 250);
                        return Ok(qrToken);
                    }
                }

                return Ok("Token informado não é válido.");
            }
            catch (InvalidTokenException e)
            {
                return StatusCode(401, e.Message);
            }
            catch (TokenException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (QrCodeException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (ServiceException e)
            {
                return StatusCode(401, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }
    }
}