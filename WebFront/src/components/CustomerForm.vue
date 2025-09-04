<template>
  <div class="customer-form-container">
    <!-- Form Header -->
    <div class="form-header">
      <div class="header-content">
        <div class="header-info">
          <h2 class="form-title">
            {{ formMode === 'add' ? $t('addNewCustomer') : $t('editCustomer') }}
          </h2>
          <p class="form-subtitle">
            {{ formMode === 'add' ? $t('createNewCustomerDescription') : $t('updateCustomerDescription') }}
          </p>
        </div>
        <div class="header-actions">
          <Button 
            :label="$t('preview')" 
            icon="pi pi-eye" 
            severity="secondary" 
            outlined 
            @click="previewCustomer"
            :disabled="!customer.name"
          />
          <Button 
            :label="$t('saveAsPending')" 
            icon="pi pi-save" 
            severity="secondary" 
            outlined 
            @click="saveAsPending"
            :loading="pendingLoading"
          />
        </div>
      </div>
      <!-- Progress Indicator -->
      <div class="form-progress">
        <div class="progress-bar">
          <div class="progress-fill" :style="{ width: `${formProgress}%` }"></div>
        </div>
        <span class="progress-text">{{ Math.round(formProgress) }}% {{ $t('complete') }}</span>
      </div>
    </div>

    <!-- Main Form Content -->
    <div class="form-content">
      <TabView v-model:activeIndex="activeTab" class="customer-tabs" @tab-change="onTabChange">
        <!-- Basic Information Tab -->
        <TabPanel :header="$t('basicInfo')" class="tab-panel">
          <div class="tab-content">
            <div class="section-header">
              <h3 class="section-title">{{ $t('customerDetails') }}</h3>
              <p class="section-description">{{ $t('basicCustomerInformation') }}</p>
            </div>
            
            <div class="form-grid">
              <!-- Customer Name -->
              <div class="form-field full-width" :class="{ 'field-error': v$.name.$error }">
                <label for="name" class="field-label required">
                  {{ $t('customerName') }}
                  <Tooltip target=".name-help" :value="$t('nameTooltip')" position="top" />
                  <i class="pi pi-question-circle name-help help-icon"></i>
                </label>
                <InputText
                  id="name"
                  v-model="localCustomer.name"
                  :class="{ 'p-invalid': v$.name.$error }"
                  :placeholder="$t('enterCustomerName')"
                  @blur="v$.name.$touch"
                  @input="updateProgress"
                  maxlength="100"
                  class="field-input"
                />
                <div class="field-footer">
                  <small v-if="v$.name.$error" class="error-message">
                    {{ v$.name.$errors[0].$message }}
                  </small>
                  <small class="char-counter">
                    {{ localCustomer.name?.length || 0 }}/100
                  </small>
                </div>
              </div>

              <!-- Email -->
              <div class="form-field" :class="{ 'field-error': v$.email.$error }">
                <label for="email" class="field-label required">
                  {{ $t('email') }}
                  <Tooltip target=".email-help" :value="$t('emailTooltip')" position="top" />
                  <i class="pi pi-question-circle email-help help-icon"></i>
                </label>
                <InputText
                  id="email"
                  v-model="localCustomer.email"
                  :class="{ 'p-invalid': v$.email.$error }"
                  :placeholder="$t('enterEmail')"
                  @blur="v$.email.$touch"
                  @input="updateProgress"
                  class="field-input"
                />
                <small v-if="v$.email.$error" class="error-message">
                  {{ v$.email.$errors[0].$message }}
                </small>
              </div>

              <!-- Phone -->
              <div class="form-field">
                <label for="phone" class="field-label">
                  {{ $t('phone') }}
                  <span class="optional-label">{{ $t('optional') }}</span>
                </label>
                <InputText
                  id="phone"
                  v-model="localCustomer.phone"
                  :placeholder="$t('enterPhone')"
                  @input="updateProgress"
                  class="field-input"
                />
              </div>

              <!-- Status -->
              <div class="form-field">
                <label for="status" class="field-label required">{{ $t('status') }}</label>
                <Dropdown
                  id="status"
                  v-model="localCustomer.status"
                  :options="statusOptions"
                  optionLabel="label"
                  optionValue="value"
                  :placeholder="$t('selectStatus')"
                  class="field-input"
                  @change="updateProgress"
                />
              </div>

              <!-- Country -->
              <div class="form-field">
                <label for="country" class="field-label required">{{ $t('country') }}</label>
                <Dropdown
                  id="country"
                  v-model="localCustomer.country"
                  :options="countryOptions"
                  optionLabel="label"
                  optionValue="value"
                  :placeholder="$t('selectCountry')"
                  class="field-input"
                  @change="updateProgress"
                />
              </div>

              <!-- Address -->
              <div class="form-field full-width">
                <label for="address" class="field-label">
                  {{ $t('address') }}
                  <span class="optional-label">{{ $t('optional') }}</span>
                </label>
                <Textarea
                  id="address"
                  v-model="localCustomer.address"
                  :placeholder="$t('enterAddress')"
                  rows="3"
                  class="field-input"
                  @input="updateProgress"
                />
              </div>
            </div>
          </div>
        </TabPanel>

        <!-- Purchase History Tab -->
        <TabPanel :header="$t('purchaseHistory')" class="tab-panel">
          <div class="tab-content">
            <div class="section-header">
              <h3 class="section-title">{{ $t('purchaseHistory') }}</h3>
              <p class="section-description">{{ $t('viewCustomerPurchases') }}</p>
            </div>

            <div class="form-grid">
              <!-- Total Orders -->
              <div class="form-field">
                <label for="orders" class="field-label">{{ $t('totalOrders') }}</label>
                <InputNumber
                  id="orders"
                  v-model="localCustomer.orders"
                  :placeholder="$t('enterOrders')"
                  :min="0"
                  class="field-input"
                  @input="updateProgress"
                />
              </div>

              <!-- Total Spent -->
              <div class="form-field">
                <label for="totalSpent" class="field-label">{{ $t('totalSpent') }}</label>
                <div class="input-group">
                  <span class="input-addon">TND</span>
                  <InputNumber
                    id="totalSpent"
                    v-model="localCustomer.totalSpent"
                    :placeholder="$t('enterTotalSpent')"
                    :min="0"
                    :maxFractionDigits="3"
                    class="field-input"
                    @input="updateProgress"
                  />
                </div>
              </div>

              <!-- Recent Orders -->
              <div class="form-field full-width">
                <label class="field-label">{{ $t('recentOrders') }}</label>
                <DataTable
                  :value="localCustomer.recentOrders"
                  responsiveLayout="scroll"
                  class="orders-table"
                >
                  <Column field="orderId" :header="$t('orderId')" style="width: 20%"></Column>
                  <Column field="date" :header="$t('date')" style="width: 30%"></Column>
                  <Column field="amount" :header="$t('amount')" style="width: 30%">
                    <template #body="{ data }">
                      {{ formatCurrency(data.amount) }}
                    </template>
                  </Column>
                  <Column field="status" :header="$t('status')" style="width: 20%">
                    <template #body="{ data }">
                      <Tag :value="data.status" :severity="getOrderStatusSeverity(data.status)" />
                    </template>
                  </Column>
                </DataTable>
                <div v-if="!localCustomer.recentOrders || localCustomer.recentOrders.length === 0" class="empty-orders">
                  <p class="empty-text">{{ $t('noOrdersYet') }}</p>
                </div>
              </div>
            </div>
          </div>
        </TabPanel>

        <!-- Tags Tab -->
        <TabPanel :header="$t('tags')" class="tab-panel">
          <div class="tab-content">
            <div class="section-header">
              <h3 class="section-title">{{ $t('customerTags') }}</h3>
              <p class="section-description">{{ $t('organizeCustomersWithTags') }}</p>
            </div>

            <div class="form-grid">
              <!-- Tags -->
              <div class="form-field full-width">
                <label for="tags" class="field-label">{{ $t('tags') }}</label>
                <Chips
                  id="tags"
                  v-model="localCustomer.tags"
                  :placeholder="$t('addTags')"
                  class="field-input"
                  separator=","
                  @input="updateProgress"
                />
                <small class="field-help">{{ $t('tagsHelp') }}</small>
              </div>
            </div>
          </div>
        </TabPanel>
      </TabView>
    </div>

    <!-- Form Actions -->
    <div class="form-actions">
      <div class="actions-left">
        <Button 
          :label="$t('cancel')" 
          icon="pi pi-times" 
          severity="secondary"
          outlined 
          @click="handleCancel"
        />
      </div>
      <div class="actions-right">
        <Button 
          :label="$t('saveAndAddAnother')" 
          icon="pi pi-plus" 
          severity="secondary"
          outlined 
          @click="saveAndAddAnother"
          :loading="saveAndAddLoading"
        />
        <Button 
          :label="$t('save')" 
          icon="pi pi-check" 
          @click="validateAndSave"
          :loading="saveLoading"
          class="save-button"
        />
      </div>
    </div>

    <!-- Unsaved Changes Dialog -->
    <Dialog 
      v-model:visible="showUnsavedDialog" 
      :header="$t('unsavedChanges')"
      :modal="true"
      :closable="false"
      style="width: 400px"
    >
      <p>{{ $t('unsavedChangesMessage') }}</p>
      <template #footer>
        <Button 
          :label="$t('discard')" 
          icon="pi pi-times" 
          severity="secondary"
          outlined 
          @click="discardChanges"
        />
        <Button 
          :label="$t('saveChanges')" 
          icon="pi pi-check" 
          @click="saveBeforeLeave"
          autofocus
        />
      </template>
    </Dialog>
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted, onBeforeUnmount } from 'vue'
import { useI18n } from 'vue-i18n'
import { useVuelidate } from '@vuelidate/core'
import { required, email, maxLength } from '@vuelidate/validators'

// Components
import TabView from 'primevue/tabview'
import TabPanel from 'primevue/tabpanel'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import Textarea from 'primevue/textarea'
import Dropdown from 'primevue/dropdown'
import Chips from 'primevue/chips'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dialog from 'primevue/dialog'
import Tag from 'primevue/tag'
import Tooltip from 'primevue/tooltip'

// Props & Emits
const props = defineProps({
  customer: { type: Object, default: () => ({}) },
  mode: { type: String, default: 'add' },
  loading: { type: Boolean, default: false }
})

const emit = defineEmits(['save', 'cancel', 'save-and-add'])

// Composables
const { t } = useI18n()

// Reactive Data
const activeTab = ref(0)
const localCustomer = ref(initializeCustomer())
const showUnsavedDialog = ref(false)
const hasUnsavedChanges = ref(false)
const saveLoading = ref(false)
const saveAndAddLoading = ref(false)
const pendingLoading = ref(false)

// Form validation rules
const rules = {
  name: { required, maxLength: maxLength(100) },
  email: { required, email },
  status: { required },
  country: { required }
}

const v$ = useVuelidate(rules, localCustomer)

// Options
const statusOptions = [
  { label: t('active'), value: 'Active' },
  { label: t('inactive'), value: 'Inactive' },
  { label: t('pending'), value: 'Pending' }
]

const countryOptions = [
  { label: t('tunisia'), value: 'Tunisia' },
  { label: t('france'), value: 'France' },
  { label: t('usa'), value: 'USA' },
  { label: t('uk'), value: 'UK' }
]

// Computed Properties
const formMode = computed(() => props.mode)

const formProgress = computed(() => {
  let progress = 0
  const fields = [
    localCustomer.value.name,
    localCustomer.value.email,
    localCustomer.value.status,
    localCustomer.value.country
  ]
  
  progress += fields.filter(field => field && field !== '').length * 20
  
  if (localCustomer.value.phone) progress += 10
  if (localCustomer.value.address) progress += 10
  if (localCustomer.value.tags && localCustomer.value.tags.length > 0) progress += 10
  
  return Math.min(progress, 100)
})

// Methods
function initializeCustomer() {
  return {
    name: '',
    email: '',
    status: 'Active',
    phone: '',
    country: '',
    address: '',
    orders: 0,
    totalSpent: 0,
    tags: [],
    recentOrders: [],
    ...props.customer
  }
}

function formatCurrency(value) {
  return new Intl.NumberFormat('fr-TN', {
    style: 'currency',
    currency: 'TND'
  }).format(value || 0)
}

function getOrderStatusSeverity(status) {
  const severityMap = {
    'Completed': 'success',
    'Pending': 'warning',
    'Cancelled': 'danger'
  }
  return severityMap[status] || 'info'
}

function updateProgress() {
  // Progress is handled by computed property
}

function previewCustomer() {
  console.log('Preview customer:', localCustomer.value)
}

async function saveAsPending() {
  pendingLoading.value = true
  try {
    localCustomer.value.status = 'Pending'
    await validateAndSave()
  } finally {
    pendingLoading.value = false
  }
}

async function validateAndSave() {
  const isValid = await v$.value.$validate()
  if (!isValid) {
    const firstError = v$.value.$errors[0]
    console.error('Validation error:', firstError.$message)
    return
  }

  saveLoading.value = true
  try {
    await emit('save', localCustomer.value)
    hasUnsavedChanges.value = false
  } finally {
    saveLoading.value = false
  }
}

async function saveAndAddAnother() {
  saveAndAddLoading.value = true
  try {
    await validateAndSave()
    emit('save-and-add')
    resetForm()
  } finally {
    saveAndAddLoading.value = false
  }
}

function resetForm() {
  localCustomer.value = initializeCustomer()
  v$.value.$reset()
  activeTab.value = 0
}

function handleCancel() {
  if (hasUnsavedChanges.value) {
    showUnsavedDialog.value = true
  } else {
    emit('cancel')
  }
}

function discardChanges() {
  showUnsavedDialog.value = false
  emit('cancel')
}

async function saveBeforeLeave() {
  showUnsavedDialog.value = false
  await validateAndSave()
  emit('cancel')
}

// Watch for changes
watch(localCustomer, () => {
  hasUnsavedChanges.value = true
}, { deep: true })

// Lifecycle Hooks
onMounted(() => {
  window.addEventListener('beforeunload', handleBeforeUnload)
})

onBeforeUnmount(() => {
  window.removeEventListener('beforeunload', handleBeforeUnload)
})

function handleBeforeUnload(e) {
  if (hasUnsavedChanges.value) {
    e.preventDefault()
    e.returnValue = t('unsavedChangesWarning')
    return t('unsavedChangesWarning')
  }
}
</script>

<style scoped>
.customer-form-container {
  display: flex;
  flex-direction: column;
  height: 100%;
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.form-header {
  padding: 1.5rem;
  border-bottom: 1px solid #e5e7eb;
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.header-info {
  flex: 1;
}

.form-title {
  font-size: 1.5rem;
  font-weight: 600;
  margin: 0 0 0.25rem 0;
  color: #1f2937;
}

.form-subtitle {
  margin: 0;
  color: #6b7280;
  font-size: 0.875rem;
}

.header-actions {
  display: flex;
  gap: 0.75rem;
}

.form-progress {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.progress-bar {
  flex: 1;
  height: 6px;
  background-color: #e5e7eb;
  border-radius: 3px;
  overflow: hidden;
}

.progress-fill {
  height: 100%;
  background-color: #3b82f6;
  transition: width 0.3s ease;
}

.progress-text {
  font-size: 0.875rem;
  color: #6b7280;
}

.form-content {
  flex: 1;
  overflow-y: auto;
  padding: 1.5rem;
}

:deep(.customer-tabs) {
  .p-tabview-nav {
    border-bottom: 1px solid #e5e7eb;
  }
  
  .p-tabview-panels {
    padding: 1rem 0;
  }
}

.tab-panel {
  padding: 0;
}

.section-header {
  margin-bottom: 1.5rem;
}

.section-title {
  font-size: 1.125rem;
  font-weight: 600;
  margin: 0 0 0.25rem 0;
  color: #1f2937;
}

.section-description {
  margin: 0;
  color: #6b7280;
  font-size: 0.875rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
}

.form-field {
  margin-bottom: 1.25rem;
}

.full-width {
  grid-column: 1 / -1;
}

.field-label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #374151;
  font-size: 0.875rem;
}

.required:after {
  content: " *";
  color: #ef4444;
}

.optional-label {
  color: #6b7280;
  font-size: 0.75rem;
  margin-left: 0.25rem;
}

.help-icon {
  margin-left: 0.25rem;
  color: #9ca3af;
  cursor: help;
}

.field-input {
  width: 100%;
}

.field-footer {
  display: flex;
  justify-content: space-between;
  margin-top: 0.25rem;
}

.error-message {
  color: #ef4444;
  font-size: 0.75rem;
}

.char-counter {
  color: #9ca3af;
  font-size: 0.75rem;
}

.input-group {
  display: flex;
  align-items: center;
}

.input-addon {
  padding: 0.5rem 0.75rem;
  background-color: #f3f4f6;
  border: 1px solid #d1d5db;
  border-right: none;
  border-radius: 6px 0 0 6px;
  font-size: 0.875rem;
  color: #4b5563;
}

:deep(.orders-table) {
  .p-datatable-tbody > tr > td {
    padding: 0.5rem 0.75rem;
  }
}

.empty-orders {
  padding: 1.5rem;
  text-align: center;
  border: 1px dashed #d1d5db;
  border-radius: 6px;
  background-color: #f9fafb;
}

.empty-text {
  margin: 0;
  color: #6b7280;
}

.form-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  border-top: 1px solid #e5e7eb;
  background-color: #fff;
}

.actions-left, .actions-right {
  display: flex;
  gap: 0.75rem;
}

.save-button {
  min-width: 120px;
}

/* Responsive */
@media (max-width: 768px) {
  .header-content {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }
  
  .header-actions {
    width: 100%;
    justify-content: flex-end;
  }
  
  .form-grid {
    grid-template-columns: 1fr;
  }
  
  .form-actions {
    flex-direction: column-reverse;
    gap: 1rem;
  }
  
  .actions-left, .actions-right {
    width: 100%;
    justify-content: space-between;
  }
}
</style>