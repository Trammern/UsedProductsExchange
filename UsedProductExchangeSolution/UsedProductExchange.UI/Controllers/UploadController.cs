using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace UsedProductExchange.UI.Controllers
{
    public class UploadController: Controller
    {
        [Route("/api/upload")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    char[] charsToTrim = { '"', '*', ' ', '\''};
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var trim = fileName.ToString().Trim(charsToTrim);
                    var fullPath = Path.Combine(pathToSave, trim);
                    var dbPath = Path.Combine(folderName, trim);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}