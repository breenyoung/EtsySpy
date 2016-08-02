using System;
using System.Collections.Generic;

namespace EtsySpy.Classes
{
    public class EtsyProduct
    {
        public int ListingId { get; set; }

        public string State { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime OriginalCreationTsz { get; set; }

        public DateTime EndingTsz { get; set; }

        public DateTime LastModifiedTsz { get; set; }

        public DateTime StateTsz { get; set; }

        public string Price { get; set; }

        public int Quantity { get; set; }

        public string Url { get; set; }

        public int Views { get; set; }

        public int NumFavorers { get; set; }

        public bool ShouldAutoRenew { get; set; }

        public List<string> CategoryPath { get; set; }

        public List<string> TaxonomyPath { get; set; }

        public List<string> Tags { get; set; } 

        public EtsyProduct()
        {
            
        }

    }
}
