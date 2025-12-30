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
        private readonly ICachingService _cachingService;
        public SubCategoryController(ISubCategoryRepository subCategoryRepository, ICachingService cachingService)
        {
            _repository = subCategoryRepository;
            _cachingService = cachingService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<SubCategoryDto>>> GetAllSubCategory(int id)
        {
            var subcate = _cachingService.GetData<IEnumerable<SubCategoryDto>>("subcategory");

            if (subcate!=null) { 
                return Ok(subcate);
            }
            subcate = await _repository.GetAll(id);


            if (!subcate.Any())
                return NotFound("there is not subCategory for this id");

            _cachingService.SetData("subcategory",subcate);
            return Ok(subcate);
        }

        [HttpPost]
        public async Task<ActionResult> AddSubCategory(SubCategoryDto subCategoryDto)
        {
            var newSub = new Subcategory { CategoryId = subCategoryDto.CategoryId,Sub_Name=subCategoryDto.Sub_Name };
            _repository.Add(newSub);

           await _cachingService.RemoveCache<Subcategory>("subcategory");
            if (await _repository.SaveAllChanges())
            {
                return Ok("subCategory added successfully");
            }
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
            await _cachingService.RemoveCache<Subcategory>("subcategory");

            if (await _repository.SaveAllChanges())
                return Ok("subCategory Deleted successfully");
            return BadRequest("something went wrong");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubCategory(int id, [FromBody] string newName) 
        {
            var Subcategory = await _repository.GetById(id);
            if (Subcategory == null)
            { 
                return NotFound($"there is no subcategory with this {id}");
            }

            _repository.Update(newName, Subcategory);
            await _cachingService.RemoveCache<Subcategory>("subcategory");

            if (await _repository.SaveAllChanges())
                return Ok("subCategory Updated successfully");
            return BadRequest("something went wrong");
        }
    }
}
