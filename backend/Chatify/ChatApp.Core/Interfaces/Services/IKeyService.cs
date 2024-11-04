using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Interfaces.Services
{
    public interface IKeyService
    {
        Task StoreKeyBundleAsync(KeyBundle keyBundle);
        Task<KeyBundle> GetKeyBundleAsync(Guid userId);

    }
}
