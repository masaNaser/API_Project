using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository.Category;
using KASHOP.DAL.Repository.UnitOfWork;
using Mapster;
using System.Linq.Expressions;

namespace KASHOP.BLL.Services.Category
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Create(CategoryRequest request)
        {
            
            
                // تحويل الريكوست الى مودل عشان التعامل مع الداتا بيس 
                var categories = request.Adapt<DAL.Models.Category>();
                /*
                 يُرسل الكائن إلى الـ Repository للبدء في عملية الإضافة.
                خلف الكواليس: أثناء تنفيذ SaveChangesAsync داخل الـ DbContext، يتدخل كود الـ Audit تلقائياً لقرآءة الـ Token وتعيين CreatedById و CreatedDate.
                تحفظ قاعدة البيانات التصنيف وتولد له رقم معرف جديد (Id).
                النتيجة المُرجعة في المتغير savedCategory تحتوي على البيانات الأساسية والـ Id الجديد، لكن خاصية CreatedBy (كائن المستخدم) تكون لا تزال null لأن EF Core لا يجلب العلاقات تلقائياً أثناء الحفظ.
                 */
                var savedCategory = await _unitOfWork.CategoryRepository.CreateAsync(categories);
                    await _unitOfWork.CompleteAsync();
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
        public async Task<Result<List<CategoryResponse>>> GetAllCategories()
        {
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync(null,new string[]
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
            
        

        public async Task<Result<CategoryResponse>> GetCategory(Expression<Func<DAL.Models.Category, bool>> filter)
        {
                var category = await _unitOfWork.CategoryRepository.GetOneAsync(filter, new string[]
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

        public async Task<Result<CategoryResponse>> Update(int id, CategoryRequest request)
        {
                var category = await _unitOfWork.CategoryRepository.GetOneAsync(c => c.Id == id, new string[]
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
                _unitOfWork.CategoryRepository.UpdateAsync(category);
                await _unitOfWork.CompleteAsync();
                var updatedCategory = await GetCategory(c => c.Id == id);
                //return updatedCategory;
             
                return Result<CategoryResponse>.SuccessResult(updatedCategory.Data, "Category updated successfully.");
        }

        public async Task<Result<bool>> Delete(int id)
        {
                var category = await _unitOfWork.CategoryRepository.GetOneAsync(c => c.Id == id);
                if (category == null)
                {
                   
                    return Result<bool>.FailureResult("Category not found");
                }
                var deleted = _unitOfWork.CategoryRepository.DeleteAsync(category);
            await _unitOfWork.CompleteAsync();
            return Result<bool>.SuccessResult(deleted, deleted ? "Category deleted successfully." : "Failed to delete category.");
        }

    }
}
