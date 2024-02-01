/*
 * Author: David Beltran
 * This file contains the LocationViewModel class. 
 */
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompStoreWeb.Models
{
    public class LocationViewModel
    {
        public string? SelectedCity { get; set; }
        // Display property
        public List<SelectListItem>? LocationsSelectList { get; set; }
    }
}
