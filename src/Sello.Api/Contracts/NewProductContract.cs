using System.Runtime.Serialization;

namespace Sello.Api.Contracts
{
    /// <summary>
    ///     Information concerning a new product that is being added to the catalog
    /// </summary>
    [DataContract]
    public class NewProductContract
    {
        /// <summary>
        ///     Name of the product
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Description about the product
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Price for buying the product
        /// </summary>
        [DataMember]
        public double Price { get; set; }
    }
}