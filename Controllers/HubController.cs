# pragma warning disable
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult LocateFile(IFormFileCollection files)
        {
            var filePaths = new List<string>();
            var result = true;
            foreach (var file in files)
            {
                var filePath = _messageService.LocateFile(file);
                filePaths.Add(filePath);
                
            }

            var successfulFilePaths = filePaths.Where(fileName =>
                            io.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName)));
            
            if(successfulFilePaths.Any()) 
            {
                return Ok(new
                {
                    Succeded = true,
                    Files = successfulFilePaths
                });
            }
            return BadRequest(new
            {
                Succeded = false,
                Files = successfulFilePaths
            });
        }
    }
}
