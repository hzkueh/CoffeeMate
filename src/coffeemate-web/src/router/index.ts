import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/authStore'
import AppLayout from '../layouts/AppLayout.vue'
import LandingView from '../views/LandingView.vue'
import DashboardView from '../views/DashboardView.vue'
import CoffeeListView from '../views/CoffeeListView.vue'
import CoffeeDetailView from '../views/CoffeeDetailView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    // Routes that share AppLayout (navbar + footer)
    {
      path: '/',
      component: AppLayout,
      children: [
        { path: '',          component: LandingView },
        { path: 'dashboard', component: DashboardView, meta: { requiresAuth: true } },
        { path: 'coffees',   component: CoffeeListView },
        { path: 'coffees/:id', component: CoffeeDetailView },
      ],
    },

    // Standalone full-page routes (own layout)
    { path: '/login',    component: LoginView,    meta: { guestOnly: true } },
    { path: '/register', component: RegisterView, meta: { guestOnly: true } },
  ],
})

router.beforeEach((to) => {
  const auth = useAuthStore()

  // Authenticated users cannot visit guest-only or the landing page → go to dashboard
  if (auth.isAuthenticated && (to.meta.guestOnly || to.path === '/')) {
    return '/dashboard'
  }

  // Unauthenticated users cannot visit protected pages → go to login
  if (!auth.isAuthenticated && to.meta.requiresAuth) {
    return '/login'
  }
})

export default router
