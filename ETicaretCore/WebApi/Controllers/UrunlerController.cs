using Business.Models;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    // https://httpstatuses.com/

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UrunlerController : ControllerBase
    {
        private readonly IUrunService _urunService;

        public UrunlerController(IUrunService urunService)
        {
            _urunService = urunService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get() // ~/api/Urunler
        {
            var model = _urunService.Query().ToList();
            if (model.Count == 0)
                return NotFound(); // 404
            return Ok(model); // 200
        }

        [HttpGet("{id}")]

        public IActionResult Get(int id) // ~/api/Urunler/1
        {
            var model = _urunService.Query().SingleOrDefault(u => u.Id == id);
            if (model == null)
                return NotFound();
            return Ok(model);
        }

        //[HttpPost]
        //[Route("Create")]
        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateNewProduct(UrunModel model) // ~/api/Urunler/Create --- Post
        {
            var result = _urunService.Add(model);
            if (result.IsSuccessful)
                return Ok(model);
            //return BadRequest(result.Message); // 400
            return StatusCode(500); // Internal server error
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult Put(UrunModel model)
        {
            var result = _urunService.Update(model);
            if (result.IsSuccessful)
                return Ok(model);
            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var result = _urunService.Delete(id);
            if (result.IsSuccessful)
                return Ok(id);
            return BadRequest(result.Message);
        }
    }
}
