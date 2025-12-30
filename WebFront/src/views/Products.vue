<template>
  <div class="products-container">
    <!-- Header Section -->
    <div class="page-header">
      <div class="header-content">
        <div class="title-section">
          <h1 class="page-title">{{ $t('products') }}</h1>
          <p class="page-subtitle">{{ $t('manageYourProductCatalog') }}</p>
        </div>
        <div class="header-actions">
          <Button 
            :label="$t('export')" 
            icon="pi pi-download" 
            severity="secondary" 
            outlined 
            class="action-btn"
            @click="exportProducts"
            :loading="exportLoading"
          />
          <Button 
            :label="$t('import')" 
            icon="pi pi-upload" 
            severity="secondary" 
            outlined 
            class="action-btn"
            @click="importProducts"
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
            :label="$t('addProduct')" 
            icon="pi pi-plus" 
            class="primary-btn"
            @click="openProductDialog"
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
              <label class="filter-label">{{ $t('category') }}</label>
              <Dropdown 
                v-model="filters.category" 
                :options="categoryOptions" 
                optionLabel="label" 
                optionValue="value" 
                :placeholder="$t('allCategories')"
                class="filter-dropdown"
                showClear
              />
            </div>
            <div class="filter-group">
              <label class="filter-label">{{ $t('inventory') }}</label>
              <InputText 
                v-model="filters.stock" 
                :placeholder="$t('searchByStock')"
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
                  :placeholder="$t('searchProducts')"
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
              v-if="filteredProductsCount > 0"
              :label="`${filteredProductsCount} ${$t('productsFound')}`"
              class="results-chip"
            />
          </div>
        </div>
      </template>
    </Card>

    <!-- Products Table -->
    <Card class="products-table-card">
      <template #content>
        <DataTable
          v-model:selection="selectedProducts"
          :value="paginatedProducts"
          :loading="loading"
          :totalRecords="filteredProductsCount"
          :rows="pagination.rows"
          :first="pagination.first"
          paginator
          :rowsPerPageOptions="[10, 25, 50]"
          responsiveLayout="scroll"
          editMode="row"
          dataKey="id"
          class="products-table"
          @page="onPageChange"
          @row-edit-complete="onRowEditComplete"
          :globalFilterFields="['name', 'category', 'status']"
        >
          <!-- Selection Column -->
          <Column selectionMode="multiple" headerStyle="width: 3rem" />
          
          <!-- Product Column -->
          <Column 
            field="name" 
            :header="$t('product')" 
            sortable
            class="product-column"
          >
            <template #body="{ data }">
              <div class="product-cell">
                <div class="product-image">
                  <Image 
                    :src="data.image || '/api/placeholder/40/40'" 
                    :alt="data.name"
                    width="40"
                    height="40"
                    preview
                    class="product-thumb"
                  />
                </div>
                <div class="product-info">
                  <span class="product-name">{{ data.name }}</span>
                  <small class="product-sku">SKU: {{ data.sku || 'N/A' }}</small>
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
          
          <!-- Inventory Column -->
          <Column 
            field="inventory" 
            :header="$t('inventory')" 
            sortable
            class="inventory-column"
          >
            <template #body="{ data }">
              <div class="inventory-cell">
                <span class="inventory-count">{{ formatNumber(data.inventory) }}</span>
                <small class="inventory-label">{{ $t('inStock') }}</small>
                <ProgressBar 
                  :value="getStockLevel(data.inventory)" 
                  :class="getStockLevelClass(data.inventory)"
                  class="stock-progress"
                />
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
          
          <!-- Category Column -->
          <Column 
            field="category" 
            :header="$t('category')" 
            sortable
            class="category-column"
          >
            <template #body="{ data }">
              <Chip 
                :label="data.category" 
                class="category-chip"
              />
            </template>
          </Column>
          
          <!-- Price Column -->
          <Column 
            field="price" 
            :header="$t('price')" 
            sortable
            class="price-column"
          >
            <template #body="{ data }">
              <div class="price-cell">
                <span class="current-price">{{ formatCurrency(data.price) }}</span>
                <small v-if="data.compareAtPrice" class="compare-price">
                  {{ formatCurrency(data.compareAtPrice) }}
                </small>
              </div>
            </template>
          </Column>
          
          <!-- Actions Column -->
          <Column 
            :header="$t('actions')" 
            class="actions-column"
            :exportable="false"
          >
            <template #body="{ data, index }">
              <div class="action-buttons">
                <Button 
                  icon="pi pi-eye" 
                  severity="info"
                  text
                  rounded
                  class="action-button"
                  @click="viewProduct(data)"
                  v-tooltip.top="$t('view')"
                />
                <Button 
                  icon="pi pi-pencil" 
                  severity="success"
                  text
                  rounded
                  class="action-button"
                  @click="editProduct(data)"
                  v-tooltip.top="$t('edit')"
                />
                <Button 
                  icon="pi pi-copy" 
                  severity="secondary"
                  text
                  rounded
                  class="action-button"
                  @click="duplicateProduct(data)"
                  v-tooltip.top="$t('duplicate')"
                />
                <Button 
                  icon="pi pi-trash" 
                  severity="danger"
                  text
                  rounded
                  class="action-button"
                  @click="confirmDeleteProduct(data)"
                  v-tooltip.top="$t('delete')"
                />
              </div>
            </template>
          </Column>
          
          <!-- Row Editor Column -->
          <Column :rowEditor="true" style="width:10%; min-width:8rem" bodyStyle="text-align:center" />
        </DataTable>
        
        <!-- Empty State -->
        <div v-if="!loading && !filteredProductsCount" class="empty-state">
          <div class="empty-content">
            <i class="pi pi-box empty-icon"></i>
            <h3 class="empty-title">{{ $t('noProductsFound') }}</h3>
            <p class="empty-message">
              {{ hasActiveFilters ? $t('tryAdjustingFilters') : $t('getStartedByAdding') }}
            </p>
            <Button 
              v-if="!hasActiveFilters"
              :label="$t('addYourFirstProduct')" 
              icon="pi pi-plus" 
              class="empty-action"
              @click="openProductDialog"
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
    <div v-if="selectedProducts.length > 0" class="bulk-actions-bar">
      <div class="bulk-info">
        <span class="selection-count">
          {{ selectedProducts.length }} {{ $t('itemsSelected') }}
        </span>
      </div>
      <div class="bulk-actions">
        <Button 
          :label="$t('bulkEdit')" 
          icon="pi pi-pencil" 
          severity="secondary"
          outlined
          @click="bulkEditProducts"
        />
        <Button 
          :label="$t('bulkExport')" 
          icon="pi pi-download" 
          severity="secondary"
          outlined
          @click="bulkExportProducts"
        />
        <Button 
          :label="$t('bulkDelete')" 
          icon="pi pi-trash" 
          severity="danger"
          @click="confirmBulkDelete"
        />
      </div>
    </div>

    <!-- Product Form Dialog -->
    <Dialog 
      v-model:visible="showProductDialog" 
      :header="dialogTitle"
      :style="{ width: '90vw', maxWidth: '1200px' }" 
      :maximizable="true" 
      :modal="true"
      class="product-dialog"
      @hide="onDialogHide"
    >
      <!-- <ProductForm 
        v-if="showProductDialog" 
        :product="selectedProduct" 
        :mode="formMode"
        :loading="saveLoading"
        @save="saveProduct" 
        @cancel="closeProductDialog"
      /> -->

      <ProductForm 
      v-if="showProductDialog" 
      :product="selectedProduct" 
      :mode="formMode"
      :loading="saveLoading"
      @save="saveProduct" 
      @cancel="closeProductDialog"
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
import { useProductStore } from '@/stores/productStore'
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
import Image from 'primevue/image'
import ProgressBar from 'primevue/progressbar'
import Toast from 'primevue/toast'
import ConfirmDialog from 'primevue/confirmdialog'
import ProductForm from '@/components/ProductForm.vue'

// Composables
const { t } = useI18n()
const toast = useToast()
const confirm = useConfirm()
const productStore = useProductStore()
const notificationStore = useNotificationStore()

// Reactive data
const loading = ref(false)
const saveLoading = ref(false)
const exportLoading = ref(false)
const showProductDialog = ref(false)
const selectedProduct = ref(null)
const selectedProducts = ref([])
const formMode = ref('add')

// Filters
const filters = ref({
  status: null,
  category: null,
  stock: '',
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
  { label: t('draft'), value: 'Draft' },
  { label: t('archived'), value: 'Archived' }
]

const categoryOptions = [
  { label: t('books'), value: 'Books' },
  { label: t('electronics'), value: 'Electronics' },
  { label: t('clothing'), value: 'Clothing' },
  { label: t('home'), value: 'Home' }
]

const moreActionsItems = [
  {
    label: t('bulkUpdate'),
    icon: 'pi pi-refresh',
    command: () => bulkEditProducts()
  },
  {
    label: t('exportAll'),
    icon: 'pi pi-file-export',
    command: () => exportAllProducts()
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
const filteredProducts = computed(() => {
  let products = productStore.products

  if (filters.value.status) {
    products = products.filter(p => p.status === filters.value.status)
  }

  if (filters.value.category) {
    products = products.filter(p => p.category === filters.value.category)
  }

  if (filters.value.stock) {
    products = products.filter(p => 
      p.inventory.toString().includes(filters.value.stock)
    )
  }

  if (filters.value.search) {
    const searchTerm = filters.value.search.toLowerCase()
    products = products.filter(p => 
      p.name.toLowerCase().includes(searchTerm) ||
      p.sku?.toLowerCase().includes(searchTerm) ||
      p.category.toLowerCase().includes(searchTerm)
    )
  }

  return products
})

const filteredProductsCount = computed(() => filteredProducts.value.length)

const paginatedProducts = computed(() => {
  const start = pagination.value.first
  const end = start + pagination.value.rows
  return filteredProducts.value.slice(start, end)
})

const hasActiveFilters = computed(() => {
  return !!(filters.value.status || 
           filters.value.category || 
           filters.value.stock || 
           filters.value.search)
})

const dialogTitle = computed(() => {
  return formMode.value === 'add' ? t('addProduct') : t('editProduct')
})

// Methods
const formatCurrency = (value) => {
  return new Intl.NumberFormat('fr-TN', {
    style: 'currency',
    currency: 'TND'
  }).format(value || 0)
}

const formatNumber = (value) => {
  return new Intl.NumberFormat('fr-TN').format(value || 0)
}

const getStatusSeverity = (status) => {
  const severityMap = {
    'Active': 'success',
    'Draft': 'warning',
    'Archived': 'secondary'
  }
  return severityMap[status] || 'info'
}

const getStatusIcon = (status) => {
  const iconMap = {
    'Active': 'pi pi-check-circle',
    'Draft': 'pi pi-clock',
    'Archived': 'pi pi-archive'
  }
  return iconMap[status] || 'pi pi-info-circle'
}

const getStockLevel = (inventory) => {
  if (inventory <= 0) return 0
  if (inventory <= 10) return 25
  if (inventory <= 50) return 50
  if (inventory <= 100) return 75
  return 100
}

const getStockLevelClass = (inventory) => {
  if (inventory <= 0) return 'stock-empty'
  if (inventory <= 10) return 'stock-low'
  if (inventory <= 50) return 'stock-medium'
  return 'stock-good'
}

const clearFilters = () => {
  filters.value = {
    status: null,
    category: null,
    stock: '',
    search: ''
  }
}

const onPageChange = (event) => {
  pagination.value.first = event.first
  pagination.value.rows = event.rows
}

const openProductDialog = () => {
  // selectedProduct.value = null
  selectedProduct.value = {}
  formMode.value = 'add'
  showProductDialog.value = true
}


const editProduct = (product) => {
  selectedProduct.value = { ...product }
  formMode.value = 'edit'
  showProductDialog.value = true
}

const viewProduct = (product) => {
  // Implement product view logic
  toast.add({
    severity: 'info',
    summary: t('productView'),
    detail: t('viewingProduct', { name: product.name }),
    life: 3000
  })
}

const duplicateProduct = async (product) => {
  try {
    const duplicated = {
      ...product,
      id: Date.now(),
      name: `${product.name} (${t('copy')})`,
      sku: `${product.sku || 'SKU'}-COPY-${Date.now()}`
    }
    
    await productStore.addProduct(duplicated)
    
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('productDuplicated'),
      life: 3000
    })
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: t('error'),
      detail: t('failedToDuplicate'),
      life: 5000
    })
  }
}

const confirmDeleteProduct = (product) => {
  confirm.require({
    message: t('confirmDeleteProduct', { name: product.name }),
    header: t('deleteConfirmation'),
    icon: 'pi pi-exclamation-triangle',
    rejectClass: 'p-button-secondary p-button-outlined',
    rejectLabel: t('cancel'),
    acceptLabel: t('delete'),
    acceptClass: 'p-button-danger',
    accept: () => deleteProduct(product.id)
  })
}

const deleteProduct = async (id) => {
  try {
    await productStore.deleteProduct(id)
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('productDeleted'),
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
    await productStore.updateProduct(event.data)
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('productUpdated'),
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

const saveProduct = async (product) => {
  saveLoading.value = true
  // alert("adda")
  try {
    if (formMode.value === 'add') {
      await productStore.addProduct(product)
      toast.add({
        severity: 'success',
        summary: t('success'),
        detail: t('productAdded'),
        life: 3000
      })
    } else {
      await productStore.updateProduct(product)
      toast.add({
        severity: 'success',
        summary: t('success'),
        detail: t('productUpdated'),
        life: 3000
      })
    }
    closeProductDialog()
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

//
// const saveProduct = async (product) => {
//   saveLoading.value = true
//   try {
//     if (formMode.value === 'add') {
//       // **MODIFICATION ICI** - Utiliser la nouvelle méthode du store
//       const newProduct = await productStore.addProduct(product)
//       toast.add({
//         severity: 'success',
//         summary: t('success'),
//         detail: t('productAdded'),
//         life: 3000
//       })
//       console.log('🎉 Nouveau produit ajouté:', newProduct)
//     } else {
//       // **MODIFICATION ICI** - Utiliser la méthode de mise à jour
//       const updatedProduct = await productStore.updateProduct(product)
//       toast.add({
//         severity: 'success',
//         summary: t('success'),
//         detail: t('productUpdated'),
//         life: 3000
//       })
//       console.log('🎉 Produit mis à jour:', updatedProduct)
//     }
//     closeProductDialog()
//   } catch (error) {
//     console.error('❌ Erreur lors de la sauvegarde:', error)
//     toast.add({
//       severity: 'error',
//       summary: t('error'),
//       detail: t('failedToSave'),
//       life: 5000
//     })
//   } finally {
//     saveLoading.value = false
//   }
// }

// nouvelle methode 

const closeProductDialog = () => {
  showProductDialog.value = false
  selectedProduct.value = null
  formMode.value = 'add'
}

const onDialogHide = () => {
  closeProductDialog()
}

const exportProducts = async () => {
  exportLoading.value = true
  try {
    await productStore.exportProducts()
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('productsExported'),
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

const importProducts = () => {
  // Implement import logic
  toast.add({
    severity: 'info',
    summary: t('comingSoon'),
    detail: t('importFeatureComingSoon'),
    life: 3000
  })
}

const bulkEditProducts = () => {
  if (selectedProducts.value.length === 0) {
    toast.add({
      severity: 'warn',
      summary: t('warning'),
      detail: t('selectProductsFirst'),
      life: 3000
    })
    return
  }
  
  // Implement bulk edit logic
  toast.add({
    severity: 'info',
    summary: t('bulkEdit'),
    detail: t('bulkEditFeatureComingSoon'),
    life: 3000
  })
}

const bulkExportProducts = async () => {
  if (selectedProducts.value.length === 0) {
    toast.add({
      severity: 'warn',
      summary: t('warning'),
      detail: t('selectProductsFirst'),
      life: 3000
    })
    return
  }
  
  try {
    await productStore.exportSelectedProducts(selectedProducts.value)
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('selectedProductsExported'),
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
  if (selectedProducts.value.length === 0) {
    toast.add({
      severity: 'warn',
      summary: t('warning'),
      detail: t('selectProductsFirst'),
      life: 3000
    })
    return
  }
  
  confirm.require({
    message: t('confirmBulkDelete', { count: selectedProducts.value.length }),
    header: t('bulkDeleteConfirmation'),
    icon: 'pi pi-exclamation-triangle',
    rejectClass: 'p-button-secondary p-button-outlined',
    rejectLabel: t('cancel'),
    acceptLabel: t('delete'),
    acceptClass: 'p-button-danger',
    accept: () => bulkDeleteProducts()
  })
}

const bulkDeleteProducts = async () => {
  try {
    const ids = selectedProducts.value.map(p => p.id)
    await productStore.bulkDeleteProducts(ids)
    selectedProducts.value = []
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('productsDeleted'),
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

const exportAllProducts = async () => {
  try {
    await productStore.exportAllProducts()
    toast.add({
      severity: 'success',
      summary: t('success'),
      detail: t('allProductsExported'),
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
    await productStore.fetchProducts()
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: t('error'),
      detail: t('failedToLoadProducts'),
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
.products-container {
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
.products-table-card {
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  border-radius: 12px;
  overflow: hidden;
}

:deep(.products-table) {
  .p-datatable-header {
    background: #f9fafb;
    border-bottom: 2px solid #e5e7eb;
  }
  
  .p-datatable-thead >tr > th {
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
.product-cell {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.product-image {
  flex-shrink: 0;
}

.product-thumb {
  border-radius: 8px;
  object-fit: cover;
  border: 1px solid #e5e7eb;
}

.product-info {
  display: flex;
  flex-direction: column;
}

.product-name {
  font-weight: 500;
  color: #1f2937;
}

.product-sku {
  color: #6b7280;
  font-size: 0.75rem;
}

.status-tag {
  min-width: 80px;
  justify-content: center;
}

.inventory-cell {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.inventory-count {
  font-weight: 500;
  color: #1f2937;
}

.inventory-label {
  color: #6b7280;
  font-size: 0.75rem;
}

.stock-progress {
  height: 4px;
  border-radius: 2px;
  
  &:deep(.p-progressbar-value) {
    transition: width 0.5s ease;
  }
  
  &.stock-empty {
    &:deep(.p-progressbar-value) {
      background-color: #ef4444;
    }
  }
  
  &.stock-low {
    &:deep(.p-progressbar-value) {
      background-color: #f59e0b;
    }
  }
  
  &.stock-medium {
    &:deep(.p-progressbar-value) {
      background-color: #3b82f6;
    }
  }
  
  &.stock-good {
    &:deep(.p-progressbar-value) {
      background-color: #10b981;
    }
  }
}

.category-chip {
  background: #e0e7ff;
  color: #4f46e5;
}

.price-cell {
  display: flex;
  flex-direction: column;
}

.current-price {
  font-weight: 600;
  color: #1f2937;
}

.compare-price {
  text-decoration: line-through;
  color: #9ca3af;
  font-size: 0.875rem;
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
.product-dialog {
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
  .products-container {
    padding: 1rem;
  }
  
  .filters-row {
    grid-template-columns: 1fr 1fr;
  }
  
  :deep(.products-table) {
    .p-datatable-thead > tr > th,
    .p-datatable-tbody > tr > td {
      padding: 0.5rem;
    }
  }
  
  .product-cell {
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
