import { createRouter, createWebHistory } from 'vue-router'
import CoffeeListView from '../views/CoffeeListView.vue'
import CoffeeDetailView from '../views/CoffeeDetailView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', component: CoffeeListView },
    { path: '/coffee/:id', component: CoffeeDetailView },
  ],
})

export default router
