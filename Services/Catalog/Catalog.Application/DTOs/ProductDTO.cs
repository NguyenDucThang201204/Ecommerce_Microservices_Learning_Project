using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.DTOs
{
    public record ProductDTO(
        string id,
        string name,
        string summary,
        string description,
        string imageFile,
        BrandDTO brand,
        TypeDTO type,
        decimal price,
        DateTimeOffset createdDate
        );

    public record BrandDTO(
        string id,
        string name
        );

    public record TypeDTO(
        string id,
        string name
        );

    // tương tự Record còn record Struct là một kiểu dữ liệu tham trị (value type) thay vì kiểu dữ liệu tham chiếu (reference type) như record class và nằm trong heap.
    // Record struct được sử dụng khi tạo ra các đối tượng nhỏ, nhẹ và nằm trong stack. Record struct cũng hỗ trợ các tính năng giống như record class, bao gồm so sánh giá trị, sao chép và khởi tạo với cú pháp ngắn gọn.
    public record class CreateProductDTO
    {
        // init: ko cho set lại giá trị sau khi khởi tạo (chỉ cho phép set lúc mới tạo lần đầu)
        [Required]
        public string Name { get; init; }
        [Required]
        public string Summary { get; init; }
        [Required]
        public string Description { get; init; }
        [Required]
        public string ImageFile { get; init; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; init; }
        [Required]
        public string BrandId { get; init; }
        [Required]
        public string TypeId { get; init; }

    }

    public record class UpdateProductDTO
    {
        // init: ko cho set lại giá trị sau khi khởi tạo (chỉ cho phép set lúc mới tạo lần đầu)
        [Required]
        public string Name { get; init; }
        [Required]
        public string Summary { get; init; }
        [Required]
        public string Description { get; init; }
        [Required]
        public string ImageFile { get; init; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; init; }
        [Required]
        public string BrandId { get; init; }
        [Required]
        public string TypeId { get; init; }

    }
}
