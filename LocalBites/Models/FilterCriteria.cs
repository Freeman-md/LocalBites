using System.ComponentModel.DataAnnotations;

namespace LocalBites.Models;

public class FilterCriteria
{
    [EnumDataType(typeof(Cuisine))]
    public Cuisine? Cuisine { get; set; }
    
    [EnumDataType(typeof(Location))]
    public Location? Location { get; set; }
}
