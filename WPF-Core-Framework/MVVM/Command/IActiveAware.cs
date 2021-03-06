//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;

namespace MVVM.Command
{
    /// <summary>
    /// Interface that defines if the object instance is active and notifies when the activity changes.
    /// </summary>
    public interface IActiveAware
    {
        #region Public Events

        /// <summary>
        /// Notifies that the value for <see cref="IsActive" /> property has changed.
        /// </summary>
        event EventHandler IsActiveChanged;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the object is active.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the object is active; otherwise <see langword="false" />.
        /// </value>
        bool IsActive { get; set; }

        #endregion Public Properties
    }
}