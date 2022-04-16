using Microsoft.AspNetCore.Hosting;
using NationalChampionship.Data.Models;
using NationalChampionship.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NationalChampionship.Logic
{
    public class ManagerLogic : IManagerLogic
    {
        private readonly IManagerRepository managerRepository;
        protected readonly IHostingEnvironment webHostEnvironment;
        protected readonly string webRoot;

        public ManagerLogic(IManagerRepository managerRepository, IHostingEnvironment webHostEnvironment)
        {
            this.managerRepository = managerRepository;
            this.webHostEnvironment = webHostEnvironment;
            webRoot = webHostEnvironment.WebRootPath + "\\";
        }

        public void AddManager(Manager manager)
        {
            managerRepository.Add(manager);
        }

        public void DeleteManager(int managerId)
        {
            managerRepository.Delete(managerId);
        }

        public IList<Manager> GetAllManager()
        {
            return managerRepository.GetAll().OrderBy(manager => manager.ManagerName).ToList();
        }

        public FileStream GetManagerImage(int managerId)
        {
            string path = webRoot + "ManagerImages/" + managerId.ToString() + ".png";
            if (File.Exists(path))
            {
                return File.OpenRead(path);
            }
            else
            {
                throw new ArgumentException("The manager has no image!");
            }
        }

        public Manager GetOneManager(int managerId)
        {
            return managerRepository.GetOne(managerId);
        }

        public void UpdateManager(int managerId, Manager newManager)
        {
            managerRepository.Update(managerId, newManager);
        }
    }
}
