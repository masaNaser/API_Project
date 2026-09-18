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

                return new Result<bool>
                {
                    Success = true,
                    Message = "Success",
                    //Data = true,
                };
            }
            catch (Exception ex)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message,
                    //Data = false
                };
                }
        }
        public async Task<Result<List<CategoryResponse>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync(new string[]
                {
                (nameof(DAL.Models.Category.Translations)),
                    (nameof(DAL.Models.Category.CreatedBy))
                }); 
                // category
                    //بجيب اللغة بشكل تلقائي بدون ما نرسل اي قيمة بالريكوست 
                    //var lang = CultureInfo.CurrentUICulture.Name;
                    // حذفناها لانه ما رح نحتاجها لانه عدلنا بكود ال mapster عشان يجيب الترجمة بشكل تلقائي حسب اللغة الحالية
                return new Result<List<CategoryResponse>>
                {
                    Success = true,
                    Message = "Success",
                    Data = categories.Adapt<List<CategoryResponse>>()
                };
            }
            catch (Exception ex)
            {
                return new Result<List<CategoryResponse>>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                    Data = null
                };
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
                    return new Result<CategoryResponse>
                    {
                        Success = false,
                        Message = "Category not found",
                        Data = null
                    };
                }
                return new Result<CategoryResponse>
                {
                    Success = true,
                    Message = "Success",
                    Data = category.Adapt<CategoryResponse>()
                };
            }
            catch (Exception ex)
            {
                return new Result<CategoryResponse>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                    Data = null
                };
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
                    return new Result<CategoryResponse>
                    {
                        Success = false,
                        Message = "Category not found",
                        Data = null
                    };
                }
                //بنستخدم Mapster لتحديث خصائص الكائن الحالي بالقيم الجديدة من الريكوست
                request.Adapt(category);
                await _categoryRepository.UpdateAsync(category);
                var updatedCategory = await GetCategory(c => c.Id == id);
                //return updatedCategory;
                return new Result<CategoryResponse>
                {
                    Success = true,
                    Message = "Success",
                    Data = updatedCategory.Data
                };
            }
            catch (Exception ex)
            {
                return new Result<CategoryResponse>
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message,  
                    Data = null
                };
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var category = await _categoryRepository.GetOneAsync(c => c.Id == id);
                if (category == null)
                {
                    return new Result<bool>
                    {
                        Success = false,
                        Message = "Category not found",
                        Data = false
                    };
                }
                var deleted = await _categoryRepository.DeleteAsync(category);
                return new Result<bool>
                {
                    Success = deleted,
                    Message = deleted ? "Success" : "Faild to delete category",
                    Data = deleted
                };
            }
            catch (Exception ex)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message,
                    Data = false
                };
            }
        }

    }
}
