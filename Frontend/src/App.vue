<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()
const isLoggedIn = computed(() => !!localStorage.getItem('token'))

const showLogoutModal = ref(false)

const executeLogout = () => {
  localStorage.removeItem('token')
  window.location.href = '/login'
}
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
      <button class="btn btn-outline-light w-100 fw-bold" @click="showLogoutModal = true">ログアウト</button>
    </div>

    <!-- メインコンテンツ -->
    <div class="flex-grow-1 p-4 bg-light overflow-auto">
      <div class="container-fluid" style="max-width: 1200px;">
        <router-view />
      </div>
    </div>

    <!-- ログアウト確認モーダル -->
    <div v-if="showLogoutModal" class="modal fade show d-block" style="background: rgba(0,0,0,0.5); z-index: 1050;" tabindex="-1">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content border-0 shadow">
          <div class="modal-header border-bottom-0">
            <h5 class="modal-title fw-bold text-dark">ログアウト</h5>
            <button type="button" class="btn-close" @click="showLogoutModal = false"></button>
          </div>
          <div class="modal-body text-secondary">
            ログアウトしてもよろしいですか？
          </div>
          <div class="modal-footer border-top-0">
            <button type="button" class="btn btn-light px-4" @click="showLogoutModal = false">キャンセル</button>
            <button type="button" class="btn btn-danger px-4 fw-bold" @click="executeLogout">ログアウトする</button>
          </div>
        </div>
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
