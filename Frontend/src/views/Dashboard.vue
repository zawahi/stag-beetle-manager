<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'

const stats = ref(null)
const router = useRouter()

const fetchStats = async () => {
  try {
    const response = await axios.get('/api/Dashboard/stats')
    stats.value = response.data
  } catch (error) {
    console.error('Error fetching dashboard stats:', error)
  }
}

onMounted(() => {
  fetchStats()
})
</script>

<template>
  <div>
    <h2>ダッシュボード</h2>
    
    <div v-if="stats" class="dashboard-grid">
      <div class="stat-card" @click="router.push('/beetles')">
        <h3>総飼育数</h3>
        <div class="stat-number">{{ stats.totalBeetles }}</div>
      </div>
      <div class="stat-card" @click="router.push('/beetles')">
        <h3>幼虫</h3>
        <div class="stat-number">{{ stats.larvaeCount }}</div>
      </div>
      <div class="stat-card" @click="router.push('/beetles')">
        <h3>成虫</h3>
        <div class="stat-number">{{ stats.adultCount }}</div>
      </div>
      <div class="stat-card" @click="router.push('/breeding')">
        <h3>進行中のペアリング</h3>
        <div class="stat-number">{{ stats.breedingActiveCount }}</div>
      </div>
    </div>

    <div v-if="stats && stats.bottleAlerts.length > 0" class="card alert-card">
      <h3 style="color: #e74c3c;">⚠️ ボトル交換アラート（3ヶ月経過）</h3>
      <table>
        <thead>
          <tr>
            <th>管理名</th>
            <th>前回交換日</th>
            <th>経過日数</th>
            <th>アクション</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="alert in stats.bottleAlerts" :key="alert.beetleId">
            <td>{{ alert.beetleName }}</td>
            <td>{{ alert.lastExchangeDate.split('T')[0] }}</td>
            <td style="color: #e74c3c; font-weight: bold;">{{ alert.daysSinceExchange }} 日</td>
            <td>
              <button class="btn btn-small" @click="router.push(`/beetle/${alert.beetleId}`)">詳細へ</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<style scoped>
.dashboard-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 20px;
  margin-bottom: 30px;
}
.stat-card {
  background: #fff;
  border-radius: 8px;
  padding: 20px;
  box-shadow: 0 4px 6px rgba(0,0,0,0.05);
  text-align: center;
  cursor: pointer;
  transition: transform 0.2s, box-shadow 0.2s;
  border-top: 4px solid #4CAF50;
}
.stat-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 6px 12px rgba(0,0,0,0.1);
}
.stat-card h3 {
  margin: 0 0 10px 0;
  font-size: 1.1rem;
  color: #7f8c8d;
}
.stat-number {
  font-size: 2.5rem;
  font-weight: bold;
  color: #2c3e50;
}
.alert-card {
  border-left: 4px solid #e74c3c;
}
.btn-small {
  padding: 5px 10px;
  font-size: 0.8rem;
}
</style>
