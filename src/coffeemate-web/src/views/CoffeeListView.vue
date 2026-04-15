<template>
  <div class="max-w-5xl mx-auto px-4 py-10">
    <h1 class="text-3xl font-bold text-stone-800 mb-8">Coffee Explorer</h1>

    <div v-if="store.loading" class="text-stone-500">Loading coffees...</div>
    <div v-else-if="store.error" class="text-red-500">{{ store.error }}</div>

    <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      <RouterLink
        v-for="coffee in store.coffees"
        :key="coffee.id"
        :to="`/coffees/${coffee.id}`"
        class="block bg-white rounded-2xl shadow hover:shadow-md transition p-6 border border-stone-100"
      >
        <h2 class="text-xl font-semibold text-stone-800 mb-1">{{ coffee.name }}</h2>
        <p class="text-sm text-stone-400 mb-3">{{ coffee.origin }}</p>
        <p class="text-stone-600 text-sm line-clamp-3">{{ coffee.description }}</p>
      </RouterLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { RouterLink } from 'vue-router'
import { useCoffeeStore } from '../stores/coffeeStore'

const store = useCoffeeStore()
onMounted(() => store.fetchAll())
</script>
