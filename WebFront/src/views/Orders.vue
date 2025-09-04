<template>
  <div class="orders-container">
    <!-- Header Section -->
    <div class="page-header">
      <div class="header-content">
        <div class="title-section">
          <h1 class="page-title">{{ $t('orders') }}</h1>
          <p class="page-subtitle">{{ $t('manageYourOrders') }}</p>
        </div>
        <div class="header-actions">
          <Button 
            :label="$t('export')" 
            icon="pi pi-download" 
            severity="secondary" 
            outlined 
            class="action-btn"
            @click="exportOrders"
            :loading="exportLoading"
          />
          <Button 
            :label="$t('import')" 
            icon="pi pi-upload" 
            severity="secondary" 
            outlined 
            class="action-btn"
            @click="importOrders"
          />
          <SplitButton 
            :label="$t('moreActions')" 
            icon="pi pi-ellipsis-v" 
            :model="moreActionsItems"
            severity="secondary"
            outlined
            class="action-btn"
          />
          <Button 
            :label="$t('addOrder')" 
            icon="pi pi-plus" 
            class="primary-btn"
            @click="openOrderDialog"
          />
        </div>
      </div>
    </div>

    <!-- Filters Section -->
    <Card class="filters-card">
      <template #content>
        <div class="filters-container">
          <div class="filters-row">
            <div class="filter-group">
              <label class="filter-label">{{ $t('status') }}</label>
              <Dropdown 
                v-model="filters.status" 
                :options="statusOptions" 
                optionLabel="label" 
                optionValue="value" 
                :placeholder="$t('allStatuses')"
                class="filter-dropdown"
                showClear
              />
            </div>
            <div class="filter-group">
              <label class="filter-label">{{ $t('customer') }}</label>
              <AutoComplete
                v-model="filters.customer"
                :suggestions="customerSuggestions"
                @complete="searchCustomers"
                :placeholder="$t('searchByCustomer')"
                class="filter-input"
                field="name"
                showClear
              />
            </div>
            <div class="filter-group">
              <label class="filter-label">{{ $t('dateRange') }}</label>
              <Calendar 
                v-model="filters.dateRange" 
                selectionMode="range" 
                :placeholder="$t('selectDateRange')"
                class="filter-input"
                showClear
              />
            </div>
            <div class="filter-group">
              <label class="filter-label">{{ $t('search') }}</label>
              <span class="p-input-icon-left">
                <i class="pi pi-search" />
                <InputText 
                  v-model="filters.search" 
                  :placeholder="$t('searchOrders')"
                  class="filter-input search-input"
                />
              </span>
            </div>
          </div>
          <div class="filters-actions">
            <Button 
              :label="$t('clearFilters')" 
              icon="pi pi-filter-slash" 
              text 
              @click="clearFilters"
              :disabled="!hasActiveFilters"
            />
            <Chip 
              v-if="filteredOrdersCount > 0"
              :label="`${filteredOrdersCount} ${$t('ordersFound')}`"
              class="results-chip"
            />
          </div>
        </div>
      </template>
    </Card>

    <!-- Orders Table -->
    <Card class="orders-table-card">
      <template #content>
        <DataTable
          v-model:selection="selectedOrders"
          :value="paginatedOrders"
          :loading="loading"
          :totalRecords="filteredOrdersCount"
          :rows="pagination.rows"
          :first="pagination.first"
          paginator
          :rowsPerPageOptions="[10, 25, 50]"
          responsiveLayout="scroll"
          dataKey="id"
          class="orders-table"
          @page="onPageChange"
          :globalFilterFields="['orderNumber', 'customer', 'status']"
        >
          <!-- Selection Column -->
          <Column selectionMode="multiple" headerStyle="width: 3rem" />
          
          <!-- Order Number Column -->
          <Column 
            field="orderNumber" 
            :header="$t('orderNumber')" 
            sortable
            class="order-number-column"
          >
            <template #body="{ data }">
              <span class="order-number">#{{ data.orderNumber }}</span>
            </template>
          </Column>
          
          <!-- Customer Column -->
          <Column 
            field="customer" 
            :header="$t('customer')" 
            sortable
            class="customer-column"
          >
            <template #body="{ data }">
              <div class="customer-cell">
                <Avatar 
                  :label="data.customer.charAt(0)" 
                  shape="circle"
                  size="large"
                  class="customer-thumb"
                />
                <span class="customer-name">{{ data.customer }}</span>
              </div>
            </template>
          </Column>
          
          <!-- Date Column -->
          <Column 
            field="date" 
            :header="$t('date')" 
            sortable
            class="date-column"
          >
            <template #body="{ data }">
              <span>{{ formatDate(data.date) }}</span>
            </template>
          </Column>
          
          <!-- Total Column -->
          <Column 
            field="total" 
            :header="$t('total')" 
            sortable
            class="total-column"
          >
            <template #body="{ data }">
              <span>{{ formatCurrency(data.total) }}</span>
            </template>
          </Column>
          
          <!-- Status Column -->
          <Column 
            field="status" 
            :header="$t('status')" 
            sortable
            class="status-column"
          >
            <template #body="{ data }">
              <Tag 
                :value="data.status" 
                :severity="getStatusSeverity(data.status)"
                :icon="getStatusIcon(data.status)"
                class="status-tag"
              />
            </template>
          </Column>
          
          <!-- Actions Column -->
          <Column 
            :header="$t('actions')" 
            class="actions-column"
            :exportable="false"
          >
            <template #body="{ data }">
              <div class="action-buttons">
                <Button 
                  icon="pi pi-eye" 
                  severity="info"
                  text
                  rounded
                  class="action-button"
                  @click="viewOrder(data)"
                  v-tooltip.top="$t('view')"
                />
                <Button 
                  icon="pi pi-pencil" 
                  severity="success"
                  text
                  rounded
                  class="action-button"
                  @click="editOrder(data)"
                  v-tooltip.top="$t('edit')"
                />
                <Button 
                  icon="pi pi-trash" 
                  severity="danger"
                  text
                  rounded
                  class="action-button"
                  @click="confirmDeleteOrder(data)"
                  v-tooltip.top="$t('delete')"
                />
              </div>
            </template>
          </Column>
        </DataTable>
        
        <!-- Empty State -->
        <div v-if="!loading && !filteredOrdersCount" class="empty-state">
          <div class="empty-content">
            <i class="pi pi-shopping-cart empty-icon"></i>
            <h3 class="empty-title">{{ $t('noOrdersFound') }}</h3>
            <p class="empty-message">
              {{ hasActiveFilters ? $t('tryAdjustingFilters') : $t('getStartedByAddingOrder') }}
            </p>
            <Button 
              v-if="!hasActiveFilters"
              :label="$t('addYourFirstOrder')" 
              icon="pi pi-plus" 
              class="empty-action"
              @click="openOrderDialog"
            />
            <Button 
              v-else
              :label="$t('clearFilters')" 
              icon="pi pi-filter-slash" 
              outlined
              class="empty-action"
              @click="clearFilters"
            />
          </div>
        </div>
      </template>
    </Card>

    <!-- Bulk Actions Bar -->
    <div v-if="selectedOrders.length > 0" class="bulk-actions-bar">
      <div class="bulk-info">
        <span class="selection-count">
          {{ selectedOrders.length }} {{ $t('itemsSelected') }}
        </span>
      </div>
      <div class="bulk-actions">
        <Button 
          :label="$t('bulkEdit')" 
          icon="pi pi-pencil" 
          severity="secondary"
          outlined
          @click="bulkEditOrders"
        />
        <Button 
          :label="$t('bulkExport')" 
          icon="pi pi-download" 
          severity="secondary"
          outlined
          @click="bulkExportOrders"
        />
        <Button 
          :label="$t('bulkDelete')" 
          icon="pi pi-trash" 
          severity="danger"
          @click="confirmBulkDelete"
        />
      </div>
    </div>

    <!-- Order Form Dialog -->
    <Dialog 
      v-model:visible="showOrderDialog" 
      :header="dialogTitle"
      :style="{ width: '90vw', maxWidth: '800px' }" 
      :maximizable="true" 
      :modal="true"
      class="order-dialog"
      @hide="onDialogHide"
    >
      <OrderForm 
        :order="selectedOrder" 
        :mode="formMode"
        :loading="saveLoading"
        @save="saveOrder" 
        @cancel="closeOrderDialog"
      />
    </Dialog>

    <!-- Delete Confirmation Dialog -->
    <ConfirmDialog />
    
    <!-- Toast Messages -->
    <Toast />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { useToast } from 'primevue/usetoast'
import { useConfirm } from 'primevue/useconfirm'
import { useOrderStore } from '@/stores/orderStore'
import { useCustomerStore } from '@/stores/customerStore'

// Components
import Card from 'primevue/card'
import Button from 'primevue/button'
import SplitButton from 'primevue/splitbutton'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dialog from 'primevue/dialog'
import Dropdown from 'primevue/dropdown'
import InputText from 'primevue/inputtext'
import Calendar from 'primevue/calendar'
import AutoComplete from 'primevue/autocomplete'
import Tag from 'primevue/tag'
import Chip from 'primevue/chip'
import Avatar from 'primevue/avatar'
import Toast from 'primevue/toast'
import ConfirmDialog from 'primevue/confirmdialog'
import OrderForm from '@/components/OrderForm.vue'

// Composables
const { t } = useI18n()
const toast = useToast()
const confirm = useConfirm()
const orderStore = useOrderStore()
const customerStore = useCustomerStore()

// Reactive data
const loading = ref(false)
const saveLoading = ref(false)
const exportLoading = ref(false)
const showOrderDialog = ref(false)
const selectedOrder = ref(null)
const selectedOrders = ref([])
const formMode = ref('add')
const customerSuggestions = ref([])

// Filters
const filters = ref({
  status: null,
  customer: null,
  dateRange: null,
  search: ''
})

// Pagination
const pagination = ref({
  first: 0,
  rows: 10
})

// Options
const statusOptions = [
  { label: t('pending'), value: 'Pending' },
  { label: t('fulfilled'), value: 'Fulfilled' },
  { label: t('cancelled'), value: 'Cancelled' }
]

const moreActionsItems = [
  {
    label: t('bulkUpdate'),
    icon: 'pi pi-refresh',
    command: () => bulkEditOrders()
  },
  {
    label: t('exportAll'),
    icon: 'pi pi-file-export',
    command: () => exportAllOrders()
  },
  {
    separator: true
  },
  {
    label: t('settings'),
    icon: 'pi pi-cog',
    command: () => openSettings()
  }
]

// Computed properties
const filteredOrders = computed(() => {
  let orders = orderStore.orders

  if (filters.value.status) {
    orders = orders.filter(o => o.status === filters.value.status)
  }

  if (filters.value.customer) {
    orders = orders.filter(o => o.customer.toLowerCase().includes(filters.value.customer.name.toLowerCase()))
  }

  if (filters.value.dateRange && filters.value.dateRange[0]) {
    const startDate = new Date(filters.value.dateRange[0]).getTime()
    const endDate = filters.value.dateRange[1] ? new Date(filters.value.dateRange[1]).getTime() : Date.now()
    orders = orders.filter(o => {
      const orderDate = new Date(o.date).getTime()
      return orderDate >= startDate && orderDate <= endDate
    })
  }

  if (filters.value.search) {
    const searchTerm = filters.value.search.toLowerCase()
    orders = orders.filter(o => 
      o.orderNumber.toLowerCase().includes(searchTerm) ||
      o.customer.toLowerCase().includes(searchTerm) ||
      o.status.toLowerCase().includes(searchTerm)
    )
  }

  return orders
})

const filteredOrdersCount = computed(() => filteredOrders.value.length)

const paginatedOrders = computed(() => {
  const start = pagination.value.first
  const end = start + pagination.value.rows
  return filteredOrders.value.slice(start, end)
})

const hasActiveFilters = computed(() => {
  return !!(filters.value.status || 
           filters.value.customer || 
           filters.value.dateRange || 
           filters.value.search)
})

const dialogTitle = computed(() => {
  return formMode.value === 'add' ? t('addOrder') : t('editOrder')
})

// Methods
const formatCurrency = (value) => {
  return new Intl.NumberFormat('fr-TN', { style: 'currency', currency: 'TND' }).format(value || 0)
}

const formatDate = (date) => {
  if (!date) return 'N/A'
  return new Intl.DateTimeFormat('fr-TN', {
    dateStyle: 'medium'
  }).format(new Date(date))
}

const getStatusSeverity = (status) => {
  const severityMap = {
    'Pending': 'warning',
    'Fulfilled': 'success',
    'Cancelled': 'danger'
  }
  return severityMap[status] || 'info'
}

const getStatusIcon = (status) => {
  const iconMap = {
    'Pending': 'pi pi-clock',
    'Fulfilled': 'pi pi-check-circle',
    'Cancelled': 'pi pi-times-circle'
  }
  return iconMap[status] || 'pi pi-info-circle'
}

const searchCustomers = (event) => {
  const query = event.query.toLowerCase()
  customerSuggestions.value = customerStore.customers.filter(c => 
    c.name.toLowerCase().includes(query) || c.email.toLowerCase().includes(query)
  )
}

const clearFilters = () => {
  filters.value = {
    status: null,
    customer: null,
    dateRange: null,
    search: ''
  }
}

const onPageChange = (event) => {
  pagination.value.first = event.first
  pagination.value.rows = event.rows
}

const openOrderDialog = () => {
  // selectedOrder.value = null
  selectedOrder.value = {}
  formMode.value = 'add'
  showOrderDialog.value = true
}

const editOrder = (order) => {
  selectedOrder.value = { ...order }
  formMode.value = 'edit'
  showOrderDialog.value = true
}

const viewOrder = (order) => {
  toast.add({
    severity: 'info',
    summary: t('orderView'),
    detail: t('viewingOrder', { number: order.orderNumber }),
    life: 3000
  })
}

const confirmDeleteOrder = (order) => {
  confirm.require({
    message: t('confirmDeleteOrder', { number: order.orderNumber }),
    header: t('deleteConfirmation'),
    icon: 'pi pi-exclamation-triangle',
    rejectClass: 'p-button-secondary p-button-outlined',
    rejectLabel: t('cancel'),
    acceptLabel: t('delete'),
    acceptClass: 'p-button-danger',
    accept: () => deleteOrder(order.id)
  })
}

const deleteOrder = async (id) => {
  try {
    await orderStore.deleteOrder(id)
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('orderDeleted'),
      life: 3000
    })
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: t('error'),
      detail: t('failedToDelete'),
      life: 5000
    })
  }
}

const saveOrder = async (order) => {
  saveLoading.value = true
  try {
    if (formMode.value === 'add') {
      await orderStore.addOrder(order)
      toast.add({
        severity: 'success',
        summary: t('success'),
        detail: t('orderAdded'),
        life: 3000
      })
    } else {
      await orderStore.updateOrder(order)
      toast.add({
        severity: 'success',
        summary: t('success'),
        detail: t('orderUpdated'),
        life: 3000
      })
    }
    closeOrderDialog()
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

const closeOrderDialog = () => {
  showOrderDialog.value = false
  selectedOrder.value = null
  formMode.value = 'add'
}

const onDialogHide = () => {
  closeOrderDialog()
}

const exportOrders = async () => {
  exportLoading.value = true
  try {
    await orderStore.exportOrders()
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('ordersExported'),
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

const importOrders = () => {
  toast.add({
    severity: 'info',
    summary: t('comingSoon'),
    detail: t('importFeatureComingSoon'),
    life: 3000
  })
}

const bulkEditOrders = () => {
  if (selectedOrders.value.length === 0) {
    toast.add({
      severity: 'warn',
      summary: t('warning'),
      detail: t('selectOrdersFirst'),
      life: 3000
    })
    return
  }
  
  toast.add({
    severity: 'info',
    summary: t('bulkEdit'),
    detail: t('bulkEditFeatureComingSoon'),
    life: 3000
  })
}

const bulkExportOrders = async () => {
  if (selectedOrders.value.length === 0) {
    toast.add({
      severity: 'warn',
      summary: t('warning'),
      detail: t('selectOrdersFirst'),
      life: 3000
    })
    return
  }
  
  try {
    await orderStore.exportSelectedOrders(selectedOrders.value)
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('selectedOrdersExported'),
      life: 3000
    })
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: t('error'),
      detail: t('failedToExport'),
      life: 5000
    })
  }
}

const confirmBulkDelete = () => {
  if (selectedOrders.value.length === 0) {
    toast.add({
      severity: 'warn',
      summary: t('warning'),
      detail: t('selectOrdersFirst'),
      life: 3000
    })
    return
  }
  
  confirm.require({
    message: t('confirmBulkDelete', { count: selectedOrders.value.length }),
    header: t('bulkDeleteConfirmation'),
    icon: 'pi pi-exclamation-triangle',
    rejectClass: 'p-button-secondary p-button-outlined',
    rejectLabel: t('cancel'),
    acceptLabel: t('delete'),
    acceptClass: 'p-button-danger',
    accept: () => bulkDeleteOrders()
  })
}

const bulkDeleteOrders = async () => {
  try {
    const ids = selectedOrders.value.map(o => o.id)
    await orderStore.bulkDeleteOrders(ids)
    selectedOrders.value = []
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('ordersDeleted'),
      life: 3000
    })
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: t('error'),
      detail: t('failedToBulkDelete'),
      life: 5000
    })
  }
}

const exportAllOrders = async () => {
  try {
    await orderStore.exportAllOrders()
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('allOrdersExported'),
      life: 3000
    })
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: t('error'),
      detail: t('failedToExportAll'),
      life: 5000
    })
  }
}

const openSettings = () => {
  toast.add({
    severity: 'info',
    summary: t('settings'),
    detail: t('settingsFeatureComingSoon'),
    life: 3000
  })
}

// Lifecycle
onMounted(async () => {
  loading.value = true
  try {
    await orderStore.fetchOrders()
    await customerStore.fetchCustomers()
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: t('error'),
      detail: t('failedToLoadOrders'),
      life: 5000
    })
  } finally {
    loading.value = false
  }
})

// Watchers
watch(filters, () => {
  pagination.value.first = 0
}, { deep: true })
</script>

<style scoped>
.orders-container {
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

/* Filters Styles */
.filters-card {
  margin-bottom: 1.5rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  border-radius: 12px;
}

.filters-container {
  padding: 1rem;
}

.filters-row {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
  margin-bottom: 1rem;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.filter-label {
  font-size: 0.875rem;
  font-weight: 600;
  color: #374151;
}

.filter-dropdown,
.filter-input {
  width: 100%;
}

.search-input {
  padding-left: 2.5rem;
}

.filters-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-top: 1rem;
  border-top: 1px solid #e5e7eb;
}

/* align input fiel on the same line than other input */
.filters-row .p-input-icon-left {
  width: 100%;
  display: flex;
  align-items: center;
}

.results-chip {
  background: #eff6ff;
  color: #1d4ed8;
}

/* Table Styles */
.orders-table-card {
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  border-radius: 12px;
  overflow: hidden;
}

:deep(.orders-table) {
  .p-datatable-header {
    background: #f9fafb;
    border-bottom: 2px solid #e5e7eb;
  }
  
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
    
    &.p-highlight {
      background: #eff6ff;
    }
  }
  
  .p-paginator {
    background: #f9fafb;
    border-top: 1px solid #e5e7eb;
    padding: 1rem;
  }
}

/* Cell Styles */
.order-number {
  font-weight: 500;
  color: #3b82f6;
}

.customer-cell {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.customer-thumb {
  background: #e5e7eb;
  color: #374151;
}

.customer-name {
  font-weight: 500;
  color: #1f2937;
}

.status-tag {
  min-width: 80px;
  justify-content: center;
}

.action-buttons {
  display: flex;
  gap: 0.5rem;
  justify-content: center;
}

.action-button {
  width: 2rem;
  height: 2rem;
  
  &:hover {
    background: #f3f4f6 !important;
  }
}

/* Empty State */
.empty-state {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 300px;
  padding: 2rem;
  background: #f9fafb;
  border-radius: 0 0 12px 12px;
}

.empty-content {
  text-align: center;
  max-width: 400px;
}

.empty-icon {
  font-size: 3rem;
  color: #9ca3af;
  margin-bottom: 1rem;
}

.empty-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #1f2937;
  margin: 0 0 0.5rem 0;
}

.empty-message {
  color: #6b7280;
  margin-bottom: 1.5rem;
}

.empty-action {
  min-width: 200px;
}

/* Bulk Actions */
.bulk-actions-bar {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  background: white;
  padding: 1rem 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  box-shadow: 0 -4px 6px -1px rgba(0, 0, 0, 0.1);
  z-index: 100;
  border-top: 1px solid #e5e7eb;
}

.bulk-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.selection-count {
  font-weight: 600;
  color: #1f2937;
}

.bulk-actions {
  display: flex;
  gap: 0.75rem;
}

/* Dialog Styles */
.order-dialog {
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
  .orders-container {
    padding: 1rem;
  }
  
  .filters-row {
    grid-template-columns: 1fr 1fr;
  }
  
  :deep(.orders-table) {
    .p-datatable-thead > tr > th,
    .p-datatable-tbody > tr > td {
      padding: 0.5rem;
    }
  }
  
  .customer-cell {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.5rem;
  }
}

@media (max-width: 576px) {
  .filters-row {
    grid-template-columns: 1fr;
  }
  
  .bulk-actions {
    flex-wrap: wrap;
    
    & > button {
      flex: 1;
      min-width: 100%;
    }
  }
}
</style>