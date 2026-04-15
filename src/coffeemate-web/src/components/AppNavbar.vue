<template>
  <nav class="bg-white border-b border-stone-200 px-6 py-4 flex items-center justify-between">

    <!-- Logo -->
    <RouterLink to="/" class="flex items-center gap-2">
      <span class="text-2xl">☕</span>
      <span class="text-lg font-bold text-stone-800 tracking-tight">CoffeeMate</span>
    </RouterLink>

    <!-- Links -->
    <div class="flex items-center gap-3">

      <RouterLink
        to="/coffees"
        class="text-sm font-medium text-stone-600 hover:text-stone-900 transition-colors px-2 py-1"
        active-class="text-amber-600"
      >
        Browse Coffees
      </RouterLink>

      <!-- Guest -->
      <template v-if="!auth.isAuthenticated">
        <RouterLink
          to="/login"
          class="text-sm font-medium text-stone-600 hover:text-stone-900 transition-colors px-2 py-1"
        >
          Sign in
        </RouterLink>
        <RouterLink
          to="/register"
          class="text-sm font-semibold bg-amber-600 hover:bg-amber-700 text-white px-4 py-2 rounded-lg transition-colors"
        >
          Get started
        </RouterLink>
      </template>

      <!-- Authenticated -->
      <template v-else>
        <span class="text-sm text-stone-600 px-2">
          👋 {{ auth.user?.displayName }}
        </span>
        <button
          @click="handleLogout"
          class="text-sm font-medium text-stone-600 hover:text-stone-900 transition-colors px-2 py-1"
        >
          Sign out
        </button>
      </template>

    </div>
  </nav>
</template>

<script setup lang="ts">
import { RouterLink, useRouter } from 'vue-router'
import { useAuthStore } from '../stores/authStore'

const auth = useAuthStore()
const router = useRouter()

function handleLogout() {
  auth.logout()
  router.push('/')
}
</script>
