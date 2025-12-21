using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cooktel_E_commrece.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class SubCategoryController:ControllerBase
    {

        private readonly ISubCategoryRepository _repository;
        public SubCategoryController(ISubCategoryRepository subCategoryRepository)
        {
            _repository = subCategoryRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAllSubCategory(int id)
        {
            var Subcategory = await _repository.GetAll(id);

            if (Subcategory == null) { 
                return NotFound("there is not subCategory");
            }
            return Ok(Subcategory);
        }

        [HttpPost]
        public async Task<ActionResult> AddSubCategory(SubCategoryDto subCategoryDto)
        {
            var newSub = new Subcategory { CategoryId = subCategoryDto.CategoryId,Sub_Name=subCategoryDto.Sub_Name };
            _repository.Add(newSub);

            if (await _repository.SaveAllChanges())
                return Ok("subCategory added successfully");
            return BadRequest("something went wrong");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubCategory(int id)
        {
            var Subcategory = await _repository.GetById(id);
            if (Subcategory == null)
            {

                return NotFound("there is no subcategory with this id");
            }

            _repository.Delete(Subcategory);

            if (await _repository.SaveAllChanges())
                return Ok("subCategory Deleted successfully");
            return BadRequest("something went wrong");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubCategory([FromQuery] int id, [FromBody] string newName) 
        {
            var Subcategory = await _repository.GetById(id);
            if (Subcategory == null)
            {

                return NotFound("there is no subcategory with this id");
            }

            _repository.Update(newName, Subcategory);

            if (await _repository.SaveAllChanges())
                return Ok("subCategory Updated successfully");
            return BadRequest("something went wrong");
        }
    }
}
