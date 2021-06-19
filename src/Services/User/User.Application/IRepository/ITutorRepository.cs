using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;

namespace User.Application.IRepository
{
    public interface ITutorRepository
    {
        Task<Tutor> getTutorbyUserIdAsync(string userId);

        Task InsertTutorAsync(Tutor tutor);
    }
}
