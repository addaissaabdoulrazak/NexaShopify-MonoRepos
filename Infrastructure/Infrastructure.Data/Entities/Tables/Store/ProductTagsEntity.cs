using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class ProductTagsEntity
    {
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string TagName { get; set; }

        public ProductTagsEntity() { }

        public ProductTagsEntity(DataRow dataRow)
        {
			Id = Convert.ToInt32(dataRow["Id"]);
			ProductId = Convert.ToInt32(dataRow["ProductId"]);
			TagName = Convert.ToString(dataRow["TagName"]);
        }
    }
}

