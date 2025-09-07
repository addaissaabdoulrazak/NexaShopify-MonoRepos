<template>
  <div class="product-form-container">
    <!-- Form Header -->
    <div class="form-header">
      <div class="header-content">
        <div class="header-info">
          <h2 class="form-title">
            {{ formMode === 'add' ? $t('addNewProduct') : $t('editProduct') }}
          </h2>
          <p class="form-subtitle">
            {{ formMode === 'add' ? $t('createNewProductDescription') : $t('updateProductDescription') }}
          </p>
        </div>
        <div class="header-actions">
          <Button 
            :label="$t('preview')" 
            icon="pi pi-eye" 
            severity="secondary" 
            outlined 
            @click="previewProduct"
            :disabled="!product.title"
          />
          <Button 
            :label="$t('saveAsDraft')" 
            icon="pi pi-save" 
            severity="secondary" 
            outlined 
            @click="saveAsDraft"
            :loading="draftLoading"
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
      <TabView v-model:activeIndex="activeTab" class="product-tabs" @tab-change="onTabChange">
        <!-- Basic Information Tab -->
        <TabPanel :header="$t('basicInfo')" class="tab-panel">
          <div class="tab-content">
            <div class="section-header">
              <h3 class="section-title">{{ $t('productDetails') }}</h3>
              <p class="section-description">{{ $t('basicProductInformation') }}</p>
            </div>
            
            <div class="form-grid">
              <!-- Product Title -->
              <div class="form-field full-width" :class="{ 'field-error': v$.title.$error }">
                <label for="title" class="field-label required">
                  {{ $t('productTitle') }}
                  <Tooltip target=".title-help" :value="$t('titleTooltip')" position="top" />
                  <i class="pi pi-question-circle title-help help-icon"></i>
                </label>
                <InputText
                  id="title"
                  v-model="localProduct.title"
                  :class="{ 'p-invalid': v$.title.$error }"
                  :placeholder="$t('enterProductTitle')"
                  @blur="v$.title.$touch"
                  @input="updateProgress"
                  maxlength="100"
                  class="field-input"
                />
                <div class="field-footer">
                  <small v-if="v$.title.$error" class="error-message">
                    {{ v$.title.$errors[0].$message }}
                  </small>
                  <small class="char-counter">
                    {{ localProduct.title?.length || 0 }}/100
                  </small>
                </div>
              </div>

              <!-- Product Description -->
              <div class="form-field full-width">
                <label class="field-label">
                  {{ $t('description') }}
                  <span class="optional-label">{{ $t('optional') }}</span>
                </label>
                <div class="editor-container">
                  <QuillEditor 
                    v-model:content="localProduct.description" 
                    contentType="html"
                    :options="editorOptions"
                    @textChange="updateProgress"
                    class="product-editor"
                  />
                </div>
              </div>

              <!-- Status and Visibility -->
              <div class="form-field">
                <label for="status" class="field-label required">{{ $t('status') }}</label>
                <Dropdown
                  id="status"
                  v-model="localProduct.status"
                  :options="statusOptions"
                  optionLabel="label"
                  optionValue="value"
                  :placeholder="$t('selectStatus')"
                  class="field-input"
                  @change="updateProgress"
                />
              </div>

              <!-- Product Type -->
              <div class="form-field">
                <label for="type" class="field-label required">{{ $t('productType') }}</label>
                <Dropdown
                  id="type"
                  v-model="localProduct.type"
                  :options="typeOptions"
                  optionLabel="label"
                  optionValue="value"
                  :placeholder="$t('selectType')"
                  class="field-input"
                  @change="updateProgress"
                />
              </div>

              <!-- Publication Settings -->
              <div class="form-field full-width">
                <label class="field-label">{{ $t('availability') }}</label>
                <div class="availability-cards">
                  <div class="checkbox-card">
                    <div class="checkbox-item">
                      <Checkbox
                        v-model="localProduct.onlineStore"
                        :binary="true"
                        inputId="onlineStore"
                        @change="updateProgress"
                      />
                      <label for="onlineStore" class="checkbox-label">
                        <span class="label-text">{{ $t('onlineStore') }}</span>
                        <small class="label-description">{{ $t('onlineStoreDescription') }}</small>
                      </label>
                    </div>
                  </div>
                  <div class="checkbox-card">
                    <div class="checkbox-item">
                      <Checkbox
                        v-model="localProduct.pointOfSale"
                        :binary="true"
                        inputId="pointOfSale"
                        @change="updateProgress"
                      />
                      <label for="pointOfSale" class="checkbox-label">
                        <span class="label-text">{{ $t('pointOfSale') }}</span>
                        <small class="label-description">{{ $t('pointOfSaleDescription') }}</small>
                      </label>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </TabPanel>

        <!-- Media Tab -->
        <TabPanel :header="$t('media')" class="tab-panel">
          <div class="tab-content">
            <div class="section-header">
              <h3 class="section-title">{{ $t('productMedia') }}</h3>
              <p class="section-description">{{ $t('addProductImages') }}</p>
            </div>

            <!-- Media Upload Zone -->
            <div class="media-upload-container">
              <div 
                class="dropzone-area"
                :class="{ 'dragover': isDragOver }"
                @drop="handleDrop"
                @dragover="handleDragOver"
                @dragleave="handleDragLeave"
                @click="triggerFileInput"
              >
                <input
                  ref="fileInput"
                  type="file"
                  multiple
                  accept="image/*,video/*"
                  @change="handleFileSelect"
                  class="file-input"
                />
                <div class="dropzone-content">
                  <i class="pi pi-cloud-upload dropzone-icon"></i>
                  <h4 class="dropzone-title">{{ $t('dragDropMedia') }}</h4>
                  <p class="dropzone-description">{{ $t('supportedFormats') }}</p>
                  <Button 
                    :label="$t('browseFiles')" 
                    icon="pi pi-plus" 
                    outlined 
                    class="browse-button"
                  />
                </div>
              </div>

              <!-- Media Preview Grid -->
              <div v-if="localProduct.media && localProduct.media.length > 0" class="media-grid">
                <TransitionGroup name="media" tag="div" class="media-items">
                  <div 
                    v-for="(mediaItem, index) in localProduct.media" 
                    :key="mediaItem.id || index"
                    class="media-item"
                    :class="{ 'featured': index === 0 }"
                  >
                    <div class="media-preview">
                      <img 
                        v-if="mediaItem.type === 'image'"
                        :src="mediaItem.url || mediaItem.preview" 
                        :alt="mediaItem.alt || `Media ${index + 1}`"
                        class="media-thumbnail"
                      />
                      <div v-else class="video-thumbnail">
                        <i class="pi pi-play-circle video-icon"></i>
                      </div>
                      
                      <!-- Media Actions -->
                      <div class="media-actions">
                        <Button 
                          icon="pi pi-star" 
                          :class="{ 'featured-btn': index === 0 }"
                          rounded 
                          text 
                          size="small"
                          @click="setFeaturedMedia(index)"
                          v-tooltip.top="index === 0 ? $t('featuredImage') : $t('setAsFeatured')"
                        />
                        <Button 
                          icon="pi pi-pencil" 
                          severity="secondary"
                          rounded 
                          text 
                          size="small"
                          @click="editMedia(index)"
                          v-tooltip.top="$t('edit')"
                        />
                        <Button 
                          icon="pi pi-trash" 
                          severity="danger"
                          rounded 
                          text 
                          size="small"
                          @click="removeMedia(index)"
                          v-tooltip.top="$t('delete')"
                        />
                      </div>
                    </div>
                    
                    <!-- Media Info -->
                    <div class="media-info">
                      <InputText 
                        v-model="mediaItem.alt"
                        :placeholder="$t('altText')"
                        class="alt-input"
                        size="small"
                      />
                      <div class="media-meta">
                        <span class="file-size">{{ formatFileSize(mediaItem.size) }}</span>
                        <Badge v-if="index === 0" value="Featured" severity="success" class="featured-badge" />
                      </div>
                    </div>
                  </div>
                </TransitionGroup>
              </div>
            </div>
          </div>
        </TabPanel>

        <!-- Pricing Tab -->
        <TabPanel :header="$t('pricing')" class="tab-panel">
          <div v-if="!productHasOptions" class="tab-content">
            <div class="section-header">
              <h3 class="section-title">{{ $t('pricingInformation') }}</h3>
              <p class="section-description">{{ $t('setPricingDetails') }}</p>
            </div>

            <div class="form-grid">
              <!-- Price -->
              <div class="form-field">
                <label for="price" class="field-label required">{{ $t('price') }}</label>
                <div class="input-group">
                  <span class="input-addon">TND</span>
                  <InputNumber
                    id="price"
                    v-model="localProduct.price"
                    :placeholder="$t('enterPrice')"
                    :min="0"
                    :maxFractionDigits="3"
                    class="field-input"
                    @input="calculateProfitMargin"
                  />
                </div>
              </div>

              <!-- Compare at Price -->
              <div class="form-field">
                <label for="compareAtPrice" class="field-label">
                  {{ $t('compareAtPrice') }}
                  <span class="optional-label">{{ $t('optional') }}</span>
                </label>
                <div class="input-group">
                  <span class="input-addon">TND</span>
                  <InputNumber
                    id="compareAtPrice"
                    v-model="localProduct.compareAtPrice"
                    :placeholder="$t('originalPrice')"
                    :min="0"
                    :maxFractionDigits="3"
                    class="field-input"
                  />
                </div>
                <small v-if="discountPercentage > 0" class="field-help success">
                  {{ $t('discountPercentage', { percentage: discountPercentage }) }}
                </small>
              </div>

              <!-- Cost per Item -->
              <div class="form-field">
                <label for="costPerItem" class="field-label">
                  {{ $t('costPerItem') }}
                  <Tooltip target=".cost-help" :value="$t('costTooltip')" position="top" />
                  <i class="pi pi-question-circle cost-help help-icon"></i>
                </label>
                <div class="input-group">
                  <span class="input-addon">TND</span>
                  <InputNumber
                    id="costPerItem"
                    v-model="localProduct.costPerItem"
                    :placeholder="$t('enterCost')"
                    :min="0"
                    :maxFractionDigits="3"
                    class="field-input"
                    @input="calculateProfitMargin"
                  />
                </div>
              </div>

              <!-- Profit Margin Display -->
              <div class="form-field">
                <label class="field-label">{{ $t('profitAnalysis') }}</label>
                <div class="profit-display">
                  <div class="profit-item">
                    <span class="profit-label">{{ $t('profit') }}</span>
                    <span class="profit-value" :class="profitClass">
                      {{ formatCurrency(calculatedProfit) }}
                    </span>
                  </div>
                  <div class="profit-item">
                    <span class="profit-label">{{ $t('margin') }}</span>
                    <span class="profit-value" :class="marginClass">
                      {{ calculatedMargin }}%
                    </span>
                  </div>
                </div>
              </div>

              <!-- Tax Settings -->
              <div class="form-field full-width">
                <div class="checkbox-item">
                  <Checkbox
                    v-model="localProduct.taxable"
                    :binary="true"
                    inputId="taxable"
                  />
                  <label for="taxable" class="checkbox-label">
                    <span class="label-text">{{ $t('chargesTax') }}</span>
                    <small class="label-description">{{ $t('taxDescription') }}</small>
                  </label>
                </div>
              </div>
            </div>
          </div>
          <div v-else class="tab-content">
             <Message severity="info">{{ $t('variantPricingMessage') }}</Message>
          </div>
        </TabPanel>

        <!-- Inventory Tab -->
        <TabPanel :header="$t('inventory')" class="tab-panel">
          <div class="tab-content">
            <!-- Main Inventory Section -->
            <div class="inventory-section">
              <div class="section-header">
                <h3 class="section-title">{{ $t('inventoryTracking') }}</h3>
                <p class="section-description">{{ $t('manageBaseInventory') }}</p>
              </div>
              <div class="form-grid inventory-grid">
                <!-- Track Quantity -->
                <div class="form-field full-width">
                  <div class="checkbox-item">
                    <Checkbox
                      v-model="localProduct.trackQuantity"
                      :binary="true"
                      inputId="trackQuantity"
                      @change="updateProgress"
                    />
                    <label for="trackQuantity" class="checkbox-label">
                      <span class="label-text">{{ $t('trackQuantity') }}</span>
                      <small class="label-description">{{ $t('trackQuantityDescription') }}</small>
                    </label>
                  </div>
                </div>

                <!-- Quantity and Continue Selling -->
              <!-- cet input doit être un bloque conditionnel une fois une variantes de product ajouter il doit disparaitre-->
                 <div v-if="localProduct.trackQuantity" class="form-field">
                  <label for="quantity" class="field-label">{{ $t('quantity') }}</label>
                  <InputNumber
                    id="quantity"
                    v-model="localProduct.quantity"
                    :placeholder="$t('enterQuantity')"
                    :min="0"
                    class="field-input"
                  />
                  <div class="inventory-status" v-if="localProduct.quantity !== null">
                    <Badge 
                      :value="getInventoryStatus(localProduct.quantity).label" 
                      :severity="getInventoryStatus(localProduct.quantity).severity"
                    />
                  </div>
                </div> 
                    <!---->


                <div v-if="localProduct.trackQuantity" class="form-field continue-selling-field">
                  <div class="checkbox-item">
                    <Checkbox
                      v-model="localProduct.continueSelling"
                      :binary="true"
                      inputId="continueSelling"
                    />
                    <label for="continueSelling" class="checkbox-label">
                      <span class="label-text">{{ $t('continueSelling') }}</span>
                      <small class="label-description">{{ $t('continueSellingDescription') }}</small>
                    </label>
                  </div>
                </div>

                <!-- Has SKU/Barcode Checkbox -->
                <div class="form-field full-width">
                  <div class="checkbox-item">
                    <Checkbox
                      v-model="hasSkuOrBarcode"
                      :binary="true"
                      inputId="hasSkuOrBarcode"
                    />
                    <label for="hasSkuOrBarcode" class="checkbox-label">
                      <span class="label-text">{{ $t('hasSkuOrBarcode') }}</span>
                    </label>
                  </div>
                </div>

                <!-- SKU and Barcode -->
                <template v-if="hasSkuOrBarcode">
                  <div class="form-field">
                    <label for="sku" class="field-label">
                      {{ $t('sku') }}
                      <span class="optional-label">{{ $t('optional') }}</span>
                    </label>
                    <div class="input-with-button">
                      <InputText 
                        id="sku"
                        v-model="localProduct.sku" 
                        :placeholder="$t('enterSku')" 
                        class="field-input"
                      />
                      <Button 
                        icon="pi pi-refresh" 
                        text 
                        rounded
                        @click="generateSku"
                        v-tooltip.top="$t('generateSku')"
                        class="generate-sku-btn"
                      />
                    </div>
                  </div>

                  <div class="form-field">
                    <label for="barcode" class="field-label">
                      {{ $t('barcode') }}
                      <span class="optional-label">{{ $t('optional') }}</span>
                    </label>
                    <InputText 
                      id="barcode"
                      v-model="localProduct.barcode" 
                      :placeholder="$t('enterBarcode')" 
                      class="field-input"
                    />
                  </div>
                </template>
              </div>
            </div>

            <!-- Divider -->
            <div class="section-divider">
              <span class="divider-title">{{ $t('variants') }}</span>
            </div>

            <!-- Variants Section -->
            <div class="variants-section">
              <div class="section-header">
                <h3 class="section-title">{{ $t('productVariants') }}</h3>
                <p class="section-description">{{ $t('addOptionsDescription') }}</p>
              </div>
              <div class="form-grid">
                <!-- Variant Options -->
                <div class="form-field full-width">
                  <div v-for="(option, optionIndex) in localProduct.options" :key="option.id" class="option-card">
                    <div class="option-header">
                      <h5 class="option-title">{{ $t('option') }} {{ optionIndex + 1 }}</h5>
                      <Button icon="pi pi-times" severity="danger" text rounded @click="removeOption(optionIndex)" />
                    </div>
                    <div class="option-body">
                      <div class="form-field">
                        <label :for="`option_name_${optionIndex}`" class="field-label">{{ $t('optionNameLabel') }}</label>
                        <InputText :id="`option_name_${optionIndex}`" v-model="option.name" :placeholder="$t('optionNamePlaceholder')" class="option-name-input" @update:modelValue="generateVariants" />
                      </div>
                      <div class="form-field">
                        <label :for="`option_values_${optionIndex}`" class="field-label">{{ $t('optionValuesLabel') }}</label>
                        <Chips :id="`option_values_${optionIndex}`" v-model="option.values" :placeholder="$t('optionValuesPlaceholder')" separator="," @update:modelValue="generateVariants" />
                        <small>{{ $t('tagsHelp') }}</small>
                      </div>
                    </div>
                  </div>

                  <Button 
                    v-if="localProduct.options.length < 3"
                    :label="$t('addAnotherOption')" 
                    icon="pi pi-plus" 
                    @click="addOption" 
                    class="p-button-text" 
                  />
                </div>

                <!-- Variants Preview Table -->
                <div v-if="localProduct.variants && localProduct.variants.length > 0" class="form-field full-width variants-table-container">
                  <h4 class="variants-preview-title">{{ $t('variantsPreview') }}</h4>
                  <DataTable :value="localProduct.variants" class="p-datatable-sm">
                    <Column v-for="option in localProduct.options.filter(o => o.name)" :key="option.name" :header="option.name" :field="option.name" />
                    <Column :header="$t('price')">
                      <template #body="{ data }">
                        <InputNumber v-model="data.price" mode="currency" currency="TND" locale="fr-TN" class="w-full" />
                      </template>
                    </Column>
                    <Column :header="$t('inventory')">
                      <template #body="{ data }">
                        <InputNumber v-model="data.stock" class="w-full" />
                      </template>
                    </Column>
                    <Column :header="$t('sku')">
                      <template #body="{ data }">
                        <InputText v-model="data.sku" class="w-full" />
                      </template>
                    </Column>
                  </DataTable>
                </div>
              </div>
            </div>
          </div>
        </TabPanel>

        <!-- Organization Tab -->
        <TabPanel :header="$t('organization')" class="tab-panel">
          <div class="tab-content">
            <div class="section-header">
              <h3 class="section-title">{{ $t('productOrganization') }}</h3>
              <p class="section-description">{{ $t('categorizeAndTagProduct') }}</p>
            </div>

            <div class="form-grid">
              <!-- Vendor -->
              <div class="form-field">
                <label for="vendor" class="field-label">{{ $t('vendor') }}</label>
                <AutoComplete
                  id="vendor"
                  v-model="localProduct.vendor"
                  :suggestions="filteredVendors"
                  @complete="searchVendors"
                  :placeholder="$t('selectOrCreateVendor')"
                  class="field-input"
                />
              </div>

              <!-- Collections -->
              <div class="form-field">
                <label for="collections" class="field-label">{{ $t('collections') }}</label>
                <MultiSelect
                  id="collections"
                  v-model="localProduct.collections"
                  :options="collectionOptions"
                  optionLabel="label"
                  optionValue="value"
                  :placeholder="$t('selectCollections')"
                  class="field-input"
                  filter
                />
              </div>

              <!-- Tags -->
              <div class="form-field full-width">
                <label for="tags" class="field-label">{{ $t('tags') }}</label>
                <Chips
                  id="tags"
                  v-model="localProduct.tags"
                  :placeholder="$t('addTags')"
                  class="field-input"
                  separator=","
                />
                <small class="field-help">{{ $t('tagsHelp') }}</small>
              </div>

              <!-- Search Engine Optimization -->
              <div class="form-field full-width">
                <div class="seo-header">
                  <h4 class="seo-title">{{ $t('searchEngineOptimization') }}</h4>
                  <Button 
                    :label="$t('autoGenerate')" 
                    icon="pi pi-magic-wand" 
                    text 
                    size="small"
                    @click="generateSeoData"
                  />
                </div>
                
                <div class="seo-fields">
                  <div class="form-field">
                    <label for="seoTitle" class="field-label">{{ $t('seoTitle') }}</label>
                    <InputText
                      id="seoTitle"
                      v-model="localProduct.seoTitle"
                      :placeholder="$t('enterSeoTitle')"
                      maxlength="60"
                      class="field-input"
                    />
                    <div class="char-counter">
                      {{ localProduct.seoTitle?.length || 0 }}/60
                    </div>
                  </div>
                  
                  <div class="form-field">
                    <label for="seoDescription" class="field-label">{{ $t('seoDescription') }}</label>
                    <Textarea
                      id="seoDescription"
                      v-model="localProduct.seoDescription"
                      :placeholder="$t('enterSeoDescription')"
                      maxlength="160"
                      rows="3"
                      class="field-input"
                    />
                    <div class="char-counter">
                      {{ localProduct.seoDescription?.length || 0 }}/160
                    </div>
                  </div>
                </div>

                <!-- SEO Preview -->
                <div class="seo-preview">
                  <h5 class="preview-title">{{ $t('searchPreview') }}</h5>
                  <div class="preview-card">
                    <div class="preview-url">yourstore.com/products/{{ generateSlug(localProduct.title) }}</div>
                    <div class="preview-link">{{ localProduct.seoTitle || localProduct.title || 'Product Title' }}</div>
                    <div class="preview-description">
                      {{ localProduct.seoDescription || $t('noDescriptionProvided') }}
                    </div>
                  </div>
                </div>
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
import { ref, reactive, computed, watch, nextTick, onMounted, onBeforeUnmount } from 'vue'
import { useI18n } from 'vue-i18n'
import { useVuelidate } from '@vuelidate/core'
import { required, maxLength, minValue } from '@vuelidate/validators'

// Components
import TabView from 'primevue/tabview'
import TabPanel from 'primevue/tabpanel'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import Textarea from 'primevue/textarea'
import Dropdown from 'primevue/dropdown'
import MultiSelect from 'primevue/multiselect'
import AutoComplete from 'primevue/autocomplete'
import Checkbox from 'primevue/checkbox'
import Chips from 'primevue/chips'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dialog from 'primevue/dialog'
import Badge from 'primevue/badge'
import Tooltip from 'primevue/tooltip'
import { QuillEditor } from '@vueup/vue-quill'
import '@vueup/vue-quill/dist/vue-quill.snow.css';
import Message from 'primevue/message';

// Props & Emits
const props = defineProps({
  product: { type: Object, default: () => ({}) },
  mode: { type: String, default: 'add' },
  loading: { type: Boolean, default: false }
})

const emit = defineEmits(['save', 'cancel', 'save-and-add'])

// Composables
const { t } = useI18n()

// Reactive Data
const activeTab = ref(0)
const localProduct = ref(initializeProduct())
const isDragOver = ref(false)
const showUnsavedDialog = ref(false)
const hasUnsavedChanges = ref(false)
const saveLoading = ref(false)
const saveAndAddLoading = ref(false)
const draftLoading = ref(false)
const fileInput = ref(null)
const filteredVendors = ref([])
const hasSkuOrBarcode = ref(false)

// Form validation rules
const rules = {
  title: { required, maxLength: maxLength(100) },
  price: { minValue: minValue(0) },
  status: { required },
  type: { required }
}

const v$ = useVuelidate(rules, localProduct)

// Options
const statusOptions = [
  { label: t('active'), value: 'active' },
  { label: t('draft'), value: 'draft' },
  { label: t('archived'), value: 'archived' }
]

const typeOptions = [
  { label: t('physicalProduct'), value: 'physical' },
  { label: t('digitalProduct'), value: 'digital' },
  { label: t('service'), value: 'service' }
]

const collectionOptions = [
  { label: t('featured'), value: 'featured' },
  { label: t('newArrivals'), value: 'new-arrivals' },
  { label: t('bestsellers'), value: 'bestsellers' }
]

const vendorOptions = [
  'Apple', 'Samsung', 'Sony', 'Nike', 'Adidas'
]

const editorOptions = {
  theme: 'snow',
  modules: {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ 'list': 'ordered'}, { 'list': 'bullet' }],
      ['link', 'clean']
    ]
  }
}

// Computed Properties
const formMode = computed(() => props.mode)

const formProgress = computed(() => {
  let progress = 0
  const fields = [
    localProduct.value.title,
    localProduct.value.status,
    localProduct.value.type,
    localProduct.value.price
  ]
  
  progress += fields.filter(field => field && field !== '').length * 20
  
  if (localProduct.value.description) progress += 10
  if (localProduct.value.media && localProduct.value.media.length > 0) progress += 10
  
  return Math.min(progress, 100)
})

const calculatedProfit = computed(() => {
  const price = localProduct.value.price || 0
  const cost = localProduct.value.costPerItem || 0
  return price - cost
})

const calculatedMargin = computed(() => {
  const price = localProduct.value.price || 0
  const cost = localProduct.value.costPerItem || 0
  if (price === 0)
      return 0
  return Math.round(((price - cost) / price) * 100)
})

const discountPercentage = computed(() => {
  if (!localProduct.value.compareAtPrice || localProduct.value.compareAtPrice <= 0) return 0
  const price = localProduct.value.price || 0
  const comparePrice = localProduct.value.compareAtPrice
  return Math.round(((comparePrice - price) / comparePrice) * 100)
})

const profitClass = computed(() => {
  return calculatedProfit.value >= 0 ? 'positive' : 'negative'
})

const marginClass = computed(() => {
  return calculatedMargin.value >= 0 ? 'positive' : 'negative'
})

const productHasOptions = computed(() => {
  return localProduct.value.options && localProduct.value.options.length > 0 && localProduct.value.options.some(o => o.name && o.values.length > 0);
});

// Methods
function formatCurrency(value) {
  if (value === null || value === undefined) return ''
  return new Intl.NumberFormat('fr-TN', { style: 'currency', currency: 'TND' }).format(value)
}

function initializeProduct() {
  const baseProduct = {
    title: '',
    description: '',
    status: 'draft',
    type: '',
    onlineStore: true,
    pointOfSale: false,
    media: [],
    price: null,
    compareAtPrice: null,
    costPerItem: null,
    taxable: false,
    trackQuantity: true,
    quantity: 0,
    continueSelling: false,
    sku: '',
    options: [],
    variants: [],
    vendor: '',
    collections: [],
    tags: [],
    seoTitle: '',
    seoDescription: '',
  };

  // Deep merge props.product into baseProduct
  const product = JSON.parse(JSON.stringify({ ...baseProduct, ...props.product }));
  
  // Ensure options and variants are arrays
  if (!Array.isArray(product.options)) {
    product.options = [];
  }
  if (!Array.isArray(product.variants)) {
    product.variants = [];
  }

  // Initialize hasSkuOrBarcode based on existing data
  if (product.sku || product.barcode) {
    hasSkuOrBarcode.value = true;
  }

  return product;
}

function calculateProfitMargin() {
  // Calcul automatique déjà géré par les computed properties
}

function updateProgress() {
  // Le calcul de progression est déjà géré par la computed property
}

function previewProduct() {
  // Implémentation de la prévisualisation
  console.log('Preview product:', localProduct.value)
}

async function saveAsDraft() {
  draftLoading.value = true
  try {
    localProduct.value.status = 'draft'
    await validateAndSave()
  } finally {
    draftLoading.value = false
  }
}

async function validateAndSave() {
  const isValid = await v$.value.$validate()
  if (!isValid) {
    // Afficher les erreurs et aller au premier onglet avec erreur
    const firstError = v$.value.$errors[0]
    console.error('Validation error:', firstError.$message)
    return
  }

  saveLoading.value = true
  try {
    await emit('save', localProduct.value)
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
  localProduct.value = initializeProduct()
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

// Media Handling
function triggerFileInput() {
  fileInput.value.click()
}

function handleFileSelect(event) {
  const files = Array.from(event.target.files)
  if (files.length === 0) return

  const newMedia = files.map(file => ({
    id: Date.now() + Math.random().toString(36).substr(2, 9),
    file,
    preview: URL.createObjectURL(file),
    type: file.type.startsWith('image') ? 'image' : 'video',
    size: file.size,
    alt: ''
  }))

  localProduct.value.media = [...localProduct.value.media, ...newMedia]
  updateProgress()
  event.target.value = '' // Reset input
}

function handleDragOver(e) {
  e.preventDefault()
  isDragOver.value = true
}

function handleDragLeave() {
  isDragOver.value = false
}

function handleDrop(e) {
  e.preventDefault()
  isDragOver.value = false
  if (e.dataTransfer.files.length > 0) {
    handleFileSelect({ target: { files: e.dataTransfer.files } })
  }
}

function setFeaturedMedia(index) {
  const media = [...localProduct.value.media]
  const [featured] = media.splice(index, 1)
  localProduct.value.media = [featured, ...media]
}

function editMedia(index) {
  // Implémenter l'édition des médias
  console.log('Edit media at index:', index)
}

function removeMedia(index) {
  localProduct.value.media.splice(index, 1)
  updateProgress()
}

function formatFileSize(bytes) {
  if (bytes === 0) return '0 Bytes'
  const k = 1024
  const sizes = ['Bytes', 'KB', 'MB', 'GB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i]
}

// Inventory Management
function getInventoryStatus(quantity) {
  if (quantity === null || quantity === undefined) {
    return { label: t('unknown'), severity: 'info' }
  }
  if (quantity <= 0) {
    return { label: t('outOfStock'), severity: 'danger' }
  }
  if (quantity <= 10) {
    return { label: t('lowStock'), severity: 'warning' }
  }
  return { label: t('inStock'), severity: 'success' }
}

// DEBUT: Gestion des variantes style Shopify
function addOption() {
  if (localProduct.value.options.length < 3) {
    localProduct.value.options.push({ id: Date.now(), name: '', values: [] });
  }
}

function removeOption(index) {
  localProduct.value.options.splice(index, 1);
  generateVariants();
}

function generateVariants() {
  const options = localProduct.value.options.filter(opt => opt.name && opt.values.length > 0);
  if (options.length === 0) {
    localProduct.value.variants = [];
    return;
  }

  const combinations = options.reduce((acc, option) => {
    if (acc.length === 0) {
      return option.values.map(value => ({ [option.name]: value }));
    }
    const newAcc = [];
    acc.forEach(existingCombo => {
      option.values.forEach(value => {
        newAcc.push({ ...existingCombo, [option.name]: value });
      });
    });
    return newAcc;
  }, []);

  localProduct.value.variants = combinations.map(combo => {
    // Try to find an existing variant to preserve price/stock
    const existing = localProduct.value.variants.find(v => 
      Object.keys(combo).every(key => combo[key] === v[key])
    );
    return {
      ...combo,
      price: existing?.price || localProduct.value.price || 0,
      stock: existing?.stock || 0,
      sku: existing?.sku || ''
    };
  });
}
// FIN: Gestion des variantes style Shopify

// SKU Generation
function generateSku() {
  const prefix = localProduct.value.title 
    ? localProduct.value.title.substring(0, 3).toUpperCase() 
    : 'SKU'
  const random = Math.floor(1000 + Math.random() * 9000)
  localProduct.value.sku = `${prefix}-${random}`
}

// Vendor Search
function searchVendors(event) {
  filteredVendors.value = vendorOptions.filter(vendor => 
    vendor.toLowerCase().includes(event.query.toLowerCase())
  )
}

// SEO Generation
function generateSeoData() {
  if (localProduct.value.title) {
    localProduct.value.seoTitle = localProduct.value.title.substring(0, 60)
    localProduct.value.seoDescription = `${localProduct.value.title} - ${t('buyNow')}`
  }
}

function generateSlug(text) {
  return text 
    ? text.toString().toLowerCase()
        .replace(/\s+/g, '-')
        .replace(/[^\w\-]+/g, '')
        .replace(/\-\-+/g, '-')
        .replace(/^-+/, '')
        .replace(/-+$/, '')
    : ''
}

// Watch for changes
watch(localProduct, () => {
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
.product-form-container {
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

:deep(.product-tabs) {
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

.editor-container {
  border: 1px solid #d1d5db;
  border-radius: 6px;
}

:deep(.product-editor) {
  .ql-toolbar {
    border-top-left-radius: 6px;
    border-top-right-radius: 6px;
    border-bottom: none;
  }
  
  .ql-container {
    border-bottom-left-radius: 6px;
    border-bottom-right-radius: 6px;
    min-height: 150px;
  }
}

.checkbox-group {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.availability-cards {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.checkbox-card {
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  padding: 1rem;
  transition: box-shadow 0.2s;
}

.checkbox-card:hover {
  box-shadow: 0 1px 3px rgba(0,0,0,0.05);
}

.checkbox-item {
  display: flex;
  align-items: flex-start;
  gap: 0.75rem;
}

.checkbox-label {
  display: flex;
  flex-direction: column;
  cursor: pointer;
}

.label-text {
  font-weight: 500;
  color: #374151;
  font-size: 0.875rem;
}

.label-description {
  color: #6b7280;
  font-size: 0.75rem;
  margin-top: 0.25rem;
}

.media-upload-container {
  margin-bottom: 2rem;
}

.dropzone-area {
  border: 2px dashed #d1d5db;
  border-radius: 8px;
  padding: 2rem;
  text-align: center;
  cursor: pointer;
  transition: all 0.2s;
  
  &:hover, &.dragover {
    border-color: #3b82f6;
    background-color: #f0f7ff;
  }
}

.file-input {
  display: none;
}

.dropzone-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
}

.dropzone-icon {
  font-size: 2rem;
  color: #9ca3af;
}

.dropzone-title {
  margin: 0;
  font-size: 1rem;
  color: #1f2937;
}

.dropzone-description {
  margin: 0;
  color: #6b7280;
  font-size: 0.875rem;
}

.browse-button {
  margin-top: 1rem;
}

.media-grid {
  margin-top: 1.5rem;
}

.media-items {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 1rem;
}

.media-item {
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  overflow: hidden;
  transition: all 0.2s;
  
  &:hover {
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  }
  
  &.featured {
    border: 2px solid #3b82f6;
  }
}

.media-preview {
  position: relative;
  padding-top: 100%;
  background-color: #f9fafb;
}

.media-thumbnail {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  object-fit: contain;
}

.video-thumbnail {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #000;
}

.video-icon {
  font-size: 2rem;
  color: #fff;
}

.media-actions {
  position: absolute;
  top: 0.5rem;
  right: 0.5rem;
  display: flex;
  gap: 0.25rem;
}

.featured-btn {
  color: #f59e0b !important;
}

.media-info {
  padding: 0.75rem;
}

.alt-input {
  width: 100%;
  margin-bottom: 0.5rem;
}

.media-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 0.75rem;
  color: #6b7280;
}

.file-size {
  color: #6b7280;
}

.featured-badge {
  font-size: 0.625rem;
}

.profit-display {
  background-color: #f9fafb;
  border-radius: 6px;
  padding: 0.75rem;
}

.profit-item {
  display: flex;
  justify-content: space-between;
  margin-bottom: 0.5rem;
  
  &:last-child {
    margin-bottom: 0;
  }
}

.profit-label {
  font-size: 0.875rem;
  color: #4b5563;
}

.profit-value {
  font-weight: 600;
  
  &.positive {
    color: #10b981;
  }
  
  &.negative {
    color: #ef4444;
  }
}

.inventory-grid {
  grid-template-columns: 1fr 1fr;
  align-items: end;
}

.continue-selling-field {
  padding-top: 2rem; /* Align with the input field label */
}

.input-with-button {
  position: relative;
  display: flex;
  align-items: center;
}

.input-with-button .field-input {
  padding-right: 2.5rem; /* space for the button */
}

.generate-sku-btn {
  position: absolute;
  right: 0.25rem;
  top: 50%;
  transform: translateY(-50%);
  height: 2rem;
  width: 2rem;
}

.inventory-status {
  margin-top: 0.5rem;
}

.inventory-section, .variants-section {
  margin-bottom: 2rem;
}

.section-divider {
  margin: 2rem 0;
  text-align: center;
  position: relative;
}

.section-divider::before {
  content: '';
  position: absolute;
  top: 50%;
  left: 0;
  right: 0;
  height: 1px;
  background-color: #e5e7eb;
  z-index: 1;
}

.divider-title {
  position: relative;
  z-index: 2;
  background-color: #fff;
  padding: 0 1rem;
  color: #6b7280;
  font-weight: 500;
  font-size: 0.875rem;
}

.generate-btn {
  margin-top: 0.25rem;
}

.option-card {
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  padding: 1rem;
  margin-bottom: 1rem;
  background-color: #f9fafb;
}

.option-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.option-title {
  margin: 0;
  font-size: 1rem;
  font-weight: 600;
  color: #1f2937;
}

.option-body {
  display: grid;
  grid-template-columns: 1fr 2fr;
  gap: 1rem;
  align-items: flex-start;
}

.variants-table-container {
  margin-top: 1.5rem;
}

.variants-preview-title {
  margin-bottom: 1rem;
  font-size: 1.125rem;
  font-weight: 600;
}

/* FIN: Styles pour la gestion dynamique des variantes */

.variants-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.variants-title {
  margin: 0;
  font-size: 1rem;
  color: #1f2937;
}

.empty-variants {
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

:deep(.editable-cells-table) {
  .p-datatable-tbody > tr > td {
    padding: 0.5rem 0.75rem;
  }
  
  .p-inputtext, .p-inputnumber {
    width: 100%;
  }
}

.seo-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.seo-title {
  margin: 0;
  font-size: 1rem;
  color: #1f2937;
}

.seo-fields {
  display: grid;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.seo-preview {
  margin-top: 2rem;
}

.preview-title {
  margin: 0 0 0.75rem 0;
  font-size: 0.875rem;
  color: #6b7280;
  font-weight: 500;
}

.preview-card {
  border: 1px solid #e5e7eb;
  border-radius: 6px;
  padding: 1rem;
  max-width: 600px;
}

.preview-url {
  color: #3b82f6;
  font-size: 0.75rem;
  margin-bottom: 0.25rem;
}

.preview-link {
  color: #1a0dab;
  font-size: 1.125rem;
  margin-bottom: 0.25rem;
}

.preview-description {
  color: #4d5156;
  font-size: 0.875rem;
  line-height: 1.4;
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

/* Animations */
.media-move {
  transition: transform 0.3s ease;
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