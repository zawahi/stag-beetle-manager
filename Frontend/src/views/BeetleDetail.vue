<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import axios from 'axios'

const route = useRoute()
const router = useRouter()

const beetle = ref(null)
const records = ref([])
const newRecord = ref({
  exchangeDate: new Date().toISOString().split('T')[0],
  bottleType: '菌糸ビン',
  capacity: 800,
  weight: null,
  headWidth: null,
  notes: ''
})

const fetchBeetleDetail = async () => {
  try {
    const res = await axios.get(`/api/Beetles/${route.params.id}`)
    beetle.value = res.data
  } catch (error) {
    console.error(error)
  }
}

const fetchBottleRecords = async () => {
  try {
    const res = await axios.get(`/api/BottleRecords/beetle/${route.params.id}`)
    records.value = res.data
  } catch (error) {
    console.error(error)
  }
}

const addBottleRecord = async () => {
  try {
    await axios.post('/api/BottleRecords', { ...newRecord.value, beetleId: route.params.id })
    newRecord.value = { exchangeDate: new Date().toISOString().split('T')[0], bottleType: '菌糸ビン', capacity: 800, weight: null, headWidth: null, notes: '' }
    fetchBottleRecords()
  } catch (error) {
    console.error(error)
    alert('追加に失敗しました。')
  }
}

const deleteBottleRecord = async (id: string) => {
  if (!confirm('この記録を削除してもよろしいですか？')) return
  try {
    await axios.delete(`/api/BottleRecords/${id}`)
    fetchBottleRecords()
  } catch (error) {
    console.error(error)
    alert('削除に失敗しました。')
  }
}

onMounted(() => {
  fetchBeetleDetail()
  fetchBottleRecords()
})

const getStageName = (stage: number) => ['卵', '幼虫', '蛹', '成虫'][stage] || '不明'
const getGenderName = (gender: number) => ['不明', 'オス', 'メス'][gender] || '不明'
</script>

<template>
  <div v-if="beetle">
    <div class="d-flex justify-content-between align-items-center mb-4 border-bottom pb-2">
      <h2 class="text-secondary m-0">個体詳細: <span class="text-dark fw-bold">{{ beetle.name || '名称未設定' }}</span></h2>
      <button class="btn btn-outline-secondary" @click="router.push('/beetles')">一覧へ戻る</button>
    </div>

    <!-- 生体情報カード -->
    <div class="card shadow-sm border-0 mb-4">
      <div class="card-header bg-white border-bottom pt-3 pb-3">
        <h5 class="m-0 text-muted fw-bold">基本情報</h5>
      </div>
      <div class="card-body p-0">
        <table class="table table-bordered m-0 align-middle">
          <tbody>
            <tr>
              <th class="bg-light" style="width: 15%;">種類</th><td style="width: 35%;">{{ beetle.species }}</td>
              <th class="bg-light" style="width: 15%;">産地</th><td style="width: 35%;">{{ beetle.locality || '-' }}</td>
            </tr>
            <tr>
              <th class="bg-light">累代</th><td>{{ beetle.generation || '-' }}</td>
              <th class="bg-light">ステージ</th><td><span class="badge bg-secondary">{{ getStageName(beetle.stage) }}</span></td>
            </tr>
            <tr>
              <th class="bg-light">性別</th><td>{{ getGenderName(beetle.gender) }}</td>
              <th class="bg-light">サイズ</th><td>{{ beetle.size ? `${beetle.size} mm` : '-' }}</td>
            </tr>
            <tr>
              <th class="bg-light">オス親 (Sire)</th>
              <td>
                <router-link v-if="beetle.sireId" :to="`/beetle/${beetle.sireId}`" class="text-decoration-none fw-bold text-primary">{{ beetle.sire?.name || beetle.sire?.species || '不明' }}</router-link>
                <span v-else class="text-muted">不明</span>
              </td>
              <th class="bg-light">メス親 (Dam)</th>
              <td>
                <router-link v-if="beetle.damId" :to="`/beetle/${beetle.damId}`" class="text-decoration-none fw-bold text-danger">{{ beetle.dam?.name || beetle.dam?.species || '不明' }}</router-link>
                <span v-else class="text-muted">不明</span>
              </td>
            </tr>
            <tr>
              <th class="bg-light">孵化日</th><td>{{ beetle.hatchDate ? beetle.hatchDate.split('T')[0] : '-' }}</td>
              <th class="bg-light">羽化日</th><td>{{ beetle.emergenceDate ? beetle.emergenceDate.split('T')[0] : '-' }}</td>
            </tr>
            <tr>
              <th class="bg-light">メモ</th><td colspan="3">{{ beetle.notes || '-' }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- 菌糸ビン交換記録 -->
    <h3 class="mb-3 text-secondary">ビン・マット交換記録</h3>
    
    <div class="card shadow-sm border-0 mb-4 bg-light">
      <div class="card-body p-3">
        <form @submit.prevent="addBottleRecord" class="row gx-2 gy-2 align-items-end">
          <div class="col-md-2">
            <label class="form-label small fw-bold">交換日</label>
            <input type="date" class="form-control form-control-sm" v-model="newRecord.exchangeDate" required />
          </div>
          <div class="col-md-2">
            <label class="form-label small fw-bold">種類</label>
            <input type="text" class="form-control form-control-sm" v-model="newRecord.bottleType" placeholder="菌糸, マット等" required />
          </div>
          <div class="col-md-1">
            <label class="form-label small fw-bold">容量(cc)</label>
            <input type="number" class="form-control form-control-sm" v-model="newRecord.capacity" required />
          </div>
          <div class="col-md-1">
            <label class="form-label small fw-bold">体重(g)</label>
            <input type="number" step="0.1" class="form-control form-control-sm" v-model="newRecord.weight" />
          </div>
          <div class="col-md-1">
            <label class="form-label small fw-bold">頭幅(mm)</label>
            <input type="number" step="0.1" class="form-control form-control-sm" v-model="newRecord.headWidth" />
          </div>
          <div class="col-md-4">
            <label class="form-label small fw-bold">メモ</label>
            <input type="text" class="form-control form-control-sm" v-model="newRecord.notes" placeholder="銘柄など" />
          </div>
          <div class="col-md-1 d-grid">
            <button type="submit" class="btn btn-primary btn-sm fw-bold">追加</button>
          </div>
        </form>
      </div>
    </div>

    <div class="card shadow-sm border-0">
      <div class="table-responsive">
        <table class="table table-hover align-middle m-0">
          <thead class="table-light">
            <tr>
              <th class="ps-3">交換日</th>
              <th>種類 / 容量</th>
              <th>体重</th>
              <th>頭幅</th>
              <th>メモ</th>
              <th class="pe-3 text-end">操作</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="record in records" :key="record.id">
              <td class="ps-3 fw-bold">{{ record.exchangeDate.split('T')[0] }}</td>
              <td><span class="badge bg-secondary">{{ record.bottleType }}</span> {{ record.capacity }}cc</td>
              <td class="fw-bold text-success">{{ record.weight ? `${record.weight} g` : '-' }}</td>
              <td>{{ record.headWidth ? `${record.headWidth} mm` : '-' }}</td>
              <td class="text-muted">{{ record.notes || '-' }}</td>
              <td class="pe-3 text-end">
                <button class="btn btn-sm btn-outline-danger" @click="deleteBottleRecord(record.id)">削除</button>
              </td>
            </tr>
            <tr v-if="records.length === 0">
              <td colspan="6" class="text-center text-muted py-4">交換記録がありません</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
  <div v-else class="text-center p-5">
    <div class="spinner-border text-success" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
</template>
