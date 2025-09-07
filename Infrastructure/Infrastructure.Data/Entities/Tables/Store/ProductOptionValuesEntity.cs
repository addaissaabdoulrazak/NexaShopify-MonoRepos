using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class ProductOptionValuesEntity
    {
		public int Id { get; set; }
		public int OptionId { get; set; }
		public int Position { get; set; }
		public string Value { get; set; }

        public ProductOptionValuesEntity() { }

        public ProductOptionValuesEntity(DataRow dataRow)
        {
			Id = Convert.ToInt32(dataRow["Id"]);
			OptionId = Convert.ToInt32(dataRow["OptionId"]);
			Position = Convert.ToInt32(dataRow["Position"]);
			Value = Convert.ToString(dataRow["Value"]);
        }
    }
}

