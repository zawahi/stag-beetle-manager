<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import axios from 'axios'

const route = useRoute()
const router = useRouter()
const beetleId = route.params.id

const beetle = ref(null)
const bottleRecords = ref([])
const showBottleForm = ref(false)

const newBottle = ref({
  beetleId: beetleId,
  exchangeDate: new Date().toISOString().split('T')[0],
  mediaType: 0, // 0: 菌糸, 1: マット, 2: 材
  mediaName: '',
  weight: null,
  headWidth: null,
  notes: ''
})

const fetchBeetle = async () => {
  try {
    const response = await axios.get(`/api/Beetles/${beetleId}`)
    beetle.value = response.data
    fetchBottleRecords()
  } catch (error) {
    console.error('Error fetching beetle:', error)
  }
}

const fetchBottleRecords = async () => {
  try {
    const response = await axios.get(`/api/BottleRecords/beetle/${beetleId}`)
    bottleRecords.value = response.data
  } catch (error) {
    console.error('Error fetching bottle records:', error)
  }
}

const addBottleRecord = async () => {
  try {
    await axios.post('/api/BottleRecords', newBottle.value)
    showBottleForm.value = false
    newBottle.value.mediaName = ''
    newBottle.value.weight = null
    newBottle.value.headWidth = null
    newBottle.value.notes = ''
    fetchBottleRecords()
  } catch (error) {
    console.error('Error adding bottle record:', error)
    alert('登録に失敗しました。')
  }
}

onMounted(() => {
  fetchBeetle()
})

const getStageName = (stage: number) => {
  const stages = ['卵', '幼虫', '蛹', '成虫']
  return stages[stage] || '不明'
}

const getGenderName = (gender: number) => {
  const genders = ['不明', 'オス', 'メス']
  return genders[gender] || '不明'
}

const getMediaTypeName = (type: number) => {
  const types = ['菌糸', 'マット', '材']
  return types[type] || '不明'
}
</script>

<template>
  <div>
    <div v-if="beetle">
      <div style="display: flex; justify-content: space-between; align-items: center;">
        <h2>{{ beetle.name || '名称未設定' }} の詳細</h2>
        <button class="btn btn-secondary" @click="router.push('/')">一覧へ戻る</button>
      </div>
      
      <div class="card">
        <h3>基本情報</h3>
        <table class="detail-table">
          <tbody>
            <tr><th>種類</th><td>{{ beetle.species }}</td><th>産地</th><td>{{ beetle.locality || '-' }}</td></tr>
            <tr><th>累代</th><td>{{ beetle.generation || '-' }}</td><th>ステージ</th><td>{{ getStageName(beetle.stage) }}</td></tr>
            <tr><th>性別</th><td>{{ getGenderName(beetle.gender) }}</td><th>サイズ</th><td>{{ beetle.size ? `${beetle.size} mm` : '-' }}</td></tr>
            <tr>
              <th>オス親 (Sire)</th>
              <td>
                <router-link v-if="beetle.sireId" :to="`/beetle/${beetle.sireId}`">{{ beetle.sire?.name || beetle.sire?.species || '不明' }}</router-link>
                <span v-else>不明</span>
              </td>
              <th>メス親 (Dam)</th>
              <td>
                <router-link v-if="beetle.damId" :to="`/beetle/${beetle.damId}`">{{ beetle.dam?.name || beetle.dam?.species || '不明' }}</router-link>
                <span v-else>不明</span>
              </td>
            </tr>
            <tr><th>孵化日</th><td>{{ beetle.hatchDate ? beetle.hatchDate.split('T')[0] : '-' }}</td><th>羽化日</th><td>{{ beetle.emergenceDate ? beetle.emergenceDate.split('T')[0] : '-' }}</td></tr>
            <tr><th>メモ</th><td colspan="3">{{ beetle.notes || '-' }}</td></tr>
          </tbody>
        </table>
      </div>

      <div class="card">
        <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px;">
          <h3>ボトル交換・計測記録</h3>
          <button class="btn" @click="showBottleForm = !showBottleForm">記録を追加</button>
        </div>

        <div v-if="showBottleForm" class="bottle-form">
          <form @submit.prevent="addBottleRecord">
            <div class="form-row">
              <div class="form-group">
                <label>交換日</label>
                <input v-model="newBottle.exchangeDate" type="date" required />
              </div>
              <div class="form-group">
                <label>種類</label>
                <select v-model="newBottle.mediaType">
                  <option :value="0">菌糸</option>
                  <option :value="1">マット</option>
                  <option :value="2">材</option>
                </select>
              </div>
              <div class="form-group">
                <label>銘柄</label>
                <input v-model="newBottle.mediaName" type="text" placeholder="例: レビンG" />
              </div>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label>体重 (g)</label>
                <input v-model="newBottle.weight" type="number" step="0.1" />
              </div>
              <div class="form-group">
                <label>頭幅 (mm)</label>
                <input v-model="newBottle.headWidth" type="number" step="0.1" />
              </div>
            </div>
            <div class="form-group">
              <label>メモ</label>
              <input v-model="newBottle.notes" type="text" />
            </div>
            <div style="text-align: right;">
              <button type="submit" class="btn">保存</button>
            </div>
          </form>
        </div>

        <table>
          <thead>
            <tr>
              <th>日付</th>
              <th>種類</th>
              <th>銘柄</th>
              <th>体重</th>
              <th>頭幅</th>
              <th>メモ</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="record in bottleRecords" :key="record.id">
              <td>{{ record.exchangeDate.split('T')[0] }}</td>
              <td>{{ getMediaTypeName(record.mediaType) }}</td>
              <td>{{ record.mediaName || '-' }}</td>
              <td>{{ record.weight ? `${record.weight} g` : '-' }}</td>
              <td>{{ record.headWidth ? `${record.headWidth} mm` : '-' }}</td>
              <td>{{ record.notes || '-' }}</td>
            </tr>
            <tr v-if="bottleRecords.length === 0">
              <td colspan="6" style="text-align: center; color: #7f8c8d;">記録がありません</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div v-else>
      <p>読み込み中...</p>
    </div>
  </div>
</template>

<style scoped>
.detail-table {
  width: 100%;
  border-collapse: collapse;
}
.detail-table th {
  background-color: #f8f9fa;
  width: 15%;
  border-bottom: 1px solid #ddd;
}
.detail-table td {
  width: 35%;
  border-bottom: 1px solid #ddd;
}
.bottle-form {
  background-color: #f8f9fa;
  padding: 15px;
  border-radius: 8px;
  margin-bottom: 20px;
  border: 1px solid #ddd;
}
.form-group {
  margin-bottom: 10px;
}
.form-row {
  display: flex;
  gap: 15px;
}
.form-row .form-group {
  flex: 1;
}
label {
  display: block;
  margin-bottom: 5px;
  font-weight: 600;
  font-size: 0.9rem;
}
input, select {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
}
.btn-secondary {
  background-color: #95a5a6;
}
.btn-secondary:hover {
  background-color: #7f8c8d;
}
</style>
