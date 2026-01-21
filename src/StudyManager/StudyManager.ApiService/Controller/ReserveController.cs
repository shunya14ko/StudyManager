using Microsoft.AspNetCore.Mvc;

namespace StudyManager.ApiService.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class ReserveController : ControllerBase
{
    // MVCにてHTTP GETリクエストを処理するサンプルメソッド
    [HttpGet]
    public IActionResult GetStatus()
    {
        return new JsonResult(new { status = "OK", timestamp = DateTime.UtcNow });
    }

    // [HttpPut]
#warning ToDo // 実際にModelにぶち込むような処理を追加する
}