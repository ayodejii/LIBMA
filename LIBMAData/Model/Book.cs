using LIBMAData.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LIBMAData
{
    public class Book: LibraryAsset
    {
        [Required][Display(Name = "ISBN #")] public string ISBN { get; set; }
        [Required] public string Author { get; set; }
        [Required][Display(Name = "DDC")]public string DeweyIndex { get; set; }
    }
}
