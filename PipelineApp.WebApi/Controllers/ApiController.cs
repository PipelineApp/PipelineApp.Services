﻿// <copyright file="ApiController.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Controller class for behavior related to testing authentication.
    /// </summary>
    [Route("api")]
    public class ApiController : BaseController
    {
        private ILogger<ApiController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Public endpoint accessible by unauthenticated users.
        /// </summary>
        /// <returns>JSON containing a message for unauthenticated users.</returns>
        [HttpGet]
        [Route("public")]
        public IActionResult Public()
        {
            return Json(new
            {
                Message = "Hello from a public endpoint! You don't need to be authenticated to see this."
            });
        }

        /// <summary>
        /// Private endpoint accessible only by authenticated users.
        /// </summary>
        /// <returns>String containing user's unique identifier.</returns>
        [HttpGet]
        [Route("private")]
        [Authorize]
        public IActionResult Private()
        {
            _logger.LogInformation("Processed request for authenticated endpoint.");
            return Ok(UserId);
        }
    }
}
