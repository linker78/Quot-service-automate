// ----------------------------------------------------------------------
// <copyright file="Confirmation.cs">
//  Copyright 2024
// </copyright>
// ----------------------------------------------------------------------

namespace Models
{
    /// <summary>
    /// The confirmation.
    /// </summary>
    public class Confirmation
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public ConfirmationLevel Level { get; set; }
    }
}
