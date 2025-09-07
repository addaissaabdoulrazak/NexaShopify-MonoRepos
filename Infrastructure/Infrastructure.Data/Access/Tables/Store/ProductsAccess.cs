using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables
{

    public class ProductsAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.ProductsEntity Get(int id)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Products] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.ProductsEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.ProductsEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Products]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.ProductsEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.ProductsEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.ProductsEntity> Get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.ProductsEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.ProductsEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.ProductsEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.ProductsEntity> get(List<int> ids)
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

                    sqlCommand.CommandText = $"SELECT * FROM [Products] WHERE [Id] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.ProductsEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.ProductsEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.ProductsEntity>();
        }

        public static int Insert(Infrastructure.Data.Entities.Tables.ProductsEntity item)
        {
            int response = int.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [Products] ([Barcode],[CompareAtPrice],[ContinueSelling],[CostPerItem],[CreatedAt],[Description],[IsAvailableAtPointOfSale],[IsAvailableOnOnlineStore],[IsTaxable],[Price],[Quantity],[SeoDescription],[SeoTitle],[SKU],[Slug],[Status],[Title],[TrackQuantity],[Type],[UpdatedAt],[Vendor])  VALUES (@Barcode,@CompareAtPrice,@ContinueSelling,@CostPerItem,@CreatedAt,@Description,@IsAvailableAtPointOfSale,@IsAvailableOnOnlineStore,@IsTaxable,@Price,@Quantity,@SeoDescription,@SeoTitle,@SKU,@Slug,@Status,@Title,@TrackQuantity,@Type,@UpdatedAt,@Vendor); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Barcode",item.Barcode == null ? (object)DBNull.Value  : item.Barcode);
					sqlCommand.Parameters.AddWithValue("CompareAtPrice",item.CompareAtPrice == null ? (object)DBNull.Value  : item.CompareAtPrice);
					sqlCommand.Parameters.AddWithValue("ContinueSelling",item.ContinueSelling);
					sqlCommand.Parameters.AddWithValue("CostPerItem",item.CostPerItem == null ? (object)DBNull.Value  : item.CostPerItem);
					sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt);
					sqlCommand.Parameters.AddWithValue("Description",item.Description == null ? (object)DBNull.Value  : item.Description);
					sqlCommand.Parameters.AddWithValue("IsAvailableAtPointOfSale",item.IsAvailableAtPointOfSale);
					sqlCommand.Parameters.AddWithValue("IsAvailableOnOnlineStore",item.IsAvailableOnOnlineStore);
					sqlCommand.Parameters.AddWithValue("IsTaxable",item.IsTaxable);
					sqlCommand.Parameters.AddWithValue("Price",item.Price == null ? (object)DBNull.Value  : item.Price);
					sqlCommand.Parameters.AddWithValue("Quantity",item.Quantity);
					sqlCommand.Parameters.AddWithValue("SeoDescription",item.SeoDescription == null ? (object)DBNull.Value  : item.SeoDescription);
					sqlCommand.Parameters.AddWithValue("SeoTitle",item.SeoTitle == null ? (object)DBNull.Value  : item.SeoTitle);
					sqlCommand.Parameters.AddWithValue("SKU",item.SKU == null ? (object)DBNull.Value  : item.SKU);
					sqlCommand.Parameters.AddWithValue("Slug",item.Slug == null ? (object)DBNull.Value  : item.Slug);
					sqlCommand.Parameters.AddWithValue("Status",item.Status);
					sqlCommand.Parameters.AddWithValue("Title",item.Title);
					sqlCommand.Parameters.AddWithValue("TrackQuantity",item.TrackQuantity);
					sqlCommand.Parameters.AddWithValue("Type",item.Type);
					sqlCommand.Parameters.AddWithValue("UpdatedAt",item.UpdatedAt);
					sqlCommand.Parameters.AddWithValue("Vendor",item.Vendor == null ? (object)DBNull.Value  : item.Vendor);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? int.MinValue:  int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.ProductsEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 22; // Nb params per query
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
        private static int insert(List<Infrastructure.Data.Entities.Tables.ProductsEntity> items)
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
                        query += " INSERT INTO [Products] ([Barcode],[CompareAtPrice],[ContinueSelling],[CostPerItem],[CreatedAt],[Description],[IsAvailableAtPointOfSale],[IsAvailableOnOnlineStore],[IsTaxable],[Price],[Quantity],[SeoDescription],[SeoTitle],[SKU],[Slug],[Status],[Title],[TrackQuantity],[Type],[UpdatedAt],[Vendor]) VALUES ( "

							+ "@Barcode"+ i +","
							+ "@CompareAtPrice"+ i +","
							+ "@ContinueSelling"+ i +","
							+ "@CostPerItem"+ i +","
							+ "@CreatedAt"+ i +","
							+ "@Description"+ i +","
							+ "@IsAvailableAtPointOfSale"+ i +","
							+ "@IsAvailableOnOnlineStore"+ i +","
							+ "@IsTaxable"+ i +","
							+ "@Price"+ i +","
							+ "@Quantity"+ i +","
							+ "@SeoDescription"+ i +","
							+ "@SeoTitle"+ i +","
							+ "@SKU"+ i +","
							+ "@Slug"+ i +","
							+ "@Status"+ i +","
							+ "@Title"+ i +","
							+ "@TrackQuantity"+ i +","
							+ "@Type"+ i +","
							+ "@UpdatedAt"+ i +","
							+ "@Vendor"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("Barcode" + i, item.Barcode == null ? (object)DBNull.Value  : item.Barcode);
							sqlCommand.Parameters.AddWithValue("CompareAtPrice" + i, item.CompareAtPrice == null ? (object)DBNull.Value  : item.CompareAtPrice);
							sqlCommand.Parameters.AddWithValue("ContinueSelling" + i, item.ContinueSelling);
							sqlCommand.Parameters.AddWithValue("CostPerItem" + i, item.CostPerItem == null ? (object)DBNull.Value  : item.CostPerItem);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value  : item.Description);
							sqlCommand.Parameters.AddWithValue("IsAvailableAtPointOfSale" + i, item.IsAvailableAtPointOfSale);
							sqlCommand.Parameters.AddWithValue("IsAvailableOnOnlineStore" + i, item.IsAvailableOnOnlineStore);
							sqlCommand.Parameters.AddWithValue("IsTaxable" + i, item.IsTaxable);
							sqlCommand.Parameters.AddWithValue("Price" + i, item.Price == null ? (object)DBNull.Value  : item.Price);
							sqlCommand.Parameters.AddWithValue("Quantity" + i, item.Quantity);
							sqlCommand.Parameters.AddWithValue("SeoDescription" + i, item.SeoDescription == null ? (object)DBNull.Value  : item.SeoDescription);
							sqlCommand.Parameters.AddWithValue("SeoTitle" + i, item.SeoTitle == null ? (object)DBNull.Value  : item.SeoTitle);
							sqlCommand.Parameters.AddWithValue("SKU" + i, item.SKU == null ? (object)DBNull.Value  : item.SKU);
							sqlCommand.Parameters.AddWithValue("Slug" + i, item.Slug == null ? (object)DBNull.Value  : item.Slug);
							sqlCommand.Parameters.AddWithValue("Status" + i, item.Status);
							sqlCommand.Parameters.AddWithValue("Title" + i, item.Title);
							sqlCommand.Parameters.AddWithValue("TrackQuantity" + i, item.TrackQuantity);
							sqlCommand.Parameters.AddWithValue("Type" + i, item.Type);
							sqlCommand.Parameters.AddWithValue("UpdatedAt" + i, item.UpdatedAt);
							sqlCommand.Parameters.AddWithValue("Vendor" + i, item.Vendor == null ? (object)DBNull.Value  : item.Vendor);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.ProductsEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [Products] SET [Barcode]=@Barcode, [CompareAtPrice]=@CompareAtPrice, [ContinueSelling]=@ContinueSelling, [CostPerItem]=@CostPerItem, [CreatedAt]=@CreatedAt, [Description]=@Description, [IsAvailableAtPointOfSale]=@IsAvailableAtPointOfSale, [IsAvailableOnOnlineStore]=@IsAvailableOnOnlineStore, [IsTaxable]=@IsTaxable, [Price]=@Price, [Quantity]=@Quantity, [SeoDescription]=@SeoDescription, [SeoTitle]=@SeoTitle, [SKU]=@SKU, [Slug]=@Slug, [Status]=@Status, [Title]=@Title, [TrackQuantity]=@TrackQuantity, [Type]=@Type, [UpdatedAt]=@UpdatedAt, [Vendor]=@Vendor WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Barcode",item.Barcode == null ? (object)DBNull.Value  : item.Barcode);
				sqlCommand.Parameters.AddWithValue("CompareAtPrice",item.CompareAtPrice == null ? (object)DBNull.Value  : item.CompareAtPrice);
				sqlCommand.Parameters.AddWithValue("ContinueSelling",item.ContinueSelling);
				sqlCommand.Parameters.AddWithValue("CostPerItem",item.CostPerItem == null ? (object)DBNull.Value  : item.CostPerItem);
				sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt);
				sqlCommand.Parameters.AddWithValue("Description",item.Description == null ? (object)DBNull.Value  : item.Description);
				sqlCommand.Parameters.AddWithValue("IsAvailableAtPointOfSale",item.IsAvailableAtPointOfSale);
				sqlCommand.Parameters.AddWithValue("IsAvailableOnOnlineStore",item.IsAvailableOnOnlineStore);
				sqlCommand.Parameters.AddWithValue("IsTaxable",item.IsTaxable);
				sqlCommand.Parameters.AddWithValue("Price",item.Price == null ? (object)DBNull.Value  : item.Price);
				sqlCommand.Parameters.AddWithValue("Quantity",item.Quantity);
				sqlCommand.Parameters.AddWithValue("SeoDescription",item.SeoDescription == null ? (object)DBNull.Value  : item.SeoDescription);
				sqlCommand.Parameters.AddWithValue("SeoTitle",item.SeoTitle == null ? (object)DBNull.Value  : item.SeoTitle);
				sqlCommand.Parameters.AddWithValue("SKU",item.SKU == null ? (object)DBNull.Value  : item.SKU);
				sqlCommand.Parameters.AddWithValue("Slug",item.Slug == null ? (object)DBNull.Value  : item.Slug);
				sqlCommand.Parameters.AddWithValue("Status",item.Status);
				sqlCommand.Parameters.AddWithValue("Title",item.Title);
				sqlCommand.Parameters.AddWithValue("TrackQuantity",item.TrackQuantity);
				sqlCommand.Parameters.AddWithValue("Type",item.Type);
				sqlCommand.Parameters.AddWithValue("UpdatedAt",item.UpdatedAt);
				sqlCommand.Parameters.AddWithValue("Vendor",item.Vendor == null ? (object)DBNull.Value  : item.Vendor);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.ProductsEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 22; // Nb params per query
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
        private static int update(List<Infrastructure.Data.Entities.Tables.ProductsEntity> items)
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
                        query += " UPDATE [Products] SET "

							+ "[Barcode]=@Barcode"+ i +","
							+ "[CompareAtPrice]=@CompareAtPrice"+ i +","
							+ "[ContinueSelling]=@ContinueSelling"+ i +","
							+ "[CostPerItem]=@CostPerItem"+ i +","
							+ "[CreatedAt]=@CreatedAt"+ i +","
							+ "[Description]=@Description"+ i +","
							+ "[IsAvailableAtPointOfSale]=@IsAvailableAtPointOfSale"+ i +","
							+ "[IsAvailableOnOnlineStore]=@IsAvailableOnOnlineStore"+ i +","
							+ "[IsTaxable]=@IsTaxable"+ i +","
							+ "[Price]=@Price"+ i +","
							+ "[Quantity]=@Quantity"+ i +","
							+ "[SeoDescription]=@SeoDescription"+ i +","
							+ "[SeoTitle]=@SeoTitle"+ i +","
							+ "[SKU]=@SKU"+ i +","
							+ "[Slug]=@Slug"+ i +","
							+ "[Status]=@Status"+ i +","
							+ "[Title]=@Title"+ i +","
							+ "[TrackQuantity]=@TrackQuantity"+ i +","
							+ "[Type]=@Type"+ i +","
							+ "[UpdatedAt]=@UpdatedAt"+ i +","
							+ "[Vendor]=@Vendor"+ i +" WHERE [Id]=@Id" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
							sqlCommand.Parameters.AddWithValue("Barcode" + i, item.Barcode == null ? (object)DBNull.Value  : item.Barcode);
							sqlCommand.Parameters.AddWithValue("CompareAtPrice" + i, item.CompareAtPrice == null ? (object)DBNull.Value  : item.CompareAtPrice);
							sqlCommand.Parameters.AddWithValue("ContinueSelling" + i, item.ContinueSelling);
							sqlCommand.Parameters.AddWithValue("CostPerItem" + i, item.CostPerItem == null ? (object)DBNull.Value  : item.CostPerItem);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value  : item.Description);
							sqlCommand.Parameters.AddWithValue("IsAvailableAtPointOfSale" + i, item.IsAvailableAtPointOfSale);
							sqlCommand.Parameters.AddWithValue("IsAvailableOnOnlineStore" + i, item.IsAvailableOnOnlineStore);
							sqlCommand.Parameters.AddWithValue("IsTaxable" + i, item.IsTaxable);
							sqlCommand.Parameters.AddWithValue("Price" + i, item.Price == null ? (object)DBNull.Value  : item.Price);
							sqlCommand.Parameters.AddWithValue("Quantity" + i, item.Quantity);
							sqlCommand.Parameters.AddWithValue("SeoDescription" + i, item.SeoDescription == null ? (object)DBNull.Value  : item.SeoDescription);
							sqlCommand.Parameters.AddWithValue("SeoTitle" + i, item.SeoTitle == null ? (object)DBNull.Value  : item.SeoTitle);
							sqlCommand.Parameters.AddWithValue("SKU" + i, item.SKU == null ? (object)DBNull.Value  : item.SKU);
							sqlCommand.Parameters.AddWithValue("Slug" + i, item.Slug == null ? (object)DBNull.Value  : item.Slug);
							sqlCommand.Parameters.AddWithValue("Status" + i, item.Status);
							sqlCommand.Parameters.AddWithValue("Title" + i, item.Title);
							sqlCommand.Parameters.AddWithValue("TrackQuantity" + i, item.TrackQuantity);
							sqlCommand.Parameters.AddWithValue("Type" + i, item.Type);
							sqlCommand.Parameters.AddWithValue("UpdatedAt" + i, item.UpdatedAt);
							sqlCommand.Parameters.AddWithValue("Vendor" + i, item.Vendor == null ? (object)DBNull.Value  : item.Vendor);
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
                string query = "DELETE FROM [Products] WHERE [Id]=@Id";
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

                    string query = "DELETE FROM [Products] WHERE [Id] IN ("+ queryIds +")";                    
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
