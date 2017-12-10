using System.Runtime.Serialization;

namespace Sello.Api.Contracts
{
    /// <summary>
    ///     Information concerning a product that is being offered
    /// </summary>
    [DataContract]
    public class ProductContract
    {
        /// <summary>
        ///     Id of the product
        /// </summary>
        [DataMember]
        public string Id { get; set; }

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