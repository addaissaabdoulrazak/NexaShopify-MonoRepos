using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class ProductMediaEntity
    {
		public string Alt { get; set; }
		public int DisplayOrder { get; set; }
		public int Id { get; set; }
		public int ProductId { get; set; }
		public long? Size { get; set; }
		public string Type { get; set; }
		public string Url { get; set; }

        public ProductMediaEntity() { }

        public ProductMediaEntity(DataRow dataRow)
        {
			Alt = (dataRow["Alt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Alt"]);
			DisplayOrder = Convert.ToInt32(dataRow["DisplayOrder"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ProductId = Convert.ToInt32(dataRow["ProductId"]);
			Size = (dataRow["Size"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["Size"]);
			Type = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
			Url = Convert.ToString(dataRow["Url"]);
        }
    }
}

