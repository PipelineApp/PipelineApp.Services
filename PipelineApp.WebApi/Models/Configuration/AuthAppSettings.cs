﻿// <copyright file="AuthAppSettings.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.WebApi.Models.Configuration
{
    /// <summary>
    /// Wrapper class for application settings related to auth tokens.
    /// </summary>
    public class AuthAppSettings
    {
        /// <summary>
        /// Gets or sets the authentication domain.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the authentication API identifier.
        /// </summary>
        public string ApiIdentifier { get; set; }
    }
}
