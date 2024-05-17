using InventoryManagement.Models;
using System.Data;

namespace InventoryManagement.Repository.Interfaces
{
    public interface IExcelData
    {
        /// <summary>
        /// Saves the uploaded excel file to server
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        string ImportExcel(IFormFile formFile);
        /// <summary>
        /// Extracts data from excel and saves to Database
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<int> ExtractExcelData(string filePath);
    }
}
