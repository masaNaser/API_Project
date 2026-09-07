using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository.Category;
using Mapster;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Category
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryServices(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<List<CategoryResponse>> Create(CategoryRequest request)
        {
            // تحويل الريكوست الى مودل عشان التعامل مع الداتا بيس 
            var categories = request.Adapt<DAL.Models.Category>();
            /*
             يُرسل الكائن إلى الـ Repository للبدء في عملية الإضافة.
خلف الكواليس: أثناء تنفيذ SaveChangesAsync داخل الـ DbContext، يتدخل كود الـ Audit تلقائياً لقرآءة الـ Token وتعيين CreatedById و CreatedDate.
تحفظ قاعدة البيانات التصنيف وتولد له رقم معرف جديد (Id).
النتيجة المُرجعة في المتغير savedCategory تحتوي على البيانات الأساسية والـ Id الجديد، لكن خاصية CreatedBy (كائن المستخدم) تكون لا تزال null لأن EF Core لا يجلب العلاقات تلقائياً أثناء الحفظ.
             */
            var savedCategory = await _categoryRepository.Create(categories);
            //هاد السطر عشان نرجع الكائن مع الـ CreatedById و CreatedDate و CreatedBy (كائن المستخدم) بعد الحفظ
            // لانه هدول القيم قبل الحفظ بكونو نلل بالتالي بنحتاج نعمل انكلود عشان نجيبهم من الداتا بيس 
            var categoryResponse = await GetCategory(c => c.Id == savedCategory.Id);

            return new List<CategoryResponse> { categoryResponse };
        }

        public async Task<List<CategoryResponse>> GetAllCategories()
        {
            var categories =await _categoryRepository.GetAll(new string[]
            {
                (nameof(DAL.Models.Category.Translations)),(nameof(DAL.Models.Category.CreatedBy))
            }); // category
                //بجيب اللغة بشكل تلقائي بدون ما نرسل اي قيمة بالريكوست 
                //var lang = CultureInfo.CurrentUICulture.Name;
                // حذفناها لانه ما رح نحتاجها لانه عدلنا بكود ال mapster عشان يجيب الترجمة بشكل تلقائي حسب اللغة الحالية

            return categories.Adapt<List<CategoryResponse>>();

        }

        public async Task<CategoryResponse> GetCategory(Expression<Func<DAL.Models.Category, bool>> filter)
        {
            var category = await _categoryRepository.GetOne(filter, new string[]
            {
                nameof(DAL.Models.Category.Translations),
                nameof(DAL.Models.Category.CreatedBy) // أضفناها هنا
            });

            return category.Adapt<CategoryResponse>();
        }

        public async Task<CategoryResponse> Update(int id, CategoryRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
          var category =await _categoryRepository.GetOne(c => c.Id == id);
            if (category == null)
            {
                return false;
            }
            await _categoryRepository.Delete(id);
            return true;

        }
    }
}
