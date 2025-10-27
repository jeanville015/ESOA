using System;

namespace ESOA.Model
{
    public partial class ResponseMessage
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ResponseMessage"/> is status.
        /// </summary>
        /// <value><c>true</c> if status; otherwise, <c>false</c>.</value>
        public bool Status {    get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is fatal.
        /// </summary>
        /// <value><c>true</c> if this instance is fatal; otherwise, <c>false</c>.</value>
        public bool IsFatal { get; set; }
        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>The reason.</value>
        public string Reason { get; set; }
        /// <summary>
        /// Gets or sets the error logs.
        /// </summary>
        /// <value>The error logs.</value>
        public string ErrorLogs { get; set; }
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public int Total { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public Guid Guid { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public int IntGuid { get; set; }
    }
}