﻿using System;
using System.Collections.Generic;

namespace Community.AspNetCore.RequestDecompression
{
    /// <summary>Provides options for the HTTP request decompression middleware.</summary>
    public sealed class RequestDecompressionOptions
    {
        private readonly Dictionary<string, Type> _providers = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Initializes a new instance of the <see cref="RequestDecompressionOptions" /> class.</summary>
        public RequestDecompressionOptions()
        {
            SkipUnsupportedEncodings = true;
        }

        internal void Apply(RequestDecompressionOptions options)
        {
            foreach (var kvp in options.Providers)
            {
                _providers[kvp.Key] = kvp.Value;
            }

            SkipUnsupportedEncodings = options.SkipUnsupportedEncodings;
        }

        /// <summary>Registers the decompression provider.</summary>
        /// <typeparam name="T">The type of decompression provider.</typeparam>
        /// <param name="encodingName">The encoding name (case insensitive).</param>
        /// <exception cref="ArgumentNullException"><paramref name="encodingName" /> is <see langword="null" />.</exception>
        public void Register<T>(string encodingName)
            where T : class, IDecompressionProvider
        {
            if (encodingName == null)
            {
                throw new ArgumentNullException(nameof(encodingName));
            }

            _providers[encodingName] = typeof(T);
        }

        /// <summary>Gets the dictionary of decompression providers with encoding name as a key.</summary>
        public IReadOnlyDictionary<string, Type> Providers
        {
            get => _providers;
        }

        /// <summary>Gets or sets the value indicating whether the middleware should pass content with unsupported encoding to the next middleware in the request pipeline.</summary>
        public bool SkipUnsupportedEncodings
        {
            get;
            set;
        }
    }
}