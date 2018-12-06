using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlog.Services
{
    public interface IGooglePictureLocator
    {
        Task<string> GetProfilePictureAsync(ExternalLoginInfo info);
    }
}
