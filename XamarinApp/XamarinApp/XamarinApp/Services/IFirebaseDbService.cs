using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinApp.Models;

namespace XamarinApp.Services
{
    public interface IFirebaseDbService
    {
        Task AddUserInfo(User userDto);

        List<Computer> GetAllComputers();

        Computer GetComputerById(string id);

        Task AddComputer(Computer computerDto);

        Task UpdateComputer(string id, Computer computerDto);
    }
}