<template>
  <div class="min-h-screen bg-stone-100 flex items-center justify-center px-4">
    <div class="w-full max-w-md">

      <!-- Branding -->
      <div class="text-center mb-8">
        <span class="text-5xl">☕</span>
        <h1 class="mt-3 text-3xl font-bold text-stone-800 tracking-tight">CoffeeMate</h1>
        <p class="mt-1 text-stone-500 text-sm">Create your account</p>
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

          <!-- Display name -->
          <div class="mb-4">
            <label for="displayName" class="block text-sm font-medium text-stone-700 mb-1">
              Display name
            </label>
            <input
              id="displayName"
              v-model="form.displayName"
              type="text"
              autocomplete="nickname"
              placeholder="Your barista name"
              class="w-full rounded-lg border border-stone-300 px-4 py-2.5 text-sm text-stone-800 placeholder-stone-400
                     focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent
                     disabled:opacity-50"
              :disabled="loading"
            />
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
          <div class="mb-2">
            <label for="password" class="block text-sm font-medium text-stone-700 mb-1">
              Password
            </label>
            <input
              id="password"
              v-model="form.password"
              type="password"
              autocomplete="new-password"
              placeholder="••••••••"
              class="w-full rounded-lg border border-stone-300 px-4 py-2.5 text-sm text-stone-800 placeholder-stone-400
                     focus:outline-none focus:ring-2 focus:ring-amber-500 focus:border-transparent
                     disabled:opacity-50"
              :disabled="loading"
            />
            <p class="mt-1.5 text-xs text-stone-400">
              Min. 8 characters, include uppercase and a special character.
            </p>
          </div>

          <!-- Confirm Password -->
          <div class="mb-6">
            <label for="confirmPassword" class="block text-sm font-medium text-stone-700 mb-1">
              Confirm password
            </label>
            <input
              id="confirmPassword"
              v-model="form.confirmPassword"
              type="password"
              autocomplete="new-password"
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
            <span v-if="loading">Creating account…</span>
            <span v-else>Create account</span>
          </button>

        </form>
      </div>

      <!-- Footer link -->
      <p class="mt-5 text-center text-sm text-stone-500">
        Already have an account?
        <RouterLink to="/login" class="font-medium text-amber-600 hover:text-amber-700">
          Sign in
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

const form = ref({
  displayName: '',
  email: '',
  password: '',
  confirmPassword: '',
})
const loading = ref(false)
const errorMessage = ref('')

async function handleSubmit() {
  errorMessage.value = ''

  if (!form.value.displayName || !form.value.email || !form.value.password || !form.value.confirmPassword) {
    errorMessage.value = 'Please fill in all fields.'
    return
  }

  if (form.value.password !== form.value.confirmPassword) {
    errorMessage.value = 'Passwords do not match.'
    return
  }

  loading.value = true
  try {
    await auth.register({
      displayName: form.value.displayName,
      email: form.value.email,
      password: form.value.password,
    })
    router.push('/dashboard')
  } catch (err) {
    if (axios.isAxiosError(err)) {
      errorMessage.value =
        err.response?.data?.message ?? 'Registration failed. Please try again.'
    } else {
      errorMessage.value = 'An unexpected error occurred.'
    }
  } finally {
    loading.value = false
  }
}
</script>
