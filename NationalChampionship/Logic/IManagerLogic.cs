using NationalChampionship.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalChampionship.Logic
{
    public interface IManagerLogic
    {
        FileStream GetManagerImage(int managerId);

        Manager GetOneManager(int managerId);

        IList<Manager> GetAllManager();

        void AddManager(Manager manager);

        void DeleteManager(int managerId);

        void UpdateManager(int managerId, Manager newManager);
    }
}
