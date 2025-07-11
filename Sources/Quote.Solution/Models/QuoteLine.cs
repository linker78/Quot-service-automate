﻿// ----------------------------------------------------------------------
// <copyright file="QuoteLine.cs">
//  Copyright 2024
// </copyright>
// ----------------------------------------------------------------------

namespace Models
{
    /// <summary>
    /// The quote line.
    /// </summary>
    public class QuoteLine
    {
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        public string? Item { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public float Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unitary price.
        /// </summary>
        public decimal UnitaryPrice { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage.
        /// </summary>
        public float DiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the discount amount.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the line price.
        /// </summary>
        public decimal LinePrice { get; set; }
    }
}
