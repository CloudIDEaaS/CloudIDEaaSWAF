// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;

namespace Utils.Kestrel;

/// <summary>
/// Defines a class that provides mechanisms for configuring the hosting
/// of a Single Page Application (SPA) and attaching middleware.
/// </summary>
public interface IKestrelSpaBuilder
{
    /// <summary>
    /// The <see cref="IApplicationBuilder"/> representing the middleware pipeline
    /// in which the SPA is being hosted.
    /// </summary>
    IApplicationBuilder ApplicationBuilder { get; }

    /// <summary>
    /// Describes configuration options for hosting a SPA.
    /// </summary>
    KestrelSpaOptions Options { get; }
}