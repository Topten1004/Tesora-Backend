// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NFTDatabase.DataAccess;
using NFTDatabaseEntities;


namespace NFTDatabase.Controllers
{

    /// <summary>
    /// Category Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<CategoryController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public CategoryController(IPostgreSql db, ILogger<CategoryController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Category Exists?
        /// </summary>
        /// <returns>bool</returns>
        /// <response code="200">bool</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetCategoryExists/{categoryId:int}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoryExists(int categoryId)
        {
            try
            {
                return Ok(await _db.CategoryExists(categoryId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCategoryExists", ex.Message);

                return Problem(title: "/Category/GetCategoryExists", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Gets the Categories
        /// </summary>
        /// <returns>List of Category records</returns>
        /// <response code="200">List of Category records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetCategories")]
        [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var result = await _db.RetrieveCategories();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCategories", ex.Message);

                return Problem(title: "/Category/GetCategories", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Category record based in the primary key id
        /// </summary>
        /// <param name="categoryId">Primary Key</param>
        /// <returns>Category</returns>
        /// <response code="200">Category</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetCategory/{categoryId:int}")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            try
            {
                var result = await _db.RetrieveCategory(categoryId);

                if (result == null)
                    throw new PostgreSql.RecordNotFound("Category not found");

                return Ok(result);
            }
            catch(PostgreSql.RecordNotFound nf)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCategory", nf.Message);

                return NotFound(nf.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCategory", ex.Message);

                return Problem(title: "/Category/GetCategory", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Gets a Category image based in the primary key id
        /// </summary>
        /// <param name="categoryId">Primary Key</param>
        /// <returns>Category</returns>
        /// <response code="200">Category Image</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetCategoryImage/{categoryId:int}")]
        [ProducesResponseType(typeof(ImageBox), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryImage(int categoryId)
        {
            try
            {
                var result = await _db.RetrieveCategoryImage(categoryId);

                if (result == null)
                    throw new PostgreSql.RecordNotFound("Category image not found");

                return Ok(result);
            }
            catch (PostgreSql.RecordNotFound nf)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCategoryImage", nf.Message);

                return NotFound(nf.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCategoryImage", ex.Message);

                return Problem(title: "/Category/GetCategoryImage", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Add a Category record
        /// </summary>
        /// <param name="record">Category</param>
        /// <returns>Category</returns>
        /// <response code="200">Category</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostCategory([FromBody]Category record)
        {
            try
            {
               await _db.CreateCategory(record);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "PostCategory", ex.Message);

                return Problem(title: "/Category/PostCategory", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a Category record
        /// </summary>
        /// <param name="record">Category</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCategory([FromBody]Category record)
        {
            try
            {
               await _db.UpdateCategory(record);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "PutCategory", ex.Message);

                return Problem(title: "/Category/PutCategory", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Delete a Category record
        /// </summary>
        /// <param name="categoryId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteCategory/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                await _db.DeleteCategory(categoryId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "DeleteCategory", ex.Message);

                return Problem(title: "/Category/DeleteCategory", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets the Category Id by Title
        /// </summary>
        /// <param name="title">Title</param>
        /// <returns>Category Id</returns>
        /// <response code="200">Category Id</response>
        [HttpGet()]
        [Route("GetCategoryIdByTitle/{title}")]
        [ProducesResponseType(typeof(int?), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCategoryIdByTitle(string title)
        {
            try
            {
                var result = await _db.GetCategoryIdByTitle(title);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCategoryIdByTitle", ex.Message);

                return Problem(title: "/Category/GetCategoryIdByTitle", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

    }
}

