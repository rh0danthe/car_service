using System.ComponentModel.DataAnnotations;

namespace car_service.Dto;

public class CarDto
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int YearOfRelease { get; set; }
    [RegularExpression(@"^[A-HJ-NPR-Za-hj-npr-z\d]{8}[\dX][A-HJ-NPR-Za-hj-npr-z\d]{2}\d{6}$", ErrorMessage = "Inappropriate VIN code")]
    public string VINcode { get; set; }
}