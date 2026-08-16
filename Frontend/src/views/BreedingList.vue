<script setup lang="ts">
import { ref, onMounted } from 'vue'
import axios from 'axios'

const breedingRecords = ref([])

const fetchRecords = async () => {
  try {
    const response = await axios.get('/api/BreedingRecords')
    breedingRecords.value = response.data
  } catch (error) {
    console.error('Error fetching breeding records:', error)
  }
}

onMounted(() => {
  fetchRecords()
})
</script>

<template>
  <div>
    <h2 class="mb-4 text-secondary border-bottom pb-2">ペアリング記録一覧</h2>
    <div class="card shadow-sm border-0">
      <div class="card-header bg-white border-bottom pt-3 pb-3">
        <router-link to="/breeding/new" class="btn btn-success fw-bold">＋ 新規ペアリング開始</router-link>
      </div>
      <div class="card-body p-0">
        <div class="table-responsive">
          <table class="table table-hover table-striped align-middle m-0">
            <thead class="table-light">
              <tr>
                <th class="ps-3">同居開始日</th>
                <th>オス親</th>
                <th>メス親</th>
                <th>セット日</th>
                <th>割り出し日</th>
                <th class="pe-3">採卵/幼虫数</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="record in breedingRecords" :key="record.id">
                <td class="ps-3">{{ record.startDate ? record.startDate.split('T')[0] : '-' }}</td>
                <td><span class="badge bg-primary bg-opacity-10 text-primary px-2 py-1">{{ record.maleName || '不明' }}</span></td>
                <td><span class="badge bg-danger bg-opacity-10 text-danger px-2 py-1">{{ record.femaleName || '不明' }}</span></td>
                <td>{{ record.setDate ? record.setDate.split('T')[0] : '未セット' }}</td>
                <td>{{ record.allocationDate ? record.allocationDate.split('T')[0] : '未割り出し' }}</td>
                <td class="pe-3">🥚 {{ record.eggCount || 0 }} / 🐛 {{ record.larvaCount || 0 }}</td>
              </tr>
              <tr v-if="breedingRecords.length === 0">
                <td colspan="6" class="text-center text-muted py-4">ペアリング記録がありません</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
