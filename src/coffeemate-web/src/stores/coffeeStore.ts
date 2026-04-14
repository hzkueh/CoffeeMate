import { defineStore } from 'pinia'
import { ref } from 'vue'
import apiClient from '../api/axios'
import type { CoffeeListDto, CoffeeDetailDto } from '../types/coffee'

export const useCoffeeStore = defineStore('coffee', () => {
  const coffees = ref<CoffeeListDto[]>([])
  const selectedCoffee = ref<CoffeeDetailDto | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchAll() {
    loading.value = true
    error.value = null
    try {
      const { data } = await apiClient.get<CoffeeListDto[]>('/coffee')
      coffees.value = data
    } catch {
      error.value = 'Failed to load coffees.'
    } finally {
      loading.value = false
    }
  }

  async function fetchById(id: string) {
    loading.value = true
    error.value = null
    selectedCoffee.value = null
    try {
      const { data } = await apiClient.get<CoffeeDetailDto>(`/coffee/${id}`)
      selectedCoffee.value = data
    } catch {
      error.value = 'Coffee not found.'
    } finally {
      loading.value = false
    }
  }

  return { coffees, selectedCoffee, loading, error, fetchAll, fetchById }
})
