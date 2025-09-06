import { createRouter, createWebHistory } from "vue-router";
import { useAuthStore } from '@/stores/auth'
import HowItWorks from '@/views/HowItWorks.vue'

// Import views seller dashboard components
import Dashboard from '../views/Dashboard.vue';
import Orders from '../views/Orders.vue';
import Products from '../views/Products.vue';
import Customers from '../views/Customers.vue';
import Analytics from '../views/Analytics.vue';
import Marketing from '../views/Marketing.vue';
import Discounts from '../views/Discounts.vue';
import Settings from '../views/Settings.vue';
import ChoosePlan from '../views/ChoosePlan.vue';

const routes = [
  { 
    path: "/", 
    name: "home", 
    component: () => import("@/views/InitialView.vue"),
    meta: { title: "Accueil - NexaShop" }
  },
  // { 
  //   path: "/products", 
  //   name: "products", 
  //   component: () => import("@/views/ProductsView.vue"), // applying Lazy Loading 
  //   meta: { title: "Produits - NexaShop" }
  // },
  // { 
  //   path: "/buyer", 
  //   name: "buyerhome", 
  //   component: () => import("@/views/BuyerHome.vue"),
  //   meta: { title: "Espace acheteur - NexaShop" }
  // },
  { 
    path: "/seller", 
    name: "seller-dashboard", 
    component: () => import("@/views/SellerDashboard.vue"),
    meta: { title: "Tableau de bord vendeur - NexaShop" },
    meta: { requiresAuth: true }
  },
  { 
    path: "/create-shop", 
    name: "create-shop", 
    component: () => import("@/views/CreateShopView.vue"),
    meta: { title: "Créer votre boutique - NexaShop" }
  },
  { 
    path: "/login", 
    name: "login", 
    component: () => import("@/views/LoginView.vue"),
    meta: { title: "Connexion - NexaShop" }
  },
  { 
    path: "/register", 
    name: "register", 
    component: () => import("@/views/SignupView.vue"),
    meta: { title: "Inscription - NexaShop" }
  },
  {
    path: '/how-it-works',
    name: 'HowItWorks',
    component: HowItWorks,
    meta: { title: 'Comment ça marche' }
  },
  {
    path: '/admin',
    name: 'admin',
    component: () => import("@/views/AdminDashboard.vue"),
    meta: { title: "Tableau de bord Admin - NexaShop" }
  },
  // Sellers Dashboard routes
  { path: '/seller-dashboard', name: 'Dashboard', component: Dashboard,meta: { layout: 'layoutseller' } },
  { path: '/orders', name: 'Orders', component: Orders, meta: { layout: 'layoutseller' } },
  { path: '/products', name: 'Products', component: Products, meta: { layout: 'layoutseller' } },
  { path: '/customers', name: 'Customers', component: Customers, meta: { layout: 'layoutseller' } },
  { path: '/analytics', name: 'Analytics', component: Analytics, meta: { layout: 'layoutseller' } },
  { path: '/marketing', name: 'Marketing', component: Marketing, meta: { layout: 'layoutseller' } },
  { path: '/discounts', name: 'Discounts', component: Discounts, meta: { layout: 'layoutseller' } },
  { path: '/settings', name: 'Settings', component: Settings, meta: { layout: 'layoutseller' } },
  { path: '/choose-plan', name: 'ChoosePlan', component: ChoosePlan, meta: { title: 'Choisissez votre plan' } },
  { path: '/:pathMatch(.*)*', name: 'NotFound', component: () => import('../views/NotFound.vue') }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition;
    }
    return { top: 0 };
  }
});

// Gestion du titre
router.beforeEach((to) => {
  document.title = to.meta.title || 'NexaShop';
});

// Gestion du spinner - version corrigée
// router.beforeEach((to, from, next) => {

//   console.log(`Navigating from ${from.path} to ${to.path} at ${new Date().toLocaleString('fr-FR', { timeZone: 'CET' })}`);
//   const spinner = document.getElementById('nb-global-spinner');
//   if (spinner) {
//     spinner.style.display = 'flex';
//     spinner.style.opacity = '1';
//   }
//   next();
// });

// Gestion du spinner - version corrigée
router.beforeEach((to, from, next) => {

  console.log(`Navigating from ${from.path} to ${to.path} at ${new Date().toLocaleString('fr-FR', { timeZone: 'CET' })}`);
  
  // Ne pas afficher le spinner pour certaines routes
  if (to.meta.layout === 'layoutseller') {
    return next(); // continuer sans spinner
  }

  const spinner = document.getElementById('nb-global-spinner');
  if (spinner) {
    spinner.style.display = 'flex';
    spinner.style.opacity = '1';
  }
  next();
});

//Gestion Authentification
router.beforeEach((to, from, next) => {
  const auth = useAuthStore()
  if (to.meta.requiresAuth && !auth.isLoggedIn) {
    next('/login')
  } else {
    next()
  }
})

router.afterEach(() => {
  setTimeout(() => {
    const spinner = document.getElementById('nb-global-spinner');
    if (spinner) {
      spinner.style.opacity = '0';
      setTimeout(() => {
        spinner.style.display = 'none';
      }, 500);
    }
  }, 300);
});

router.onError((error) => {
  const spinner = document.getElementById('nb-global-spinner');
  if (spinner) spinner.style.display = 'none';
  console.error("Erreur de navigation :", error);
});



export default router;