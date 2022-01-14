using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Services
{
    public interface ISlugService
    {
        // Mandatory methods that must be implemented by concrete classes:
        string UrlFriendly(string title);

        // Ensure slug is unique in the DB
        bool IsUnique(string slug);
    }
}
