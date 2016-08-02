using System.Collections.Generic;

namespace EtsySpy.Classes
{
    public class EtsyProductResults
    {
        public int Count { get; set; }
        
        public List<EtsyProduct> Results { get; set; }

        public EtsyProductResults()
        {
            
        }
    }
}
