using System;
using System.Text.Json.Serialization;

// a little cheat ;)
namespace Orun.Domain
{
    /// <summary>
    /// Define the data of a trackable entity in the system. It adds
    /// two new properties to the entity:
    /// <li><see cref="IBusinessEntity{TKey}.Deleted"></see></li>
    /// </summary>
    public interface IBusinessEntity<TKey>
    {
        /// <summary>
        /// Mark entity as soft deleted
        /// </summary>
        [JsonPropertyName("deleted")]
        bool Deleted{get;set;}
        
        /// <summary>
        /// creation date
        /// </summary>
        [JsonPropertyName("createdAt")]
        DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// last modification date
        /// </summary>
        [JsonPropertyName("updateAt")]
        DateTime? UpdatedAt { get; set; }
    }
}