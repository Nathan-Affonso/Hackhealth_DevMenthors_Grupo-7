import React, { useMemo, useState } from 'react'
import ReactDOM from 'react-dom/client'
import { QueryClient, QueryClientProvider, useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { BrowserRouter, Link, Navigate, Route, Routes, useLocation, useNavigate } from 'react-router-dom'
import { Loader } from '@googlemaps/js-api-loader'
import { Activity, AlertTriangle, Baby, Boxes, ClipboardList, Database, FlaskConical, HeartHandshake, Home, LogOut, MapPinned, Milk, PackageCheck, Plus, Search, Settings, ShieldCheck, Truck, Users } from 'lucide-react'
import { Bar, BarChart, CartesianGrid, Cell, Line, LineChart, Pie, PieChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts'
import toast, { Toaster } from 'react-hot-toast'
import { api, Entity, list, TrackingPoint } from './api'
import './index.css'

type MenuItem = { path: string; label: string; icon: React.ElementType; resource?: string }

const menu: MenuItem[] = [
  { path: '/', label: 'Dashboard geral', icon: Home },
  { path: '/instituicoes', label: 'Instituicoes', icon: Home, resource: 'instituicoes' },
  { path: '/usuarios', label: 'Usuarios', icon: Users, resource: 'usuarios' },
  { path: '/doadoras', label: 'Doadoras', icon: HeartHandshake, resource: 'doadoras' },
  { path: '/doadoras/detalhe', label: 'Detalhe da doadora', icon: HeartHandshake, resource: 'doadoras' },
  { path: '/triagem', label: 'Triagem da doadora', icon: ShieldCheck, resource: 'doadoras' },
  { path: '/exames', label: 'Exames', icon: ClipboardList, resource: 'doadoras' },
  { path: '/caixas-termicas', label: 'Caixas termicas', icon: Boxes, resource: 'caixas-termicas' },
  { path: '/rotas', label: 'Rotas de coleta', icon: Truck, resource: 'rotas' },
  { path: '/planejamento-rota', label: 'Planejamento da rota', icon: MapPinned, resource: 'rotas' },
  { path: '/coletas', label: 'Coletas', icon: ClipboardList, resource: 'coletas' },
  { path: '/frascos', label: 'Registro/consulta de frascos', icon: Milk, resource: 'frascos' },
  { path: '/recepcao-blh', label: 'Recepcao no BLH', icon: PackageCheck, resource: 'recepcao-blh' },
  { path: '/lotes', label: 'Lotes de processamento', icon: FlaskConical, resource: 'lotes' },
  { path: '/reenvase', label: 'Reenvase', icon: FlaskConical, resource: 'lotes' },
  { path: '/pasteurizacao', label: 'Pasteurizacao', icon: FlaskConical, resource: 'lotes' },
  { path: '/qualidade', label: 'Controle de qualidade', icon: ShieldCheck, resource: 'controle-qualidade/frasco/88888888-8888-8888-8888-888888888888' },
  { path: '/estoque-cru', label: 'Estoque cru', icon: Database, resource: 'estoque' },
  { path: '/estoque-pasteurizado', label: 'Estoque pasteurizado', icon: Database, resource: 'estoque' },
  { path: '/hospitais', label: 'Hospitais', icon: Home, resource: 'instituicoes' },
  { path: '/receptores', label: 'Receptores/bebes', icon: Baby, resource: 'receptores' },
  { path: '/prescricoes', label: 'Prescricoes', icon: ClipboardList, resource: 'prescricoes' },
  { path: '/matching', label: 'Matching frasco -> bebe', icon: Activity, resource: 'frascos' },
  { path: '/distribuicoes', label: 'Distribuicoes', icon: Truck, resource: 'dashboard/estoque' },
  { path: '/recebimento-hospitalar', label: 'Recebimento hospitalar', icon: PackageCheck, resource: 'dashboard/estoque' },
  { path: '/administracao', label: 'Administracao ao bebe', icon: Baby, resource: 'dashboard/estoque' },
  { path: '/rastreabilidade-frasco', label: 'Rastreabilidade por frasco', icon: Search },
  { path: '/rastreabilidade-bebe', label: 'Rastreabilidade por bebe', icon: Search },
  { path: '/rastreabilidade-doadora', label: 'Rastreabilidade por doadora', icon: Search },
  { path: '/rastreamento-caixa', label: 'Rastreamento de caixa termica', icon: MapPinned },
  { path: '/alertas-temperatura', label: 'Alertas de temperatura', icon: AlertTriangle, resource: 'dashboard/alertas' },
  { path: '/relatorios', label: 'Relatorios', icon: BarChartIcon, resource: 'dashboard/resumo' },
  { path: '/auditoria', label: 'Auditoria', icon: Database, resource: 'dashboard/resumo' },
  { path: '/configuracoes', label: 'Configuracoes', icon: Settings, resource: 'dashboard/resumo' }
]

function BarChartIcon(props: React.ComponentProps<typeof Activity>) {
  return <Activity {...props} />
}

function App() {
  return (
    <QueryClientProvider client={new QueryClient()}>
      <BrowserRouter>
        <Toaster position="top-right" />
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/*" element={<Shell />} />
        </Routes>
      </BrowserRouter>
    </QueryClientProvider>
  )
}

function Shell() {
  const location = useLocation()
  const nav = useNavigate()
  if (!localStorage.getItem('token')) return <Navigate to="/login" replace />
  const current = menu.find((x) => x.path === location.pathname)
  return (
    <div className="flex h-screen overflow-hidden">
      <aside className="w-72 shrink-0 overflow-y-auto border-r border-slate-200 bg-white">
        <div className="sticky top-0 z-10 border-b border-slate-200 bg-white px-5 py-4">
          <div className="text-lg font-semibold text-blh-ink">BLH Rastreabilidade</div>
          <div className="text-xs text-slate-500">Banco de Leite Humano</div>
        </div>
        <nav className="space-y-1 p-3">
          {menu.map((item) => {
            const Icon = item.icon
            const active = location.pathname === item.path
            return (
              <Link key={item.path} to={item.path} className={`flex items-center gap-3 rounded-md px-3 py-2 text-sm ${active ? 'bg-teal-50 text-teal-800' : 'text-slate-700 hover:bg-slate-100'}`}>
                <Icon className="h-4 w-4" />
                <span className="truncate">{item.label}</span>
              </Link>
            )
          })}
        </nav>
      </aside>
      <main className="flex min-w-0 flex-1 flex-col">
        <header className="flex h-16 items-center justify-between border-b border-slate-200 bg-white px-6">
          <div>
            <h1 className="text-xl font-semibold">{current?.label || 'Dashboard geral'}</h1>
            <p className="text-xs text-slate-500">Rastreabilidade ponta a ponta do frasco ao receptor</p>
          </div>
          <button className="inline-flex items-center gap-2 rounded-md border border-slate-300 px-3 py-2 text-sm" onClick={() => { localStorage.clear(); nav('/login') }}>
            <LogOut className="h-4 w-4" /> Sair
          </button>
        </header>
        <section className="min-h-0 flex-1 overflow-y-auto p-6">
          <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/rastreamento-caixa" element={<TrackingPage />} />
            <Route path="/rastreabilidade-frasco" element={<TracePage type="frasco" />} />
            <Route path="/rastreabilidade-bebe" element={<TracePage type="receptor" />} />
            <Route path="/rastreabilidade-doadora" element={<TracePage type="doadora" />} />
            {menu.filter((m) => m.path !== '/' && m.path !== '/rastreamento-caixa').map((item) => (
              <Route key={item.path} path={item.path.replace('/', '')} element={<GenericPage title={item.label} resource={item.resource} />} />
            ))}
          </Routes>
        </section>
      </main>
    </div>
  )
}

function Login() {
  const nav = useNavigate()
  const [email, setEmail] = useState('admin@blh.local')
  const [senha, setSenha] = useState('Admin@123')
  const login = useMutation({
    mutationFn: async () => (await api.post('/auth/login', { email, senha })).data,
    onSuccess: (data) => { localStorage.setItem('token', data.token); toast.success('Login realizado'); nav('/') },
    onError: () => toast.error('Credenciais invalidas')
  })
  return (
    <div className="grid min-h-screen place-items-center bg-slate-100 px-4">
      <form className="w-full max-w-sm rounded-lg border border-slate-200 bg-white p-6 shadow-sm" onSubmit={(e) => { e.preventDefault(); login.mutate() }}>
        <h1 className="text-2xl font-semibold">BLH Rastreabilidade</h1>
        <p className="mt-1 text-sm text-slate-500">Acesse o sistema web do Banco de Leite Humano.</p>
        <label className="mt-6 block text-sm font-medium">Email</label>
        <input className="mt-1 w-full rounded-md border border-slate-300 px-3 py-2" value={email} onChange={(e) => setEmail(e.target.value)} />
        <label className="mt-4 block text-sm font-medium">Senha</label>
        <input className="mt-1 w-full rounded-md border border-slate-300 px-3 py-2" type="password" value={senha} onChange={(e) => setSenha(e.target.value)} />
        <button className="mt-6 w-full rounded-md bg-teal-700 px-4 py-2 font-medium text-white disabled:opacity-60" disabled={login.isPending}>Entrar</button>
        <p className="mt-4 text-xs text-slate-500">Admin padrao: admin@blh.local / Admin@123</p>
      </form>
    </div>
  )
}

function Dashboard() {
  const resumo = useQuery({ queryKey: ['resumo'], queryFn: async () => (await api.get('/dashboard/resumo')).data })
  const coletas = useQuery({ queryKey: ['coletas-chart'], queryFn: async () => (await api.get('/dashboard/coletas')).data })
  const estoque = useQuery({ queryKey: ['estoque-chart'], queryFn: async () => (await api.get('/dashboard/estoque')).data })
  const temps = useQuery({ queryKey: ['temps-chart'], queryFn: async () => (await api.get('/dashboard/temperaturas')).data })
  const cards = [
    ['Doadoras ativas', resumo.data?.doadorasAtivas], ['Coletas hoje', resumo.data?.coletasHoje], ['Frascos coletados', resumo.data?.frascosColetados],
    ['Frascos em estoque', resumo.data?.frascosEmEstoque], ['Frascos descartados', resumo.data?.frascosDescartados], ['Litros coletados', resumo.data?.litrosColetados],
    ['Litros distribuidos', resumo.data?.litrosDistribuidos], ['Bebes atendidos', resumo.data?.bebesAtendidos], ['Alertas temperatura', resumo.data?.alertasTemperatura], ['Prescricoes abertas', resumo.data?.prescricoesAbertas]
  ]
  return (
    <div className="space-y-6">
      <div className="grid grid-cols-2 gap-4 lg:grid-cols-5">
        {cards.map(([label, value]) => <div key={label} className="rounded-lg border border-slate-200 bg-white p-4"><div className="text-xs text-slate-500">{label}</div><div className="mt-2 text-2xl font-semibold">{value ?? '-'}</div></div>)}
      </div>
      <div className="grid gap-6 lg:grid-cols-3">
        <ChartPanel title="Coletas por dia"><ResponsiveContainer width="100%" height={240}><BarChart data={coletas.data || []}><CartesianGrid strokeDasharray="3 3" /><XAxis dataKey="data" hide /><YAxis allowDecimals={false} /><Tooltip /><Bar dataKey="total" fill="#0f766e" /></BarChart></ResponsiveContainer></ChartPanel>
        <ChartPanel title="Frascos por status"><ResponsiveContainer width="100%" height={240}><PieChart><Pie data={estoque.data || []} dataKey="total" nameKey="status" outerRadius={80}>{(estoque.data || []).map((_: unknown, i: number) => <Cell key={i} fill={['#0f766e', '#0369a1', '#be123c', '#64748b'][i % 4]} />)}</Pie><Tooltip /></PieChart></ResponsiveContainer></ChartPanel>
        <ChartPanel title="Temperatura media por rota"><ResponsiveContainer width="100%" height={240}><LineChart data={temps.data || []}><CartesianGrid strokeDasharray="3 3" /><XAxis dataKey="dataHoraRegistro" hide /><YAxis /><Tooltip /><Line type="monotone" dataKey="temperatura" stroke="#be123c" strokeWidth={2} /></LineChart></ResponsiveContainer></ChartPanel>
      </div>
    </div>
  )
}

function ChartPanel({ title, children }: { title: string; children: React.ReactNode }) {
  return <div className="rounded-lg border border-slate-200 bg-white p-4"><h2 className="mb-4 font-semibold">{title}</h2>{children}</div>
}

function GenericPage({ title, resource }: { title: string; resource?: string }) {
  const [q, setQ] = useState('')
  const query = useQuery({ queryKey: [resource], queryFn: async () => resource ? (await api.get(`/${resource}`)).data : [], enabled: !!resource })
  const rows: Entity[] = Array.isArray(query.data) ? query.data : query.data ? [query.data] : []
  const filtered = rows.filter((r) => JSON.stringify(r).toLowerCase().includes(q.toLowerCase()))
  return (
    <div className="space-y-4">
      <div className="flex flex-wrap items-center justify-between gap-3">
        <div className="relative min-w-72">
          <Search className="absolute left-3 top-2.5 h-4 w-4 text-slate-400" />
          <input className="w-full rounded-md border border-slate-300 py-2 pl-9 pr-3 text-sm" placeholder="Buscar na tabela" value={q} onChange={(e) => setQ(e.target.value)} />
        </div>
        <button className="inline-flex items-center gap-2 rounded-md bg-teal-700 px-3 py-2 text-sm text-white"><Plus className="h-4 w-4" /> Novo registro</button>
      </div>
      <DataTable rows={filtered} title={title} />
    </div>
  )
}

function DataTable({ rows, title }: { rows: Entity[]; title: string }) {
  const columns = useMemo(() => Array.from(new Set(rows.flatMap((r) => Object.keys(r)))).slice(0, 8), [rows])
  return (
    <div className="overflow-hidden rounded-lg border border-slate-200 bg-white">
      <div className="border-b border-slate-200 px-4 py-3 text-sm font-medium">{title} ({rows.length})</div>
      <div className="overflow-x-auto">
        <table className="min-w-full divide-y divide-slate-200 text-sm">
          <thead className="bg-slate-50"><tr>{columns.map((c) => <th key={c} className="px-4 py-3 text-left font-medium text-slate-600">{c}</th>)}</tr></thead>
          <tbody className="divide-y divide-slate-100">
            {rows.map((r) => <tr key={String(r.id)}>{columns.map((c) => <td key={c} className="max-w-xs truncate px-4 py-3">{String(r[c] ?? '')}</td>)}</tr>)}
            {!rows.length && <tr><td className="px-4 py-8 text-center text-slate-500" colSpan={Math.max(columns.length, 1)}>Nenhum registro encontrado.</td></tr>}
          </tbody>
        </table>
      </div>
    </div>
  )
}

function TrackingPage() {
  const [rotaId, setRotaId] = useState('55555555-5555-5555-5555-555555555555')
  const caixas = useQuery({ queryKey: ['caixas'], queryFn: () => list<Entity>('caixas-termicas') })
  const rotas = useQuery({ queryKey: ['rotas'], queryFn: () => list<Entity>('rotas') })
  const pontos = useQuery({ queryKey: ['rastreamento', rotaId], queryFn: async () => (await api.get<TrackingPoint[]>(`/rastreamento-caixa/rota/${rotaId}`)).data, enabled: !!rotaId })
  const exportCsv = () => {
    const rows = pontos.data || []
    const csv = ['dataHoraRegistro,tipoRegistro,temperatura,latitude,longitude,observacao', ...rows.map((p) => [p.dataHoraRegistro, p.tipoRegistro, p.temperatura, p.latitude, p.longitude, p.observacao || ''].join(','))].join('\n')
    const url = URL.createObjectURL(new Blob([csv], { type: 'text/csv' }))
    const a = document.createElement('a'); a.href = url; a.download = 'rastreamento-caixa.csv'; a.click(); URL.revokeObjectURL(url)
  }
  const alertas = (pontos.data || []).filter((p) => p.temperatura < 2 || p.temperatura > 8)
  return (
    <div className="grid min-h-[calc(100vh-7rem)] gap-6 xl:grid-cols-[minmax(0,1fr)_420px]">
      <div className="space-y-4">
        <div className="flex flex-wrap gap-3 rounded-lg border border-slate-200 bg-white p-4">
          <select className="rounded-md border border-slate-300 px-3 py-2 text-sm" aria-label="Caixa termica">
            {caixas.data?.map((c) => <option key={c.id} value={c.id}>{String(c.codigo || c.id)}</option>)}
          </select>
          <select className="rounded-md border border-slate-300 px-3 py-2 text-sm" value={rotaId} onChange={(e) => setRotaId(e.target.value)} aria-label="Rota">
            {rotas.data?.map((r) => <option key={r.id} value={r.id}>{String(r.nome || r.id)}</option>)}
          </select>
          <button className="rounded-md border border-slate-300 px-3 py-2 text-sm" onClick={exportCsv}>Exportar CSV</button>
          {alertas.length > 0 && <div className="inline-flex items-center gap-2 rounded-md bg-rose-50 px-3 py-2 text-sm text-rose-800"><AlertTriangle className="h-4 w-4" /> {alertas.length} alerta(s)</div>}
        </div>
        <GoogleMap points={pontos.data || []} />
        <DataTable rows={(pontos.data || []) as unknown as Entity[]} title="Registros de rastreamento" />
      </div>
      <aside className="space-y-4">
        <ChartPanel title="Temperatura ao longo do tempo"><ResponsiveContainer width="100%" height={240}><LineChart data={pontos.data || []}><CartesianGrid strokeDasharray="3 3" /><XAxis dataKey="dataHoraRegistro" hide /><YAxis domain={[0, 12]} /><Tooltip /><Line type="monotone" dataKey="temperatura" stroke="#be123c" strokeWidth={2} /></LineChart></ResponsiveContainer></ChartPanel>
        <div className="rounded-lg border border-slate-200 bg-white p-4">
          <h2 className="font-semibold">Timeline</h2>
          <ol className="mt-4 space-y-3">
            {(pontos.data || []).map((p) => <li key={p.id} className="border-l-2 border-teal-700 pl-3"><div className="text-sm font-medium">{p.tipoRegistro}</div><div className="text-xs text-slate-500">{new Date(p.dataHoraRegistro).toLocaleString()} - {p.temperatura} C</div><div className="text-xs text-slate-600">{p.observacao}</div></li>)}
          </ol>
        </div>
      </aside>
    </div>
  )
}

function GoogleMap({ points }: { points: TrackingPoint[] }) {
  const ref = React.useRef<HTMLDivElement>(null)
  React.useEffect(() => {
    const key = import.meta.env.VITE_GOOGLE_MAPS_API_KEY
    if (!ref.current || !points.length || !key) return
    const loader = new Loader({ apiKey: key, version: 'weekly' })
    loader.load().then((google) => {
      const center = { lat: Number(points[0].latitude), lng: Number(points[0].longitude) }
      const map = new google.maps.Map(ref.current!, { center, zoom: 14, mapTypeControl: false, streetViewControl: false })
      const path = points.map((p) => ({ lat: Number(p.latitude), lng: Number(p.longitude) }))
      new google.maps.Polyline({ path, strokeColor: '#0f766e', strokeWeight: 4, map })
      const bounds = new google.maps.LatLngBounds()
      points.forEach((p) => {
        const pos = { lat: Number(p.latitude), lng: Number(p.longitude) }
        bounds.extend(pos)
        const marker = new google.maps.Marker({ position: pos, map, title: p.tipoRegistro })
        const info = new google.maps.InfoWindow({ content: `<div style="font-size:13px"><strong>${p.tipoRegistro}</strong><br/>Temp: ${p.temperatura} C<br/>Lat/Lng: ${p.latitude}, ${p.longitude}<br/>Data: ${new Date(p.dataHoraRegistro).toLocaleString()}<br/>Caixa: ${p.caixaTermicaId}<br/>Rota: ${p.rotaColetaId || ''}<br/>Responsavel: ${p.usuarioId}<br/>${p.observacao || ''}</div>` })
        marker.addListener('click', () => info.open({ map, anchor: marker }))
      })
      map.fitBounds(bounds)
    })
  }, [points])
  if (!import.meta.env.VITE_GOOGLE_MAPS_API_KEY) {
    return <div className="grid h-[560px] place-items-center rounded-lg border border-dashed border-slate-300 bg-white text-center text-sm text-slate-600">Configure VITE_GOOGLE_MAPS_API_KEY para carregar o Google Maps.<br />Os dados de rota, tabela, timeline e grafico ja estao carregados pela API.</div>
  }
  return <div ref={ref} className="h-[560px] rounded-lg border border-slate-200 bg-white" />
}

function TracePage({ type }: { type: 'frasco' | 'receptor' | 'doadora' }) {
  const defaults = { frasco: 'BLH-2026-0001', receptor: '99999999-9999-9999-9999-999999999999', doadora: '44444444-4444-4444-4444-444444444444' }
  const [term, setTerm] = useState(defaults[type])
  const [result, setResult] = useState<unknown>()
  const run = async () => {
    const endpoint = type === 'frasco' ? `/rastreabilidade/frasco/${term}` : `/rastreabilidade/${type}/${term}`
    setResult((await api.get(endpoint)).data)
  }
  return (
    <div className="space-y-4">
      <div className="flex gap-3 rounded-lg border border-slate-200 bg-white p-4">
        <input className="min-w-96 rounded-md border border-slate-300 px-3 py-2 text-sm" value={term} onChange={(e) => setTerm(e.target.value)} />
        <button className="rounded-md bg-teal-700 px-3 py-2 text-sm text-white" onClick={run}>Consultar</button>
      </div>
      <pre className="max-h-[70vh] overflow-auto rounded-lg border border-slate-200 bg-white p-4 text-xs">{JSON.stringify(result, null, 2)}</pre>
    </div>
  )
}

ReactDOM.createRoot(document.getElementById('root')!).render(<App />)
