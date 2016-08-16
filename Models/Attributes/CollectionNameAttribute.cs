using System;

namespace Models.Attributes
{
    /// <summary>
    /// Attribute used to specify the mongodb collection name.
    /// If not used, the class name will be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CollectionNameAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the collection
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Constructor used to initialize CollectionNameAttribute class with the specified collection name
        /// </summary>
        /// <param name="name">The name of the collection</param>
        public CollectionNameAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Please specify a collection name");
            }
            Name = name;
        }
    }
}
