using System.ComponentModel.DataAnnotations;

namespace PurchaseBillAPI.Models;

public class LocationDetail
{
    [Key]
    public string Location_Code { get; set; } = string.Empty;
    public string Location_Name { get; set; } = string.Empty;
}