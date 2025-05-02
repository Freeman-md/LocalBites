using System.ComponentModel.DataAnnotations;
using LocalBites.Models.Enums;

namespace LocalBites.Models;

public class FilterCriteria
{
    [EnumDataType(typeof(Cuisine))]
    public Cuisine? Cuisine { get; set; }
    
    [EnumDataType(typeof(Location))]
    public Location? Location { get; set; }
}
