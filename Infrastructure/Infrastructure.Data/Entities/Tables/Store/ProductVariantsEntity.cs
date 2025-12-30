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
		public string OptionsJson { get; set; }	 // JSON contenant les options spécifiques à cette variante (séparées par des virgules) | Ex: {"Color":"Red","Size":"M"} | null si pas d'options (= produit simple) 
		public decimal Price { get; set; }
		public int ProductId { get; set; } // Clé étrangère vers ProductEntity
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

#region 

/// <summary>
/// Ancienne version de ProductVariantsEntity avec des commentaires détaillés
/// </summary>


 public class ProductVariantsEntity
    {
		/* public bool Available { get; set; }
		public string Barcode { get; set; }
		public decimal? CompareAtPrice { get; set; }
		public decimal? CostPerItem { get; set; }
		public int Id { get; set; }
  
		// soit on utiliseOptionJson pour stocker les options sous forme de JSON
		// soit on crée une table ProductVariantOptionsEntity pour gérer les options de manière relationnelle
		// Pour l'instant, on utilise OptionJson pour simplifier
		// Exemple de JSON: {"Color":"Red","Size":"M"}
		// Si le produit n'a pas de variantes, ce champ est null ou vide
		// On pourrait aussi envisager de stocker les options dans un format plus structuré
		// mais cela complexifierait la gestion des variantes
		// et n'est pas nécessaire pour une boutique simple
		// On pourrait aussi envisager d'ajouter un champ pour le nom de la variante
		// mais cela pourrait être redondant avec les options
		// Donc pour l'instant, on se contente de OptionsJson
		// et on gère le nom de la variante au niveau de l'application si besoin
		// Cela permet aussi de garder la flexibilité pour ajouter ou modifier des options sans changer la structure de la table
		// En résumé, OptionsJson est une solution simple et flexible
		// pour gérer les options des variantes de produits
		// sans complexifier la structure de la base de données
		// et en gardant la possibilité d'évolution future si besoin
		// Note: Il faudra gérer la sérialisation/désérialisation du JSON
		// dans l'application lors de la lecture/écriture de ce champ
		// et s'assurer que le format est cohérent
		// avec les options définies pour le produit parent
		// Cela peut être fait via des utilitaires JSON standards
		// ou des bibliothèques dédiées selon le langage/framework utilisé
		// Ici, on se concentre sur la structure de la table uniquement
		// et on laisse la gestion des données au niveau de l'application
		// Ce champ peut être null ou vide si le produit n'a pas de variantes
		// Dans ce cas, le produit est considéré comme un produit simple
		// et les informations de prix, stock, etc. sont gérées au niveau du produit parent
		// Si le produit a des variantes, chaque variante aura ses propres informations
		// et OptionsJson permettra de différencier les variantes entre elles
		// En conclusion, OptionsJson est un champ clé pour gérer les variantes de produits
		// de manière flexible et évolutive dans une boutique en ligne
		public string OptionsJson { get; set; }	 // JSON contenant les options spécifiques à cette variante (séparées par des virgules) | Ex: {"Color":"Red","Size":"M"} | null si pas d'options (= produit simple) 
		public decimal Price { get; set; }
		public int ProductId { get; set; } // Clé étrangère vers ProductEntity
		public string SKU { get; set; }
		public int Stock { get; set; } */


	}

#endregion

