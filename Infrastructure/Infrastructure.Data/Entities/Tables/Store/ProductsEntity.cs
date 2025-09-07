using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class ProductsEntity
    {
		public string Barcode { get; set; }
		public decimal? CompareAtPrice { get; set; }
		public bool ContinueSelling { get; set; }
		public decimal? CostPerItem { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Description { get; set; }
		public int Id { get; set; }
		public bool IsAvailableAtPointOfSale { get; set; }
		public bool IsAvailableOnOnlineStore { get; set; }
		public bool IsTaxable { get; set; }
		public decimal? Price { get; set; }
		public int Quantity { get; set; }
		public string SeoDescription { get; set; }
		public string SeoTitle { get; set; }
		public string SKU { get; set; }
		public string Slug { get; set; }
		public string Status { get; set; }
		public string Title { get; set; }
		public bool TrackQuantity { get; set; }
		public string Type { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string Vendor { get; set; }

        public ProductsEntity() { }

        public ProductsEntity(DataRow dataRow)
        {
			Barcode = (dataRow["Barcode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Barcode"]);
			CompareAtPrice = (dataRow["CompareAtPrice"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CompareAtPrice"]);
			ContinueSelling = Convert.ToBoolean(dataRow["ContinueSelling"]);
			CostPerItem = (dataRow["CostPerItem"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CostPerItem"]);
			CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsAvailableAtPointOfSale = Convert.ToBoolean(dataRow["IsAvailableAtPointOfSale"]);
			IsAvailableOnOnlineStore = Convert.ToBoolean(dataRow["IsAvailableOnOnlineStore"]);
			IsTaxable = Convert.ToBoolean(dataRow["IsTaxable"]);
			Price = (dataRow["Price"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Price"]);
			Quantity = Convert.ToInt32(dataRow["Quantity"]);
			SeoDescription = (dataRow["SeoDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SeoDescription"]);
			SeoTitle = (dataRow["SeoTitle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SeoTitle"]);
			SKU = (dataRow["SKU"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SKU"]);
			Slug = (dataRow["Slug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Slug"]);
			Status = Convert.ToString(dataRow["Status"]);
			Title = Convert.ToString(dataRow["Title"]);
			TrackQuantity = Convert.ToBoolean(dataRow["TrackQuantity"]);
			Type = Convert.ToString(dataRow["Type"]);
			UpdatedAt = Convert.ToDateTime(dataRow["UpdatedAt"]);
			Vendor = (dataRow["Vendor"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vendor"]);
        }
    }
}

