use nexashop;

-- =============================================
-- Script de création des tables pour le système de gestion de produits
-- Base de données : SQL Server
-- Version mise à jour avec les nouveaux noms de colonnes
-- =============================================

-- Supprimer les tables si elles existent (dans l'ordre inverse des dépendances)
IF OBJECT_ID('dbo.ProductVariants', 'U') IS NOT NULL DROP TABLE dbo.ProductVariants;
IF OBJECT_ID('dbo.ProductOptionValues', 'U') IS NOT NULL DROP TABLE dbo.ProductOptionValues;
IF OBJECT_ID('dbo.ProductOptions', 'U') IS NOT NULL DROP TABLE dbo.ProductOptions;
IF OBJECT_ID('dbo.ProductCollections', 'U') IS NOT NULL DROP TABLE dbo.ProductCollections;
IF OBJECT_ID('dbo.ProductTags', 'U') IS NOT NULL DROP TABLE dbo.ProductTags;
IF OBJECT_ID('dbo.ProductMedia', 'U') IS NOT NULL DROP TABLE dbo.ProductMedia;
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL DROP TABLE dbo.Products;

-- =============================================
-- Table principale : Products
-- =============================================
CREATE TABLE dbo.Products (
    Id INT IDENTITY(1,1) NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Slug NVARCHAR(120) NULL,
    Status NVARCHAR(20) NOT NULL,
    Type NVARCHAR(20) NOT NULL,
    IsAvailableOnOnlineStore BIT NOT NULL DEFAULT 1,
    IsAvailableAtPointOfSale BIT NOT NULL DEFAULT 0,
    Price DECIMAL(18,3) NULL,
    CompareAtPrice DECIMAL(18,3) NULL,
    CostPerItem DECIMAL(18,3) NULL,
    IsTaxable BIT NOT NULL DEFAULT 0,
    TrackQuantity BIT NOT NULL DEFAULT 1,
    Quantity INT NOT NULL DEFAULT 0,
    ContinueSelling BIT NOT NULL DEFAULT 0,
    SKU NVARCHAR(50) NULL,
    Barcode NVARCHAR(50) NULL,
    Vendor NVARCHAR(100) NULL,
    SeoTitle NVARCHAR(60) NULL,
    SeoDescription NVARCHAR(160) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_Products PRIMARY KEY CLUSTERED (Id)
);

-- Index pour améliorer les performances
CREATE INDEX IX_Products_Status ON dbo.Products(Status);
CREATE INDEX IX_Products_Type ON dbo.Products(Type);
CREATE INDEX IX_Products_Slug ON dbo.Products(Slug);
CREATE UNIQUE INDEX IX_Products_SKU ON dbo.Products(SKU) WHERE SKU IS NOT NULL;
CREATE UNIQUE INDEX IX_Products_Barcode ON dbo.Products(Barcode) WHERE Barcode IS NOT NULL;
CREATE INDEX IX_Products_Vendor ON dbo.Products(Vendor);

-- =============================================
-- Table : ProductMedia
-- =============================================
CREATE TABLE dbo.ProductMedia (
    Id INT IDENTITY(1,1) NOT NULL,
    ProductId INT NOT NULL,
    Url NVARCHAR(500) NOT NULL,
    Type NVARCHAR(20) NULL,
    Alt NVARCHAR(200) NULL,
    Size BIGINT NULL,
    DisplayOrder INT NOT NULL DEFAULT 0,
    CONSTRAINT PK_ProductMedia PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_ProductMedia_Products FOREIGN KEY (ProductId) 
        REFERENCES dbo.Products(Id) ON DELETE CASCADE
);

CREATE INDEX IX_ProductMedia_ProductId ON dbo.ProductMedia(ProductId);
CREATE INDEX IX_ProductMedia_DisplayOrder ON dbo.ProductMedia(DisplayOrder);

-- =============================================
-- Table : ProductTags
-- =============================================
CREATE TABLE dbo.ProductTags (
    Id INT IDENTITY(1,1) NOT NULL,
    ProductId INT NOT NULL,
    TagName NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_ProductTags PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_ProductTags_Products FOREIGN KEY (ProductId) 
        REFERENCES dbo.Products(Id) ON DELETE CASCADE
);

CREATE INDEX IX_ProductTags_ProductId ON dbo.ProductTags(ProductId);
CREATE INDEX IX_ProductTags_TagName ON dbo.ProductTags(TagName);

-- Index unique pour éviter les doublons de tags par produit
CREATE UNIQUE INDEX IX_ProductTags_ProductId_TagName 
    ON dbo.ProductTags(ProductId, TagName);

-- =============================================
-- Table : ProductCollections
-- =============================================
CREATE TABLE dbo.ProductCollections (
    Id INT IDENTITY(1,1) NOT NULL,
    ProductId INT NOT NULL,
    CollectionName NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_ProductCollections PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_ProductCollections_Products FOREIGN KEY (ProductId) 
        REFERENCES dbo.Products(Id) ON DELETE CASCADE
);

CREATE INDEX IX_ProductCollections_ProductId ON dbo.ProductCollections(ProductId);
CREATE INDEX IX_ProductCollections_CollectionName ON dbo.ProductCollections(CollectionName);

-- Index unique pour éviter les doublons de collections par produit
CREATE UNIQUE INDEX IX_ProductCollections_ProductId_CollectionName 
    ON dbo.ProductCollections(ProductId, CollectionName);

-- =============================================
-- Table : ProductOptions
-- =============================================
CREATE TABLE dbo.ProductOptions (
    Id INT IDENTITY(1,1) NOT NULL,
    ProductId INT NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    Position INT NOT NULL DEFAULT 0,
    CONSTRAINT PK_ProductOptions PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_ProductOptions_Products FOREIGN KEY (ProductId) 
        REFERENCES dbo.Products(Id) ON DELETE CASCADE
);

CREATE INDEX IX_ProductOptions_ProductId ON dbo.ProductOptions(ProductId);

-- =============================================
-- Table : ProductOptionValues
-- =============================================
CREATE TABLE dbo.ProductOptionValues (
    Id INT IDENTITY(1,1) NOT NULL,
    OptionId INT NOT NULL,
    Value NVARCHAR(50) NOT NULL,
    Position INT NOT NULL DEFAULT 0,
    CONSTRAINT PK_ProductOptionValues PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_ProductOptionValues_ProductOptions FOREIGN KEY (OptionId) 
        REFERENCES dbo.ProductOptions(Id) ON DELETE CASCADE
);

CREATE INDEX IX_ProductOptionValues_OptionId ON dbo.ProductOptionValues(OptionId);

-- Index unique pour éviter les doublons de valeurs par option
CREATE UNIQUE INDEX IX_ProductOptionValues_OptionId_Value 
    ON dbo.ProductOptionValues(OptionId, Value);

-- =============================================
-- Table : ProductVariants
-- =============================================
CREATE TABLE dbo.ProductVariants (
    Id INT IDENTITY(1,1) NOT NULL,
    ProductId INT NOT NULL,
    SKU NVARCHAR(50) NULL,
    Price DECIMAL(18,3) NOT NULL,
    CompareAtPrice DECIMAL(18,3) NULL,
    CostPerItem DECIMAL(18,3) NULL,
    Stock INT NOT NULL DEFAULT 0,
    Barcode NVARCHAR(50) NULL,
    Available BIT NOT NULL DEFAULT 1,
    OptionsJson NVARCHAR(MAX) NULL, -- Stockage JSON des combinaisons d'options
    CONSTRAINT PK_ProductVariants PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_ProductVariants_Products FOREIGN KEY (ProductId) 
        REFERENCES dbo.Products(Id) ON DELETE CASCADE
);

CREATE INDEX IX_ProductVariants_ProductId ON dbo.ProductVariants(ProductId);
CREATE UNIQUE INDEX IX_ProductVariants_SKU ON dbo.ProductVariants(SKU) WHERE SKU IS NOT NULL;
CREATE UNIQUE INDEX IX_ProductVariants_Barcode ON dbo.ProductVariants(Barcode) WHERE Barcode IS NOT NULL;

-- =============================================
-- Trigger pour mettre à jour UpdatedAt automatiquement
-- =============================================
GO
CREATE TRIGGER TR_Products_UpdatedAt
ON dbo.Products
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Products
    SET UpdatedAt = GETUTCDATE()
    FROM dbo.Products p
    INNER JOIN inserted i ON p.Id = i.Id;
END;
GO

-- =============================================
-- Vérification de la création des tables
-- =============================================
SELECT 
    t.name AS TableName,
    p.rows AS RowCounts,
    SUM(a.total_pages) * 8 AS TotalSpaceKB,
    SUM(a.used_pages) * 8 AS UsedSpaceKB
FROM 
    sys.tables t
INNER JOIN      
    sys.indexes i ON t.OBJECT_ID = i.object_id
INNER JOIN 
    sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
INNER JOIN 
    sys.allocation_units a ON p.partition_id = a.container_id
WHERE 
    t.name IN ('Products', 'ProductMedia', 'ProductTags', 'ProductCollections', 
               'ProductOptions', 'ProductOptionValues', 'ProductVariants')
    AND t.is_ms_shipped = 0
    AND i.OBJECT_ID > 255 
GROUP BY 
    t.name, p.rows
ORDER BY 
    t.name;

-- =============================================
-- Procédure stockée pour générer un slug unique
-- =============================================
GO
CREATE PROCEDURE sp_GenerateProductSlug
    @Title NVARCHAR(100),
    @ProductId INT = NULL,
    @Slug NVARCHAR(120) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Nettoyer le titre pour créer le slug de base
    SET @Slug = LOWER(@Title);
    SET @Slug = REPLACE(@Slug, ' ', '-');
    SET @Slug = REPLACE(@Slug, '''', '');
    SET @Slug = REPLACE(@Slug, '"', '');
    SET @Slug = REPLACE(@Slug, '&', 'and');
    SET @Slug = REPLACE(@Slug, '@', 'at');
    SET @Slug = REPLACE(@Slug, '.', '');
    SET @Slug = REPLACE(@Slug, ',', '');
    SET @Slug = REPLACE(@Slug, '!', '');
    SET @Slug = REPLACE(@Slug, '?', '');
    SET @Slug = REPLACE(@Slug, '/', '-');
    SET @Slug = REPLACE(@Slug, '\', '-');
    SET @Slug = REPLACE(@Slug, '--', '-');
    
    -- Vérifier l'unicité et ajouter un suffixe si nécessaire
    DECLARE @Counter INT = 1;
    DECLARE @TempSlug NVARCHAR(120) = @Slug;
    
    WHILE EXISTS (SELECT 1 FROM dbo.Products WHERE Slug = @TempSlug AND (@ProductId IS NULL OR Id != @ProductId))
    BEGIN
        SET @TempSlug = @Slug + '-' + CAST(@Counter AS NVARCHAR(10));
        SET @Counter = @Counter + 1;
    END;
    
    SET @Slug = @TempSlug;
END;
GO

-- =============================================
-- Exemples d'insertion de données de test (optionnel)
-- =============================================
/*
-- Insérer un produit de test
DECLARE @GeneratedSlug NVARCHAR(120);
EXEC sp_GenerateProductSlug @Title = N'T-Shirt Premium', @ProductId = NULL, @Slug = @GeneratedSlug OUTPUT;

INSERT INTO dbo.Products (
    Title, 
    Description, 
    Slug,
    Status, 
    Type, 
    IsAvailableOnOnlineStore, 
    IsAvailableAtPointOfSale, 
    Price, 
    IsTaxable,
    TrackQuantity, 
    Quantity
)
VALUES (
    N'T-Shirt Premium', 
    N'<p>T-shirt de haute qualité en coton bio</p>', 
    @GeneratedSlug,
    'active', 
    'physical', 
    1, 
    0, 
    29.900, 
    1,
    1, 
    100
);

-- Récupérer l'ID du produit inséré
DECLARE @ProductId INT = SCOPE_IDENTITY();

-- Ajouter des médias
INSERT INTO dbo.ProductMedia (ProductId, Url, Type, Alt, DisplayOrder)
VALUES 
    (@ProductId, '/uploads/tshirt-main.jpg', 'image', N'T-shirt vue de face', 0),
    (@ProductId, '/uploads/tshirt-back.jpg', 'image', N'T-shirt vue de dos', 1);

-- Ajouter des tags
INSERT INTO dbo.ProductTags (ProductId, TagName)
VALUES 
    (@ProductId, N'nouveau'),
    (@ProductId, N'été'),
    (@ProductId, N'coton-bio');

-- Ajouter des collections
INSERT INTO dbo.ProductCollections (ProductId, CollectionName)
VALUES 
    (@ProductId, N'featured'),
    (@ProductId, N'new-arrivals');

-- Ajouter des options
INSERT INTO dbo.ProductOptions (ProductId, Name, Position)
VALUES 
    (@ProductId, N'Couleur', 0),
    (@ProductId, N'Taille', 1);

-- Récupérer les IDs des options
DECLARE @ColorOptionId INT = (SELECT Id FROM dbo.ProductOptions WHERE ProductId = @ProductId AND Name = N'Couleur');
DECLARE @SizeOptionId INT = (SELECT Id FROM dbo.ProductOptions WHERE ProductId = @ProductId AND Name = N'Taille');

-- Ajouter les valeurs d'options
INSERT INTO dbo.ProductOptionValues (OptionId, Value, Position)
VALUES 
    (@ColorOptionId, N'Noir', 0),
    (@ColorOptionId, N'Blanc', 1),
    (@ColorOptionId, N'Bleu', 2),
    (@SizeOptionId, N'S', 0),
    (@SizeOptionId, N'M', 1),
    (@SizeOptionId, N'L', 2),
    (@SizeOptionId, N'XL', 3);

-- Ajouter quelques variantes
INSERT INTO dbo.ProductVariants (ProductId, SKU, Price, Stock, OptionsJson)
VALUES 
    (@ProductId, 'TSH-NOIR-S', 29.900, 10, N'{"Couleur": "Noir", "Taille": "S"}'),
    (@ProductId, 'TSH-NOIR-M', 29.900, 15, N'{"Couleur": "Noir", "Taille": "M"}'),
    (@ProductId, 'TSH-BLANC-L', 29.900, 20, N'{"Couleur": "Blanc", "Taille": "L"}');

-- Vérifier les données insérées
SELECT * FROM dbo.Products WHERE Id = @ProductId;
SELECT * FROM dbo.ProductMedia WHERE ProductId = @ProductId;
SELECT * FROM dbo.ProductTags WHERE ProductId = @ProductId;
SELECT * FROM dbo.ProductVariants WHERE ProductId = @ProductId;
*/
