using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK.Domain.Dto.Request.Feedback;
using MK.Domain.Dto.Response.Feedback;
using MK.Service.Common;

namespace MK.API.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        /// <summary>
        /// Function to create new Feedback
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Guid of object have been created successfully</returns>
        /// <response code="200">Returns the newly created feedback</response>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([Required] CreateFeedbackReq req)
        {
            var result = await _feedbackService.Create(req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to delete feedback
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <returns></returns>
        /// <response code="200">Returns the deleted feedback</response>
        [HttpDelete("{feedbackId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid feedbackId)
        {
            var result = await _feedbackService.Delete(feedbackId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to update feedback
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("{feedbackId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid feedbackId, [Required] UpdateFeedbackReq req)
        {
            var result = await _feedbackService.Update(feedbackId, req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get feedback by id
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <returns></returns>
        /// <response code="200">Returns the feedback</response>
        /// <response code="400">If the feedback is null</response>
        [HttpGet("{feedbackId}")]
        [ProducesResponseType(typeof(FeedbackRes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(Guid feedbackId)
        {
            var result = await _feedbackService.GetById(feedbackId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get all feedback with paging
        /// </summary>
        /// <param name="req"></param>
        /// <returns>
        /// Paging List of feedback response
        [HttpGet]
        [ProducesResponseType(typeof(PagingResponse<FeedbackRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] PagingParameters req, [FromQuery] string[]? fields)
        {
            if (fields != null && !fields.IsMatchFieldPattern())
            {
                return BadRequest(new ResponseObject<FeedbackRes>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Fields are not valid, they are not match with pattern [fieldName:action]"
                });
            }
            var result = await _feedbackService.GetAll(req, fields);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Function to get all feedback by kitchen id
        /// </summary>
        /// <param name="kitchenId"></param>
        /// <returns></returns>
        /// <response code="200">Returns the feedback</response>
        [HttpGet("kitchen/{kitchenId}")]
        [ProducesResponseType(typeof(PagingResponse<FeedbackRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByKitchenId([Required] Guid kitchenId, [FromQuery] PagingParameters req, [FromQuery] string[]? fields)
        {
            if (fields != null && !fields.IsMatchFieldPattern())
            {
                return BadRequest(new ResponseObject<FeedbackRes>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Fields are not valid, they are not match with pattern [fieldName:action]"
                });
            }
            var result = await _feedbackService.GetFeedbacksByKitchenId(kitchenId, req, fields);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
