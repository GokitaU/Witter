using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Witter.Data
{
    public interface IAdminRepository
    {
        void AddPermanentBan(int userId);
        void AddBan(int userId, DateTime date);
        void RemovePermanentBan(int userId);
        void RemoveBan(int userId);
        void DeleteUser(int userId);
        void GrantAdminRights(int userId);
        void TakeAdminRights(int userId);
    }
}
