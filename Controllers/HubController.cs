using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using MyCarearApi.Services.Chat;
using io = System.IO;

namespace MyCarearApi.Controllers
{
    [Route("api/[controller]")]
    public class HubController: ControllerBase
    {
        private readonly IMessageService _messageService;

        public HubController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public IActionResult LocateFile(IFormFile file)
        {
            var fileName = _messageService.LocateFile(file);
            if(io.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName))) 
            {
                return Ok(new
                {
                    Succeded = true,
                    FileName = fileName
                });
            }
            return BadRequest(new
            {
                Succeded = false,
                FileName = string.Empty
            });
        }
    }
}
