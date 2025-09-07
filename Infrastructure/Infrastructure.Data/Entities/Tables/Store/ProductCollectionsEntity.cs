using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class ProductCollectionsEntity
    {
		public string CollectionName { get; set; }
		public int Id { get; set; }
		public int ProductId { get; set; }

        public ProductCollectionsEntity() { }

        public ProductCollectionsEntity(DataRow dataRow)
        {
			CollectionName = Convert.ToString(dataRow["CollectionName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ProductId = Convert.ToInt32(dataRow["ProductId"]);
        }
    }
}

