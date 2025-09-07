using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core.Shop.Enums
{
    class ProductEnums
    {
        public enum ProductStatus
        {
            Draft,    // Brouillon
            Active,   // Actif
            Archived  // Archivé
        }

        public enum ProductType
        {
            Physical, // Physique
            Digital,  // Numérique
            Service   // Service
        }
    }
}
