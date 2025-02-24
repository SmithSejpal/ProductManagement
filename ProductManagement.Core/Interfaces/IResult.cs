namespace ProductManagement.Core.Interfaces
{
    public interface IResult
    {
        /// <summary>
        /// Was the result successful?
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Response message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Merges another result's message into the current result.
        /// </summary>
        void Merge(IResult other);

        /// <summary>
        /// Adds a single message to the result.
        /// </summary>
        void AddMessage(string message);
    }
}
