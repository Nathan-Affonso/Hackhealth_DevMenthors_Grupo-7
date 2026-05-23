import axios from 'axios'

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || '/api'
})

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

export type Entity = { id: string; [key: string]: unknown }
export type TrackingPoint = {
  id: string
  caixaTermicaId: string
  rotaColetaId?: string
  coletaId?: string
  distribuicaoId?: string
  usuarioId: string
  tipoRegistro: string
  latitude: number
  longitude: number
  temperatura: number
  dataHoraRegistro: string
  observacao?: string
  origemRegistro: string
}

export async function list<T = Entity>(resource: string): Promise<T[]> {
  const { data } = await api.get<T[]>(`/${resource}`)
  return data
}
