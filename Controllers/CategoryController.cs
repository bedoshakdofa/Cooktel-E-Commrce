using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cooktel_E_commrece.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    public class CategoryController:ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICachingService _cachingService;
        public CategoryController(ICategoryRepository categoryRepository, ICachingService cachingService)
        {
            _categoryRepository = categoryRepository;
            _cachingService = cachingService;
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddCategory([FromBody] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("please enter vaild input");
            }
            _categoryRepository.Create(name);

            await _cachingService.RemoveCache<Category>("category");

            await _categoryRepository.SaveChanges();

            return Ok("category added successfully");
        }

        [HttpDelete("Delete/{id}")]

        public async Task<ActionResult> DeleteCategory(int id)
        {

            var category = await _categoryRepository.GetOne(id);

            if (category == null) {
                return BadRequest("Category Not found");
            }

            _categoryRepository.Delete(category);
            await _cachingService.RemoveCache<Category>("category");

            await _categoryRepository.SaveChanges();

            return Ok("Category deleted successfully");
        }

        [HttpPut("update/{id}")]

        public async Task<ActionResult> UpdateCategory(int id, [FromBody] string Newname)
        {
            var category = await _categoryRepository.GetOne(id);
            if (category == null)
            {
                return BadRequest("Category Not found");
            }
            category.Name=Newname;

            await _cachingService.RemoveCache<Category>("category");
            await _categoryRepository.SaveChanges();

            return Ok("category updated successfully");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategory()
        {
            var category = _cachingService.GetData<IEnumerable<Category>>("category");

            if (category!=null)
            {
                return Ok(category);
            }
            
            category=await _categoryRepository.GetAll();

            if (!category.Any())
                return NotFound("there is no category");

            _cachingService.SetData("category", category);
            return Ok(category);
        }
    }
}
