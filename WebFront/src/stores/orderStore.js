import { defineStore } from 'pinia';

export const useOrderStore = defineStore('orders', {
  state: () => ({
    orders: [
      {
        id: 1,
        orderNumber: 'ORD-001',
        customer: 'John Doe',
        date: '2025-06-15T10:00:00Z',
        total: 150.0,
        status: 'Pending',
        items: [
          { productId: 1, name: 'Ligne', quantity: 2, price: 75.0 }
        ],
        shippingAddress: {
          street: '123 Main St',
          city: 'Tunis',
          postalCode: '1000',
          country: 'Tunisia'
        },
        notes: ''
      }
    ],
    newOrder: {
      id: null,
      orderNumber: '',
      customer: '',
      date: new Date().toISOString(),
      total: 0.0,
      status: 'Pending',
      items: [],
      shippingAddress: {
        street: '',
        city: '',
        postalCode: '',
        country: ''
      },
      notes: ''
    }
  }),
  actions: {
    async fetchOrders() {
      try {
        // Simulated API call
        return this.orders;
      } catch (error) {
        throw new Error('Failed to fetch orders');
      }
    },

    // addOrder(order) {
    //   this.newOrder.id = Date.now();
    //   this.newOrder.orderNumber = this.generateOrderNumber();
    //   this.orders.push({ ...this.newOrder, ...order });
    //   this.resetNewOrder();
    // },

//

 addOrder(orderFromForm) {
  
      // 1. Créez une copie de l'objet venant du formulaire pour éviter les mutations inattendues.
      const newOrder = { ...orderFromForm };

      // 2. Assignez directement les valeurs que le store est censé générer.
      newOrder.id = Date.now(); // Génère un nouvel ID.
      newOrder.orderNumber = this.generateOrderNumber(); // Génère et assigne le numéro de commande.

      // 3. Assurez-vous que le statut par défaut est bien "Pending" si rien n'est fourni.
      if (!newOrder.status) {
        newOrder.status = 'Pending';
      }
      
      // 4. Ajoutez le nouvel objet complet au tableau des commandes.
      this.orders.push(newOrder);

      // La méthode resetNewOrder() n'est plus nécessaire ici car nous n'utilisons plus this.newOrder.
    },
//

    updateOrder(updatedOrder) {
      const index = this.orders.findIndex(o => o.id === updatedOrder.id);
      if (index !== -1) this.orders.splice(index, 1, updatedOrder);
    },
    deleteOrder(id) {
      this.orders = this.orders.filter(o => o.id !== id);
    },
    bulkDeleteOrders(ids) {
      this.orders = this.orders.filter(o => !ids.includes(o.id));
    },
    async exportOrders() {
      try {
        // Simulated export logic
        console.log('Exporting filtered orders:', this.orders);
        return true;
      } catch (error) {
        throw new Error('Failed to export orders');
      }
    },
    async exportSelectedOrders(orders) {
      try {
        // Simulated export logic for selected orders
        console.log('Exporting selected orders:', orders);
        return true;
      } catch (error) {
        throw new Error('Failed to export selected orders');
      }
    },
    async exportAllOrders() {
      try {
        // Simulated export all logic
        console.log('Exporting all orders:', this.orders);
        return true;
      } catch (error) {
        throw new Error('Failed to export all orders');
      }
    },
    resetNewOrder() {
      this.newOrder = {
        id: null,
        orderNumber: '',
        customer: '',
        date: new Date().toISOString(),
        total: 0.0,
        status: 'Pending',
        items: [],
        shippingAddress: {
          street: '',
          city: '',
          postalCode: '',
          country: ''
        },
        notes: ''
      };
    },
    generateOrderNumber() {
      const prefix = 'ORD';
      const number = (this.orders.length + 1).toString().padStart(3, '0');
      return `${prefix}-${number}`;
    }
  }
});