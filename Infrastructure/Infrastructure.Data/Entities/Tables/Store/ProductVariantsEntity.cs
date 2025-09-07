using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class ProductVariantsEntity
    {
		public bool Available { get; set; }
		public string Barcode { get; set; }
		public decimal? CompareAtPrice { get; set; }
		public decimal? CostPerItem { get; set; }
		public int Id { get; set; }
		public string OptionsJson { get; set; }			public decimal Price { get; set; }
		public int ProductId { get; set; }
		public string SKU { get; set; }
		public int Stock { get; set; }

        public ProductVariantsEntity() { }

        public ProductVariantsEntity(DataRow dataRow)
        {
			Available = Convert.ToBoolean(dataRow["Available"]);
			Barcode = (dataRow["Barcode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Barcode"]);
			CompareAtPrice = (dataRow["CompareAtPrice"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CompareAtPrice"]);
			CostPerItem = (dataRow["CostPerItem"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CostPerItem"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			OptionsJson = (dataRow["OptionsJson"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OptionsJson"]);
			Price = Convert.ToDecimal(dataRow["Price"]);
			ProductId = Convert.ToInt32(dataRow["ProductId"]);
			SKU = (dataRow["SKU"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SKU"]);
			Stock = Convert.ToInt32(dataRow["Stock"]);
        }
    }
}

