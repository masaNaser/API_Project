using KASHOP.BLL.Services.FileServices;
using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Repository.Product;
using Mapster;
using System.Linq.Expressions;

namespace KASHOP.BLL.Services.Product
{
    public class ProductServices : IProductServices
    {
        private readonly IFileServices _fileServices;
        private readonly IProductRepository _productRepository;

        public ProductServices(IFileServices fileServices, IProductRepository productRepository)
        {
            _fileServices = fileServices;
            _productRepository = productRepository;
        }
        public async Task<Result<bool>> CreateProduct(ProductRequest request)
        {
            try
            {
                if (request.MainImage == null || request.MainImage.Length == 0)
                {
                //    return new Result<bool>
                //    {
                //        Success = false,
                //        Message = "Main image is required.",
                //    };
                    return Result<bool>.FailureResult("Main image is required.");
            }
                var mainImageUrl = await _fileServices.UploadFileAsync(request.MainImage);
                if (!mainImageUrl.Success)
                {
               
                return Result<bool>.FailureResult("Failed to upload main image.");
            }
                var subImageUrls = new List<string>();
                //هل قام المستخدم بإرسال قائمة صور فرعية من الأساس
                if (request.SubImages != null && request.SubImages.Any())
                {
                    foreach (var image in request.SubImages)
                    {
                        //هل هذه الصورة المحددة داخل القائمة صالحة ولها حجم، أم أنها ملف فارغ / معطوب؟
                        if (image != null && image.Length > 0)
                        {
                            var subImageUrl = await _fileServices.UploadFileAsync(image);
                            if (subImageUrl.Success)
                            {
                                //ضفنا الصور الفرعية ع ليست عشان نقدر نعمل ال مابينج ونخزنهن بالداتا بيس
                                subImageUrls.Add(subImageUrl.Data);
                            }
                        }

                    }
                }
                var product = request.Adapt<DAL.Models.Product>();
                product.MainImage = mainImageUrl.Data;
                if (subImageUrls.Any())
                {
                    product.SubImages = subImageUrls.Select(url => new DAL.Models.ProductImage
                    {
                        ImageUrl = url
                    }).ToList();
                }
                var result = await _productRepository.CreateAsync(product);
                return Result<bool>.SuccessResult(true, "Product created successfully.");

            }
            catch (Exception ex)
            {
            //    return new Result<bool>
            //    {
            //        Success = false,
            //        Message = $"An error occurred: {ex.InnerException?.Message ?? ex.Message}"
            //    };
                return Result<bool>.FailureResult($"An error occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
        public async Task<Result<List<ProductListResponse>>> GetAllProducts(Expression<Func<DAL.Models.Product, bool>>? filter = null)
        {
            try
            {
                var products = await _productRepository.GetAllAsync(filter, new string[]
                {
                    nameof(DAL.Models.Product.Translations),
                    nameof(DAL.Models.Product.Brand),
                 $"{nameof(DAL.Models.Product.Category)}.{nameof(DAL.Models.Category.Translations)}",
                    nameof(DAL.Models.Product.SubImages),
                    nameof(DAL.Models.Product.CreatedBy)
                });
             
                return Result<List<ProductListResponse>>.SuccessResult(products.Adapt<List<ProductListResponse>>(), "Success");
            }
            catch (Exception ex)
            {
                return Result<List<ProductListResponse>>.FailureResult(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        public async Task<Result<ProductDetailsResponse>> GetProduct(Expression<Func<DAL.Models.Product, bool>> filter)
        {
            try
            {
                var product = await _productRepository.GetOneAsync(filter, new string[]
                {
                    nameof(DAL.Models.Product.Translations),
                    nameof(DAL.Models.Product.Brand),
                 $"{nameof(DAL.Models.Product.Category)}.{nameof(DAL.Models.Category.Translations)}",
                    nameof(DAL.Models.Product.SubImages),
                    nameof(DAL.Models.Product.CreatedBy)
                });
                if (product == null) return Result<ProductDetailsResponse>.FailureResult("Product not found");
               
                return Result<ProductDetailsResponse>.SuccessResult(product.Adapt<ProductDetailsResponse>(), "Success");
            }
            catch (Exception ex)
            {
                return Result<ProductDetailsResponse>.FailureResult(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        public Task<Result<bool>> Delete(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<Result<ProductListResponse>> Update(int id, ProductRequest request)
        {
            try
            {
                var product = await _productRepository.GetOneAsync(p => p.Id == id);
                if (product == null)
                {
                    return Result<ProductListResponse>.FailureResult("Product not found");
                }

                // إنشاء إعداد خاص لـ Mapster يمنع نسخ الـ null
                var config = new TypeAdapterConfig();
                config.ForType<ProductRequest,DAL.Models.Product>()
                      .IgnoreNullValues(true) // طنّش أي حقل جاي null وخلّي القيمة القديمة
                      .Ignore(dest => dest.MainImage); // استثني مسار الصورة عشان نعالجه يدوياً

                // تنفيذ عملية المابينغ
                request.Adapt(product, config);

                // معالجة الصورة بشكل مستقل
                if (request.MainImage != null && request.MainImage.Length > 0)
                {
                    var uploadResult = await _fileServices.UploadFileAsync(request.MainImage);
                    if (uploadResult.Success)
                    {
                        // 3. مسح الصورة القديمة لو كانت موجودة (في حالة التعديل)
                        if (!string.IsNullOrEmpty(product.MainImage))
                        {
                            // كود حذف الصورة القديمة
                        }

                        // 4. تعيين اسم الملف أو المسار المرجع في خاصية الكيان
                        product.MainImage = uploadResult.Data;
                    }
                    else
                    {
                        // إرجاع خطأ في حال فشل رفع الصورة (مثلاً الامتداد غير مسموح أو الحجم كبير)
                        return Result<ProductListResponse>.FailureResult(uploadResult.Message);
                    }
                }

                await _productRepository.UpdateAsync(product);
                return Result<ProductListResponse>.SuccessResult(null,"Product updated successfully");
            }
            catch (Exception ex)
            {
                return Result<ProductListResponse>.FailureResult($"An error occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
