using AppCore.Business.Utils.JsonWebToken.Bases;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HesaplarController : ControllerBase
    {
        private readonly IHesapService _accountService;
        private readonly JwtUtilBase _jwtUtil;

        public HesaplarController(IHesapService accountService, JwtUtilBase jwtUtil)
        {
            _accountService = accountService;
            _jwtUtil = jwtUtil;
        }

        [HttpPost("Giris")]
        public IActionResult Giris(KullaniciGirisModel model) // ~/api/Hesaplar/Giris
        {
            var girisResult = _accountService.Giris(model);
            if (!girisResult.IsSuccessful)
                return BadRequest(girisResult.Message);
            var tokenResult = _jwtUtil.CreateJwt(girisResult.Data.KullaniciAdi, girisResult.Data.RolModel.Adi, girisResult.Data.Id.ToString());
            return Ok(tokenResult.Data);
        }
    }
}
