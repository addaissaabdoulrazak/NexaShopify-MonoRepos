<template>
  <div class="customers-container">
    <!-- Header Section -->
    <div class="page-header">
      <div class="header-content">
        <div class="title-section">
          <h1 class="page-title">{{ $t('customers') }}</h1>
          <p class="page-subtitle">{{ $t('manageYourCustomerBase') }}</p>
        </div>
        <div class="header-actions">
          <Button 
            :label="$t('export')" 
            icon="pi pi-download" 
            severity="secondary" 
            outlined 
            class="action-btn"
            @click="exportCustomers"
            :loading="exportLoading"
          />
          <Button 
            :label="$t('import')" 
            icon="pi pi-upload" 
            severity="secondary" 
            outlined 
            class="action-btn"
            @click="importCustomers"
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
            :label="$t('addCustomer')" 
            icon="pi pi-plus" 
            class="primary-btn"
            @click="openCustomerDialog"
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
              <label class="filter-label">{{ $t('group') }}</label>
              <Dropdown 
                v-model="filters.group" 
                :options="groupOptions" 
                optionLabel="label" 
                optionValue="value" 
                :placeholder="$t('allGroups')"
                class="filter-dropdown"
                showClear
              />
            </div>
            <div class="filter-group">
              <label class="filter-label">{{ $t('orders') }}</label>
              <InputText 
                v-model="filters.orders" 
                :placeholder="$t('searchByOrders')"
                class="filter-input"
                type="number"
              />
            </div>
            <div class="filter-group">
              <label class="filter-label">{{ $t('search') }}</label>
              <span class="p-input-icon-left">
                <i class="pi pi-search" />
                <InputText 
                  v-model="filters.search" 
                  :placeholder="$t('searchCustomers')"
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
              v-if="filteredCustomersCount > 0"
              :label="`${filteredCustomersCount} ${$t('customersFound')}`"
              class="results-chip"
            />
          </div>
        </div>
      </template>
    </Card>

    <!-- Customers Table -->
    <Card class="customers-table-card">
      <template #content>
        <DataTable
          v-model:selection="selectedCustomers"
          :value="paginatedCustomers"
          :loading="loading"
          :totalRecords="filteredCustomersCount"
          :rows="pagination.rows"
          :first="pagination.first"
          paginator
          :rowsPerPageOptions="[10, 25, 50]"
          responsiveLayout="scroll"
          editMode="row"
          dataKey="id"
          class="customers-table"
          @page="onPageChange"
          @row-edit-complete="onRowEditComplete"
          :globalFilterFields="['name', 'email', 'status']"
        >
          <!-- Selection Column -->
          <Column selectionMode="multiple" headerStyle="width: 3rem" />
          
          <!-- Customer Column -->
          <Column 
            field="name" 
            :header="$t('customer')" 
            sortable
            class="customer-column"
          >
            <template #body="{ data }">
              <div class="customer-cell">
                <div class="customer-avatar">
                  <Avatar 
                    :label="data.name.charAt(0)" 
                    shape="circle"
                    size="large"
                    class="customer-thumb"
                  />
                </div>
                <div class="customer-info">
                  <span class="customer-name">{{ data.name }}</span>
                  <small class="customer-email">{{ data.email }}</small>
                </div>
              </div>
            </template>
            <template #editor="{ data, field }">
              <InputText 
                v-model="data[field]" 
                class="w-full"
                autofocus
              />
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
            <template #editor="{ data, field }">
              <Dropdown 
                v-model="data[field]" 
                :options="statusOptions" 
                optionLabel="label" 
                optionValue="value"
                class="w-full"
              />
            </template>
          </Column>
          
          <!-- Orders Column -->
          <Column 
            field="totalOrders" 
            :header="$t('orders')" 
            sortable
            class="orders-column"
          >
            <template #body="{ data }">
              <div class="orders-cell">
                <span class="orders-count">{{ formatNumber(data.totalOrders) }}</span>
                <small class="orders-label">{{ $t('totalOrders') }}</small>
              </div>
            </template>
            <template #editor="{ data, field }">
              <InputNumber 
                v-model="data[field]" 
                class="w-full"
                :min="0"
              />
            </template>
          </Column>
          
          <!-- Group Column -->
          <Column 
            field="group" 
            :header="$t('group')" 
            sortable
            class="group-column"
          >
            <template #body="{ data }">
              <Chip 
                :label="data.group" 
                class="group-chip"
              />
            </template>
          </Column>
          
          <!-- Last Order Column -->
          <Column 
            field="lastOrderDate" 
            :header="$t('lastOrder')" 
            sortable
            class="last-order-column"
          >
            <template #body="{ data }">
              <span>{{ formatDate(data.lastOrderDate) }}</span>
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
                  @click="viewCustomer(data)"
                  v-tooltip.top="$t('view')"
                />
                <Button 
                  icon="pi pi-pencil" 
                  severity="success"
                  text
                  rounded
                  class="action-button"
                  @click="editCustomer(data)"
                  v-tooltip.top="$t('edit')"
                />
                <Button 
                  icon="pi pi-trash" 
                  severity="danger"
                  text
                  rounded
                  class="action-button"
                  @click="confirmDeleteCustomer(data)"
                  v-tooltip.top="$t('delete')"
                />
              </div>
            </template>
          </Column>
          
          <!-- Row Editor Column -->
          <Column :rowEditor="true" style="width:10%; min-width:8rem" bodyStyle="text-align:center" />
        </DataTable>
        
        <!-- Empty State -->
        <div v-if="!loading && !filteredCustomersCount" class="empty-state">
          <div class="empty-content">
            <i class="pi pi-users empty-icon"></i>
            <h3 class="empty-title">{{ $t('noCustomersFound') }}</h3>
            <p class="empty-message">
              {{ hasActiveFilters ? $t('tryAdjustingFilters') : $t('getStartedByAddingCustomer') }}
            </p>
            <Button 
              v-if="!hasActiveFilters"
              :label="$t('addYourFirstCustomer')" 
              icon="pi pi-plus" 
              class="empty-action"
              @click="openCustomerDialog"
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
    <div v-if="selectedCustomers.length > 0" class="bulk-actions-bar">
      <div class="bulk-info">
        <span class="selection-count">
          {{ selectedCustomers.length }} {{ $t('itemsSelected') }}
        </span>
      </div>
      <div class="bulk-actions">
        <Button 
          :label="$t('bulkEdit')" 
          icon="pi pi-pencil" 
          severity="secondary"
          outlined
          @click="bulkEditCustomers"
        />
        <Button 
          :label="$t('bulkExport')" 
          icon="pi pi-download" 
          severity="secondary"
          outlined
          @click="bulkExportCustomers"
        />
        <Button 
          :label="$t('bulkDelete')" 
          icon="pi pi-trash" 
          severity="danger"
          @click="confirmBulkDelete"
        />
      </div>
    </div>

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
import { useCustomerStore } from '@/stores/customerStore'
import { useNotificationStore } from '@/stores/notificationStore'

// Components
import Card from 'primevue/card'
import Button from 'primevue/button'
import SplitButton from 'primevue/splitbutton'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dialog from 'primevue/dialog'
import Dropdown from 'primevue/dropdown'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import Tag from 'primevue/tag'
import Chip from 'primevue/chip'
import Avatar from 'primevue/avatar'
import Toast from 'primevue/toast'
import ConfirmDialog from 'primevue/confirmdialog'
import CustomerForm from '@/components/CustomerForm.vue'

// Composables
const { t } = useI18n()
const toast = useToast()
const confirm = useConfirm()
const customerStore = useCustomerStore()
const notificationStore = useNotificationStore()

// Reactive data
const loading = ref(false)
const saveLoading = ref(false)
const exportLoading = ref(false)
const showCustomerDialog = ref(false)
const selectedCustomer = ref(null)
const selectedCustomers = ref([])
const formMode = ref('add')

// Filters
const filters = ref({
  status: null,
  group: null,
  orders: '',
  search: ''
})

// Pagination
const pagination = ref({
  first: 0,
  rows: 10
})

// Options
const statusOptions = [
  { label: t('active'), value: 'Active' },
  { label: t('inactive'), value: 'Inactive' },
  { label: t('pending'), value: 'Pending' }
]

const groupOptions = [
  { label: t('regular'), value: 'Regular' },
  { label: t('vip'), value: 'VIP' },
  { label: t('wholesale'), value: 'Wholesale' }
]

const moreActionsItems = [
  {
    label: t('bulkUpdate'),
    icon: 'pi pi-refresh',
    command: () => bulkEditCustomers()
  },
  {
    label: t('exportAll'),
    icon: 'pi pi-file-export',
    command: () => exportAllCustomers()
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
const filteredCustomers = computed(() => {
  let customers = customerStore.customers

  if (filters.value.status) {
    customers = customers.filter(c => c.status === filters.value.status)
  }

  if (filters.value.group) {
    customers = customers.filter(c => c.group === filters.value.group)
  }

  if (filters.value.orders) {
    customers = customers.filter(c => 
      c.totalOrders.toString().includes(filters.value.orders)
    )
  }

  if (filters.value.search) {
    const searchTerm = filters.value.search.toLowerCase()
    customers = customers.filter(c => 
      c.name.toLowerCase().includes(searchTerm) ||
      c.email.toLowerCase().includes(searchTerm) ||
      c.group.toLowerCase().includes(searchTerm)
    )
  }

  return customers
})

const filteredCustomersCount = computed(() => filteredCustomers.value.length)

const paginatedCustomers = computed(() => {
  const start = pagination.value.first
  const end = start + pagination.value.rows
  return filteredCustomers.value.slice(start, end)
})

const hasActiveFilters = computed(() => {
  return !!(filters.value.status || 
           filters.value.group || 
           filters.value.orders || 
           filters.value.search)
})

const dialogTitle = computed(() => {
  return formMode.value === 'add' ? t('addCustomer') : t('editCustomer')
})

// Methods
const formatNumber = (value) => {
  return new Intl.NumberFormat('fr-TN').format(value || 0)
}

const formatDate = (date) => {
  if (!date) return 'N/A'
  return new Intl.DateTimeFormat('fr-TN', {
    dateStyle: 'medium'
  }).format(new Date(date))
}

const getStatusSeverity = (status) => {
  const severityMap = {
    'Active': 'success',
    'Inactive': 'secondary',
    'Pending': 'warning'
  }
  return severityMap[status] || 'info'
}

const getStatusIcon = (status) => {
  const iconMap = {
    'Active': 'pi pi-check-circle',
    'Inactive': 'pi pi-times-circle',
    'Pending': 'pi pi-clock'
  }
  return iconMap[status] || 'pi pi-info-circle'
}

const clearFilters = () => {
  filters.value = {
    status: null,
    group: null,
    orders: '',
    search: ''
  }
}

const onPageChange = (event) => {
  pagination.value.first = event.first
  pagination.value.rows = event.rows
}

const openCustomerDialog = () => {
  // selectedCustomer.value = null
  selectedCustomer.value = {}
  formMode.value = 'add'
  showCustomerDialog.value = true
}

const editCustomer = (customer) => {
  selectedCustomer.value = { ...customer }
  formMode.value = 'edit'
  showCustomerDialog.value = true
}

const viewCustomer = (customer) => {
  toast.add({
    severity: 'info',
    summary: t('customerView'),
    detail: t('viewingCustomer', { name: customer.name }),
    life: 3000
  })
}

const confirmDeleteCustomer = (customer) => {
  confirm.require({
    message: t('confirmDeleteCustomer', { name: customer.name }),
    header: t('deleteConfirmation'),
    icon: 'pi pi-exclamation-triangle',
    rejectClass: 'p-button-secondary p-button-outlined',
    rejectLabel: t('cancel'),
    acceptLabel: t('delete'),
    acceptClass: 'p-button-danger',
    accept: () => deleteCustomer(customer.id)
  })
}

const deleteCustomer = async (id) => {
  try {
    await customerStore.deleteCustomer(id)
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('customerDeleted'),
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

const onRowEditComplete = async (event) => {
  try {
    await customerStore.updateCustomer(event.data)
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('customerUpdated'),
      life: 3000
    })
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: t('error'),
      detail: t('failedToUpdate'),
      life: 5000
    })
  }
}

const saveCustomer = async (customer) => {
  saveLoading.value = true
  try {
    if (formMode.value === 'add') {
      await customerStore.addCustomer(customer)
      toast.add({
        severity: 'success',
        summary: t('success'),
        detail: t('customerAdded'),
        life: 3000
      })
    } else {
      await customerStore.updateCustomer(customer)
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

const exportCustomers = async () => {
  exportLoading.value = true
  try {
    await customerStore.exportCustomers()
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('customersExported'),
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

const importCustomers = () => {
  toast.add({
    severity: 'info',
    summary: t('comingSoon'),
    detail: t('importFeatureComingSoon'),
    life: 3000
  })
}

const bulkEditCustomers = () => {
  if (selectedCustomers.value.length === 0) {
    toast.add({
      severity: 'warn',
      summary: t('warning'),
      detail: t('selectCustomersFirst'),
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

const bulkExportCustomers = async () => {
  if (selectedCustomers.value.length === 0) {
    toast.add({
      severity: 'warn',
      summary: t('warning'),
      detail: t('selectCustomersFirst'),
      life: 3000
    })
    return
  }
  
  try {
    await customerStore.exportSelectedCustomers(selectedCustomers.value)
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('selectedCustomersExported'),
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
  if (selectedCustomers.value.length === 0) {
    toast.add({
      severity: 'warn',
      summary: t('warning'),
      detail: t('selectCustomersFirst'),
      life: 3000
    })
    return
  }
  
  confirm.require({
    message: t('confirmBulkDelete', { count: selectedCustomers.value.length }),
    header: t('bulkDeleteConfirmation'),
    icon: 'pi pi-exclamation-triangle',
    rejectClass: 'p-button-secondary p-button-outlined',
    rejectLabel: t('cancel'),
    acceptLabel: t('delete'),
    acceptClass: 'p-button-danger',
    accept: () => bulkDeleteCustomers()
  })
}

const bulkDeleteCustomers = async () => {
  try {
    const ids = selectedCustomers.value.map(c => c.id)
    await customerStore.bulkDeleteCustomers(ids)
    selectedCustomers.value = []
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('customersDeleted'),
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

const exportAllCustomers = async () => {
  try {
    await customerStore.exportAllCustomers()
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('allCustomersExported'),
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
    await customerStore.fetchCustomers()
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: t('error'),
      detail: t('failedToLoadCustomers'),
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
.customers-container {
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
.customers-table-card {
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  border-radius: 12px;
  overflow: hidden;
}

:deep(.customers-table) {
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
.customer-cell {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.customer-avatar {
  flex-shrink: 0;
}

.customer-thumb {
  background: #e5e7eb;
  color: #374151;
}

.customer-info {
  display: flex;
  flex-direction: column;
}

.customer-name {
  font-weight: 500;
  color: #1f2937;
}

.customer-email {
  color: #6b7280;
  font-size: 0.75rem;
}

.status-tag {
  min-width: 80px;
  justify-content: center;
}

.orders-cell {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.orders-count {
  font-weight: 500;
  color: #1f2937;
}

.orders-label {
  color: #6b7280;
  font-size: 0.75rem;
}

.group-chip {
  background: #e0e7ff;
  color: #4f46e5;
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
  .customers-container {
    padding: 1rem;
  }
  
  .filters-row {
    grid-template-columns: 1fr 1fr;
  }
  
  :deep(.customers-table) {
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