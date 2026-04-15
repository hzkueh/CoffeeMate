<template>
  <div class="min-h-screen bg-stone-100 flex items-center justify-center px-4">
    <div class="w-full max-w-md">

      <!-- Branding -->
      <div class="text-center mb-8">
        <span class="text-5xl">☕</span>
        <h1 class="mt-3 text-3xl font-bold text-stone-800 tracking-tight">CoffeeMate</h1>
        <p class="mt-1 text-stone-500 text-sm">Sign in to your account</p>
      </div>

      <!-- Card -->
      <div class="bg-white rounded-2xl shadow-sm border border-stone-200 p-8">
        <form @submit.prevent="handleSubmit" novalidate>

          <!-- Error banner -->
          <div
            v-if="errorMessage"
            class="mb-5 rounded-lg bg-red-50 border border-red-200 px-4 py-3 text-sm text-red-700"
          >
            {{ errorMessage }}
          </div>

          <!-- Email -->
          <div class="mb-4">
            <label for="email" class="block text-sm font-medium text-stone-700 mb-1">
              Email address
            </label>
            <input
              id="email"
              v-model="form.email"
              type="email"
              autocomplete="email"
              placeholder="you@example.com"
              class="w-full rounded-lg border border-stone-300 px-4 py-2.5 text-sm text-stone-800 placeholder-stone-400
                     focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent
                     disabled:opacity-50"
              :disabled="loading"
            />
          </div>

          <!-- Password -->
          <div class="mb-6">
            <label for="password" class="block text-sm font-medium text-stone-700 mb-1">
              Password
            </label>
            <input
              id="password"
              v-model="form.password"
              type="password"
              autocomplete="current-password"
              placeholder="••••••••"
              class="w-full rounded-lg border border-stone-300 px-4 py-2.5 text-sm text-stone-800 placeholder-stone-400
                     focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent
                     disabled:opacity-50"
              :disabled="loading"
            />
          </div>

          <!-- Submit -->
          <button
            type="submit"
            class="w-full rounded-lg bg-amber-600 hover:bg-amber-700 active:bg-amber-800
                   text-white font-semibold py-2.5 text-sm
                   transition-colors duration-150
                   disabled:opacity-60 disabled:cursor-not-allowed"
            :disabled="loading"
          >
            <span v-if="loading">Signing in…</span>
            <span v-else>Sign in</span>
          </button>

        </form>
      </div>

      <!-- Footer link -->
      <p class="mt-5 text-center text-sm text-stone-500">
        Don't have an account?
        <RouterLink to="/register" class="font-medium text-amber-600 hover:text-amber-700">
          Create one
        </RouterLink>
      </p>

    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter, RouterLink } from 'vue-router'
import axios from 'axios'
import { useAuthStore } from '../stores/authStore'

const auth = useAuthStore()
const router = useRouter()

const form = ref({ email: '', password: '' })
const loading = ref(false)
const errorMessage = ref('')

async function handleSubmit() {
  errorMessage.value = ''

  if (!form.value.email || !form.value.password) {
    errorMessage.value = 'Please fill in all fields.'
    return
  }

  loading.value = true
  try {
    await auth.login(form.value)
    router.push('/dashboard')
  } catch (err) {
    if (axios.isAxiosError(err)) {
      errorMessage.value =
        err.response?.data?.message ?? 'Login failed. Please try again.'
    } else {
      errorMessage.value = 'An unexpected error occurred.'
    }
  } finally {
    loading.value = false
  }
}
</script>
