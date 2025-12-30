// utils/productTestData.js
export const generateTestProducts = () => [
  {
    title: 'T-Shirt Premium Coton Bio',
    description: '<p>T-shirt haut de gamme en coton biologique certifié. Coupe moderne et confortable pour un usage quotidien.</p><ul><li>100% coton bio</li><li>Coupe ajustée</li><li>Résistant au lavage</li></ul>',
    status: 'Active',
    type: 'physical',
    onlineStore: true,
    pointOfSale: true,
    price: 29.99,
    compareAtPrice: 39.99,
    costPerItem: 12.00,
    taxable: true,
    trackQuantity: true,
    quantity: 100,
    sku: 'TSHIRT-PREMIUM-001',
    vendor: 'EcoWear',
    collections: ['featured', 'clothing'],
    tags: ['t-shirt', 'coton-bio', 'premium', 'eco-friendly'],
    seoTitle: 'T-Shirt Premium Coton Bio - Confort et Style',
    seoDescription: 'Découvrez notre T-shirt premium en coton bio certifié. Confort optimal et style moderne.',
    media: [
      {
        url: '/api/placeholder/400/400',
        type: 'image',
        alt: 'T-shirt premium vue de face',
        size: 204800,
        displayOrder: 0
      }
    ],
    options: [
      {
        name: 'Couleur',
        values: ['Blanc', 'Noir', 'Gris', 'Bleu Marine']
      },
      {
        name: 'Taille',
        values: ['S', 'M', 'L', 'XL', 'XXL']
      }
    ],
    variants: [
      {
        sku: 'TSHIRT-BLANC-M',
        price: 29.99,
        stock: 25,
        Couleur: 'Blanc',
        Taille: 'M'
      },
      {
        sku: 'TSHIRT-NOIR-L',
        price: 29.99,
        stock: 30,
        Couleur: 'Noir',
        Taille: 'L'
      }
    ]
  },
  {
    title: 'Casque Audio Bluetooth Premium',
    description: '<p>Casque audio sans fil haut de gamme avec réduction de bruit active et autonomie exceptionnelle.</p><h3>Caractéristiques :</h3><ul><li>Autonomie 40h</li><li>Réduction de bruit active</li><li>Bluetooth 5.3</li><li>Charge rapide USB-C</li></ul>',
    status: 'Active',
    type: 'physical',
    onlineStore: true,
    pointOfSale: true,
    price: 199.99,
    compareAtPrice: 249.99,
    costPerItem: 89.00,
    taxable: true,
    trackQuantity: true,
    quantity: 45,
    sku: 'CASQUE-BT-PREMIUM-001',
    barcode: '5432167890123',
    vendor: 'SoundTech',
    collections: ['featured', 'electronics', 'audio'],
    tags: ['casque', 'bluetooth', 'audio', 'premium', 'noise-canceling'],
    seoTitle: 'Casque Audio Bluetooth Premium - Son Haute Fidélité',
    seoDescription: 'Casque Bluetooth premium avec réduction de bruit active. Qualité audio exceptionnelle et confort longue durée.',
    media: [
      {
        url: '/api/placeholder/400/400',
        type: 'image',
        alt: 'Casque audio vue principale',
        size: 256000,
        displayOrder: 0
      },
      {
        url: '/api/placeholder/400/400',
        type: 'image',
        alt: 'Casque audio vue profil',
        size: 230000,
        displayOrder: 1
      }
    ],
    options: [
      {
        name: 'Couleur',
        values: ['Noir', 'Blanc', 'Bleu']
      }
    ],
    variants: [
      {
        sku: 'CASQUE-NOIR',
        price: 199.99,
        stock: 20,
        Couleur: 'Noir'
      },
      {
        sku: 'CASQUE-BLANC',
        price: 199.99,
        stock: 15,
        Couleur: 'Blanc'
      },
      {
        sku: 'CASQUE-BLEU',
        price: 219.99,
        stock: 10,
        Couleur: 'Bleu'
      }
    ]
  },
  {
    title: 'Formation JavaScript Avancé',
    description: '<p>Formation complète JavaScript moderne avec ES6+, frameworks et bonnes pratiques.</p><h3>Programme :</h3><ul><li>ES6+ et syntaxe moderne</li><li>Programmation asynchrone</li><li>Frameworks populaires</li><li>Projets pratiques</li></ul>',
    status: 'Active',
    type: 'digital',
    onlineStore: true,
    pointOfSale: false,
    price: 149.99,
    compareAtPrice: 199.99,
    costPerItem: 25.00,
    taxable: false,
    trackQuantity: false,
    quantity: 999,
    sku: 'FORMATION-JS-ADV-001',
    vendor: 'CodeAcademy Pro',
    collections: ['digital', 'education', 'programming'],
    tags: ['javascript', 'formation', 'programmation', 'web-development'],
    seoTitle: 'Formation JavaScript Avancé - Maîtrisez JS Moderne',
    seoDescription: 'Formation JavaScript complète : ES6+, async/await, frameworks modernes. Devenez expert en développement web.',
    media: [
      {
        url: '/api/placeholder/400/300',
        type: 'image',
        alt: 'Aperçu formation JavaScript',
        size: 180000,
        displayOrder: 0
      },
      {
        url: '/api/placeholder/800/450',
        type: 'video',
        alt: 'Présentation de la formation',
        size: 8500000,
        displayOrder: 1
      }
    ],
    options: [],
    variants: []
  },
  {
    title: 'Consultation Web Design - 2h',
    description: '<p>Session de consultation personnalisée avec un expert en design web pour optimiser votre site.</p><h3>Ce qui est inclus :</h3><ul><li>Audit complet UX/UI</li><li>Recommandations personnalisées</li><li>Plan d\'action détaillé</li><li>Support post-consultation</li></ul>',
    status: 'Draft',
    type: 'service',
    onlineStore: false,
    pointOfSale: false,
    price: 180.00,
    costPerItem: 90.00,
    taxable: true,
    trackQuantity: false,
    quantity: 0,
    sku: 'CONSULT-WEBDESIGN-2H',
    vendor: 'Design Studio Pro',
    collections: ['services', 'consultation', 'design'],
    tags: ['consultation', 'web-design', 'ux-ui', 'service'],
    seoTitle: 'Consultation Web Design 2h - Optimisez votre Site',
    seoDescription: 'Consultation personnalisée avec expert en design web. Audit UX/UI complet et recommandations sur mesure.',
    media: [
      {
        url: '/api/placeholder/400/400',
        type: 'image',
        alt: 'Consultation web design',
        size: 145000,
        displayOrder: 0
      }
    ],
    options: [],
    variants: []
  }
]

// Fonction pour ajouter un produit de test rapide
export const createQuickTestProduct = (title = 'Produit Test', type = 'physical') => ({
  title,
  description: `<p>Description du ${title.toLowerCase()}</p>`,
  status: 'Draft',
  type,
  onlineStore: true,
  pointOfSale: false,
  price: Math.floor(Math.random() * 200) + 10,
  taxable: false,
  trackQuantity: true,
  quantity: Math.floor(Math.random() * 100) + 5,
  sku: `TEST-${Date.now().toString().slice(-6)}`,
  vendor: 'Test Vendor',
  collections: ['test'],
  tags: ['test', 'demo'],
  media: [{
    url: '/api/placeholder/400/400',
    type: 'image',
    alt: `Image ${title}`,
    size: 200000,
    displayOrder: 0
  }],
  options: [],
  variants: []
})