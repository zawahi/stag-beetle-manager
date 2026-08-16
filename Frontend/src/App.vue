<script setup lang="ts">
import { computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()
const isLoggedIn = computed(() => !!localStorage.getItem('token'))

const logout = () => {
  localStorage.removeItem('token')
  window.location.href = '/login'
}

const isActive = (path: string) => route.path === path
</script>

<template>
  <!-- ログイン済みの場合のレイアウト -->
  <div class="d-flex vh-100 overflow-hidden" v-if="isLoggedIn">
    <!-- サイドバー -->
    <div class="bg-dark text-white p-3 d-flex flex-column" style="width: 260px;">
      <div class="mb-4 mt-2 text-center">
        <h4 class="fw-bold m-0">Kuwagata App</h4>
      </div>
      <hr class="text-secondary">
      <ul class="nav nav-pills flex-column mb-auto gap-2">
        <li class="nav-item">
          <router-link to="/" class="nav-link text-white" :class="{ 'active': route.path === '/' }">
            ダッシュボード
          </router-link>
        </li>
        <li class="nav-item">
          <router-link to="/beetles" class="nav-link text-white" :class="{ 'active': route.path.startsWith('/beetle') }">
            生体・幼虫管理
          </router-link>
        </li>
        <li class="nav-item">
          <router-link to="/breeding" class="nav-link text-white" :class="{ 'active': route.path.startsWith('/breeding') }">
            ペアリング記録
          </router-link>
        </li>
      </ul>
      <hr class="text-secondary">
      <button class="btn btn-outline-light w-100 fw-bold" @click="logout">ログアウト</button>
    </div>

    <!-- メインコンテンツ -->
    <div class="flex-grow-1 p-4 bg-light overflow-auto">
      <div class="container-fluid" style="max-width: 1200px;">
        <router-view />
      </div>
    </div>
  </div>

  <!-- 未ログイン（ログイン画面など）の場合 -->
  <div v-else>
    <router-view />
  </div>
</template>

<style>
/* Active state customization for sidebar */
.nav-pills .nav-link.active {
  background-color: #198754; /* Bootstrap Success Color */
}
</style>
