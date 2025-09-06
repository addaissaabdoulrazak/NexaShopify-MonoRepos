<template>
  <div class="settings-page">
    <div class="settings-header">
      <h1 class="page-title">{{ $t('settings') }}</h1>
      <div class="header-actions">
        <Button :label="$t('saveChanges')" icon="pi pi-save" @click="saveSettings" :loading="isSaving" />
      </div>
    </div>

    <div class="settings-content">
      <TabView v-model:activeIndex="activeTab" class="settings-tabs">
        <TabPanel :header="$t('taxesAndDuties')">
          <div class="tab-content">
            <Card class="tax-card">
              <template #title>
                <div class="card-title">
                  <i class="pi pi-globe icon"></i>
                  <span>{{ $t('taxRegions') }}</span>
                </div>
              </template>
              <template #subtitle>
                {{ $t('taxRegionsDescription') }}
              </template>
              <template #content>
                <DataTable :value="taxRegions" responsiveLayout="scroll" class="tax-table">
                  <template #header>
                    <div class="table-header">
                      <span class="p-input-icon-left">
                        <i class="pi pi-search" />
                        <InputText v-model="filters['global'].value" :placeholder="$t('search')" />
                      </span>
                      <Button :label="$t('addTaxRegion')" icon="pi pi-plus" severity="primary" @click="openNewRegionDialog" />
                    </div>
                  </template>
                  
                  <Column field="country" :header="$t('region')" :sortable="true">
                    <template #body="{ data }">
                      <div class="country-cell">
                        <img :src="getCountryFlagUrl(data.countryCode)" :alt="data.country" class="country-flag" />
                        <span>{{ getCountryName(data.countryCode) }}</span>
                      </div>
                    </template>
                  </Column>
                  
                  <Column field="taxRate" :header="$t('taxRate')" :sortable="true">
                    <template #body="{ data }">
                      <span>{{ data.taxRate }}%</span>
                    </template>
                  </Column>

                  <Column field="taxIncluded" :header="$t('taxIncludedInPrice')">
                     <template #body="{ data }">
                        <Badge :value="data.taxIncluded ? $t('yes') : $t('no')" :severity="data.taxIncluded ? 'success' : 'info'"></Badge>
                    </template>
                  </Column>

                  <Column :header="$t('actions')" bodyClass="text-right">
                    <template #body="{ data, index }">
                      <Button icon="pi pi-pencil" text rounded @click="editRegion(data, index)" />
                      <Button icon="pi pi-trash" severity="danger" text rounded @click="confirmDeleteRegion(data, index)" />
                    </template>
                  </Column>

                   <template #empty>
                    {{ $t('noTaxRegionsFound') }}
                  </template>
                </DataTable>
              </template>
            </Card>
          </div>
        </TabPanel>
        
        <TabPanel :header="$t('general')">
          <p class="p-m-0">General settings coming soon.</p>
        </TabPanel>

        <TabPanel :header="$t('plan')">
          <div class="tab-content">
            <div class="plan-grid">
              <Card class="plan-card">
                <template #title>{{ $t('planDetails') }}</template>
                <template #content>
                  <div class="plan-details-content">
                    <div class="current-plan-info">
                      <div class="plan-name">
                        <span>{{ currentPlan.name }}</span>
                        <Badge v-if="currentPlan.name === 'Trial'" :value="`${currentPlan.daysRemaining} ${$t('daysRemaining')}`" severity="success"></Badge>
                      </div>
                      <div class="plan-actions">
                        <Button :label="$t('cancelTrial')" severity="secondary" text />
                        <Button :label="$t('choosePlan')" @click="goToChoosePlan" />
                      </div>
                    </div>
                    <p class="terms-text">
                      {{ $t('viewThe') }} <a href="#">{{ $t('termsOfService') }}</a> {{ $t('and') }} <a href="#">{{ $t('privacyPolicy') }}</a>.
                    </p>
                  </div>
                </template>
              </Card>

              <Card class="plan-card">
                <template #title>{{ $t('subscriptions') }}</template>
                <template #content>
                  <p>{{ $t('subscriptionsDescription') }}</p>
                  <Button :label="$t('viewAllSubscriptions')" class="p-button-text p-button-plain" />
                </template>
              </Card>
            </div>
          </div>
        </TabPanel>

        <TabPanel :header="$t('shipping')">
          <p class="p-m-0">Shipping settings coming soon.</p>
        </TabPanel>
      </TabView>
    </div>

    <!-- Add/Edit Tax Region Dialog -->
    <Dialog v-model:visible="isRegionDialogVisible" :header="dialogHeader" :modal="true" style="width: 450px">
      <div class="p-fluid">
        <div class="field">
          <label for="country">{{ $t('country') }}</label>
          <Dropdown id="country" v-model="editableRegion.countryCode" :options="availableCountries" optionLabel="name" optionValue="code" :placeholder="$t('selectCountry')" :disabled="!isNewRegion" filter />
        </div>
        <div class="field">
          <label for="taxRate">{{ $t('taxRate') }} (%)</label>
          <InputNumber id="taxRate" v-model="editableRegion.taxRate" :min="0" :max="100" :maxFractionDigits="2" />
        </div>
        <div class="field-checkbox">
          <Checkbox id="taxIncluded" v-model="editableRegion.taxIncluded" :binary="true" />
          <label for="taxIncluded">{{ $t('pricesIncludeTax') }}</label>
        </div>
      </div>
      <template #footer>
        <Button :label="$t('cancel')" icon="pi pi-times" text @click="isRegionDialogVisible = false" />
        <Button :label="$t('save')" icon="pi pi-check" @click="saveRegion" />
      </template>
    </Dialog>

    <!-- Delete Confirmation Dialog -->
    <Dialog v-model:visible="isDeleteDialogVisible" header="Confirmation" :modal="true" :style="{ width: '350px' }">
        <div class="confirmation-content">
            <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
            <span>{{ $t('confirmDeleteMessage') }}</span>
        </div>
        <template #footer>
            <Button :label="$t('no')" icon="pi pi-times" text @click="isDeleteDialogVisible = false"/>
            <Button :label="$t('yes')" icon="pi pi-check" text @click="deleteRegion" />
        </template>
    </Dialog>

  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import Card from 'primevue/card';
import Button from 'primevue/button';
import TabView from 'primevue/tabview';
import TabPanel from 'primevue/tabpanel';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
import Dialog from 'primevue/dialog';
import Dropdown from 'primevue/dropdown';
import InputNumber from 'primevue/inputnumber';
import Checkbox from 'primevue/checkbox';
import Badge from 'primevue/badge';

const { t } = useI18n();
const router = useRouter();

// State
const activeTab = ref(0);
const isSaving = ref(false);
const isRegionDialogVisible = ref(false);
const isDeleteDialogVisible = ref(false);
const isNewRegion = ref(true);
const editableRegion = ref({});
const regionToDelete = ref(null);
const currentPlan = ref({
  name: 'Trial',
  daysRemaining: 1,
});

// Data
const taxRegions = ref([
  { countryCode: 'TN', taxRate: 19, taxIncluded: false },
  { countryCode: 'FR', taxRate: 20, taxIncluded: true },
  { countryCode: 'US', taxRate: 8.5, taxIncluded: false },
]);

const countries = ref([
    { name: 'Tunisia', code: 'TN' },
    { name: 'France', code: 'FR' },
    { name: 'United States', code: 'US' },
    { name: 'Canada', code: 'CA' },
    { name: 'Germany', code: 'DE' },
    { name: 'United Kingdom', code: 'GB' },
]);

const filters = ref({
    'global': { value: null, matchMode: 'contains' },
});

// Computed Properties
const dialogHeader = computed(() => isNewRegion.value ? t('addTaxRegion') : t('editTaxRegion'));

const availableCountries = computed(() => {
  const usedCountryCodes = taxRegions.value.map(r => r.countryCode);
  return countries.value.filter(c => !usedCountryCodes.includes(c.code));
});

// Methods
const saveSettings = () => {
  isSaving.value = true;
  console.log('Saving settings...', taxRegions.value);
  setTimeout(() => {
    isSaving.value = false;
    // Add toast notification for success
  }, 1000);
};

const openNewRegionDialog = () => {
  isNewRegion.value = true;
  editableRegion.value = { countryCode: null, taxRate: 0, taxIncluded: false };
  isRegionDialogVisible.value = true;
};

const editRegion = (region, index) => {
  isNewRegion.value = false;
  editableRegion.value = { ...region, index };
  isRegionDialogVisible.value = true;
};

const saveRegion = () => {
  if (isNewRegion.value) {
    taxRegions.value.push({ ...editableRegion.value });
  } else {
    taxRegions.value[editableRegion.value.index] = { ...editableRegion.value };
  }
  isRegionDialogVisible.value = false;
};

const confirmDeleteRegion = (region, index) => {
    regionToDelete.value = { ...region, index };
    isDeleteDialogVisible.value = true;
};

const deleteRegion = () => {
    taxRegions.value.splice(regionToDelete.value.index, 1);
    isDeleteDialogVisible.value = false;
    regionToDelete.value = null;
};

const getCountryName = (code) => {
    const country = countries.value.find(c => c.code === code);
    return country ? country.name : code;
};

const getCountryFlagUrl = (code) => {
  return `https://flagcdn.com/w20/${code.toLowerCase()}.png`;
};

const goToChoosePlan = () => {
  router.push({ name: 'ChoosePlan' });
};
</script>

<style scoped>
.settings-page {
  padding: 1.5rem;
  background-color: #f9fafb;
  min-height: 100vh;
}

.settings-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.page-title {
  font-size: 1.75rem;
  font-weight: 600;
  color: #1f2937;
}

.settings-content {
  background-color: #ffffff;
  border-radius: 8px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.05);
}

:deep(.settings-tabs .p-tabview-nav) {
  padding: 0.5rem 1.5rem 0 1.5rem;
  border-bottom: 1px solid #e5e7eb;
}

:deep(.settings-tabs .p-tabview-panels) {
  padding: 1.5rem;
}

.tax-card {
  border: none;
  box-shadow: none;
}

.card-title {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 1.25rem;
}

.card-title .icon {
  font-size: 1.5rem;
  color: #6b7280;
}

.table-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.5rem 0;
}

.country-cell {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.country-flag {
  width: 20px;
  border: 1px solid #eee;
}

.field {
    margin-bottom: 1rem;
}

.field-checkbox {
    margin-top: 1.5rem;
    display: flex;
    align-items: center;
    gap: 0.5rem;
}

.confirmation-content {
    display: flex;
    align-items: center;
    justify-content: center;
    text-align: center;
}

.confirmation-content i {
    margin-right: 1rem;
}

.plan-grid {
  display: grid;
  grid-template-columns: 1fr;
  gap: 1.5rem;
}

.plan-card .p-card-content {
  padding-top: 0;
}

.plan-details-content {
  display: flex;
  flex-direction: column;
}

.current-plan-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  background-color: #f9fafb;
}

.plan-name {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-weight: 600;
}

.plan-actions {
  display: flex;
  gap: 0.5rem;
}

.terms-text {
  margin-top: 1rem;
  font-size: 0.875rem;
  color: #6b7280;
}

.terms-text a {
  color: #3b82f6;
  text-decoration: none;
}

.terms-text a:hover {
  text-decoration: underline;
}
</style>