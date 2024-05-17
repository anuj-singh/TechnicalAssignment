using InventoryManagement.Common;
using InventoryManagement.DataAccess;
using InventoryManagement.Models;
using InventoryManagement.Repository.Interfaces;
using System.Data;
using System.Data.OleDb;

namespace InventoryManagement.Repository.Implementations
{
    public class ExcelDataImport : IExcelData
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDbProductOps _appDbContext;
        public ExcelDataImport(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IDbProductOps appDbContext)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _appDbContext = appDbContext;
        }

        public async Task<int> ExtractExcelData(string filePath)
        {
            string fileExtension=Path.GetExtension(filePath);
            string excelConnStr=string.Empty;
            excelConnStr = fileExtension switch
            {
                ".xls" => _configuration.GetConnectionString("excel97_03Conn"),
                ".xlsx" => _configuration.GetConnectionString("excel07Conn")
            };
            DataTable excelDataTable = new DataTable();
            excelConnStr=string.Format(excelConnStr, filePath);
            using (OleDbConnection oleDbConn = new OleDbConnection(excelConnStr))
            {
                using (OleDbCommand cmd = oleDbConn.CreateCommand())
                {

                    using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter())
                    {
                        oleDbConn.Open();
                        cmd.Connection = oleDbConn;
                        DataTable excelSchema;
                        excelSchema = oleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        var sheetName = excelSchema.Rows[0]["Table_Name"].ToString();
                        oleDbConn.Close();

                        oleDbConn.Open();
                        cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                        oleDbDataAdapter.SelectCommand = cmd;
                        oleDbDataAdapter.Fill(excelDataTable);
                        oleDbConn.Close();
                    }
                }
            }
            
            List<Product> products = new List<Product>();
            products = HelperMethods.ConvertToList<Product>(excelDataTable);
            var response= await _appDbContext.SaveProductsFromExcel(products);
            return response;
        }

        public string ImportExcel(IFormFile formFile)
        {
            var uploadPath = _webHostEnvironment.WebRootPath;
            string destinationPath = Path.Combine(uploadPath, "UploadFiles");
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }
            var sourceFile=formFile.FileName;
            var filePath = Path.Combine(destinationPath, sourceFile);
            using (FileStream fileStream= new FileStream(filePath,FileMode.Create))
            {
                formFile.CopyTo(fileStream);
            }
            return filePath;
        }
    }
}
