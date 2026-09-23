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

        public ProductServices(IFileServices fileServices,IProductRepository productRepository)
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
                    return new Result<bool>
                    {
                        Success = false,
                        Message = "Main image is required.",
                    };
                }
                var mainImageUrl = await _fileServices.UploadFileAsync(request.MainImage);
                if (!mainImageUrl.Success)
                {
                    return new Result<bool>
                    {
                        Success = false,
                        Message = "Failed to upload main image.",
                    };
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
                return new Result<bool>
                {
                    Success = true,
                    Message = "Success"
                };

            }
            catch (Exception ex)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.InnerException?.Message ?? ex.Message}"
                };
            }
        }
        public async Task<Result<List<ProductListResponse>>> GetAllProducts(Expression<Func<DAL.Models.Product, bool>>? filter=null)
        {
            try
            {
                var products = await _productRepository.GetAllAsync(filter,new string[]
                {
                    nameof(DAL.Models.Product.Translations),
                    nameof(DAL.Models.Product.Brand),
                 $"{nameof(DAL.Models.Product.Category)}.{nameof(DAL.Models.Category.Translations)}",
                    nameof(DAL.Models.Product.SubImages),
                    nameof(DAL.Models.Product.CreatedBy)
                });
                return new Result<List<ProductListResponse>>
                {
                    Success = true,
                    Message = "Success",
                    Data = products.Adapt<List<ProductListResponse>>()
                };
            }
            catch (Exception ex)
            {
                return new Result<List<ProductListResponse>>
                {
                    Success = false,
                    Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
                };
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
                if (product == null)
                {
                    return new Result<ProductDetailsResponse>
                    {
                        Success = false,
                        Message = "Product not found",
                    };
                }
                return new Result<ProductDetailsResponse>
                {
                    Success = true,
                    Message = "Success",
                    Data = product.Adapt<ProductDetailsResponse>()
                };
            }
            catch (Exception ex)
            {
                return new Result<ProductDetailsResponse>
                {
                    Success = false,
                    Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
                };
            }
        }

        public Task<Result<bool>> Delete(int id)
        {
            throw new NotImplementedException();
        }       
        public Task<Result<ProductListResponse>> Update(int id, ProductRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
