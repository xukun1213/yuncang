using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wms.ApplicationCore.Interfaces
{
    public interface IUserService
    {
        Task<IList<string>> GetRoleNamesAsync(int userId);
    }
}
