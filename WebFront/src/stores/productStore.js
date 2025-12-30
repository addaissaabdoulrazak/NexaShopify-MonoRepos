// import { defineStore } from 'pinia';

// export const useProductStore = defineStore('products', {
//   state: () => ({
//     products: [
//       { id: 1, name: 'Ligne', status: 'Active', inventory: 20, category: 'Books', channels: 2 },
//     ],
//     newProduct: {
//       id: null,
//       title: '',
//       description: '',
//       status: 'Active',
//       onlineStore: true,
//       pointOfSale: false,
//       type: null,
//       vendor: null,
//       media: [],
//       price: '',
//       compareAtPrice: '',
//       costPerItem: '',
//       profit: '',
//       margin: '',
//       taxable: false,
//       trackQuantity: false,
//       quantity: 0,
//       continueSelling: false,
//       sku: '',
//       isPhysical: false,
//       weight: '0.0',
//       customsInfo: false,
//       variants: [],
//       seoTitle: '',
//     },
//   }),
//   actions: {
//     addProduct(product) {
//       this.newProduct.id = Date.now();
//       this.products.push({ ...this.newProduct, ...product });
//       this.resetNewProduct();
//     },
//     updateProduct(updatedProduct) {
//       const index = this.products.findIndex(p => p.id === updatedProduct.id);
//       if (index !== -1) this.products.splice(index, 1, updatedProduct);
//     },
//     deleteProduct(id) {
//       this.products = this.products.filter(p => p.id !== id);
//     },
//     resetNewProduct() {
//       this.newProduct = {
//         id: null,
//         title: '',
//         description: '',
//         status: 'Active',
//         onlineStore: true,
//         pointOfSale: false,
//         type: null,
//         vendor: null,
//         media: [],
//         price: '',
//         compareAtPrice: '',
//         costPerItem: '',
//         profit: '',
//         margin: '',
//         taxable: false,
//         trackQuantity: false,
//         quantity: 0,
//         continueSelling: false,
//         sku: '',
//         isPhysical: false,
//         weight: '0.0',
//         customsInfo: false,
//         variants: [],
//         seoTitle: '',
//       };
//     },
//     calculateProfitMargin() {
//       const price = parseFloat(this.newProduct.price) || 0;
//       const compareAtPrice = parseFloat(this.newProduct.compareAtPrice) || 0;
//       const cost = parseFloat(this.newProduct.costPerItem) || 0;
//       this.newProduct.profit = (price - cost).toFixed(3);
//       this.newProduct.margin = price > 0 ? ((price - cost) / price * 100).toFixed(2) : '0.00';
//     },
//   },
// });
// stores/productStore.js
import { defineStore } from 'pinia'

export const useProductStore = defineStore('products', {
  state: () => ({
    products: [
      // Données de test initiales
      {
        id: 1,
        title: 'Nike Air Max 270',
        name: 'Nike Air Max 270', // Pour compatibilité avec products.vue
        description: '<p>Sneakers Nike Air Max 270 avec technologie Air visible pour un confort maximal.</p>',
        status: 'Active',
        type: 'physical',
        onlineStore: true,
        pointOfSale: true,
        price: 189.990,
        compareAtPrice: 220.000,
        costPerItem: 95.000,
        taxable: true,
        trackQuantity: true,
        inventory: 150,
        quantity: 150,
        continueSelling: false,
        sku: 'NIKE-AM270-001',
        barcode: '1234567890123',
        vendor: 'Nike',
        category: 'Sneakers',
        image: '/api/placeholder/400/400',
        collections: ['featured', 'new-arrivals'],
        tags: ['nike', 'sneakers', 'air-max', 'sport'],
        seoTitle: 'Nike Air Max 270 - Sneakers Confort',
        seoDescription: 'Sneakers Nike Air Max 270 avec technologie Air visible.',
        media: [
          {
            id: 1,
            url: '/api/placeholder/400/400',
            type: 'image',
            alt: 'Nike Air Max 270 vue principale',
            size: 245760,
            displayOrder: 0
          }
        ],
        options: [
          {
            id: 1,
            name: 'Couleur',
            values: ['Noir/Blanc', 'Bleu/Blanc', 'Rouge/Noir']
          },
          {
            id: 2,
            name: 'Pointure',
            values: ['39', '40', '41', '42', '43']
          }
        ],
        variants: [
          {
            id: 1,
            sku: 'NIKE-AM270-NB-40',
            price: 189.990,
            stock: 25,
            Couleur: 'Noir/Blanc',
            Pointure: '40'
          },
          {
            id: 2,
            sku: 'NIKE-AM270-BB-41',
            price: 189.990,
            stock: 20,
            Couleur: 'Bleu/Blanc',
            Pointure: '41'
          }
        ],
        createdAt: new Date('2024-01-15'),
        updatedAt: new Date('2024-01-15')
      },
      {
        id: 2,
        title: 'iPhone 15 Pro',
        name: 'iPhone 15 Pro',
        description: '<p>iPhone 15 Pro avec puce A17 Pro et appareil photo révolutionnaire.</p>',
        status: 'Active',
        type: 'physical',
        onlineStore: true,
        pointOfSale: true,
        price: 1299.000,
        compareAtPrice: 1399.000,
        costPerItem: 800.000,
        taxable: true,
        trackQuantity: true,
        inventory: 50,
        quantity: 50,
        continueSelling: false,
        sku: 'APPLE-IP15P-001',
        barcode: '9876543210987',
        vendor: 'Apple',
        category: 'Electronics',
        image: 'https://images.unsplash.com/photo-1496181133206-80ce9b88a853?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80',
        collections: ['featured', 'premium'],
        tags: ['apple', 'iphone', 'smartphone'],
        seoTitle: 'iPhone 15 Pro - Smartphone Premium',
        seoDescription: 'iPhone 15 Pro avec puce A17 Pro et design en titane.',
        media: [
          {
            id: 3,
            url: '/api/placeholder/400/400',
            type: 'image',
            alt: 'iPhone 15 Pro vue principale',
            size: 345760,
            displayOrder: 0
          }
        ],
        options: [
          {
            id: 3,
            name: 'Couleur',
            values: ['Titane naturel', 'Titane bleu', 'Titane blanc']
          },
          {
            id: 4,
            name: 'Stockage',
            values: ['128GB', '256GB', '512GB', '1TB']
          }
        ],
        variants: [
          {
            id: 3,
            sku: 'APPLE-IP15P-TN-128',
            price: 1299.000,
            stock: 15,
            Couleur: 'Titane naturel',
            Stockage: '128GB'
          }
        ],
        createdAt: new Date('2024-01-10'),
        updatedAt: new Date('2024-01-12')
      }
    ],
    loading: false,
    error: null,
    // Structure pour nouveau produit
    newProduct: {
      id: null,
      title: '',
      description: '',
      status: 'draft',
      type: 'physical',
      onlineStore: true,
      pointOfSale: false,
      price: null,
      compareAtPrice: null,
      costPerItem: null,
      taxable: false,
      trackQuantity: true,
      quantity: 0,
      continueSelling: false,
      sku: '',
      barcode: '',
      vendor: '',
      collections: [],
      tags: [],
      seoTitle: '',
      seoDescription: '',
      media: [],
      options: [],
      variants: []
    }
  }),

  getters: {
    // Statistiques
    totalProducts: (state) => state.products.length,
    activeProducts: (state) => state.products.filter(p => p.status === 'Active'),
    draftProducts: (state) => state.products.filter(p => p.status === 'Draft'),
    lowStockProducts: (state) => state.products.filter(p => p.inventory <= 10 && p.inventory > 0),
    outOfStockProducts: (state) => state.products.filter(p => p.inventory === 0),
    
    // Calculer la valeur totale de l'inventaire
    totalInventoryValue: (state) => {
      return state.products.reduce((total, product) => {
        return total + ((product.price || 0) * (product.inventory || 0))
      }, 0)
    }
  },

  actions: {
    // **DÉBUT MODIFICATION - Méthode addProduct améliorée**
    async addProduct(productData) {
      this.loading = true
      this.error = null
      
      try {
        // Simuler un délai d'API
        await new Promise(resolve => setTimeout(resolve, 800))
        
        // Générer un nouvel ID
        const newId = Math.max(...this.products.map(p => p.id), 0) + 1
        
        // Créer le slug à partir du titre
        const slug = this.generateSlug(productData.title)
        
        // Créer le nouveau produit avec tous les champs nécessaires
        const newProduct = {
          // ID et métadonnées
          id: newId,
          title: productData.title || '',
          name: productData.title || '', // Pour compatibilité
          description: productData.description || '',
          slug: slug,
          status: productData.status || 'draft',
          type: productData.type || 'physical',
          
          // Disponibilité
          onlineStore: productData.onlineStore ?? true,
          pointOfSale: productData.pointOfSale ?? false,
          
          // Prix
          price: productData.price || null,
          compareAtPrice: productData.compareAtPrice || null,
          costPerItem: productData.costPerItem || null,
          taxable: productData.taxable ?? false,
          
          // Inventaire
          trackQuantity: productData.trackQuantity ?? true,
          quantity: productData.quantity || 0,
          inventory: productData.quantity || 0, // Pour compatibilité
          continueSelling: productData.continueSelling ?? false,
          
          // Identification
          sku: productData.sku || `PRD-${newId}-${Date.now().toString().slice(-6)}`,
          barcode: productData.barcode || '',
          vendor: productData.vendor || '',
          category: productData.category || 'General',
          
          // Collections et tags
          collections: productData.collections || [],
          tags: productData.tags || [],
          
          // SEO
          seoTitle: productData.seoTitle || '',
          seoDescription: productData.seoDescription || '',
          
          // Média
          media: productData.media || [],
          image: productData.media?.[0]?.url || '/api/placeholder/400/400',
          
          // Options et variantes
          options: productData.options || [],
          variants: this.processVariants(productData.variants || [], newId),
          
          // Timestamps
          createdAt: new Date(),
          updatedAt: new Date()
        }

        // Si des variantes existent, calculer le stock total
        if (newProduct.variants.length > 0) {
          newProduct.inventory = newProduct.variants.reduce((total, variant) => total + (variant.stock || 0), 0)
          newProduct.quantity = newProduct.inventory
        }

        // Ajouter le produit au début de la liste
        this.products.unshift(newProduct)

        console.log('✅ Produit ajouté avec succès:', newProduct)
        return newProduct

      } catch (error) {
        this.error = 'Erreur lors de l\'ajout du produit'
        console.error('❌ Erreur lors de l\'ajout:', error)
        throw error
      } finally {
        this.loading = false
      }
    },
    // **FIN MODIFICATION addProduct**

    // **DÉBUT MODIFICATION - Méthode updateProduct améliorée**
    async updateProduct(productData) {
      this.loading = true
      this.error = null
      
      try {
        await new Promise(resolve => setTimeout(resolve, 500))
        
        const index = this.products.findIndex(p => p.id === productData.id)
        if (index === -1) {
          throw new Error('Produit non trouvé')
        }

        // Mettre à jour le produit existant
        const updatedProduct = {
          ...this.products[index],
          ...productData,
          name: productData.title || productData.name, // Synchronisation
          slug: this.generateSlug(productData.title),
          inventory: productData.quantity || productData.inventory || 0,
          updatedAt: new Date()
        }

        // Recalculer le stock si des variantes existent
        if (updatedProduct.variants && updatedProduct.variants.length > 0) {
          updatedProduct.inventory = updatedProduct.variants.reduce((total, variant) => total + (variant.stock || 0), 0)
          updatedProduct.quantity = updatedProduct.inventory
        }

        this.products[index] = updatedProduct
        
        console.log('✅ Produit mis à jour:', updatedProduct)
        return updatedProduct

      } catch (error) {
        this.error = 'Erreur lors de la mise à jour'
        console.error('❌ Erreur lors de la mise à jour:', error)
        throw error
      } finally {
        this.loading = false
      }
    },
    // **FIN MODIFICATION updateProduct**

    // **DÉBUT MODIFICATION - Méthodes utilitaires**
    generateSlug(title) {
      if (!title) return ''
      return title
        .toLowerCase()
        .trim()
        .replace(/[^\w\s-]/g, '')
        .replace(/[\s_-]+/g, '-')
        .replace(/^-+|-+$/g, '')
    },

    processVariants(variants, productId) {
      return variants.map((variant, index) => ({
        id: (productId * 100) + index + 1,
        sku: variant.sku || `VAR-${productId}-${index + 1}`,
        price: variant.price || 0,
        compareAtPrice: variant.compareAtPrice || null,
        costPerItem: variant.costPerItem || null,
        stock: variant.stock || 0,
        barcode: variant.barcode || '',
        available: variant.available ?? true,
        ...variant.options || {}
      }))
    },
    // **FIN MODIFICATION méthodes utilitaires**

    // **DÉBUT MODIFICATION - Autres méthodes du store**
    async deleteProduct(id) {
      this.loading = true
      try {
        await new Promise(resolve => setTimeout(resolve, 300))
        
        const index = this.products.findIndex(p => p.id === id)
        if (index === -1) {
          throw new Error('Produit non trouvé')
        }

        this.products.splice(index, 1)
        console.log('✅ Produit supprimé, ID:', id)
        return true

      } catch (error) {
        this.error = 'Erreur lors de la suppression'
        throw error
      } finally {
        this.loading = false
      }
    },

    async bulkDeleteProducts(ids) {
      this.loading = true
      try {
        await new Promise(resolve => setTimeout(resolve, 500))
        
        ids.forEach(id => {
          const index = this.products.findIndex(p => p.id === id)
          if (index !== -1) {
            this.products.splice(index, 1)
          }
        })

        console.log('✅ Suppression en lot réussie:', ids.length, 'produits')
        return { deletedCount: ids.length }

      } catch (error) {
        this.error = 'Erreur lors de la suppression en lot'
        throw error
      } finally {
        this.loading = false
      }
    },

    async fetchProducts() {
      this.loading = true
      this.error = null
      
      try {
        // Simuler un appel API
        await new Promise(resolve => setTimeout(resolve, 1000))
        console.log('✅ Produits chargés:', this.products.length)
        
      } catch (error) {
        this.error = 'Erreur lors du chargement'
        throw error
      } finally {
        this.loading = false
      }
    },

    async exportProducts() {
      this.loading = true
      try {
        await new Promise(resolve => setTimeout(resolve, 800))
        
        // Simuler l'export CSV
        const csvData = this.convertToCSV(this.products)
        this.downloadCSV(csvData, 'products-export.csv')
        
        console.log('✅ Export réussi')
        
      } catch (error) {
        this.error = 'Erreur lors de l\'export'
        throw error
      } finally {
        this.loading = false
      }
    },

    async exportSelectedProducts(selectedProducts) {
      this.loading = true
      try {
        await new Promise(resolve => setTimeout(resolve, 500))
        
        const csvData = this.convertToCSV(selectedProducts)
        this.downloadCSV(csvData, 'selected-products-export.csv')
        
        console.log('✅ Export sélection réussi')
        
      } catch (error) {
        this.error = 'Erreur lors de l\'export'
        throw error
      } finally {
        this.loading = false
      }
    },

    exportAllProducts() {
      return this.exportProducts()
    },

    // Méthodes utilitaires pour l'export
    convertToCSV(data) {
      if (!data || data.length === 0) return ''
      
      const headers = ['ID', 'Titre', 'Status', 'Prix', 'Stock', 'Vendeur', 'Date création']
      const rows = data.map(product => [
        product.id,
        `"${product.title}"`,
        product.status,
        product.price || 0,
        product.inventory || 0,
        `"${product.vendor || ''}"`,
        new Date(product.createdAt).toLocaleDateString()
      ])
      
      return [headers, ...rows].map(row => row.join(',')).join('\n')
    },

    downloadCSV(csvData, filename) {
      const blob = new Blob([csvData], { type: 'text/csv;charset=utf-8;' })
      const link = document.createElement('a')
      const url = URL.createObjectURL(blob)
      link.setAttribute('href', url)
      link.setAttribute('download', filename)
      link.style.visibility = 'hidden'
      document.body.appendChild(link)
      link.click()
      document.body.removeChild(link)
    },

    // Réinitialisation du produit
    resetNewProduct() {
      this.newProduct = {
        id: null,
        title: '',
        description: '',
        status: 'draft',
        type: 'physical',
        onlineStore: true,
        pointOfSale: false,
        price: null,
        compareAtPrice: null,
        costPerItem: null,
        taxable: false,
        trackQuantity: true,
        quantity: 0,
        continueSelling: false,
        sku: '',
        barcode: '',
        vendor: '',
        collections: [],
        tags: [],
        seoTitle: '',
        seoDescription: '',
        media: [],
        options: [],
        variants: []
      }
    },

    // Calcul des marges (pour compatibilité)
    calculateProfitMargin() {
      if (this.newProduct.price && this.newProduct.costPerItem) {
        const price = parseFloat(this.newProduct.price) || 0
        const cost = parseFloat(this.newProduct.costPerItem) || 0
        this.newProduct.profit = (price - cost).toFixed(3)
        this.newProduct.margin = price > 0 ? ((price - cost) / price * 100).toFixed(2) : '0.00'
      }
    }
    // **FIN MODIFICATION autres méthodes**
  }
})