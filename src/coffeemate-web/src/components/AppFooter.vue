<template>
  <footer class="bg-white border-t border-stone-200 py-6 text-center text-xs text-stone-400">
    © {{ year }} CoffeeMate. Built with Vue 3 &amp; ASP.NET Core.
  </footer>
</template>

<script setup lang="ts">
const year = new Date().getFullYear()
</script>
