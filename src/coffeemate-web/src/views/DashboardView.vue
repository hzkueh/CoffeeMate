<template>
  <div class="max-w-5xl mx-auto px-4 py-10">

    <!-- Welcome -->
    <div class="mb-10">
      <h1 class="text-3xl font-bold text-stone-800">
        {{ greeting }}, {{ auth.user?.displayName }} ☕
      </h1>
      <p class="text-stone-500 mt-1">What are you brewing today?</p>
    </div>

    <!-- Quick actions -->
    <section class="mb-12">
      <h2 class="text-sm font-semibold text-stone-400 uppercase tracking-widest mb-4">
        Quick actions
      </h2>
      <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">

        <RouterLink
          to="/coffees"
          class="group flex items-start gap-4 bg-white rounded-2xl border border-stone-200 p-6
                 hover:border-amber-400 hover:shadow-md transition-all"
        >
          <span class="text-3xl">📖</span>
          <div>
            <p class="font-semibold text-stone-800 group-hover:text-amber-700 transition-colors">
              Browse Coffees
            </p>
            <p class="text-sm text-stone-500 mt-0.5">
              Explore recipes and step-by-step brewing guides.
            </p>
          </div>
        </RouterLink>

        <div
          class="flex items-start gap-4 bg-white rounded-2xl border border-stone-200 p-6
                 opacity-60 cursor-not-allowed"
        >
          <span class="text-3xl">🤝</span>
          <div>
            <p class="font-semibold text-stone-800">
              Start a Brew Session
              <span class="ml-2 text-xs font-medium bg-amber-100 text-amber-700 px-2 py-0.5 rounded-full">
                Coming soon
              </span>
            </p>
            <p class="text-sm text-stone-500 mt-0.5">
              Invite others and brew together in real time.
            </p>
          </div>
        </div>

      </div>
    </section>

    <!-- Featured coffees -->
    <section>
      <div class="flex items-center justify-between mb-4">
        <h2 class="text-sm font-semibold text-stone-400 uppercase tracking-widest">
          Featured coffees
        </h2>
        <RouterLink
          to="/coffees"
          class="text-sm font-medium text-amber-600 hover:text-amber-700 transition-colors"
        >
          View all →
        </RouterLink>
      </div>

      <div v-if="store.loading" class="text-stone-400 text-sm">Loading...</div>
      <div v-else-if="store.error" class="text-red-500 text-sm">{{ store.error }}</div>

      <div v-else class="grid grid-cols-1 sm:grid-cols-3 gap-4">
        <RouterLink
          v-for="coffee in featured"
          :key="coffee.id"
          :to="`/coffees/${coffee.id}`"
          class="block bg-white rounded-2xl border border-stone-200 p-5
                 hover:border-amber-400 hover:shadow-md transition-all"
        >
          <h3 class="font-semibold text-stone-800 mb-1">{{ coffee.name }}</h3>
          <p class="text-xs text-stone-400 mb-2">{{ coffee.origin }}</p>
          <p class="text-sm text-stone-500 line-clamp-2">{{ coffee.description }}</p>
        </RouterLink>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { RouterLink } from 'vue-router'
import { useAuthStore } from '../stores/authStore'
import { useCoffeeStore } from '../stores/coffeeStore'

const auth = useAuthStore()
const store = useCoffeeStore()

const greeting = computed(() => {
  const hour = new Date().getHours()
  if (hour < 12) return 'Good morning'
  if (hour < 18) return 'Good afternoon'
  return 'Good evening'
})

const featured = computed(() => store.coffees.slice(0, 3))

onMounted(() => store.fetchAll())
</script>
