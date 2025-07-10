// ----------------------------------------------------------------------
// <copyright file="CreateQuoteRequest.cs">
//  Copyright 2024
// </copyright>
// ----------------------------------------------------------------------

namespace Models
{
    /// <summary>
    /// The create quote request.
    /// </summary>
    public class CreateQuoteRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateQuoteRequest"/> class.
        /// </summary>
        public CreateQuoteRequest()
        {
            this.Items = new List<CreateQuoteRequestItem>();
        }

        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        public string? Customer { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IList<CreateQuoteRequestItem> Items { get; set; }
    }
}
