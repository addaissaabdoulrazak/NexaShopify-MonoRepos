<template>
  <div 
    class="sidebar-container" 
    :class="{ 
      'mobile': mobile, 
      'collapsed': collapsed && !mobile,
      'expanded': !collapsed || mobile 
    }"
  >
    <!-- Sidebar Header -->
    <div class="sidebar-header">
      <div class="header-content">
        <!-- Logo Section -->
        <div class="logo-section" :class="{ 'centered': collapsed && !mobile }">
          <div class="logo-icon">
            <i class="pi pi-shopping-bag"></i>
          </div>
          <Transition name="fade-slide">
            <div v-if="!collapsed || mobile" class="logo-content">
              <span class="logo-text">NexaShop</span>
              <span class="logo-subtitle">Admin Panel</span>
            </div>
          </Transition>
        </div>

        <!-- Mobile Close Button -->
        <Button 
          v-if="mobile"
          icon="pi pi-times" 
          text
          rounded
          size="small"
          class="close-btn"
          @click="$emit('close')"
          aria-label="Close menu"
        />

        <!-- Desktop Toggle Button -->
        <Button 
          v-if="!mobile"
          :icon="collapsed ? 'pi pi-angle-double-right' : 'pi pi-angle-double-left'"
          text
          rounded
          size="small"
          class="toggle-btn"
          @click="$emit('toggle-collapse')"
          v-tooltip.right="collapsed ? 'Expand sidebar' : 'Collapse sidebar'"
          aria-label="Toggle sidebar"
        />
      </div>
    </div>

    <!-- Navigation Menu -->
    <nav class="sidebar-nav">
      <div class="nav-section">
        <div 
          v-if="!collapsed || mobile" 
          class="section-title"
        >
          <span>Navigation</span>
        </div>
        
        <div class="nav-items">
          <router-link
            v-for="item in mainMenuItems"
            :key="item.name"
            :to="item.path"
            class="nav-item"
            :class="{ 
              'active': isActiveRoute(item),
              'collapsed': collapsed && !mobile 
            }"
            @click="handleNavClick"
            v-tooltip.right="collapsed && !mobile ? item.label : ''"
          >
            <div class="nav-icon">
              <i :class="item.icon"></i>
              <div v-if="item.badge" class="nav-badge">
                <Badge :value="item.badge" severity="info" size="small" />
              </div>
            </div>
            <Transition name="fade-slide">
              <span v-if="!collapsed || mobile" class="nav-label">
                {{ item.label }}
              </span>
            </Transition>
            <Transition name="fade-slide">
              <i 
                v-if="(!collapsed || mobile) && item.submenu" 
                class="nav-arrow pi pi-chevron-right"
              ></i>
            </Transition>
          </router-link>
        </div>
      </div>

      <!-- Secondary Menu -->
      <div class="nav-section">
        <div 
          v-if="!collapsed || mobile" 
          class="section-title"
        >
          <span>Tools</span>
        </div>
        
        <div class="nav-items">
          <router-link
            v-for="item in toolsMenuItems"
            :key="item.name"
            :to="item.path"
            class="nav-item"
            :class="{ 
              'active': isActiveRoute(item),
              'collapsed': collapsed && !mobile 
            }"
            @click="handleNavClick"
            v-tooltip.right="collapsed && !mobile ? item.label : ''"
          >
            <div class="nav-icon">
              <i :class="item.icon"></i>
            </div>
            <Transition name="fade-slide">
              <span v-if="!collapsed || mobile" class="nav-label">
                {{ item.label }}
              </span>
            </Transition>
          </router-link>
        </div>
      </div>
    </nav>

    <!-- Sidebar Bottom -->
    <div class="sidebar-bottom">
      <div 
        v-if="!collapsed || mobile" 
        class="info-card sales-channel"
      >
        <div class="card-header">
          <i class="pi pi-globe card-icon"></i>
          <span class="card-title">Canal de vente</span>
        </div>
        <div class="card-content">
          <div class="channel-info">
            <span class="channel-name">Boutique en ligne</span>
            <div class="channel-status">
              <div class="status-dot active"></div>
              <span class="status-text">Actif</span>
            </div>
          </div>
        </div>
      </div>

      <div 
        v-if="!collapsed || mobile" 
        class="info-card apps-card"
      >
        <div class="card-header">
          <i class="pi pi-th-large card-icon"></i>
          <span class="card-title">Applications</span>
        </div>
        <div class="card-content">
          <Button 
            label="Ajouter des apps" 
            icon="pi pi-plus" 
            text 
            size="small"
            class="add-apps-btn"
          />
        </div>
      </div>

      <div v-if="collapsed && !mobile" class="collapsed-actions">
        <Button
          icon="pi pi-globe"
          text
          rounded
          class="collapsed-action-btn"
          v-tooltip.right="'Canal de vente'"
        />
        <Button
          icon="pi pi-th-large"
          text
          rounded
          class="collapsed-action-btn"
          v-tooltip.right="'Applications'"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { computed } from 'vue';
import { useRoute } from 'vue-router';

export default {
  name: 'AppSidebar',
  props: {
    mobile: {
      type: Boolean,
      default: false
    },
    collapsed: {
      type: Boolean,
      default: false
    }
  },
  emits: ['close', 'toggle-collapse'],
  setup(props, { emit }) {
    const route = useRoute();

    const mainMenuItems = [
      { 
        name: 'Dashboard', 
        path: '/seller-dashboard', 
        label: 'Tableau de bord', 
        icon: 'pi pi-home'
      },
      { 
        name: 'Orders', 
        path: '/orders', 
        label: 'Commandes', 
        icon: 'pi pi-shopping-cart',
        badge: '12'
      },
      { 
        name: 'Products', 
        path: '/products', 
        label: 'Produits', 
        icon: 'pi pi-box',
        submenu: true
      },
      { 
        name: 'Customers', 
        path: '/customers', 
        label: 'Clients', 
        icon: 'pi pi-users'
      },
      { 
        name: 'Analytics', 
        path: '/analytics', 
        label: 'Analytics', 
        icon: 'pi pi-chart-line'
      },
      { 
        name: 'Marketing', 
        path: '/marketing', 
        label: 'Marketing', 
        icon: 'pi pi-megaphone',
        submenu: true
      }
    ];

    const toolsMenuItems = [
      { 
        name: 'Discounts', 
        path: '/discounts', 
        label: 'Réductions', 
        icon: 'pi pi-tag'
      },
      { 
        name: 'Reports', 
        path: '/reports', 
        label: 'Rapports', 
        icon: 'pi pi-file-pdf'
      },
      { 
        name: 'Settings', 
        path: '/settings', 
        label: 'Paramètres', 
        icon: 'pi pi-cog'
      }
    ];

    const isActiveRoute = (item) => {
      return route.path === item.path || route.name === item.name;
    };

    const handleNavClick = () => {
      if (props.mobile) {
        emit('close');
      }
    };

    return {
      mainMenuItems,
      toolsMenuItems,
      isActiveRoute,
      handleNavClick
    };
  }
}
</script>

<style scoped>
/* Base Sidebar Styles */
.sidebar-container {
  height: 100vh;
  background: linear-gradient(180deg, #ffffff 0%, #fafbfc 100%);
  border-right: 1px solid #e2e8f0;
  display: flex;
  flex-direction: column;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: 0 0 0 1px rgba(0, 0, 0, 0.05);
  position: relative;
  overflow: hidden;
}

.sidebar-container.expanded {
  width: 280px;
}

.sidebar-container.collapsed {
  width: 72px;
}

.sidebar-container.mobile {
  width: 280px;
}

/* Header */
.sidebar-header {
  padding: 20px;
  border-bottom: 1px solid #e2e8f0;
  background: white;
  position: relative;
}

.header-content {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.logo-section {
  display: flex;
  align-items: center;
  gap: 12px;
  transition: all 0.3s ease;
}

.logo-section.centered {
  justify-content: center;
  width: 100%;
}

.logo-icon {
  width: 40px;
  height: 40px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 18px;
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);
  flex-shrink: 0;
}

.logo-content {
  display: flex;
  flex-direction: column;
}

.logo-text {
  font-size: 18px;
  font-weight: 700;
  color: #1a202c;
  line-height: 1.2;
}

.logo-subtitle {
  font-size: 12px;
  color: #64748b;
  font-weight: 500;
}

.close-btn,
.toggle-btn {
  color: #64748b;
  width: 32px;
  height: 32px;
  flex-shrink: 0;
}

.close-btn:hover,
.toggle-btn:hover {
  background: #f1f5f9;
  color: #334155;
}

/* Navigation */
.sidebar-nav {
  flex: 1;
  padding: 16px 0;
  overflow-y: auto;
  overflow-x: hidden;
}

.nav-section {
  margin-bottom: 24px;
}

.section-title {
  padding: 0 20px 12px;
  font-size: 11px;
  font-weight: 600;
  text-transform: uppercase;
  color: #64748b;
  letter-spacing: 0.5px;
}

.nav-items {
  display: flex;
  flex-direction: column;
}

.nav-item {
  display: flex;
  align-items: center;
  padding: 12px 20px;
  color: #475569;
  text-decoration: none;
  transition: all 0.2s ease;
  position: relative;
  border-left: 3px solid transparent;
}

.nav-item.collapsed {
  padding: 12px;
  justify-content: center;
}

.nav-item:hover {
  background: linear-gradient(90deg, rgba(102, 126, 234, 0.08) 0%, rgba(102, 126, 234, 0.04) 100%);
  color: #334155;
  border-left-color: #667eea;
}

.nav-item.active {
  background: linear-gradient(90deg, rgba(102, 126, 234, 0.12) 0%, rgba(102, 126, 234, 0.06) 100%);
  color: #667eea;
  border-left-color: #667eea;
  font-weight: 600;
}

.nav-item.active::before {
  content: '';
  position: absolute;
  right: 20px;
  top: 50%;
  transform: translateY(-50%);
  width: 6px;
  height: 6px;
  background: #667eea;
  border-radius: 50%;
}

.nav-item.collapsed.active::before {
  right: 50%;
  transform: translate(50%, -50%);
}

.nav-icon {
  width: 20px;
  height: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  flex-shrink: 0;
}

.nav-icon i {
  font-size: 18px;
  transition: all 0.2s ease;
}

.nav-badge {
  position: absolute;
  top: -8px;
  right: -8px;
}

.nav-label {
  flex: 1;
  margin-left: 12px;
  font-size: 14px;
  font-weight: 500;
}

.nav-arrow {
  margin-left: auto;
  font-size: 12px;
  color: #94a3b8;
  transition: transform 0.2s ease;
}

.nav-item:hover .nav-arrow {
  transform: translateX(2px);
}

/* Bottom Section */
.sidebar-bottom {
  padding: 20px;
  border-top: 1px solid #e2e8f0;
  background: #fafbfc;
}

.info-card {
  background: white;
  border: 1px solid #e2e8f0;
  border-radius: 12px;
  padding: 16px;
  margin-bottom: 12px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
  transition: all 0.2s ease;
}

.info-card:hover {
  border-color: #cbd5e1;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
}

.card-header {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 12px;
}

.card-icon {
  color: #667eea;
  font-size: 16px;
}

.card-title {
  font-size: 13px;
  font-weight: 600;
  color: #374151;
}

.card-content {
  margin-top: 8px;
}

.channel-info {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.channel-name {
  font-size: 14px;
  font-weight: 500;
  color: #1f2937;
}

.channel-status {
  display: flex;
  align-items: center;
  gap: 6px;
}

.status-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: #10b981;
}

.status-dot.active {
  background: #10b981;
  box-shadow: 0 0 0 2px rgba(16, 185, 129, 0.2);
}

.status-text {
  font-size: 12px;
  color: #10b981;
  font-weight: 500;
}

.add-apps-btn {
  width: 100%;
  justify-content: flex-start;
  color: #667eea;
  font-weight: 500;
  padding: 8px 12px;
  border-radius: 8px;
}

.add-apps-btn:hover {
  background: rgba(102, 126, 234, 0.08);
}

/* Collapsed Actions */
.collapsed-actions {
  display: flex;
  flex-direction: column;
  gap: 8px;
  align-items: center;
}

.collapsed-action-btn {
  width: 40px;
  height: 40px;
  color: #64748b;
  border-radius: 10px;
}

.collapsed-action-btn:hover {
  background: rgba(102, 126, 234, 0.08);
  color: #667eea;
}

/* Transitions */
.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.fade-slide-enter-from {
  opacity: 0;
  transform: translateX(-10px);
}

.fade-slide-leave-to {
  opacity: 0;
  transform: translateX(-10px);
}

/* Scrollbar */
.sidebar-nav::-webkit-scrollbar {
  width: 4px;
}

.sidebar-nav::-webkit-scrollbar-track {
  background: transparent;
}

.sidebar-nav::-webkit-scrollbar-thumb {
  background: #cbd5e1;
  border-radius: 2px;
}

.sidebar-nav::-webkit-scrollbar-thumb:hover {
  background: #94a3b8;
}

/* Mobile Responsive */
@media (max-width: 767px) {
  .sidebar-container {
    position: fixed;
    top: 0;
    left: 0;
    z-index: 1100;
    box-shadow: 0 20px 40px rgba(0, 0, 0, 0.15);
  }

  .sidebar-header {
    padding: 16px 20px;
  }

  .nav-item {
    padding: 14px 20px;
  }

  .sidebar-bottom {
    padding: 16px 20px;
  }
}

/* Tablet Responsive */
@media (min-width: 768px) and (max-width: 1023px) {
  .sidebar-container.expanded {
    width: 72px;
  }

  .section-title {
    display: none;
  }

  .nav-item {
    padding: 12px;
    justify-content: center;
  }
}

/* High contrast mode */
@media (prefers-contrast: high) {
  .sidebar-container {
    border-right-width: 2px;
  }
  
  .info-card {
    border-width: 2px;
  }
}

/* Reduced motion */
@media (prefers-reduced-motion: reduce) {
  .sidebar-container,
  .nav-item,
  .fade-slide-enter-active,
  .fade-slide-leave-active {
    transition: none;
  }
}
</style>