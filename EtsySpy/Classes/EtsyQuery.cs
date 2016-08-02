using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace EtsySpy.Classes
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class EtsyQuery
    {
        public enum QueryTypes
        {
            Product,
            Shop
        }

        public string QueryText { get; set; }

        public string FriendlyName { get; set; }

        public QueryTypes QueryType {  get; set;}

        protected bool Equals(EtsyQuery other)
        {
            return other.QueryText == this.QueryText;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EtsyQuery) obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
