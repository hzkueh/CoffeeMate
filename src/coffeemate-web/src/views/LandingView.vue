<template>
  <div class="flex flex-col">

    <!-- Hero -->
    <section class="flex-1 flex flex-col items-center justify-center px-4 text-center py-24">
      <span class="text-7xl mb-6">☕</span>
      <h1 class="text-4xl sm:text-5xl font-bold text-stone-800 tracking-tight leading-tight mb-4">
        Brew better,<br />together.
      </h1>
      <p class="text-stone-500 text-lg max-w-md mb-10">
        Explore coffee recipes, follow step-by-step brewing guides, and collaborate
        with fellow coffee lovers in real time.
      </p>
      <div class="flex flex-col sm:flex-row gap-3">
        <RouterLink
          to="/register"
          class="px-6 py-3 rounded-lg bg-amber-600 hover:bg-amber-700 text-white font-semibold text-sm transition-colors"
        >
          Create an account
        </RouterLink>
        <RouterLink
          to="/coffees"
          class="px-6 py-3 rounded-lg bg-white hover:bg-stone-50 text-stone-700 font-semibold text-sm border border-stone-300 transition-colors"
        >
          Browse coffees
        </RouterLink>
      </div>
    </section>

    <!-- Feature strip -->
    <section class="bg-white border-t border-stone-200 px-6 py-10">
      <div class="max-w-4xl mx-auto grid grid-cols-1 sm:grid-cols-3 gap-8 text-center">
        <div>
          <div class="text-3xl mb-3">📖</div>
          <h3 class="font-semibold text-stone-800 mb-1">Recipe guides</h3>
          <p class="text-sm text-stone-500">Step-by-step instructions for every brewing method.</p>
        </div>
        <div>
          <div class="text-3xl mb-3">🌍</div>
          <h3 class="font-semibold text-stone-800 mb-1">Origins &amp; flavours</h3>
          <p class="text-sm text-stone-500">Discover coffees from around the world and their stories.</p>
        </div>
        <div>
          <div class="text-3xl mb-3">🤝</div>
          <h3 class="font-semibold text-stone-800 mb-1">Brew sessions</h3>
          <p class="text-sm text-stone-500">Invite others to join a live session and brew in sync.</p>
        </div>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>
