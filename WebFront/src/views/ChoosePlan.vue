<template>
  <div class="pick-plan-page">
    <div class="page-header">
      <Button icon="pi pi-arrow-left" text rounded @click="goBack" v-tooltip.bottom="$t('back')" />
      <h1 class="page-title">{{ $t('pickYourPlan') }}</h1>
      <Button icon="pi pi-times" text rounded @click="close" class="close-button" v-tooltip.bottom="$t('close')" />
    </div>

    <div class="header-features">
      <span>✓ {{ $t('worldsBestCheckout') }}</span>
      <span>✓ {{ $t('sellOnlineAndInPerson') }}</span>
      <span>✓ {{ $t('chatSupport247') }}</span>
      <span>✓ {{ $t('over13000Apps') }}</span>
    </div>

    <div class="plan-selection-grid">
      <Card v-for="plan in mainPlans" :key="plan.name" :class="['plan-card', { popular: plan.popular }]">
        <template #header v-if="plan.popular">
          <div class="popular-badge">{{ $t('mostPopular') }}</div>
        </template>
        <template #title>
          <div class="plan-title">
            <i :class="plan.icon"></i>
            <span>{{ $t(plan.name) }}</span>
          </div>
        </template>
        <template #subtitle>{{ $t(plan.subtitle) }}</template>
        <template #content>
          <div class="price">
            <span class="price-currency">{{ plan.price.currency }}</span>
            <span class="price-amount">{{ plan.price.amount }}</span>
            <span class="price-period">/{{ $t('month') }}</span>
          </div>
          <p class="price-promo">{{ $t(plan.price.promo) }}</p>
          <Button :label="`${$t('select')} ${$t(plan.name)}`" class="w-full select-button" :outlined="!plan.popular" />
          <ul class="features-list">
            <li v-for="feature in plan.features" :key="feature">
              <i class="pi pi-check"></i>
              <span>{{ $t(feature) }}</span>
            </li>
          </ul>
        </template>
      </Card>
    </div>
    
    <!-- Comparison table and other plans can be added here later -->

  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useI18n } from 'vue-i18n';
import Card from 'primevue/card';
import Button from 'primevue/button';

const router = useRouter();
const { t } = useI18n();

const mainPlans = ref([
  { name: 'planRetail', icon: 'pi pi-store', subtitle: 'subtitleRetail', price: { currency: 'USD', amount: '89', promo: 'promoForFirst3Months' }, popular: false, features: ['featureRetail1', 'featureRetail2', 'featureRetail3', 'featureRetail4'] },
  { name: 'planBasic', icon: 'pi pi-user', subtitle: 'subtitleBasic', price: { currency: 'USD', amount: '27', promo: 'promoForFirst3Months' }, popular: true, features: ['featureBasic1', 'featureBasic2', 'featureBasic3', 'featureBasic4'] },
  { name: 'planGrow', icon: 'pi pi-users', subtitle: 'subtitleGrow', price: { currency: 'USD', amount: '72', promo: 'promoForFirst3Months' }, popular: false, features: ['featureGrow1', 'featureGrow2', 'featureGrow3', 'featureGrow4', 'featureGrow5'] },
  { name: 'planAdvanced', icon: 'pi pi-chart-line', subtitle: 'subtitleAdvanced', price: { currency: 'USD', amount: '399', promo: 'promoForFirst3Months' }, popular: false, features: ['featureAdvanced1', 'featureAdvanced2', 'featureAdvanced3', 'featureAdvanced4', 'featureAdvanced5', 'featureAdvanced6', 'featureAdvanced7'] }
]);

const goBack = () => router.back();
const close = () => router.push('/settings');

</script>

<style scoped>
.pick-plan-page {
  background-color: #ffffff;
  padding: 1.5rem;
  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif, "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol";
}
.page-header {
  display: flex;
  align-items: center;
  max-width: 1200px;
  margin: 0 auto 2rem auto;
}
.page-title {
  font-size: 1.5rem;
  font-weight: 600;
  text-align: center;
  flex-grow: 1;
}
.close-button {
  margin-left: auto;
}
.header-features {
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  gap: 2rem;
  margin-bottom: 3rem;
  color: #4b5563;
  font-size: 0.875rem;
}
.plan-selection-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
  gap: 1.5rem;
  max-width: 1200px;
  margin: 0 auto 3rem auto;
}
.plan-card {
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.05);
  position: relative;
  padding: 1rem;
}
.plan-card.popular {
  border: 2px solid #3b82f6;
}
.popular-badge {
  position: absolute;
  top: -15px;
  left: 50%;
  transform: translateX(-50%);
  background-color: #3b82f6;
  color: white;
  padding: 0.25rem 0.75rem;
  border-radius: 16px;
  font-size: 0.75rem;
  font-weight: 500;
}
.plan-title {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 600;
}
.price {
  margin-bottom: 0.25rem;
  display: flex;
  align-items: baseline;
  gap: 0.25rem;
}
.price-currency {
  font-size: 1.5rem;
  font-weight: 600;
}
.price-amount {
  font-size: 2.5rem;
  font-weight: 600;
}
.price-period {
  color: #6b7280;
  font-size: 1rem;
}
.price-promo {
  color: #6b7280;
  font-size: 0.875rem;
  margin-bottom: 1.5rem;
}
.select-button {
  margin-bottom: 1.5rem;
}
.features-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 1rem;
  font-size: 0.875rem;
}
.features-list li {
  display: flex;
  align-items: flex-start;
  gap: 0.75rem;
}
.features-list .pi-check {
  color: #10b981;
  margin-top: 3px;
}
</style>
