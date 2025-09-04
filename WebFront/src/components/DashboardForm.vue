<template>
  <div class="dashboard-container">
    <!-- Header Section -->
    <div class="page-header">
      <div class="header-content">
        <div class="title-section">
          <h1 class="page-title">{{ $t('dashboard') }}</h1>
          <p class="page-subtitle">{{ $t('overviewOfYourBusiness') }}</p>
        </div>
        <div class="header-actions">
          <Button 
            :label="$t('exportReport')" 
            icon="pi pi-download" 
            severity="secondary" 
            outlined 
            class="action-btn"
            @click="exportReport"
            :loading="exportLoading"
          />
          <Button 
            :label="$t('addCustomer')" 
            icon="pi pi-plus" 
            class="primary-btn"
            @click="openCustomerDialog"
          />
        </div>
      </div>
    </div>

    <!-- Key Metrics Section -->
    <div class="metrics-grid">
      <Card class="metric-card">
        <template #content>
          <div class="metric-content">
            <i class="pi pi-users metric-icon"></i>
            <div class="metric-info">
              <span class="metric-value">{{ formatNumber(totalCustomers) }}</span>
              <span class="metric-label">{{ $t('totalCustomers') }}</span>
            </div>
          </div>
        </template>
      </Card>
      <Card class="metric-card">
        <template #content>
          <div class="metric-content">
            <i class="pi pi-shopping-cart metric-icon"></i>
            <div class="metric-info">
              <span class="metric-value">{{ formatNumber(totalOrders) }}</span>
              <span class="metric-label">{{ $t('totalOrders') }}</span>
            </div>
          </div>
        </template>
      </Card>
      <Card class="metric-card">
        <template #content>
          <div class="metric-content">
            <i class="pi pi-chart-line metric-icon"></i>
            <div class="metric-info">
              <span class="metric-value">{{ formatCurrency(totalRevenue) }}</span>
              <span class="metric-label">{{ $t('totalRevenue') }}</span>
            </div>
          </div>
        </template>
      </Card>
      <Card class="metric-card">
        <template #content>
          <div class="metric-content">
            <i class="pi pi-user-plus metric-icon"></i>
            <div class="metric-info">
              <span class="metric-value">{{ formatNumber(newCustomersThisMonth) }}</span>
              <span class="metric-label">{{ $t('newCustomersThisMonth') }}</span>
            </div>
          </div>
        </template>
      </Card>
    </div>

    <!-- Charts Section -->
    <div class="charts-grid">
      <Card class="chart-card">
        <template #header>
          <div class="chart-header">
            <h3 class="chart-title">{{ $t('customerGrowth') }}</h3>
            <Dropdown 
              v-model="customerChartPeriod" 
              :options="chartPeriods" 
              optionLabel="label" 
              optionValue="value" 
              class="chart-filter"
            />
          </div>
        </template>
        <template #content>
          <Chart type="line" :data="customerChartData" :options="chartOptions" class="chart" />
        </template>
      </Card>
      <Card class="chart-card">
        <template #header>
          <div class="chart-header">
            <h3 class="chart-title">{{ $t('orderTrends') }}</h3>
            <Dropdown 
              v-model="orderChartPeriod" 
              :options="chartPeriods" 
              optionLabel="label" 
              optionValue="value" 
              class="chart-filter"
            />
          </div>
        </template>
        <template #content>
          <Chart type="bar" :data="orderChartData" :options="chartOptions" class="chart" />
        </template>
      </Card>
    </div>

    <!-- Recent Activity Section -->
    <Card class="activity-card">
      <template #header>
        <div class="activity-header">
          <h3 class="activity-title">{{ $t('recentActivity') }}</h3>
          <Button 
            :label="$t('viewAll')" 
            text 
            @click="viewAllActivities"
            class="view-all-btn"
          />
        </div>
      </template>
      <template #content>
        <DataTable :value="recentActivities" class="activity-table" responsiveLayout="scroll">
          <Column field="date" :header="$t('date')" sortable>
            <template #body="{ data }">
              {{ formatDate(data.date) }}
            </template>
          </Column>
          <Column field="customer" :header="$t('customer')" sortable />
          <Column field="action" :header="$t('action')" />
          <Column field="details" :header="$t('details')" />
        </DataTable>
        <div v-if="!recentActivities.length" class="empty-state">
          <i class="pi pi-info-circle empty-icon"></i>
          <p class="empty-message">{{ $t('noRecentActivity') }}</p>
        </div>
      </template>
    </Card>

    <!-- Customer Form Dialog -->
    <Dialog 
      v-model:visible="showCustomerDialog" 
      :header="dialogTitle"
      :style="{ width: '90vw', maxWidth: '800px' }" 
      :maximizable="true" 
      :modal="true"
      class="customer-dialog"
      @hide="onDialogHide"
    >
      <CustomerForm 
        :customer="selectedCustomer" 
        :mode="formMode"
        :loading="saveLoading"
        @save="saveCustomer" 
        @cancel="closeCustomerDialog"
      />
    </Dialog>

    <!-- Toast Messages -->
    <Toast />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useToast } from 'primevue/usetoast'
import { useDashboardStore } from '@/stores/dashboardStore'
import Chart from 'primevue/chart'
import Card from 'primevue/card'
import Button from 'primevue/button'
import Dropdown from 'primevue/dropdown'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dialog from 'primevue/dialog'
import Toast from 'primevue/toast'
import CustomerForm from '@/components/CustomerForm.vue'

// Composables
const { t } = useI18n()
const toast = useToast()
const dashboardStore = useDashboardStore()

// Reactive Data
const exportLoading = ref(false)
const showCustomerDialog = ref(false)
const selectedCustomer = ref(null)
const formMode = ref('add')
const saveLoading = ref(false)
const customerChartPeriod = ref('monthly')
const orderChartPeriod = ref('monthly')

// Options
const chartPeriods = [
  { label: t('weekly'), value: 'weekly' },
  { label: t('monthly'), value: 'monthly' },
  { label: t('yearly'), value: 'yearly' }
]

// Computed Properties
const totalCustomers = computed(() => dashboardStore.customers.length)
const totalOrders = computed(() => 
  dashboardStore.customers.reduce((sum, customer) => sum + customer.totalOrders, 0)
)
const totalRevenue = computed(() => totalOrders.value * 100) // Placeholder: $100 per order
const newCustomersThisMonth = computed(() => 
  dashboardStore.customers.filter(c => {
    const date = new Date(c.lastOrderDate || Date.now())
    return date.getMonth() === new Date().getMonth() && date.getFullYear() === new Date().getFullYear()
  }).length
)

const customerChartData = computed(() => ({
  labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
  datasets: [{
    label: t('customers'),
    data: [50, 75, 100, 120, 150, totalCustomers.value],
    borderColor: '#3b82f6',
    backgroundColor: 'rgba(59, 130, 246, 0.2)',
    fill: true
  }]
}))

const orderChartData = computed(() => ({
  labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
  datasets: [{
    label: t('orders'),
    data: [200, 250, 300, 280, 350, totalOrders.value],
    backgroundColor: '#3b82f6'
  }]
}))

const chartOptions = computed(() => ({
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: { display: true, position: 'top' }
  }
}))

const recentActivities = computed(() => [
  // Placeholder data
  {
    date: new Date().toISOString(),
    customer: 'John Doe',
    action: t('placedOrder'),
    details: t('orderDetails', { id: '1234' })
  },
  {
    date: new Date(Date.now() - 86400000).toISOString(),
    customer: 'Jane Smith',
    action: t('updatedProfile'),
    details: t('changedEmail')
  }
])

const dialogTitle = computed(() => 
  formMode.value === 'add' ? t('addCustomer') : t('editCustomer')
)

// Methods
const formatNumber = (value) => 
  new Intl.NumberFormat('fr-TN').format(value || 0)

const formatCurrency = (value) => 
  new Intl.NumberFormat('fr-TN', { style: 'currency', currency: 'TND' }).format(value || 0)

const formatDate = (date) => 
  date ? new Intl.DateTimeFormat('fr-TN', { dateStyle: 'medium' }).format(new Date(date)) : 'N/A'

const exportReport = async () => {
  exportLoading.value = true
  try {
    // Placeholder for report export
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('reportExported'),
      life: 3000
    })
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: t('error'),
      detail: t('failedToExport'),
      life: 5000
    })
  } finally {
    exportLoading.value = false
  }
}

const openCustomerDialog = () => {
  selectedCustomer.value = null
  formMode.value = 'add'
  showCustomerDialog.value = true
}

const saveCustomer = async (customer) => {
  saveLoading.value = true
  try {
    if (formMode.value === 'add') {
      await dashboardStore.addCustomer(customer)
      toast.add({
        severity: 'success',
        summary: t('success'),
        detail: t('customerAdded'),
        life: 3000
      })
    } else {
      await dashboardStore.updateCustomer(customer)
      toast.add({
        severity: 'success',
        summary: t('success'),
        detail: t('customerUpdated'),
        life: 3000
      })
    }
    closeCustomerDialog()
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: t('error'),
      detail: t('failedToSave'),
      life: 5000
    })
  } finally {
    saveLoading.value = false
  }
}

const closeCustomerDialog = () => {
  showCustomerDialog.value = false
  selectedCustomer.value = null
  formMode.value = 'add'
}

const onDialogHide = () => {
  closeCustomerDialog()
}

const viewAllActivities = () => {
  toast.add({
    severity: 'info',
    summary: t('comingSoon'),
    detail: t('activityLogComingSoon'),
    life: 3000
  })
}

// Lifecycle
onMounted(async () => {
  try {
    await dashboardStore.fetchCustomers()
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: t('error'),
      detail: t('failedToLoadData'),
      life: 5000
    })
  }
})
</script>

<style scoped>
.dashboard-container {
  padding: 1.5rem;
  background: #f8fafc;
  min-height: 100vh;
}

/* Header Styles */
.page-header {
  margin-bottom: 2rem;
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  background: white;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.title-section {
  flex: 1;
}

.page-title {
  font-size: 2rem;
  font-weight: 700;
  color: #1f2937;
  margin: 0 0 0.5rem 0;
}

.page-subtitle {
  color: #6b7280;
  font-size: 1rem;
  margin: 0;
}

.header-actions {
  display: flex;
  gap: 0.75rem;
  align-items: center;
}

.action-btn {
  min-width: 120px;
}

.primary-btn {
  background: linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%);
  border: none;
  font-weight: 600;
  box-shadow: 0 4px 6px -1px rgba(59, 130, 246, 0.3);
  transition: all 0.2s;
}

.primary-btn:hover {
  transform: translateY(-1px);
  box-shadow: 0 8px 15px -3px rgba(59, 130, 246, 0.4);
}

/* Metrics Styles */
.metrics-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.metric-card {
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  border-radius: 12px;
  overflow: hidden;
}

.metric-content {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1.5rem;
}

.metric-icon {
  font-size: 2rem;
  color: #3b82f6;
}

.metric-info {
  display: flex;
  flex-direction: column;
}

.metric-value {
  font-size: 1.5rem;
  font-weight: 600;
  color: #1f2937;
}

.metric-label {
  font-size: 0.875rem;
  color: #6b7280;
}

/* Charts Styles */
.charts-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.chart-card {
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  border-radius: 12px;
}

.chart-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  border-bottom: 1px solid #e5e7eb;
}

.chart-title {
  font-size: 1.125rem;
  font-weight: 600;
  color: #1f2937;
  margin: 0;
}

.chart-filter {
  width: 150px;
}

.chart {
  height: 300px;
  padding: 1.5rem;
}

/* Activity Styles */
.activity-card {
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  border-radius: 12px;
}

.activity-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  border-bottom: 1px solid #e5e7eb;
}

.activity-title {
  font-size: 1.125rem;
  font-weight: 600;
  color: #1f2937;
  margin: 0;
}

.view-all-btn {
  font-size: 0.875rem;
}

.activity-table {
  padding: 1.5rem;
}

:deep(.activity-table) {
  .p-datatable-thead > tr > th {
    background: #f9fafb;
    color: #374151;
    font-weight: 600;
    text-transform: uppercase;
    font-size: 0.75rem;
    letter-spacing: 0.05em;
    border-bottom: 2px solid #e5e7eb;
  }
  
  .p-datatable-tbody > tr {
    transition: background-color 0.2s;
    
    &:hover {
      background: #f3f4f6 !important;
    }
  }
}

.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 2rem;
  text-align: center;
}

.empty-icon {
  font-size: 2rem;
  color: #9ca3af;
  margin-bottom: 1rem;
}

.empty-message {
  color: #6b7280;
  font-size: 1rem;
}

/* Dialog Styles */
.customer-dialog {
  &:deep(.p-dialog-header) {
    border-bottom: 1px solid #e5e7eb;
  }
  
  &:deep(.p-dialog-content) {
    padding: 0;
    overflow-y: auto;
    max-height: calc(100vh - 150px);
  }
}

/* Responsive Adjustments */
@media (max-width: 992px) {
  .header-content {
    flex-direction: column;
    gap: 1rem;
  }
  
  .header-actions {
    width: 100%;
    flex-wrap: wrap;
  }
  
  .action-btn, .primary-btn {
    flex: 1;
    min-width: auto;
  }
}

@media (max-width: 768px) {
  .dashboard-container {
    padding: 1rem;
  }
  
  .metrics-grid {
    grid-template-columns: 1fr;
  }
  
  .charts-grid {
    grid-template-columns: 1fr;
  }
}
</style>