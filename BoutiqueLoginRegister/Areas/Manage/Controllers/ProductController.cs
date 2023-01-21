using BoutiqueLoginRegister.DataContext;
using BoutiqueLoginRegister.Helpers;
using BoutiqueLoginRegister.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoutiqueLoginRegister.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        public AppDbContext _appDbContext { get; }
        public IWebHostEnvironment _webHostEnvironment { get; }

        public ProductController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(_appDbContext.Products.Include(x => x.Category).ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _appDbContext.Categories.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if(!ModelState.IsValid) return RedirectToAction("Index");

            string imageName = product.ImageFile.FileName;
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/products", imageName);

            product.ImageName= product.ImageFile.SaveFile(_webHostEnvironment.WebRootPath, "uploads/products");

            _appDbContext.Products.Add(product);
            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int Id)
        {
            ViewBag.Categories = _appDbContext.Categories.ToList();

            Product product = _appDbContext.Products.FirstOrDefault(x => x.Id == Id);

            return View(product);
        }

        [HttpPost]
        public IActionResult Update(Product changedProduct)
        {
            Product product = _appDbContext.Products.FirstOrDefault(x => x.Id == changedProduct.Id);

            if(product != null)
            {
                if(changedProduct.ImageFile != null)
                {
                    string previousImageName = product.ImageName;
                    string previousImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/products", previousImageName);
                    System.IO.File.Delete(previousImagePath);

                    string imageName = changedProduct.ImageFile.FileName;
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/products", previousImageName);

                    product.ImageName = changedProduct.ImageFile.SaveFile(_webHostEnvironment.WebRootPath, "uploads/products");
                }
                product.Name = changedProduct.Name;
                product.Description = changedProduct.Description;
                product.CategoryId = changedProduct.CategoryId;
                product.Price = changedProduct.Price;
            }

            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int Id)
        {
            Product product = _appDbContext.Products.FirstOrDefault(x => x.Id == Id);

            string imageName = product.ImageName;
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/products", imageName);
            System.IO.File.Delete(imagePath);

            _appDbContext.Products.Remove(product);
            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


    }
}
