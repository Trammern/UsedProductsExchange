using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace UsedProductExchange.UI.Controllers
{
    public class UploadController: Controller
    {
        [Route("/api/upload")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload(IFormCollection data, IFormFile imageFile)
        {
            try
            {
                var folder = "";
                var file = Request.Form.Files[0];
                folder = Request.Form["folder"];
                
                if (folder == "")
                {
                    folder = "Products";
                }
                
                var folderName = Path.Combine("Resources", "Images", folder);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                
                if (file.Length <= 0) return BadRequest();
                
                char[] charsToTrim = { '"', '*', ' ', '\''};
                var fileName = Guid.NewGuid() + Path.GetExtension(file.ContentDisposition);
                var trim = fileName.Trim(charsToTrim);
                var fullPath = Path.Combine(pathToSave, trim);
                var dbPath = Path.Combine(folderName, trim);
                
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                
                return Ok(new { dbPath });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}