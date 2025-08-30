namespace Cosmetic.Models.ViewModels
{
    public class CategoryManagementViewModel
    {
        public List<Category> Categories { get; set; }

        public CategoryCreateViewModel NewCategory { get; set; }
    }
}
