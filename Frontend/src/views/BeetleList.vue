<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'

const beetles = ref([])
const router = useRouter()

const fetchBeetles = async () => {
  try {
    const response = await axios.get('/api/Beetles')
    beetles.value = response.data
  } catch (error) {
    console.error('Error fetching beetles:', error)
  }
}

onMounted(() => {
  fetchBeetles()
})

const getStageName = (stage: number) => {
  const stages = ['卵', '幼虫', '蛹', '成虫']
  return stages[stage] || '不明'
}

const getGenderName = (gender: number) => {
  const genders = ['不明', 'オス', 'メス']
  return genders[gender] || '不明'
}

const getGenderBadgeClass = (gender: number) => {
  if (gender === 1) return 'bg-primary bg-opacity-10 text-primary'
  if (gender === 2) return 'bg-danger bg-opacity-10 text-danger'
  return 'bg-secondary bg-opacity-10 text-secondary'
}
</script>

<template>
  <div>
    <h2 class="mb-4 text-secondary border-bottom pb-2">生体・幼虫管理</h2>
    
    <div class="card shadow-sm border-0">
      <div class="card-header bg-white border-bottom pt-3 pb-3 d-flex justify-content-between align-items-center">
        <h5 class="m-0 text-muted">登録データ一覧</h5>
        <router-link to="/beetle/new" class="btn btn-success fw-bold">＋ 新規登録</router-link>
      </div>
      <div class="card-body p-0">
        <div class="table-responsive">
          <table class="table table-hover align-middle m-0" style="cursor: pointer;">
            <thead class="table-light">
              <tr>
                <th class="ps-4">管理名</th>
                <th>種類</th>
                <th>ステージ</th>
                <th>性別</th>
                <th>サイズ</th>
                <th class="pe-4 text-end">詳細</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="beetle in beetles" :key="beetle.id" @click="router.push(`/beetle/${beetle.id}`)" class="clickable-row">
                <td class="ps-4 fw-bold">{{ beetle.name || '名称未設定' }}</td>
                <td>{{ beetle.species }}</td>
                <td><span class="badge bg-secondary">{{ getStageName(beetle.stage) }}</span></td>
                <td><span class="badge px-2 py-1" :class="getGenderBadgeClass(beetle.gender)">{{ getGenderName(beetle.gender) }}</span></td>
                <td>{{ beetle.size ? `${beetle.size} mm` : '-' }}</td>
                <td class="pe-4 text-end text-muted">＞</td>
              </tr>
              <tr v-if="beetles.length === 0">
                <td colspan="6" class="text-center text-muted py-5">登録されているデータがありません</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.clickable-row:hover {
  background-color: #f8f9fa;
}
</style>
