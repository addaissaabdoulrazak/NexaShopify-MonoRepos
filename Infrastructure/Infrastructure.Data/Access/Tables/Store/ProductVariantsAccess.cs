using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infrastructure.Data.Access.Tables
{

    public class ProductVariantsAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.ProductVariantsEntity Get(int id)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [ProductVariants] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.ProductVariantsEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [ProductVariants]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.ProductVariantsEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity> Get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity> get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                var dataTable = new DataTable();
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    
                    string queryIds = string.Empty;
                    for(int i=0; i<ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    sqlCommand.CommandText = $"SELECT * FROM [ProductVariants] WHERE [Id] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.ProductVariantsEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity>();
        }

        public static int Insert(Infrastructure.Data.Entities.Tables.ProductVariantsEntity item)
        {
            int response = int.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [ProductVariants] ([Available],[Barcode],[CompareAtPrice],[CostPerItem],[OptionsJson],[Price],[ProductId],[SKU],[Stock])  VALUES (@Available,@Barcode,@CompareAtPrice,@CostPerItem,@OptionsJson,@Price,@ProductId,@SKU,@Stock); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Available",item.Available);
					sqlCommand.Parameters.AddWithValue("Barcode",item.Barcode == null ? (object)DBNull.Value  : item.Barcode);
					sqlCommand.Parameters.AddWithValue("CompareAtPrice",item.CompareAtPrice == null ? (object)DBNull.Value  : item.CompareAtPrice);
					sqlCommand.Parameters.AddWithValue("CostPerItem",item.CostPerItem == null ? (object)DBNull.Value  : item.CostPerItem);
					sqlCommand.Parameters.AddWithValue("OptionsJson",item.OptionsJson == null ? (object)DBNull.Value  : item.OptionsJson);
					sqlCommand.Parameters.AddWithValue("Price",item.Price);
					sqlCommand.Parameters.AddWithValue("ProductId",item.ProductId);
					sqlCommand.Parameters.AddWithValue("SKU",item.SKU == null ? (object)DBNull.Value  : item.SKU);
					sqlCommand.Parameters.AddWithValue("Stock",item.Stock);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? int.MinValue:  int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
                int results=0;
                if(items.Count <= maxParamsNumber)
                {
                    results = insert(items);
                }else
                {
                    int batchNumber = items.Count / maxParamsNumber;
                    for(int i = 0; i < batchNumber; i++)
                    {
                        results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
                }
                return results;
            }

            return -1;
        }
        private static int insert(List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " INSERT INTO [ProductVariants] ([Available],[Barcode],[CompareAtPrice],[CostPerItem],[OptionsJson],[Price],[ProductId],[SKU],[Stock]) VALUES ( "

							+ "@Available"+ i +","
							+ "@Barcode"+ i +","
							+ "@CompareAtPrice"+ i +","
							+ "@CostPerItem"+ i +","
							+ "@OptionsJson"+ i +","
							+ "@Price"+ i +","
							+ "@ProductId"+ i +","
							+ "@SKU"+ i +","
							+ "@Stock"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("Available" + i, item.Available);
							sqlCommand.Parameters.AddWithValue("Barcode" + i, item.Barcode == null ? (object)DBNull.Value  : item.Barcode);
							sqlCommand.Parameters.AddWithValue("CompareAtPrice" + i, item.CompareAtPrice == null ? (object)DBNull.Value  : item.CompareAtPrice);
							sqlCommand.Parameters.AddWithValue("CostPerItem" + i, item.CostPerItem == null ? (object)DBNull.Value  : item.CostPerItem);
							sqlCommand.Parameters.AddWithValue("OptionsJson" + i, item.OptionsJson == null ? (object)DBNull.Value  : item.OptionsJson);
							sqlCommand.Parameters.AddWithValue("Price" + i, item.Price);
							sqlCommand.Parameters.AddWithValue("ProductId" + i, item.ProductId);
							sqlCommand.Parameters.AddWithValue("SKU" + i, item.SKU == null ? (object)DBNull.Value  : item.SKU);
							sqlCommand.Parameters.AddWithValue("Stock" + i, item.Stock);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.ProductVariantsEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [ProductVariants] SET [Available]=@Available, [Barcode]=@Barcode, [CompareAtPrice]=@CompareAtPrice, [CostPerItem]=@CostPerItem, [OptionsJson]=@OptionsJson, [Price]=@Price, [ProductId]=@ProductId, [SKU]=@SKU, [Stock]=@Stock WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Available",item.Available);
				sqlCommand.Parameters.AddWithValue("Barcode",item.Barcode == null ? (object)DBNull.Value  : item.Barcode);
				sqlCommand.Parameters.AddWithValue("CompareAtPrice",item.CompareAtPrice == null ? (object)DBNull.Value  : item.CompareAtPrice);
				sqlCommand.Parameters.AddWithValue("CostPerItem",item.CostPerItem == null ? (object)DBNull.Value  : item.CostPerItem);
				sqlCommand.Parameters.AddWithValue("OptionsJson",item.OptionsJson == null ? (object)DBNull.Value  : item.OptionsJson);
				sqlCommand.Parameters.AddWithValue("Price",item.Price);
				sqlCommand.Parameters.AddWithValue("ProductId",item.ProductId);
				sqlCommand.Parameters.AddWithValue("SKU",item.SKU == null ? (object)DBNull.Value  : item.SKU);
				sqlCommand.Parameters.AddWithValue("Stock",item.Stock);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
                int results = 0;
                if(items.Count <= maxParamsNumber)
                {
                    results = update(items);
                }else
                {
                    int batchNumber = items.Count / maxParamsNumber;
                    for(int i = 0; i < batchNumber; i++)
                    {
                        results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
                }

                return results;
            }

            return -1;
        }
        private static int update(List<Infrastructure.Data.Entities.Tables.ProductVariantsEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " UPDATE [ProductVariants] SET "

							+ "[Available]=@Available"+ i +","
							+ "[Barcode]=@Barcode"+ i +","
							+ "[CompareAtPrice]=@CompareAtPrice"+ i +","
							+ "[CostPerItem]=@CostPerItem"+ i +","
							+ "[OptionsJson]=@OptionsJson"+ i +","
							+ "[Price]=@Price"+ i +","
							+ "[ProductId]=@ProductId"+ i +","
							+ "[SKU]=@SKU"+ i +","
							+ "[Stock]=@Stock"+ i +" WHERE [Id]=@Id" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
							sqlCommand.Parameters.AddWithValue("Available" + i, item.Available);
							sqlCommand.Parameters.AddWithValue("Barcode" + i, item.Barcode == null ? (object)DBNull.Value  : item.Barcode);
							sqlCommand.Parameters.AddWithValue("CompareAtPrice" + i, item.CompareAtPrice == null ? (object)DBNull.Value  : item.CompareAtPrice);
							sqlCommand.Parameters.AddWithValue("CostPerItem" + i, item.CostPerItem == null ? (object)DBNull.Value  : item.CostPerItem);
							sqlCommand.Parameters.AddWithValue("OptionsJson" + i, item.OptionsJson == null ? (object)DBNull.Value  : item.OptionsJson);
							sqlCommand.Parameters.AddWithValue("Price" + i, item.Price);
							sqlCommand.Parameters.AddWithValue("ProductId" + i, item.ProductId);
							sqlCommand.Parameters.AddWithValue("SKU" + i, item.SKU == null ? (object)DBNull.Value  : item.SKU);
							sqlCommand.Parameters.AddWithValue("Stock" + i, item.Stock);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(int id)
        {
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [ProductVariants] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);

                results = sqlCommand.ExecuteNonQuery();
            }

            return results;
        }
        public static int Delete(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE; 
                int results=0;
                if(ids.Count <= maxParamsNumber)
                {
                    results = delete(ids);
                } else
                {
                    int batchNumber = ids.Count / maxParamsNumber;
                    for(int i = 0; i < batchNumber; i++)
                    {
                        results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
                }
            }
            return -1;
        }
        private static int delete(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;

                    string queryIds = string.Empty;
                    for(int i=0; i<ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    string query = "DELETE FROM [ProductVariants] WHERE [Id] IN ("+ queryIds +")";                    
                    sqlCommand.CommandText = query;
                        
                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }
            return -1;
        }
        #endregion

        #region Custom Methods


        
        #endregion
    }
}
