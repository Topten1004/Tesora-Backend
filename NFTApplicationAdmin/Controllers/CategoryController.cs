// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Mvc;

using NFTDatabaseService;
using NFTDatabaseEntities;

using NFTApplicationAdmin.Models;
using NFTAdminApplication.Utility;

namespace NFTApplicationAdmin.Controllers
{

    /// <summary>
    /// Category Database Controller
    /// </summary>
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly ILogger<CategoryController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public CategoryController(INFTDatabaseService db, ILogger<CategoryController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Categories
        /// </summary>
        /// <returns>List of Category records</returns>
        /// <response code="200">List of Category records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetCategories")]
        [ProducesResponseType(typeof(List<GetCategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories()
        {
            try
            {             
                var response = new List<GetCategoryResponse>();
                var embedImage = false;

                var records = await _db.GetCategories();

                foreach (var record in records)
                {
                    var categoryBox = await _db.GetCategoryImage(record.CategoryId);

                    response.Add(new GetCategoryResponse
                    {
                        CategoryId = record.CategoryId,
                        CategoryImage = embedImage ? $"data:{categoryBox.Type}:base64, {Convert.ToBase64String(categoryBox.Data)}" 
                                                   : $"/api/v1/Category/GetCategoryImage/{record.CategoryId}",
                        CreateDate = record.CreateDate,
                        Status = record.Status.ToString(),
                        Title = record.Title,
                        ContractId = record.ContractId
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetCategories, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Category/GetCategorys", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Category record based in the primary key id
        /// </summary>
        /// <param name="CategoryId">Primary Key</param>
        /// <returns>Category</returns>
        /// <response code="200">Category</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetCategory/{CategoryId:int}")]
        [ProducesResponseType(typeof(GetCategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategory(int CategoryId)
        {
            try
            {
                var record = await _db.GetCategory(CategoryId);
                var embedImage = false;

                var categoryBox = await _db.GetCategoryImage(CategoryId);

                var response = new GetCategoryResponse
                {
                    CategoryId = record.CategoryId,
                    CategoryImage = embedImage ? $"data:{categoryBox.Type}:base64, {Convert.ToBase64String(categoryBox.Data)}"
                                               : $"/api/v1/Category/GetCategoryImage/{record.CategoryId}",
                    CreateDate = record.CreateDate,
                    Status = record.Status.ToString(),
                    Title = record.Title,
                    ContractId = record.ContractId
                };


                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetCategory, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Category/GetCategory", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Create a Category record
        /// </summary>
        /// <param name="request">CreateCategoryRequest</param>
        /// <returns></returns>
        /// <response code="200">Category</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("CreateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryRequest request)
        {
            try
            {
                if (request == null)
                    throw new Exception("Invalid request");

                var image = UploadFileHandler.GetFileContents(request.Image);

                if (image == null)
                    throw new Exception("Invalid image");

                var record = new Category
                {
                    Title = request.Title,
                    CategoryImage = image,
                    CategoryImageType = request.Image.ContentType,
                    Status = Category.CategoryStatuses.active,
                    CreateDate = DateTime.UtcNow,
                    ContractId = request.ContractId
                };

                await _db.PostCategory(record);

                return Ok("Category Created");
            }
            catch (Exception ex)
            {
                var msg = $"Method: CreateCategory, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Category/CreateCategory", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a Category record
        /// </summary>
        /// <param name="request">Category</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryRequest request)
        {
            try
            {
                if (request == null)
                    throw new Exception("Invalid request");

                var image = UploadFileHandler.GetFileContents(request.Image);

                string fileType = request.Image?.ContentType;

                var imageBox = await _db.GetCategoryImage(request.CategoryId);

                if (image == null)
                {
                    image = imageBox.Data;
                    fileType = imageBox.Type;
                }

                var record = new Category
                {
                    CategoryId = request.CategoryId,
                    Title = request.Title,
                    CategoryImage = image,
                    CategoryImageType = fileType,
                    Status = request.Status,
                    ContractId = request.ContractId
                };

                await _db.PutCategory(record);

                return Ok("Category Updated");
            }
            catch (Exception ex)
            {
                var msg = $"Method: UpdateCategory, Exeption: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Category/UpdateCategory", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Delete a Category record
        /// </summary>
        /// <param name="CategoryId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteCategory/{CategoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int CategoryId)
        {
            try
            {
                await _db.DeleteCategory(CategoryId);

                return Ok("Category Deleted");
            }
            catch (Exception ex)
            {
                var msg = $"Method: DeleteCategory, Exeption: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Category/DeleteCategory", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Get Image Category
        /// </summary>
        /// <returns>Category Image</returns>
        /// <response code="200">Category Image</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetCategoryImage/{categoryId:int}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoryImage(int categoryId)
        {
            try
            {
                var imageBox = await _db.GetCategoryImage(categoryId);

                if (imageBox != null && imageBox.Data != null && imageBox.Type != null)
                    return File(imageBox.Data, imageBox.Type);
                else
                    return NotFound("Category does not have an Image");
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetCategoryImage, Exeption: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Category/GetCategoryImage", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}