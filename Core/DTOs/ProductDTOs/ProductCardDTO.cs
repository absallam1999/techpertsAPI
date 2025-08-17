using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechpertsSolutions.Core.Entities;
using Core.Utilities;

namespace Core.DTOs.ProductDTOs
{
    public class ProductCardDTO
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }  
        public string? ImageUrl { get; set; }
        public List<string>? ImageURLs { get; set; }
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public ProductCategory? CategoryEnum { get; set; }
        public string? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public List<SpecificationDTO>? Specifications { get; set; }
        public List<WarrantyDTO>? Warranties { get; set; }
        public string Status { get; set; } = "Pending";
        public string TechCompanyId { get; set; }
        public string? TechCompanyName { get; set; }
        public string TechCompanyAddress { get; set; } = null!;
        public string TechCompanyUserId { get; set; } = null!;
        public string TechCompanyImage { get; set; } = null!;
        //public ProductCardDTO(Product product)
        //{
        //    Id = product.Id;
        //    Name = product.Name;
        //    Price = product.Price;
        //    DiscountPrice = product.DiscountPrice;
        //    ImageUrl = product.ImageUrl;
        //    //ImageURLs = product.ImageURLs?.Select(img => img.ImageUrl).ToList();
        //    CategoryId = product.CategoryId;
        //    //CategoryName = product.CategoryName;
        //    //CategoryEnum = product.CategoryEnum;
        //    SubCategoryId = product.SubCategoryId;
        //    //SubCategoryName = product.SubCategoryName;
        //    Specifications = product.Specifications?.Select(spec => new SpecificationDTO(spec)).ToList();
        //    Warranties = product.Warranties?.Select(warranty => new WarrantyDTO(warranty)).ToList();
        //    //Status = product.Status.ToString();
        //    TechCompanyId = product.TechCompanyId;
        //    //TechCompanyName = product.TechCompanyName;
        //    //TechCompanyAddress = product.TechCompanyAddress ?? string.Empty;
        //    //TechCompanyUserId = product.TechCompanyUserId ?? string.Empty;
        //    //TechCompanyImage = product.TechCompanyImage ?? string.Empty;
        //}
    }
}
