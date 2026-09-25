using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository.Category;
using Mapster;
using System.Linq.Expressions;

namespace KASHOP.BLL.Services.Category
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryServices(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<Result<bool>> Create(CategoryRequest request)
        {
            try
            {
                // تحويل الريكوست الى مودل عشان التعامل مع الداتا بيس 
                var categories = request.Adapt<DAL.Models.Category>();
                /*
                 يُرسل الكائن إلى الـ Repository للبدء في عملية الإضافة.
                خلف الكواليس: أثناء تنفيذ SaveChangesAsync داخل الـ DbContext، يتدخل كود الـ Audit تلقائياً لقرآءة الـ Token وتعيين CreatedById و CreatedDate.
                تحفظ قاعدة البيانات التصنيف وتولد له رقم معرف جديد (Id).
                النتيجة المُرجعة في المتغير savedCategory تحتوي على البيانات الأساسية والـ Id الجديد، لكن خاصية CreatedBy (كائن المستخدم) تكون لا تزال null لأن EF Core لا يجلب العلاقات تلقائياً أثناء الحفظ.
                 */
                var savedCategory = await _categoryRepository.CreateAsync(categories);
                //هاد السطر عشان نرجع الكائن مع الـ CreatedById و CreatedDate و CreatedBy (كائن المستخدم) بعد الحفظ
                // لانه هدول القيم قبل الحفظ بكونو نلل بالتالي بنحتاج نعمل انكلود عشان نجيبهم من الداتا بيس 
                // var categoryResponse = await GetCategory(c => c.Id == savedCategory.Id);

                //return new Result<bool>
                //{
                //    Success = true,
                //    Message = "Success",
                //    //Data = true,
                //};
                return Result<bool>.SuccessResult(false,"Category created successfully.");
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<Result<List<CategoryResponse>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync(null,new string[]
                {
                (nameof(DAL.Models.Category.Translations)),
                    (nameof(DAL.Models.Category.CreatedBy))
                }); 
                // category
                    //بجيب اللغة بشكل تلقائي بدون ما نرسل اي قيمة بالريكوست 
                    //var lang = CultureInfo.CurrentUICulture.Name;
                    // حذفناها لانه ما رح نحتاجها لانه عدلنا بكود ال mapster عشان يجيب الترجمة بشكل تلقائي حسب اللغة الحالية
                return Result<List<CategoryResponse>>.SuccessResult(categories.Adapt<List<CategoryResponse>>(), "Success");
            }
            catch (Exception ex)
            {
                return Result<List<CategoryResponse>>.FailureResult(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<Result<CategoryResponse>> GetCategory(Expression<Func<DAL.Models.Category, bool>> filter)
        {
            try
            {
                var category = await _categoryRepository.GetOneAsync(filter, new string[]
                {
                nameof(DAL.Models.Category.Translations),
                nameof(DAL.Models.Category.CreatedBy)
                });
                if (category == null)
                {
                    return Result<CategoryResponse>.FailureResult("Category not found");
                }
                return Result<CategoryResponse>.SuccessResult(category.Adapt<CategoryResponse>(), "Success");
            }
            catch (Exception ex)
            {
                return Result<CategoryResponse>.FailureResult(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<Result<CategoryResponse>> Update(int id, CategoryRequest request)
        {
            try
            {
                var category = await _categoryRepository.GetOneAsync(c => c.Id == id, new string[]
                {
                nameof(DAL.Models.Category.Translations),
                nameof(DAL.Models.Category.CreatedBy)
                });
                if (category == null)
                {
                    return Result<CategoryResponse>.FailureResult("Category not found");
                }
                //بنستخدم Mapster لتحديث خصائص الكائن الحالي بالقيم الجديدة من الريكوست
                request.Adapt(category);
                await _categoryRepository.UpdateAsync(category);
                var updatedCategory = await GetCategory(c => c.Id == id);
                //return updatedCategory;
             
                return Result<CategoryResponse>.SuccessResult(updatedCategory.Data, "Category updated successfully.");
            }
            catch (Exception ex)
            {
              
                return Result<CategoryResponse>.FailureResult(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var category = await _categoryRepository.GetOneAsync(c => c.Id == id);
                if (category == null)
                {
                   
                    return Result<bool>.FailureResult("Category not found");
                }
                var deleted = await _categoryRepository.DeleteAsync(category);
              
                return Result<bool>.SuccessResult(deleted, deleted ? "Category deleted successfully." : "Failed to delete category.");
            }
            catch (Exception ex)
            {
              
                return Result<bool>.FailureResult(ex.InnerException?.Message ?? ex.Message);
            }
        }

    }
}
