import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const useDashboardStore = defineStore('dashboard', () => {
  // State
  const setupTasks = ref({
    importProducts: false,
    manageServices: false,
    customizeStore: false,
    importCustomers: false,
    importOrders: false,
    addPaymentMethods: false,
    configureShipping: false,
    setTaxRates: false,
    createCategories: false,
    addTeamMembers: false,
    setupAnalytics: false,
    configureEmail: false,
    addSocialMedia: false,
    createHomepage: false,
    addBlog: false,
    setupSeo: false,
    launchStore: false
  });

  const notifications = ref([
    {
      id: 1,
      type: 'info',
      title: 'Nouvelle commande',
      message: 'Vous avez reçu une nouvelle commande #1234',
      timestamp: new Date(),
      read: false
    },
    {
      id: 2,
      type: 'warning',
      title: 'Stock faible',
      message: 'Le produit "T-shirt Rouge" a un stock faible',
      timestamp: new Date(Date.now() - 3600000),
      read: false
    },
    {
      id: 3,
      type: 'success',
      title: 'Paiement reçu',
      message: 'Paiement de 150€ reçu pour la commande #1233',
      timestamp: new Date(Date.now() - 7200000),
      read: true
    }
  ]);

  const currentUser = ref({
    name: 'Abdoul Razak...',
    email: 'abdoul.hamid@example.com',
    avatar: 'AH',
    role: 'Admin'
  });

  const storeSettings = ref({
    storeName: 'Ma Boutique Nexashop',
    currency: 'EUR',
    language: 'fr',
    timezone: 'Europe/Paris',
    planType: 'trial',
    trialDaysLeft: 3
  });

  // Getters
  const completedTasksCount = computed(() => {
    return Object.values(setupTasks.value).filter(Boolean).length;
  });

  const totalTasksCount = computed(() => {
    return Object.keys(setupTasks.value).length;
  });

  const setupProgress = computed(() => {
    return Math.round((completedTasksCount.value / totalTasksCount.value) * 100);
  });

  const unreadNotificationsCount = computed(() => {
    return notifications.value.filter(n => !n.read).length;
  });

  const isTrialExpiring = computed(() => {
    return storeSettings.value.planType === 'trial' && storeSettings.value.trialDaysLeft <= 3;
  });

  // Actions
  const toggleTask = (taskKey) => {
    if (taskKey in setupTasks.value) {
      setupTasks.value[taskKey] = !setupTasks.value[taskKey];
    }
  };

  const completeTask = (taskKey) => {
    if (taskKey in setupTasks.value) {
      setupTasks.value[taskKey] = true;
    }
  };

  const markNotificationAsRead = (notificationId) => {
    const notification = notifications.value.find(n => n.id === notificationId);
    if (notification) {
      notification.read = true;
    }
  };

  const markAllNotificationsAsRead = () => {
    notifications.value.forEach(notification => {
      notification.read = true;
    });
  };

  const addNotification = (notification) => {
    const newNotification = {
      id: Date.now(),
      timestamp: new Date(),
      read: false,
      ...notification
    };
    notifications.value.unshift(newNotification);
  };

  const removeNotification = (notificationId) => {
    const index = notifications.value.findIndex(n => n.id === notificationId);
    if (index > -1) {
      notifications.value.splice(index, 1);
    }
  };

  const updateStoreSettings = (newSettings) => {
    storeSettings.value = { ...storeSettings.value, ...newSettings };
  };

  const updateUserProfile = (newProfile) => {
    currentUser.value = { ...currentUser.value, ...newProfile };
  };

  // Analytics data
  const analyticsData = ref({
    totalOrders: 156,
    totalRevenue: 12450.80,
    totalCustomers: 89,
    conversionRate: 3.2,
    averageOrderValue: 79.94,
    topProducts: [
      { name: 'T-shirt Premium', sales: 45, revenue: 1350 },
      { name: 'Jeans Slim', sales: 32, revenue: 1920 },
      { name: 'Sneakers Classic', sales: 28, revenue: 2240 }
    ],
    salesChart: [
      { date: '2024-01-01', sales: 1200 },
      { date: '2024-01-02', sales: 1450 },
      { date: '2024-01-03', sales: 1100 },
      { date: '2024-01-04', sales: 1800 },
      { date: '2024-01-05', sales: 1650 },
      { date: '2024-01-06', sales: 2100 },
      { date: '2024-01-07', sales: 1900 }
    ]
  });

  const updateAnalytics = (newData) => {
    analyticsData.value = { ...analyticsData.value, ...newData };
  };

  // Recent activities
  const recentActivities = ref([
    {
      id: 1,
      type: 'order',
      message: 'Nouvelle commande #1234 reçue',
      timestamp: new Date(),
      icon: 'pi-shopping-cart',
      color: 'text-green-500'
    },
    {
      id: 2,
      type: 'customer',
      message: 'Nouveau client inscrit: Marie Dupont',
      timestamp: new Date(Date.now() - 1800000),
      icon: 'pi-user-plus',
      color: 'text-blue-500'
    },
    {
      id: 3,
      type: 'product',
      message: 'Produit "Chaussures Sport" ajouté',
      timestamp: new Date(Date.now() - 3600000),
      icon: 'pi-box',
      color: 'text-purple-500'
    }
  ]);

  const addActivity = (activity) => {
    const newActivity = {
      id: Date.now(),
      timestamp: new Date(),
      ...activity
    };
    recentActivities.value.unshift(newActivity);
    
    if (recentActivities.value.length > 10) {
      recentActivities.value = recentActivities.value.slice(0, 10);
    }
  };

  return {
    setupTasks,
    notifications,
    currentUser,
    storeSettings,
    analyticsData,
    recentActivities,
    completedTasksCount,
    totalTasksCount,
    setupProgress,
    unreadNotificationsCount,
    isTrialExpiring,
    toggleTask,
    completeTask,
    markNotificationAsRead,
    markAllNotificationsAsRead,
    addNotification,
    removeNotification,
    updateStoreSettings,
    updateUserProfile,
    updateAnalytics,
    addActivity
  };
});