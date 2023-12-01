[GroupBy](./src/HotelBooking.Infrastructure/Services/HotelService.cs) 

```c#
protected async Task<List<MatrixItem>> CreateMatrixItems(
        HotelSearchWithSpecification specification,
        FilterKey filterKey,
        FilterRequestType filterRequestType)
    {
        var hotels = await _unitOfWork.Repository<Hotel>()
              .FindAsync(
                  expression: specification.Criteria,
                  includeFunc: _ => _.Include(_ => _.Address.Area)
                                     .Include(_ => _.Category));

        var groupFilter = filterKey switch
        {
            // lưu ý: nếu GroupBy là class thì class đó phải có Equals GetHashCode
            // nếu dùng new GroupFilter phải có Equals GetHashCode trong GroupFilter
            // nếu dùng _.Category phải có Equals GetHashCode trong GroupFilter Category
            // nếu không muốn Equals GetHashCode thì dùng anonymous funtion new { } , lưu ý tất cả các case phải có param anonymous funtion new { } giống nhau
            FilterKey.HotelAreaId => hotels.GroupBy(_ => new
            {
                _.Address.Area.Id,
                _.Address.Area.Name,
            }),
            FilterKey.AccommodationType => hotels.GroupBy(_ => new
            {
                _.Category.Id,
                _.Category.Name,
            }),
            FilterKey.StarRating => hotels.GroupBy(_ => new
            {
                // do không có properties name Id , Name nên phải map
                Id = _.ReviewRating,
                Name = $"{_.ReviewRating} rating",
            }),
            _ => throw new BadRequestException($"`{filterKey}` not supporter for response filter")
        };

        return groupFilter.Select(_ => new MatrixItem
        {
            Id = _.Key.Id,
            Name = _.Key.Name,
            Count = _.Count(),
            FilterKey = filterKey,
            FilterRequestType = filterRequestType,
        }).OrderByDescending(_ => _.Count).ThenBy(_ => _.Id).ToList();
    }
```

[Vnpay-Library](./src/HotelBooking.Application/Helpers/VnPayLibrary.cs)  
[Vnpay-Controller](./src/HotelBooking.WebApi/Controllers/PaymentsController.cs)  
[Vnpay-Service](./src/HotelBooking.Infrastructure/Services/PaymentService.cs)  


[Swagger-SecurityRequirementsOperationFilterForNotAuthorize](./src/HotelBooking.WebApi/Swagger/SecurityRequirementsOperationFilterForNotAuthorize.cs)  

```c#
c.OperationFilter<SecurityRequirementsOperationFilterForNotAuthorize>();
```


