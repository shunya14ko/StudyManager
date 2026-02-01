using Microsoft.AspNetCore.Mvc;
using StudyManager.ApiService.Models;

namespace StudyManager.ApiService.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        // メモリ上の簡易ストア（実際はDB等へ保存してください）
        private static readonly List<ProjectModel> projects = [];

        // JSON を PUT で受け取り ProjectModel を作成（または更新）
        [HttpPut]
        public IActionResult CreateOrUpdate([FromBody] ProjectModel model)
        {
            // 既存なら更新（簡易実装）
            var existing = projects.FirstOrDefault(p => p.ProjectId == model.ProjectId);
            if (existing == null)
            {
                model.CreatedAt = DateTime.UtcNow;
                projects.Add(model);
                return Ok();
            }

            existing.ProjectName = model.ProjectName;
            // CreatedAt は通常更新しないが必要なら更新してください
            return Ok(existing);
        }

        // 作成したリソースを取得するエンドポイント（CreatedAtAction 用）
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var p = projects.Select(x => x.ProjectId == id);
            if (p == null) return NotFound();
            return Ok(p);
        }
    }
}