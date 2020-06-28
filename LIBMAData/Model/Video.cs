using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LIBMAData.Model
{
    public class Video: LibraryAsset
    {
        [Required] public string Director { get; set; }
    }
}
