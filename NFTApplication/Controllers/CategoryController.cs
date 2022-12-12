// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using NFTDatabaseService;
using NFTApplication.Models.Category;

namespace NFTApplication.Controllers
{

    /// <summary>
    /// Category Controller
    /// </summary>
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
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCategorys", ex.Message);

                return Problem(title: "/Category/GetCategorys", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
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
        public async Task<IActionResult> GetCategoryImage(int categoryId)
        {
            try
            {
                // if image tag needed
                // $"data:{collection.CollectionImageType}:base64, {Convert.ToBase64String(collection.CollectionImage)}"

                var imageBox = await _db.GetCategoryImage(categoryId);

                if (imageBox != null && imageBox.Data != null && imageBox.Type != null)
                    return File(imageBox.Data, imageBox.Type);
                else
                    return NotFound("Category does not have an Image");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCategoryImage", ex.Message);

                return Problem(title: "/Category/GetCategoryImage", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


    }
}

