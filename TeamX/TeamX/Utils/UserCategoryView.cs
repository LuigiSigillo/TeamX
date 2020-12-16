using System;
using System.Drawing;

namespace TeamX.Utils
{
    public class UserCategoryView
    {
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public Color CategoryColor { get; set; }
        public string Description { get; set; }
        public bool IsAdmin { get; set; }

        public UserCategoryView(int category, string description, bool isAdmin)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                Description = "No description available";
            }
            else Description = description;
            CategoryId = category;
            Category = CategoryService.GetCategory(category);
            CategoryColor = CategoryService.GetColor(category);
            IsAdmin = isAdmin;

        }
    }
}
