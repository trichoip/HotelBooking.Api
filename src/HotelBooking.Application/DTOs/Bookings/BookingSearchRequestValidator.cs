﻿using FluentValidation;

namespace HotelBooking.Application.DTOs.Bookings;
public class BookingSearchRequestValidator : AbstractValidator<BookingSearchRequest>
{
    public BookingSearchRequestValidator()
    {
        RuleFor(x => x.CheckInDate)
            .LessThanOrEqualTo(x => x.CheckOutDate).WithMessage("CheckInDate must be less than CheckOutDate")
            .GreaterThanOrEqualTo(x => DateOnly.FromDateTime(DateTime.Now)).WithMessage("CheckInDate must be greater than today");

        RuleFor(x => x.CheckOutDate)
            .GreaterThanOrEqualTo(x => x.CheckInDate).WithMessage("CheckOutDate must be greater than CheckInDate")
            .GreaterThanOrEqualTo(x => DateOnly.FromDateTime(DateTime.Now)).WithMessage("CheckOutDate must be greater than today");
    }
}
