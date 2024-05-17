using InventoryManagement.Models;
using InventoryManagement.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Text;

namespace InventoryManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly IExcelData _excelUpload;
        private readonly IDbProductOps _dbProductOps;
        public ProductController(IExcelData excelData, IDbProductOps dbProductOps)
        {
            _excelUpload = excelData;
            _dbProductOps = dbProductOps;

        }
        /// <summary>
        /// Get method for index
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var products = await _dbProductOps.GetAllProducts();
            return View(products);
        }
        /// <summary>
        /// Gets view for excel upload
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportExcelData()
        {
            return View();
        }
        /// <summary>
        /// Saves excel data to Database
        /// </summary>
        /// <param name="formfile"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ImportExcelData(IFormFile formfile)
        {
            try
            {
                var path = _excelUpload.ImportExcel(formfile);
                var response = await _excelUpload.ExtractExcelData(path);
                if (response > 0)
                {
                    TempData["successMessage"] = "Products successfully created!";
                }
                else
                {
                    TempData["failureMessage"] = "Products creation failed!";
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction("Index");
        }
        /// <summary>
        /// Gets view for product.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AddProduct()
        {
            return View();
        }
        /// <summary>
        /// Saves Product to Database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _dbProductOps.AddProduct(product);
                    if (product.Id != 0)
                    {
                        TempData["successMessage"] = "Product successfully created!";
                    }
                    else
                    {
                        TempData["failureMessage"] = "Product creation failed!";
                    }
                }
                else
                {
                    return View(product);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Gets teh edit product view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int id)
        {
            Product product;
            if (id is 0)
            {
                return BadRequest();
            }
            else
            {
                product = await _dbProductOps.GetProductById(id);
                if (product is null)
                {
                    return NotFound();
                }

            }
            return View(product);
        }
        /// <summary>
        /// Saves the product to database
        /// </summary>
        /// <param name="Save"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Edit(Product Save)
        {
            try
            {
                if (!ModelState.IsValid || Save.Id is 0 )
                {
                    //return BadRequest();
                    return View("Edit");
                }
                else
                {
                    var status = await _dbProductOps.UpdateProduct(Save);
                    if (status is 1)
                    {
                        TempData["successMessage"] = "Product successfully updated!";
                    }
                    else
                    {
                        TempData["failureMessage"] = "Product updation failed!";
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        /// <summary>
        /// Deletes the product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                if (id != 0)
                {
                    var response = await _dbProductOps.DeleteProduct(id);
                    if (response == 1)
                    {
                        TempData["successMessage"] = "Product Successfully deleted!";
                    }
                    else
                    {
                        TempData["failureMessage"] = "Product deletion failed!";
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Exports the table to excel.
        /// </summary>
        /// <param name="htmlTable"></param>
        /// <returns></returns>
        public FileResult ExportToExcel(string htmlTable)
        {
            var datetimeStamp = DateTime.Now.ToString();
            return File(Encoding.ASCII.GetBytes(htmlTable), "application/vnd.ms-excel", "htmlTableToExcel" + datetimeStamp + ".xls");
        }
    }
}
