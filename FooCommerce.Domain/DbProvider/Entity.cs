using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace FooCommerce.Domain.DbProvider
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }

        [JsonIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public byte[] RowVersion { get; set; }

        public bool IsDeleted { get; set; }
    }
}