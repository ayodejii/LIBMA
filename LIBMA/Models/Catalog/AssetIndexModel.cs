using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIBMA.Models.Catalog
{
    public class AssetIndexModel
    {
        public IEnumerable<AssetIndexListingModel> Assets { get; set; }
        public string Title { get; set; }
    }
}
