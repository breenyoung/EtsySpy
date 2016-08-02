using System;
using System.Collections.Generic;

namespace EtsySpy.Classes
{
    public class EtsyShop
    {
        public int ShopId { get; set; }

        public int UserId { get; set; }

        public string ShopName { get; set; }

        public DateTime CreationTsz { get; set; }

        public bool IsVacation { get; set; }

        public string VacationMessage { get; set; }

        public DateTime LastUpdatedTsz { get; set; }

        public string LoginName { get; set; }

        public bool AcceptsCustomRequests { get; set; }

        public string Url { get; set; }

        public int NumFavorers { get; set; }

        public List<EtsyProduct> Listings { get; set; } 

        public EtsyShop()
        {
            
        }
    }
}
