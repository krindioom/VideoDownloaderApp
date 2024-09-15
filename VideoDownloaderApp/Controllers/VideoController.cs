using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace VideoDownloaderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        [HttpPost("download-video")]
        public async Task<ActionResult> DownloadVideoFromUrlAsync(string videoUrl)
        {
            var uniqueFileName = $"{Guid.NewGuid()}";
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "Downloads", $"{uniqueFileName}.%(ext)s");

            var ytDlpCommand = $" --xff US --extract-audio --audio-format mp3 -o \"{savePath}\" {videoUrl}";

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "yt-dlp",
                    Arguments = ytDlpCommand,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                return StatusCode(500, $"Ошибка при загрузке видео: {error}");
            }

            var finalPath = Path.Combine(Directory.GetCurrentDirectory(), "Downloads", $"{uniqueFileName}.mp4");

            if (!System.IO.File.Exists(finalPath))
            {
                return StatusCode(500, "Ошибка: файл не был создан.");
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(finalPath);

            // Возвращаем клиенту загруженный файл
            return File(fileBytes, "video/mp4", $"{uniqueFileName}.mp4");
        }
    }
}
