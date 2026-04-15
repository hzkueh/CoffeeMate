<template>
  <div class="max-w-3xl mx-auto px-4 py-10">
    <RouterLink to="/coffees" class="text-sm text-amber-600 hover:underline mb-6 inline-block">
      &larr; Back to Coffee Explorer
    </RouterLink>

    <div v-if="store.loading" class="text-stone-500">Loading...</div>
    <div v-else-if="store.error" class="text-red-500">{{ store.error }}</div>

    <div v-else-if="store.selectedCoffee">
      <h1 class="text-3xl font-bold text-stone-800 mb-1">{{ store.selectedCoffee.name }}</h1>
      <p class="text-sm text-stone-400 mb-4">{{ store.selectedCoffee.origin }}</p>
      <p class="text-stone-600 mb-8">{{ store.selectedCoffee.description }}</p>

      <div v-if="store.selectedCoffee.recipe">
        <h2 class="text-xl font-semibold text-stone-800 mb-1">
          {{ store.selectedCoffee.recipe.title }}
        </h2>
        <p v-if="store.selectedCoffee.recipe.description" class="text-stone-500 text-sm mb-6">
          {{ store.selectedCoffee.recipe.description }}
        </p>

        <ol class="space-y-4">
          <li
            v-for="step in store.selectedCoffee.recipe.steps"
            :key="step.id"
            class="flex gap-4 bg-white rounded-xl border border-stone-100 shadow-sm p-4"
          >
            <span class="flex-shrink-0 w-8 h-8 rounded-full bg-amber-100 text-amber-700 font-bold flex items-center justify-center text-sm">
              {{ step.order }}
            </span>
            <div>
              <p class="font-medium text-stone-800">{{ step.name }}</p>
              <p class="text-stone-500 text-sm mt-1">{{ step.description }}</p>
              <p class="text-xs text-stone-400 mt-1">{{ step.durationSeconds }}s</p>
            </div>
          </li>
        </ol>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRoute, RouterLink } from 'vue-router'
import { useCoffeeStore } from '../stores/coffeeStore'

const route = useRoute()
const store = useCoffeeStore()
onMounted(() => store.fetchById(route.params.id as string))
</script>
