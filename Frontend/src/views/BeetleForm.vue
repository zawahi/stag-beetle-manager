<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'

const router = useRouter()

const males = ref([])
const females = ref([])

const form = ref({
  name: '',
  species: 'オオクワガタ',
  locality: '',
  generation: '',
  stage: 1, 
  gender: 0, 
  size: null,
  hatchDate: null,
  emergenceDate: null,
  sireId: null,
  damId: null,
  notes: ''
})

const isSubmitting = ref(false)

const fetchAdults = async () => {
  try {
    const response = await axios.get('/api/Beetles')
    males.value = response.data.filter(b => b.stage === 3 && b.gender === 1)
    females.value = response.data.filter(b => b.stage === 3 && b.gender === 2)
  } catch (error) {
    console.error('Error fetching beetles:', error)
  }
}

onMounted(() => {
  fetchAdults()
})

const submitForm = async () => {
  isSubmitting.value = true
  try {
    await axios.post('/api/Beetles', form.value)
    router.push('/beetles')
  } catch (error) {
    console.error('Registration failed:', error)
    alert('登録に失敗しました。')
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <div>
    <h2>生体・幼虫の新規登録</h2>
    <div class="card">
      <form @submit.prevent="submitForm">
        <div class="form-group">
          <label>管理名</label>
          <input v-model="form.name" type="text" placeholder="例: 能勢YG-01" />
        </div>
        
        <div class="form-group">
          <label>種類 <span class="required">*</span></label>
          <input v-model="form.species" type="text" required placeholder="例: オオクワガタ" />
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>産地</label>
            <input v-model="form.locality" type="text" placeholder="例: 能勢" />
          </div>
          <div class="form-group">
            <label>累代</label>
            <input v-model="form.generation" type="text" placeholder="例: CBF1" />
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>ステージ <span class="required">*</span></label>
            <select v-model="form.stage" required>
              <option :value="0">卵</option>
              <option :value="1">幼虫</option>
              <option :value="2">蛹</option>
              <option :value="3">成虫</option>
            </select>
          </div>
          <div class="form-group">
            <label>性別 <span class="required">*</span></label>
            <select v-model="form.gender" required>
              <option :value="0">不明</option>
              <option :value="1">オス</option>
              <option :value="2">メス</option>
            </select>
          </div>
        </div>

        <div class="form-row">
          <div class="form-group" v-if="form.stage === 3">
            <label>サイズ (mm)</label>
            <input v-model="form.size" type="number" step="0.1" />
          </div>
          <div class="form-group" v-if="form.stage >= 1">
            <label>孵化日</label>
            <input v-model="form.hatchDate" type="date" />
          </div>
          <div class="form-group" v-if="form.stage === 3">
            <label>羽化日</label>
            <input v-model="form.emergenceDate" type="date" />
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>オス親 (種親)</label>
            <select v-model="form.sireId">
              <option :value="null">不明・登録なし</option>
              <option v-for="m in males" :key="m.id" :value="m.id">
                {{ m.name || m.species }} ({{ m.size ? m.size + 'mm' : 'サイズ不明' }})
              </option>
            </select>
          </div>
          <div class="form-group">
            <label>メス親 (種親)</label>
            <select v-model="form.damId">
              <option :value="null">不明・登録なし</option>
              <option v-for="f in females" :key="f.id" :value="f.id">
                {{ f.name || f.species }} ({{ f.size ? f.size + 'mm' : 'サイズ不明' }})
              </option>
            </select>
          </div>
        </div>

        <div class="form-group">
          <label>メモ</label>
          <textarea v-model="form.notes" rows="3" placeholder="種親の情報や特記事項など"></textarea>
        </div>

        <div class="form-actions">
          <button type="button" class="btn btn-secondary" @click="$router.push('/beetles')">キャンセル</button>
          <button type="submit" class="btn" :disabled="isSubmitting">登録する</button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.form-group {
  margin-bottom: 20px;
}
.form-row {
  display: flex;
  gap: 20px;
}
.form-row .form-group {
  flex: 1;
}
label {
  display: block;
  margin-bottom: 8px;
  font-weight: 600;
  color: #2c3e50;
}
.required {
  color: #e74c3c;
}
input, select, textarea {
  width: 100%;
  padding: 10px 12px;
  border: 1px solid #ccc;
  border-radius: 6px;
  box-sizing: border-box;
  font-size: 1rem;
}
input:focus, select:focus, textarea:focus {
  outline: none;
  border-color: #4CAF50;
  box-shadow: 0 0 0 2px rgba(76, 175, 80, 0.2);
}
.form-actions {
  margin-top: 30px;
  display: flex;
  gap: 15px;
}
.btn-secondary {
  background-color: #95a5a6;
}
.btn-secondary:hover {
  background-color: #7f8c8d;
}
</style>
