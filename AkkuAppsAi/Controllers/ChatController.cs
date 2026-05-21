using AkkuAppsAi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkkuAppsAi.Controllers;

[Authorize]
public class ChatController : Controller
{
    private readonly IOllamaService _ollama;

    public ChatController(IOllamaService ollama)
    {
        _ollama = ollama;
    }

    public async Task<IActionResult> Index()
    {
        var models = await _ollama.GetAvailableModelsAsync();
        ViewBag.Models = models;
        ViewBag.DefaultModel = models.FirstOrDefault() ?? "llama3.2";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return Json(new { success = false, error = "Message is empty" });

        var model = string.IsNullOrWhiteSpace(request.Model) ? "llama3.2" : request.Model;
        var response = await _ollama.GetResponseAsync(request.Message, model);

        return Json(new { success = true, response });
    }
}

public class SendMessageRequest
{
    public string Message { get; set; } = "";
    public string? Model { get; set; }
}
