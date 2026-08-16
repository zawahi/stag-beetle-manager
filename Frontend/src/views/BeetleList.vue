<script setup lang="ts">
import { ref, onMounted } from 'vue'
import axios from 'axios'

const API_URL = '/api/Beetles'

const beetles = ref([])

const fetchBeetles = async () => {
  try {
    const response = await axios.get(API_URL)
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
</script>

<template>
  <div>
    <h2>生体・幼虫一覧</h2>
    <div class="card">
      <div class="action-bar" style="margin-bottom: 15px;">
        <router-link to="/beetle/new" class="btn">新規登録</router-link>
      </div>
      <table>
        <thead>
          <tr>
            <th>管理名</th>
            <th>種類</th>
            <th>産地</th>
            <th>累代</th>
            <th>ステージ</th>
            <th>性別</th>
            <th>サイズ (mm)</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="beetle in beetles" :key="beetle.id" @click="$router.push(`/beetle/${beetle.id}`)" class="clickable-row">
            <td>{{ beetle.name || '名称未設定' }}</td>
            <td>{{ beetle.species }}</td>
            <td>{{ beetle.locality }}</td>
            <td>{{ beetle.generation }}</td>
            <td>{{ getStageName(beetle.stage) }}</td>
            <td>{{ getGenderName(beetle.gender) }}</td>
            <td>{{ beetle.size ? `${beetle.size} mm` : '-' }}</td>
          </tr>
          <tr v-if="beetles.length === 0">
            <td colspan="7" style="text-align: center; color: #7f8c8d;">データがありません</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<style scoped>
.clickable-row {
  cursor: pointer;
  transition: background-color 0.2s;
}
.clickable-row:hover {
  background-color: #f1f8e9;
}
</style>
