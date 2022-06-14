using System;
using System.Text.Json.Serialization;

// a little cheat ;)
namespace Orun.Domain
{
    /// <inheritdoc cref="IBusinessEntity{TKey}"/>
    public class BusinessEntity<TKey> : IBusinessEntity<TKey>
    {
        /// <summary>
        /// return a new instance of <see cref="BusinessEntity{TKey}"/>
        /// </summary>
        public BusinessEntity()
        {
            Deleted = false;
        }

        /// <inheritdoc cref="IBusinessEntity{TKey}.Deleted"/>
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
        
        /// <inheritdoc cref="IBusinessEntity{TKey}.CreatedAt"/>
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        
        /// <inheritdoc cref="IBusinessEntity{TKey}.UpdatedAt"/>
        [JsonPropertyName("updateAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}