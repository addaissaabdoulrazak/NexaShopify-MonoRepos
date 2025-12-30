using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core.Shop.Enums
{
    class ProductEnums
    {
        public enum ProductStatus : int
        {
            [Description("Draft")]
            Draft = 1, // Brouillon
            [Description("Active")]   
            Active = 2, // Actif
            [Description("Archived")]  
            Archived = 3 // Archivé
        }

        public enum ProductType
        {
            Physical, // Physique
            Digital,  // Numérique
            Service   // Service
        }
    }
}
