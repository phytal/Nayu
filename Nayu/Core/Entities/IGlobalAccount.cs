using System.Collections.Generic;

namespace Nayu.Core.Entities
{
    public interface IGlobalAccount
    {
        ulong Id { get; set; }
        Dictionary<string, string> Tags { get; set; }
    }
}
