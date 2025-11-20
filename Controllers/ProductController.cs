using AutoMapper;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Extenstions;
using Cooktel_E_commrece.Helper;
using Cooktel_E_commrece.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Cooktel_E_commrece.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductController(IProductRepository productRepository, IMapper mapper, IFileService fileService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts([FromQuery] FilterParams Params)
        {
            
            var product = await _productRepository.GetAll(Params);

            if (product == null) { 
                return NotFound("there is no Product yet");
            }


            foreach (var item in product) {
                HttpContext.AddImageLink(item.Image);
            }

            Response.AddPaginationHeader(new PaginationHeaders(product.CurrantPage, product.PageSize, product.TotalPages, product.TotalCount));

            return Ok(product);
        }


        [HttpGet("{id}")]

        public async Task<ActionResult<ProductWithReviews>> GetProduct([FromRoute] int id)
        {
            var product = await _productRepository.GetProductWithReview(id);

            if (product == null) {
                return NotFound("No Product with is id");
            }

            HttpContext.AddImageLink(product.Image);

            return Ok(product);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddProduct([FromForm] ProductRequest productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var filename = await _fileService.UploadFile(productDto.Image, new[] { ".png", ".jpg", ".svg" });

            var product = _mapper.Map<Product>(productDto);
            product.Image.Add(filename);
            _productRepository.CreatProduct(product);


            if (await _productRepository.SaveChanges())
            {
                return Ok(product);
            }
            else
            {
                _fileService.DeleteFile(filename);
                return BadRequest("Can't save the data");
            }

        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateProduct([FromBody]JsonPatchDocument<ProductResponse> productDto, [FromRoute] int id)
        {
            var product= await _productRepository.GetById(id);

            _productRepository.UpdateProduct(productDto,product,ModelState);

            if (await _productRepository.SaveChanges())
                return Ok(product);
            return BadRequest("can't save to database");
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] int id)
        {
            var product = await _productRepository.GetById(id);

            _productRepository.DeleteProduct(product);

            if (await _productRepository.SaveChanges())
            {
                foreach (var img in product.Image)
                {
                    _fileService.DeleteFile(img);
                }
                return Ok("product Deleted");
            }
            return BadRequest("Can't deleted product");
        }


        [HttpPut("Upload-Img/{id}")]
        public async Task<ActionResult>UploadImage(int id, [FromForm]IFormFile img)
        {
            var product= await _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound("no product with this id");
            }
            var fileName=await _fileService.UploadFile(img, new[] { ".png", ".jpg", ".svg" });

            product.Image.Add(fileName);

            if (await _productRepository.SaveChanges())
                return Ok("Photo uploaded successfully");
            return BadRequest("can't save to database");

        }
    }
}
