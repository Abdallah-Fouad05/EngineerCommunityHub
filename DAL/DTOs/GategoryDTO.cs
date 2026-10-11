using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; //IFormFile

namespace DAL.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public string? Image {  get; set; }
        public CategoryDTO(int categoryId, string categoryName, string? description, string? image)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            Description = description;
            Image = image;
        }
    }

    public class category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public category(int categoryId,string categoryName)
        {
            CategoryId = categoryId; CategoryName = categoryName;
        }
    }
    public class Category_Created
    {
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public Category_Created(string categoryName, string? description, string? image)
        {
            CategoryName = categoryName;
            Description = description;
            Image = image;
        }
    }

    public class Category_Created_Request()
    {
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class Category_Updated_Request()
    {

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }


}
