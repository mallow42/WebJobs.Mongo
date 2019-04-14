namespace Mallow.Azure.WebJobs.Extensions.Mongo
{
    public enum InsertMode
    {
        NotSet = 0,
        
        /// <summary>
        /// Creates new document if document with same ID doesnt exist.
        /// </summary>
        Create = 1,
        
        /// <summary>
        /// Replaces document if document with same ID exists.
        /// </summary>
        Replace = 2,
        
        /// <summary>
        /// Creates new document if document with same ID doesnt exist or
        /// replaces document if document with same ID exists.
        /// </summary>
        CreateOrReplace = 3
    }
}